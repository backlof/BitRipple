using BitRipple.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Model
{
	[DataContract]
	public class Feed : NotifyBase
	{
		public Feed() { }

		public Feed(Feed original)
		{
			URL = original.URL;
			Title = original.Title;
		}

		private string _URL;
		[DataMember(IsRequired = false)]
		public string URL
		{
			get
			{
				return _URL;
			}
			set
			{
				_URL = value.Trim();
				onPropertyChanged("URL");
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
				_Title = value.Trim();
				onPropertyChanged("Title");
			}
		}

		private ObservableCollection<Torrent> _Torrents;
		[IgnoreDataMember]
		public ObservableCollection<Torrent> Torrents
		{
			get
			{
				return _Torrents;
			}
			set
			{
				_Torrents = value;
				onPropertyChanged("Torrents");
			}
		}
	}
}
