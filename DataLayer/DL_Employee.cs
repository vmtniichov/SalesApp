using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_Employee:Database
    {
        string error = "";
        public DataTable Employee_GetList()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = GetDataTable("Select*from mtemployee", CommandType.Text, ref error);
                return dt;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
