using DataLayer;
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

namespace SalesApp.Master.Product
{
    public partial class wdProductUpdate : Form
    {
        Database dtb;
        string err = string.Empty;
        public wdProductUpdate()
        {
            InitializeComponent();
        }

        bool action;
        public wdProductUpdate(bool isAddNew)
        {
            InitializeComponent();
            LoadUM();
            
            action = isAddNew;
            if (isAddNew == true)
            {
                lblTitle.Text = "PRODUCT - ADD NEW";
            }
            else
            {
                lblTitle.Text = "PRODUCT - UPDATE";
            }
        }

        DataTable dtable;
        private void LoadUM()
        {
            dtb = new Database();
            string sql = "select * from MTMeasure where active = 1";
            SqlDataReader reader = dtb.MyExecuteReader(sql, CommandType.Text, ref err);
            dtable = new DataTable();
            dtable.Load(reader);
            cboUM.DataSource = dtable;
            cboUM.DisplayMember = "measurename";
            cboUM.ValueMember = "measureid";


        }
        private string GetNextProductID()
        {
            string ProID = "";
            dtb = new Database();
            string sql = "select max(cast(productid as int)) +1 from mtproduct";
            object result = dtb.GetValue(sql, CommandType.Text, ref err);
            ProID = result.ToString().PadLeft(10, '0');
            return ProID;

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dtb = new Database();
            if (action)
            {
                string sql = @"insert MTProduct([productid], [productname], [description], [active], [username], [modifieddate], [measureid]) 
                            values (@productid,@productname,@description, 1,@username, @modifieddate, @measureid)";
                SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@productid", GetNextProductID()),
                    new SqlParameter("@productname", txtProName.Text),
                    new SqlParameter("@description",txtDesc.Text),
                    new SqlParameter("@username",BaseClass.username),
                    new SqlParameter("@modifieddate",DateTime.Now),
                    new SqlParameter("@measureid",cboUM.SelectedValue.ToString()),
                };
                bool result = dtb.SetValue(sql, CommandType.Text, ref err, param);
                if (result)
                {
                    MessageBox.Show("Add new Product successfully!");
                }
                else MessageBox.Show("Failed to add new product!");
            }
            else
            {
                string sql = @"update MTProduct set    productname =  @productname,
                                                                description = @description, 
                                                                username = @username,
                                                                modifieddate = @modifieddate, 
                                                                measureid = @measureid
                                                            where productid = @productid";
                SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@productid", txtProID.Text),
                    new SqlParameter("@productname", txtProName.Text),
                    new SqlParameter("@description",txtDesc.Text),
                    new SqlParameter("@username",BaseClass.username),
                    new SqlParameter("@modifieddate",DateTime.Now),
                    new SqlParameter("@measureid",cboUM.SelectedValue.ToString()),
                };
                bool result = dtb.SetValue(sql, CommandType.Text, ref err, param);
                if (result)
                {
                    MessageBox.Show("Update Product successfully!");
                }
                else MessageBox.Show("Failed to update product!");
            }
        }
    }
}
