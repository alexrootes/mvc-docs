using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs
{
	public static class StringExtensions
	{
		public static string EnsureAbsolutePath(this string path)
		{
			if (path.StartsWith("~/") == false)
			{
				return path;
			}

			return System.Web.Hosting.HostingEnvironment.MapPath(path);
		}
	}
}