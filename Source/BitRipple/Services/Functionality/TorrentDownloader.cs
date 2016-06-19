using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using BitRipple.Utilities;
using System.IO;
using System.IO.Compression;

namespace BitRipple.Services
{
	public static class TorrentDownloader
	{
		public static async Task<bool> DownloadTorrentFile(this Torrent torrent, string location)
		{
			try
			{
				byte[] result;
				byte[] buffer = new byte[4096];

				WebRequest wr = WebRequest.Create(torrent.URL);
				wr.ContentType = "application/x-bittorrent";
				wr.Timeout = ApplicationSettings.TORRENT_TIMEOUT_INTERVAL;

				if (!Directory.Exists(location))
				{
					Directory.CreateDirectory(location);
				}

				using (WebResponse response = await wr.GetResponseAsync())
				{
					bool gzip = response.Headers["Content-Encoding"] == "gzip";

					var responseStream = gzip ? new GZipStream(response.GetResponseStream(), CompressionMode.Decompress) : response.GetResponseStream();

					using (MemoryStream memoryStream = new MemoryStream())
					{
						int count = 0;
						do
						{
							count = responseStream.Read(buffer, 0, buffer.Length);
							memoryStream.Write(buffer, 0, count);
						}
						while (count != 0);

						result = memoryStream.ToArray();

						using (BinaryWriter writer = new BinaryWriter(new FileStream(location + Paths.CleanFileName(torrent.Title) + ".torrent", FileMode.Create)))
						{
							writer.Write(result);
						}
					}
				}

				return true;
			}
			catch (WebException we)
			{
				Errors.Print("Connection timed out while downloading torrent", torrent, we);
				return false;
			}
			catch (Exception e)
			{
				Errors.Print("Generic exception", torrent, e);
				return false;
			}
		}
	}
}
