using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Model
{
    class SUMMARY
    {
        public string productid { get; set; }
        public string productname{ get; set; }
        public int quantity { get; set; }
        public double discount { get; set; }
        public double amount { get; set; }
        public double total_amount { get; set; }
    }
}
