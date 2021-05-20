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
    public partial class frmUserSetting : Form
    {

        public frmUserSetting()
        {
            InitializeComponent();
            try
            {
                txtUserList.Text = DisplayUserList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string DisplayUserList()
        {
            SqlConnection conn = new SqlConnection(BaseClass.StrConnect);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select*from MTUsers where active = 1";
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                string userlist = "";
                while (reader.Read())
                {
                    userlist = userlist + reader[0].ToString() + "  -  " + reader[2].ToString()
                               + Environment.NewLine;
                }
                return userlist;
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
        private void frmUserSetting_Load(object sender, EventArgs e)
        {
            cboExecute.Text = "/";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            frmLogin frmlogin = new frmLogin();
            frmlogin.Show();
            this.Hide();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SqlConnection cnn = null;
            try
            {
                cnn = new SqlConnection(BaseClass.StrConnect);
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "Select * from MTUsers where username like @username";
                cmd.CommandType = CommandType.Text;

                SqlParameter paraUsername = new SqlParameter();
                paraUsername.ParameterName = "@username";
                paraUsername.SqlValue = txtSearch.Text;
                cmd.Parameters.Add(paraUsername);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    txtUsername.Text = reader[0].ToString();
                    txtPassword.Text = reader[1].ToString();
                    txtFullname.Text = reader[2].ToString();
                    cboRole.Text = reader[5].ToString() == "U" ? "User" : "Administrator";
                }
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

        private void cboExecute_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboExecute.Text == "Add New")
            {
                txtUsername.Enabled = true;
                txtSearch.Enabled = false;
                btnSave.Enabled = true;
                btnSearch.Enabled = false;
                btnDelete.Enabled = false;
            }
            else if (cboExecute.Text == "Update")
            {
                txtSearch.Enabled = true;
                txtUsername.Enabled = false;
                btnSave.Enabled = true;
                btnSearch.Enabled = true;
                btnDelete.Enabled = false;
            }
            else if (cboExecute.Text == "Delete")
            {
                txtUsername.Enabled = false;
                txtSearch.Enabled = true;
                btnSave.Enabled = false;
                btnSearch.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                txtUsername.Enabled = false;
                btnSave.Enabled = false;
                btnSearch.Enabled = true;
                btnDelete.Enabled = false;
            }
            txtSearch.Text = "";
            txtFullname.Text = "";
            txtPassword.Text = "";
            txtUsername.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cboExecute.Text == "Add New")
            {
                SqlConnection conn = null;
                //insert...
                try
                {
                    conn = new SqlConnection(BaseClass.StrConnect);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"insert into MTUsers (username,password,fullname,modifieddate,active,role)
                                                      values(@username,@password,@fullname,@modifieddate,@active,@role)";
                    //Truyền username;
                    SqlParameter paraUsername = new SqlParameter();
                    paraUsername.ParameterName = "@username";
                    paraUsername.SqlValue = txtUsername.Text;
                    cmd.Parameters.Add(paraUsername);

                    //Truyền password;
                    SqlParameter paraPassword = new SqlParameter();
                    paraPassword.ParameterName = "@password";
                    paraPassword.SqlValue = txtPassword.Text;
                    cmd.Parameters.Add(paraPassword);
                    //Truyền fullname;
                    SqlParameter parafullname = new SqlParameter();
                    parafullname.ParameterName = "@fullname";
                    parafullname.SqlValue = txtFullname.Text;
                    cmd.Parameters.Add(parafullname);
                    //Truyền modifieddate;
                    SqlParameter paraModifieddate = new SqlParameter();
                    paraModifieddate.ParameterName = "@modifieddate";
                    paraModifieddate.SqlValue = DateTime.Now;
                    cmd.Parameters.Add(paraModifieddate);
                    //Truyền active;
                    SqlParameter paraActvie = new SqlParameter();
                    paraActvie.ParameterName = "@active";
                    paraActvie.SqlValue = 1;
                    cmd.Parameters.Add(paraActvie);
                    //Truyền Role;
                    SqlParameter paraRole = new SqlParameter();
                    paraRole.ParameterName = "@role";
                    paraRole.SqlValue = (cboRole.Text == "User") ? "U" : "A";
                    cmd.Parameters.Add(paraRole);

                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    int KQ = cmd.ExecuteNonQuery();
                    if (KQ > 0)
                    {
                        txtUserList.Text = DisplayUserList();
                    }
                    else
                    {
                        MessageBox.Show("Insert error!");
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
            if (cboExecute.Text == "Update")
            {
                //update...
                SqlConnection conn = null;
                //insert...
                try
                {
                    conn = new SqlConnection(BaseClass.StrConnect);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"update MTUsers set password=@password,
                                                           fullname=@fullname,
                                                           modifieddate=@modifieddate,
                                                           active=@active,
                                                           role = @role
                                               where username=@username";
                    //Truyền username;
                    SqlParameter paraUsername = new SqlParameter();
                    paraUsername.ParameterName = "@username";
                    paraUsername.SqlValue = txtUsername.Text;
                    cmd.Parameters.Add(paraUsername);
                    //Truyền password;
                    SqlParameter paraPassword = new SqlParameter();
                    paraPassword.ParameterName = "@password";
                    paraPassword.SqlValue = txtPassword.Text;
                    cmd.Parameters.Add(paraPassword);
                    //Truyền fullname;
                    SqlParameter parafullname = new SqlParameter();
                    parafullname.ParameterName = "@fullname";
                    parafullname.SqlValue = txtFullname.Text;
                    cmd.Parameters.Add(parafullname);
                    //Truyền modifieddate;
                    SqlParameter paraModifieddate = new SqlParameter();
                    paraModifieddate.ParameterName = "@modifieddate";
                    paraModifieddate.SqlValue = DateTime.Now;
                    cmd.Parameters.Add(paraModifieddate);
                    //Truyền active;
                    SqlParameter paraActvie = new SqlParameter();
                    paraActvie.ParameterName = "@active";
                    paraActvie.SqlValue = 1;
                    cmd.Parameters.Add(paraActvie);
                    //Truyền Role;
                    SqlParameter paraRole = new SqlParameter();
                    paraRole.ParameterName = "@role";
                    paraRole.SqlValue = (cboRole.Text == "User") ? "U" : "A";
                    cmd.Parameters.Add(paraRole);

                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    int KQ = cmd.ExecuteNonQuery();
                    if (KQ > 0)
                    {
                        txtUserList.Text = DisplayUserList();
                    }
                    else
                    {
                        MessageBox.Show("Update error!");
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
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(BaseClass.StrConnect);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"delete from mtUsers where username like @username";
                //Truyền username;
                SqlParameter paraUsername = new SqlParameter();
                paraUsername.ParameterName = "@username";
                paraUsername.SqlValue = txtUsername.Text;
                cmd.Parameters.Add(paraUsername);

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                int KQ = cmd.ExecuteNonQuery();
                if (KQ > 0)
                {
                    txtUserList.Text = DisplayUserList();
                }
                else
                {
                    MessageBox.Show("Delete error!");
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
    }
}