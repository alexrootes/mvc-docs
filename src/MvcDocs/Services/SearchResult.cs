using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public class SearchResult
	{
		public string Title { get; set; }
		public float Score { get; set; }
		public string Snippet { get; set; }
	}
}