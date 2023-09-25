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
            string subPath = "\\files";
            bool exists = Directory.Exists(connectionInfo.ProjEnv + subPath);
            if (!exists) {
                Directory.CreateDirectory(connectionInfo.ProjEnv + subPath);
            }
            DirectoryInfo dirInfo = new DirectoryInfo(connectionInfo.ProjEnv + $"{subPath}\\");
            
            foreach (FileInfo file in dirInfo.GetFiles()) {
                file.Delete();
            }
        }
    }
}
