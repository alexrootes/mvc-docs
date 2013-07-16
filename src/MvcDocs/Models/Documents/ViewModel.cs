using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcDocs.Models.Documents
{
    public class ViewModel
    {
	    public string Title { get; set; }
	    public string Product { get; set; }
	    public string Html { get; set; }
		public IList<string> Languages { get; set; }
		public string Language { get; set; }
		public IList<string> Versions { get; set; }
		public string Version { get; set; }
		public string Url { get; set; }

		public ViewModel()
		{
			this.Languages = new List<string>();
			this.Versions = new List<string>();
		}

        public ViewModel(string title, string product, string language, string version, string html, string url)
			: this()
        {
	        this.Title = title;
	        this.Product = product;
	        this.Language = language;
	        this.Version = version;
	        this.Html = html;
	        this.Url = url;
        }
    }
}