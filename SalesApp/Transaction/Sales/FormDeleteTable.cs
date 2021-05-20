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

namespace SalesApp.Transaction.Sales
{
    public partial class FormDeleteTable : Form
    {
        public FormDeleteTable()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult message = MessageBox.Show("Are you sure to delete this table?", "Confirm Delete Message", MessageBoxButtons.YesNo);
            if (message == DialogResult.Yes) {
                Database dtb = new Database();
                string err = string.Empty;
                string query = "update mttable set active = 0 where id = @id";
                bool result = dtb.SetValue(query, CommandType.Text, ref err, new SqlParameter("@id", txtTableID.Text));
                if (result)
                {
                    MessageBox.Show("Deleted table!");
                }
            }

        }
    }
}
