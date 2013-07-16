using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MvcDocs.Models.Search;
using MvcDocs.Models.Shared;
using MvcDocs.Services;

namespace MvcDocs.Controllers
{
	public class SearchController : Controller
	{
		private readonly ISearcher _searcher;

		public SearchController(ISearcher searcher)
		{
			if (searcher == null)
			{
				throw new ArgumentNullException("searcher");
			}

			_searcher = searcher;
		}

		[HttpGet]
		public ActionResult Index(DocumentRootModel rootModel, [Required] string term)
		{
			if (ModelState.IsValid == false)
			{
				return HttpNotFound();
			}

			var root = rootModel.ToDocumentRoot();

			var results = _searcher.Search(root, term);

			return View(
				new IndexModel(results, term, rootModel.Product)
			);
		}
	}
}
