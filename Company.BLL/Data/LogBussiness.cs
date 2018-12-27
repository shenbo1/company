using Company.DAL.Data;
using Company.Dto;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Data
{
    public class LogBussiness
    {
        #region 日志
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="request">请求</param>
        /// <param name="content">日志内容</param>
        /// <param name="logLevel">日志等级</param>
        public static void AddLog(string key, string request, string content, LogLevel logLevel = LogLevel.Warn)
        {
            SystemLog log = new SystemLog()
            {
                Ip = CommonMethod.GetClientIP,
                MainContent = content,
                MainKey = key,
                MainRequest = request,
                LogLevel = logLevel.ToString(),
            };
            var user = CookieOperate.MemberCookie;
            if (user != null) { log.UserId = user.Id; log.UserName = user.Name; }
            SystemLogDBOperate.AddSystemLog(log);
        }
        #endregion
    }
}
