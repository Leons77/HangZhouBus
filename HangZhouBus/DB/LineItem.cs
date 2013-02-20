using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace HangZhouBus.DB
{
    [Table]
    public class LineItem : BaseItem
    {
        private int _id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                RaisePropertyChanging("Id");
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        /// <summary>
        /// 车ID
        /// </summary>
        [Column]
        internal int _busId;
        private EntityRef<BusItem> _busItem;
        [Association(IsForeignKey = true, Storage="_busItem", ThisKey="_busId", OtherKey="Id")]
        public BusItem BusItem
        {
            get
            {
                return _busItem.Entity;
            }
            set
            {
                RaisePropertyChanging("BusItem");
                _busItem.Entity = value;

                if (value != null)
                {
                    _busId = value.Id; 
                }

                RaisePropertyChanged("BusItem");
            }
        }

        private int _type;
        /// <summary>
        /// 上行：1；下行：2
        /// </summary>
        [Column]
        public int Type
        {
            get
            {
                return _type;
            }
            set
            {
                RaisePropertyChanging("Type");
                _type = value;
                RaisePropertyChanged("Type");
            }
        }

        private string _stopId;
        /// <summary>
        /// 站ID，已“;”隔开
        /// </summary>
        [Column]
        public string StopId
        {
            get
            {
                return _stopId;
            }
            set
            {
                RaisePropertyChanging("StopId");
                _stopId = value;
                RaisePropertyChanged("StopId");
            }
        }

        private string _startTime;
        /// <summary>
        /// 始发时间
        /// </summary>
        [Column]
        public string StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                RaisePropertyChanging("StartTime");
                _startTime = value;
                RaisePropertyChanged("StartTime");
            }
        }

        private string _endTime;
        /// <summary>
        /// 末班时间
        /// </summary>
        [Column]
        public string EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                RaisePropertyChanging("EndTime");
                _endTime = value;
                RaisePropertyChanged("EndTime");
            }
        }

        private string _long;
        /// <summary>
        /// 线路长
        /// </summary>
        [Column]
        public string Long
        {
            get
            {
                return _long;
            }
            set
            {
                RaisePropertyChanging("Long");
                _long = value;
                RaisePropertyChanged("Long");
            }
        }
    }
}
