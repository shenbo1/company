using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace CvtUtil
{
    public static class JsonUtil
    {
        //DataTable转Json
        public static string toJson(this DataTable dt)
        {
            return JsonConvert.SerializeObject(dt);
        }

        //DataRow转Json
        public static string toJson(this DataRow row)
        {
            DataTable dt = row.Table.Clone();
            DataRow nrow = dt.NewRow();
            nrow.ItemArray = row.ItemArray;
            dt.Rows.Add(nrow);
            return JsonConvert.SerializeObject(dt).Trim('[', ']');
        }

        //List<Model>转Json
        public static string toJson<T>(this List<T> list)
        {
            return JsonConvert.SerializeObject(list);
        }

        //Model转Json
        public static string toJson<T>(this T model)
        {
            return JsonConvert.SerializeObject(model);
        }

        //Json转Model
        public static T toModel<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        //Json转List<Model>
        public static List<T> toList<T>(this string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        //Json转DataRow
        public static DataRow toDataRow(this string json)
        {
            string jsonstr = "[" + json + "]";
            return JsonConvert.DeserializeObject<DataTable>(jsonstr).Rows[0];
        }

        //Json转DataTable
        public static DataTable toDataTable(this string json)
        {
            return JsonConvert.DeserializeObject<DataTable>(json);
        }

        //DataRow转Model
        public static T toModel<T>(this DataRow row)
        {
            string json = row.toJson();
            return json.toModel<T>();
        }

        //DataTable转List<Model>
        public static List<T> toList<T>(this DataTable dt)
        {
            string json = JsonConvert.SerializeObject(dt);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        //DataReader存入dynamic/外部须包裹while(DataReader.Read()){}
        public static void saveToDynamic(SqlDataReader rd, dynamic model)
        {
            for (int i = 0; i < rd.FieldCount; i++)
            {
                string col = rd.GetName(i).Trim();
                (model as IDictionary<string, object>).Add(col, rd[col].ToString().Trim());
            }
        }
    }
}
