This is an overview of the major changes that make up the 1.0 release. Specific changes since 0.1 aren't available as there have been so many, but you can see the changes that have been [[made since 1.0RC here|Release_notes 1.0RTM]].

# Fluent mapping

  * *Cleaned up method names* - Removed a lot of **noise** in method names, such as *WithLengthOf* is now just *Length*; ColumnName to Column, WithTableName to Table etc...
  * *Removed SetAttribute* - SetAttribute was a stop-gap measure to allow people to use Fluent NHibernate when we didn't support the attributes they needed. We've now gone to a great length to support all of the main attributes in the fluent interface, so you shouldn't need this anymore. If there are any attributes you need that we've missed, let us know (or even better, send us a pull request/patch)
  * *Separated subclass mapping* - Subclasses can (and should be) defined separately from their parent mapping. Use SubclassMap<T> the same way as you would ClassMap<T>; if the top-most mapping (ClassMap) contains a DiscriminateSubclassesOnColumn call, the subclasses will be mapped as table-per-class-hierarchy, otherwise (by default) they'll be mapped as table-per-subclass.

# Auto mapping

  * *Renamed the static entry point* - <code>AutoPersistenceModel.MapEntitiesFromAssemblyOf<Product></code> was always a bit *wordy*, it's been renamed to <code>AutoMap.AssemblyOf<Product></code>
  * *Renamed ForTypesThatDeriveFrom* - The method for overriding automappings was always a bit wordy, <code>ForTypesThatDeriveFrom</code>, it has now been shorted to <code>Override</code>.
  * *Easier to ignore base types* - If you often find yourself ignoring base types, then the <code>IgnoreBase<T></code> method should prove easier to swallow than <code>Setup(x => x.IsBaseType = /* ... */)</code>
  * *Components* - Improved support for components in automapping. They still work the same as before but now have support for all the collections and things they should have.<br />**See [[Auto_mapping#Components|components]] for more info.**
  * *IgnoreProperty for all types* - You can now use IgnoreProperty against multiple types, instead of on a per-entity basis.

# Conventions

  * *Always applied first* - They're now applied before any of your explicit settings in your ClassMap. This means that there's no accidently overwriting your mappings.
  * *Always apply by default* - Conventions used to have an Accept method that most people just either returned true, which meant apply to everything, or checked if a value had been set in the ClassMap; considering the above change, this meant Accept was mostly redundant. Conventions now always apply to everything. If you don't want that, there's an IClassAcceptance (and equivilant for other conventions) interface to add this behavior
  * *Acceptance Criteria* - For when you do need an Accept defined, there's a new criteria API for defining it in a much more consistent manner.<br />**See [[acceptance criteria]] for more info.**
  * *ForeignKeyConvention* - Base-class provided for setting the foreign-key naming consistently across the whole of your mappings.
  * *ManyToManyTableNameConvention* - Base-class (and defaults) for setting the table name of many-to-many's. This is much smarter than it was before, no overwriting explicit settings, support for bi-directional relationships.

For more information on conventions, please refer to the [[conventions]] page.