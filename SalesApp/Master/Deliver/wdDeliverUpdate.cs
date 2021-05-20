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
    public partial class wdDeliverUpdate : Form
    {
        Database dtb;
        string err = string.Empty;
        public wdDeliverUpdate()
        {
            InitializeComponent();
        }

        bool action;
        public wdDeliverUpdate(bool isAddNew)
        {
            InitializeComponent();
            action = isAddNew;
            if(isAddNew)
            {
                lblTitle.Text = "DELIVERY - ADD NEW";
            }
            else
            {
                lblTitle.Text = "DELIVERY - UPDATE";
            }
        }
        private string getNextID()
        {
            dtb = new Database();
            string ID = "";

            string sql = "select max(cast(deliveryid as int)) +1 from mtdelivery";
            var temp_id = dtb.GetValue(sql, CommandType.Text, ref err);
            ID = temp_id.ToString().PadLeft(10, '0');
            return ID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            dtb = new Database();
            if(action)
            {
                string sql = @"insert mtdelivery ([deliveryid], [deliveryname], [address], [tell], [modifieddate], [username], [active]) 
                                        values (@deliveryid, @deliveryname, @address, @tell, @modifieddate, @username, 1)";
                SqlParameter[] param = new SqlParameter[] { 
                    new SqlParameter("@deliveryid", getNextID()),
                    new SqlParameter("@deliveryname", txtName.Text),
                    new SqlParameter("@address", txtDeAddr.Text),
                    new SqlParameter("@tell",txtDeTell.Text),
                    new SqlParameter("@modifieddate",DateTime.Now),
                    new SqlParameter("@username",BaseClass.username),
                };
                bool result = dtb.SetValue(sql, CommandType.Text, ref err, param);
                if(result){
                    MessageBox.Show("Add new Delivery Successfully!");
                }
                else MessageBox.Show("Failed to add new Delivery!");
            }
            else
            {
                string sql = @"update mtdelivery 
                                                set deliveryname = @deliveryname, 
                                                address = @address, 
                                                tell = @tell, 
                                                modifieddate = @modifieddate, 
                                                username = @username, active = 1
                                        where deliveryid = @deliveryid";
                SqlParameter[] param = new SqlParameter[] { 
                    new SqlParameter("@deliveryid", txtID.Text),
                    new SqlParameter("@deliveryname", txtName.Text),
                    new SqlParameter("@address", txtDeAddr.Text),
                    new SqlParameter("@tell",txtDeTell.Text),
                    new SqlParameter("@modifieddate",DateTime.Now),
                    new SqlParameter("@username",BaseClass.username),
                };
                bool result = dtb.SetValue(sql, CommandType.Text, ref err, param);
                if (result)
                {
                    MessageBox.Show("Updated Delivery Successfully!");
                }
                else MessageBox.Show("Failed to UPDATED Delivery!");
            }
        }
    }
}
