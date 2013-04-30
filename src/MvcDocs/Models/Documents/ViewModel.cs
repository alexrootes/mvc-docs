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

        public ViewModel(string title, string product, string html)
        {
	        this.Title = title;
	        this.Product = product;
	        this.Html = html;
        }
    }
}