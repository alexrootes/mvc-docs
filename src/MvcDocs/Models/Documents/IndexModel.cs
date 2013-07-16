using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDocs.Models.Shared;
using MvcDocs.Services;

namespace MvcDocs.Models.Documents
{
	public class IndexModel
	{
		public DocumentRoot DocumentRoot { get; set; }
		public IEnumerable<string> Documents { get; set; } 

		public IndexModel(DocumentRoot documentRoot, IEnumerable<string> documents)
		{
			this.DocumentRoot = documentRoot;
			this.Documents = documents;
		}
	}
}