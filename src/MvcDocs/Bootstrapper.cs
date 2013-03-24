using System.Linq;
using System.Collections.Generic;
using System;
using Ninject;

namespace MvcDocs
{
	public class Bootstrapper
	{
		public static IKernel Kernel { get; set; }

		public static void Configure()
		{
			Kernel = new StandardKernel();

			Kernel.Load<SiteModule>();
		}
	}
}