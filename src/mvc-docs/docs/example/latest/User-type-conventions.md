## Using conventions for user defined types
There are times when you have a complex type - let's say, Instant from [NodaTime](http://code.google.com/p/noda-time/) library - and you don't exactly know how to store it in your database. There are at least two ways of handling this situation - define a component for this type or implement a IUserType. At the time of this writing, Component mapping has a downside - it's not _currently_ compatible with automapping.

## Integrating IUserType into your existing mapping infrastructure
Getting to persist your magic type in the database using IUserType is easy - all you need to do is:
* implement IUserType interface which is part of NHibernate core library
* implement corresponding UserTypeConvention to let Fluent know how exactly do you want it mapped