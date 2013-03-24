using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs.Services
{
	public class DocumentRoot
	{
		public string Product { get; set; }
		public string Language { get; set; }
		public string Version { get; set; }

		public DocumentRoot(string product, string language, string version)
		{
			this.Product = product;
			this.Language = language;
			this.Version = version;
		}

		public string ToPath()
		{
			return string.Format("/{0}/{1}/{2}", this.Product, this.Language, this.Version);
		}
	}
}