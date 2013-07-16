using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDocs.Models.Shared;
using MvcDocs.Services;

namespace MvcDocs.Models.Documents
{
	public class IndexProductsModel
	{
		public IList<string> Products { get; set; }
		
		public IndexProductsModel(IList<string> products)
		{
			this.Products = products;
		}
	}
}