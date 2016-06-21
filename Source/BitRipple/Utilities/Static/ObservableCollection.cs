using System.Collections.ObjectModel;
using System.Linq;

namespace BitRipple.Utilities.Static
{
	public static class ObservableCollection
	{
		public static ObservableCollection<T> Create<T>(params T[] items)
		{
			return new ObservableCollection<T>(items);
		}

		public static void Add<T>(this ObservableCollection<T> collection, params T[] items)
		{
			collection.Concat(items);
		}
	}
}
