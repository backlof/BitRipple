using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Services
{
    public interface IDefaultService
    {
        List<Feed> GetDefaultFeeds();
        Feed GetDefaultNewFeed();
        List<Filter> GetDefaultFilters();
        Filter GetDefaultNewFilter();
        State GetDefaultState();
        List<Torrent> GetDefaultDownloadedTorrents();
        MainWindowSettings GetDefaultMainWindow();
        WindowBase GetDefaultEditFeedWindow();
    }
}
