using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Services
{
    public interface IFileHandler
    {
        Object ReadSerializedXml(string filename, Type type);
        bool WriteSerializedXml(Object obj, string filename);
    }
}
