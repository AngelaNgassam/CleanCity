using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AvisDAL
    {
        public void EnregistrerAvis(string avis, DateTime date_poste, int Id_user)
        {

            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "INSERT INTO Avis (avis,date_poste, Id_user) VALUES (@avis, @date_poste, @Id_user)";
                SqlCommand command = new SqlCommand(query, cnx);
                command.Parameters.AddWithValue("@avis", avis);
                command.Parameters.AddWithValue("@date_poste", date_poste);
                command.Parameters.AddWithValue("@Id_user", Id_user);
                command.ExecuteNonQuery();
            }
        }

        public DataTable ObtenirAvis(string local)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT A.Id as Id, A.date_poste as date, U.nom as nom, A.avis as avis FROM avis A join users U on A.Id_user = U.Id where U.localisation = @local ";
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@local", local);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }
            }
        }


    }
}
