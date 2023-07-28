/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;*/
using System.Diagnostics;
using OpenNetFolder;

namespace GUI
{
    public class MailEvents
    {
        public static void StartProcess(string ProcType, string ScriptArgs)
        {

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = ProcType;
            startInfo.Arguments = ScriptArgs;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            Process processTemp = new Process();
            processTemp.StartInfo = startInfo;
            processTemp.EnableRaisingEvents = true;
            try
            {
                processTemp.Start();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        static bool AllExisted(List<string> searchFiles, string Fp)
        { 
            foreach (string file in searchFiles)
            {
                if (File.Exists(Fp + @"\"+ file))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        async public static void SendMail(string Theme, string Body, string Recepient)
        {
            string FilesPath = connectionInfo.ProjEnv + "\\files\\";
            Directory.CreateDirectory(FilesPath);

            List<string> AllContractsPaths = new List<string>();

            foreach(KeyValuePair<string, string> kvp in SendDocs.DocPaths)
            {
                string command = $@"/k copy ""\\dtc01-sp01.corp.gpbl.ru\DavWWWRoot{kvp.Value}"" ""{FilesPath}""";
                StartProcess("cmd", command);
                AllContractsPaths.Add(FilesPath + kvp.Key);
            }
            string ScriptArgs = $"\"{connectionInfo.ProjEnv}\\SendMail.vbs\" \"{Theme}\"  \"{Body}\" \"{Recepient}\" \"{String.Join(",", AllContractsPaths)}\"";
            //MessageBox.Show(ScriptArgs);
            var c = 0;
            while (!AllExisted(SendDocs.DocPaths.Keys.ToList(), FilesPath) && c < 10)
            {
                c += 1;
                Thread.Sleep(1000);
                continue;
            }
            await DiskConnection.Main(args: new string[] { });
            Task t1 = Task.Run(() => StartProcess(@"cscript", "//B " + ScriptArgs));
            try
            {
                await t1;
                Application.Exit();

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка 2");
            }
        }
    }
}
