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
    public partial class ucPriceList : UserControl
    {
        Database dtb;
        string err = string.Empty;
        private DataSet dsList;
        SqlDataAdapter adapter;
        public ucPriceList()
        {
            InitializeComponent();
            Get_Price_Group();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucMenu());
        }


        private void GetPriceDetail()
        {
            SqlConnection conn = null;
            try
            {

                string cellclick = dgPriceGr.CurrentRow.Cells["pricegroupid"].Value.ToString();
                conn = new SqlConnection(BaseClass.StrConnect);
                string sql = @"SELECT PRO.PRODUCTID, 
                                    PRO.PRODUCTNAME, 
                                    PRO.DESCRIPTION, 
                                    PRI.PRICE,
                                    PRI.PRICEGROUPID
                                    FROM MTPRODUCT PRO INNER JOIN PRICEDETAIL PRI ON PRO.PRODUCTID = PRI.PRODUCTID 
                                    WHERE pricegroupid = @pricegroupid and pro.active = 1 and pri.active = 1";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@pricegroupid", SqlDbType.Int).Value = Convert.ToInt32(cellclick);
                using (adapter = new SqlDataAdapter(cmd))
                {
                    dsList = new DataSet();
                    adapter.Fill(dsList, "pricedetail");
                    dgDetail.DataSource = dsList.Tables["pricedetail"];
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Get_Price_Group()
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(BaseClass.StrConnect);
                string sql = @"select pricegroupid, pricegroupname, isused from pricegroup where active = 1";
                using (adapter = new SqlDataAdapter(sql, conn))
                {
                    dsList = new DataSet();
                    adapter.Fill(dsList, "pricegroup");
                    dgPriceGr.DataSource = dsList.Tables["pricegroup"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }


        }

        private void btnAddprod_Click(object sender, EventArgs e)
        {
            FormPriceDetail frm = new FormPriceDetail();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(BaseClass.StrConnect);
            try
            {
                conn.Open();
                adapter.InsertCommand = new SqlCommand();

                adapter.InsertCommand.Connection = conn;
                adapter.InsertCommand.CommandText = @"insert dbo.pricegroup ([pricegroupname], [modifieddate], [username], [active], [isused]) values (@pricegroupname, @modifieddate, @username, 1, 1)";
                adapter.InsertCommand.CommandType = CommandType.Text;
                adapter.InsertCommand.Parameters.AddWithValue("@pricegroupname", dgPriceGr.CurrentRow.Cells["pricegroupname"].Value.ToString());
                adapter.InsertCommand.Parameters.AddWithValue("@modifieddate", DateTime.Now);
                adapter.InsertCommand.Parameters.AddWithValue("@username", BaseClass.username);
                adapter.Update(dsList.Tables["pricegroup"]);
                MessageBox.Show("Add new Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            Get_Price_Group();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(BaseClass.StrConnect);

            try
            {
                adapter.UpdateCommand = new SqlCommand();
                adapter.UpdateCommand.Connection = conn;
                adapter.UpdateCommand.CommandText = @"update dbo.pricegroup set pricegroupname = @pricegroupname, isused = @isused,modifieddate = @modifieddate, username= @username where pricegroupid = @pricegroupid";
                adapter.UpdateCommand.CommandType = CommandType.Text;
                adapter.UpdateCommand.Parameters.AddWithValue("@pricegroupname", SqlDbType.NVarChar).SourceColumn = "pricegroupname";
                adapter.UpdateCommand.Parameters.AddWithValue("@pricegroupid", SqlDbType.Int).SourceColumn = "pricegroupid";
                adapter.UpdateCommand.Parameters.Add("@isused", SqlDbType.Bit).SourceColumn = "isused";
                adapter.UpdateCommand.Parameters.AddWithValue("@modifieddate", DateTime.Now);
                adapter.UpdateCommand.Parameters.AddWithValue("@username", BaseClass.username);
                adapter.Update(dsList.Tables["pricegroup"]);
                MessageBox.Show("Update Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Get_Price_Group();
            }
        }

        //Xử lý sự kiện trên datagridview Price Group
        private void dgPriceGr_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dgPriceGr.Columns["delete"].Index)
            {
                try
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Price Group?", "App Alert", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            dtb = new Database();
                            string sql = @"update pricegroup set active = 0, modifieddate = @modifieddate, username = @username where pricegroupid = @pricegroupid";
                            SqlParameter[] param = new SqlParameter[]{
                                new SqlParameter("@modifieddate", DateTime.Now),
                                new SqlParameter("@username", BaseClass.username),
                                new SqlParameter("@pricegroupid", dgPriceGr.CurrentRow.Cells["pricegroupid"].Value.ToString()),
                            };
                            bool result = dtb.SetValue(sql, CommandType.Text, ref err, param);
                            if (result)
                            {
                                MessageBox.Show("Deleted!");
                            }
                            else MessageBox.Show("Can't delete!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            Get_Price_Group();
                        }
                    }

                }
                catch
                {

                }
            }
            else if (e.ColumnIndex == dgPriceGr.Columns["pricegroupname"].Index)
            {
                GetPriceDetail();
            }
            else if (e.ColumnIndex == dgPriceGr.Columns["isused"].Index)
            {
                string checkpoint = dgPriceGr.CurrentRow.Cells["isused"].Value.ToString();
                dtb = new Database();
                string sql =checkpoint == "True" ?  @"update pricegroup set isused = 0, modifieddate = @modifieddate, username = @username where pricegroupid = @pricegroupid": @"update pricegroup set isused = 1, modifieddate = @modifieddate, username = @username where pricegroupid = @pricegroupid";
                SqlParameter[] param = new SqlParameter[]{
                                new SqlParameter("@modifieddate", DateTime.Now),
                                new SqlParameter("@username", BaseClass.username),
                                new SqlParameter("@pricegroupid", dgPriceGr.CurrentRow.Cells["pricegroupid"].Value.ToString()),
                            };
                bool result = dtb.SetValue(sql, CommandType.Text, ref err, param);
                if (result)
                {
                    MessageBox.Show("Updated!");
                    Get_Price_Group();
                }
                else MessageBox.Show("Error when update!");
            }


        }

        ////Xử lý sự kiện trên datagridview Price Detail
        private void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgDetail.Columns["priceDetailDelete"].Index)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to delete this product?", "App Alert", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        dtb = new Database();
                        string sql = "delete dbo.pricedetail where productid = @productid and pricegroupid = @pricegroupid";
                        SqlParameter[] param = new SqlParameter[] { 
                            new SqlParameter("@productid",dgDetail.CurrentRow.Cells["productid"].Value.ToString()),
                            new SqlParameter("@pricegroupid",dgDetail.CurrentRow.Cells["prigroupid"].Value.ToString()),
                        };
                        bool result = dtb.SetValue(sql, CommandType.Text, ref err, param);
                        if (result)
                        {
                            MessageBox.Show("Delete product successfully!");
                            GetPriceDetail();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete product");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //Update price trong price detail
        private void btnPDUpd_Click(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(BaseClass.StrConnect);
                adapter.UpdateCommand = new SqlCommand();
                adapter.UpdateCommand.Connection = conn;
                adapter.UpdateCommand.CommandText = @"update dbo.pricedetail set price=@price
                                                        where productid = @productid and pricegroupid = @prigroupid";
                adapter.UpdateCommand.CommandType = CommandType.Text;
                adapter.UpdateCommand.Parameters.AddWithValue("@price", SqlDbType.NVarChar).SourceColumn = "price";
                adapter.UpdateCommand.Parameters.AddWithValue("@prigroupid", SqlDbType.Int).SourceColumn = "pricegroupid";
                adapter.UpdateCommand.Parameters.AddWithValue("@productid", SqlDbType.NVarChar).SourceColumn = "productid";

                adapter.Update(dsList, "pricedetail");
                MessageBox.Show("Update Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                GetPriceDetail();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
