using BitRipple.Model;
using BitRipple.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BitRipple.Services
{
    public static class RssFeedParser
    {
        public static async Task<bool> DownloadRssFeed(this Feed feed)
        {
            if (String.IsNullOrWhiteSpace(feed.URL)) { return false; }

            XmlDocument xmlDocument = await CreateXmlDocumentFromFeedUrl(feed.URL);

            if (xmlDocument == null)
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(feed.Title))
            {
                feed.Title = ParseXmlFeedForTitle(xmlDocument);
            }
            feed.Torrents = new ObservableCollection<Torrent>(ParseXmlFeedForTorrents(xmlDocument));

            return true;
        }

        static string RemoveInvalidXmlChars(this string text)
        {
            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        public static async Task<XmlDocument> CreateXmlDocumentFromFeedUrl(this string url)
        {
            try
            {
                WebRequest wr = WebRequest.Create(url);
                wr.Timeout = ApplicationSettings.FEED_TIMEOUT_INTERVAL;

                using (HttpWebResponse response = (HttpWebResponse)await wr.GetResponseAsync())
                {
                    XmlDocument xmlDocument = new XmlDocument();

                    //StreamReader reader = new StreamReader(response.GetResponseStream());
                    //string xmlString = reader.ReadToEnd();
                    //string valid = xmlString.RemoveInvalidXmlChars();
                    //xmlDocument.LoadXml(xmlString.RemoveInvalidXmlChars());

                    xmlDocument.Load(response.GetResponseStream());

                    return xmlDocument;
                }
            }
            catch (FileNotFoundException fnfe)
            {
                Errors.Print("RSS feed not found", url, fnfe);
                return null;
            }
            catch (NullReferenceException nre)
            {
                Errors.Print("Not able to parse RSS feed", url, nre);
                return null;
            }
            catch (XmlException xe)
            {
                Errors.Print("RSS feed is broken", url, xe);
                return null;
            }
            catch (WebException we)
            {
                Errors.Print("Connection timed out while loading feed", url, we);
                return null;
            }
            catch (Exception e)
            {
                Errors.Print("Generic exception", url, e);
                return null;
            }
        }

        public static List<Torrent> ParseXmlFeedForTorrents(XmlDocument xmlDocument)
        {
            XmlNodeList itemNodes = xmlDocument.SelectNodes("rss/channel/item");

            var torrents = new List<Torrent>();

            foreach (XmlNode itemNode in itemNodes)
            {
                Torrent torrent = new Torrent();
                torrent.Title = itemNode.SelectSingleNode("title").InnerText.Trim();
                torrent.GUID = itemNode.SelectSingleNode("guid").InnerText.Trim();
                torrent.Published = DateTime.Parse(itemNode.SelectSingleNode("pubDate").InnerText.Trim());
                torrent.URL = itemNode.SelectSingleNode("enclosure") != null
                    ? itemNode.SelectSingleNode("enclosure").Attributes["url"].InnerText.Trim()
                    : itemNode.SelectSingleNode("link").InnerText.Trim();

                torrents.Add(torrent);
            }

            return torrents;
        }

        public static string ParseXmlFeedForTitle(XmlDocument xmlDocument)
        {
            return xmlDocument.SelectSingleNode("rss/channel/title").InnerText.Trim();
        }
    }
}
