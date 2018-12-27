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
    public class WorkListDetailDBOperate
    {
        const string TableName = "WorkListDetail";

        #region 添加
        public static bool AddWorkListDetail(WorkListDetail model)
        {

            string sql = string.Format("insert into {0}([WorkId],[Infos],[RoleCode],[IsDeleted],[CreateDate],[CreateBy])  values(@WorkId,@Infos,@RoleCode,0,getdate(),@CreateBy)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyWorkListDetail(WorkListDetail model)
        {
            string sql = string.Format(@"update {0} set [WorkId]=@WorkId,[Infos]=@Infos,[RoleCode]=@RoleCode,[IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        public static bool DeleteWorkListDetail(String id) {
            string sql = string.Format(@"update {0} set [IsDeleted]=1
            where GUID=@ID", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, new {Id= id });
        }
        #endregion

        #region 获取单个对象
        public static WorkListDetail GetModelById(int Id)
        {
            string sql = string.Format(@"select [Id],[WorkId],[Infos],[RoleCode],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<WorkListDetail>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static WorkListDetail GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[WorkId],[Infos],[RoleCode],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<WorkListDetail>(sql, Name);
        }
        public static List<WorkListDetail> GetModelByNames(string[] names,string WorkId) {
            string sql = string.Format(@"select [Id],[Infos],[Guid] from {0} (nolock) where Infos in @Name and isdeleted=0 and workid=@WorkId", TableName);
            return DBAccess.GetEntityList<WorkListDetail>(sql, new { Name = names , WorkId  = WorkId });
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
        public static List<WorkListDetail> GetPagerList(QueryBase query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[WorkId],A.[Infos],A.[RoleCode],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[ModifyBy]";
            pager.WhereStr += " and A.[IsDeleted]=0 and a.CompanyId=@CompanyId";
            param.Add("CompanyId", query.CompanyId.ToString());
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            var list = PagerDBOperate<WorkListDetail>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        public static List<WorkListDetail> GetList(string[] guid) {
            string sql = string.Format(@"select * from {0} (nolock) where WorkId in @guid and isdeleted=0 order by [index]", TableName);
            return DBAccess.GetEntityList<WorkListDetail>(sql, new { guid = guid });
        }
       
    }
}