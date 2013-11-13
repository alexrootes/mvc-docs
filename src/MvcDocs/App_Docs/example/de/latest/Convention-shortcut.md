There are some shortcuts available for [[conventions]] that can be used inline (much like the old convention style); they're to be used for very simple situations where some people might think defining new types is overkill.

> Creating implementations of the [[interface convention types|available conventions]] is always the recommended approach, but if you think otherwise these are available.

# Builders

There is a ConventionBuilder class that you can use to define simple conventions. The general rule-of-thumb is if your convention has any logic, or spans multiple lines, then don't use the builders. That being said, they can be useful for simple scenarios.

The ConventionBuilder defines several static properties, one for just about all the [[available conventions]]. Through each property you can access two methods, Always and When. These can be used to create simple conventions that are either always applied, or only applied in particular circumstances.

## Always

These inline conventions are applied to every mapping instance of their type, they're handy for catch-all situations.

    ConventionBuilder.Class.Always(x => x.Table(x.EntityType.Name.ToLower()))
    ConventionBuilder.Id.Always(x => x.Column("ID"))

## When

These conventions are only applied when the first parameter evaluates to true.

    ConventionBuilder.Class.When(
      c => c.Expect(x.TableName, Is.Not.Set), // when this is true
      x => x.Table(x.EntityType.Name + "Table") // do this
    )

# Even shorter shortcuts

There are some situations that are so obvious that they just cried out for an even simpler shortcut.

    Table.Is(x => x.EntityType.Name + "Table")
    PrimaryKey.Name.Is(x => "ID")
    AutoImport.Never()
    DefaultAccess.Field()
    DefaultCascade.All()
    DefaultLazy.Always()
    DynamicInsert.AlwaysTrue()
    DynamicUpdate.AlwaysTrue()
    OptimisticLock.Is(x => x.Dirty())
    Cache.Is(x => x.AsReadOnly())
    ForeignKey.EndsWith("ID")

## Usage

To use these conventions you just need to call Add on the [[fluent configuration]] Conventions property. This method accepts a generics parameter of T, where T is constrained to IConvention interfaces.

    Fluently.Configure()
      .Database(/* database config */)
      .Mappings(m =>
      {
        m.FluentMappings
          .AddFromAssemblyOf<Entity>()
          .Conventions.Add(PrimaryKey.Name.Is(x => "ID"));
      })

The Add method accepts a params array, so you can specify multiple conventions together.

    .Conventions.Add(
      PrimaryKey.Name.Is(x => "ID"),
      DefaultLazy.Always(),
      ForeignKey.EndsWith("ID")
    )