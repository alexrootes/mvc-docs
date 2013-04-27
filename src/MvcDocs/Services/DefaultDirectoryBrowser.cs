using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;

namespace MvcDocs.Services
{
	public class DefaultDirectoryBrowser : IDirectoryBrowser
	{
		private const string DocumentExtension = ".md";

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
			var path = this.Settings.GetRepositoryPath();

			return Directory.GetDirectories(path)
				.Select(Path.GetFileName)
				.OrderBy(name => name)
				.ToList();
		}

		public IList<string> ListLanguages(string product)
		{
			var path = Path.Combine(this.Settings.GetRepositoryPath(), product);

			return Directory.GetDirectories(path)
				.Select(Path.GetFileName)
				.OrderBy(name => name)
				.ToList();
		}

		public IList<string> ListVersions(string product, string language)
		{
			var path = Path.Combine(this.Settings.GetRepositoryPath(), Path.Combine(product, language));

			return Directory.GetDirectories(path)
				.Select(Path.GetFileName)
				.OrderBy(name => name)
				.ToList();
		}

		public IEnumerable<DocumentRoot> ListDocumentRoots()
		{
			return (
				from product in ListProducts() 
				from language in ListLanguages(product) 
				from version in ListVersions(product, language) 
				select new DocumentRoot(product, language, version)
			);
		}

		public IEnumerable<string> ListDocumentNames(DocumentRoot root)
		{
			var path = Path.Combine(this.Settings.GetRepositoryPath(), root.ToPath());

			return Directory.GetFiles(path, "*" + DocumentExtension)
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
			var rootPath = GetAbsoluteDocumentRootPath(root);
			var docPath = Path.Combine(rootPath, name + DocumentExtension);

			if (File.Exists(docPath) == false)
			{
				throw new Exception(string.Format("No document with the name {0} could be found. [{1}]", name, rootPath));
			}

			return new MarkdownDocument { Title = name, Markdown = File.ReadAllText(docPath, Encoding.UTF8) };
		}

		private string GetAbsoluteDocumentRootPath(DocumentRoot root)
		{
			return Path.Combine(this.Settings.GetRepositoryPath(), root.ToPath());
		}
	}
}