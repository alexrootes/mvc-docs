Fluent NHibernate is distributed with a [Ruby](http://ruby-lang.org/) [Rake](http://rake.rubyforge.org/) script, which can be used to build without having to open Visual Studio; more importantly, it also provides a simple mechanism for switching versions of NHibernate (see the [[supported NHiberate versions]] for more details on our versioning policy).

# Building with rake

Before you can run the rake script, you need to make sure you have all the necessary [gems](http://www.rubygems.org/) installed; the easiest way to do that is to run the <code>InstallGems.bat</code> distributed with Fluent NHibernate.

Once you've got all the gems installed, you can either run <code>Build.bat</code> which just runs rake, or you can run rake directly: <code>rake</code>. Either of those will use [MSBuild](http://en.wikipedia.org/wiki/MSBuild) to build the solution, and then will run the unit tests before copying the built files to the <code>build</code> directory.

When the build is complete, you can reference the Fluent NHibernate binaries from the <code>build</code> directory.