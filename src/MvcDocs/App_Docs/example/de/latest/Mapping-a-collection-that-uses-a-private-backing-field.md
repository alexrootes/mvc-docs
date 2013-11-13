**This is part of the [[FAQs]]**

If your entity uses a backing field for a property, or some other non-standard design, then you can map it using the Access property.

    // model  
    public class Account  
    {  
       private IList<Customer> customers = new List<Customer>();  
    
       public IList<Customer> Customers  
       {  
         get { return customers; }  
       }  
    }  
  
    // mapping  
    HasMany(x => x.Customers)  
      .Access.CamelCaseField();  

The Access property can be used to set various combinations of Field and Property, with various casings and prefixes.

For example, for the same mapping but with the field called _customers you could use the Prefix.Underscore overload: Access.CamelCaseField(Prefix.Underscore).