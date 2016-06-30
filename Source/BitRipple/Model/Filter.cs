using BitRipple.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Model
{
	public class FilterWithIndex
	{
		public Filter Filter { get; set; }
		public int Index { get; set; }
	}

	[DataContract]
	public class Filter : NotifyBase
	{
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

		private bool _Enabled;
		[DataMember]
		public bool Enabled
		{
			get
			{
				return _Enabled;
			}
			set
			{
				_Enabled = value;
				onPropertyChanged("Enabled");
			}
		}

		private bool _IgnoreCaps;
		[DataMember]
		public bool IgnoreCaps
		{
			get
			{
				return _IgnoreCaps;
			}
			set
			{
				_IgnoreCaps = value;
				onPropertyChanged("IgnoreCaps");
			}
		}

		private bool _MatchOnce;
		[DataMember]
		public bool MatchOnce
		{
			get
			{
				return _MatchOnce;
			}
			set
			{
				_MatchOnce = value;
				onPropertyChanged("MatchOnce");
			}
		}

		private Feed _Feed;
		[IgnoreDataMember]
		public Feed Feed
		{
			get
			{
				return _Feed;
			}
			set
			{
				_Feed = value;
				onPropertyChanged("Feed");
			}
		}


		private int _FeedIndex;
		[DataMember]
		public int FeedIndex
		{
			get
			{
				return _FeedIndex;
			}
			set
			{
				_FeedIndex = value;
				onPropertyChanged("FeedIndex");
			}
		}

		[IgnoreDataMember]
		public bool HasFeed
		{
			get
			{
				return _FeedIndex != -1;
			}
		}

		private bool _IsTV;
		[DataMember(Name = "TV")]
		public bool IsTV
		{
			get
			{
				return _IsTV;
			}
			set
			{
				_IsTV = value;
				onPropertyChanged("IsTV");
			}
		}

		private int _Season;
		[DataMember]
		public int Season
		{
			get
			{
				return _Season;
			}
			set
			{
				_Season = value;
				onPropertyChanged("Season");
			}
		}

		private int _Episode;
		[DataMember]
		public int Episode
		{
			get
			{
				return _Episode;
			}
			set
			{
				_Episode = value;
				onPropertyChanged("Episode");
			}
		}

		#region INCLUDE & EXCLUDE

		[IgnoreDataMember]
		public List<string> IncludeList
		{
			get
			{
				string include = IgnoreCaps ? Include.ToLower() : Include;
				return new List<string>(include.Split(ApplicationSettings.INCLUDE_EXCLUDE_SEPARATORS, StringSplitOptions.RemoveEmptyEntries));
			}
		}

		private string _Include;
		[DataMember(IsRequired = false)]
		public string Include
		{
			get
			{
				return _Include;
			}
			set
			{
				_Include = Strings.RemoveDiacritics(value.Trim());
				onPropertyChanged("Include");
			}
		}

		[IgnoreDataMember]
		public List<string> ExcludeList
		{
			get
			{
				string exclude = IgnoreCaps ? Exclude.ToLower() : Exclude;
				return new List<string>(exclude.Split(ApplicationSettings.INCLUDE_EXCLUDE_SEPARATORS, StringSplitOptions.RemoveEmptyEntries));
			}
		}

		private string _Exclude = "";
		[DataMember(IsRequired = false)]
		public string Exclude
		{
			get
			{
				return _Exclude;
			}
			set
			{
				_Exclude = Strings.RemoveDiacritics(value.Trim());
				onPropertyChanged("Exclude");
			}
		}

		#endregion

		#region TORRENT NAME FILTER

		private string _TorrentNameFilter = "";
		[DataMember(Name = "Filter", IsRequired = false)]
		public string TorrentNameFilter
		{
			get
			{
				return _TorrentNameFilter;
			}
			set
			{
				// Replace all Regex Operators - except the ones I translate
				_TorrentNameFilter = Strings.ReplaceCharacters(value.Trim(), ApplicationSettings.TORRENT_NAME_FILTER_REJECT, "");
				onPropertyChanged("TitleFilter");
				RegexPattern = TorrentNameFilter;
			}
		}

		private string _RegexPattern;
		[IgnoreDataMember]
		public string RegexPattern
		{
			get
			{
				return _RegexPattern;
			}
			set
			{
				StringBuilder sb = new StringBuilder();

				// * = Wildcard
				// . = Whitespaces
				// ? = Any character 0 or 1 times

				int length = value.Length;

				if (length > 0)
				{
					// No wildcard in beginning, then "Begins with"
					if (value[0] != '*')
					{
						sb.Append(@"^");
					}

					foreach (char letter in value)
					{
						if (letter == '*')
						{
							sb.Append(@".*");
						}
						else if (letter == '?')
						{
							sb.Append(".?");
						}
						else if (letter == '.')
						{
							sb.Append(@"[\s\.\-_]");
						}
						else
						{
							sb.Append(letter);
						}
					}
				}
				else
				{
					sb.Append(".^"); // No matches
				}

				_RegexPattern = @sb.ToString();
				onPropertyChanged("RegexPattern");
			}
		}

		#endregion
	}
}
