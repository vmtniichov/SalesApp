using BussinessLayer;
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
namespace SalesApp.Master.Employee
{
    public partial class ucEmployeeList : UserControl
    {
        Database dtbCon = new Database();
        private string err = "";
        public ucEmployeeList()
        {
            InitializeComponent();
            Employee_GetList();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucMenu());
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            wdEmployeeUpdate frmEmpNew = new wdEmployeeUpdate(true);
            frmEmpNew.StartPosition = FormStartPosition.CenterScreen;
            frmEmpNew.ShowDialog();

            Employee_GetList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            wdEmployeeUpdate frmEmpNew = new wdEmployeeUpdate(false);

            frmEmpNew.txtID.Text = dgEmpList.CurrentRow.Cells["empid"].Value.ToString();
            frmEmpNew.txtEmpName.Text = dgEmpList.CurrentRow.Cells["empname"].Value.ToString();
            frmEmpNew.txtIDCard.Text = dgEmpList.CurrentRow.Cells["idcard"].Value.ToString();
            frmEmpNew.txtIDDate.Text = dgEmpList.CurrentRow.Cells["idcarddate"].Value.ToString();
            frmEmpNew.txtTell.Text = dgEmpList.CurrentRow.Cells["tell"].Value.ToString();
            frmEmpNew.txtAddr.Text = dgEmpList.CurrentRow.Cells["address"].Value.ToString();
            frmEmpNew.cboPosition.Text = dgEmpList.CurrentRow.Cells["positionname"].Value.ToString();
            string strIDdate = dgEmpList.CurrentRow.Cells["idcarddate"].Value.ToString();
            frmEmpNew.txtIDDate.Value = DateTime.ParseExact(strIDdate, "M/d/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
            frmEmpNew.StartPosition = FormStartPosition.CenterScreen;
            frmEmpNew.ShowDialog();
            Employee_GetList();
        }
        DataTable dtTable;
        private void Employee_GetList()
        {
            try
            {
                string cmd = @"select empid, 
                                        empname, 
                                        address,
                                        tell,
                                        idcard,
                                        idcarddate,
                                        pos.positionname 
                                        from mtemployee emp inner join mtposition pos on emp.positionid = pos.positionid 
                                where emp.active = 1";

                dtTable = dtbCon.GetDataTable(cmd, CommandType.Text, ref err);
                dgEmpList.DataSource = dtTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this Employee?", "App Alert", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string ID = dgEmpList.CurrentRow.Cells["empid"].Value.ToString();
                string cmd = @"update mtemployee set active = 0, username = @username, modifieddate = @modifieddate where empid=@id";
                try
                {
                    bool kq = dtbCon.SetValue(cmd, CommandType.Text, ref err, new SqlParameter("@username", BaseClass.username),
                                                                               new SqlParameter("modifieddate", DateTime.Now),
                                                                               new SqlParameter("@id", ID));
                    Employee_GetList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtbFilter = dtTable.AsEnumerable().Where(
                                                                row => row.Field<String>("empname").ToLower().Contains(txtSearch.Text.ToLower())
                                                                || row.Field<String>("empid").ToLower().Contains(txtSearch.Text.ToLower())
                                                            )
                                                            .OrderByDescending(row => row.Field<String>("empname"))
                                                            .CopyToDataTable();
                dgEmpList.DataSource = dtbFilter;
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}