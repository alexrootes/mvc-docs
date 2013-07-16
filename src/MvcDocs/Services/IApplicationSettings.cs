using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public interface IApplicationSettings
	{
		string GetRepositoryPath();
		string GetSearchIndexPath();
		IEnumerable<string> GetHomeDocuments();
	}
}