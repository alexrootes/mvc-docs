using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDocs.Models.Shared;
using MvcDocs.Services;

namespace MvcDocs.Models.Documents
{
	public class IndexVersionsModel
	{
		public string Product { get; set; }
		public string Language { get; set; }
		public IList<string> Versions { get; set; }

		public IndexVersionsModel(string product, string language, IList<string> versions)
		{
			this.Product = product;
			this.Language = language;
			this.Versions = versions;
		}
	}
}