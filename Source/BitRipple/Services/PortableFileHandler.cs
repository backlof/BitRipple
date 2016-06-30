using BitRipple.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BitRipple.Services
{
    public class PortableFileHandler : IFileHandler
    {
        public string SaveFolder { get; set; }

        public PortableFileHandler(string saveFolder)
        {
            SaveFolder = saveFolder;
        }

        private string SavePath(string filename)
        {
            return Path.Combine(SaveFolder, filename);
        }

        public Object ReadSerializedXml(string filename, Type type)
        {
            string fullpath = SavePath(filename);

            if (File.Exists(fullpath))
            {
                using (FileStream fs = new FileStream(fullpath, FileMode.Open, FileAccess.Read))
                {
                    DataContractSerializer dcs = new DataContractSerializer(type);
                    return dcs.ReadObject(fs);
                }
            }
            else
            {
                return null;
            }
        }

        public bool WriteSerializedXml(object obj, string filename)
        {
            if (!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }

            try
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true };
                using (XmlWriter xmlWriter = XmlWriter.Create(SavePath(filename), xmlWriterSettings))
                {
                    DataContractSerializer dcs = new DataContractSerializer(obj.GetType());
                    dcs.WriteObject(xmlWriter, obj);
                }
                return true;
            }
            catch (Exception e)
            {
                Errors.Print(e);
                return false;
            }
        }
    }
}
