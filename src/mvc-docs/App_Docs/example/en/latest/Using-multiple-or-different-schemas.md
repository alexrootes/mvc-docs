**This is part of the [[FAQs]]**

Sometimes you may need to map a table that is in a different schema than the rest of your tables, to do this you need to use the Schema method on the ClassMap of your entity.

    public class PersonMap : ClassMap<Person>
    {
      public PersonMap()
      {
        Schema("alternativeSchema");
      }
    }

If you want to explicitly specify the schema for all the entities in your application, then you can use the [[database configuration]] to set that. There's also [evidence](http://geekswithblogs.net/billy/archive/2006/03/08/71736.aspx) that explicitly setting the schema can yield performance increases for querying.

To set the schema for your entire application, use the DefaultSchema method on your favorite DatabaseConfiguration.

    MsSqlServerConfiguration.MsSql2005
      .DefaultSchema("dbo");