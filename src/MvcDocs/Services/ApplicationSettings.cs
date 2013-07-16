using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace MvcDocs.Services
{
	public class ApplicationSettings : IApplicationSettings
	{
		private const char HomeDocumentsDelimiter = ';';

		public string GetRepositoryPath()
		{
			return GetSetting("App.Repository.Path").EnsureAbsolutePath();
		}

		public string GetSearchIndexPath()
		{
			return GetSetting("App.Search.Index.Path").EnsureAbsolutePath();
		}

		public IEnumerable<string> GetHomeDocuments()
		{
			var homeDocuments = GetSetting("App.Repository.HomeDocuments");
			return String.IsNullOrEmpty(homeDocuments)
				       ? (IEnumerable<string>) new List<string>()
				       : homeDocuments.Split(new[] {HomeDocumentsDelimiter}, StringSplitOptions.RemoveEmptyEntries);
		}

		private static string GetSetting(string name)
		{
			return System.Configuration.ConfigurationManager.AppSettings[name];
		}
	}
}