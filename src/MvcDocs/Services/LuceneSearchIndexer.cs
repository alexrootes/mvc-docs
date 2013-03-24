using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace MvcDocs.Services
{
	public class LuceneSearchIndexer : ISearchIndexer
	{
		private IDirectoryBrowser Browser { get; set; }
		private IApplicationSettings Settings { get; set; }

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

			this.Browser = browser;
			this.Settings = settings;
		}

		public async void IndexAsync()
		{
			await Task.Run(() => RebuildIndex());
		}

		private void RebuildIndex()
		{
			this.Browser.ListDocumentRoots().ForEach(Index);
		}

		private void Index(DocumentRoot root)
		{
			var indexPath = this.Settings.GetSearchIndexPath();
			var documents = this.Browser.ListDocuments(root);

			var fordFiesta = new Document();
			fordFiesta.Add(new Field("Id", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
			fordFiesta.Add(new Field("Make", "Ford", Field.Store.YES, Field.Index.ANALYZED));
			fordFiesta.Add(new Field("Model", "Fiesta", Field.Store.YES, Field.Index.ANALYZED));

			var fordFocus = new Document();
			fordFocus.Add(new Field("Id", "2", Field.Store.YES, Field.Index.NOT_ANALYZED));
			fordFocus.Add(new Field("Make", "Ford", Field.Store.YES, Field.Index.ANALYZED));
			fordFocus.Add(new Field("Model", "Focus", Field.Store.YES, Field.Index.ANALYZED));

			var vauxhallAstra = new Document();
			vauxhallAstra.Add(new Field("Id", "3", Field.Store.YES, Field.Index.NOT_ANALYZED));
			vauxhallAstra.Add(new Field("Make", "Vauxhall", Field.Store.YES, Field.Index.ANALYZED));
			vauxhallAstra.Add(new Field("Model", "Astra", Field.Store.YES, Field.Index.ANALYZED));

			Directory directory = FSDirectory.Open(new DirectoryInfo(indexPath));
			Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);

			using (var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED))
			{
				writer.AddDocument(fordFiesta);
				writer.AddDocument(fordFocus);
				writer.AddDocument(vauxhallAstra);

				writer.Optimize();
			}			
		}
	}
}