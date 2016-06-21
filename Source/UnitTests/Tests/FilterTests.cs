using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitRipple.Model;
using BitRipple.Utilities;
using BitRipple.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnitTests.Utilities;

namespace UnitTests.Tests
{

	[TestClass]
	public class FilterTests
	{
		[TestMethod]
		public void GetEpisodeAndSeason()
		{
			Assert.AreEqual(Builders.GetTvTorrent(1, 1).GetSeason(), 1);
			Assert.AreNotEqual(Builders.GetTvTorrent(1, 1).GetSeason(), 2);
			Assert.AreEqual(Builders.GetTvTorrent(3, 3).GetEpisode(), 3);
			Assert.AreNotEqual(Builders.GetTvTorrent(2, 2).GetEpisode(), 3);
		}

		[TestMethod]
		public void IsTvShow()
		{
			Assert.IsTrue(Builders.GetTvTorrent(1, 1).IsTvShow());
			Assert.IsFalse(Builders.GetMovieTorrent().IsTvShow());
		}

		[TestMethod]
		public void HasDownloadedBefore()
		{
			Assert.IsTrue(Builders.GetDownloadsWithTvTorrent().HasDownloadedBefore(Builders.GetTvTorrent()));
			Assert.IsFalse(Builders.GetDownloadsWithTvTorrent().HasDownloadedBefore(Builders.GetMovieTorrent()));
			Assert.IsFalse(Builders.GetEmptyDownloads().HasDownloadedBefore(Builders.GetTvTorrent()));
			Assert.IsFalse(Builders.GetDownloadsWithTvTorrent().HasDownloadedBefore(Builders.GetTvTorrent().GUID("123")));
		}

		[TestMethod]
		public void ShouldDownloadEpisode()
		{
			Assert.IsFalse(Builders.GetTvFilter().MatchOnce(true).ShouldDownloadEpisode(Builders.GetTvTorrent(), Builders.GetDownloadsWithTvTorrent()));
			Assert.IsTrue(Builders.GetTvFilter().MatchOnce(false).ShouldDownloadEpisode(Builders.GetTvTorrent(), Builders.GetDownloadsWithTvTorrent()));
			Assert.IsFalse(Builders.GetTvFilter(2, 3).MatchOnce(true).ShouldDownloadEpisode(Builders.GetTvTorrent(2, 2), Builders.GetDownloadsWithTvTorrent(2, 1)));
			Assert.IsTrue(Builders.GetTvFilter(2, 2).MatchOnce(true).ShouldDownloadEpisode(Builders.GetTvTorrent(2, 2), Builders.GetDownloadsWithTvTorrent(2, 1)));
			Assert.IsFalse(Builders.GetTvFilter(2, 2).MatchOnce(true).ShouldDownloadEpisode(Builders.GetTvTorrent(2, 1), Builders.GetDownloadsWithTvTorrent(2, 1)));
			Assert.IsFalse(Builders.GetTvFilter(2, 2).MatchOnce(true).ShouldDownloadEpisode(Builders.GetTvTorrent(2, 2), Builders.GetDownloadsWithTvTorrent(2, 2)));
		}

		[TestMethod]
		public void TitleMatching()
		{
			Assert.IsTrue(Builders.GetMovieFilter().TitleMatches(Builders.GetMovieTorrent()));
			Assert.IsFalse(Builders.GetMovieFilter().Exclude("720p").TitleMatches(Builders.GetMovieTorrent()));
			Assert.IsFalse(Builders.GetMovieFilter().Include("1080p;hdtv").TitleMatches(Builders.GetMovieTorrent()));
			Assert.IsFalse(Builders.GetMovieFilter().IgnoreCaps(false).TitleMatches(Builders.GetMovieTorrent()));
			Assert.IsFalse(Builders.GetMovieFilter().TorrentNameFilter("").TitleMatches(Builders.GetMovieTorrent()));
		}

		[TestMethod]
		public void ShouldDownload()
		{
			Assert.IsTrue(Builders.GetMovieFilter().Enabled(true).ShouldDownload(Builders.GetMovieTorrent(), Builders.GetEmptyDownloads()));
			// Not enabled
			Assert.IsFalse(Builders.GetMovieFilter().Enabled(false).ShouldDownload(Builders.GetMovieTorrent(), Builders.GetEmptyDownloads()));
			// Has been downloaded
			Assert.IsFalse(Builders.GetMovieFilter().Enabled(true).ShouldDownload(Builders.GetMovieTorrent(), Builders.GetDownloadsWithMovie()));
			// Not same torrent
			Assert.IsTrue(Builders.GetMovieFilter().Enabled(true).ShouldDownload(Builders.GetMovieTorrent().GUID("123"), Builders.GetDownloadsWithMovie()));
		}
	}
}
