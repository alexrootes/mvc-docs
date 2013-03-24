using System.Linq;
using System.Collections.Generic;
using System;

namespace MvcDocs
{
	public static class EnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T item in source)
				action(item);
		}
	}
}