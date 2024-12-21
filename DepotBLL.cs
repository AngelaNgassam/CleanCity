using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DepotBLL
    {
        public int id { get; set; }
        public double poid { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public int id_user { get; set; }

        public DepotBLL(int id, double poid, string type, string description, DateTime date, int id_user)
        {
            this.id = id;
            this.poid = poid;
            this.type = type;
            this.description = description;
            this.date = date;
            this.id_user = id_user;
        }

        public DepotBLL() { }

    }
}
