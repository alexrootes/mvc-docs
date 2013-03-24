using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;

namespace MvcDocs.Services
{
	public class DefaultDirectoryBrowser : IDirectoryBrowser
	{
		private IApplicationSettings Settings { get; set; }

		public DefaultDirectoryBrowser(IApplicationSettings settings)
		{
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}

			this.Settings = settings;
		}

		public IList<string> ListProducts()
		{
			throw new NotImplementedException();
		}

		public IList<string> ListLanguages(string product)
		{
			throw new NotImplementedException();
		}

		public IList<string> ListVersions(string product, string language)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<DocumentRoot> ListDocumentRoots()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> ListDocumentNames(DocumentRoot root)
		{
			var path = Path.Combine(this.Settings.GetRepositoryPath(), root.ToPath());

			return Directory.GetFiles(path, "*.md")
				.Select(Path.GetFileName)
				.Select(f => f.Substring(0, f.Length - 3)) // strip file extension
				.ToList();
		}

		public IEnumerable<MarkdownDocument> ListDocuments(DocumentRoot root)
		{
			return ListDocumentNames(root).Select(name => GetDocument(root, name));
		}

		public MarkdownDocument GetDocument(DocumentRoot root, string name)
		{
			var path = GetAbsoluteDocumentRootPath(root);

			if (File.Exists(path) == false)
			{
				throw new Exception(string.Format("No document with the name {0} could be found. [{1}]", name, path));
			}

			return new MarkdownDocument { Title = name, Markdown = File.ReadAllText(path, Encoding.UTF8) };
		}

		private string GetAbsoluteDocumentRootPath(DocumentRoot root)
		{
			return Path.Combine(this.Settings.GetRepositoryPath(), root.ToPath());
		}
	}
}