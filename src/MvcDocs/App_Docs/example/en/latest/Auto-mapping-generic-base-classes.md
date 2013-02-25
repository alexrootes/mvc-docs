It's common to extract properties common to all (or most) of your entities into a base-class. Sometimes you may want your base-class to be an open generic type, with which each inheritor can change the generic argument for different behaviour.

An example of this is using a single generic argument to dictate the Id type.

    public abstract class BaseEntity<T>  
    {  
      public T Id { get; private set; }  
    }

When using this kind of base-class, you just need to ignore it like any other base type.

    AutoMap.AssemblyOf<Entity>()
      .IgnoreBase(typeof(BaseEntity<>));

You just need to remember to use the open type definition for it to apply to all subclasses (`BaseEntity<>` instead of `BaseEntity<int>`).