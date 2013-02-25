These are the release notes for Fluent NHibernate 1.1. They're not exhaustive, and some points have been collated.

Read the [[upgrade guide|Release 1.1 upgrade guide]] if you are having any issues.

There are some breaking changes in this release, but they are minor. Some methods have been marked as obsolete, and you should follow the instructions on how to update your code; most obsolete methods are related to the automapping configuration changes (see below).

# General changes

  * Upgraded NHibernate binaries to 2.1.2GA
  * *Validation* - added clearer exceptions for when obvious mistakes are made when mapping classes (missing ids, for example)
  * *Fields* - private or otherwise, fields are now supported in the Fluent NHibernate automapper and using Reveal in the fluent interface

# Automapping

  * *External configuration* - compartmentalisation of the automapping configuration; no more giant-method-chain-lump. See: [[Auto mapping#Configuration|automapping configuration]]
  * *Multiple assembly support* - automap multiple assemblies at the same time; previously each assembly had to be configured individually
  * *Self-Referential relationships* - the automapper is now capable of understanding recursive/self-referential relationships
  * *Read-only properties* - you can now use read-only auto-properties, or backing-field based properties, to expose your collections in your entities.
  * *Intermediary base-type skipping* - you can ignore intermediary classes in an inheritance hierarchy without breaking the chain
  * *Collections of value types* - the automapper can now understand collections of value types
  * *Example project* - created an automapping example project.

# PersistenceSpecification

  * Additional checks are available, including enumerable support
  * VerifyTheMappings can be supplied with a pre-instantiated instance for when your entity doesn't have a default constructor
  * VerifyTheMappings returns the reloaded instance, which can be used for further assertions

# Databases

  * *Informix* database support
  * *Oracle8i* database support
  * *DB2* database support

# Fluent Interface

  * *ComponentMap* - map reusable components separate from where they'll be used.
  * Full entity-name support
  * Filters
  * Stored procedure support
  * Tuplizer for classes
  * Custom identity generator support
  * Bi-directional many-to-many method-based collections (pairing)
  * Natural Id support
  * Union subclass support
  * Nested components for collections
  * Improved multiple column support
  * Improved cascades, generators, and lazy compliance
  * General improved attribute compliance
  * Meta value and meta type support for any mappings

# Bug fixes

  * [automapping] Ignore static properties
  * [automapping] Map types in a predictable order
  * [automapping] Exclude components from being mapped as entities
  * [automapping] Joins and Discriminators in overrides not working
  * [automapping] Disabled dictionary automapping
  * [automapping] Removed SubclassStrategy in favour of the clearer IsDiscriminated
  * [conventions] ConventionBuilder not respecting acceptance criteria
  * [conventions] Collection conventions now apply to element and composite-element collections
  * [interface] Exceptions marked as serializable
  * [interface] Mutable defaults to true for class and collections
  * [interface] Component collection parent ordering issues
  * [interface] Class attribute written for components
  * [interface] byte[] version columns now work if they're declared in a baseclass
  * [interface] Formula properties don't output columns
  * [interface] HasOne lazy uses proxy instead of true
  * [interface] Properties in parallel subclasses were being excluded
  * [interface] GenericEnumMapper serializable
  * [interface] Added missing identity generators
  * [interface] Propertyless ids
  * [interface] Removed constraints from persister methods
  * [interface] Composite Id now respects reference and property ordering
  * [interface] Descriptive error messages for many-to-many
  * [interface] Extends support for SubclassMap
  * [interface] No proxy lazy support
  * [general] Export mappings to a TextWriter instead of just to file
  * [database] IsolationLevel in database config
  * [database] Password setting for SQLite configuration