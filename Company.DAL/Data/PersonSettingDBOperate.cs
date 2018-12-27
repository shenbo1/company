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
    public class PersonSettingDBOperate
    {
        const string TableName = "PersonSetting";

        #region 添加
        public static bool AddPersonSetting(PersonSetting model)
        {

            string sql = string.Format("insert into {0}([IsDeleted],[CreateDate],[CreateBy],[WorkGuid],[Price],[DepartId],[DepartName],[Infos],[Month],[TotalPrice],[Discount])  values(0,getdate(),@CreateBy,@WorkGuid,@Price,@DepartId,@DepartName,@Infos,@Month,@TotalPrice,@Discount)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyPersonSetting(PersonSetting model)
        {
            string sql = string.Format(@"update {0} set [IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[WorkGuid]=@WorkGuid,[Price]=@Price,[DepartId]=@DepartId,[DepartName]=@DepartName,[Infos]=@Infos,[Month]=@Month,[TotalPrice]=@TotalPrice,[Discount]=@Discount
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static PersonSetting GetModelById(int Id)
        {
            string sql = string.Format(@"select [Id],[IsDeleted],[CreateDate],[CreateBy],[WorkGuid],[Price],[DepartId],[DepartName],[Infos],[Month],[TotalPrice],[Discount] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<PersonSetting>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static PersonSetting GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[IsDeleted],[CreateDate],[CreateBy],[WorkGuid],[Price],[DepartId],[DepartName],[Infos],[Month],[TotalPrice],[Discount],[Id],[IsDeleted],[CreateDate],[CreateBy],[WorkGuid],[Price],[DepartId],[DepartName],[Infos],[Month],[TotalPrice],[Discount] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<PersonSetting>(sql, Name);
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
        public static List<PersonSetting> GetPagerList(QueryBase query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[WorkGuid],A.[Price],A.[DepartId],A.[DepartName],A.[Infos],A.[Month],A.[TotalPrice],A.[Discount]";
            pager.WhereStr += " and A.[IsDeleted]=0";
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            var list = PagerDBOperate<PersonSetting>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion
    }
}