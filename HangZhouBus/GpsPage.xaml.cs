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

namespace HangZhouBus
{
    public partial class GpsPage : BasePage
    {
        private int officeId;
        private int type;
        private string x;
        private string y;

        public GpsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            officeId = Convert.ToInt32(NavigationContext.QueryString["OfficeId"]);
            type = Convert.ToInt32(NavigationContext.QueryString["Type"]);
            x = NavigationContext.QueryString["X"];
            y = NavigationContext.QueryString["Y"];

            Api3 api3 = new Api3(officeId, type, x, y);
            api3.Response += new Api3.ResponseDelegate(api3_Response);
            api3.Submit();
        }

        void api3_Response(object sender, Api3.ResonseEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                textBlock.Text = e.Message;
            });
        }
    }
}