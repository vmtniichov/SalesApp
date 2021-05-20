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

namespace SalesApp.Master.Deliver
{
    public partial class ucDeliverList : UserControl
    {
        Database dtb;
        string err = string.Empty;
        public ucDeliverList()
        {
            InitializeComponent();
            getDeliList();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucMenu());
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            wdDeliverUpdate frmDelivery = new wdDeliverUpdate(true);
            frmDelivery.StartPosition = FormStartPosition.CenterScreen;
            frmDelivery.ShowDialog();
            getDeliList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            wdDeliverUpdate frmDelivery = new wdDeliverUpdate(false);
            frmDelivery.txtID.Text = dgDeli.CurrentRow.Cells["deliveryid"].Value.ToString();
            frmDelivery.txtName.Text = dgDeli.CurrentRow.Cells["deliveryname"].Value.ToString();
            frmDelivery.txtDeAddr.Text = dgDeli.CurrentRow.Cells["address"].Value.ToString();
            frmDelivery.txtDeTell.Text = dgDeli.CurrentRow.Cells["tell"].Value.ToString();

            frmDelivery.StartPosition = FormStartPosition.CenterScreen;
            frmDelivery.ShowDialog();
            getDeliList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this delivery?", "App Alert", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                dtb = new Database();
                string sql = "update mtdelivery set active = 0 where deliveryid = @deliveryid";
                bool result = dtb.SetValue(sql, CommandType.Text, ref err, new SqlParameter("@deliveryid", dgDeli.CurrentRow.Cells["deliveryid"].Value.ToString())

                    );
                if (result)
                {
                    MessageBox.Show("Delete Delivery successfully!");
                    getDeliList();
                }
                else
                {
                    MessageBox.Show("Fail to delete delivery");
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }
        DataTable dtTable;
        private void getDeliList()
        {
            dtb = new Database();
            try
            {
                string sql = @"select deliveryid, deliveryname, address,tell
                                    from mtdelivery 
                                    where active = 1";
                //conn = new SqlConnection(BaseClass.StrConnect);
                //conn.Open();
                //SqlCommand cmd = new SqlCommand();
                //cmd.Connection = conn;
                //cmd.CommandText = sql;
                //cmd.CommandType = CommandType.Text;
                //SqlDataReader  reader = cmd.ExecuteReader();
                //dtTable = new DataTable();
                //dtTable.Load(reader);
                //dgDeli.DataSource = dtTable;

                SqlDataReader dt = dtb.MyExecuteReader(sql, CommandType.Text, ref err);
                dtTable = new DataTable();
                dtTable.Load(dt);
                dgDeli.DataSource = dtTable;

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
                DataTable dtbFilter = dtTable.AsEnumerable().Where(
                                                                row => row.Field<String>("deliveryname").ToLower().Contains(txtSearch.Text.ToLower())
                                                                || row.Field<String>("deliveryname").ToLower().Contains(txtSearch.Text.ToLower())
                                                            )
                                                            .OrderByDescending(row => row.Field<String>("deliveryname"))
                                                            .CopyToDataTable();
                dgDeli.DataSource = dtbFilter;
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
