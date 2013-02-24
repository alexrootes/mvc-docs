One of Fluent NHibernate's main goals is to reach feature parity with NHibernate. Currently, we've implemented many of the most used/core features of NHibernate within the Fluent model. There are still some cases that exist which are not supported in Fluent but are available in NHibernate. 

If the feature you're looking for is on this list, you have three options available to you:

  * Submit a patch with the functionality added to the Fluent NHibernate source. Open source survives by user contributions.
  * Use mixed mode mapping in the Fluent Configuration so you're using hbm.xml files for mappings not supported by Fluent, and Fluent files for mappings that are.
  * Wait for the main project contributors to get around to it. Typically, unless a we can establish an immediate user need for a feature, we hold off on implementing it.

Features not (currently) supported:
  
  * `<sql-insert>`
  * `<loader>`
  * `<database-object>`
  * `<sql-query>`
  * Changing Primary Key names (NHibernate limitation as well, currently)