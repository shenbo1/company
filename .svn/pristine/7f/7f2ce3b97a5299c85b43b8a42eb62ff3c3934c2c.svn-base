using Company.DAL.Common;
using Company.Dto;
using Company.Dto.Model;
using Company.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data
{
    public class AuditMainDBOperate
    {
        const string TableName = "AuditMain";
        const string DetailTableName = "AuditDetail";

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool AuditMainAdd(AuditMain model) 
        {
            using (SqlConnection con = new SqlConnection(ConfigSetting.DataConnection))
            {
                con.Open();
                var transaction = con.BeginTransaction();
                try
                {
                    string sql = string.Format("insert into {0}([UserId],[Money],[ConfigId],[CompanyId],[CreateDate],[IsDeleted],[Status],[OperateId],[Remark],[MainId],[Images],[NextOperateId],[NowCount],[TotalCount])  values(@UserId,@Money,@ConfigId,@CompanyId,getdate(),0,@Status,@OperateId,@Remark,@MainId,@Images,@NextOperateId,@NowCount,@TotalCount)", TableName);
                    con.Execute(sql, model,transaction);
                    sql = string.Format("insert into {0}([UserId],[Step],[AuditMainId],[CompanyId],[Remark],[OperateBy],[Status],[CreateDate],[ModifyDate])  values(@UserId,@Step,@AuditMainId,@CompanyId,@Remark,@OperateBy,@Status,getdate(),getdate())", DetailTableName);
                    con.Execute(sql, model.DetailList, transaction);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SystemLogDBOperate.AddSystemLog(new SystemLog()
                    {
                        Ip = Util.CommonMethod.GetClientIP,
                        LogLevel = LogLevel.Error.ToString(),
                        MainContent = ex.Message,
                        MainKey = "AuditMainAdd",
                        MainRequest = Util.CommonMethod.ToJson(model),
                        UserId = model.UserId,
                    });
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region 获取我发起的审核
        public  static List<AuditMain> GetMyAuditMain(int userId){
            string sql = @"select a.Id,a.Remark,a.Money,a.Status,a.NextOperateId,b.Name NextOperateName,a.CreateDate,a.ConfigId ,d.Value+' - '+c.Value ConfigName
from AuditMain(nolock) a
left join CompanyUser(nolock) b
on a.NextOperateId = b.Id
left join FinanceConfig(nolock) c 
on a.ConfigId = c.Id
left join FinanceConfig(nolock)d
on c.PId=d.Id
where a.UserId=@UserId
Order By a.Id desc";
            return DBAccess.GetEntityList<AuditMain>(sql, new { UserId = userId });
        }
        #endregion

        #region 获取单个审核详情
        /// <summary>
        /// 获取单个审核详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static AuditMain GetAuditMainById(int id) 
        {
            string sql = @"select a.MainId,a.Remark,a.Money,a.Status,a.NextOperateId,b.Name NextOperateName,a.CreateDate,a.ConfigId ,d.Value+' - '+c.Value ConfigName,
a.Images
from AuditMain(nolock) a
left join CompanyUser(nolock) b
on a.NextOperateId = b.Id
left join FinanceConfig(nolock) c 
on a.ConfigId = c.Id
left join FinanceConfig(nolock)d
on c.PId=d.Id
where a.Id=@Id";
            return DBAccess.GetEntityFirstOrDefault<AuditMain>(sql, new { Id = id });
        }
        #endregion

        #region 获取审核详情
        /// <summary>
        /// 获取审核详情
        /// </summary>
        /// <param name="mainId"></param>
        /// <returns></returns>
        public static List<AuditDetail> GetAuditDetailList(string mainId) {
            string sql = @"select [Id],[UserId],[Step],[Remark],OperateBy,Status,CreateDate,ModifyDate 
from AuditDetail(nolock) 
where AuditMainId=@AuditMainId and status!=0";
            return DBAccess.GetEntityList<AuditDetail>(sql, new { AuditMainId = mainId });

        }
        #endregion

    }
}