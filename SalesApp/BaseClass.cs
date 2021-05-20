using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SalesApp
{
    public static class BaseClass
    {
        public static string username = "";
        public static string StrConnect = "Data Source=.;Initial Catalog=BanHang;Integrated Security=True";
        public static void LoadUC(Panel nen, UserControl UControl)
        {
            nen.Controls.Clear();
            UControl.Top = (nen.Height - UControl.Height) / 2;
            UControl.Left = (nen.Width - UControl.Width) / 2;
            nen.Controls.Add(UControl);
            UControl.Dock = DockStyle.Fill;
        }

    }
}