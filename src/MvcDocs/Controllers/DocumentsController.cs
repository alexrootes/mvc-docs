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
	    private readonly IApplicationSettings _settings;

		public DocumentsController(IDirectoryBrowser browser, IDocumentFormatter formatter, IApplicationSettings settings)
		{
			if (browser == null)
			{
				throw new ArgumentNullException("browser");
			}

			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}

			this._browser = browser;
			this._formatter = formatter;
			this._settings = settings;
		}

        [HttpGet]
        public ActionResult Index(DocumentRootModel model)
        {
	        var root = model.ToDocumentRoot();

	        var homeDoc = _settings.GetHomeDocuments().FirstOrDefault(doc => _browser.DocumentExists(root, doc));
	        if (homeDoc == null) return View(new IndexModel(root, _browser.ListDocumentNames(root)));
	        return RedirectToAction("View", new { url = homeDoc } );
        }

		[HttpGet]
		public ActionResult IndexVersions(string product, string language)
		{
			return View(new IndexVersionsModel(product, language, _browser.ListVersions(product, language)));
		}

		[HttpGet]
		public ActionResult IndexLanguages(string product)
		{
			return View(new IndexLanguagesModel(product, _browser.ListLanguages(product)));
		}

		[HttpGet]
		public ActionResult IndexProducts()
		{
			return View(new IndexProductsModel(_browser.ListProducts()));
		}

        [HttpGet]
        public ActionResult View(DocumentRootModel rootModel, string url)
        {
			// HACK: find a better way to resolve image urls to disk locations ...
			if (MimeSniffer.IsImage(url))
			{
				var repo = this._settings.GetRepositoryPath();
				return File(Path.Combine(repo, rootModel.ToDocumentRoot().ToPath(), url), MimeSniffer.GetMimeForPath(url));
			}
			
			var doc = _browser.GetDocument(rootModel.ToDocumentRoot(), url);

			if (doc == null)
			{
				return HttpNotFound();
			}

	        return View(
		        new ViewModel(doc.Title, rootModel.Product, rootModel.Language, rootModel.Version, _formatter.ToHtml(doc, Url),
		                      url)
			        {
				        Languages =
					        _browser.ListLanguages(rootModel.Product)
					                .Where(language => _browser.DocumentExists(rootModel.ToDocumentRoot().OfLanguage(language), url))
					                .ToList(),
				        Versions =
					        _browser.ListVersions(rootModel.Product, rootModel.Language)
					                .Where(version => _browser.DocumentExists(rootModel.ToDocumentRoot().OfVersion(version), url))
					                .ToList()
			        }
		        );
        }
    }
}
