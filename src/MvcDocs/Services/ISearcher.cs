using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public interface ISearcher
	{
		SearchResults Search(string productName, string language, string version, string[] terms);
	}
}