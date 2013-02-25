This page details the changes that have been made **since** the 1.0RC release. For a general overview of changes since 0.1, you can read the [[overview 1.0 release notes|Release notes 1.0]].

# Features
  * Support Id mappings without a property, used for value-type style mappings. <code>Id<int>("id-column")</code>
  * Allow <code>null</code> as a valid value for nullable ids
  * Basic enum support to the HasMany Where expression parser
  * Brought version, discriminator, and id to the same level as property for column attribute support
  * Native generator with a sequence to identity generator
  * SchemaAction to ClassMap
  * Removed magic inside that was preventing use in medium trust scenarios
  * HashSet added to collection type auto-detection routines
  * Abstract classes that **aren't** layer supertypes to the automapper
  * Ignore open generic types in the automapper
  * format_sql in configuration
  * Custom identity generator class support
  * AutoImport convention
  * Id generator prediction support for the automapper


# Fixes
  * Ability to mix inline subclass mappings with SubclassMap based mappings
  * Inheritance hierarchies where every level may not be mapped
  * Interfaces as parents in inheritance hierarchies
  * Changed AsSet and AsMap to use IComparer<T> instead of IComparer
  * Stopped the debugger from evaluating the Not properties, which causes debugging nightmares
  * Any properties used in an automapped entity with a composite-id are now not re-mapped
  * ForeignKeyConvention breaking on automapped one-to-many's
  * <code>byte[]</code> version columns are now automapped as sql timestamp properties, following [ayende's example](http://ayende.com/Blog/archive/2009/04/15/nhibernate-mapping-concurrency.aspx)
  * Cache alterations working in the automapper again
  * Repeated Joins
  * Overriding generators in conventions
  * Collections always got a one-to-many element

# Misc
  * Renamed ForTypesThatDeriveFrom to Override 
  * Renamed ForAllTypes to OverrideAll