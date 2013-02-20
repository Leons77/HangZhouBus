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
using System.Collections.Generic;
using WindowsPhoneUtil;
using System.Text.RegularExpressions;
using System.Linq;

namespace HangZhouBus.DB
{
    public class Api2
    {
        private const string UriString = "http://www.hzbus.cn/Page/LineSearch.aspx";

        private int id;

        //从xml中获取的属性
        private string price;
        private string upStartTime;
        private string upEndTime;
        private string upLong;
        private string downStartTime;
        private string downEndTime;
        private string downLong;

        //Api参数:车辆名称
        private string name;

        //回应的委托
        public delegate void ResponseDelegate(Object sender, ResonseEventArgs e);
        public event ResponseDelegate Response;

        public class ResonseEventArgs : EventArgs
        {
            public readonly int Id;
            public readonly string Price;
            public readonly string UpStartTime;
            public readonly string UpEndTime;
            public readonly string UpLong;
            public readonly string DownStartTime;
            public readonly string DownEndTime;
            public readonly string DownLong;

            public ResonseEventArgs(int id, string price, string upStartTime, string upEndTime, string upLong, string downStartTime, string downEndTime, string downLong)
            {
                Id = id;
                Price = price;
                UpStartTime = upStartTime;
                UpEndTime = upEndTime;
                UpLong = upLong;
                DownStartTime = downStartTime;
                DownEndTime = downStartTime;
                DownLong = downLong;
            }
        }

        protected virtual void OnResponse(int id, string price, string upStartTime, string upEndTime, string upLong, string downStartTime, string downEndTime, string downLong)
        {
            if (Response != null)
            {
                Response(this, new ResonseEventArgs(id, price, upStartTime, upEndTime, upLong, downStartTime, downEndTime, downLong));
            }
        }

        /// <summary>
        /// Api2能得到：
        /// Bus表：Price
        /// Line表：StartTime、EndTime、Long
        /// </summary>
        /// <param name="name"></param>
        public Api2(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public void Submit()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("flinename", name);

            HttpRequestHelper httpRequestHelper = new HttpRequestHelper(UriString, dic, HttpRequestHelper.RequestMethod.GET);
            httpRequestHelper.Response += new HttpRequestHelper.ResponseDelegate(httpRequestHelper_Response);
            httpRequestHelper.SendRequest();
        }

        void httpRequestHelper_Response(object sender, HttpRequestHelper.ResonseEventArgs e)
        {
            //去除所有标签
            string s = Regex.Replace(e.Response, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase).Replace("\r", "").Replace("\n", "").Replace(" ", "");

            price = Regex.Match(s, @"普通车票价:\d+\.\d\d元").Value;
            if (price != "") price += " ";
            price += Regex.Match(s, @"空调车票价:\d+\.\d\d元").Value;

            string up = Regex.Match(s, @"(上行|环形)\(.+").Value;

            upStartTime = Regex.Match(up, @"(?<=首班时间:)\d?\d:\d\d").Value;
            upEndTime = Regex.Match(up, @"(?<=末班时间:)\d?\d:\d\d").Value;
            upLong = Regex.Match(up, @"(?<=线路长度:).+?米").Value;

            string down = Regex.Match(s, @"下行\(.+").Value;

            downStartTime = Regex.Match(down, @"(?<=首班时间:)\d?\d:\d\d").Value;
            downEndTime = Regex.Match(down, @"(?<=末班时间:)\d?\d:\d\d").Value;
            downLong = Regex.Match(down, @"(?<=线路长度:).+?米").Value;

            OnResponse(id, price, upStartTime, upEndTime, upLong, downStartTime, downEndTime, downLong);
        }
    }
}
