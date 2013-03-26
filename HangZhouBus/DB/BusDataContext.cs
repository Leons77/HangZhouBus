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
using System.Data.Linq;

namespace HangZhouBus.DB
{
    public class BusDataContext : DataContext
    {
        public const string DBName = "DB.sdf";

        public Table<BusItem> BusTable;
        public Table<StopItem> StopTable;
        public Table<LineItem> LineTable;

        public BusDataContext()
            : base("Data Source=appdata:/Assets/" + DBName + ";File Mode=read only;")
            //: base("Data Source=isostore:/" + DBName)
        {
        }

    }
}
