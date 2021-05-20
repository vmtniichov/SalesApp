using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;
namespace SalesApp.Master.Customer
{
    public partial class ucCustomerList : UserControl
    {
        Database dtbCon = new Database();
        string err = "";
        public ucCustomerList()
        {
            InitializeComponent();
            Customer_Getlist();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucMenu());
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            bool isAddNew = true;
            wdCustomerUpdate frmCustomerNew = new wdCustomerUpdate(isAddNew);

            frmCustomerNew.StartPosition = FormStartPosition.CenterScreen;
            frmCustomerNew.ShowDialog();
            Customer_Getlist();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool isAddNew = false;
            wdCustomerUpdate frmCustomerNew = new wdCustomerUpdate(isAddNew);

            frmCustomerNew.txtID.Text = dgCustomer.CurrentRow.Cells["custid"].Value.ToString();
            frmCustomerNew.txtCustName.Text = dgCustomer.CurrentRow.Cells["custname"].Value.ToString();
            frmCustomerNew.txtCustIDCard.Text = dgCustomer.CurrentRow.Cells["idcard"].Value.ToString();
            frmCustomerNew.txtIDDate.Text = dgCustomer.CurrentRow.Cells["idcarddate"].Value.ToString();
            frmCustomerNew.txtTax.Text = dgCustomer.CurrentRow.Cells["tax"].Value.ToString();
            frmCustomerNew.txtTell.Text = dgCustomer.CurrentRow.Cells["tel"].Value.ToString();
            frmCustomerNew.txtDesc.Text = dgCustomer.CurrentRow.Cells["description"].Value.ToString();
            frmCustomerNew.txtAddr.Text = dgCustomer.CurrentRow.Cells["address"].Value.ToString();
            string strIDdate = dgCustomer.CurrentRow.Cells["idcarddate"].Value.ToString();
            frmCustomerNew.txtIDDate.Value = DateTime.ParseExact(strIDdate, "M/d/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

            frmCustomerNew.StartPosition = FormStartPosition.CenterScreen;
            frmCustomerNew.ShowDialog();
            Customer_Getlist();
        }
        DataTable dtTable;
        private void Customer_Getlist()
        {

            try
            {
                string cmd = "select custid, custname, idcard, idcarddate, address, tax, tel, description from mtcustomer where active = 1";
                dtTable = dtbCon.GetDataTable(cmd, CommandType.Text, ref err);
                dgCustomer.DataSource = dtTable;   //dgEmployeeList là DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Customer?", "App Alert", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string id = dgCustomer.CurrentRow.Cells["custid"].Value.ToString();
                string cmd = "update mtcustomer set active = 0, username = @username where custid = @id";
                try
                {
                    bool kq = dtbCon.SetValue(cmd, CommandType.Text, ref err, new SqlParameter("@username", BaseClass.username),
                                                                               new SqlParameter("@id", id));
                    Customer_Getlist();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtbFilter = dtTable.AsEnumerable().Where(
                                                                row => row.Field<String>("custname").ToLower().Contains(txtSearch.Text.ToLower())
                                                                || row.Field<String>("custname").ToLower().Contains(txtSearch.Text.ToLower())
                                                            )
                                                            .OrderByDescending(row => row.Field<String>("custname"))
                                                            .CopyToDataTable();
                dgCustomer.DataSource = dtbFilter;
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}