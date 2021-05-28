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
        DataTable productlist;
        string err = string.Empty;
        public ConfirmPrintBillForm()
        {
            InitializeComponent();
            LoadDeli();
            cboDeli.SelectedItem = null;
        }

        private void ConfirmPrintBillForm_Load(object sender, EventArgs e)
        {
            try
            {
                Display_SODT_Detail(txtSOID.Text);
                txtAmount.Text = string.Format("{0:#,##0}", double.Parse(txtAmount.Text));
                txtNetAmount.Text = string.Format("{0:#,##0}", double.Parse(txtNetAmount.Text));
                txtDiscountAmount.Text = string.Format("{0:#,##0}", double.Parse(txtDiscountAmount.Text));
                loadCustName();
                loadEmpName();
            }
            catch{}

        }

        //Display chi tiết Hóa đơn
        private void Display_SODT_Detail(string soid)
        {
            string query = @"select productname,count(dt.productid) as quantity,price,sum(price) as total, dt.productid,soid
                            from sodt dt join mtproduct pro on dt.productid = pro.productid
                            where dt.productid in(select distinct productid from sodt) and soid = @soid
                            group by dt.productid,price,productname,soid order by dt.productid";

            dtb = new Database();
            productlist = new DataTable();
            SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err, new SqlParameter("@soid", soid));
            if (reader.HasRows)
            {
                productlist.Load(reader);
                dgProduct.DataSource = productlist;
            }

        }


        //Lấy Employee ID
        private string getEmpID(string empname)
        {
            dtb = new Database();
            string id = string.Empty;
            string query = @"select empid from mtemployee where empname like @empname";
            object result = dtb.GetValue(query, CommandType.Text, ref err, new SqlParameter("@empname", txtEmpSearch.Text));
            try
            {
                id = result.ToString();
            }
            catch { }
            return id;
        }

        //Lấy Customer ID
        private string getCustID(string custname)
        {
            dtb = new Database();
            string id = string.Empty;
            string query = @"select custid from mtcustomer where custname like @custname";
            object result = dtb.GetValue(query, CommandType.Text, ref err, new SqlParameter("@custname", txtCustSearch.Text));
            try
            {
                id = result.ToString();
            }
            catch { }
            return id;
        }

        //Nút Xác nhận in bill
        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {

                DialogResult dia = MessageBox.Show("Are you sure to print bill and end transaction?", "App Alert", MessageBoxButtons.YesNo);
                if (dia == DialogResult.Yes)
                {
                    dtb = new Database();
                    string query = string.Empty;
                    SqlParameter[] param;

                    //Cả 2 đều empty
                    if (txtEmpSearch.Text == "" && txtCustSearch.Text == "")
                    {
                        query = @"update sohd set discountpercent = @discountpercent, 
                                        discountamount = @discountamount,
                                        amount = @amount, 
                                        modifieddate = @modifieddate, 
                                        username = @username,
                                        deliveryid = @deliveryid,
                                        printbill = 1
                                        where soid = @soid";
                        param = new SqlParameter[]
                        {
                        new SqlParameter("@discountpercent",txtPercent.Text),
                        new SqlParameter("@discountamount",txtDiscountAmount.Text),
                        new SqlParameter("@amount",txtAmount.Text),
                        new SqlParameter("@modifieddate",DateTime.Now),
                        new SqlParameter("@deliveryid",cboDeli.SelectedValue),
                        new SqlParameter("@username",BaseClass.username),
                        new SqlParameter("@soid",txtSOID.Text)
                        };
                    }

                    //Chỉ có tên customer
                    else if (txtEmpSearch.Text == "" && txtCustSearch.Text != "")
                    {
                        query = @"update sohd set discountpercent = @discountpercent, 
                                        discountamount = @discountamount,
                                        amount = @amount, 
                                        modifieddate = @modifieddate, 
                                        username = @username,
                                        deliveryid = @deliveryid,
                                        custid = @custid,
                                        printbill = 1
                                        where soid = @soid";
                        param = new SqlParameter[]
                        {
                        new SqlParameter("@discountpercent",txtPercent.Text),
                        new SqlParameter("@discountamount",txtDiscountAmount.Text),
                        new SqlParameter("@amount",txtAmount.Text),
                        new SqlParameter("@modifieddate",DateTime.Now),
                        new SqlParameter("@deliveryid",cboDeli.SelectedValue),
                        new SqlParameter("@username",BaseClass.username),
                        new SqlParameter("@custid",getCustID(txtCustSearch.Text)),
                        new SqlParameter("@soid",txtSOID.Text)
                        };
                    }

                    //chỉ có tên nhân viên
                    else if (txtEmpSearch.Text != "" && txtCustSearch.Text == "")
                    {
                        query = @"update sohd set discountpercent = @discountpercent, 
                                        discountamount = @discountamount,
                                        amount = @amount, 
                                        modifieddate = @modifieddate, 
                                        username = @username,
                                        deliveryid = @deliveryid,
                                        empid = @empid,
                                        printbill = 1
                                        where soid = @soid";
                        param = new SqlParameter[]
                        {
                        new SqlParameter("@discountpercent",txtPercent.Text),
                        new SqlParameter("@discountamount",txtDiscountAmount.Text),
                        new SqlParameter("@amount",txtAmount.Text),
                        new SqlParameter("@modifieddate",DateTime.Now),
                        new SqlParameter("@deliveryid",cboDeli.SelectedValue),
                        new SqlParameter("@username",BaseClass.username),
                        new SqlParameter("@empid",getEmpID(txtEmpSearch.Text)),
                        new SqlParameter("@soid",txtSOID.Text)
                        };
                    }

                    //Cả 2 có giá trị
                    else if (txtEmpSearch.Text != "" && txtCustSearch.Text != "")
                    {
                        query = @"update sohd set discountpercent = @discountpercent, 
                                        discountamount = @discountamount,
                                        amount = @amount, 
                                        modifieddate = @modifieddate, 
                                        username = @username,
                                        deliveryid = @deliveryid,
                                        empid = @empid,
                                        custid = @custid,
                                        printbill = 1
                                        where soid = @soid";
                        param = new SqlParameter[]
                        {
                        new SqlParameter("@discountpercent",txtPercent.Text),
                        new SqlParameter("@discountamount",txtDiscountAmount.Text),
                        new SqlParameter("@amount",txtAmount.Text),
                        new SqlParameter("@modifieddate",DateTime.Now),
                        new SqlParameter("@username",BaseClass.username),
                        new SqlParameter("@deliveryid",cboDeli.SelectedValue),
                        new SqlParameter("@empid",getEmpID(txtEmpSearch.Text)),
                        new SqlParameter("@custid",getCustID(txtCustSearch.Text)),
                        new SqlParameter("@soid",txtSOID.Text)
                        };
                    }
                    else { param = null; }

                    bool print_result = dtb.SetValue(query, CommandType.Text, ref err, param);

                    //Update table to default
                    if (print_result)
                    {
                        string update_query = "update mttable set isused = 0, soid = null where id=@id";
                        bool tab_result = dtb.SetValue(update_query, CommandType.Text, ref err, new SqlParameter("@id", lblTableID.Text));
                        MessageBox.Show(string.Format("Hoàn tất in hóa đơn! Mã Hóa Đơn: {0}", txtSOID.Text));
                        this.Close();
                    }
                }

            }
            else
            {

                DialogResult dia = MessageBox.Show("Are you sure to print bill and end transaction?", "App Alert", MessageBoxButtons.YesNo);
                if (dia == DialogResult.Yes)
                {
                    dtb = new Database();
                    string query = string.Empty;
                    SqlParameter[] param;

                    if (txtEmpSearch.Text == "" && txtCustSearch.Text == "")
                    {
                        query = @"update sohd set discountpercent = @discountpercent, 
                                        discountamount = @discountamount,
                                        amount = @amount, 
                                        modifieddate = @modifieddate, 
                                        username = @username,
                                        printbill = 1
                                        where soid = @soid";
                        param = new SqlParameter[]
                        {
                        new SqlParameter("@discountpercent",txtPercent.Text),
                        new SqlParameter("@discountamount",txtDiscountAmount.Text),
                        new SqlParameter("@amount",txtAmount.Text),
                        new SqlParameter("@modifieddate",DateTime.Now),
                        new SqlParameter("@username",BaseClass.username),
                        new SqlParameter("@soid",txtSOID.Text)
                        };
                    }

                    //only textbox customer have value
                    else if (txtEmpSearch.Text == "" && txtCustSearch.Text != "")
                    {
                        query = @"update sohd set discountpercent = @discountpercent, 
                                        discountamount = @discountamount,
                                        amount = @amount, 
                                        modifieddate = @modifieddate, 
                                        username = @username,
                                        custid = @custid,
                                        printbill = 1
                                        where soid = @soid";
                        param = new SqlParameter[]
                        {
                        new SqlParameter("@discountpercent",txtPercent.Text),
                        new SqlParameter("@discountamount",txtDiscountAmount.Text),
                        new SqlParameter("@amount",txtAmount.Text),
                        new SqlParameter("@modifieddate",DateTime.Now),
                        new SqlParameter("@username",BaseClass.username),
                        new SqlParameter("@custid",getCustID(txtCustSearch.Text)),
                        new SqlParameter("@soid",txtSOID.Text)
                        };
                    }
                    //only textbox employee have value
                    else if (txtEmpSearch.Text != "" && txtCustSearch.Text == "")
                    {
                        query = @"update sohd set discountpercent = @discountpercent, 
                                        discountamount = @discountamount,
                                        amount = @amount, 
                                        modifieddate = @modifieddate, 
                                        username = @username,
                                        empid = @empid,
                                        printbill = 1
                                        where soid = @soid";
                        param = new SqlParameter[]
                        {
                        new SqlParameter("@discountpercent",txtPercent.Text),
                        new SqlParameter("@discountamount",txtDiscountAmount.Text),
                        new SqlParameter("@amount",txtAmount.Text),
                        new SqlParameter("@modifieddate",DateTime.Now),
                        new SqlParameter("@username",BaseClass.username),
                        new SqlParameter("@empid",getEmpID(txtEmpSearch.Text)),
                        new SqlParameter("@soid",txtSOID.Text)
                        };
                    }

                    //Both text box has value
                    else if (txtEmpSearch.Text != "" && txtCustSearch.Text != "")
                    {
                        query = @"update sohd set discountpercent = @discountpercent, 
                                        discountamount = @discountamount,
                                        amount = @amount, 
                                        modifieddate = @modifieddate, 
                                        username = @username,
                                        empid = @empid,
                                        custid = @custid,
                                        printbill = 1
                                        where soid = @soid";
                        param = new SqlParameter[]
                        {
                        new SqlParameter("@discountpercent",txtPercent.Text),
                        new SqlParameter("@discountamount",txtDiscountAmount.Text),
                        new SqlParameter("@amount",txtAmount.Text),
                        new SqlParameter("@modifieddate",DateTime.Now),
                        new SqlParameter("@username",BaseClass.username),
                        new SqlParameter("@empid",getEmpID(txtEmpSearch.Text)),
                        new SqlParameter("@custid",getCustID(txtCustSearch.Text)),
                        new SqlParameter("@soid",txtSOID.Text)
                        };
                    }
                    else { param = null; }
                    
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
        
        //Load dữ liệu của delivery lên combobox
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

        // Hiện/Ẩn combobox Deli
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                cboDeli.Visible = true;
            }
            else
            {
                cboDeli.Visible = false;
            }
        }

        //Load dữ liệu lên textbox để Autocomplete 
        #region Autocomplete
        private void loadCustName()
        {
            dtb = new Database();

            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();

            string query = @"select * from mtcustomer";

            SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err);
            try
            {
                if (reader.Read())
                {
                    while (reader.Read())
                        namesCollection.Add(reader["custname"].ToString());
                }
                txtCustSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtCustSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtCustSearch.AutoCompleteCustomSource = namesCollection;
            }
            catch { }
        }
        private void loadEmpName()
        {
            dtb = new Database();

            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();

            string query = @"select * from mtemployee";

            SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err);
            try
            {
                if (reader.Read())
                {
                    while (reader.Read())
                        namesCollection.Add(reader["empname"].ToString());
                }
                txtEmpSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtEmpSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtEmpSearch.AutoCompleteCustomSource = namesCollection;
            }
            catch { }
        }
        #endregion
    }

}
