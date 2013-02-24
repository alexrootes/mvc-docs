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
    public class DocumentController : Controller
    {
        [HttpGet]
        public ActionResult Index(string product, string version, string url)
        {
            var repo = Server.MapPath("~/docs/");
          
            var file = url.Split("/"[0]).Last().Replace("-", " ");
            var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(file);

            // TODO: does file exist on disk, if so return it ...
            if (file.EndsWith(".png"))
            {
                return File(repo + file, "image/png");
            }

            var path = Path.Combine(repo, product + "/" + version + "/" + url + ".md");

            // does file exist? 404

            var raw = System.IO.File.ReadAllText(path, Encoding.UTF8);
            
            var html = new Markdown(new MarkdownOptions()).Transform(raw);

            const string internalLinkPattern = @"\[\[(.+?)\]\]"; // match: [[ any chars ]]

            Func<string, string> nameToDocPath = name =>
            {
                var encodedName = name.Replace(" ", "-");

                //return string.Join("/", folder, encodedName);
                return encodedName.ToLower();
            };

            //<a class="internal present" href="/alexrootes/fluent-nhibernate/wiki/Getting-started">Getting started guide</a>
            const string internalLinkTemplate = @"<a class='internal' href='{0}'>{1}</a>";
            Func<Match, string> interalLinkResolver = match =>
            {
                var value = match.Value.Substring(2, match.Value.Length - 4);

                var text = (value.Contains("|"))
                    ? value.Substring(0, value.IndexOf("|", StringComparison.Ordinal))
                    : value;

                var name = (value.Contains("|"))
                    ? value.Substring(value.IndexOf("|", StringComparison.Ordinal) + 1)
                    : value;

                var docPath = nameToDocPath(name);
                var href = Url.Action("Index", "Document", new { product = "example", version = "latest", url = docPath });

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
