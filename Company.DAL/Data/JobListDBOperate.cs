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
    public class JobListDBOperate
    {
        const string TableName = "JobList";

        #region 添加
        public static bool AddJobList(JobList model)
        {

            string sql = string.Format("insert into {0}([Name],[Index],[IsParent],[IsDeleted],[CreateDate],[CreateBy],[Price],[Ico],[IsPublish],[Info],[Level])  values(@Name,@Index,@IsParent,0,getdate(),@CreateBy,@Price,@Ico,@IsPublish,@Info,@Level)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyJobList(JobList model)
        {
            string sql = string.Format(@"update {0} set [Name]=@Name,[Index]=@Index,[IsParent]=@IsParent,[IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy,[Price]=@Price,[Ico]=@Ico,[IsPublish]=@IsPublish,[Info]=@Info,[Level]=@Level
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion
        #region 修改
        public static bool ModifyJobList(int model,string ModifyBy)
        {
            string sql = string.Format(@"update {0} set [IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, new { IsDeleted = 1 , ModifyBy = ModifyBy ,Id = model});
        }
        #endregion

        #region 获取单个对象
        public static JobList GetModelById(int Id)
        {
            string sql = string.Format(@"select [Id],[Name],[Index],[IsParent],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[Price],[Ico],[IsPublish],[Info],[Level] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<JobList>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static JobList GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[Name],[Index],[IsParent],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[Price],[Ico],[IsPublish],[Id],[Name],[Index],[IsParent],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[Price],[Ico],[IsPublish],[Info] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<JobList>(sql, Name);
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
        public static List<JobList> GetPagerList(QueryBase query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[Name],A.[Index],A.[IsParent],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[ModifyBy],A.[Price],A.[Ico],A.[IsPublish],a.[Info],a.[Level], isnull(b.[Name],'顶级') ParentName";
            pager.WhereStr += " and A.[IsDeleted]=0 ";
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            pager.JoinSql = string.Format( " left join {0} b on a.IsParent = b.Id ",TableName);
            var list = PagerDBOperate<JobList>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        public static List<JobList> GetList(int parentId) {
            string sql = string.Format(" select * from {0} where isdeleted = 0 and isparent = {1} ; ", TableName, parentId);
            return DBAccess.GetEntityList<JobList>(sql, new {  });
        }
        public static List<JobList> GetPublishList()
        {
            string sql = string.Format(" select * from {0} where isdeleted = 0 and ispublish = 1 order by [Index]; ", TableName);
            return DBAccess.GetEntityList<JobList>(sql, new { });
        }
    }
}