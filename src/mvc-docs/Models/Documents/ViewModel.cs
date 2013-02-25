using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc_docs.Models.Documents
{
	public static class FileHelper
	{
		public static string GetRepositoryPath(string product, string language, string version)
		{
			return HttpContext.Current.Server.MapPath("~/docs/" + product + "/" + version);
		}

		public static string[] ListDocuments(string path)
		{
			return System.IO.Directory.GetFiles(path, "*.md");
		}
	}

	public class DocumentRepositoryReader : IDisposable
	{
		public DocumentRepositoryReader(string path)
		{
			// open a stateful view of the document directory
		}

		public void Dispose()
		{
			// throw away any open file handles
		}
	}

	public class ViewModel
	{
		public FolderModel Folder { get; set; }

		public string Name { get; set; }
		public string Content { get; set; }

		public ViewModel(string product, string language, string version, string name)
		{
			var repo = FileHelper.GetRepositoryPath(product, language, version);

			using (var reader = new DocumentRepositoryReader(repo))
			{
				
			}

			// need version picker
			// need view > folder > file > sections

			//this.Content = document.Content;
			//this.Name = document.Name;

			//this.Folder = new FolderModel(folder);
		}

		public class FolderModel
		{
			public IList<DocumentModel> Documents { get; set; }
			public IList<FolderModel> Folders { get; set; }

			public string Name { get; set; }

			public FolderModel()
			{
				this.Documents = new List<DocumentModel>();
				this.Folders = new List<FolderModel>();

				//this.Documents = folder.Documents
				//	.Select(doc => new DocumentModel { Name = doc.Name })
				//	.OrderBy(doc => doc.Name)
				//	.ToList();
			}
		}

		public class DocumentModel
		{
			public string Name { get; set; }
		}
	}
}