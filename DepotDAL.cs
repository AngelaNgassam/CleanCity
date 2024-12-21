using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BLL;
using System.Threading.Tasks;

namespace DAL
{
    public class DepotDAL
    {
        public void EnregistrerDechet(float poids, List<string> typesOrdures, string description, string statut, DateTime dateDepot, int Id_user)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "INSERT INTO depose (poid, type, description, date, Id_user, statut) VALUES (@Poids, @TypesOrdures, @Description, @DateDepot, @Id_user, @Statut)";
                SqlCommand command = new SqlCommand(query, cnx);
                command.Parameters.AddWithValue("@Poids", poids);
                command.Parameters.AddWithValue("@TypesOrdures", string.Join(",", typesOrdures));
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@DateDepot", dateDepot);
                command.Parameters.AddWithValue("@Id_user", Id_user);
                command.Parameters.AddWithValue("@Statut", statut);
                command.ExecuteNonQuery();
            }
        }

        public DataTable SelectDepos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT U.nom as nom, D.poid, D.date, D.statut, U.localisation as localisation FROM depose D join users U on D.Id_user = U.Id ";
                SqlCommand cmd = new SqlCommand(query, cnx);
                SqlDataAdapter sdap = new SqlDataAdapter(cmd);
                sdap.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenirDepots(int Id_u)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT Id, statut, poid, type,date, description FROM depose where Id_user = @Id_u ";
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@Id_u", Id_u);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }
            }
        }

        public bool SupprimerDernierDepot(int Id_user)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();

                string selectQuery = @" SELECT TOP 1 Id FROM depose  WHERE Id_user = @Id_user  ORDER BY date DESC";

                int dernierDepotId;

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, cnx))
                {
                    selectCommand.Parameters.AddWithValue("@Id_user ", Id_user);

                    object result = selectCommand.ExecuteScalar();

                    if (result == null)
                    {  
                        return false;
                    }

                    dernierDepotId = Convert.ToInt32(result);
                }

              
                string deleteQuery = "DELETE FROM depose WHERE Id = @Id";

                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, cnx))
                {
                    deleteCommand.Parameters.AddWithValue("@Id", dernierDepotId);
                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public string ObtenirStatutDernierDepot(int Id_usr)
            {
                using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
                {
                    cnx.Open();
                    string query = "SELECT statut FROM depose where Id_user = @Id_usr and Id = (select max(Id) from depose)";
                    SqlCommand cmd = new SqlCommand(query, cnx);
                    cmd.Parameters.AddWithValue("@Id_usr", Id_usr);
                    return (string)cmd.ExecuteScalar();
           
                }
            }
            


        public DataTable ObtenirDepotWorker(string Id_us)

        {
            int Id_usr = int.Parse(Id_us);
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT Id, statut, poid, type,date, description FROM depose where Id_user = @Id_usr";
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@Id_usr", Id_usr);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }
            }
        }



        public double ObtenirPoid(string Id_us)
        {
            int Id_usr = int.Parse(Id_us);
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT sum(poid) as nbre FROM depose where Id_user = @Id_usr";
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@Id_usr", Id_usr);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            double poid = double.Parse(reader["nbre"].ToString());

                            return (float)poid;
                        }
                        return 0;
                    }
                }
            }
        }

        public DataTable ObtenirDepotTrier(string Id_us)
        {
            int Id_usr = int.Parse(Id_us);
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT Id, statut, poid, type,date, description FROM depose where Id_user = @Id_usr and statut = 'en cours'";
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@Id_usr", Id_usr);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }
            }
        }

        public double PoidTotal()
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT sum(poid) as TotalPoids  FROM depose";
                SqlCommand cmd = new SqlCommand(query, cnx);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    double nombre = double.Parse(reader["TotalPoids"].ToString());

                    return nombre;
                }
                return 0;
            }
        }

        public void ModifierStatutDepot(string Id_us, string nouveauStatut, DateTime date)
        {
            int Id_usr = int.Parse(Id_us);
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "UPDATE depose SET statut = @NouveauStatut, date_ramassage= @date WHERE Id_user= @Id_usr and  statut = 'en cours'";
                SqlCommand command = new SqlCommand(query, cnx);
                command.Parameters.AddWithValue("@NouveauStatut", nouveauStatut);
                command.Parameters.AddWithValue("@Id_usr", Id_usr);
                command.Parameters.AddWithValue("@date", date);
                command.ExecuteNonQuery();
            }
        }

        public string GetDateCollecte(int Id_usr)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT date_ramassage FROM depose WHERE Id_user= @Id_usr order by date_ramassage desc ";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@Id_usr", Id_usr);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string date = (reader["date_ramassage"].ToString());

                    return date;
                }
                return null;
                
            }
        }
    }
}
