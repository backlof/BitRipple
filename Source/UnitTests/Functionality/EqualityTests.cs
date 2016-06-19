using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitRipple.Model;
using BitRipple.Services;

namespace UnitTests.Functionality
{
	[TestClass]
	public class EqualityTests
	{
		private Torrent GetTorrent()
		{
			return new Torrent()
			{
				GUID = "123",
				Published = DateTime.Today,
				URL = "url"
			};
		}

		[TestMethod]
		public void TwoIdenticalTorrentsShouldBeSameAs()
		{
			Assert.IsTrue(GetTorrent().SameAs(GetTorrent()));
		}

		[TestMethod]
		public void TwoIdenticalTorrentsShouldNotEqual()
		{
			Assert.AreNotEqual(GetTorrent(), GetTorrent());
		}
	}
}
