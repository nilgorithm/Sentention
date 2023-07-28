/*using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Text;
*/



namespace GUI
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //args = new string[1] { "MyProtocol:/GPBL/main.aspx?etc=4212&extraqs=formid%3d16654191-19ad-48ca-9e46-c4b4fbdad76f&id=%7b9F34E0F1-7B28-EE11-A3D9-00505601285E%7d&pagetype=entityrecord#527649259:Form_2"};
            string parse = args[0].Split(':').Last();
            
            if (parse == "Form_1")
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new HistoryPhones(args));
            }
            else if (parse == "Form_2")
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new SendDocs(args));
            }

        }
    }
    
}

