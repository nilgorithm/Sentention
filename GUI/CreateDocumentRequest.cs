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

namespace GUI
{
    public class CreateDocumentRequest
    {

        public static async Task<Dictionary<string,string>> MakeRequest(List<string> Doc, Dictionary<string, List<string>> entities_id)
        {
            Dictionary<string, string> Paths = new Dictionary<string, string>();

            //Task[] taskArray = new Task[Convert.ToInt32(entities_id.Count)*Convert.ToInt32(Doc.Count)];
            var taskArray = new List<Task<Match>>();

            foreach (KeyValuePair<string, List<string>> kvp in entities_id)
            {
                taskArray.Add(Task.Run(() => Loop("Акт-сверки", kvp.Value[1])));
            }
            Task t1 = Task.WhenAll(taskArray);
            try
            {
                await t1;
                taskArray.ForEach(tk => {
                    if (tk != null)
                    {
                        Paths.Add(
                        tk.Result.Groups[tk.Result.Groups.Count - 1].ToString(), //key: name of pdf
                        tk.Result.Value.Replace(@"/", @"\") //value: alternative way to pdf 
                            );
                    }}
                );
                return Paths;
            }
            catch
            {
                return Paths;
            }

        }

        private static async Task<Match>? Loop(string Doc, string entity_id)
        {
            try
            {
                Dictionary<string, List<string>> DocURLS = new Dictionary<string, List<string>>() {
                { "Акт-сверки",  new List<string> { "http://crmext.gpbl.ru:8082/WebServiceApp.svc/GetReconciliationAct?contractId=%7B@entity_id%7D", "FileUrl"} }
                };
                //генерация запроса
                string url = DocURLS[Doc][0].Replace("@entity_id", entity_id);
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
