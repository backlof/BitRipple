using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Services
{
    public class XmlDataService : IDataService
    {
        public static readonly string FEED_FILENAME = "Feeds.xml";
        public static readonly string FILTER_FILENAME = "Filters.xml";
        public static readonly string STATE_FILENAME = "State.xml";
        public static readonly string DOWNLOADS_FILENAME = "Downloads.xml";
        public static readonly string STATISTICS_FILENAME = "Statistics.xml";

        private IFileHandler FileHandler;
        private IDefaultService DefaultService;

        public XmlDataService(IFileHandler fileHandler, IDefaultService defaultService)
        {
            FileHandler = fileHandler;
            DefaultService = defaultService;
        }

        #region FEEDS

        public bool SaveFeeds(List<Feed> feeds)
        {
            return FileHandler.WriteSerializedXml(feeds, FEED_FILENAME);
        }

        public List<Feed> GetFeeds()
        {
            List<Feed> feeds = FileHandler.ReadSerializedXml(FEED_FILENAME, typeof(List<Feed>)) as List<Feed>;
            if (feeds == null)
            {
                feeds = DefaultService.GetDefaultFeeds();
                foreach (Feed feed in feeds)
                {
                    feed.Torrents = new System.Collections.ObjectModel.ObservableCollection<Torrent>();
                }
            }
            return feeds;
        }

        #endregion
        #region FILTERS

        public List<Filter> GetFilters()
        {
            List<Filter> filters = FileHandler.ReadSerializedXml(FILTER_FILENAME, typeof(List<Filter>)) as List<Filter>;
            if (filters == null)
            {
                filters = DefaultService.GetDefaultFilters();
            }
            return filters;
        }

        public bool SaveFilters(List<Filter> filters)
        {
            return FileHandler.WriteSerializedXml(filters, FILTER_FILENAME);
        }

        #endregion
        #region STATE

        public State GetState()
        {
            State state = FileHandler.ReadSerializedXml(STATE_FILENAME, typeof(State)) as State;
            if (state == null)
            {
                state = DefaultService.GetDefaultState();
            }
            return state;
        }

        public bool SaveState(State state)
        {
            return FileHandler.WriteSerializedXml(state, STATE_FILENAME);
        }

        #endregion
        #region TORRENTS

        public bool SaveDownloadedTorrents(List<Torrent> torrents)
        {
            return FileHandler.WriteSerializedXml(torrents, DOWNLOADS_FILENAME);
        }

        public List<Torrent> GetDownloadedTorrents()
        {
            List<Torrent> torrents = FileHandler.ReadSerializedXml(DOWNLOADS_FILENAME, typeof(List<Torrent>)) as List<Torrent>;
            if (torrents == null)
            {
                torrents = DefaultService.GetDefaultDownloadedTorrents();
            }
            return torrents;
        }

        #endregion
    }
}
