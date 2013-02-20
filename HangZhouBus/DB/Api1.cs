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
using System.Xml.Linq;

namespace HangZhouBus.DB
{
    public class Api1
    {
        private const string UriString = "http://www.hzbus.cn/Page/linestop.axd";

        //从xml中获取的属性
        private BusItem busItem;
        private List<StopItem> stopList;
        private LineItem lineItem;

        //Api参数:车辆ID、上下行
        private int id;
        private int type;

        //消息
        private bool success;
        private string message;

        //回应的委托
        public delegate void ResponseDelegate(Object sender, ResonseEventArgs e);
        public event ResponseDelegate Response;

        public class ResonseEventArgs : EventArgs
        {
            public readonly BusItem BusItem;
            public readonly List<StopItem> StopList;
            public readonly LineItem LineItem;
            public readonly bool Success;
            public readonly string Message;

            public ResonseEventArgs(BusItem busItem, List<StopItem> stopList, LineItem lineItem, bool success, string message)
            {
                BusItem = busItem;
                StopList = stopList;
                LineItem = lineItem;
                Success = success;
                Message = message;
            }
        }

        protected virtual void OnResponse(BusItem busItem, List<StopItem> stopList, LineItem lineItem, bool success, string message)
        {
            if (Response != null)
            {
                Response(this, new ResonseEventArgs(busItem, stopList, lineItem, success, message));
            }
        }

        /// <summary>
        /// Api1能得到：
        /// Bus表：ID、Name
        /// Stop表：ID、Name、X、Y
        /// Line表：Type
        /// </summary>
        /// <param name="id">车官方ID</param>
        /// <param name="type">上行：1；下行：2</param>
        public Api1(int id, int type)
        {
            this.id = id;
            this.type = type;
        }

        public void Submit()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("id", id.ToString());
            dic.Add("type", type.ToString());

            HttpRequestHelper httpRequestHelper = new HttpRequestHelper(UriString, dic, HttpRequestHelper.RequestMethod.GET);
            httpRequestHelper.Response += new HttpRequestHelper.ResponseDelegate(httpRequestHelper_Response);
            httpRequestHelper.SendRequest();
        }

        void httpRequestHelper_Response(object sender, HttpRequestHelper.ResonseEventArgs e)
        {
            //这个官方ID没有车
            if (e.Response == null || e.Response == "")
            {
                success = false;
                message = string.Format("{0}车{1}行没有车。", id, type);
            }
            else
            {
                XElement line = XElement.Parse(e.Response);

                busItem = ParseBus(line);
                stopList = ParseStop(line);
                lineItem = ParseLine();

                if (stopList == null)
                {
                    success = false;
                    message = string.Format("{0}车{1}行没有站。", id, type);
                }
                else
                {
                    success = true;
                    message = string.Format("{0}车{1}行获取成功。", id, type);
                }
            }

            OnResponse(busItem, stopList, lineItem, success, message);
        }

        private BusItem ParseBus(XElement line)
        {
            BusItem item = null;

            if (line.Attribute("name") != null)
            {
                item = new BusItem();
                item.OfficialId = id;
                item.Name = line.Attribute("name").Value;
            }

            return item;
        }

        private List<StopItem> ParseStop(XElement line)
        {
            List<StopItem> list = null;

            if (line.HasElements)
            {
                list = new List<StopItem>();

                foreach (XElement stop in line.Elements("Stop"))
                {
                    StopItem item = new StopItem();
                    item.OfficialId = stop.Attribute("uid").Value;
                    item.Name = stop.Attribute("name").Value;
                    item.X = stop.Attribute("x") != null ? stop.Attribute("x").Value : "";
                    item.Y = stop.Attribute("y") != null ? stop.Attribute("y").Value : "";

                    list.Add(item);
                }
            }

            return list;
        }

        private LineItem ParseLine()
        {
            LineItem item = new LineItem();
            item.Type = type;

            return item;
        }
    }
}
