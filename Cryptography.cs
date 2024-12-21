using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace DAL
{
    public class Cryptography
    {
        public static string Encrypter(string textedeCryptage)
        {
            // Clé de chiffrement (à ne pas stocker en clair dans le code)
            string EncryptionKey = "Vivre_dans_une_planette_en_sante"; // Nous pouvons changer la  clé de chiffrement selon nos besoins
            // Convertit la chaîne de texte en tableau d'octets en utilisant l'encodage Unicode
            byte[] clearBytes = Encoding.Unicode.GetBytes(textedeCryptage);
            // Crée un nouvel objet AES (Advanced Encryption Standard) pour le chiffrement
            using (Aes encryptor = Aes.Create())
            {
                // Définit la clé de chiffrement en dérivant une clé à partir de la clé principale
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32); // Définit la clé (256 bits)
                encryptor.IV = pdb.GetBytes(16); // Définit le vecteur d'initialisation (IV) (128 bits)
                                                 // Utilise un MemoryStream pour stocker les données chiffrées
                using (MemoryStream ms = new MemoryStream())
                {

                    // Crée un CryptoStream pour effectuer le chiffrement
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(),
                    CryptoStreamMode.Write))
                    {
                        // Écrit les données claires dans le CryptoStream pour les chiffrer
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    // Convertit les données chiffrées en une chaîne Base64 et la renvoie
                    textedeCryptage = Convert.ToBase64String(ms.ToArray());
                }


            }
            return textedeCryptage;
        }

        public static string Decrypter(string texteCrypte)
        {
            // Clé de chiffrement (devrait être identique à celle utilisée pour le chiffrement)
            string EncryptionKey = "Vivre_dans_une_planette_en_sante"; // Nous pouvons changer la clé de chiffrement selon nos besoins, mais la clé de déchiffrement doit être identique à la clé de chiffrement
            // Remplace les espaces dans la chaîne chiffrée par le caractère '+' (pour décodage Base64)
            texteCrypte = texteCrypte.Replace(" ", "+");
            // Convertit la chaîne chiffrée en tableau d'octets en utilisant Base64
            byte[] cipherBytes = Convert.FromBase64String(texteCrypte);
            // Crée un nouvel objet AES (Advanced Encryption Standard) pour le déchiffrement
            using (Aes encryptor = Aes.Create())
            {
                // Définit la clé de déchiffrement en dérivant une clé à partir de la clé principale
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32); // Définit la clé (256 bits)
                encryptor.IV = pdb.GetBytes(16); // Définit le vecteur d'initialisation (IV) (128 bits)
                                                 // Utilise un MemoryStream pour stocker les données déchiffrées
                using (MemoryStream ms = new MemoryStream())
                {
                    // Crée un CryptoStream pour effectuer le déchiffrement
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(),
                    CryptoStreamMode.Write))
                    {
                        // Écrit les données chiffrées dans le CryptoStream pour les déchiffrer
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    // Convertit les données déchiffrées en une chaîne en utilisant l'encodage Unicode et la  renvoie
                    texteCrypte = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return texteCrypte;
        }
    }
}
