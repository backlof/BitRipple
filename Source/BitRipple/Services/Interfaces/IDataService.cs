using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Services
{
    public interface IDataService
    {
        List<Feed> GetFeeds();
        bool SaveFeeds(List<Feed> feeds);

        List<Filter> GetFilters();
        bool SaveFilters(List<Filter> filters);

        State GetState();
        bool SaveState(State settings);

        List<Torrent> GetDownloadedTorrents();
        bool SaveDownloadedTorrents(List<Torrent> torrents);
    }
}
