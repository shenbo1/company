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
    public class UserCalendarDBOperate
    {
        const string TableName = "UserCalendar";

        #region 添加
        public static bool AddUserCalendar(UserCalendar model)
        {
            string sql = string.Format("insert into {0}([CompanyId],[CompanyName],[Name],[StartDate],[EndDate],[UserId],[Type],[PId],[Status],[CreateDate],[CreateBy],[Title],[UserName],[IsDeleted],[Description])  values(@CompanyId,@CompanyName,@Name,@StartDate,@EndDate,@UserId,@Type,@PId,@Status,getdate(),@CreateBy,@Title,@UserName,0,@Description)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyUserCalendar(UserCalendar model)
        {
            string sql = string.Format(@"update {0} set [CompanyId]=@CompanyId,[CompanyName]=@CompanyName,[Name]=@Name,[StartDate]=@StartDate,[EndDate]=@EndDate,[UserId]=@UserId,[Type]=@Type,[PId]=@PId,[Status]=@Status,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy,[Title]=@Title
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static UserCalendar GetModelById(int Id)
        {
            string sql = string.Format(@"select [Id],[CompanyId],[CompanyName],[Name],[StartDate],[EndDate],[UserId],[Type],[PId],[Status],[CreateDate],[CreateBy],[ModifyBy],[Title] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<UserCalendar>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static UserCalendar GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[CompanyId],[CompanyName],[Name],[StartDate],[EndDate],[UserId],[Type],[PId],[Status],[CreateDate],[CreateBy],[ModifyBy],[Title],[Id],[CompanyId],[CompanyName],[Name],[StartDate],[EndDate],[UserId],[Type],[PId],[Status],[CreateDate],[CreateBy],[ModifyBy],[Title] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<UserCalendar>(sql, Name);
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
        public static List<UserCalendar> GetPagerList(QueryBase query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[CompanyId],A.[CompanyName],A.[Name],A.[StartDate],A.[EndDate],A.[UserId],A.[Type],A.[PId],A.[Status],A.[CreateDate],A.[CreateBy],A.[ModifyBy],A.[Title]";
            pager.WhereStr += " and A.[IsDeleted]=0 ";
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            var list = PagerDBOperate<UserCalendar>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        public static List<UserCalendar> GetList(string userId, DateTime start, DateTime end) {
            string sql = string.Format(" select * from {0} where IsDeleted = 0  ", TableName);
            var param = new DynamicParameters();
            if (!string.IsNullOrEmpty(userId)) {
                sql += " and UserId = @UserId ";
                param.Add("UserId", userId);
            }
            sql += " and StartDate>=@Start and EndDate<=@End";
            param.Add("Start", start);
            param.Add("End", end);
            return DBAccess.GetEntityList<UserCalendar>(sql, param);
        }

        public static bool UpdateDate(int type, int pid, DateTime start) {
            string sql = @"update UserCalendar set StartDate =@StartDate ,EndDate = @EndDate where Type=@Type and PId=@id";
            return DBAccess.ExecuteSqlWithEntity(sql, new { StartDate =start, EndDate=start,Type = type,id=pid });
        }
    }
}