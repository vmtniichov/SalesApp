using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace DataLayer
{
    public class Database
    {

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataAdapter da;

        public string constring = "Data Source=.;Initial Catalog=BanHang;Integrated Security=True";

        public Database()
        {
            try
            {
                string connectring = constring;
                cnn = new SqlConnection(connectring);
                cmd = cnn.CreateCommand();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool kiemtraketnoi()
        {
            bool ketqua = false;
            try
            {
                cnn.Open();
                ketqua = true;
            }
            catch (Exception)
            {
                ketqua = false;
            }
            finally
            {
                cnn.Close();
            }
            return ketqua;
        }


        /// <summary>
        /// Phương thức lấy dữ liệu trả về một DataTable
        /// </summary>
        /// <param name="sql">Câu lệnh truy vấn hay tên thủ tục trong sql</param>
        /// <param name="ct">CommandType</param>
        /// <param name="err">Biến lưu trữ lỗi</param>
        /// <param name="param">Danh sách tham số truyền vào</param>
        /// <returns>Một đối tượng DataTable</returns>
        /// 

        public DataTable GetDataTable(string sql, CommandType ct, ref string err, params SqlParameter[] param)
        {
            DataTable dt = new DataTable();
            cnn.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = sql;
            cmd.CommandType = ct;
            cmd.Parameters.Clear();
            if (param != null)
            {
                foreach (SqlParameter p in param)
                    cmd.Parameters.Add(p);
            }
            try
            {
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                err = ex.Message;
            }
            finally
            {
                cnn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Phương thức lấy dữ liệu trả về một DataSet
        /// </summary>
        /// <param name="sql">Câu lệnh truy vấn hay tên thủ tục trong sql</param>
        /// <param name="ct">CommandType</param>
        /// <param name="err">Biến lưu trữ lỗi</param>
        /// <param name="param">Danh sách tham số truyền vào</param>
        /// <returns>Một đối tượng DataSet</returns>
        public DataSet GetDataSet(string sql, CommandType ct, ref string err, params SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            cnn.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = sql;
            cmd.CommandType = ct;
            cmd.Parameters.Clear();
            if (param != null)
            {
                foreach (SqlParameter p in param)
                    cmd.Parameters.Add(p);
            }
            try
            {
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (SqlException ex)
            {
                err = ex.Message;
            }
            finally
            {
                cnn.Close();
            }
            return ds;
        }
        /// <summary>
        /// Hàm cho lấy về một giá trị duy nhất trong sql sử dụng phương thức ExecuteScalar
        /// </summary>
        /// <param name="sql">Câu lệnh sql hay tên thủ tục nội tại của SQL</param>
        /// <param name="ct">đối tượng CommandType</param>
        /// <param name="err">Biến lưu trữ lỗi</param>
        /// <param name="param">Danh sách tham số cho thủ tục</param>
        /// <returns>một đối tượng object</returns>
        public object GetValue(string sql, CommandType ct, ref string err, params SqlParameter[] param)
        {
            object ketqua = null;
            try
            {
                cnn.Open();
                cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = ct;
                cmd.Parameters.Clear();
                if (param != null)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                ketqua = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            finally
            {
                cnn.Close();
            }
            return ketqua;

        }

        public bool SetValue(string sql, CommandType ct, ref string err, params SqlParameter[] param)
        {
            bool ketqua = false;
            try
            {
                cnn.Open();
                cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = ct;
                cmd.Parameters.Clear();
                if (param != null)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                cmd.ExecuteNonQuery();
                ketqua = true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                ketqua = false;
            }
            finally
            {
                cnn.Close();
            }
            return ketqua;
        }
        /// <summary>
        /// Lấy dữ liệu lên comboBox
        /// </summary>
        /// <param name="sql">Tên câu lệnh truy vấn hay tên thủ tục</param>
        /// <param name="ct">CommandType</param>
        /// <param name="cmb">Tên của comboBox</param>
        /// <param name="text">Giá trị hiển thị [string]</param>
        /// <param name="value">Giá trị nhận [string]</param>
        /// <param name="err">Lưu lỗi của phương thức</param>
        /// <returns>trả về trạng thái load comboBox sử dụng khi cần kiểm tra sự kiện selectindexchanged</returns>

        public bool fillCombo(string sql, CommandType ct, ComboBox cmb, string text, string value, ref string err, string hienthi)
        {

            bool trangthaiload = false;
            cnn.Open();
            cmd = new SqlCommand(sql, cnn);
            cmd.CommandType = ct;
            try
            {
                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    cmb.DataSource = dt;
                    cmb.DisplayMember = text;
                    cmb.ValueMember = value;
                    cmb.SelectedIndex = -1;
                    cmb.Text = hienthi;
                    trangthaiload = true;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            finally
            {
                cnn.Close();
            }
            return trangthaiload;
        }

        /// <summary>
        /// Phương thức trả về đối tượng SqlDataReader
        /// </summary>
        /// <param name="sql">Câu truy vấn hay tên thủ tục trong sql</param>
        /// <param name="ct">CommandType</param>
        /// <param name="err">biến lưu lỗi</param>
        /// <param name="param">Danh sách tham số truyền cho thủ tục nếu không có tham số thi để là null</param>
        /// <returns>Trả về đối tượng sqlDataReader</returns>
        public SqlDataReader MyExecuteReader(string sql, CommandType ct, ref string err, params SqlParameter[] param)
        {
            SqlDataReader _reader = null;
            try
            {
                cnn.Open();
                cmd = new SqlCommand(sql, cnn);
                cmd.CommandType = ct;
                cmd.Parameters.Clear();
                if (param != null)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                _reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return _reader;
        }
    }
}