using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    internal class ClearFolders
    {
        public static void Clear() {
            DirectoryInfo dirInfo = new DirectoryInfo(connectionInfo.ProjEnv + "\\files\\");

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
