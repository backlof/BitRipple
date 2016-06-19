using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRipple.Utilities;
using System.Runtime.Serialization;
using System.Windows;

namespace BitRipple.Model
{
    [DataContract]
    public class WindowBase : NotifyBase
    {
        private ResizeMode _Resizable;
        [DataMember]
        public ResizeMode Resizable
        {
            get
            {
                return _Resizable;
            }
            set
            {
                _Resizable = value;
                onPropertyChanged("Resizable");
            }
        }

        private WindowState _State;
        [DataMember]
        public WindowState State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
                onPropertyChanged("State");
            }
        }

        private double _Width;
        [DataMember]
        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
                onPropertyChanged("Width");
            }
        }

        private double _Height;
        [DataMember]
        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
                onPropertyChanged("Height");
            }
        }

        private double _Top;
        [DataMember]
        public double Top
        {
            get
            {
                return _Top;
            }
            set
            {
                _Top = value;
                onPropertyChanged("Top");
            }
        }

        private double _Left;
        [DataMember]
        public double Left
        {
            get
            {
                return _Left;
            }
            set
            {
                _Left = value;
                onPropertyChanged("Left");
            }
        }
    }
}
