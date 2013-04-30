using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public class MarkdownDocument
	{
		public DocumentRoot Root { get; set; }

		public string Title { get; set; }
		public string Markdown { get; set; }

		public MarkdownDocument(DocumentRoot root, string title, string markdown)
		{
			this.Root = root;
			this.Title = title;
			this.Markdown = markdown;
		}
	}
}