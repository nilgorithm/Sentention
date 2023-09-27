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
using Outlook = Microsoft.Office.Interop.Outlook;
using SEx = System.Exception;
using WApp = System.Windows.Forms.Application;
//using System.Drawing.Drawing2D;

namespace GUI
{
    public class MailEvents
    {
        static string FilePath = connectionInfo.ProjEnv + "\\files\\";
        // run terminal process
        private static void StartProcess(string ProcType, string ScriptArgs)
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
        private static bool AllExisted(List<string> searchFiles, string Fp)
        {
            foreach (string file in searchFiles)
            {
                if (File.Exists(Fp + @"\" + file))
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
        private static List<string> DownloadFiles(Dictionary<string, string> DocPaths)
        {
            List<string> AllContractsPaths = new List<string>();
            foreach (KeyValuePair<string, string> kvp in DocPaths)
            {
                if (!string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value))
                {
                    string command = $@"/k copy ""\\dtc01-sp01.corp.gpbl.ru\DavWWWRoot{kvp.Value}"" ""{FilePath}""";
                    AllContractsPaths.Add(FilePath + kvp.Key);
                    Task t1 = Task.Run(() => StartProcess("cmd", command));
                    t1.Wait();
                }
            }
            return AllContractsPaths;
        }

        private static void DownloadFile(string netpath){
            string command = $@"/k copy ""\\dtc01-sp01.corp.gpbl.ru\DavWWWRoot{netpath}"" ""{FilePath}""";
            Task t1 = Task.Run(() => StartProcess("cmd", command));
            t1.Wait();
        }

        async public static void SendMail(string Theme, string Body, string Recepient)
        {
            //Directory.CreateDirectory(FilesPath);
            List<string> AllContractsPaths = DownloadFiles(SendDocs.DocPaths);
            //Dictionary<string, string> PastCommands = new Dictionary<string, string>();
            
            var c = 0;
            Debug.WriteLine(string.Join("\n", SendDocs.DocPaths.Keys.ToArray()));
            while (!AllExisted(SendDocs.DocPaths.Keys.ToList(), FilePath) && c < 10)
            {
                c += 1;
                Thread.Sleep(1000);
                //var not_existed_fies = AllContractsPaths.Where(f => !File.Exists(f)).ToArray();
                Debug.WriteLine(string.Join("\n", AllContractsPaths));
                Debug.WriteLine("search");
                foreach (string f in AllContractsPaths) {
                    Debug.WriteLine(f.Split("\\").Last());
                    if (!File.Exists(f)) {
                        Debug.WriteLine(SendDocs.DocPaths[f.Split("\\").Last()]);
                        DownloadFile(netpath: SendDocs.DocPaths[f.Split("\\").Last()]);
                    }
                    else {
                        Debug.WriteLine("EXISTES");
                    }
                }
                continue;
            }
            await DiskConnection.Main(args: new string[] { });
            Debug.WriteLine(string.Join("\n&&", AllContractsPaths.Where(f => File.Exists(f)).ToArray()));
            //string ScriptArgs = $"\"{connectionInfo.ProjEnv}\\SendMail.vbs\" \"{Theme}\"  \"{Body}\" \"{Recepient}\" \"{String.Join(",", AllContractsPaths.Where(f => File.Exists(f)).ToArray())}\"";
            //Debug.WriteLine(ScriptArgs);
            //Task t2 = Task.Run(() => StartProcess("cscript", "//B " + ScriptArgs));
            //Task t2 = Task.Run(() => createMail(theme: Theme, body:Body, recepient: Recepient, apaths: AllContractsPaths.Where(f => File.Exists(f)).ToList()));
            //createMail(theme: Theme, body:Body, recepient: Recepient, apaths: AllContractsPaths.Where(f => File.Exists(f)).ToList());

            try
            {
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.HTMLBody = Body;
                foreach (string path in AllContractsPaths)
                {
                    Outlook.Attachment oAttach = oMsg.Attachments.Add(path);
                }
                oMsg.Subject = Theme;
                oMsg.Recipients.Add(Recepient);
                ((Outlook._MailItem)oMsg).Display();
                WApp.Exit();

                /*MailMessage message = new MailMessage();

                // Set subject of the message, body and sender information
                message.Subject = "New message created by Aspose.Email for .NET";
                message.Body = "This is the body of the email.";

                // Add To recipients and CC recipients
                message.To.Add(new MailAddress("to1@domain.com", "Recipient 1", false));

                // Add attachments
                message.Attachments.Add(new Attachment(AllContractsPaths[0]));*/


            }
            catch (SEx exc)
            {
                Debug.WriteLine(exc.Message);
            }

           /* try
            {
                await t2;
                WApp.Exit();

            }
            catch (SEx)
            {
                MessageBox.Show("Ошибка 2");
            }*/
        }

    }
}
