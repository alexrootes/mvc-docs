using System;
using System.Linq;
using System.Collections.Generic;

namespace MvcDocs.Services
{
	public class ApplicationSettings : IApplicationSettings
	{
		public string GetRepositoryPath()
		{
			return GetSetting("App.Repository.Path").EnsureAbsolutePath();
		}

		public string GetSearchIndexPath()
		{
			return GetSetting("App.Search.Index.Path").EnsureAbsolutePath();
		}

		private static string GetSetting(string name)
		{
			return System.Configuration.ConfigurationManager.AppSettings[name];
		}
	}
}