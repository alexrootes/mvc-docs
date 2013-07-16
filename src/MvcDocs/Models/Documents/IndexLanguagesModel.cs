using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDocs.Models.Shared;
using MvcDocs.Services;

namespace MvcDocs.Models.Documents
{
	public class IndexLanguagesModel
	{
		public string Product { get; set; }
		public IList<string> Languages { get; set; } 

		public IndexLanguagesModel(string product, IList<string> languages)
		{
			this.Product = product;
			this.Languages = languages;
		}
	}
}