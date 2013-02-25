There's been a point of contention for some users of Fluent NHibernate since the beginning, and that's the lack of a method of mapping private and protected properties on their domain entities.

The issue stems from our use of lambda expressions for static reflection of your entities, one of the appealing properties of Fluent NHibernate; by utilising expressions we're able to protect your mappings from refactoring side-effects. However, lambda expressions can only reference properties that are public on an object, so that makes it difficult to use against protected or private properties.

None of the solutions we have are ideal, we'll be the first to admit that; but considering Fluent NHibernate was never designed to support these situations, and the limitations C# imposes on us, we've got some pretty reasonable choices. Each option comes with its own compromises, so it's important you pick the method that has the compromises you're more willing to accept; I'll outline the pros and cons of each approach.

## Reveal static class and string-based names

    public class Product
    {
      private int Id { get; set; }
    }
    
    public ProductMap : ClassMap<Product>
    {
      public ProductMap()
      {
        Id(Reveal.Member<Product>("Id"));
      }  
    }

Our final option is different to the previous two, in that it utilises an expression generator to create an expression for a private or protected member. This is essentially what the first two are doing, just with strings instead of nesting tricks.

### Pros

  * No modifications to entities needed
  * Mappings and entity remain separate

### Cons

  * Potential renaming issues


## Nested expression exposition class

    public class Produc
    {
      private int Id { get; set; 
      
      public static class Expressions  
      {
        public static readonly Expression<Func<Product, object>> Id = x => x.Id
      }
    }
      
    public ProductMap : ClassMap<Product>
    {
      public ProductMap()
      {
        Id(Product.Expressions.Id);
      }
    }

This option takes advantage of an interesting side effect of nested class scope and access modifiers. If you haven't done something like this before, basically nested classes can access their parent's private/protected members. We create a nested static class that exposes an Expression for each hidden member. We can then use the expressions declared in this static class to reflect against the hidden members. It's reasonably clean, made even more so if you separate the Expressions class into a partial class of your entity; so you could have Product.cs and ProductExpressions.cs.

### Pros

  * Refactoring friendly
  * Mappings still readable

### Cons

  * Modification to entities required
  * Need 3 classes to map an entity (entity, expression class, and mapping)

## Nested mapping

    public class Product  
    {  
      private int Id { get; set; }  
      
      public ProductMap : ClassMap<Product>
      {  
        public ProductMap()  
        {  
          Id(x => x.Id);  
        }  
      }  
    }

Again using the scope trick, you can wrap your mapping inside your entity. This allows you to use the expressions as normal, without having to do any expression tricks. Like the expression class previously, this can be made neater by using partial classes.

### Pros

  * Refactoring friendly
  * Can use normal expressions

### Cons

  * Modification to entities required
  * Mapping nested in entity, so can't be separated across assemblies/namespaces

You have the power, now pick the one that suits you best. Compile time safety, or entity purity? You're now free to make the decision, instead of us.

I don't think anyone on the Fluent NHibernate team is particularly happy with that we have to write these hacks, but we're doing the best with what we've got. We all have different preferences, but at least there's something for everyone now.