The Acceptance Criteria is an API used for defining the **criteria** that a mapping must satisfy for a [[convention|Conventions]] to be applied to it.

The criteria is defined inside the <code>Accept</code> method of a convention that implements an acceptance interface. The API is based around the <code>IAcceptanceCriteria<TInspector></code> interface, which exposes various methods that record the criteria against the <code>TInspector</code> instance.

> Inspectors are a view onto your mapped entities and their parts, they represent a read-only version and are just for ''inspecting'' the existing structure and values.

A typical example of a convention that's defined with a acceptance criteria would look like this:

    public class LegacyEntityTableConvention : IClassConvention, IClassConventionAcceptance
    {
      public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
      {
        criteria.Expect(x => x.EntityType.IsAny(typeof(OldClass), typeof(AnotherOldClass)));
      }
    
      public void Apply(IClassInstance instance)
      {
        instance.Table("tbl_" + instance.EntityType.Name);
      }
    }

# Expectations

Expectations are what the Acceptance Criteria is all about. Expectations are recorded in the <code>Accept</code> definition then played back over your mappings; any mappings that satisfy the expectations will have the conventions applied to them.

An expectation is defined using the <code>Expect</code> method of the criteria instance. This method has two definitions, one that takes a <code>Func<TInspector, bool></code>, and one that takes a lambda and an acceptance criterion instance.

`criteria.Expect(x => x.EntityType == typeof(Example));`

That's the <code>Func<TInspector, bool></code> signature, where the body of the delegate (<code>x.EntityType == typeof(Example)</code>) can be substituted for any code as long as it returns a boolean. There are several extension methods available to help keep things clean when using this syntax, which you can read about [[#Extension methods|a bit further down]]. In this example we've set an expectation that the <code>EntityType</code> property of the mapping will be equal to the <code>Example</code> type.

`criteria.Expect(x => x.TableName, Is.Not.Set);`

That's the other signature, which takes a property lambda and a criterion instance. You can read more about criterions below, but for now all you need to know is they look at the property you specify and determine if it fits a certain criteria. In this case we're expecting that the <code>TableName</code> property hasn't been set.

## Chaining expectations

Expectations are cumulative, you can specify multiples and they will work together. Taking the examples we used above, we could chain them together for a convention that should only be applied to <code>Example</code> types without a table name.

    criteria
      .Expect(x => x.EntityType == typeof(Example))
      .Expect(x => x.TableName, Is.Not.Set);

You can chain as many expectations together as needed.

As the default behaviour of the Acceptance Criteria is effectively to **and** everything, there's separate methods available to simulate **or's**.

Firstly there's <code>Either</code>, which takes two arguments, both of which are sub-acceptance criteria's. If the mapping satisfies either of the criterias, then the convention will be applied.

    criteria.Either(
      sub =>
        sub.Expect(x => x.EntityType == typeof(Example)),
      sub =>
        sub.Expect(x => x.TableName, Is.Not.Set));

Anything that you can do on the main acceptance criteria can be done on the sub-criterias, and they can be as simple or complex as you need. This particular example expects either the type to be <code>Example</code> or the table name to be set.

Similar to <code>Either</code> is <code>Any</code>, which takes a params array of sub criterias, instead of just two.

> <code>Either</code> actually just calls <code>Any</code> behind the scenes with just two arguments.

    criteria.Any(
      sub =>
        sub.Expect(x => x.EntityType == typeof(Example)),
      sub =>
        sub.Expect(x => x.TableName, Is.Not.Set),
      sub =>
        sub.Expect(x => x.TableName == "tbl_Example"));

Of course, if you're purely using the single parameter <code>Expect</code> method then you could just use a regular **or**.

`criteria.Expect(x => x.EntityType == typeof(Example) || x.TableName == "tbl_Example");`

## Extension methods

We like the simplicity of the single parameter <code>Expect</code> method, but sometimes you can't always do what you need in there very easily; .Net doesn't provide a lot of useful methods on <code>IEnumerables</code> that can be applied in the context of the criteria. We give you a few methods to help with that.

For collections we've added <code>Contains</code>, which takes a predicate instead of a concrete instance; this makes it easier to check if a collection has a matching element rather than an exact instance. There's also a plain string overload, which will match against whatever is deemed to be the identifier of a particular mapping (usually the name, but sometimes it may be the type if a mapping doesn't have a name).

Also for collections there's a <code>IsEmpty</code> and <code>IsNotEmpty</code>, which equates to <code>collection.Count() == 0</code> but is a little shorter.

For everything else there's <code>IsAny</code>, which takes a params array of whatever type you're using it against, and returns true if any of the items are equal to the one you're calling it on.

## Criterions

Criterions are what you use in the right-hand side of the two parameter <code>Expect</code> method. There's really only one available right now, which is <code>Is.Set</code> (and the <code>Not</code> inverse).

`criteria.Expect(x => x.TableName, Is.Not.Set);`

The criterions are just implementations of the <code>IAcceptanceCriterion</code> interface, exposed through a static class. You can quite easily create your own implementations if you have some custom behaviour that you find yourself repeating a lot.