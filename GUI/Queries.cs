using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Diagnostics;
using System.Security.Cryptography;
using System;
using GUI;

namespace GUI
{
    // Делегаты для особых реплейсментов (для их вызова в последствии из класса LaunchQuery, мы знаем какой особый метод реплейсамента достать составленного класса)
    internal delegate string[]? ReplacementMetod(ref string st);
    internal delegate string? ConvertRes(ref string? st);

    // Особые методы реплейсмента в параметров в запросах
    static internal class AdditionalModifications {

        // Т.к. бд достает дату не в нашем часовом поясе добавляем 3 часа
        static internal string DatePlus3H(ref string? date) {
            if (string.IsNullOrEmpty(date)) {
                return string.Empty;
            }
            else {
                DateTime DatePlus3H = Convert.ToDateTime(date).AddHours(3);
                return DatePlus3H.ToString();
            }
        }

        // scpecial replacemnet method for GetRegionFromKPPDaData
        static internal string[]? GetRegion(ref string INN) {
            try {
                var url = "https://suggestions.dadata.ru/suggestions/api/4_1/rs/findById/party";

                //генерация запроса
                HttpWebRequest? req = WebRequest.Create(url) as HttpWebRequest;
                req.Method = "POST";
                req.Timeout = 10000;
                req.Headers.Add("Authorization", "Token 8f2187965372f5aa9974ef22d69d9a6bf87f9a43 **ce");  //указываем токен, полученный при регистрации
                req.ContentType = "application/json";
                req.Accept = "application/json";

                //данные для отправки
                string query_string = "{\"query\": \"" + INN[0] + "\", \"branch_type\": \"MAIN\"}";
                var sentData = Encoding.UTF8.GetBytes(query_string); //т.к. тело запроса небольшое, генерируем его строкой
                req.ContentLength = sentData.Length;
                Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);

                //получение ответа
                var res = req.GetResponse();
                var resStream = res.GetResponseStream();
                var sr = new StreamReader(resStream, Encoding.UTF8);
                string StringSr = sr.ReadToEnd();
                JObject o = JObject.Parse(StringSr);

                //получае кпп из json пути 
                var ReqValue = o.SelectToken("suggestions[0].data.kpp");
                string? KPP = (ReqValue is null || ReqValue.Type == JTokenType.Null) ? null : ReqValue.ToString();

                //если кпп был найден, возвращаем srting array c 3 вариациями кода региона по КПП
                if (!string.IsNullOrEmpty(KPP) && KPP.Length > 0) {
                    return new string[3] { KPP.Substring(0, 1), KPP.Substring(0, 2), KPP.Substring(0, 3) };
                }
                //если кпп был НЕ найден, возвращаем srting array c 3 вариациями кода региона по ИНН
                else {
                    return new string[3] { INN.Substring(0, 1), INN.Substring(0, 2), INN.Substring(0, 3) };
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }


    // декриптим предварительно енкрипченные строки подлючения в Queries
    static class Decrypter2 {
        // decrypting encrypted strings
        internal static string Decrypt(string cypherText) {
            var bytesToDecrypt = Convert.FromBase64String(cypherText);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                TextReader reader = new StreamReader(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\config2.xml");
                //TextReader reader = new XmlReader(connectionInfo.ConfigKeys);
                string privateKey = reader.ReadToEnd();
                reader.Close();
                rsa.FromXmlString(privateKey);
                byte[] decryptedData = rsa.Decrypt(bytesToDecrypt, false);
                var Decrypted = Encoding.Unicode.GetString(decryptedData);
                //MessageBox.Show(Decrypted);
                return Decrypted;
            }
        }
    }

    // Для перевода запросов в объект, возвращающих Dict и использующих параметр "token" конструктора QueryConstruction
    //delegate GetInfoManagerToObj DGetInfoManagerToObj(ref object self);
    public class GetInfoLeasingManagerToObj {
        public string? ManagerLeasing { get; set; } // менеджер по лизингу 
        public string? DepartmentCM { get; set; } // дп менеджера по лизингу
        public string? StatementCM { get; set; } // статус учетки
        public string? FullnameDirectorCM { get; set; } // имя директора

        /*public GetInfoManagerToObj(string? ml = null, string? dp = null, string? status = null, string? dir = null)
        {
            ManagerLeasing = ml;
            DepartmentCM = dp;
            StatementCM = status;
            FullnameDirectorCM = dir;
        }*/

        //GetInfoManagerToObj RetSelf(ref object Self) {
        GetInfoLeasingManagerToObj RetSelf(object Self)
        {
            return (GetInfoLeasingManagerToObj)Self;
        }
    }
    //delegate GetRegionFromKPPDaDataToObj DGetRegionFromKPPDaDataToObj(ref object self);
    public class GetRegionFromKPPDaDataToObj {
        public string? r1 { get; set; } // первый символ от КПП || ИНН
        public string? r2 { get; set; } // первые 2 символа от КПП || ИНН
        public string? r3 { get; set; } // первые 3 символа от КПП || ИНН

        //GetRegionFromKPPDaDataToObj RetSelf(ref object Self) {
        GetRegionFromKPPDaDataToObj RetSelf(object Self) {
            return (GetRegionFromKPPDaDataToObj)Self;
        }
    }

    internal class QueryConstruction {

        internal string Query { get; set; } // запрос
        internal string[]? Tokens { get; set; } // Токенизация по колонкам
        internal Type? SpecialTypeForObjectConvet { get; set; }
        internal string Connection { get; set; } // Строка Подключения
        internal ReplacementMetod? AdditionalReplacement { get; set; } //если нужен какой то особый реплейс, к примеру с использованием запроса к DaData
        internal ConvertRes? ConvertMethod { get; set; } // если результат надо сконвертировать в последствии во что то еще, к примеру в дату

        internal QueryConstruction(string query, string connection, string[]? tokens = null, ReplacementMetod? replacementMethod = null, ConvertRes? convertMethod = null, Type? specialType = null)
        {
            this.Query = query;
            this.Tokens = tokens;
            this.Connection = connection;
            this.AdditionalReplacement = replacementMethod;
            this.ConvertMethod = convertMethod;
            this.SpecialTypeForObjectConvet = specialType;
        }
    }

    // Блок всех запросов 
    internal class Queries {
        //connections
        const string Dc = "CF5S3kNMu5Zlq7syEahEezgCkpCYW0lBzyPOPtlgV8wJufs60H6pTBAbaSQpKgUzF0L/mb/WM1CTeFr3TMtUcUadW2DvyjZmRDCOlq48Z1uOmgyNimyGj53P6ymez3TlvMcR01QBEqhFj8eCHmRVfvFhin5/OGS2JVb3YGeDj+4=";
        static string ConnectionDKS = $"Data Source=DTC01-SQLTST01\\TEST;Initial Catalog = klecov;{Decrypter2.Decrypt(Dc)};MultipleActiveResultSets = True";
        

        /*const string Sc = "Kc4c+57PosooUU93IUpOgeFS9XASPsFFMx8mAIiVPYt7bg3sk3sGt65IRAZJHM25WLVhxwNyhn5Q3TimAcxNxAhY5i4bC3vr2vXV+Vx/dt/fnsXRzpSNJyrNwtWQhsc9HPLLS+XFC9Rvmr3trSLmpZL9dea6gson8imvhd0DOyc=";
        static string ConnectionCRM = $"Data Source=DTC01-SQLCRM02\\CRMSHIP;Initial Catalog = GPBL_MSCRM;{Decrypter.Decrypt(Sc)};ApplicationIntent=ReadOnly;MultipleActiveResultSets = True";*/
        
        static string ConnectionCRM = connectionInfo.CRMConnectionString;

        //str
        internal static QueryConstruction ActualProductVersion = new QueryConstruction(
        query: $"SELECT ProductVersion as 'POVersion' FROM klecov.dbo.DKSProjectsVersions WHERE ProductName = '{Application.ProductName}'",
        connection: ConnectionDKS); // проверка на новое обновление приложения
    }
}
