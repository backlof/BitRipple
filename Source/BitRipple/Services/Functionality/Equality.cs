using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Services
{
    public static class Equality
    {
        public static bool SameAs(this Torrent compare, Torrent to)
        {
            return compare.Title == to.Title && compare.GUID == to.GUID && compare.Published == to.Published && compare.URL == to.URL;
        }
    }
}
