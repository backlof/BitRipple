using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitRipple.Model;
using BitRipple.Utilities;
using BitRipple.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace UnitTests.Functionality
{
	public static class Extensions
	{
		public static Torrent ChangeGUID(this Torrent torrent, string guid)
		{
			torrent.GUID = guid;
			return torrent;
		}

		public static Filter Disable(this Filter filter)
		{
			filter.Enabled = false;
			return filter;
		}

		public static Filter DisableMatchOnce(this Filter filter)
		{
			filter.MatchOnce = false;
			return filter;
		}
	}

	[TestClass]
	public class FilterTests
	{
		public Torrent GetMovie()
		{
			return new Torrent()
			{
				Title = "Before I Wake 2016 720p ARABSUB WEBRip x264 YESMOVIES",
				GUID = "http://kat.cr/before-i-wake-2016-720p-arabsub-webrip-x264-yesmovies-t12712425.html",
				URL = "https://torcache.net/torrent/99B377131B248CA7C80AFF75621D70FBE716D4CE.torrent?title=[kat.cr]before.i.wake.2016.720p.arabsub.webrip.x264.yesmovies",
				Published = DateTime.Parse("Fri, 03 Jun 2016 21:57:59 +0000")
			};
		}

		public Torrent GetTvShow()
		{
			return GetTvShow(null, null);
		}

		public Torrent GetTvShow(int? season, int? episode)
		{
			var defaultOrSeason = season ?? 1;
			var defaultOrEpisode = episode ?? 2;

			return new Torrent()
			{
				Title = "Mum S0" + defaultOrSeason + "E0" + defaultOrEpisode + " HDTV x264-TLA[rartv]",
				GUID = "http://kat.cr/mum-s01e04-hdtv-x264-tla-rartv-t12712324.html",
				URL = "https://torcache.net/torrent/9C52CB86089EDAC816EF76785A1C671B60C2DC39.torrent?title=[kat.cr]mum.s01e04.hdtv.x264.tla.rartv",
				Published = DateTime.Parse("Fri, 03 Jun 2016 21:49:36 +0000")
			};
		}

		public Filter GetMovieFilter()
		{
			return new Filter()
			{
				Enabled = true,
				TorrentNameFilter = "Before I Wake",
				Include = "",
				Exclude = "",
				IgnoreCaps = false,
				MatchOnce = false
			};
		}

		public Filter GetTvFilter()
		{
			return new Filter()
			{
				Enabled = true,
				TorrentNameFilter = "Mum",
				Include = "",
				Exclude = "",
				IgnoreCaps = false,
				MatchOnce = true,
				IsTV = true,
				Season = 1,
				Episode = 2
			};
		}

		[TestMethod]
		public void GetEpisodeAndSeason()
		{
			Assert.AreEqual(GetTvShow().GetSeason(), 1);
			Assert.AreEqual(GetTvShow().GetEpisode(), 2);
		}

		[TestMethod]
		public void IsTvShow()
		{
			Assert.IsTrue(GetTvShow().IsTvShow());
			Assert.IsFalse(GetMovie().IsTvShow());
		}

		[TestMethod]
		public void HasDownloadedBefore()
		{
			List<Torrent> downloads = new List<Torrent>();
			Assert.IsFalse(downloads.HasDownloadedBefore(GetTvShow()));
			downloads.Add(GetTvShow());
			Assert.IsTrue(downloads.HasDownloadedBefore(GetTvShow()));
			Assert.IsFalse(downloads.HasDownloadedBefore(GetMovie()));
		}

		[TestMethod]
		public void EpisodeHasDownloadedBefore()
		{
			List<Torrent> downloads = new List<Torrent>();
			downloads.Add(GetTvShow(2, 2));

			Assert.IsTrue(downloads.EpisodeHasDownloadedBefore(GetTvShow(2, 2)));
			Assert.IsFalse(downloads.EpisodeHasDownloadedBefore(GetTvShow(2, 3)));
			Assert.IsFalse(downloads.EpisodeHasDownloadedBefore(GetTvShow(3, 2)));
			Assert.IsFalse(downloads.EpisodeHasDownloadedBefore(GetTvShow(1, 1)));
		}

		[TestMethod]
		public void ShouldDownloadEpisode()
		{
			List<Torrent> downloads = new List<Torrent>();
			downloads.Add(GetTvShow(2, 2));

			Filter filter = GetTvFilter();
			filter.MatchOnce = true;

			Assert.IsTrue(filter.ShouldDownloadEpisode(GetTvShow(2, 3), downloads));
			Assert.IsFalse(filter.ShouldDownloadEpisode(GetTvShow(2, 2), downloads));

			filter.MatchOnce = false;
			Assert.IsTrue(filter.ShouldDownloadEpisode(GetTvShow(2, 2), downloads));
		}

		[TestMethod]
		public void TitleMatching()
		{
			Filter pass = GetTvFilter();
			pass.Include = "x264";

			Assert.IsTrue(pass.TitleMatches(GetTvShow()));

			var failOnExclude = GetTvFilter();
			failOnExclude.Exclude = "HDTV";

			Assert.IsFalse(failOnExclude.TitleMatches(GetTvShow()));

			var failOnInclude = GetTvFilter();
			failOnInclude.Include = "1080p;720p";

			Assert.IsFalse(failOnInclude.TitleMatches(GetTvShow()));

			var ignoreCapsFail = GetTvFilter();
			ignoreCapsFail.TorrentNameFilter = "mom";

			Assert.IsFalse(ignoreCapsFail.TitleMatches(GetTvShow()));
		}

		[TestMethod]
		public void ShouldDownload()
		{
			List<Torrent> downloads = new List<Torrent>();
			downloads.Add(GetMovie());
			downloads.Add(GetTvShow());

			Assert.IsFalse(GetMovieFilter().ShouldDownload(GetMovie(), downloads));
			Assert.IsTrue(GetMovieFilter().ShouldDownload(GetMovie().ChangeGUID("123"), downloads));
			Assert.IsFalse(GetMovieFilter().Disable().ShouldDownload(GetMovie().ChangeGUID("123"), downloads));

			// Same torrent
			Assert.IsFalse(GetTvFilter().ShouldDownload(GetTvShow(), downloads));
			// Same episode
			Assert.IsFalse(GetTvFilter().ShouldDownload(GetTvShow(1, 2).ChangeGUID("123"), downloads));
			// Later episode
			Assert.IsTrue(GetTvFilter().ShouldDownload(GetTvShow(1, 3).ChangeGUID("123"), downloads));
			// Without match once
			Assert.IsTrue(GetTvFilter().DisableMatchOnce().ShouldDownload(GetTvShow().ChangeGUID("123"), downloads));
		}
	}
}
