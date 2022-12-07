using System;
using System.Linq;
//using System.Threading.Tasks;
//using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;


namespace VisualiseTableProd
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string parse = args[0].Split(':').Last();
            //string parse = "Form_2";
            if (parse == "Form_1")
            {
                Application.Run(new Form1(args));
            }
            else if (parse == "Form_2")
            {
                Application.Run(new Form2(args));
                //Application.Run(new Form2());
            }
            
        }
    }
    static class connectionInfo
    {

        //public static Dictionary<string, string> Data = GetDecryptedHashTable.GetSorce();
        public static string ConfigPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\files\\config.xml";

        public static string Fc = "KgkIQ5YdXnF+T0kXRx4NUFZ1r5kk1irm/RuTeWcBoAF4MZ6OD+ZqkpSzy9BCNUinD7sVtFhltEt+zoiyyrAItajsCtfYZGZssvsPHvO9bDktOwa4TATWHdYo5+b7qkk3IYV61GNmIy4SpLKgkG9I9rR3sJXs02BQRkQXMFvpwYs=";
        public static string AsteriskConnectionString { get; } = $"Server=10.10.0.20;Database=asteriskcdrdb;{Decrypter.Decrypt(Fc)};SSL Mode=None;Port=3306;Default command timeout = 0;";
        public static MySqlConnection AsteriskConnection { get; } = new MySqlConnection(AsteriskConnectionString);

        public static string Sc = "k5bMbibM52qwjwOkAEnygW+EUNbCfh2YCo1kOh4gn8/7P3FuqsKZBVP9CpN7HYdUMcK36eQzuI0+OnQQrBJ9li+yZ4h059hDQ/1llDwntgayquSlO8dCA/QeZEHp6ZVBk6X3OxfceOLz1ExtHKJi8vtFYqc0lqoD99g3UeQx4vQ=";
        public static string CRMConnectionString { get; } = $"Data Source=DTC01-SQLCRM02\\CRMSHIP;Initial Catalog = GPBL_MSCRM;{Decrypter.Decrypt(Sc)};ApplicationIntent=ReadOnly;MultipleActiveResultSets = True";
        public static SqlConnection CRMConnection { get; } = new SqlConnection(CRMConnectionString);

        public static string Thc = "LqTHqjW5SjMS5R/uO+YxIFcBqEoULjE2NajPWxDaj3m3vP8sFLDU+l+IiMFEDTguwU8Zbhis8lyx6SseG9yVGWyksjLj27J24ASKZImdIY5udykHZFdb159DO5wzeIiMsyJyvM2HzEWiJ7jbDQOFi4uAJ2jHMUAz9AUp7MDNHFM=";
        public static string NetFolderCredentials = Decrypter.Decrypt(Thc);

    }

    /*public class GetDecryptedHashTable
    {
        public static Dictionary<string, string> GetSorce()
        {
            Decrypter rsa = new Decrypter();
            string ConfigPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\files\\config.txt";
            var GetHashTable = ReadFromBinaryFile<Hashtable>(ConfigPath);
            Dictionary<string, string> ConvertToD = new Dictionary<string, string>();
            foreach (DictionaryEntry pair in GetHashTable)
            {
                ConvertToD[pair.Key.ToString()] = rsa.Decrypt(pair.Value.ToString()).ToString();
            }
            return ConvertToD;
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }*/
    public class Decrypter
    {
        /*protected static CspParameters _cp = new CspParameters { KeyContainerName = "DecryptConfigFileOfCredentials:Asterisk/CRMSHIP/NetFolder" };
        public RSACryptoServiceProvider csp = new RSACryptoServiceProvider(_cp);
        private RSAParameters _privateKey;

        public Decrypter()
        {
            _privateKey = csp.ExportParameters(true);
        }*/

        public static string Decrypt(string cypherText)
        {
            var bytesToDecrypt = Convert.FromBase64String(cypherText);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                TextReader reader = new StreamReader(connectionInfo.ConfigPath);
                string privateKey = reader.ReadToEnd();
                reader.Close();

                rsa.FromXmlString(privateKey);
             
                byte[] decryptedData = rsa.Decrypt(bytesToDecrypt, false);  

                var Decrypted = Encoding.Unicode.GetString(decryptedData);

                return Decrypted;
            }
            /*public string Decrypt(string cypherText)
            {
                var dataBytes = Convert.FromBase64String(cypherText);
                csp.ImportParameters(_privateKey);
                var plainText = csp.Decrypt(dataBytes, false);
                return Encoding.Unicode.GetString(plainText);
            }*/
        }

    }
}
