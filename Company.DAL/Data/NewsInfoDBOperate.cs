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
    public class NewsInfoDBOperate
    {
        const string TableName = "NewsInfo";

        #region 添加
        public static bool AddNewsInfo(NewsInfo model)
        {

            string sql = string.Format("insert into {0}([Name],[Tag],[MainImg],[Infos],[PublishDate],[PublishName],[IsDeleted],[CreateDate],[CreateBy],[GUID],[IsPublish],[Index],[Desc])  values(@Name,@Tag,@MainImg,@Infos,@PublishDate,@PublishName,0,getdate(),@CreateBy,@GUID,@IsPublish,@Index,@Desc)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyNewsInfo(NewsInfo model)
        {
            string sql = string.Format(@"update {0} set [Name]=@Name,[Tag]=@Tag,[MainImg]=@MainImg,[Infos]=@Infos,[PublishDate]=@PublishDate,[PublishName]=@PublishName,[IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy,[GUID]=@GUID,[IsPublish]=@IsPublish,[Index]=@Index,[Desc]=@Desc
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static NewsInfo GetModelById(int Id)
        {
            string sql = string.Format(@"select A.[Id],A.[Name],A.[Tag],A.[MainImg],A.[Infos],A.[PublishDate],A.[PublishName],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[ModifyBy],A.[GUID],a.[ISPUBLISH],a.[Index],a.[Desc] from {0} a (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<NewsInfo>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static NewsInfo GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[Name],[Tag],[MainImg],[Infos],[PublishDate],[PublishName],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[GUID],[Id],[Name],[Tag],[MainImg],[Infos],[PublishDate],[PublishName],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[GUID] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<NewsInfo>(sql, Name);
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
        public static List<NewsInfo> GetPagerList(QueryBase query,bool IsPublish, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[Index]",Direction = Direction.ASC };
            pager.Columns = @"A.[Id],A.[Name],A.[Tag],A.[MainImg],A.[Infos],A.[PublishDate],A.[PublishName],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[ModifyBy],A.[GUID],a.[ISPUBLISH],a.[Index],a.[Desc]";
            pager.WhereStr += " and A.[IsDeleted]=0 ";
            if (IsPublish) { pager.WhereStr += " and a.[ispublish]=1 "; }
            pager.Direction = Direction.ASC;
            if (!string.IsNullOrEmpty(query.Sort))
                pager.ColName = string.Format(" a.[{0}] ", query.Sort);

            if (!string.IsNullOrEmpty(query.Order))
                 pager.Direction = Direction.DESC;

            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            var list = PagerDBOperate<NewsInfo>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        public static int GetPublishCount() {
            string sql = string.Format("select count(0) from {0} where isdeleted=0 and ispublish = 1", TableName);
            return DBAccess.ExecuteSql<int>(sql);
        }
    }
}