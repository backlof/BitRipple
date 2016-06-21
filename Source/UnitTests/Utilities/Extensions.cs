using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Utilities
{
	public static class Extensions
	{
		#region Torrent

		public static Torrent GUID(this Torrent torrent, string guid)
		{
			torrent.GUID = guid;
			return torrent;
		}

		#endregion

		#region Filter

		public static Filter Enabled(this Filter filter, bool enabled)
		{
			filter.Enabled = enabled;
			return filter;
		}

		public static Filter MatchOnce(this Filter filter, bool matchOnce)
		{
			filter.MatchOnce = matchOnce;
			return filter;
		}

		public static Filter Include(this Filter filter, string include)
		{
			filter.Include = include;
			return filter;
		}

		public static Filter Exclude(this Filter filter, string exclude)
		{
			filter.Exclude = exclude;
			return filter;
		}

		public static Filter IgnoreCaps(this Filter filter, bool ignoreCaps)
		{
			filter.IgnoreCaps = ignoreCaps;
			return filter;
		}

		public static Filter TorrentNameFilter(this Filter filter, string torrentNameFilter)
		{
			filter.TorrentNameFilter = torrentNameFilter;
			return filter;
		}

		#endregion
	}
}
