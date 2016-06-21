using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitRipple.Model;
using BitRipple.Services;
using UnitTests.Utilities;

namespace UnitTests.Tests
{
	[TestClass]
	public class EqualityTests
	{
		[TestMethod]
		public void TwoIdenticalTorrentsShouldBeSameAs()
		{
			Assert.IsTrue(Builders.GetMovieTorrent().SameAs(Builders.GetMovieTorrent()));
		}

		[TestMethod]
		public void TwoIdenticalTorrentsShouldNotEqual()
		{
			Assert.AreNotEqual(Builders.GetMovieTorrent(), Builders.GetMovieTorrent());
		}
	}
}
