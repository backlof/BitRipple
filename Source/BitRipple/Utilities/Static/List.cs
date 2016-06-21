using System.Collections.Generic;
using System.Linq;

namespace BitRipple.Utilities.Static
{
	public static class List
	{
		public static List<T> Create<T>(params T[] items)
		{
			return new List<T>(items);
		}

		public static void Add<T>(this List<T> list, params T[] items)
		{
			list.Concat(items);
		}
	}

}
