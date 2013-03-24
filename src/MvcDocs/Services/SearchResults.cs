using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public class SearchResults
	{
		public int Hits { get; set; }
		
		public IList<SearchResult> Documents { get; set; }

		public SearchResults()
		{
			this.Documents = new List<SearchResult>();
		}
	}
}