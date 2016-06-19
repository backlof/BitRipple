using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Utilities
{
	public static class ApplicationSettings
	{
		public static readonly char[] INCLUDE_EXCLUDE_SEPARATORS = { ';', '|', '+' };
		public static readonly string TORRENT_NAME_FILTER_REJECT = @"$^{[(|)]}+\";

		public static readonly int[] UPDATE_FREQUENCY_IN_MINUTES = { 1, 2, 3, 5, 15, 30, 60 };
		public static readonly string[] UPDATE_FREQUENCY_OPTIONS = { "1 minute", "2 minutes", "3 minutes", "5 minutes", "15 minutes", "30 minutes", "1 hour" };

		public static readonly string DEFAULT_TORRENT_DROP_FOLDER = @"Torrents\";

		public static readonly int FEED_TIMEOUT_INTERVAL = 5000;
		public static readonly int TORRENT_TIMEOUT_INTERVAL = 10000;
	}
}
