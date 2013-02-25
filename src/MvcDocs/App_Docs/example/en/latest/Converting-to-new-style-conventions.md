I'd advise that you read about the [[conventions]] first, and the [[available conventions]], once you understand how the new conventions work you can use this page to help migrate any existing ones.

# Replacing existing conventions

Below are all the original conventions (or near enough) with the equivalent interface or base-class you need to use.

I'll show some examples of updating your conventions after the table.

{| class="grid" style="width:80%"
|-
! Old style
! New style
|- style="text-align:left;"
| ITypeConvention
| IClassConvention
|- style="text-align:left;"
| IPropertyConvention
| Same but different signature
|- style="text-align:left;"
| Conventions.GetTableName
| IClassConvention or a [[convention shortcut]]
|- style="text-align:left;"
| Conventions.GetPrimaryKeyName and Conventions.GetPrimaryKeyNameFromType
| IIdConvention or a [[convention shortcut]]
|- style="text-align:left;"
| Conventions.GetForeignKeyName
| [[Available_conventions#ForeignKeyConvention|ForeignKeyConvention]] base-class, or the specific relationship interface you need (IHasManyConvention for example).
|- style="text-align:left;"
| Conventions.GetReadOnlyCollectionBackingFieldName
| ICollectionConvention
|- style="text-align:left;"
| Conventions.IdConvention
| IIdConvention
|- style="text-align:left;"
| Conventions.OneToManyConvention
| IHasManyConvention
|- style="text-align:left;"
| Conventions.ManyToOneConvention
| IReferenceConvention
|- style="text-align:left;"
| Conventions.OneToOneConvention
| IHasOneConvention
|- style="text-align:left;"
| Conventions.GetManyToManyTableName
| [[Available_conventions#ManyToManyTableNameConvention|ManyToManyTableNameConvention]] base-class
|- style="text-align:left;"
| Conventions.JoinConvention
| IJoinConvention
|- style="text-align:left;"
| Conventions.DefaultCache
| IClassConvention or a [[convention shortcut]]
|- style="text-align:left;"
| Conventions.GetVersionColumnName
| IVersionConvention
|- style="text-align:left;"
| Conventions.DefaultLazyLoad
| IClassConvention or a [[convention shortcut]]
|- style="text-align:left;"
| Conventions.DynamicUpdate
| IClassConvention or a [[convention shortcut]]
|- style="text-align:left;"
| Conventions.DynamicInsert
| IClassConvention or a [[convention shortcut]]
|- style="text-align:left;"
| Conventions.GetComponentColumnPrefix
| IComponentConvention
|}

# Primary Key naming

You used to write the following to override the primary key naming to be TableNameId:

    .WithConvention(c =>
      c.GetPrimaryKeyName = type => type.Name + "Id");

Now you'd implement an IIdConvention which allows you to alter the actual Id mapping itself.

    public class PrimaryKeyNamingConvention
      : IIdConvention
    {
      public void Apply(IIdentityInstance instance)
      {
        instance.Column(instance.EntityType.Name + "Id");
      }
    }

That's your new convention, which you'd situate with all your other conventions, then use either of the following snippets to hook them in:

    .Conventions.AddFromAssemblyOf<PrimaryKeyNamingConvention>();
    .Conventions.Add<PrimaryKeyNamingConvention>();

# Many-to-many table naming

You used to write the following to override the table name used for many-to-many relationships:

    .WithConvention(c =>
      c.GetManyToManyTableName = (child, parent) => child.Name + "To" + parent.Name);

Now you'd derive from the ManyToManyTableNameConvention which already has all the logic created for setting the other side of bi-directional relationships, and knows not to overwrite the table names if one already is set.

    public class CustomManyToManyTableNameConvention
      : ManyToManyTableNameConvention
    {
      protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
      {
        return collection.EntityType.Name + "To" + otherSide.EntityType.Name;
      }
     
      protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
      {
        return collection.EntityType.Name + "To" + collection.ChildType.Name;
      }
    }

That's your new convention, which you'd situate with all your other conventions, then use either of the following snippets to hook them in:

    .Conventions.AddFromAssemblyOf<ManyToManyTableNameConvention>();
    .Conventions.Add<ManyToManyTableNameConvention>();

# Automapping

If you're using automapping, the conventions used to be mixed up together with the configuration options. Now anything that can be thought of configuring the automapper's discovery capabilities (for example <code>IsBaseType</code>) is now available only under the <code>Setup</code> method (which in-part replaces <code>WithConvention</code>), everything else is a standard convention and is handled through the <code>Conventions</code> property like [[the regular conventions|conventions]].

    // old
    .WithConvention(c =>
      c.IsBaseType = type => type == typeof(BaseEntity));

    // new
    .Setup(s =>
      s.IsBaseType = type => type == typeof(BaseEntity))

# Attribute based conventions

If you were using the <code>ForAttribute<T></code> method, then you can inherit from the `AttributePropertyConvention` base-class.