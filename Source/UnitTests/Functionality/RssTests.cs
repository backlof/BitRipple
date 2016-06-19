using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitRipple.Services;
using BitRipple.Model;
using System.Xml;
using System.Collections.Generic;
using System.Net;
using BitRipple.Utilities;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace UnitTests.Functionality
{
	public static class Statics
	{ }

	[TestClass]
	public class RssTests
	{
		[TestMethod]
		public void XmlFromFile()
		{
			List<Torrent> list = new List<Torrent>();

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load("Feed.xml");

			list = RssFeedParser.ParseXmlFeedForTorrents(xmlDocument);
		}

		[TestMethod]
		public async Task RssFeedsFromMininova()
		{
			MininovaDefaultData Defaults = new MininovaDefaultData();

			foreach (var feed in Defaults.GetDefaultFeeds())
			{
				Assert.IsNotNull(await feed.URL.CreateXmlDocumentFromFeedUrl());
			}
		}
	}
}
