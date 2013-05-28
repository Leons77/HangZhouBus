using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using HangZhouBus.DB;
using System.ComponentModel;

namespace HangZhouBus
{
    public partial class BusLinePage : BasePage
    {
        private int id;
        private BusDataContext db;
        private BusItem busItem;
        private List<ShowItem> showList;

        public class ShowItem : INotifyPropertyChanged
        {
            public int Id { get; set; }
            public int Type { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string LastStop { get; set; }
            public string[] StopId { get; set; }
            public List<StopItem> StopList { get; set; }
            private bool isVisible;
            public bool IsVisible
            {
                get
                {
                    return isVisible;
                }
                set
                {
                    isVisible = value;
                    RaisePropertyChanged("IsVisible");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        public BusLinePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!IsInit)
            {
                Init();
            }
        }

        protected override void Init()
        {
            base.Init();

            id = Convert.ToInt32(NavigationContext.QueryString["Id"]);
            db = new BusDataContext();
            busItem = GetBusItem(id);

            tbkName.Text = busItem.Name;
            tbkPrice.Text = busItem.Price;

            var list = from line in busItem.LineItems.ToList()
                       select new ShowItem()
                       {
                           Id = line.Id,
                           Type = line.Type,
                           StartTime = line.StartTime,
                           EndTime = line.EndTime,
                           StopId = line.StopId.Split(';'),
                           StopList = new List<StopItem>(),
                           IsVisible = false
                       };

            showList = new List<ShowItem>(list);

            foreach (ShowItem item in showList)
            {
                var unSortStopList = from stop in db.StopTable.ToList()
                                     where item.StopId.Contains(stop.Id.ToString())
                                     select stop;

                foreach (string s in item.StopId)
                {
                    StopItem stopItem = unSortStopList.FirstOrDefault((i) => i.Id.ToString() == s);

                    if (stopItem != null)
                    {
                        item.StopList.Add(stopItem);
                    }
                }

                item.LastStop = item.StopList.LastOrDefault().Name;
            }

            listBox.ItemsSource = showList;
        }

        private BusItem GetBusItem(int id)
        {
            var busList = from bus in db.BusTable.ToList()
                          where bus.Id == id
                          select bus;

            return busList.FirstOrDefault();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowItem item = listBox.SelectedItem as ShowItem;

            if (item != null)
            {
                item.IsVisible = !item.IsVisible;
                listBox.SelectedIndex = -1;
            }
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            StopItem item = listBox.SelectedItem as StopItem;

            if (item != null)
            {
                NavigationService.Navigate(new Uri(string.Format("/GpsPage.xaml?OfficeId={0}&Type={1}&X={2}&Y={3}", busItem.OfficialId, listBox.Tag, item.X, item.Y), UriKind.Relative));
                listBox.SelectedIndex = -1;
            }
        }
    }
}