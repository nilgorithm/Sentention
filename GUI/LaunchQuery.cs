using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace GUI
{
    public static class ObjectExtensions {

        public static object ToObject(this IDictionary<string, object> source, Type tobj) {
            
            var someObject = Activator.CreateInstance(tobj);
            var someObjectType = someObject.GetType();

            foreach (var item in source) {
                someObjectType.GetProperty(item.Key)
                     .SetValue(someObject, item.Value, null);
            }
            return someObject;
        }
    }

    class SqlExecuter {
        internal QueryConstruction QueryType { get; set; }
        internal string ModifiedQuery { get; set; }

        internal SqlExecuter(QueryConstruction query, string Replacement)
        {
            this.QueryType = query;
            if (query.AdditionalReplacement is null)
            {
                this.ModifiedQuery = (!string.IsNullOrEmpty(Replacement)) ? String.Format(query.Query, Replacement) : query.Query;
            }
            else
            {
                var additionalRepl = query.AdditionalReplacement(ref Replacement);
                this.ModifiedQuery = (additionalRepl != null) ? String.Format(query.Query, additionalRepl) : query.Query;
            }
            //Debug.WriteLine(ModifiedQuery);
        }

        internal Dictionary<string, List<object>> ReturnDictByAssignedRowKeyRowValue() {
            DataTable dt = new DataTable();
            //var dictionaries = new Dictionary<string, object>();
            var dictionaries = new Dictionary<string, List<object>>();
            using (SqlConnection connection = new SqlConnection(QueryType.Connection))
            {
                try
                {
                    SqlCommand InfoCMD = new SqlCommand(ModifiedQuery, connection);
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(InfoCMD);
                    da.Fill(dt);
                    connection.Close();
                    //dictionaries = dt.AsEnumerable().ToDictionary<DataRow, string, object>(row => row[0].ToString(), row => row[1]);
                    foreach(var d in dt.AsEnumerable()) {
                        //Debug.WriteLine(d[0], d[1].ToString());
                        var templ = new List<object>();
                        if (d[0] is not null && !dictionaries.ContainsKey(d[0].ToString())) {
                            templ.Add(d[1].ToString());
                            dictionaries.Add(d[0].ToString(), templ); 
                        }
                        else {
                            dictionaries[d[0].ToString()].Add(d[1].ToString());
                        }
                    }
                }
                catch { };
            }
            return dictionaries;
        }

        // 
        internal List<Dictionary<string, object>> ReturnListDicts() { 
            DataTable dt = new DataTable();
            var dictionaries = new List<Dictionary<string, object>>();
            using (SqlConnection connection = new SqlConnection(QueryType.Connection)){
                try {
                    SqlCommand InfoCMD = new SqlCommand(ModifiedQuery, connection);
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(InfoCMD);
                    da.Fill(dt);
                    connection.Close();
                    foreach (DataRow row in dt.Rows)
                    {
                        Dictionary<string, object> dictionary = Enumerable.Range(0, dt.Columns.Count).ToDictionary(i => dt.Columns[i].ColumnName, i => row.ItemArray[i]);
                        dictionaries.Add(dictionary);
                    }
                }
                catch { }   
            }
            return dictionaries;
        }

        //internal Dictionary<string, object> ReturnTokenizedDictRes() {
        internal object ReturnTokenizedObjectRes() {
        //internal T ReturnTokenizedDictRes<T>() where T : class, new() {
            Dictionary<string, object> results = new Dictionary<string, object>();
            using (SqlConnection connection = new SqlConnection(QueryType.Connection)) {
                try {
                    connection.Open();
                    using (SqlCommand InfoCMD = connection.CreateCommand()) {
                        InfoCMD.CommandText = ModifiedQuery;
                        InfoCMD.CommandTimeout = 0;
                        using (SqlDataReader reader = InfoCMD.ExecuteReader()) {
                            while (reader.Read()) {
                                foreach (string indexer in QueryType.Tokens) {
                                        results.Add(indexer, reader[indexer].ToString());
                                }
                            }
                            reader.Close();
                            connection.Close();
                        }
                    }
                }
                catch (Exception) {
                    throw;
                }
            }
            return results.ToObject(QueryType.SpecialTypeForObjectConvet);
        }

        internal void ExecuteNonQ() {
            using (SqlConnection connection = new SqlConnection(QueryType.Connection)) {
                try {
                    connection.Open();
                    using (SqlCommand InfoCMD = connection.CreateCommand()) {
                        InfoCMD.CommandText = ModifiedQuery;
                        InfoCMD.CommandTimeout = 0;
                        InfoCMD.ExecuteNonQuery();
                    }
                    connection.Close() ; 

                }
                catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        internal string? ReturnStrRes() {
            string? res = String.Empty;
            using (SqlConnection connection = new SqlConnection(QueryType.Connection)) {
                try {
                    connection.Open();
                    using (SqlCommand InfoCMD = connection.CreateCommand()) {
                        InfoCMD.CommandText = ModifiedQuery;
                        InfoCMD.CommandTimeout = 0;
                        try {
                            var local_res = InfoCMD.ExecuteScalar();
                            if (local_res != null)
                            {
                                var lr = local_res.ToString();
                                res = (QueryType.ConvertMethod == null) ? local_res.ToString() : QueryType.ConvertMethod(ref lr);
                            }
                            connection.Close();
                        }
                        catch (Exception ex) { Debug.WriteLine(ex); }   
                    }
                }
                catch { throw; }
            }
            return res;
        }


        internal bool? ReturnBoolRes()
        {
            string res = String.Empty; 
            using (SqlConnection connection = new SqlConnection(QueryType.Connection)) {
                try {
                    connection.Open();
                    using (SqlCommand InfoCMD = connection.CreateCommand()) {
                        InfoCMD.CommandText = ModifiedQuery;
                        InfoCMD.CommandTimeout = 0;
                        try {
                            var local_res = InfoCMD.ExecuteScalar();
                            if (local_res != null) {
                                res = local_res.ToString();
                            }
                            connection.Close();
                        }
                        catch (Exception ex) { Debug.WriteLine(ex); 
                        }
                    }
                }
                catch {
                    throw;  }
            }
            return (res is not null and "1") ? true : false;
        }

        /*private static void ReadSingleRow(IDataRecord dataRecord)
        {
            Debug.WriteLine(String.Format("{0}", dataRecord.GetString(0)));
        }*/
    }
}
