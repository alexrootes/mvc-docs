using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
			{
				return source;
			}

			return source.Substring(0, length);
		}

		public static string Highlight(this string source, string[] terms)
		{
			if (terms == null || terms.Length == 0)
			{
				return source;
			}

			var pattern = @"\b(" + terms.Aggregate((x, y) => x + "|" + y) + @")\b";

			return Regex.Replace(source, pattern, "<span class=\"hl\">$0</span>", RegexOptions.IgnoreCase);
		}

		public static string FormatTitleForDisplay(this string source)
		{
			if (string.IsNullOrEmpty(source))
			{
				return source;
			}

			var textInfo = CultureInfo.CurrentCulture.TextInfo;

			return textInfo.ToTitleCase(source).Replace("-", " ");
		}

		public static string FormatVersionForDisplay(this string source)
		{
			return string.IsNullOrEmpty(source) ? source : source.Replace("_", ".");
		}
	}
}