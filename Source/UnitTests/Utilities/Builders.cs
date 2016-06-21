using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Utilities
{
	public static class Builders
	{
		public static readonly int DEFAULT_SEASON = 1;
		public static readonly int DEFAULT_EPISODE = 1;

		public static Torrent GetTorrentWithTitle(string title)
		{
			return new Torrent() { Title = title };
		}

		public static Torrent GetTvTorrent(int season, int episode)
		{
			return new Torrent() { Title = String.Format("Mum S{0}E{1} HDTV x264-TLA[rartv]", season, episode) };
		}

		public static Torrent GetTvTorrent()
		{
			return GetTvTorrent(DEFAULT_SEASON, DEFAULT_EPISODE);
		}

		public static Torrent GetMovieTorrent()
		{
			return new Torrent() { Title = "Before I Wake 2016 720p ARABSUB WEBRip x264 YESMOVIES" };
		}

		public static List<Torrent> GetDownloadsWithTvTorrent()
		{
			return new List<Torrent>() { GetTvTorrent() };
		}

		public static List<Torrent> GetDownloadsWithTvTorrent(int season, int episode)
		{
			return new List<Torrent>() { GetTvTorrent(season, episode) };
		}

		public static List<Torrent> GetEmptyDownloads()
		{
			return new List<Torrent>();
		}

		public static List<Torrent> GetDownloadsWith(params Torrent[] items)
		{
			return new List<Torrent>(items);
		}

		public static Filter GetTvFilter()
		{
			return new Filter()
			{
				Season = DEFAULT_SEASON,
				Episode = DEFAULT_EPISODE
			};
		}

		public static Filter GetTvFilter(int season, int episode)
		{
			return new Filter()
			{
				Season = season,
				Episode = episode
			};
		}

		public static Filter GetMovieFilter()
		{
			return new Filter()
			{
				IgnoreCaps = true,
				Include = "720p;web;x264",
				Exclude = "1080p;hdtv",
				TorrentNameFilter = "Before.I.Wake"
			};
		}

		public static List<Torrent> GetDownloadsWithMovie()
		{
			return new List<Torrent>() { GetMovieTorrent() };
		}
	}
}
