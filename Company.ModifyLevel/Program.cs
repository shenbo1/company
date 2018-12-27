using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Data;
using Company.DAL;
using Company.DAL.Data;
using Company.Util;

namespace Company.ModifyLevel
{
    class Program
    {
        public static void Main(string[] args)
        {
            var list = WorkListItemsDBOperate.GetWeekModifyList();
            var ids = list.Select(a => a.Id).ToArray();
            WorkListItemsDBOperate.ModifyWeekList(ids);
            SystemLogDBOperate.AddSystemLog(new Dto.SystemLog()
            {
                CreateDate = DateTime.Now,
                LogLevel = LogLevel.Success.ToString(),
                MainContent = CommonMethod.ToJson(ids),
                MainKey = "WeekModify",
                UserName = "SYS",
                MainRequest = "系统自动更新工单时间"
            });
        }
    }
}
