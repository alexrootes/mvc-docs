using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace MvcDocs
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BuildSearchIndex();
        }

        private static void BuildSearchIndex()
        {
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
            
            Directory directory = FSDirectory.Open(new DirectoryInfo("C:\\Temp\\index"));
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