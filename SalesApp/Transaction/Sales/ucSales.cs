using DataLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SalesApp.Transaction.Sales
{
    public partial class ucSales : UserControl
    {
        Database dtb;
        string err = string.Empty;
        private DataTable tablelist;
        private DataTable menulist;
        private DataTable dtable;
        private DataTable productlist;
        public ucSales()
        {
            InitializeComponent();
            LoadTableList();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            BaseClass.LoadUC((Panel)Parent, new ucMenu());
        }

        
        #region Sự kiện của các nút bấm
        //Dùng để tạo bàn mới trong database
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            dtb = new Database();
            try
            {
                int idtable = GetNextTableID();
                string sql = @"insert mttable(id,tablename, 
                                                active,
                                                username,
                                                modifieddate,
                                                isused)
                            values(@ID,@tablename, 1, @username, @modifieddate, 0)";
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID",idtable),
                    new SqlParameter("@tablename",idtable),
                    new SqlParameter("@username",BaseClass.username),
                    new SqlParameter("@modifieddate",DateTime.Now)
                };
                bool result = dtb.SetValue(sql, CommandType.Text, ref err, param);
                if (result)
                {
                    MessageBox.Show("Added New Table!");
                    LoadTableList();
                }
                else
                {
                    MessageBox.Show("Error!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        //Cập nhật amount và discount amount
        private void btnUpdSOHD_Click(object sender, EventArgs e)
        {
            dtb = new Database();
            string query = @"update sohd set discountpercent = @discountpercent, 
                            discountamount = @discountamount,
                            amount = @amount, 
                            modifieddate = @modifieddate, 
                            username = @username
                            where soid = @soid";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@discountpercent",txtPercent.Text),
                new SqlParameter("@discountamount",lblDiscountAmount.Text),
                new SqlParameter("@amount",txtAmount.Text),
                new SqlParameter("@modifieddate",DateTime.Now),
                new SqlParameter("@username",BaseClass.username),
                new SqlParameter("@soid",txtSOID.Text),
            };
            bool result = dtb.SetValue(query, CommandType.Text, ref err, param);
            if (result)
            {
                MessageBox.Show("Update successfully!");
            }
        }

        //Hoàn tất in bill và reset lại isused và SOID của bàn
        private void btnPrintBill_Click(object sender, EventArgs e)
        {
            ConfirmPrintBillForm frm = new ConfirmPrintBillForm();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.txtAmount.Text = txtAmount.Text;
            frm.txtDiscountAmount.Text = lblDiscountAmount.Text == "" ? "0" : lblDiscountAmount.Text;
            frm.txtSOID.Text = txtSOID.Text;
            frm.lblTableID.Text = txtTableID.Text;
            frm.txtPercent.Text = txtPercent.Text == "" ? "0": txtPercent.Text;
            frm.txtNetAmount.Text = txtNetA.Text == "" ? txtAmount.Text : Convert.ToDouble(txtNetA.Text).ToString();
            frm.ShowDialog();

            btnBack.Visible = false;
            btnPrintBill.Visible = false;
            btnBillDel.Visible = false;
            btnUpdSOHD.Visible = false;
            lblQuantity.Visible = false;
            txtQuantity.Visible = false;

            txtPercent.ReadOnly = true;
            dgProductMask.Visible = true;
            cboPGMask.Visible = true;
            btnDeleteTable.Visible = true;
            btnAddTable.Visible = true;
            foreach (Control ctr in groupOrder.Controls)
            {
                if (ctr is TextBox)
                {
                    ctr.Text = "";
                }
            }
            PanelItemList.Controls.Clear();
            LoadTableList();
            
        }

        //Button danh sách bàn khi click hiển thị menu
        private void Newbutton_Click(object sender, EventArgs e)
        {
            //Dùng để tạo hóa đơn mới
            #region DISPLAY

            Button btn = (Button)sender;
            txtTableID.Text = btn.Name.Split('_')[0];

            PanelItemList.Visible = true;
            btnBillDel.Visible = true;
            btnBack.Visible = true;
            btnUpdSOHD.Visible = true;
            btnPrintBill.Visible = true;
            txtQuantity.Visible = true;
            lblQuantity.Visible = true;
            txtPercent.ReadOnly = false;

            btnAddTable.Visible = false;
            btnDeleteTable.Visible = false;
            cboPGMask.Visible = false;

            LoadPG();
            PanelItemList.Controls.Clear();
            LoadProList(cboPG.SelectedValue.ToString());

            #endregion


            string isused = btn.Name.Split('_')[1];
            string currentSOID = btn.Name.Split('_')[2];
            //Thuộc tính Name của button gồm 3 phần tableID_Price_SOID
            if (isused == "0")
            {
                dgProductMask.Visible = true;
                dtb = new Database();
                string createSOHD_query = @"INSERT SOHD([soid], [tableid], [sodate], [modifieddate], [username], [printbill], [isdelete]) 
                                                VALUES (@soid, @tableid, @sodate, @modifieddate, @username, 0, 0)";
                string soid = GetNextSOID();
                SqlParameter[] param = new SqlParameter[]
                {
                        new SqlParameter("@soid",soid),
                        new SqlParameter("@tableid",txtTableID.Text),
                        new SqlParameter("@sodate",DateTime.Now.ToShortDateString()),
                        new SqlParameter("@modifieddate",DateTime.Now),
                        new SqlParameter("@username",BaseClass.username),
                };
                dtb = new Database();
                bool add_HD_success = dtb.SetValue(createSOHD_query, CommandType.Text, ref err, param);
                if (add_HD_success)
                {
                    string update_query = "update mttable set isused = 1,soid = @soid where id=@id";
                    bool result = dtb.SetValue(update_query, CommandType.Text, ref err, new SqlParameter("@id", txtTableID.Text), new SqlParameter("@soid", soid));
                    txtSOID.Text = soid;
                    Display_SODT_Detail(soid);
                }
            }
            else if (btn.Name.Split('_')[1] == "1")
            {

                dgProductMask.Visible =false;
                //TODO Load SODT và thông tin hóa đơn lên datagidview
                Display_SODT_Detail(currentSOID);
                txtAmount.Text = LoadSOHD_Amount(currentSOID);
                SqlDataReader reader = GetAmountInfo(currentSOID);
                while (reader.Read())
                {
                    txtPercent.Text = reader["discountpercent"].ToString();
                    lblDiscountAmount.Text = reader["discountamount"].ToString();
                }
                txtSOID.Text = btn.Name.Split('_')[2];
            }
            groupOrder.Refresh();
        }

        //Button product dùng để thêm product vào SODT
        private void Prod_Click(object sender, EventArgs e)
        {
            /*
             TODO khi click sẽ add thông tin product vào SODT
             */
            Button btn = (Button)sender;
            //btn.Name gồm productid_price
            dgProductMask.Visible = false;

            txtPercent.Text = txtPercent.Text == "" ? "0" : txtPercent.Text;
            string productid = btn.Name.Split('_')[0];
            string price = btn.Name.Split('_')[1];
            try
            {
                if (txtQuantity.Text == "")
                {
                    string query = @"INSERT SODT ([id], [soid], [productid], [price], [username], [modifieddate]) 
                                            VALUES (@id, @soid, @productid, @price, @username, @modifieddate)";
                    SqlParameter[] param = new SqlParameter[] {
                        new SqlParameter("@id", Get_Next_ID_of_SOID()),
                        new SqlParameter("@soid", txtSOID.Text),
                        new SqlParameter("@productid", productid),
                        new SqlParameter("@price", price),
                        new SqlParameter("@username", BaseClass.username),
                        new SqlParameter("@modifieddate", DateTime.Now),
                    };
                    dtb = new Database();
                    bool result = dtb.SetValue(query, CommandType.Text, ref err, param);
                    if (result)
                    {
                        Display_SODT_Detail(txtSOID.Text);
                    }
                }
                else
                {
                    for (int i = 0; i < Convert.ToInt32(txtQuantity.Text); i++)
                    {
                        string query = @"INSERT SODT ([id], [soid], [productid], [price], [username], [modifieddate]) 
                                            VALUES (@id, @soid, @productid, @price, @username, @modifieddate)";
                        SqlParameter[] param = new SqlParameter[] {
                        new SqlParameter("@id", Get_Next_ID_of_SOID()),
                        new SqlParameter("@soid", txtSOID.Text),
                        new SqlParameter("@productid", productid),
                        new SqlParameter("@price", price),
                        new SqlParameter("@username", BaseClass.username),
                        new SqlParameter("@modifieddate", DateTime.Now),
                    };
                        dtb = new Database();
                        bool result = dtb.SetValue(query, CommandType.Text, ref err, param);
                        if (result)
                        {
                            Display_SODT_Detail(txtSOID.Text);
                        }
                    }
                }
            }
            catch { }
            finally
            {
                txtAmount.Text = LoadSOHD_Amount(txtSOID.Text);
            }




            //MessageBox.Show("Mai add product vào SODT với ID khác, SOHD giống nhau,tạo 1 cái HD sau khi bấm order gồm tableid, \nsoid tự động tăng \n discount bằng null cập nhật sau khi in bill inbill sẽ cập nhật tên khách hàng \n và deli vào SOHD \nvà chọn nhân viên in bill");
        }

        //Trở lại danh sách bàn
        private void btnBack_Click(object sender, EventArgs e)
        {
            
            
            btnBack.Visible = false;
            btnPrintBill.Visible = false;
            btnBillDel.Visible = false;
            btnUpdSOHD.Visible = false;
            lblQuantity.Visible = false;
            txtQuantity.Visible = false;

            txtPercent.ReadOnly = true;
            dgProductMask.Visible = true;
            cboPGMask.Visible = true;
            btnDeleteTable.Visible = true;
            btnAddTable.Visible = true;
            foreach (Control ctr in groupOrder.Controls)
            {
                if (ctr is TextBox)
                {
                    ctr.Text = "";
                }
            }
            PanelItemList.Controls.Clear();
            LoadTableList();

        }

        private void btnBillDel_Click(object sender, EventArgs e)
        {
            dtb = new Database();
            string query = @"update sohd set isdelete = 1 where soid = @soid";
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this bill?", "App Alert", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool result = dtb.SetValue(query, CommandType.Text, ref err, new SqlParameter("@soid", txtSOID.Text));
                if (result)
                {

                    string update_query = "update mttable set isused = 0, soid = @soid where id=@id";
                    bool default_table = dtb.SetValue(update_query, CommandType.Text, ref err, new SqlParameter("@id", txtTableID.Text), new SqlParameter("@soid", txtSOID.Text));
                    if (default_table)
                    {
                        btnBack.Visible = false;
                        btnPrintBill.Visible = false;
                        btnBillDel.Visible = false;
                        btnUpdSOHD.Visible = false;
                        lblQuantity.Visible = false;
                        txtQuantity.Visible = false;

                        txtPercent.ReadOnly = true;
                        dgProductMask.Visible = true;
                        cboPGMask.Visible = true;
                        btnDeleteTable.Visible = true;
                        btnAddTable.Visible = true;
                        foreach (Control ctr in groupOrder.Controls)
                        {
                            if (ctr is TextBox)
                            {
                                ctr.Text = "";
                            }
                        }
                        PanelItemList.Controls.Clear();
                        LoadTableList();

                        MessageBox.Show("Deleted Bill!");
                    }

                }

            }
        }

        #endregion



        /*
         * Phần này chứa các hàm dùng để lấy ID
         * và lấy dữ liệu từ database để hiển thị lên app
         */
        #region Các hàm lấy ID và hiển thị dữ liệu
        //Display dữ liệu của PriceGroup lên combobox
        private void LoadPG()
        {
            dtb = new Database();
            string sql = "select * from pricegroup where active = 1 and isused =1";
            SqlDataReader reader = dtb.MyExecuteReader(sql, CommandType.Text, ref err);
            dtable = new DataTable();
            dtable.Load(reader);
            cboPG.DataSource = dtable;
            cboPG.DisplayMember = "pricegroupname";
            cboPG.ValueMember = "pricegroupid";
        }

        //Trả về ID của bàn tiếp theo sẽ đc tạo trong database
        private int GetNextTableID()
        {
            dtb = new Database();
            string sql = "select max(cast(id as int))+1 from mttable";
            object result = dtb.GetValue(sql, CommandType.Text, ref err);
            return Convert.ToInt32(result);
        }

        //Trả về số ID của bàn đang chọn
        private int GetTable_SoID(string tableID)
        {

            dtb = new Database();
            string query = @"select soid from mttable where isused = 1 and id = @id";
            object result = dtb.GetValue(query, CommandType.Text, ref err, new SqlParameter("@id", tableID));
            return Convert.ToInt32(result);

        }

        //Dùng để tạo HD mới
        private string GetNextSOID()
        {

            dtb = new Database();
            string sql = "select max(cast(soid as int))+1 from SOHD";
            object result = dtb.GetValue(sql, CommandType.Text, ref err);
            string id = result.ToString().PadLeft(10, '0');
            return id;
        }

        //Dùng để thêm SODT mới vào SOHD CÓ SẴN
        private string Get_Next_ID_of_SOID()
        {
            string query = @"if exists (select * from sodt where soid = @soid) begin select max(cast(id as int))+1 from SODT where soid = @soid end else begin select 1 end
";
            dtb = new Database();
            object result = dtb.GetValue(query, CommandType.Text, ref err, new SqlParameter("@soid", txtSOID.Text));
            string id = result.ToString().PadLeft(2, '0');
            return id;
        }

        //Hàm dùng để gọi dữ liệu load lên dgProduct
        private void Display_SODT_Detail(string soid)
        {
            string query = @"select productname,count(dt.productid) as quantity,price,sum(price) as total, dt.productid,soid
                            from sodt dt join mtproduct pro on dt.productid = pro.productid
                            where dt.productid in(select distinct productid from sodt) and soid = @soid
                            group by dt.productid,price,productname,soid order by dt.productid";

            dtb = new Database();
            productlist = new DataTable();
            SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err, new SqlParameter("@soid", soid));
            if (reader.HasRows)
            {
                productlist.Load(reader);
                dgProduct.DataSource = productlist;
            }

        }

        //Hiển thị menu dưới dạng button sau khi nhấp vào 1 bàn
        private void LoadProList(string pgid)
        {

            try
            {
                dtb = new Database();
                string query = @"select pro.productname, pro.productid, pri.price 
                                from pricedetail pri join mtproduct pro on pri.productid = pro.productid join pricegroup pg on pri.pricegroupid = pg.pricegroupid
                                where pg.pricegroupid = @pricegroupid";

                SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err, new SqlParameter("@pricegroupid", pgid));
                menulist = new DataTable();
                menulist.Load(reader);
                int locX = 0, locY = 0;
                for (int i = 0; i < menulist.Rows.Count; i++)
                {
                    Button prod = new Button();
                    if (i % 3 == 0)
                    {
                        locX = 0;
                    }
                    else if (i % 3 == 1)
                    {
                        locX = 190;
                    }
                    else
                    {
                        locX = 380;
                    }

                    if (i % 3 == 0 && i != 0)
                    {
                        locY += 80;
                    }

                    prod.Location = new Point(locX, locY);
                    prod.BackColor = Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(128)))));
                    prod.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    prod.Name = menulist.Rows[i]["productid"].ToString() + "_" + menulist.Rows[i]["price"].ToString();
                    prod.Size = new Size(185, 78);
                    prod.TabIndex = 1;
                    prod.Text = menulist.Rows[i]["productname"].ToString() + "\n" + menulist.Rows[i]["price"].ToString();
                    prod.UseVisualStyleBackColor = false;
                    prod.Click += Prod_Click;
                    PanelItemList.Controls.Add(prod);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        //Hàm dùng để lấy ra danh sách bàn từ database
        private void LoadTableList()
        {
            try
            {
                dtb = new Database();
                string query = "select * from mttable where active=1";
                SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err);
                tablelist = new DataTable();
                tablelist.Load(reader);
                int locX = 0, locY = 0;
                for (int i = 0; i < tablelist.Rows.Count; i++)
                {
                    Button newbutton = new Button();
                    if (i % 3 == 0)
                    {
                        locX = 0;
                    }
                    else if (i % 3 == 1)
                    {
                        locX = 190;
                    }
                    else
                    {
                        locX = 380;
                    }

                    if (i % 3 == 0 && i != 0)
                    {
                        locY += 80;
                    }

                    newbutton.Location = new Point(locX, locY);
                    newbutton.Name = tablelist.Rows[i]["id"].ToString() + "_" + tablelist.Rows[i]["isused"].ToString() + "_" + tablelist.Rows[i]["soid"].ToString();
                    newbutton.BackColor = tablelist.Rows[i]["isused"].ToString() == "1" ? Color.MediumSpringGreen : Color.Crimson;
                    newbutton.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    newbutton.Size = new Size(185, 78);
                    newbutton.TabIndex = 1;
                    newbutton.Text = "Bàn " + tablelist.Rows[i]["tablename"].ToString();
                    newbutton.UseVisualStyleBackColor = false;
                    newbutton.Click += Newbutton_Click;
                    PanelItemList.Controls.Add(newbutton);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Dùng để lấy discountpercent và discountamount
        private SqlDataReader GetAmountInfo(string soid)
        {
            dtb = new Database();
            string query = @"select discountamount,discountpercent from sohd where soid = @soid";
            SqlDataReader reader = dtb.MyExecuteReader(query, CommandType.Text, ref err, new SqlParameter("@soid", soid));
            return reader;

        }

        //Lấy amount
        private string LoadSOHD_Amount(string soid)
        {
            string amount = string.Empty;

            dtb = new Database();
            string query = @"select sum(price) as amount_total from sodt where soid = @soid";
            object result = dtb.GetValue(query, CommandType.Text, ref err, new SqlParameter("@soid", soid));
            amount = result.ToString();

            return amount;
        }



        #endregion

        private void cboPG_SelectedValueChanged(object sender, EventArgs e)
        {
            PanelItemList.Controls.Clear();
            LoadProList(cboPG.SelectedValue.ToString());
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            FormDeleteTable frm = new FormDeleteTable();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            PanelItemList.Controls.Clear();
            LoadTableList();
        }

        

        private void txtPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtPercent_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPercent.Text != "")
                {
                    double dis_percent = Convert.ToDouble(txtPercent.Text);
                    double amount = Convert.ToDouble(txtAmount.Text);
                    double dis_amount = (dis_percent * amount) / 100;
                    double net_amount = Convert.ToDouble(txtAmount.Text) - dis_amount;
                    lblDiscountAmount.Text = dis_amount.ToString();
                    txtNetA.Text = net_amount.ToString().PadLeft(10, '0');
                }
                else
                {
                    double dis_percent = 0;
                    double amount = Convert.ToDouble(txtAmount.Text);
                    double dis_amount = (dis_percent * amount) / 100;
                    double net_amount = Convert.ToDouble(txtAmount.Text) - dis_amount;
                    lblDiscountAmount.Text = dis_amount.ToString();
                    txtNetA.Text = net_amount.ToString().PadLeft(10, '0');
                }
            }

            catch { }
        }

       

        

        private void dgProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dgProduct.Columns["deleteproduct"].Index)
            {
                dtb = new Database();
                if(dgProduct.CurrentRow.Cells["deletequantity"].Value == null)
                {
                    string query = @"delete sodt from sodt where id = (select max(cast(id as int)) from sodt where soid =@soid) and soid = @soid";
                    bool result = dtb.SetValue(query, CommandType.Text, ref err, new SqlParameter("@soid", txtSOID.Text));
                    if (result)
                    {
                        MessageBox.Show("Delete product successfully!");
                        txtAmount.Text = LoadSOHD_Amount(txtSOID.Text);
                        Display_SODT_Detail(txtSOID.Text);
                    }
                }
                else
                {
                    try
                    {
                        int quantity = Convert.ToInt32(dgProduct.CurrentRow.Cells["deletequantity"].Value);
                        int real_quantity = Convert.ToInt32(dgProduct.CurrentRow.Cells["quantity"].Value);
                        if (quantity > real_quantity)
                        {
                            MessageBox.Show("Please enter quantity again!");
                        }
                        else
                        {
                            bool result= false;
                            for (int i = 0; i < quantity; i++) {
                                string query = @"delete sodt from sodt where id = (select max(cast(id as int)) from sodt where soid =@soid) and soid = @soid";
                                result = dtb.SetValue(query, CommandType.Text, ref err, new SqlParameter("@soid", txtSOID.Text));
                            }
                            if (result)
                            {
                                MessageBox.Show("Delete product successfully!");
                                txtAmount.Text = LoadSOHD_Amount(txtSOID.Text);
                                Display_SODT_Detail(txtSOID.Text);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Please enter a number to delete and it must less than quantity!");
                    }
                }
            }
        }

        private void dgProductMask_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtAmount.Text != "")
                {
                    double amount = Convert.ToDouble(txtAmount.Text);
                    double percent = Convert.ToDouble(txtPercent.Text);
                    double net_a = amount - amount * (percent / 100);
                    txtNetA.Text = net_a.ToString().PadLeft(10,'0');
                }
                else
                {
                    
                }
            }

            catch { }
        }

        
    }
}
