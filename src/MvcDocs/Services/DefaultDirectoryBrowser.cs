using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;

namespace MvcDocs.Services
{
	public class DefaultDirectoryBrowser : IDirectoryBrowser
	{
		private static readonly IList<string> MarkdownExtensions = new[] {"markdown", "mdown", "mkdn", "md", "mkd", "mdwn", "mdtxt", "mdtext", "text"};

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
			if (!Directory.Exists(path))
				throw new InvalidOperationException(String.Format("Document root directory {0} does not exist. Please check settings.", path));
			var docRoot = new DirectoryInfo(path);
			return docRoot.GetDirectories()
			              .Where(directory => !directory.Attributes.HasFlag(FileAttributes.Hidden))
			              .Select(directory => directory.Name)
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

			return GetMarkdownFiles(path).Select(Path.GetFileNameWithoutExtension)
			                             .ToList();
		}

		public IEnumerable<MarkdownDocument> ListDocuments(DocumentRoot root)
		{
			return ListDocumentNames(root).Select(name => GetDocument(root, name));
		}

		public MarkdownDocument GetDocument(DocumentRoot root, string name)
		{
			var rootPath = GetAbsoluteDocumentRootPath(root);
			if (!MarkdownFileExists(rootPath, name))
				throw new Exception(string.Format("No document with the name {0} could be found. [{1}]", name, rootPath));
			
			var docPath = GetMarkdownFiles(rootPath, name).FirstOrDefault();

			return new MarkdownDocument(root, name, File.ReadAllText(docPath, Encoding.UTF8));
		}

		public bool DocumentExists(DocumentRoot root, string name)
		{
			var rootPath = GetAbsoluteDocumentRootPath(root);
			return MarkdownFileExists(rootPath, name);
		}

		private string GetAbsoluteDocumentRootPath(DocumentRoot root)
		{
			return Path.Combine(this.Settings.GetRepositoryPath(), root.ToPath());
		}

		private static IEnumerable<string> GetMarkdownFiles(string directory, string name = "*")
		{
			return MarkdownExtensions.SelectMany(ext => Directory.GetFiles(directory, String.Format("{0}.{1}", name, ext)));
		}

		private static bool MarkdownFileExists(string directory, string name)
		{
			return
				!String.IsNullOrEmpty(
					MarkdownExtensions.Select(ext => Path.Combine(directory, String.Format("{0}.{1}", name, ext)))
									  .FirstOrDefault(File.Exists));
		}
	}
}