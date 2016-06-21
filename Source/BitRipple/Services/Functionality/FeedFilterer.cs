using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRipple.Utilities;
using System.Text.RegularExpressions;

namespace BitRipple.Services
{
	public static class FeedFilterer
	{
		private static readonly string SEASON_MATCHNAME = "season";
		private static readonly string EPISODE_MATCHNAME = "episode";
		private static readonly Regex EPISODE_REGEX_FILTER = new Regex(@"S(?<" + SEASON_MATCHNAME + @">\d{1,3})E(?<" + EPISODE_MATCHNAME + @">\d{1,3})", RegexOptions.IgnoreCase);

		public static List<Torrent> FindDownloadedTorrents(this ObservableCollection<Torrent> downloads, int indexOfFilter)
		{
			return downloads.Where(x => x.Download.CaughtByFilter == indexOfFilter).ToList();
		}

		public static bool DisableAfterFirstDownload(this Filter filter)
		{
			return !filter.IsTV && filter.MatchOnce;
		}

		public static bool ShouldDownload(this Filter filter, Torrent torrent, List<Torrent> downloaded)
		{
			if (!filter.Enabled)
			{
				return false;
			}
			if (downloaded.HasDownloadedBefore(torrent))
			{
				return false;
			}
			if (!filter.TitleMatches(torrent))
			{
				return false;
			}
			if (filter.IsTV && !torrent.IsTvShow())
			{
				return false;
			}
			if (filter.IsTV && !filter.ShouldDownloadEpisode(torrent, downloaded))
			{
				return false;
			}

			return true;
		}

		public static bool TitleMatches(this Filter filter, Torrent torrent)
		{
			string title = (filter.IgnoreCaps ? torrent.Title.ToLower() : torrent.Title).RemoveDiacritics();
			RegexOptions option = filter.IgnoreCaps ? RegexOptions.IgnoreCase : RegexOptions.None;

			if (!Regex.IsMatch(title, filter.RegexPattern, option))
			{
				return false;
			}
			if (!filter.IncludeList.All(title.Contains))
			{
				return false;
			}
			if (filter.ExcludeList.Any(title.Contains))
			{
				return false;
			}

			return true;
		}

		public static bool ShouldDownloadEpisode(this Filter filter, Torrent torrent, List<Torrent> downloaded)
		{
			if (!(torrent.GetSeason() > filter.Season || (torrent.GetSeason() == filter.Season && torrent.GetEpisode() >= filter.Episode)))
			{
				return false;
			}
			if (filter.MatchOnce && downloaded.EpisodeHasDownloadedBefore(torrent))
			{
				return false;
			}

			return true;
		}

		public static bool EpisodeHasDownloadedBefore(this List<Torrent> downloaded, Torrent torrent)
		{
			return downloaded.Any(download => download.IsTvShow() && download.GetSeason() == torrent.GetSeason() && download.GetEpisode() == torrent.GetEpisode());
		}

		public static bool IsTvShow(this Torrent torrent)
		{
			var match = EPISODE_REGEX_FILTER.Match(torrent.Title);
			return match.Success;
		}

		public static int GetSeason(this Torrent torrent)
		{
			var match = EPISODE_REGEX_FILTER.Match(torrent.Title);
			return Int32.Parse(match.Groups[SEASON_MATCHNAME].Value);
		}

		public static int GetEpisode(this Torrent torrent)
		{
			var match = EPISODE_REGEX_FILTER.Match(torrent.Title);
			return Int32.Parse(match.Groups[EPISODE_MATCHNAME].Value);
		}

		public static bool HasDownloadedBefore(this List<Torrent> torrentsToCheckAgainst, Torrent torrent)
		{
			return torrentsToCheckAgainst.Any(y => torrent.SameAs(y));
		}
	}
}
