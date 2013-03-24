using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using MarkdownSharp;
using MvcDocs.Models.Documents;
using Version = Lucene.Net.Util.Version;

namespace MvcDocs.Controllers
{
    public class DocumentsController : Controller
    {
        [HttpGet]
        public ActionResult Index(string product, string language, string version)
        {
            // show TOC
            var repo = FileHelper.GetRepositoryPath(product, language, version);

            IEnumerable<string> files;
            using (var reader = new DocumentRepositoryReader(repo))
            {
                files = reader.GetMarkDownFiles();
            }

            return View(files);
        }

        public ActionResult Search(string term)
        {
            var results = new List<dynamic>();

            using (var directory = FSDirectory.Open(new DirectoryInfo("C:\\Temp\\index")))
            using (var indexReader = IndexReader.Open(directory, true))
            using (var indexSearch = new IndexSearcher(indexReader))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                // multi-field example
                //var fields = new[] {"Make", "Model"};
                //var queryParser = new MultiFieldQueryParser(Version.LUCENE_30, fields, analyzer);

                var queryParser = new QueryParser(Version.LUCENE_30, "Make", analyzer);
                var query = queryParser.Parse("Ford");

                var resultDocs = indexSearch.Search(query, indexReader.MaxDoc);
                var hits = resultDocs.ScoreDocs;
                foreach (var hit in hits)
                {
                    var doc = indexSearch.Doc(hit.Doc);

                    dynamic result = new ExpandoObject();
                    result.Make = doc.Get("Make");
                    result.Model = doc.Get("Model");
                    results.Add(result);
                }
            }

            return View(results);
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
                var href = Url.Action("View", "Documents", new { product = "example", language = "en", version = "latest", url = docPath });

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
