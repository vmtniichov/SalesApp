using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_Employee
    {
        private string empid;
        private string empname;
        private string tell;
        private string idcard;
        private string address;
        private string positionid;
        private bool active;
        private string username;
        private DateTime modifieddate;

        public string Empname { get => empname; set => empname = value; }
        public string Empid { get => empid; set => empid = value; }
        public string Tell { get => tell; set => tell = value; }
        public string Idcard { get => idcard; set => idcard = value; }
        public string Address { get => address; set => address = value; }
        public string Positionid { get => positionid; set => positionid = value; }
        public bool Active { get => active; set => active = value; }
        public string Username { get => username; set => username = value; }
        public DateTime Modifieddate { get => modifieddate; set => modifieddate = value; }

        public DTO_Employee()
        {

        }

        public DTO_Employee(string empid, string empname, string tell,string idcard, string address,string position,bool active,string username,DateTime modifieddate)
        {
            this.empid = empid;
            this.empname = empname;
            this.tell = tell;
            this.idcard = idcard;
            this.address = address;
            this.positionid = position;
            this.active = active;
            this.modifieddate = modifieddate;
            this.username = username;
        }
    }
}
