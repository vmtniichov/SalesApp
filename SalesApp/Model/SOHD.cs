using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Model
{
    class SOHD
    {
        public string soid { get; set; }
        public string tableid { get; set; }
        public string discountpercent { get; set; }
        public double discountamount { get; set; }
        public double amount { get; set; }
        public DateTime sodate { get; set; }
        public string deliveryname { get; set; }
        public string custname { get; set; }
        public string empname { get; set; }
    }
}
