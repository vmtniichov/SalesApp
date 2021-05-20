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
    public partial class ucProductList : UserControl
    {
        Database dtb;
        string err = string.Empty;
        public ucProductList()
        {
            InitializeComponent();
            getProductList();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucMenu());
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            wdProductUpdate frmProduct = new wdProductUpdate(true);
            frmProduct.StartPosition = FormStartPosition.CenterScreen;
            frmProduct.ShowDialog();
            getProductList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            wdProductUpdate frmProduct = new wdProductUpdate(false);
            frmProduct.StartPosition = FormStartPosition.CenterScreen;
            frmProduct.txtProID.Text = dgProduct.CurrentRow.Cells["productid"].Value.ToString();
            frmProduct.txtProName.Text = dgProduct.CurrentRow.Cells["productname"].Value.ToString();
            frmProduct.txtDesc.Text = dgProduct.CurrentRow.Cells["description"].Value.ToString();
            frmProduct.cboUM.Text = dgProduct.CurrentRow.Cells["measurename"].Value.ToString();
            frmProduct.ShowDialog();
            getProductList();
        }
        DataTable dttb;
        private void getProductList()
        {
            dtb = new Database();
            try
            {
                string cmd = @"select productid, productname,description, mea.measurename 
                                    from mtproduct pro inner join mtmeasure mea on pro.measureid = mea.measureid 
                                    where pro.active = 1";

                dttb = dtb.GetDataTable(cmd, CommandType.Text, ref err);
                dgProduct.DataSource = dttb;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtbFilter = dttb.AsEnumerable().Where(
                                                                row => row.Field<String>("productname").ToLower().Contains(txtSearch.Text.ToLower())
                                                                || row.Field<String>("productname").ToLower().Contains(txtSearch.Text.ToLower())
                                                            )
                                                            .OrderByDescending(row => row.Field<String>("productname"))
                                                            .CopyToDataTable();
                dgProduct.DataSource = dtbFilter;
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this product?", "App Alert", MessageBoxButtons.YesNo);
            string proid = dgProduct.CurrentRow.Cells["productid"].Value.ToString();
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string cmd = "update mtproduct set active = 0 where productid = @productid";
                    bool kq = dtb.SetValue(cmd, CommandType.Text, ref err, new SqlParameter("@productid", proid));
                    if (!kq)
                    {
                        MessageBox.Show("Fail to delete product");
                    }
                    else
                    {
                        MessageBox.Show("Delete product successfully!");
                    }
                    getProductList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else

            }
        }

        private void dgProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
