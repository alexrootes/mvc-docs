using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcDocs.Services;

namespace MvcDocs.Controllers
{
    public class SearchController : Controller
    {
		private ISearcher Searcher { get; set; }

		public SearchController(ISearcher searcher)
		{
			if (searcher == null)
			{
				throw new ArgumentNullException("searcher");
			}

			this.Searcher = searcher;
		}

		[HttpGet]
        public ActionResult Index(string productName, string language, string version, string term)
		{
			var results = this.Searcher.Search(productName, language, version, term.Split(" "[0]));

			return View(results);
        }
    }
}
