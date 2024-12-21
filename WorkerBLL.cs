using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class WorkerBLL
    {
        public int id { get; set; }
        public string nom { get; set; }
        public string password { get; set; }
        public string indice { get; set; }
        public string localite { get; set; }
        public string type { get; set; }

        public static string Role;
        public byte[] photo { get; set; }

        public WorkerBLL(int id, string nom, string password, string indice, string localite, string type, byte[] photo)
        {
            this.id = id;
            this.nom = nom;
            this.password = password;
            this.indice = indice;
            this.localite = localite;
            this.type = type;
            this.photo = photo;
        }

        public WorkerBLL() { Role = ""; }
    }
}
