using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace VisualiseTableProd
{
    public class MailEvents
    {
        public static void StartProcess(string ProcType, string ScriptArgs)
        {
            Process scriptProc = new Process();
            scriptProc.StartInfo.FileName = ProcType;
            scriptProc.StartInfo.Arguments = ScriptArgs;
            scriptProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            scriptProc.Start();
            //scriptProc.WaitForExit();
            scriptProc.Close();
        }
        async public static void SendMail(string Theme, string Body, string Recepient)
        {
            string EnvPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\files\\";
            //Task OpenConnction = Task.Run(() => StartProcess($@"{EnvPath}\net\OpenNetFolder.exe", String.Join(" ", connectionInfo.NetFolderCredentials)));
            List<string> AllContractsPaths = new List<string>();
            
            foreach (string contract in Form2.SelectedContracts)
            {
                string[] files = new DirectoryInfo(@"\\dtc01-sp01.corp.gpbl.ru\DavWWWRoot\leasing_contract\" + contract + "\\").GetFiles("*.pdf", SearchOption.TopDirectoryOnly).Select(f => f.FullName).ToArray();

                foreach (string filename in files)
                {
                    if (filename.Contains("Акт-сверки"))//|| filename.Contains("Пени")
                    {
                        StartProcess("cmd", $"/k copy \"{filename}\" {EnvPath}");
                        AllContractsPaths.Add(EnvPath + filename.Split('\\').Last());
                        break;
                    }
                }
            }
            //Thread.Sleep(3000);
            string ScriptArgs = $"{EnvPath}SendMail.vbs \"{Theme}\"  \"{Body}\" \"{Recepient}\" \"{String.Join(",", AllContractsPaths)}\"";

            StartProcess($@"{EnvPath}\net\OpenNetFolder.exe", string.Empty);
            Task t1 = Task.Run(() => StartProcess(@"cscript", ScriptArgs));
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
