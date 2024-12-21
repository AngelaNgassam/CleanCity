using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using BLL;
using System.Threading.Tasks;
using System.Data;

namespace DAL
{
    public class WorkerDAL
    {
        public void UserInsert(WorkerBLL usr)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "INSERT INTO [worker] (nom, password, indice, localite, type, photo) VALUES (@nom, @password, @indice, @localite, @type, @photo)";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@nom", usr.nom);
                cmd.Parameters.AddWithValue("@password", usr.password);
                cmd.Parameters.AddWithValue("@indice", usr.indice);
                cmd.Parameters.AddWithValue("@localite", usr.localite);
                cmd.Parameters.AddWithValue("@type", usr.type);
                cmd.Parameters.AddWithValue("@photo", usr.photo);
                cmd.ExecuteNonQuery();
            }
        }

        public void WorkerUpdate(WorkerBLL usr)
        {

            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();

                StringBuilder queryBuilder = new StringBuilder("UPDATE worker SET ");
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
                if (usr.localite != null)
                {
                    queryBuilder.Append("localite = @localisation, ");
                    cmd.Parameters.AddWithValue("@localisation", usr.localite);
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

        public WorkerBLL GetWorkerById(string id)
        {
            WorkerBLL user = null;
            using (SqlConnection cnx = new SqlConnection((ConfigurationManager.ConnectionStrings["CS"].ConnectionString)))
            {
                string query = "SELECT nom, password, indice,localite,photo FROM worker WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@Id", id);

                cnx.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new WorkerBLL
                    {
                        nom = reader["nom"].ToString(),
                        indice = reader["indice"].ToString(),
                        password = reader["password"].ToString(),
                        photo = reader["photo"] as byte[],
                        localite = reader["localite"].ToString(),
                    };
                }
            }
            return user;
        }

        public bool WorkerExist(WorkerBLL usr)
        {
            if (usr == null || string.IsNullOrEmpty(usr.nom))
            {
                return false;
            }
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();

                string query = "SELECT COUNT(*) FROM [worker] WHERE nom = @nom";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@nom", usr.nom);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public bool LocalizationExist(WorkerBLL usr)
        {
            if (usr == null || string.IsNullOrEmpty(usr.localite))
            {
                return false;
            }
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();

                string query = "SELECT COUNT(*) FROM [worker] WHERE localite = @localite";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@localite", usr.localite);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public bool VerifyWorkerInDatabase(string username, string password)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand("SELECT password, type FROM [worker] WHERE nom = @nom", cnx);

                cmd.Parameters.AddWithValue("@nom", username);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Récupérer le mot de passe stocké dans la base de données
                    string storedPassword = reader.GetString(0);
                    WorkerBLL.Role = reader["type"].ToString();
                    
                    // Déchiffrer le mot de passe stocké
                    string decryptedPassword = Cryptography.Decrypter(storedPassword);
                    // Comparer le mot de passe entré avec le mot de passe stocké
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


                SqlCommand cmd = new SqlCommand("SELECT password FROM [worker] WHERE nom = @nom", cnx);

                cmd.Parameters.AddWithValue("@nom", nom);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Récupérer le mot de passe stocké dans la base de données
                    return reader.GetString(0);
                }
                return "";
            }
        }

        public string GetPassword(string id)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();


                SqlCommand cmd = new SqlCommand("SELECT password FROM [worker] WHERE Id = @Id", cnx);

                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Récupérer le mot de passe stocké dans la base de données
                    return reader.GetString(0);
                }
                return "";
            }
        }

        public string GetIndiceForWorkername(string username)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT indice FROM [worker] WHERE nom = @nom";
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

        public string GetLocalisationForWorkername(string username)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT localite FROM [worker] WHERE nom = @nom";
                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.AddWithValue("@nom", username);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedLocalisation = reader["localite"].ToString();

                    return storedLocalisation;
                }

                
                // Si l'utilisateur n'existe pas, renvoyer null ou une valeur par défaut
                return "";
            }

            
        }

        public byte[] GetPhotoForWorkername(string username)
        {
            byte[] storedImage = null;
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT photo FROM [worker] WHERE nom = @nom";
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

        public DataTable SelectWorker()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT nom, localite, photo  FROM worker where type ='Employer_simple' ORDER BY nom ASC";
                SqlCommand cmd = new SqlCommand(query, cnx);
                SqlDataAdapter sdap = new SqlDataAdapter(cmd);
                sdap.Fill(dt);
            }
            return dt;
        }

        public int NbrWorker()
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT count(*) as nbre  FROM worker";
                SqlCommand cmd = new SqlCommand(query, cnx);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int nombre = int.Parse(reader["nbre"].ToString());

                    return nombre;
                }
                return 0;
            }
        }

        public int NbrClentTotal()
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT count(*) as nbre  FROM users";
                SqlCommand cmd = new SqlCommand(query, cnx);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int nombre = int.Parse(reader["nbre"].ToString());

                    return nombre;
                }
                return 0;
            }
        }


        public int  NbrClent(string local)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT count(*) as nbre  FROM users where  localisation = @localite";
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@localite", local);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            int nombre = int.Parse(reader["nbre"].ToString());

                            return nombre;
                        }
                        return 0;
                    }
                }
            }

           
        }

        public DataTable IDCleint(string local)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ConnectionString))
            {
                cnx.Open();
                string query = "SELECT Id as id  FROM users where  localisation = @localite";
                using (SqlCommand cmd = new SqlCommand(query, cnx))
                {
                    cmd.Parameters.AddWithValue("@localite", local);
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
