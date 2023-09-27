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
    // main launch stack
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //args = new string[1] { "MyProtocol:/GPBL/main.aspx?etc=4212&extraqs=formid%3d16654191-19ad-48ca-9e46-c4b4fbdad76f&id=%7b9F34E0F1-7B28-EE11-A3D9-00505601285E%7d&pagetype=entityrecord#527649259:Form_2"};
            //args = new string[1] { "myprotocol:C6E32E5A-D401-ED11-A3CA-00505601285E:Form_2" };  //2
            //args = new string[1] { "myprotocol:DDAF8036-EBFC-EA11-8180-005056019243:Form_2" }; //6
            args = new string[1] { "myprotocol:64F6EC13-7253-EC11-A358-00505601285E:Form_2" };  //12
            //args = new string[1] { "myprotocol:1D6DCB77-9403-E911-A83F-005056010A7B:Form_2" };  //xxx
            string parse = args[0].Split(':').Last();
            ClearFolders.Clear();
            // first application run
            if (parse == "Form_1")
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new HistoryPhones(args));
            }
            // second application run
            else if (parse == "Form_2")
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new SendDocs(args));
            }

        }
    }
    
}

