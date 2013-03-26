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

namespace HangZhouBus.DB
{
    public class Api3
    {
        private const string UriString = "http://www.hzbus.cn/page/get3GPS.aspx";

        //Api参数：车辆官方id
        private int officeId;
        private int type;
        private string x;
        private string y;

        //回应的委托
        public delegate void ResponseDelegate(Object sender, ResonseEventArgs e);
        public event ResponseDelegate Response;

        public class ResonseEventArgs : EventArgs
        {
            public readonly string Message;

            public ResonseEventArgs(string message)
            {
                Message = message;
            }
        }

        protected virtual void OnResponse(string message)
        {
            if (Response != null)
            {
                Response(this, new ResonseEventArgs(message));
            }
        }

        /// <summary>
        /// Api3得到车还有多久到达
        /// </summary>
        /// <param name="officeId">车官方Id</param>
        /// <param name="type">上下行</param>
        /// <param name="x">纬度</param>
        /// <param name="y">经度</param>
        public Api3(int officeId, int type, string x, string y)
        {
            this.officeId = officeId;
            this.type = type;
            this.x = x;
            this.y = y;
        }

        public void Submit()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("fuid", officeId.ToString());
            dic.Add("type", type.ToString());
            dic.Add("x", x);
            dic.Add("y", y);

            HttpRequestHelper httpRequestHelper = new HttpRequestHelper(UriString, dic, HttpRequestHelper.RequestMethod.GET);
            httpRequestHelper.Response += new HttpRequestHelper.ResponseDelegate(httpRequestHelper_Response);
            httpRequestHelper.SendRequest();
        }

        void httpRequestHelper_Response(object sender, HttpRequestHelper.ResonseEventArgs e)
        {
            OnResponse(e.Response);
        }
    }
}
