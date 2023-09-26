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
using Microsoft.Office.Interop.Outlook;
using OutlookApp = Microsoft.Office.Interop.Outlook.Application;
using SEx = System.Exception;
using WApp = System.Windows.Forms.Application;


namespace GUI
{
    public class MailEvents
    {
        // run terminal process
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
            catch (SEx e)
            {
                throw;
            }
        }
        // check all files are in folder
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
        // prepair message send 
        async public static void SendMail(string Theme, string Body, string Recepient)
        {
            string FilesPath = connectionInfo.ProjEnv + "\\files\\";
            Directory.CreateDirectory(FilesPath);

            List<string> AllContractsPaths = new List<string>();

            foreach(KeyValuePair<string, string> kvp in SendDocs.DocPaths)
            {
                string command = $@"/k copy ""\\dtc01-sp01.corp.gpbl.ru\DavWWWRoot{kvp.Value}"" ""{FilesPath}""";
                AllContractsPaths.Add(FilesPath + kvp.Key);
                Task t1 = Task.Run(() => StartProcess("cmd", command));
                t1.Wait();
                
            }
            string ScriptArgs = $"\"{connectionInfo.ProjEnv}\\SendMail.vbs\" \"{Theme}\"  \"{Body}\" \"{Recepient}\" \"{String.Join(",", AllContractsPaths)}\"";
            //MessageBox.Show(ScriptArgs);
            var c = 0;
            while (!AllExisted(SendDocs.DocPaths.Keys.ToList(), FilesPath) && c < 100)
            {
                c += 1;
                Thread.Sleep(1000);
                continue;
            }
            await DiskConnection.Main(args: new string[] { });

            Task t2 = Task.Run(() => StartProcess(@"cscript", "//B " + ScriptArgs));

            try
            {
                await t2;
                WApp.Exit();

            }
            catch (SEx)
            {
                MessageBox.Show("Ошибка 2");
            }
        }

        private static void createMail(bool success, int objectsCount, int pathsCount, DateTime startTime, DateTime endTime, string queryDate)
        {

            OutlookApp outlookApp = new OutlookApp();
            MailItem mailItem = (MailItem)outlookApp.CreateItem(OlItemType.olMailItem);


            mailItem.Subject = "Specification Paths Update {success}";
            mailItem.HTMLBody = @$"";

            var a = mailItem.Attachments;
            
            mailItem.Recipients.Add("dks@gpbl.ru");
            mailItem.Send();
        }

    }
}
