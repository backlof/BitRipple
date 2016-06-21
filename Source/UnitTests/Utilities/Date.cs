using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Utilities
{
	public static class Date
	{
		public static DateTime Today()
		{
			return DateTime.Today;
		}

		public static DateTime Yesterday()
		{
			return DateTime.Today.AddDays(-1);
		}

		public static DateTime DayBefore(this DateTime date)
		{
			return date.AddDays(-1);
		}
	}
}
