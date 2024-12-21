using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class CollecteBLL
    {
        public int id  { get; set; }
        public int id_depos { get; set; }
        public int id_user { get; set; }
        public int id_worker{ get; set; }
        public DateTime date { get; set; }
        public string statut { get; set; }

        public CollecteBLL(int id, int id_depos, int id_user, int id_worker, DateTime date, string statut)
        {
            this.id = id;
            this.id_depos = id_depos;
            this.id_user = id_user;
            this.id_worker = id_worker;
            this.date = date;
            this.statut = statut;
        }

        public CollecteBLL() { }

    }
}
