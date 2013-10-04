using System.Linq;
using System.Collections.Generic;
using System;
using MvcDocs.Services;
using Ninject.Modules;

namespace MvcDocs
{
	public class SiteModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IApplicationSettings>().To<ApplicationSettings>();
			Bind<ISearchIndexer>().To<LuceneSearchIndexer>();
			Bind<ISearcher>().To<LuceneSearcher>();
			Bind<IDirectoryBrowser>().To<DefaultDirectoryBrowser>();
			Bind<IDocumentFormatter>().To<AdvancedDocumentFormatter>();
		}
	}
}