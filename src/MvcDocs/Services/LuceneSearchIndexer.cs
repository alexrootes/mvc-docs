using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace MvcDocs.Services
{
	public class LuceneSearchIndexer : ISearchIndexer
	{
		private readonly IDirectoryBrowser _browser;
		private readonly IApplicationSettings _settings;

		public LuceneSearchIndexer(IDirectoryBrowser browser, IApplicationSettings settings)
		{
			if (browser == null)
			{
				throw new ArgumentNullException("browser");
			}

			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}

			_browser = browser;
			_settings = settings;
		}

		public async void IndexAsync()
		{
			await Task.Run(() => RebuildIndex());
		}

		private void RebuildIndex()
		{
			_browser.ListDocumentRoots().ForEach(Index);
		}

		private void Index(DocumentRoot root)
		{
			var indexPath = _settings.GetSearchIndexPath();
			var documents = _browser.ListDocuments(root);

			var directory = FSDirectory.Open(new DirectoryInfo(indexPath));
			var analyzer = new StandardAnalyzer(Version.LUCENE_30);

			using (var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
			{
				foreach (var doc in documents)
				{
					var luceneDoc = new Document();

					luceneDoc.Add(new Field("Title", doc.Title, Field.Store.YES, Field.Index.ANALYZED));
					luceneDoc.Add(new Field("Snippet", doc.Markdown.Cut(500), Field.Store.YES, Field.Index.NO));
					luceneDoc.Add(new Field("Body", doc.Markdown, Field.Store.NO, Field.Index.ANALYZED));

					writer.AddDocument(luceneDoc);
				}

				writer.Optimize();
			}
		}
	}
}