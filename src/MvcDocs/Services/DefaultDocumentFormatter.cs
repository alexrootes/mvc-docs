using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public class DefaultDocumentFormatter : IDocumentFormatter
	{
		public string ToHtml(MarkdownDocument document)
		{
			return document.Markdown;
		}
	}
}