using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

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

		public static string Cut(this string source, int length)
		{
			if (string.IsNullOrEmpty(source) || source.Length < length)
				return source;

			return source.Substring(0, length);
		}

		public static string Highlight(this string source, string[] terms)
		{
			return source.Replace(terms[0], "<strong>" + terms[0] + "</strong>");
		}

		public static string FormatTitleForDisplay(this string source)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source).Replace("-", " ");
		}
	}
}