using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcDocs
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Documents_TOC",
				url: "docs/{product}/{language}/{version}",
				defaults: new { controller = "Documents", action = "Index" }
			);

			routes.MapRoute(
				name: "Documents",
				url: "docs/{product}/{language}/{version}/{*url}",
				defaults: new { controller = "Documents", action = "View" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}