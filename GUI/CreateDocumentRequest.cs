/*using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;*/
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;
using System.Globalization;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using String = System.String;
using System.Diagnostics;
using System.Net.WebSockets;

namespace GUI
{
    public class CreateDocumentRequest
    {
        // looping docs requests
        public static async Task<Dictionary<string,string>> MakeRequest(List<string> Doc, Dictionary<string, List<string>> entities_id, List<string> selected)
        {
           

            Dictionary<string, List<string>> DocURLS = new Dictionary<string, List<string>>() {
                //{ "Акт-сверки",  new List<string> { "http://crmext.gpbl.ru:8082/WebServiceApp.svc/GetReconciliationAct?contractId=%7B@entity_id%7D", "FileUrl"}},
                { "Акт-сверки",  new List<string> {
                    "http://crmext.gpbl.ru:8082/WebServiceApp.svc/GetReconciliationActOnDate?contractId=%7B{0}%7D&accountId=&date={1}&_={2}", "FileUrl"}
                },
                { "Cчет на пени",  new List<string> {
                    "http://crmext.gpbl.ru:8082/WebServiceApp.svc/GetPenaltiesOnDate?contractId=%7B{0}%7D&accountId=&date={1}&_={2}", "FileUrl"}
                } 
            };

            DateTime epochTime = DateTime.Parse("1970-01-01");
            var milliseconds = Convert.ToString(SendDocs.Date.Subtract(epochTime).TotalSeconds);
            string Repdate = SendDocs.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Replace("/", "%2F");

            Dictionary<string, string> Paths = new Dictionary<string, string>();

            //Task[] taskArray = new Task[Convert.ToInt32(entities_id.Count)*Convert.ToInt32(Doc.Count)];


            var taskArray = new List<Task<Match>>();

            foreach (KeyValuePair<string, List<string>> kvp in entities_id)
            {
                foreach (string d in Doc)
                {
                    //Debug.WriteLine(d);
                    //Debug.WriteLine(kvp.Key);
                    if (selected.Contains(kvp.Key)) {
                        string url = String.Format(DocURLS[d][0], kvp.Value[1], Repdate, milliseconds);
                        //MessageBox.Show(url);
                        taskArray.Add(
                            Task.Run(() => Loop(url)
                        ));
                    }
                }
                
            }
            Task t1 = Task.WhenAll(taskArray);
            try
            {
                await t1;
                taskArray.ForEach(tk => {
                    var k = tk.Result.Groups[tk.Result.Groups.Count - 1].ToString(); //key: name of pdf
                    var v = tk.Result.Value.Replace(@"/", @"\"); //value: alternative way to pdf 
                    if (tk != null && !string.IsNullOrEmpty(k) && !string.IsNullOrEmpty(v))
                    {
                        Paths.Add(k, v);
                    }}
                );
                return Paths;
            }
            catch
            {
                return Paths;
            }

        }
        // creating requests and getting paths for files on net folder
        private static async Task<Match>? Loop(string url)
        {
            try
            {
                //генерация запроса
                //string url = DocURLS[Doc][0].Replace("@entity_id", entity_id);
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.Method = "GET";
                req.Timeout = 100000;
                req.ContentType = "application/json";
                req.Accept = "application/json";
                //получение ответа
                var res = await req.GetResponseAsync() as HttpWebResponse;
                var resStream = res.GetResponseStream();
                var sr = new StreamReader(resStream, Encoding.UTF8);
                string StringSr = sr.ReadToEnd();
                JObject o = JObject.Parse(StringSr);
                string? link = Convert.ToString(o["FileUrl"]);
                //Debug.WriteLine(link);
                string pattern = @"/(\w+)/([А-Я0-9\-]+)/(.*)";
                Match matchres = Regex.Match(link, pattern);
                return matchres;
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                return null;
            }
        }
    }
}
