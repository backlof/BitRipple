using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Model
{
    [DataContract]
    public class MainWindowSettings : WindowBase
    {
        #region TOOLBAR
        private int _SaveBand;
        [DataMember]
        public int SaveBand
        {
            get
            {
                return _SaveBand;
            }
            set
            {
                _SaveBand = value;
                onPropertyChanged("SaveBand");
            }
        }

        private int _SaveBandIndex;
        [DataMember]
        public int SaveBandIndex
        {
            get
            {
                return _SaveBandIndex;
            }
            set
            {
                _SaveBandIndex = value;
                onPropertyChanged("SaveBandIndex");
            }
        }

        private int _BrowseBand;
        [DataMember]
        public int BrowseBand
        {
            get
            {
                return _BrowseBand;
            }
            set
            {
                _BrowseBand = value;
                onPropertyChanged("BrowseBand");
            }
        }

        private int _BrowseBandIndex;
        [DataMember]
        public int BrowseBandIndex
        {
            get
            {
                return _BrowseBandIndex;
            }
            set
            {
                _BrowseBandIndex = value;
                onPropertyChanged("BrowseBandIndex");
            }
        }

        private int _IntervalBand;
        [DataMember]
        public int IntervalBand
        {
            get
            {
                return _IntervalBand;
            }
            set
            {
                _IntervalBand = value;
                onPropertyChanged("IntervalBand");
            }
        }

        private int _IntervalBandIndex;
        [DataMember]
        public int IntervalBandIndex
        {
            get
            {
                return _IntervalBandIndex;
            }
            set
            {
                _IntervalBandIndex = value;
                onPropertyChanged("IntervalBandIndex");
            }
        }

        private int _UpdateBand;
        [DataMember]
        public int UpdateBand
        {
            get
            {
                return _UpdateBand;
            }
            set
            {
                _UpdateBand = value;
                onPropertyChanged("UpdateBand");
            }
        }

        private int _UpdateBandIndex;
        [DataMember]
        public int UpdateBandIndex
        {
            get
            {
                return _UpdateBandIndex;
            }
            set
            {
                _UpdateBandIndex = value;
                onPropertyChanged("UpdateBandIndex");
            }
        }

        #endregion

        #region GRIDSPLITTERS
        private double _FilterSplitterPosition;
        [DataMember]
        public double FilterSplitterPosition
        {
            get
            {
                return _FilterSplitterPosition;
            }
            set
            {
                _FilterSplitterPosition = value;
                onPropertyChanged("FilterSplitterPosition");
            }
        }

        private double _FeedSplitterPosition;
        [DataMember]
        public double FeedSplitterPosition
        {
            get
            {
                return _FeedSplitterPosition;
            }
            set
            {
                _FeedSplitterPosition = value;
                onPropertyChanged("FeedSplitterPosition");
            }
        }

        #endregion

        #region DATAGRID COLUMNS
        private double _FeedWidthTitle;
        [DataMember]
        public double FeedWidthTitle
        {
            get
            {
                return _FeedWidthTitle;
            }
            set
            {
                _FeedWidthTitle = value;
                onPropertyChanged("FeedTitleWidth");
            }
        }

        private double _FeedWidthPublished;
        [DataMember]
        public double FeedWidthPublished
        {
            get
            {
                return _FeedWidthPublished;
            }
            set
            {
                _FeedWidthPublished = value;
                onPropertyChanged("FeedPublishedWidth");
            }
        }

        private double _FilterWidthTitle;
        [DataMember]
        public double FilterWidthTitle
        {
            get
            {
                return _FilterWidthTitle;
            }
            set
            {
                _FilterWidthTitle = value;
                onPropertyChanged("FilterWidthTitle");
            }
        }

        private double _FilterWidthPublished;
        [DataMember]
        public double FilterWidthPublished
        {
            get
            {
                return _FilterWidthPublished;
            }
            set
            {
                _FilterWidthPublished = value;
                onPropertyChanged("FilterWidthPublished");
            }
        }

        private double _FilterWidthDownloaded;
        [DataMember]
        public double FilterWidthDownloaded
        {
            get
            {
                return _FilterWidthDownloaded;
            }
            set
            {
                _FilterWidthDownloaded = value;
                onPropertyChanged("FilterWidthDownloaded");
            }

        }
        #endregion
    }
}
