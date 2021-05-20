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

namespace SalesApp.Master.Employee
{
    public partial class wdEmployeeUpdate : Form
    {

        public wdEmployeeUpdate()
        {
            InitializeComponent();
        }
        bool action;
        public wdEmployeeUpdate(bool isAddNew)
        {
            action = isAddNew;
            InitializeComponent();

            LoadPosition();
            if(isAddNew==true)
            {
                lblTitle.Text = "EMPLOYEE - ADD NEW";
            }
            else
            {
                lblTitle.Text = "EMPLOYEE - UPDATE";
            }
        }

        private void LoadPosition()
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(BaseClass.StrConnect);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select* from mtposition where active = 1";
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dtTab = new DataTable();
                dtTab.Load(reader);
                cboPosition.DataSource = dtTab;
                cboPosition.DisplayMember = "positionname";
                cboPosition.ValueMember = "positionid";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        private void LoadPositionList()
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(BaseClass.StrConnect);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select * from MTPosition where active = 1";
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                //Đổ dự liệu vào dtTable nắm giữ
                DataTable dtTable = new DataTable();   //bảng chứa dữ liệu lấy từ câu select 
                dtTable.Load(reader);                  //Tải dữ liệu từ DB lên bảng
                //Show dữ liệu lên combobox
                cboPosition.DataSource = dtTable;      //Gắn dữ liệu cho Combobox
                cboPosition.DisplayMember = "positionname"; //Chỉ định hiển thị tên vị trí
                cboPosition.ValueMember = "positionid";     //Giá trị ẩn của Combobox
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
        private string GetEmpID()
        {
            string EmID = "";
            string err = string.Empty;
            Database dtb = new Database();
            try
            {
                string cmd = "select max(cast(empid as int)) +1 from mtemployee";

                object id = dtb.GetValue(cmd, CommandType.Text, ref err);
                EmID = id.ToString().PadLeft(10, '0');

                return EmID;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Database dtb = new Database();
            string err = string.Empty;
            if (action == true)
                {
                    try
                    {
                        string cmd = @"insert into mtemployee (empid, empname, tell, idcard, idcarddate, address, positionid, active, username, modifieddate)
                                                      values(@empid, @paraEmpname, @paratell, @idcard, @idcarddate, @address, @positionid, @active, @username, @modifieddate)";
                        bool kq = dtb.SetValue(cmd, CommandType.Text, ref err,
                                                                                new SqlParameter("@empid", GetEmpID()),
                                                                                new SqlParameter("@paraEmpname", txtEmpName.Text),
                                                                                new SqlParameter("@paratell", txtTell.Text),
                                                                                new SqlParameter("@idcard", txtIDCard.Text),
                                                                                new SqlParameter("@idcarddate", txtIDDate.Value),
                                                                                new SqlParameter("@address", txtAddr.Text),
                                                                                new SqlParameter("@positionid", cboPosition.SelectedValue.ToString()),
                                                                                new SqlParameter("@active", 1),
                                                                                new SqlParameter("@username", BaseClass.username),
                                                                                new SqlParameter("@modifieddate", DateTime.Now)
                                                                                );
                        if (kq)
                        {
                            if (txtEmpName.Text != "")
                            {
                                MessageBox.Show("Add Employee Successfully!");
                            }
                            else MessageBox.Show("Please check again");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }
                else
                {
                    string cmd = @"update MTEmployee set empname=@paraEmpname, 
                                                            tell=@paratell, 
                                                            idcard=@idcard, 
                                                            idcarddate=@idcarddate, 
                                                            address=@address, 
                                                            positionid=@positionid,
                                                            username=@username,
                                                            modifieddate=@modifieddate
			                            where empid=@empid";
                    try{
                        bool kq = dtb.SetValue(cmd, CommandType.Text, ref err,
                                                                               new SqlParameter("@empid", txtID.Text),
                                                                               new SqlParameter("@paraEmpname", txtEmpName.Text),
                                                                               new SqlParameter("@paratell", txtTell.Text),
                                                                               new SqlParameter("@idcard", txtIDCard.Text),
                                                                               new SqlParameter("@idcarddate", txtIDDate.Value),
                                                                               new SqlParameter("@address", txtAddr.Text),
                                                                               new SqlParameter("@positionid", cboPosition.SelectedValue.ToString()),
                                                                               new SqlParameter("@active", 1),
                                                                                new SqlParameter("@username", BaseClass.username),
                                                                               new SqlParameter("@modifieddate", DateTime.Now));

                        if (kq)
                        {
                            if (txtEmpName.Text != "")
                            {

                                MessageBox.Show("Update Employee Infomation Successfully!");
                            }
                            else MessageBox.Show("Please check again");
                        }
                    }
                     catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                   
                }
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        }

    }
