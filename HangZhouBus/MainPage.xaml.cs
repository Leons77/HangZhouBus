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
using System.Collections.ObjectModel;
using System.Threading;

namespace HangZhouBus
{
    public partial class MainPage : BasePage
    {
        private BusDataContext db = new BusDataContext();
        private object o = new object();

        // 构造函数
        public MainPage()
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

            GetBusList(this);

            //NavigationService.Navigate(new Uri("/DBBuildPage.xaml", UriKind.Relative));
        }

        private void GetBusList(object state)
        {
            lock (o)
            {
                var list = from bus in db.BusTable.ToList()
                           where bus.Name.IndexOf(textBox.Text, StringComparison.OrdinalIgnoreCase) != -1
                           select bus;

                Dispatcher.BeginInvoke(() =>
                {
                    listBox.ItemsSource = list;
                });
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(GetBusList);
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                listBox.Focus();
            }
        }

        private void listBox_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            //获得当前的焦点对象
            TextBox focus = FocusManager.GetFocusedElement() as TextBox;
            if (focus != null)
            {
                ListBox listBox = sender as ListBox;
                listBox.Focus();
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            BusItem item = listBox.SelectedItem as BusItem;

            if (item != null)
            {
                NavigationService.Navigate(new Uri("/BusLinePage.xaml?Id=" + item.Id, UriKind.Relative));
                listBox.SelectedIndex = -1;
            }
        }
    }
}