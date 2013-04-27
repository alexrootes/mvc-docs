using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcDocs.Services;

namespace MvcDocs.Models.Shared
{
	public class DocumentRootModel
	{
		[Required]
		public string Product { get; set; }
		[Required]
		public string Language { get; set; }
		[Required]
		public string Version { get; set; }

		public DocumentRoot ToDocumentRoot()
		{
			return new DocumentRoot(this.Product, this.Language, this.Version);
		}
	}
}