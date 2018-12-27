using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Dto;
using Company.Util;
namespace Company.DAL
{
    public  class WeChatInfoDBOperate
    {
        const string TableName = "T_WeChat";

        #region 根据项目名获取微信信息
        public static WeChatInfo GetWeChatInfoByProjectName(string projectName) 
        {
            string sql = string.Format("select * from {0}(nolock) where ProjectName=@ProjectName", TableName);
            return DBAccess.GetEntityFirstOrDefault<WeChatInfo>(sql, ConfigSetting.EeeYoocompany, new { ProjectName = projectName });
        }
        #endregion
    }
}
