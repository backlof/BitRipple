using BitRipple.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Model
{
	[DataContract]
	public class Torrent : NotifyBase
	{
		public Torrent() { }

		public Torrent(Torrent original)
		{
			GUID = original.GUID;
			Title = original.Title;
			URL = original.URL;
			Published = original.Published;
		}

		private string _GUID;
		[DataMember(IsRequired = false)]
		public string GUID
		{
			get
			{
				return _GUID;
			}
			set
			{
				_GUID = value;
				onPropertyChanged("GUID");
			}
		}

		private string _Title;
		[DataMember]
		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title = value;
				onPropertyChanged("Title");
			}
		}

		private string _URL;
		[DataMember]
		public string URL
		{
			get
			{
				return _URL;
			}
			set
			{
				_URL = value;
				onPropertyChanged("URL");
			}
		}

		private DateTime _Published;
		[DataMember]
		public DateTime Published
		{
			get
			{
				return _Published;
			}
			set
			{
				_Published = value;
				onPropertyChanged("Published");
			}
		}

		private Download _Download;
		[DataMember(IsRequired = false)]
		public Download Download
		{
			get
			{
				return _Download;
			}
			set
			{
				_Download = value;
				onPropertyChanged("Download");
			}
		}
	}

	[DataContract]
	public class Download : NotifyBase
	{
		private int _CaughtByFilter;
		[DataMember]
		public int CaughtByFilter
		{
			get
			{

				return _CaughtByFilter;
			}
			set
			{
				_CaughtByFilter = value;
				onPropertyChanged("CaughtByFilter");
			}
		}

		private DateTime _TimeOfDownload;
		[DataMember]
		public DateTime TimeOfDownload
		{
			get
			{
				return _TimeOfDownload;
			}
			set
			{
				_TimeOfDownload = value;
				onPropertyChanged("TimeOfDownload");
			}
		}

		private string _Location;
		[DataMember]
		public string Location
		{
			get
			{
				return _Location;
			}
			set
			{
				_Location = value;
				onPropertyChanged("Location");
			}
		}
	}
}
