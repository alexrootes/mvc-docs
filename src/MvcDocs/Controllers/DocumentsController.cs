using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcDocs.Models.Documents;
using MvcDocs.Models.Shared;
using MvcDocs.Services;

namespace MvcDocs.Controllers
{
    public class DocumentsController : Controller
    {
	    private readonly IDirectoryBrowser _browser;
	    private readonly IDocumentFormatter _formatter;

		public DocumentsController(IDirectoryBrowser browser, IDocumentFormatter formatter)
		{
			if (browser == null)
			{
				throw new ArgumentNullException("browser");
			}

			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}

			this._browser = browser;
			this._formatter = formatter;
		}

        [HttpGet]
        public ActionResult Index(DocumentRootModel model)
        {
	        var root = model.ToDocumentRoot();

            return View(
				_browser.ListDocumentNames(root)
			);
        }

        [HttpGet]
        public ActionResult View(DocumentRootModel rootModel, string url)
        {
			// HACK: find a better way to resolve image urls to disk locations ...
			if (url.EndsWith(".png"))
			{
				var repo = Server.MapPath("~/App_Docs/");
				return File(Path.Combine(repo, rootModel.Product + "/" + rootModel.Language + "/" + rootModel.Version + "/" + url), "image/png");
			}
			
			var doc = _browser.GetDocument(rootModel.ToDocumentRoot(), url);

			if (doc == null)
			{
				return HttpNotFound();
			}

	        return View(
				new ViewModel(doc.Title, rootModel.Product, _formatter.ToHtml(doc, Url))
			);
        }
    }
}
