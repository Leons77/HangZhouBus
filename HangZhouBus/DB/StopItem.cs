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
using Microsoft.Phone.Data.Linq.Mapping;

namespace HangZhouBus.DB
{
    [Table]
    public class StopItem : BaseItem
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

        private string _officialId;
        /// <summary>
        /// 站官方ID
        /// </summary>
        [Column]
        public string OfficialId
        {
            get
            {
                return _officialId;
            }
            set
            {
                RaisePropertyChanging("OfficialId");
                _officialId = value;
                RaisePropertyChanged("OfficialId");
            }
        }

        private string _name;
        /// <summary>
        /// 站名
        /// </summary>
        [Column]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                RaisePropertyChanging("Name");
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string _x;
        /// <summary>
        /// 纬度
        /// </summary>
        [Column]
        public string X
        {
            get
            {
                return _x;
            }
            set
            {
                RaisePropertyChanging("X");
                _x = value;
                RaisePropertyChanged("X");
            }
        }

        private string _y;
        /// <summary>
        /// 经度
        /// </summary>
        [Column]
        public string Y
        {
            get
            {
                return _y;
            }
            set
            {
                RaisePropertyChanging("Y");
                _y = value;
                RaisePropertyChanged("Y");
            }
        }
    }
}
