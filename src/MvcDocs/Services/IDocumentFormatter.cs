using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public interface IDocumentFormatter
	{
		string ToHtml(MarkdownDocument document);
	}
}