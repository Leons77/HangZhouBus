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
        //官方车ID数
        private int count = 700;

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
            progressBar.Visibility = Visibility.Visible;
            progressBar.Maximum = count * 2;
            progressBar.Value = 0;
            textBlock.Text = "0/" + (int)progressBar.Maximum;

            DBBuild dbBuild = DBBuild.GetInstance();
            dbBuild.Message += new DBBuild.MessageDelegate(dbBuild_Message);
            dbBuild.Build(count);
        }

        void dbBuild_Message(object sender, DBBuild.MessageEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                listSource.Add(e.Message);
                progressBar.Value++;
                textBlock.Text = string.Format("{0}/{1}", (int)progressBar.Value, (int)progressBar.Maximum);

                if (progressBar.Value == progressBar.Maximum)
                {
                    button.IsEnabled = true;
                }
            });
        }
    }
}