using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using System.Configuration;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class UserDAL
    {
        public void UserInsert(UserBLL usr)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "INSERT INTO [users] (nom, password, indice, localisation, photo) VALUES (@nom, @password, @indice, @localisation, @photo)";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@nom", usr.nom);
                cmd.Parameters.AddWithValue("@password", usr.password);
                cmd.Parameters.AddWithValue("@indice", usr.indice);
                cmd.Parameters.AddWithValue("@localisation", usr.localisation);
                cmd.Parameters.AddWithValue("@photo", usr.photo);
                cmd.ExecuteNonQuery();
            }
        }

        public void UserUpdate(UserBLL usr)
        {

            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();

                StringBuilder queryBuilder = new StringBuilder("UPDATE [users] SET ");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;

                if (usr.nom != null)
                {
                    queryBuilder.Append("nom = @nom, ");
                    cmd.Parameters.AddWithValue("@nom", usr.nom);
                }
                if (usr.password != null)
                {
                    queryBuilder.Append("password = @password, ");
                    cmd.Parameters.AddWithValue("@password", usr.password);
                }
                if (usr.indice != null)
                {
                    queryBuilder.Append("indice = @indice, ");
                    cmd.Parameters.AddWithValue("@indice", usr.indice);
                }
                if (usr.localisation != null)
                {
                    queryBuilder.Append("localisation = @localisation, ");
                    cmd.Parameters.AddWithValue("@localisation", usr.localisation);
                }
                if (usr.photo != null)
                {
                    queryBuilder.Append("photo = @photo, ");
                    cmd.Parameters.AddWithValue("@photo", usr.photo);
                }

                // Retirer la dernière virgule et espace
                if (queryBuilder.Length > 0)
                {
                    queryBuilder.Length -= 2; // Retirer ", " à la fin
                }

                
                queryBuilder.Append(" WHERE Id = @id");
                cmd.Parameters.AddWithValue("@id", usr.id);


                cmd.CommandText = queryBuilder.ToString();

                
                cmd.ExecuteNonQuery();
            }


        }


        public bool UserExist(UserBLL usr)
        {
            if (usr == null || string.IsNullOrEmpty(usr.nom))
            {
                return false;
            }
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();

                string query = "SELECT COUNT(*) FROM [users] WHERE nom = @nom";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@nom", usr.nom);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public bool VerifyUserInDatabase(string username, string password)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("SELECT password FROM [users] WHERE nom = @nom", cnx);

                cmd.Parameters.AddWithValue("@nom", username);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string storedPassword = reader.GetString(0);
                    string decryptedPassword = Cryptography.Decrypter(storedPassword);
                    if (decryptedPassword == password)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public string GetPasswordFromDatabase(string nom)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();


                SqlCommand cmd = new SqlCommand("SELECT password FROM [users] WHERE nom = @nom", cnx);

                cmd.Parameters.AddWithValue("@nom", nom);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
            
                    return reader.GetString(0);
                }
                return "";
            }
        }


        public string GetIndiceForUsername(string username)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT indice FROM [users] WHERE nom = @nom";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@nom", username);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedIndice = reader["indice"].ToString();

                    return storedIndice;
                }

                // Si l'utilisateur n'existe pas, renvoyer null ou une valeur par défaut
                return null;
            }
        }

        public string GetLocalisationForUsername(string username)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT localisation FROM [users] WHERE nom = @nom";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@nom", username);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedLocalisation = reader["localisation"].ToString();

                    return storedLocalisation;
                }
                return null;
            }
        }

        

        public byte[] GetPhotoForUsername(string username)
        {
            byte[] storedImage = null;
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT photo FROM [users] WHERE nom = @nom";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@nom", username);
                SqlDataReader reader = cmd.ExecuteReader();
                

                if (reader.Read())
                {
                     storedImage = reader["photo"] as byte[];

                }

                // Si l'utilisateur n'existe pas, renvoyer null ou une valeur par défaut
                return storedImage;

            }

           
        }

        public UserBLL GetUserById(int id)
        {
            UserBLL user = null;
            using (SqlConnection conn = new SqlConnection((ConfigurationManager.ConnectionStrings["CS"].ConnectionString)))
            {
                string query = "SELECT * FROM users WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new UserBLL
                    {
                        nom = reader["nom"].ToString(),
                        indice = reader["indice"].ToString(),
                        password = reader["password"].ToString(),
                        localisation = reader["localisation"].ToString(),
                        photo = reader["photo"] as byte[],
                    };
                }
            }
            return user;
        }

        public string GetIDForUsername(string username)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT Id FROM [users] WHERE nom = @nom";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@nom", username);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedLocalisation = reader["Id"].ToString();

                    return storedLocalisation;
                }

                return null;
            }
        }

        public DataTable SelectUser()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT nom, localisation, photo  FROM users ORDER BY Id ASC";
                SqlCommand cmd = new SqlCommand(query, cnx);
                SqlDataAdapter sdap = new SqlDataAdapter(cmd);
                sdap.Fill(dt);
            }
            return dt;
        }
    }
}
