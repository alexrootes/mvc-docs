If you don't know what conventions are, you should read the [[conventions]] page first. Here you can find details of each of the individual conventions that you're able to create.

Your conventions are made by either deriving from a particular class supplied by Fluent NHibernate (`ForeignKeyConvention` for example), or by implementing one of the many interfaces we provide.

# Base-classes

We provide several classes that you can derive from to help you with common conventions, naming of foreign keys, many-to-many table names; these are particularly useful when the convention may have some logic behind it that you aren't really interested in implementing.

## AttributePropertyConvention<T>

The `AttributePropertyConvention<T>` is a special convention that only gets applied to properties that have a particular attribute on them, that attribute being defined by the generic argument `T`.

To use this convention, subclass it and supply the attribute as the generic argument, then implement the abstract `Apply` method.

    public class ThereBeDragonsAttribute : Attribute
    {}

    public class ThereBeDragonsConvention
      : AttributePropertyConvention<ThereBeDragonsAttribute>
    {
      protected override void Apply(ThereBeDragonsAttribute attribute, IPropertyInstance instance)
      {
        // do something to the instance
      }
    }

## ForeignKeyConvention

The `ForeignKeyConvention` is an amalgamation of several other conventions to provide an easy way to specify the naming scheme for all foreign-keys in your domain. This is particularly useful because not all the foreign-keys are accessible in the same way, depending on where they are; this convention negates need to know about the underlying structure.

The only consideration that needs to be made is that for many-to-one relationships you're setting the name of the key on the current entity, while all others you're setting the key on the **other** entity. This manifests itself in many-to-one's receiving a `PropertyInfo` instance to build their key from, and everything else a `Type` instance.

> Suggestions are welcome for this signature, as it's not exactly ideal.

    public class CustomForeignKeyConvention
      : ForeignKeyConvention
    {
      protected override string GetKeyName(PropertyInfo property, Type type)
      {
        if (property == null)
          return type.Name + "ID";  // many-to-many, one-to-many, join
        
        return property.Name + "ID"; // many-to-one
      }
    }

## ManyToManyTableNameConvention

The `ManyToManyTableNameConvention` is, as the name describes, used for setting the name of the many-to-many tables. This convention has quite a bit of logic in to make sure that bi-directional many-to-many's have both sides of the relationship set correctly, and that it doesn't overwrite any explicit table names set in the fluent interface.

There are two methods you need to implement, one for uni-directional many-to-many's, and the other for bi-directional ones.

    public class CustomManyToManyTableNameConvention
      : ManyToManyTableNameConvention
    {
      protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
      {
        return collection.EntityType.Name + "_" + otherSide.EntityType.Name;
      }
     
      protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
      {
        return collection.EntityType.Name + "_" + collection.ChildType.Name;
      }
    }

## UserTypeConvention<T>

The `UserTypeConvention<T>` is used to change any properties to use the `IUserType` of `T` that could accept it. What that means is that any properties in your entire domain that have a type that matches the `ReturnedType` property of the IUserType, will be updated to use that IUserType.

So given this IUserType:

    public class LatitudeUserType : IUserType
    {
      /* snip */

      public Type ReturnedType
      {
        get { return typeof(Latitude); }
      }
    }

With this convention:

    public class LatitudeUserTypeConvention
      : UserTypeConvention<LatitudeUserType>
    {}

Any properties that are of the type `Latitude` will be automatically updated to use the `IUserType` for persistence.

You can override the `Accept` or `Apply` methods in your convention if you need to do anything else other than just setting the type.

# Interfaces

There are **many** interfaces available to implement for your conventions, you can find them all in the `FluentNHibernate.Conventions` namespace; below are the most common ones that you're likely to need, most of the other ones are pretty esoteric. You can see the full list in the [Conventions folder](http://github.com/jagregory/fluent-nhibernate/tree/master/src/FluentNHibernate/Conventions) on github.

**'IClassConvention**' - Use to alter your `ClassMap`s values. You can't change properties, collections, etc with this convention, only alter the settings such as `Lazy` and `BatchSize`.

**'IIdConvention**' - Use to alter any properties mapped with `Id`.

**'IPropertyConvention**' - Use to alter any properties mapped with `Map`; this is useful for setting access strategies and column names globally.

**'IHasManyConvention**' - Used to alter HasMany/one-to-many relationship mappings; good for setting cascades.

**'IHasManyToManyConvention**' - Used to alter HasManyToMany/many-to-many relationship mappings; good for setting cascades.

**'IReferenceConvention**' - Used to alter References/many-to-one relationships.

**'IHasOneConvention**' - Used to alter HasOne/one-to-one relationships.

**'IVersionConvention**' - Used to alter Version mappings.

**'IJoinConvention**' - Used to alter Join mappings.

**'IComponentConvention**' - Used to alter Component mappings.

**'IDynamicComponentConvention**' - Used to alter DynamicComponent mappings.

**'ISubclassConvention**' - Used to alter Subclasses.

**'IJoinedSubclassConvention**' - Used to alter JoinedSubclasses.