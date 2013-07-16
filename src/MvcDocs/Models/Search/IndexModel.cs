using System;
using System.Collections.Generic;
using System.Linq;
using MvcDocs.Services;

namespace MvcDocs.Models.Search
{
	public class IndexModel
	{
		public IList<DocumentModel> Documents { get; set; }

		public int Count { get; set; }
		public string Term { get; set; }
		public string Product { get; set; }

		public IndexModel(SearchResults results, string term, string product)
		{
			this.Count = results.Hits;
			this.Term = term;
			this.Product = product;

			this.Documents = results.Documents.Select(r => new DocumentModel
			{
				Url = r.Title.ToLower(),
				Title = r.Title.FormatTitleForDisplay(),
				SnippetHtml = r.Snippet.Highlight(term.Split(" "[0])),
				Score = Math.Round(r.Score, 4)
			})
			.ToList();
		}

		public class DocumentModel
		{
			public string Url { get; set; }
			public string Title { get; set; }
			public string SnippetHtml { get; set; }
			public double Score { get; set; }
		}
	}
}