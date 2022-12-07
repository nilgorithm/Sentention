using System;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;


namespace VisualiseTableProd
{
    public class CreateDocumentRequest
    {

        public static async Task<List<string>> MakeRequest(List<string> Doc, Dictionary<string,List<string>> entities_id)
        {
            List<string> Paths = new List<string>();
            
            //Task[] taskArray = new Task[Convert.ToInt32(entities_id.Count)*Convert.ToInt32(Doc.Count)];
            var taskArray = new List<Task<string>>();

            foreach(KeyValuePair<string, List<string>> kvp in entities_id)
            {
                taskArray.Add(Task.Run(()=>Loop("Акт-сверки",kvp.Value[1])));
            }
            Task t1 = Task.WhenAll(taskArray);
            try {
                await t1;
                taskArray.ForEach(tk => Paths.Add(tk.Result));
                return Paths;
            }
            catch{
                return Paths;
            }

        }

        private static async Task<string> Loop(string Doc, string entity_id)
        {
            try
            {
                Dictionary<string, List<string>> DocURLS = new Dictionary<string, List<string>>() {
                { "Акт-сверки",  new List<string> { "http://crmext.gpbl.ru:8082/WebServiceApp.svc/GetReconciliationAct?contractId=%7B@kaka%7D", "FileUrl"} }
                };
                //генерация запроса
                string url = DocURLS[Doc][0].Replace("@kaka", entity_id);
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
                return Convert.ToString(o["FileUrl"]);
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                return "kaka";
            }
        }
    }
}
