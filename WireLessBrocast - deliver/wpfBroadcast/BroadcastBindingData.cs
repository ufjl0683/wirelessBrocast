using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace wpfBroadcast
{
    public  class BroadcastBindingData:INotifyPropertyChanged

    {
        private bool _IsSelected, _IsSend, _IsSuccess,_CanEcho ;
        private string   _SITE_NAME;
        int _SITE_ID;
        private int _RepeatCnt;
        public bool IsSelected {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    if(PropertyChanged!=null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
       }
        public int SITE_ID
        {
            get
            {
                return _SITE_ID;
            }
            set
            {
                if (value != _SITE_ID)
                {
                    _SITE_ID = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("SITE_ID"));
                }
            }
        }
        public string SITE_NAME {
            get
            {
                return _SITE_NAME;
            }
            set
            {
                if (value != _SITE_NAME)
                {
                    _SITE_NAME = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("SITE_NAME"));
                }
            }
        }
        public int RepeatCnt {
            get
            {
                return _RepeatCnt;
            }
            set
            {
                if (value != _RepeatCnt)
                {
                    _RepeatCnt = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("RepeatCnt"));
                }
            }
        }
        public bool IsSend
        {
            get
            {
                return _IsSend;
            }
            set
            {
                if (value != _IsSend)
                {
                    _IsSend = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSend"));
                        
                    }
                  
                }
            }
        }
        public bool IsSuccess {
            get
            {
                return _IsSuccess;
            }
            set
            {
                if (value != _IsSuccess)
                {
                    _IsSuccess = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSuccess"));
                }
            }
        }

        bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                if (value != _IsBusy)
                {
                    _IsBusy = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
                    
                        CanEcho = IsBusy;
                }
            }

        }

        public bool CanEcho
        {
            get
            {
                return _CanEcho;
            }
            set
            {
                if (value != _CanEcho)
                {
                    _CanEcho = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("CanEcho"));
                }
            }

        }
       


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
