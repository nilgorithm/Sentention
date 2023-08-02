/*using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;*/
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Text;
using System.Xml;

namespace GUI
{
    static class connectionInfo
    {
        // public connections
        public static string ProjEnv = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
        //public static string ConfigPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\files\\config.xml";
        //public static XmlDocument ConfigKeys = Load("Resources/DefaultConfig.xml");
        //document.Load("Resources/DefaultConfig.xml");
        public static string ConfigPath = ProjEnv + "\\config.xml";

        public static string Fc = "KgkIQ5YdXnF+T0kXRx4NUFZ1r5kk1irm/RuTeWcBoAF4MZ6OD+ZqkpSzy9BCNUinD7sVtFhltEt+zoiyyrAItajsCtfYZGZssvsPHvO9bDktOwa4TATWHdYo5+b7qkk3IYV61GNmIy4SpLKgkG9I9rR3sJXs02BQRkQXMFvpwYs=";
        public static string AsteriskConnectionString { get; } = $"Server=10.10.0.20;Database=asteriskcdrdb;{Decrypter.Decrypt(Fc)};SSL Mode=None;Port=3306;Default command timeout = 0;";
        public static MySqlConnection AsteriskConnection { get; } = new MySqlConnection(AsteriskConnectionString);

        public static string Sc = "k5bMbibM52qwjwOkAEnygW+EUNbCfh2YCo1kOh4gn8/7P3FuqsKZBVP9CpN7HYdUMcK36eQzuI0+OnQQrBJ9li+yZ4h059hDQ/1llDwntgayquSlO8dCA/QeZEHp6ZVBk6X3OxfceOLz1ExtHKJi8vtFYqc0lqoD99g3UeQx4vQ=";
        public static string CRMConnectionString { get; } = $"Data Source=DTC01-SQLCRM02\\CRMSHIP;Initial Catalog = GPBL_MSCRM;{Decrypter.Decrypt(Sc)};ApplicationIntent=ReadOnly;MultipleActiveResultSets = True";
        public static SqlConnection CRMConnection { get; } = new SqlConnection(CRMConnectionString);

        public static string Thc = "LqTHqjW5SjMS5R/uO+YxIFcBqEoULjE2NajPWxDaj3m3vP8sFLDU+l+IiMFEDTguwU8Zbhis8lyx6SseG9yVGWyksjLj27J24ASKZImdIY5udykHZFdb159DO5wzeIiMsyJyvM2HzEWiJ7jbDQOFi4uAJ2jHMUAz9AUp7MDNHFM=";
        public static string NetFolderCredentials = Decrypter.Decrypt(Thc);

    }

    static class Decrypter
    {
        // decrypting encrypted strings
        public static string Decrypt(string cypherText)
        {
            var bytesToDecrypt = Convert.FromBase64String(cypherText);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                

                TextReader reader = new StreamReader(connectionInfo.ConfigPath);
                //TextReader reader = new XmlReader(connectionInfo.ConfigKeys);
                string privateKey = reader.ReadToEnd();
                reader.Close();

                rsa.FromXmlString(privateKey);

                byte[] decryptedData = rsa.Decrypt(bytesToDecrypt, false);

                var Decrypted = Encoding.Unicode.GetString(decryptedData);
                //MessageBox.Show(Decrypted);
                return Decrypted;
            }
        }

    }
}
