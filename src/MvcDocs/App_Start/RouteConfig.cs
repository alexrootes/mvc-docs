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
				name: "Documents_Versions",
				url: "docs/{product}/{language}",
				defaults: new { controller = "Documents", action = "IndexVersions" }
			);

			routes.MapRoute(
				name: "Documents_Languages",
				url: "docs/{product}",
				defaults: new { controller = "Documents", action = "IndexLanguages" }
			);

			routes.MapRoute(
				name: "Documents_Products",
				url: "docs",
				defaults: new { controller = "Documents", action = "IndexProducts" }
			);

			routes.MapRoute(
				name: "Search",
				url: "search/{product}/{language}/{version}/",
				defaults: new { controller = "Search", action = "Index" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}