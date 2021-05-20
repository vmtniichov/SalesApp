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
    public partial class wdCustomerUpdate : Form
    {
        public wdCustomerUpdate()
        {
            InitializeComponent();
        }
        bool action;
        public wdCustomerUpdate(bool isAddNew)
        {
            InitializeComponent();
            action = isAddNew;
            if (isAddNew == true)
            {
                lblTitle.Text = "CUSTOMER - ADD NEW";
            }
            else
            {
                lblTitle.Text = "CUSTOMER - UPDATE";
            }
        }

        private string GetCustID()
        {
            string CustID = "";
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(BaseClass.StrConnect);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select max(cast(custid as int)) +1 from mtcustomer";
                cmd.CommandType = CommandType.Text;
                object id = cmd.ExecuteScalar();
                CustID = id.ToString().PadLeft(10, '0');
                return CustID;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
            finally
            {
                conn.Close();
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            
            if (action)
            {
                string err = string.Empty;
                string sql = @"insert into mtcustomer (custid, custname, idcard, idcarddate, address, tax, tel, description, modifieddate, username, active)
                                                          values(@custid, @custname, @idcard, @idcarddate, @address, @tax, @tel, @description, @modifieddate, @username, @active)";
                Database data = new Database();
                    bool result = data.SetValue(sql, CommandType.Text,ref err, 
                        new SqlParameter("@custid", GetCustID()),
                        new SqlParameter("@custname", txtCustName.Text),
                        new SqlParameter("@idcard", txtID.Text),
                        new SqlParameter("@idcarddate", txtIDDate.Value),
                        new SqlParameter("@address", txtAddr.Text),
                        new SqlParameter("@tax", txtTax.Text),
                        new SqlParameter("@tel", txtTell.Text),
                        new SqlParameter("@description", txtDesc.Text),
                        new SqlParameter("@modifieddate", DateTime.Now),
                        new SqlParameter("@username", BaseClass.username),
                        new SqlParameter("@active", 1)
                    );

                if(result){
                    MessageBox.Show("Add new custommer successfully!");
                }
                else{
                    MessageBox.Show("Add failed!");
                }
            }//if action
            else
            {
                string err = string.Empty;
                    string sql = @"update mtcustomer set custname=@custname, 
                                                            tel=@tel, 
                                                            tax=@tax, 
                                                            idcard=@idcard, 
                                                            idcarddate=@idcarddate, 
                                                            address=@address, 
                                                            username=@username, 
                                                            description = @description,
                                                            modifieddate=@modifieddate
			                            where custid=@custid";
                    Database data = new Database();
                    bool result = data.SetValue(sql, CommandType.Text, ref err,
                        new SqlParameter("@custid", txtID.Text),
                        new SqlParameter("@custname", txtCustName.Text),
                        new SqlParameter("@idcard", txtID.Text),
                        new SqlParameter("@idcarddate", txtIDDate.Value),
                        new SqlParameter("@address", txtAddr.Text),
                        new SqlParameter("@tax", txtTax.Text),
                        new SqlParameter("@tel", txtTell.Text),
                        new SqlParameter("@description", txtDesc.Text),
                        new SqlParameter("@modifieddate", DateTime.Now),
                        new SqlParameter("@username", BaseClass.username),
                        new SqlParameter("@active", 1)
                    );
                    if (result)
                    {
                        if (txtCustName.Text != "")
                        {

                            MessageBox.Show("Updated Customer Successfully!");
                        }
                        else MessageBox.Show("Please check again");
                    }
                
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}