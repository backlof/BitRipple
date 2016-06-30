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
	public class State : NotifyBase
	{
		private int _SelectedFilterIndex;
		[DataMember]
		public int SelectedFilterIndex
		{
			get
			{
				return _SelectedFilterIndex;
			}
			set
			{
				_SelectedFilterIndex = value;
				onPropertyChanged("SelectedFilterIndex");
			}
		}


		private int _SelectedFeedIndex;
		[DataMember]
		public int SelectedFeedIndex
		{
			get
			{
				return _SelectedFeedIndex;
			}
			set
			{
				_SelectedFeedIndex = value;
				onPropertyChanged("SelectedFeedIndex");
			}
		}

		private string _TorrentDropDirectory;
		[DataMember]
		public string TorrentDropDirectory
		{
			get
			{
				return _TorrentDropDirectory;
			}
			set
			{
				_TorrentDropDirectory = value;
				onPropertyChanged("TorrentDropDirectory");
			}
		}

		private int _SelectedUpdateFrequency;
		[DataMember]
		public int SelectedUpdateFrequency
		{
			get
			{
				return _SelectedUpdateFrequency;
			}
			set
			{
				_SelectedUpdateFrequency = value;
				onPropertyChanged("SelectedUpdateOption");
			}
		}


		private DateTime? _LastUpdate;
		[DataMember]
		public DateTime? LastUpdate
		{
			get
			{
				return _LastUpdate;
			}
			set
			{
				_LastUpdate = value;
				onPropertyChanged("LastUpdate");
			}
		}

		private int _CountUpdates;
		[DataMember]
		public int CountUpdates
		{
			get
			{
				return _CountUpdates;
			}
			set
			{
				_CountUpdates = value;
				onPropertyChanged("CountUpdates");
			}
		}
	}
}
