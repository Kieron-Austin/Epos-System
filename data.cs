using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace Motapart_Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CustomerData
    {
        public int id;
        public string name;
        public string address;
        public string number;
    }

    public struct StockData
    {
        public int barcode;
        public string name;
        public int stocklevel;
        public string supplier;
        public string price;
        public string costprice;
        public string image;
        public int warninglevel;
    }

}
