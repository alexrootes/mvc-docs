There are quite a few deprecated methods, and one or two breaking changes. I'll cover how to migrate your existing 1.0RTM solution to 1.1, removing the use of deprecated methods.

# Removal of SubclassStrategy

This is the only obsolete method that has been flagged to cause an error if used, and therefore the only change in this guide that is absolutely necessary.

SubclassStrategy has been removed in favour of the IsDiscriminated method. Both of these methods have always existed, but it's never been clear which one to use and when. Ends up, one of them is redundant as they both do the same thing. We've removed SubclassStrategy because it was the more complicated of the two.

Your 1.0RTM code should look something like this:

    Setup(cfg =>
    {
      cfg.SubclassStrategy = type =>
      {
        if (type == typeof(ClassA))
          return SubclassStrategy.Subclass;

        return SubclassStrategy.JoinedSubclass;
      };
    })

Upgrading to 1.1, you just need to replace SubclassStrategy with IsDiscriminated:

    Setup(cfg =>
    {
      cfg.IsDiscriminated = type => type == typeof(ClassA);
    });

Much cleaner.

You've probably already noticed that Setup itself is deprecated, as well as all the methods inside. Lets get onto that next.

# Automapping deprecations

One of the new changes in 1.1 is the introduction of a way to configure the automapper outside of the fluent interface. The goal of this change is to encourage people to move away from the giant-method-chain that the automapper has become, and more towards an injectable, replacable, compartmentalised configuration. This move is not complete yet, but 1.1 introduces the largest change to the interface in anticipation of future releases.

It's not a difficult change to move to the new design, but it does require a bit of work.

The change can be summarised as such: Everything that is in your Setup method now has been moved into a separate object.

It's not required that you make this change now, but we do encourage it. The Setup method will be removed in the future, so the sooner you can move the better.

What you need to do is implement the IAutomappingConfiguration interface, which contains several methods that instruct the automapper on how you expect it to work. Fear not though, you don't have to implement this interface directly if you don't want to, the DefaultAutomappingConfiguration class is pre-configured with all the default rules; if you derive from that class, you can just override whichever methods are necessary for your deviations from the default.

All the methods in the Setup block are available in IAutomappingConfiguration, though some of them have had a slight name change (you should be able to figure them out easily enough).

Given this 1.0RTM setup:

    AutoMap.AssemblyOf<Person>()
      .Where(x => x.Namespace.EndsWith("Domain")
      .Setup(cfg =>
      {
        cfg.FindIdentity = member => member.Name == member.DeclaringType.Name + "Id";
        cfg.IsComponentType = type => type == typeof(Address);
        cfg.IsDiscriminated = type => type == typeof(ClassA);
      });

You should end up with the following automapping configuration in 1.1:

    public class YourAppAutomappingConfiguration : DefaultAutomappingConfiguration
    {
      public override bool IsId(Member member)
      {
        return member.Name == member.DeclaringType.Name + "Id";
      }

      public override bool IsComponent(Type type)
      {
        return type == typeof(Address);
      }

      public override bool IsDiscriminated(Type type)
      {
        return type == typeof(ClassA);
      }
    }

Your <code>AutoMap</code> usage is now as follows:

    var cfg = new YourAppAutomappingConfiguration();

    AutoMap.AssemblyOf<Person>(cfg);

# Member changes

There has been a large refactoring internally to genericise the use of properties, and make them interchangeable with fields and methods. This has had some minimal impact on the user API. A few of the conventions now expose a Member instance instead of a PropertyInfo. This is a breaking change, but you should just need to replace the PropertyInfo type with Member. ForeignKeyConvention is a notable example of this.

Additionally, Reveal.Property has been deprecated in favour of Reveal.Member. The behaviour is the same in most circumstances, except Reveal.Member will also operate with fields.

# Hibernate-mapping methods in automapping overrides

When creating an IAutomappingOverride, it was possible for you to (attempt) to modify the hibernate-mapping attributes (default-access, etc...). These methods *never functioned correctly* and have therefore been removed.