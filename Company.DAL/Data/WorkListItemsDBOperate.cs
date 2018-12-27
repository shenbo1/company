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
    public class WorkListItemsDBOperate
    {
        const string TableName = "WorkListItems";

        #region 添加
        public static bool AddWorkListItems(WorkListItems model)
        {
            string sql = string.Format("insert into {0}([CompanyId],[CompanyName],[ProjectId],[WorkId],[Name],[Level],[Status],[UserId],[UserName],[StartDate],[EndDate],[Infos],[CreateDate],[CreateBy],[IsDeleted],[DayCount],[Type])  values(@CompanyId,@CompanyName,@ProjectId,@WorkId,@Name,@Level,@Status,@UserId,@UserName,@StartDate,@EndDate,@Infos,getdate(),@CreateBy,0,@DayCount,@Type)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        public static bool AddWorkListItems(List<WorkListItems> model)
        {
            string sql = string.Format("insert into {0}([CompanyId],[CompanyName],[ProjectId],[WorkId],[Name],[Level],[Status],[Infos],[CreateDate],[CreateBy],[IsDeleted],[DayCount],[DetailId],[DetailGuid])  values(@CompanyId,@CompanyName,@ProjectId,@WorkId,@Name,@Level,@Status,@Infos,getdate(),@CreateBy,0,@DayCount,@DetailId,@DetailGuid)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion
        
        #region 修改
        public static bool ModifyWorkListItems(WorkListItems model)
        {
            string sql = string.Format(@"update {0} set [CompanyId]=@CompanyId,[CompanyName]=@CompanyName,[ProjectId]=@ProjectId,[WorkId]=@WorkId,[Name]=@Name,[Level]=@Level,[Status]=@Status,[UserId]=@UserId,[UserName]=@UserName,[StartDate]=@StartDate,[EndDate]=@EndDate,[Infos]=@Infos,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy,[DayCount]=@DayCount,[Type]=@Type
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        public static bool DeleteWorkListItems(WorkListItems model) {
            string sql = string.Format(@"update {0} set [IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        public static bool ModifyWorkListItemsStatus(WorkListItems model)
        {
            string sql = string.Format(@"update {0} set [Status]=@Status,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy
            ", TableName);
            if (model.RelStartDate != DateTime.MinValue)
                sql += " ,[RelStartDate]=@RelStartDate";
            if (model.RelEndDate != DateTime.MinValue)
                sql += " ,[RelEndDate]=@RelEndDate";
            sql += " where Id=@Id";
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static WorkListItems GetModelById(int Id)
        {
            string sql = string.Format(@"select * from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<WorkListItems>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static WorkListItems GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[CompanyId],[CompanyName],[ProjectId],[WorkId],[Name],[Level],[Status],[UserId],[UserName],[StartDate],[EndDate],[RelStartDate],[RelEndDate],[Infos],[CreateDate],[CreateBy],[ModifyBy],[Id],[CompanyId],[CompanyName],[ProjectId],[WorkId],[Name],[Level],[Status],[UserId],[UserName],[StartDate],[EndDate],[RelStartDate],[RelEndDate],[Infos],[CreateDate],[CreateBy],[ModifyBy] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<WorkListItems>(sql, Name);
        }
        #endregion

        public static List<WorkListItems> GetModelByWorkId(int id) {
            string sql = string.Format("select * from {0} (nolock) where WorkId=@WorkId",TableName);
            return DBAccess.GetEntityList<WorkListItems>(sql, new { WorkId = id });
        }

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
        public static List<WorkListItems> GetPagerList(WorkListItemQuery query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[ProjectId],C.Name ProjectName,A.[WorkId],B.Name WorkName,D.Infos Name,A.[Level],A.[Status],A.[UserId],A.[UserName],A.[StartDate],A.[EndDate],A.[RelStartDate],A.[RelEndDate],A.[Infos],a.[DayCount],a.[Type]";
            pager.WhereStr += " and A.[IsDeleted]=0 and a.CompanyId=@CompanyId";
            pager.JoinSql = "left join WorkList (nolock) B on a.[workId]=B.[ID]";
            pager.JoinSql += "left join ProjectManage (nolock) C on a.[ProjectId]=C.[ID]";
            pager.JoinSql += "left join WorkListDetail (nolock) D on a.DetailGUID=D.[GUID]";
            param.Add("CompanyId", query.CompanyId.ToString());
            if(!string.IsNullOrEmpty( query.Order))
                pager.Direction = query.Order == "asc" ? Direction.ASC : Direction.DESC;
            if(!string.IsNullOrEmpty(query.Sort))
                pager.ColName = string.Format("a.[{0}]", query.Sort);
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            if (!string.IsNullOrEmpty(query.Status)) {

                pager.WhereStr += " and A.[Status] in @Status";
                param.Add("Status",  query.Status.Split(','));
            }
            if (!string.IsNullOrEmpty(query.Level))
            {
                pager.WhereStr += " and A.[Level]=@Level";
                param.Add("Level", query.Level);
            }
            if (query.WorkId!=0)
            {
                pager.WhereStr += " and A.[WorkId]=@WorkId";
                param.Add("WorkId", query.WorkId);
            }
            if (!string.IsNullOrEmpty(query.User))
            {
                var users = query.User.Split(',');
                pager.WhereStr += " and (";
                for (int i = 0; i < users.Length - 1; i++)
                {
                    pager.WhereStr += " A.[UserId] like '%"+users[i]+"%' ";
                    if (i != users.Length - 2) { pager.WhereStr += " or ";  }
                }
                pager.WhereStr += " )";
            }
            var list = PagerDBOperate<WorkListItems>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        /// <summary>
        /// 获取某人时间安排
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<WorkListItems> getUserDayCount(string user)
        {
            string sql = @"select Level,sum(daycount) DayCount from WorkListItems (nolock)
 where userid like '%" + user + @"%' and [IsDeleted]=0
 group by Level
 order by level
";
            return DBAccess.GetEntityList<WorkListItems>(sql);
        }

        public static List<WorkListItems> getUserDayCount(string user,int[] level)
        {
            string sql = @"select Level,sum(daycount) DayCount from WorkListItems (nolock)
 where userid like '%" + user + @"%' and [IsDeleted]=0 and level in @Level
 group by Level
 order by level
";
            return DBAccess.GetEntityList<WorkListItems>(sql,new { Level = level });
        }

        public static int getCountByDetailId(string DetailGuid) {
            string sql = "select count(0) from WorkListItems(nolock) where isdeleted=0 and DetailGuid=@DetailGuid";
            return DBAccess.GetEntityList<int>(sql, new { DetailGuid = DetailGuid }).FirstOrDefault();
        }

        public static List<WorkListItems> GetWeekModifyList() {
            string sql = "select * from WorkListItems(nolock) where isdeleted=0 and [Level] <>999 and [Status]<>'End' and  [Status] <> 'YuEnd'";
            return DBAccess.GetEntityList<WorkListItems>(sql);
        }
        public static bool ModifyWeekList(int[] id) {
            string sql = "update WorkListItems set [Level] = [Level] - 1 where Id in @Ids";
            return DBAccess.ExecuteSqlWithEntity(sql,new { Ids = id });
        }
        public static bool ModifyItemList(string userid, string username,int pid)
        {
            string sql = @"update WorkListItems set UserId=@UserId,UserName=@UserName where WorkId=@WorkId";
            return DBAccess.ExecuteSqlWithEntity(sql, new { UserId = userid, UserName = username, WorkId = pid });
        }
        public static List<WorkListItems> GetList(string[] guid) {
            string sql = @"select * from WorkListItems(nolock) where DetailGuid in @Guid and isDeleted=0;";
            return DBAccess.GetEntityList<WorkListItems>(sql, new { Guid =guid });
        }

        
    }
}