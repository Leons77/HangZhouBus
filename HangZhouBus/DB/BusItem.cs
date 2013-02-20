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
using System.Data.Linq;

namespace HangZhouBus.DB
{
    [Table]
    [Index(Name = "NameIndex", Columns = "Name", IsUnique = true)]
    public class BusItem : BaseItem
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

        private int _officialId;
        /// <summary>
        /// 车官方ID
        /// </summary>
        [Column]
        public int OfficialId
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
        /// 多少路
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

        private string _price;
        /// <summary>
        /// 价钱
        /// </summary>
        [Column]
        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                RaisePropertyChanging("Price");
                _price = value;
                RaisePropertyChanged("Price");
            }
        }

        private EntitySet<LineItem> _lineItems;
        [Association(Storage = "_lineItems", ThisKey = "Id", OtherKey = "_busId")]
        public EntitySet<LineItem> LineItems
        {
            get
            {
                return _lineItems;
            }
            set
            {
                _lineItems.Assign(value);
            }
        }

        public BusItem()
        {
            _lineItems = new EntitySet<LineItem>(new Action<LineItem>(AttachToLineItem), new Action<LineItem>(DetachFromLineItem));
        }

        private void AttachToLineItem(LineItem lineItem)
        {
            RaisePropertyChanging("LineItems");
            lineItem.BusItem = this;
            RaisePropertyChanged("LineItems");
            
        }

        private void DetachFromLineItem(LineItem lineItem)
        {
            RaisePropertyChanging("LineItems");
            lineItem.BusItem = null;
            RaisePropertyChanged("LineItems");
        }
    }
}
