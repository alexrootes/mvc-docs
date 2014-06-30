using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MvcDocs.Services;
using Ninject;

namespace MvcDocs
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			Bootstrapper.Configure();

			// register DI with the asp.net framework
			DependencyResolver.SetResolver(
				t => Bootstrapper.Kernel.TryGet(t),
				t => Bootstrapper.Kernel.GetBindings(t)
			);

			// start an async build of our search indexes
			Bootstrapper.Kernel.Get<ISearchIndexer>().IndexAsync();

            MvcHandler.DisableMvcResponseHeader = true;
		}
	}
}