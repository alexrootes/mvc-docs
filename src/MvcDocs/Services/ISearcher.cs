using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public interface ISearcher
	{
		SearchResults Search(DocumentRoot root, string term);
	}
}