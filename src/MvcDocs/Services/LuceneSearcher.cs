using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace MvcDocs.Services
{
	public class LuceneSearcher : ISearcher
	{
		private readonly IApplicationSettings _settings;

		public LuceneSearcher(IApplicationSettings settings)
		{
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}

			_settings = settings;
		}

		public SearchResults Search(DocumentRoot root, string term)
		{
			var results = new SearchResults();
			var indexPath = _settings.GetSearchIndexPath();
			var version = Lucene.Net.Util.Version.LUCENE_30;

			using (var directory = FSDirectory.Open(new DirectoryInfo(indexPath)))
			using (var indexReader = IndexReader.Open(directory, true))
			using (var indexSearch = new IndexSearcher(indexReader))
			{
				var analyzer = new StandardAnalyzer(version);
				var queryParser = new MultiFieldQueryParser(version, new[] { "Title", "Body" }, analyzer);
				var query = queryParser.Parse(term);

				var resultDocs = indexSearch.Search(query, indexReader.MaxDoc);
				var hits = resultDocs.ScoreDocs;
				foreach (var hit in hits)
				{
					var doc = indexSearch.Doc(hit.Doc);

					results.Documents.Add(new SearchResult
					{
						Score = hit.Score,
						Snippet = doc.Get("Snippet"),
						Title = doc.Get("Title")
					});
				}
			}

			return results;
		}
	}
}