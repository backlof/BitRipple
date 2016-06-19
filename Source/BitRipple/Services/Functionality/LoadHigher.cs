using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Services
{
	public static class LoadHigher
	{
		public static void LoadHighestEpisode(this Filter filter, List<Torrent> downloadsByFilter)
		{
			if (downloadsByFilter.Count > 0)
			{
				Torrent highestEpisodeYet = downloadsByFilter.FindHighestEpisode();
				filter.Season = highestEpisodeYet.GetSeason();
				filter.Episode = highestEpisodeYet.GetEpisode() + 1;
			}
		}

		public static bool HasHigher(this Filter filter, List<Torrent> torrents)
		{
			Torrent highest = torrents.FindHighestEpisode();
			if (!highest.IsTvShow())
			{
				return false;
			}

			return highest.GetSeason() > filter.Season || (highest.GetSeason() == filter.Season && highest.GetEpisode() + 1 > filter.Episode);
		}

		public static Torrent FindHighestEpisode(this List<Torrent> torrents)
		{
			Torrent highestEpisodeYet = torrents[0];

			foreach (Torrent downloaded in torrents)
			{
				if (downloaded.IsTvShow())
				{
					if (downloaded.IsHigherThan(highestEpisodeYet))
					{
						highestEpisodeYet = downloaded;
					}
				}
			}

			return highestEpisodeYet;
		}

		public static bool IsHigherThan(this Torrent torrent, Torrent compareTo)
		{
			return torrent.GetSeason() > compareTo.GetSeason() || (torrent.GetSeason() == compareTo.GetSeason() && torrent.GetEpisode() > compareTo.GetEpisode());
		}
	}
}
