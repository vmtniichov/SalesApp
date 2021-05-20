using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class BL_Employee
    {
        DL_Employee dlEmp = new DL_Employee();

        public DataTable Employee_GetList()
        {
            return dlEmp.Employee_GetList();
        }
    }
}
