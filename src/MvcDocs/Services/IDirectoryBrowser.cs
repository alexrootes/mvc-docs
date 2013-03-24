using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public interface IDirectoryBrowser
	{
		IList<string> ListProducts();
		IList<string> ListLanguages(string product);
		IList<string> ListVersions(string product, string language);

		IEnumerable<DocumentRoot> ListDocumentRoots();
		IEnumerable<string> ListDocumentNames(DocumentRoot root);
		IEnumerable<MarkdownDocument> ListDocuments(DocumentRoot root);

		MarkdownDocument GetDocument(DocumentRoot root, string name);
	}
}