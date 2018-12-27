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
    public class AuditStepDBOperate
    {
        const string TableName = "AuditStep";

        #region 添加
        public static bool AddAuditStep(AuditStep model)
        {
            string sql = string.Format("insert into {0}([DepartId],[Step],[UserId],[CompanyId],[CompanyName],[CreateDate])  values(@DepartId,@Step,@UserId,@CompanyId,@CompanyName,getdate())", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyAuditStep(AuditStep model)
        {
            string sql = string.Format(@"update {0} set [DepartId]=@DepartId,[Step]=@Step,[UserId]=@UserId,[CompanyId]=@CompanyId,[CompanyName]=@CompanyName,[ModifyDate]=getdate()
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion
      
        #region 获取单个对象
        public static AuditStep GetModelById(int Id)
        {
            string sql = string.Format(@"select [Id],[DepartId],[Step],[UserId],[CompanyId],[CompanyName],[CreateDate] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<AuditStep>(sql, Id);
        }
        #endregion

        #region 获取分页列表  
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagesize">每页显示多少行</param>
        /// <param name="offset">跳过行数</param>
        /// <param name="key">关键词</param>
        /// <param name="totalcount">输出行数</param>
        /// <param name="GroupId">输出行数</param>
        /// <returns></returns>
        public static List<AuditStep> GetPagerList(int pagesize, int offset, string name, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = offset, PageSize = pagesize, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[DepartId],A.[Step],A.[UserId],A.[CompanyId],A.[CompanyName],A.[CreateDate]";
            pager.WhereStr += " and A.[Deleted]=@Deleted";
            param.Add("Deleted", 0);
            if (!string.IsNullOrEmpty(name))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + name + "%");
            }
            var list = PagerDBOperate<AuditStep>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        #region 审核步骤
        public static bool AuditStepAdd(string Content, int DepartMent,int CompanyId,string CompanyName) 
        {
            //10002,管理部-Boss:Jeo Yang,1|10000,技术部-程序员:沈波,2|
            string[] array = Content.Split('|');
            List<AuditStep> list = new List<AuditStep>();
            for (int i = 0; i < array.Length-1; i++)
            {
                var name = array[i].Split(',');
                list.Add(new AuditStep() { 
                    CompanyId = CompanyId,
                    CreateDate = DateTime.Now,
                    CompanyName = CompanyName,
                    DepartId = DepartMent,
                    UserId = Convert.ToInt32( name[0]),
                    Step = Convert.ToInt32(name[2])
                });
            }
            using (SqlConnection con = new SqlConnection(ConfigSetting.DataConnection))
            {
                con.Open();
                var transaction = con.BeginTransaction();
                try
                {
                    //清除之前记录
                    string deleteSql = "delete from [AuditStep] where CompanyId = @CompanyId and DepartId=@DepartId";
                    con.Execute(deleteSql, new { CompanyId = CompanyId, DepartId = DepartMent }, transaction);
                    //添加审核流程
                    string insertSql = string.Format("insert into {0}([DepartId],[Step],[UserId],[CompanyId],[CompanyName],[CreateDate])  values(@DepartId,@Step,@UserId,@CompanyId,@CompanyName,getdate())", TableName);
                    con.Execute(insertSql, list, transaction);
                    //添加冗余字段
                    string updateSql = "update [CompanyDepartMent] set [AuditStep] = @AuditStep,[ModifyDate]=getdate() where CompanyId = @CompanyId and Id=@DepartId";
                    con.Execute(updateSql, new
                    {
                        AuditStep = Content,
                        CompanyId = CompanyId,
                        DepartId = DepartMent 
                    }, transaction);
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 获取某个部门的审核详情
        public static List<AuditStep> GetAuditStepByDepartId(int DepartId) 
        {
            string sql = "select Id,DepartId,[Step],[UserId] from AuditStep(nolock) where DepartId=@DepartId order by [Step]";
            return DBAccess.GetEntityList<AuditStep>(sql, new { DepartId = DepartId });
        }
        #endregion
    }
}