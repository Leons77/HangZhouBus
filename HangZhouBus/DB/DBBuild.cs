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
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Linq;

namespace HangZhouBus.DB
{
    public class DBBuild
    {
        private const string Api2 = "http://www.hzbus.cn/Page/LineSearch.aspx";

        private BusDataContext db;

        private int i;
        private int j;
        private int count;

        private int k;
        private int n;

        //消息的委托
        public delegate void MessageDelegate(Object sender, MessageEventArgs e);
        public event MessageDelegate Message;

        public class MessageEventArgs : EventArgs
        {
            public readonly string Message;

            public MessageEventArgs(string message)
            {
                Message = message;
            }
        }

        protected virtual void OnMessage(string message)
        {
            if (Message != null)
            {
                Message(this, new MessageEventArgs(message));
            }
        }


        private static DBBuild dbBuild;

        public static DBBuild GetInstance()
        {
            if (dbBuild == null)
            {
                dbBuild = new DBBuild();
            }

            return dbBuild;
        }

        private DBBuild()
        {
            db = new BusDataContext();
        }

        public void Build(int count)
        {
            if (db.DatabaseExists())
            {
                db.DeleteDatabase();
            }

            db.CreateDatabase();

            i = 1;
            j = 1;
            this.count = count;

            SubmitApi1Request();
        }

        private void SubmitApi1Request()
        {
            //当从Api1获取完所有数据以后再去用Api2获取数据
            if (i > count)
            {
                k = 1;
                n = db.BusTable.ToList().Count;

                SubmitApi2Request();
                return;
            }

            if (j == 1)
            {
                UseApi1();
                j = 2;
            }
            else
            {
                UseApi1();
                j = 1;
                i++;
            }
        }

        private void UseApi1()
        {
            Api1 api1 = new Api1(i, j);
            api1.Response += new Api1.ResponseDelegate(api1_Response);
            api1.Submit();
        }

        void api1_Response(object sender, Api1.ResonseEventArgs e)
        {
            if (e.Success)
            {
                string stopID = "";

                BusItem busInDB = db.BusTable.FirstOrDefault(bus => bus.OfficialId == e.BusItem.OfficialId && bus.Name == e.BusItem.Name);
                
                if (busInDB == null)
                {
                    db.BusTable.InsertOnSubmit(e.BusItem);

                    busInDB = e.BusItem;
                }

                foreach (StopItem stopItem in e.StopList)
                {
                    //将数据库里没有的站加入到数据库中
                    StopItem stopInDB = db.StopTable.FirstOrDefault(stop => stop.OfficialId == stopItem.OfficialId);

                    if (stopInDB == null)
                    {
                        db.StopTable.InsertOnSubmit(stopItem);
                        db.SubmitChanges();

                        stopID += stopItem.Id.ToString() + ";";
                    }
                    else
                    {
                        stopID += stopInDB.Id.ToString() + ";";
                    }
                }

                //移除最后一个“;”
                if (stopID != "")
                {
                    stopID = stopID.Substring(0, stopID.Length - 1);
                }

                e.LineItem.BusItem = busInDB;
                e.LineItem.StopId = stopID;
                db.LineTable.InsertOnSubmit(e.LineItem);
                db.SubmitChanges();
            }

            OnMessage(e.Message);
            SubmitApi1Request();
        }

        private void SubmitApi2Request()
        {
            if (k > n) return;

            var b = from bus in db.BusTable.ToList()
                    where bus.Id == k
                    select bus;

            foreach (BusItem item in b)
            {
                Api2 api2 = new Api2(item.Id, item.Name);
                api2.Response += new DB.Api2.ResponseDelegate(api2_Response);
                api2.Submit();
            }

            k++;
        }

        void api2_Response(object sender, Api2.ResonseEventArgs e)
        {
            var lines = from line in db.LineTable.ToList()
                        where line.BusItem.Id == e.Id
                        select line;

            foreach (LineItem item in lines)
            {
                item.BusItem.Price = e.Price;

                if (item.Type == 1)
                {
                    item.StartTime = e.UpStartTime;
                    item.EndTime = e.UpEndTime;
                    item.Long = e.UpLong;
                }
                else if (item.Type == 2)
                {
                    item.StartTime = e.DownStartTime;
                    item.EndTime = e.DownEndTime;
                    item.Long = e.DownLong;
                }
            }

            db.SubmitChanges();
            OnMessage(string.Format("{0}/{1} {2}", e.Id, n, string.IsNullOrEmpty(e.Price) ? "无数据" : "成功"));
            SubmitApi2Request();
        }
    }
}
