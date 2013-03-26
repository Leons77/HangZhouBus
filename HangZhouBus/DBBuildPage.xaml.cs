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

namespace HangZhouBus
{
    public partial class DBBuildPage : PhoneApplicationPage
    {
        ObservableCollection<string> listSource = new ObservableCollection<string>();

        public DBBuildPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            listBox.ItemsSource = listSource;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;

            DBBuild dbBuild = DBBuild.GetInstance();
            dbBuild.Message += new DBBuild.MessageDelegate(dbBuild_Message);
            dbBuild.Build();
        }

        void dbBuild_Message(object sender, DBBuild.MessageEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                listSource.Add(e.Message);

                if (e.Message == "Done")
                {
                    button.IsEnabled = true;
                }
            });
        }
    }
}