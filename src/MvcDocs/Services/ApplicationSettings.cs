using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public class ApplicationSettings : IApplicationSettings
	{
		public string GetRepositoryPath()
		{
			return System.Configuration.ConfigurationManager.AppSettings["App.Repository.Path"].EnsureAbsolutePath();
		}

		public string GetSearchIndexPath()
		{
			return System.Configuration.ConfigurationManager.AppSettings["App.Search.Index.Path"].EnsureAbsolutePath();
		}
	}
}