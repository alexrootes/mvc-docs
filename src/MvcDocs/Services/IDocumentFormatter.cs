using System.Linq;
using System.Collections.Generic;
using System;
using System.Web.Mvc;

namespace MvcDocs.Services
{
	public interface IDocumentFormatter
	{
		string ToHtml(MarkdownDocument doc, UrlHelper urlHelper);
	}
}