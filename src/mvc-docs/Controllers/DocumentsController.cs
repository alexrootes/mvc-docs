using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MarkdownSharp;

namespace mvc_docs.Controllers
{
	public class DocumentsController : Controller
	{
		[HttpGet]
		public ActionResult Index(string product, string language, string version)
		{
			// show TOC

			return View();
		}

		[HttpGet]
		public ActionResult View(string product, string language, string version, string url)
		{
			var repo = Server.MapPath("~/App_Docs/");

			var file = url.Split("/"[0]).Last().Replace("-", " ");
			var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(file);

			// TODO: does file exist on disk, if so return it ...
			if (file.EndsWith(".png"))
			{
				return File(Path.Combine(repo, product + "/" + language + "/" + version + "/" + file), "image/png");
			}

			var path = Path.Combine(repo, product + "/" + language + "/" + version + "/" + url + ".md");

			if (System.IO.File.Exists(path) == false)
				return HttpNotFound();

			var raw = System.IO.File.ReadAllText(path, Encoding.UTF8);

			var html = new Markdown(new MarkdownOptions()).Transform(raw);

			const string internalLinkPattern = @"\[\[(.+?)\]\]"; // match: [[ any chars ]]

			Func<string, string> nameToDocPath = name =>
			{
				var encodedName = name.Replace(" ", "-");

				//return string.Join("/", folder, encodedName);
				return encodedName.ToLower();
			};

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
				var href = Url.Action("Index", "Documents", new { product = "example", version = "latest", url = docPath });

				return string.Format(internalLinkTemplate, href, text);
			};

			html = Regex.Replace(html, internalLinkPattern, match => interalLinkResolver(match));

			dynamic doc = new ExpandoObject();

			doc.Title = title;
			doc.Content = html;

			return View(doc);
		}
	}
}
