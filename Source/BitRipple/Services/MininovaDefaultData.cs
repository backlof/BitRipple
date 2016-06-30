using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRipple.Model;
using BitRipple.Utilities;

namespace BitRipple.Services
{
	public class MininovaDefaultData : IDefaultService
	{
		public List<Torrent> GetDefaultDownloadedTorrents()
		{
			return new List<Torrent>();
		}

		public WindowBase GetDefaultEditFeedWindow()
		{
			double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
			double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
			double height = 250;
			double width = 240;

			return new WindowBase()
			{
				Height = height,
				Width = width,
				Left = (screenWidth / 2) - (width / 2),
				Top = (screenHeight / 2) - (height / 2),
				Resizable = System.Windows.ResizeMode.CanResize,
				State = System.Windows.WindowState.Normal
			};
		}

		public List<Feed> GetDefaultFeeds()
		{
			return new List<Feed>()
			{
				new Feed() { Title = "Movies", URL = "http://www.mininova.org/rss.xml?cat=4"},
				new Feed() { Title = "Movies - Action", URL = "http://www.mininova.org/rss.xml?sub=1" },
				new Feed() { Title = "Games", URL = "http://www.mininova.org/rss.xml?cat=3" },
				new Feed() { Title = "Books", URL = "http://www.mininova.org/rss.xml?cat=2" },
				new Feed() { Title = "Music", URL = "http://www.mininova.org/rss.xml?cat=5" },
				new Feed() { Title = "Tv", URL = "http://www.mininova.org/rss.xml?cat=8" }
			};
		}

		public List<Filter> GetDefaultFilters()
		{
			return new List<Filter>() {
				new Filter()
				{
					Title = "HD filter",
					Enabled = false,
					Include = "1080p",
					Exclude = "SD;480p;720p",
					FeedIndex = -1,
					MatchOnce = true,
					IgnoreCaps = true,
					TorrentNameFilter = "",
					IsTV = false,
					Episode = 1,
					Season = 1
				},
				new Filter()
				{
					Title = "New filter",
					Enabled = false,
					Include = "",
					Exclude = "",
					FeedIndex = -1,
					MatchOnce = true,
					IgnoreCaps = true,
					TorrentNameFilter = "",
					IsTV = false,
					Episode = 1,
					Season = 1
				}
			};
		}

		public MainWindowSettings GetDefaultMainWindow()
		{
			double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
			double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
			double height = 563;
			double width = 550;

			return new MainWindowSettings()
			{
				SaveBand = 0,
				SaveBandIndex = 0,
				BrowseBand = 0,
				BrowseBandIndex = 1,
				IntervalBand = 0,
				IntervalBandIndex = 2,
				UpdateBand = 0,
				UpdateBandIndex = 3,
				FilterSplitterPosition = 100,
				FeedSplitterPosition = 100,
				FeedWidthTitle = 250,
				FeedWidthPublished = 100,
				FilterWidthTitle = 250,
				FilterWidthPublished = 100,
				FilterWidthDownloaded = 100,
				Height = height,
				Width = width,
				Left = (screenWidth / 2) - (width / 2),
				Top = (screenHeight / 2) - (height / 2),
				Resizable = System.Windows.ResizeMode.CanResize,
				State = System.Windows.WindowState.Normal
			};
		}

		public Feed GetDefaultNewFeed()
		{
			return new Feed()
			{
				Title = "New feed",
				URL = ""
			};
		}

		public Filter GetDefaultNewFilter()
		{
			return new Filter()
			{
				Title = "New filter",
				Enabled = false,
				Include = "",
				Exclude = "",
				FeedIndex = -1,
				MatchOnce = true,
				IgnoreCaps = true,
				TorrentNameFilter = "",
				IsTV = false,
				Episode = 1,
				Season = 1
			};
		}

		public State GetDefaultState()
		{
			return new State()
			{
				CountUpdates = 0,
				LastUpdate = null,
				SelectedUpdateFrequency = 4,
				TorrentDropDirectory = ApplicationSettings.DEFAULT_TORRENT_DROP_FOLDER,
				SelectedFeedIndex = -1,
				SelectedFilterIndex = -1
			};
		}
	}
}
