using BitRipple.Model;
using BitRipple.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace BitRipple.Services
{
	public class ApplicationService : NotifyBase
	{
		private IDataService DataService;

		public ObservableCollection<Filter> Filters { get; set; }
		public ObservableCollection<Feed> Feeds { get; set; }
		public ObservableCollection<Torrent> DownloadedTorrents { get; set; }
		public State State { get; set; }
		public FeedEdit CurrentEditedFeed { get; set; }

		public ApplicationService(IDataService dataService)
		{
			DataService = dataService;

			Feeds = new ObservableCollection<Feed>(DataService.GetFeeds());
			Filters = new ObservableCollection<Filter>(DataService.GetFilters());
			DownloadedTorrents = new ObservableCollection<Torrent>(DataService.GetDownloadedTorrents());
			State = DataService.GetState();

			if (!Directory.Exists(Path.GetDirectoryName(ApplicationSettings.DEFAULT_TORRENT_DROP_FOLDER)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(ApplicationSettings.DEFAULT_TORRENT_DROP_FOLDER));
			}
		}

		public void Save()
		{
			DataService.SaveFilters(Filters.ToList());
			DataService.SaveFeeds(Feeds.ToList());
			DataService.SaveDownloadedTorrents(DownloadedTorrents.ToList());
			DataService.SaveState(State);
		}

		public async Task<bool> Update()
		{
			List<Task<bool>> filterUpdates = new List<Task<bool>>();

			foreach (var feed in Feeds)
			{
				filterUpdates.Add(UpdateFeed(feed));
			}

			await Task.WhenAll(filterUpdates);
			bool updateResultedInOneOrMoreDownloads = filterUpdates.Any(x => x.Result);
			return updateResultedInOneOrMoreDownloads;
		}

		private async Task<bool> UpdateFeed(Feed feed)
		{
			List<Task<bool>> tasks = new List<Task<bool>>();

			if (await feed.DownloadRssFeed())
			{
				foreach (var filter in Filters.Where(x => x.Feed == feed))
				{
					if (filter.Enabled)
					{
						tasks.Add(FilterFeed(feed, filter));
					}
				}
			}

			await Task.WhenAll(tasks);
			bool feedUpdateResultedInOneOrMoreDownloads = tasks.Any(x => x.Result);
			return feedUpdateResultedInOneOrMoreDownloads;
		}

		public async Task<bool> FilterFeed(Feed feed, Filter filter)
		{
			int indexOfFilter = Filters.IndexOf(filter);
			List<Torrent> torrentsDownloadedByFilter = DownloadedTorrents.Where(x => x.Download.CaughtByFilter == indexOfFilter).ToList();
			List<Task<bool>> downloadTorrents = new List<Task<bool>>();

			foreach (Torrent torrent in feed.Torrents)
			{
				if (filter.ShouldDownload(torrent, torrentsDownloadedByFilter))
				{
					Torrent copy = new Torrent(torrent);
					copy.Download = new Download()
					{
						CaughtByFilter = indexOfFilter
					};

					downloadTorrents.Add(Download(copy));

					if (filter.DisableAfterFirstDownload())
					{
						break; // Break so that no more torrents are added to download list
					}

				}
			}

			await Task.WhenAll(downloadTorrents);
			bool filterDownloadedAtLeastOneTorrent = downloadTorrents.Any(x => x.Result);

			if (filterDownloadedAtLeastOneTorrent)
			{
				if (filter.DisableAfterFirstDownload())
				{
					filter.Enabled = false;
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> Download(Torrent torrent)
		{
			string location = State.TorrentDropDirectory;
			bool torrentSuccessfullyDownloaded = await torrent.DownloadTorrentFile(location);

			if (torrentSuccessfullyDownloaded)
			{
				torrent.Download.Location = location;
				torrent.Download.TimeOfDownload = DateTime.Now;
				DownloadedTorrents.Add(torrent);
				Log.Download(torrent);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}