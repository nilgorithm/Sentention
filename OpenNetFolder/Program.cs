//using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace OpenNetFolder
{
    public class DiskConnection
    {
        public async static Task Main(string[] args)
        {
            string ConnectionClose = @"use w:  /delete";
            if (args.Length > 0)
            {
                string ConnectionOpen = @$"use w: \\dtc01-sp01.corp.gpbl.ru\DavWWWRoot\leasing_contract /user:{args[0].Replace('@', ' ')}";
                var T = Task.Run(() => { Launch(ConnectionOpen); });
                await T;
            }
            else
            {
                var T = Task.Run(() => { Launch(ConnectionClose); });
                await T; //T.Wait()
            }
        }
        private static void Launch(string command)
        {
            System.Diagnostics.ProcessStartInfo procStartInfo = new
            System.Diagnostics.ProcessStartInfo("net.exe", command);
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            System.Diagnostics.Process processTemp = new System.Diagnostics.Process();
            processTemp.StartInfo = procStartInfo;
            processTemp.Start();
        }
    }
}
