using SalesApps;
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

namespace SalesApp
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            txtUsername.GotFocus += TxtUsername_GotFocus;
            txtPassword.GotFocus += TxtPassword_GotFocus;
            txtUsername.Text = "admin1";
            txtPassword.Text = "123457";
        }

        private void TxtPassword_GotFocus(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }

        private void TxtUsername_GotFocus(object sender, EventArgs e)
        {
            txtUsername.SelectAll();
        }


        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            bool LoginSuccess = false; //Tạo biển kiểm tra tài khoản
            LoginSuccess = CheckAccount("%");  //Thực thi hàm kiểm tra tài khoản
            if (LoginSuccess == true)  //trả về true nếu có tài khoản
            {
                BaseClass.username = txtUsername.Text;
                frmMainForm frmMain = new frmMainForm();
                frmMain.Show();
                this.Hide();
            }
            else  //ngược lại không có tài khoản
            {
                MessageBox.Show("Login Failed");
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private bool CheckAccount(string role)
        {
            //Viết hàm kiểm tra tài khoản

            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(BaseClass.StrConnect);
                conn.Open();
                //Lệnh kết nối
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"select username 
                                    from mtusers 
                                    where username = @username and
                                          password = @password and
                                          role like @role";
                cmd.CommandType = CommandType.Text;

                //cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = txtUsername.Text;
                SqlParameter paraUsername = new SqlParameter();//Khai báo đối tượng SqlParameter
                paraUsername.ParameterName = "@username";
                paraUsername.SqlValue = txtUsername.Text;
                cmd.Parameters.Add(paraUsername);

                //cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = txtPassword.Text;
                SqlParameter paraPassword = new SqlParameter();//Khai báo đối tượng SqlParameter
                paraPassword.ParameterName = "@password";
                paraPassword.SqlValue = txtPassword.Text;
                cmd.Parameters.Add(paraPassword);

                SqlParameter paraRole = new SqlParameter();//Khai báo đối tượng SqlParameter
                paraRole.ParameterName = "@role";
                paraRole.SqlValue = role;
                cmd.Parameters.Add(paraRole);

                object KQ = cmd.ExecuteScalar();
                if (KQ == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool LoginSuccess = false; //Tạo biển kiểm tra tài khoản
            LoginSuccess = CheckAccount("A");  //Thực thi hàm kiểm tra tài khoản
            if (LoginSuccess == true)  //trả về true nếu có tài khoản
            {
                frmUserSetting frmuser = new frmUserSetting();
                frmuser.Show();
                this.Hide();
            }
            else  //ngược lại không có tài khoản
            {
                MessageBox.Show("Login failed!!");
            }
        }

        internal static object GetUsername()
        {
            throw new NotImplementedException();
        }
    }
}