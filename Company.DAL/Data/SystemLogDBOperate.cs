using Company.DAL.Common;
using Company.Dto;
using Company.Dto.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data
{   
    public class SystemLogDBOperate
    {
        const string TableName = "SystemLog";

        #region 添加
        public static bool AddSystemLog(SystemLog model)
        {
            string sql = string.Format("insert into {0}([UserId],[UserName],[MainKey],[MainRequest],[MainContent],[CreateDate],[Ip],[LogLevel])  values(@UserId,@UserName,@MainKey,@MainRequest,@MainContent,getdate(),@Ip,@LogLevel)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

    }
}