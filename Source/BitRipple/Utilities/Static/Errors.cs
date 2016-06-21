using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using BitRipple.Model;
using System.IO;

namespace BitRipple.Utilities
{
	public static class Errors
	{
		public static void Print(string message, object sender, Exception e)
		{
			Print(message);
			Print(sender);
			Print(e);
		}

		public static void Print(string message, Exception e)
		{
			Print(message);
			Print(e);
		}

		public static void Print(string message)
		{
			Debug.WriteLine(message);
		}

		private static void Print(object sender)
		{
			Debug.WriteLine(sender.ToString());
		}

		public static void Print(Exception e)
		{
			Debug.WriteLine(e.ToString() + Environment.NewLine);
		}
	}

	public static class Log
	{
		public static void Download(Torrent download)
		{
			using (StreamWriter sw = File.AppendText(@"Matches.log"))
			{
				sw.WriteLine(String.Format("[{0}]", DateTime.Now.ToString("g")));
				sw.WriteLine(download.ToString() + Environment.NewLine);
			}
		}
	}
}
