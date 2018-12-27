using Company.Dto.Model;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Company.DAL.Common
{
    public class PagerDBOperate<T>
    {
        public static PagerDBOperate<T> Init { get { return new PagerDBOperate<T>(); } }

        #region 通用分页查询
        /// <summary>
        /// 通用分页查询
        /// </summary>
        /// <param name="pager">Pager类</param>
        /// <param name="param">参数</param>
        /// <param name="totalcount">返回值，总条数</param>
        /// <returns></returns>
        public List<T> GetList(Pager pager, object param, out int totalcount)
        {
            Type t = typeof(T);
            if (string.IsNullOrEmpty(pager.TableName))
                pager.TableName = t.Name;

            totalcount = 0;

            string sqlCount = pager.GetTotalCountSql();//数量
            string sqlList = pager.GetDataSql();//数据
            using (SqlConnection con = new SqlConnection(ConfigSetting.DataConnection))
            {
                totalcount = Convert.ToInt32(con.ExecuteScalar<int>(sqlCount, param).ToString());
                var list = con.Query<T>(sqlList, param).ToList();
                return list;
            }
        }
        #endregion
    }
}
