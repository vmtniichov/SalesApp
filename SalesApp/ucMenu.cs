using SalesApp.Master.Customer;
using SalesApp.Master.Deliver;
using SalesApp.Master.Employee;
using SalesApp.Master.Product;
using SalesApp.Transaction.PriceSetting;
using SalesApp.Transaction.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesApp
{
    public partial class ucMenu : UserControl
    {
        public ucMenu()
        {
            InitializeComponent();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent,new ucEmployeeList());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucProductList());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucCustomerList());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucDeliverList());
        }

        private void btnPrice_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucPriceList());
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucSales());
        }

        private void btnSaleDetailReport_Click_1(object sender, EventArgs e)
        {
            frmSaleReport frm = new frmSaleReport(true);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        private void btnSaleSummaryReport_Click(object sender, EventArgs e)
        {
            frmSaleReport frm = new frmSaleReport(false);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }
    }
}
