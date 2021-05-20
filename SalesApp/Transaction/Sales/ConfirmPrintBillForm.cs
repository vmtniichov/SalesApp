using DataLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SalesApp.Transaction.Sales
{
    public partial class ConfirmPrintBillForm : Form
    {
        Database dtb;
        DataTable delilist;
        string err = string.Empty;
        public ConfirmPrintBillForm()
        {
            InitializeComponent();
            LoadDeli();
            cboDeli.SelectedItem = null;
            cboDeli.SelectedValue = "null";
            cboDeli.SelectedText = "--select--";
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                cboDeliMask.Visible = false;
                if (txtEmpSearch.Text == "")
                {
                    label2.Text = "Input your employee ID!";
                }
                else
                {
                    DialogResult dia = MessageBox.Show("Are you sure to print bill and end transaction?", "App Alert", MessageBoxButtons.YesNo);
                    if (dia == DialogResult.Yes)
                    {
                        dtb = new Database();
                        string query = txtCustSearch.Text == "" ? @"update sohd set discountpercent = @discountpercent, 
                            discountamount = @discountamount,
                            amount = @amount, 
                            modifieddate = @modifieddate, 
                            username = @username,
                            printbill = 1,
                            empid = @empid,
                            deliveryid = @deliveryid
                            where soid = @soid"
                                        :
                                    @"update sohd set discountpercent = @discountpercent, 
                            discountamount = @discountamount,
                            amount = @amount, 
                            modifieddate = @modifieddate, 
                            username = @username,
                            printbill = 1,
                            custid = @custid,
                            deliveryid = @deliveryid,
                            empid = @empid
                            where soid = @soid";
                        SqlParameter[] param = txtCustSearch.Text == "" ?
                         new SqlParameter[]
                        {
                    new SqlParameter("@discountpercent",txtPercent.Text),
                    new SqlParameter("@discountamount",txtDiscountAmount.Text),
                    new SqlParameter("@amount",txtAmount.Text),
                    new SqlParameter("@modifieddate",DateTime.Now),
                    new SqlParameter("@username",BaseClass.username),
                    new SqlParameter("@empid",txtEmpSearch.Text),
                    new SqlParameter("@deliveryid",cboDeli.SelectedValue),
                    new SqlParameter("@soid",txtSOID.Text)
                        }
                        :
                        new SqlParameter[]
                        {
                    new SqlParameter("@discountpercent",txtPercent.Text),
                    new SqlParameter("@discountamount",txtDiscountAmount.Text),
                    new SqlParameter("@amount",txtAmount.Text),
                    new SqlParameter("@modifieddate",DateTime.Now),
                    new SqlParameter("@username",BaseClass.username),
                    new SqlParameter("@empid",txtEmpSearch.Text),
                    new SqlParameter("@deliveryid",cboDeli.SelectedValue),
                    new SqlParameter("@soid",txtSOID.Text),
                    new SqlParameter("@custid",txtCustSearch.Text)
                        };
                        bool print_result = dtb.SetValue(query, CommandType.Text, ref err, param);

                        //Update table to default
                        if (print_result)
                        {
                            string update_query = "update mttable set isused = 0, soid = null where id=@id";
                            bool tab_result = dtb.SetValue(update_query, CommandType.Text, ref err, new SqlParameter("@id", lblTableID.Text));
                            MessageBox.Show(string.Format("Hoàn tất in hóa đơn bàn: {0}. Mã Hóa Đơn: {1}", lblTableID.Text, txtSOID.Text));
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                cboDeliMask.Visible = true;
                if (txtEmpSearch.Text == "")
                {
                    label2.Text = "Input your employee ID!";
                }
                else
                {
                    DialogResult dia = MessageBox.Show("Are you sure to print bill and end transaction?", "App Alert", MessageBoxButtons.YesNo);
                    if (dia == DialogResult.Yes)
                    {
                        dtb = new Database();
                        string query = 
                            txtCustSearch.Text == "" ? 
                            @"update sohd set discountpercent = @discountpercent, 
                            discountamount = @discountamount,
                            amount = @amount, 
                            modifieddate = @modifieddate, 
                            username = @username,
                            printbill = 1,
                            empid = @empid
                            where soid = @soid"
                            :
                            @"update sohd set discountpercent = @discountpercent, 
                            discountamount = @discountamount,
                            amount = @amount, 
                            modifieddate = @modifieddate, 
                            username = @username,
                            printbill = 1,
                            custid = @custid,
                            empid = @empid
                            where soid = @soid";
                        SqlParameter[] param = txtCustSearch.Text == "" ?
                         new SqlParameter[]
                        {
                    new SqlParameter("@discountpercent",txtPercent.Text),
                    new SqlParameter("@discountamount",txtDiscountAmount.Text),
                    new SqlParameter("@amount",txtAmount.Text),
                    new SqlParameter("@modifieddate",DateTime.Now),
                    new SqlParameter("@username",BaseClass.username),
                    new SqlParameter("@empid",txtEmpSearch.Text),
                    new SqlParameter("@soid",txtSOID.Text)
                        }
                        :
                        new SqlParameter[]
                        {
                    new SqlParameter("@discountpercent",txtPercent.Text),
                    new SqlParameter("@discountamount",txtDiscountAmount.Text),
                    new SqlParameter("@amount",txtAmount.Text),
                    new SqlParameter("@modifieddate",DateTime.Now),
                    new SqlParameter("@username",BaseClass.username),
                    new SqlParameter("@empid",txtEmpSearch.Text),
                    new SqlParameter("@soid",txtSOID.Text),
                    new SqlParameter("@custid",txtCustSearch.Text)
                        };
                        bool print_result = dtb.SetValue(query, CommandType.Text, ref err, param);

                        //Update table to default
                        if (print_result)
                        {
                            string update_query = "update mttable set isused = 0, soid = null where id=@id";
                            bool tab_result = dtb.SetValue(update_query, CommandType.Text, ref err, new SqlParameter("@id", lblTableID.Text));
                            MessageBox.Show(string.Format("Hoàn tất in hóa đơn bàn: {0}. Mã Hóa Đơn: {1}", lblTableID.Text, txtSOID.Text));
                            this.Close();
                        }
                    }
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnCustSearch_Click(object sender, EventArgs e)
        {
            dtb = new Database();
            string query = string.Format("select custname,custid from mtcustomer where custid = {0}", txtCustSearch.Text);
            SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err);
            if(reader.HasRows) {
                while (reader.Read())
                {
                    txtCustSearch.Text = reader["custid"].ToString();
                    MessageBox.Show(string.Format("ID: {0}\nName: {1}", reader["custid"].ToString(), reader["custname"].ToString()));
                }
            }

        }

        private void txtCustSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnEmpCheck_Click(object sender, EventArgs e)
        {
            try
            {
                dtb = new Database();
                string query = string.Format("select empname,empid from mtemployee where empid = {0}", txtEmpSearch.Text);
                SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err, new SqlParameter("@empid", txtEmpSearch.Text));
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        label2.Text = "Name: " + reader["empname"].ToString();
                        txtEmpSearch.Text = reader["empid"].ToString();
                    }
                }
                else
                {
                    label2.Text = "No Employee was found!";
                }
            }
            catch { }
            
        }
        private void LoadDeli()
        {
            dtb = new Database();
            string sql = "select * from mtdelivery where active = 1";
            SqlDataReader reader = dtb.MyExecuteReader(sql, CommandType.Text, ref err);
            delilist = new DataTable();
            delilist.Load(reader);
            cboDeli.DataSource = delilist;
            cboDeli.DisplayMember = "deliveryname";
            cboDeli.ValueMember = "deliveryid";


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                cboDeliMask.Visible = false;
            }
            else
            {
                cboDeliMask.Visible = true;
            }
        }
    }
}
