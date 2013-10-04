using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MarkdownDeep;

namespace MvcDocs.Services
{
	public class AdvancedDocumentFormatter : IDocumentFormatter
	{
		public string ToHtml(MarkdownDocument doc, UrlHelper urlHelper)
		{
			var html = new Markdown().Transform(doc.Markdown);

			const string internalLinkPattern = @"\[\[(.+?)\]\]"; // match: [[ any chars ]]

			Func<string, string> nameToDocPath = name => name.Replace(" ", "-").ToLower();

			const string internalLinkTemplate = @"<a class='internal' href='{0}'>{1}</a>";
			Func<Match, string> interalLinkResolver = match =>
			{
				// match.Value = [[text|doc_name]] or [[doc_name]]
				var value = match.Value.Substring(2, match.Value.Length - 4);

				var text = (value.Contains("|"))
					? value.Substring(0, value.IndexOf("|", StringComparison.Ordinal))
					: value;

				var name = (value.Contains("|"))
					? value.Substring(value.IndexOf("|", StringComparison.Ordinal) + 1)
					: value;

				var docPath = nameToDocPath(name);
				var root = doc.Root;
				var href = urlHelper.Action("View", "Documents", new { root.Product, root.Language, root.Version, url = docPath });

				return string.Format(internalLinkTemplate, href, text);
			};

			return Regex.Replace(html, internalLinkPattern, match => interalLinkResolver(match));
		}
	}
}