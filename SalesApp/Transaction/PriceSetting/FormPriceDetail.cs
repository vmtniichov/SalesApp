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

namespace SalesApp.Transaction.PriceSetting
{
    public partial class FormPriceDetail : Form
    {
        Database dtb;
        string err = string.Empty;
        public FormPriceDetail()
        {
            InitializeComponent();
            LoadPG();
        }
        DataTable dtable;
        private void LoadPG()
        {
            dtb = new Database();
            string sql = "select * from pricegroup where active = 1";
            SqlDataReader reader = dtb.MyExecuteReader(sql, CommandType.Text, ref err);
            dtable = new DataTable();
            dtable.Load(reader);
            cboPG.DataSource = dtable;
            cboPG.DisplayMember = "pricegroupname";
            cboPG.ValueMember = "pricegroupid";


        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SqlConnection cnn = null;
            try
            {
                var sql = string.Format("Select * from MTProduct where productid = {0}",txtSearch.Text);
                cnn = new SqlConnection(BaseClass.StrConnect);
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        txtProID.Text = reader[0].ToString();
                        txtProName.Text = reader[1].ToString();
                        txtDesc.Text = reader[2].ToString();
                    }
                else MessageBox.Show("Can't find item!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cnn.Close();
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }


        private string GetPGID()
        {
            string PgID = "";
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(BaseClass.StrConnect);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select pricegroupid from pricegroup where pricegroupname = @pricegroupname";
                cmd.CommandType = CommandType.Text;

                SqlParameter pricegroupname = new SqlParameter();
                pricegroupname.ParameterName = "@pricegroupname";
                pricegroupname.SqlValue = cboPG.Text;
                cmd.Parameters.Add(pricegroupname);

                object result = cmd.ExecuteScalar();
                PgID = result.ToString();
                return PgID;

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
        private void btnSave_Click(object sender, EventArgs e)
        {
            dtb = new Database();
            try
            {
                string sql = @"insert pricedetail ([productid], [price], [modifieddate], [username], [active], [pricegroupid])
                                    values (@productid, @price, @modifieddate, @username, 1, @pricegroupid)";
                SqlParameter[] param = new SqlParameter[] { 
                    new SqlParameter("@productid", txtProID.Text),
                    new SqlParameter("@price", txtPrice.Text),
                    new SqlParameter("@modifieddate", DateTime.Now),
                    new SqlParameter("@username", BaseClass.username),
                    new SqlParameter("@pricegroupid", GetPGID())
                };
                bool result = dtb.SetValue(sql,CommandType.Text,ref err, param);
                if (result)
                {
                    MessageBox.Show("Set price for product successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to set price");
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
