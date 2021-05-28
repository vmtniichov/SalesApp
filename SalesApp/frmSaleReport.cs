using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SalesApp
{
    public partial class frmSaleReport : Form
    {
        DataLayer.Database dtb;
        string err = string.Empty;
        public frmSaleReport()
        {
            InitializeComponent();
        }
        bool check;
        public frmSaleReport(bool action)
        {
            InitializeComponent();
            check = action;
            if (action)
            {
                lblTitle.Text = "Sales Detail Report";
                lblSSM.Visible = false;
                startDate.Visible = false;
            }
            else
            {
                lblTitle.Text = "Sales Summary Report";
                lblSSM.Visible = true;
                startDate.Visible = true;
            }
        }

        private void SalesDetailForm_Load(object sender, EventArgs e)
        {

        }
        private DataTable loadHD()
        {
            DataTable dataTable;
            dtb = new DataLayer.Database();
            if (check)
            {
                dtb = new DataLayer.Database();
                string query = @"select soid, tableid, discountpercent, discountamount, amount, sodate, deli.deliveryname, ct.custname, em.empname, SUM(hd.amount)-sum(hd.discountamount) as total_amount
                            from sohd hd left join mtcustomer ct on hd.custid = ct.custid left join MTEmployee em on em.empid = hd.empid left join MTDelivery deli on deli.deliveryid = hd.deliveryid
                            where sodate = @enddate and hd.isdelete = 0 and hd.printbill = 1
							group by soid, tableid, discountpercent, discountamount, amount, sodate, deli.deliveryname, ct.custname, em.empname";
                SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err, new SqlParameter("@enddate", endDate.Value.ToShortDateString()));
                dataTable = new DataTable();
                dataTable.Load(reader);
            }
            else
            {
                dtb = new DataLayer.Database();
                string query = @"select dt.productid, pro.productname, count(dt.productid) as quantity,sum(price) as amount, sum(hd.discountamount)as discount,  SUM(price)-sum(hd.discountamount) as total_amount
                                from sodt dt join MTProduct pro on pro.productid = dt.productid join sohd hd on hd.soid = dt.soid
                                where dt.productid in(select distinct dt.productid from sodt) and hd.sodate between @startdate and @enddate
                                group by pro.productname,dt.productid";
                SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err, new SqlParameter("@startdate", startDate.Value.ToShortDateString()), new SqlParameter("@enddate", endDate.Value));
                dataTable = new DataTable();
                dataTable.Load(reader);
            }
            return dataTable;

        }

        //private DataTable loadSummary()
        //{

        //}

        private void btnSee_Click(object sender, EventArgs e)
        {
            DataTable hd_report = loadHD();
            ReportDocument rptDoc = new ReportDocument();
            string reportPath = check == true ? Application.StartupPath + @"\Report\SalesDetail\SaleDetail.rpt" : Application.StartupPath + @"\Report\SalesSummary\SalesSummary.rpt";
            rptDoc.Load(reportPath);
            rptDoc.SetDataSource(hd_report);
            cRv.ReportSource = rptDoc;
            cRv.DisplayToolbar = true;
        }
    }
}
