using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserBLL
    {
        public int id { get; set; }
        public string nom { get; set; }
        public string password { get; set; }
        public string indice { get; set; }
        public string localisation { get; set; }
        public byte[] photo { get; set; }

        public UserBLL(int id, string nom, string password, string indice, string localisation, byte[] photo)
        {
            this.id = id;
            this.nom = nom;
            this.password = password;
            this.indice = indice;
            this.localisation = localisation;
            this.photo = photo;
        }

        public UserBLL() { }
    }
}
