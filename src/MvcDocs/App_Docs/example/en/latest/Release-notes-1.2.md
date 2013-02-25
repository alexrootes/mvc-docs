Below are the changes that make up our 1.2 release. This release is primarily a bug-fix release, with a few new features thrown in. The most significant changes are below, followed by everything else.

# Significant changes

  * Upgraded to NHibernate 3.1 [239cc5](http://github.com/jagregory/fluent-nhibernate/commit/239cc587e2122aac1c18f29415038c56facc1a3b)
  * [Auto-detect access strategies](http://wiki.fluentnhibernate.org/Fluent_mapping#Access_strategies) [91b6b5](http://github.com/jagregory/fluent-nhibernate/commit/91b6b509e69af2e5b75e5d4f7b60ee0c3937509f)
  * [Auto-detect collection type for IEnumerable collections](http://wiki.fluentnhibernate.org/Fluent_mapping#Collection_types) [91b6b5](http://github.com/jagregory/fluent-nhibernate/commit/91b6b509e69af2e5b75e5d4f7b60ee0c3937509f)
  * Added basic diagnostics [8c7ad8](http://github.com/jagregory/fluent-nhibernate/commit/8c7ad8d3887d7c5146a8982e06e9062986bf15e4)

# General

  * Obsoleted methods which'll be removed or changed in vNext [7019aa](http://github.com/jagregory/fluent-nhibernate/commit/7019aa0887a575e65ed95a2b1259315f05519a67) [5702ec](http://github.com/jagregory/fluent-nhibernate/commit/5702ec4cc5cce1c268cebddacf921e1f5b22965f)
  * Made FluentNHibernate.dll CLS compliant [c5d8de](http://github.com/jagregory/fluent-nhibernate/commit/c5d8de88bc20221b1b8b3845a380a1e972b3bba7)

# Bug fixes

  * Model generation bugfixes [f3e841](http://github.com/jagregory/fluent-nhibernate/commit/f3e8419a1c4aa826b0d6331ae7709453cac34d1d) [f3a846](http://github.com/jagregory/fluent-nhibernate/commit/f3a84670c8e593659136b8222dc53c49887cc5e8) [151de3](http://github.com/jagregory/fluent-nhibernate/commit/151de39c57ff3c11ef8347ae96777bbb1e1f8a33) [94bbd0](http://github.com/jagregory/fluent-nhibernate/commit/94bbd045423d8cb791e87cd43554fa43f8bd8178) [d5200c](http://github.com/jagregory/fluent-nhibernate/commit/d5200ca7a5e8ae5a1dfe2009bd076195dce04c1b)
  * Fixed key column regression with References [a71f81](http://github.com/jagregory/fluent-nhibernate/commit/a71f81f2e1000382a17f0d4160560b83f04008b0)
  * Mono namespace bugfix [64b2ff](http://github.com/jagregory/fluent-nhibernate/commit/64b2ffb12acb0ea182885edeb3caf17f467c2b29)
  * GeneratedBy returning wrong object [a10125](http://github.com/jagregory/fluent-nhibernate/commit/a101254a81555e6f3a9deab5a34007feb301e041)
  * Lazy on KeyReference was being set as True instead of Proxy [8f6900](http://github.com/jagregory/fluent-nhibernate/commit/8f690018d364acb606c04aed5abdb6ced0756de6)
  * Exception when mapping interface as user-type [3c1523](http://github.com/jagregory/fluent-nhibernate/commit/3c1523540367ca2b6390d73b336aeec2c50b2876)
  * Conventions can now override access strategies set by the automapper [8294f2](http://github.com/jagregory/fluent-nhibernate/commit/8294f2a911f83829e7fd9d78dcb378cd352fdffb) [de5964](http://github.com/jagregory/fluent-nhibernate/commit/de59640436c576ac2fa1480a19d4d0661b0b1dc8) [53e23c](http://github.com/jagregory/fluent-nhibernate/commit/53e23cf47b1d554f1cbe4213ff939122411af2d7)
  * Formulas now remove columns [2c635c](http://github.com/jagregory/fluent-nhibernate/commit/2c635cd67c46a8e4d2a67cfab70b101b2c7cd562)

# Configuration

  * Added CollectionTypeFactory support [020deb](http://github.com/jagregory/fluent-nhibernate/commit/020deb92d480002a54a296ad7151208780103f96)
  * Made Mappings call repeatable [371554](http://github.com/jagregory/fluent-nhibernate/commit/3715542cc4e5791f450282d94bc28c16cdb4719d)
  * Moved cache configuration from PersistentConfiguration to FluentConfiguration; moved methods CurrentSessionContext, ProxyFactoryFactory and CollectionTypeFactory to FluentConfiguration; added second level cache configuration. [448bba](http://github.com/jagregory/fluent-nhibernate/commit/448bba99dfe1e28ed48e58164d28d04b2d2bd9f8)
  * Added DB2 support for the AS400 [375f33](http://github.com/jagregory/fluent-nhibernate/commit/375f33e23ac0048549e1b559bcb11ac70da1487e)
  * Sybase SQL Anywhere support [6d3b1d](http://github.com/jagregory/fluent-nhibernate/commit/6d3b1d5b0e7c18e83e73ed944bbd89ddb6bc3531)

# Fluent interface

  * Added Access.ReadOnly() [259361](http://github.com/jagregory/fluent-nhibernate/commit/25936163a2332c4dde93f561c71af83acf57f2c8)
  * Added column prefixes to CustomType [7630ac](http://github.com/jagregory/fluent-nhibernate/commit/7630ac8dfd81eaa0b47446cac7738fe59224eb54)
  * Added OptimisticLock on References [0c1867](http://github.com/jagregory/fluent-nhibernate/commit/0c1867cc5d6356f68b200dff72e4844c48a8fee7)
  * Added name and length to key properties [ac86a5](http://github.com/jagregory/fluent-nhibernate/commit/ac86a5bd452526f8793f9bc5824efa79a85680d5)
  * Added multi-column support for Joins [58be48](http://github.com/jagregory/fluent-nhibernate/commit/58be48f2f536b16d31db9f6ee522d6ccd2acc4a8)
  * Validation warning for HasManyToMany collections with Inverse specified on both sides of the relationship [6215ca](http://github.com/jagregory/fluent-nhibernate/commit/6215cae2dfce0622d25e6b566bf97bafd37bd75c)
  * Added boolean support to the fluent Where clause generator [405c50](http://github.com/jagregory/fluent-nhibernate/commit/405c50dba0d38ed3f18c91080ba3df6fef0f7ddc)
  * Added HasMany support inside Joins [c2aed1](http://github.com/jagregory/fluent-nhibernate/commit/c2aed147e73ffb48d13614a9c0ec2514d2f73713)
  * Added SqlType to Discriminator [b390e6](http://github.com/jagregory/fluent-nhibernate/commit/b390e658b14c318aa9675e75982641b825ff1095)

# Conventions

  * Where and OrderBy added for child collections [4c96c4](http://github.com/jagregory/fluent-nhibernate/commit/4c96c437ff552fd2b50283ac51d7da41010e2ed9)
  * Added ApplyFilter support for classes and collections [fb8b9a](http://github.com/jagregory/fluent-nhibernate/commit/fb8b9afe9ac643ba2c2cf547ffe7759e8ec5b968)
  * Can set collection element type and name [d492cf](http://github.com/jagregory/fluent-nhibernate/commit/d492cfded5816b3a79b19bf8463fd0dfc3cef911) [ecef14](http://github.com/jagregory/fluent-nhibernate/commit/ecef14074f5b73b13683115878d2cf11405500d1)

# Automapping

  * Support nested classes [ae546f](http://github.com/jagregory/fluent-nhibernate/commit/ae546f6015adce67b1e12e7f23b996707c28fea9)
  * Support ComponentMap-based components [969fa0](http://github.com/jagregory/fluent-nhibernate/commit/969fa0afc6f679642d90ac0054fb84061591ea63) [3100cf](http://github.com/jagregory/fluent-nhibernate/commit/3100cf55ebe6b965c9ada6498ebda5710c53f606)
  * Support overrides from multiple assemblies [40abc6](http://github.com/jagregory/fluent-nhibernate/commit/40abc62fdfaca3e65fe227c7689b4b9c645e294a)
  * Can control version property detection [620a39](http://github.com/jagregory/fluent-nhibernate/commit/620a39b7700338c3ad58e930554e68ad81eeb280)

# Testing

  * Improved PersistenceSpecification.CheckProperty exception messages [4e64ff](http://github.com/jagregory/fluent-nhibernate/commit/4e64ffff2a651be24a6806e5e5045c6287569658)