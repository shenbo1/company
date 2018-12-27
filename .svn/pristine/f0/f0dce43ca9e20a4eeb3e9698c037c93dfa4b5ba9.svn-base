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
    public class CompanyInfoDBOperate
    {
        const string TableName = "CompanyInfo";

        #region 添加
        public static bool AddCompanyInfo(CompanyInfo model)
        {

            string sql = string.Format("insert into {0}(CompanyName,CompanyFullName,Address,Way,Name,Mobile,CreateDate,EndTime,LastPayTime,IsDeleted)  values(@CompanyName,@CompanyFullName,@Address,@Way,@Name,@Mobile,getdate(),@EndTime,@LastPayTime,0)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyCompanyInfo(CompanyInfo model)
        {
            string sql = string.Format(@"update {0} set [CompanyName]=@CompanyName,[CompanyFullName]=@CompanyFullName,[Address]=@Address,[Way]=@Way,[Name]=@Name,[Mobile]=@Mobile,[EndTime]=@EndTime,[LastPayTime]=@LastPayTime,[IsDeleted]=@IsDeleted,[ModifyDate]=getdate()
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static CompanyInfo GetModelById(int Id)
        {
            string sql = string.Format(@"select Id,CompanyName,CompanyFullName,Address,Way,Name,Mobile,CreateDate,EndTime,LastPayTime,IsDeleted from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<CompanyInfo>(sql, Id);
        }
        public static CompanyInfo GetModelByCompanyName(string name)
        {
            string sql = string.Format(@"select Id,CompanyName,CompanyFullName,Address,Way,Name,Mobile,CreateDate,EndTime,LastPayTime,IsDeleted from {0} (nolock) where CompanyName=@CompanyName", TableName);
            return DBAccess.GetEntityFirstOrDefault<CompanyInfo>(sql, new { CompanyName = name });
        }
        #endregion

        #region 获取所有数据
        public static List<CompanyInfo> GetList(int CompanyId)
        {
            string sql = string.Format(@"select Id,CompanyName,CompanyFullName,Address,Way,Name,Mobile,CreateDate,EndTime,LastPayTime,IsDeleted from {0} (nolock) where 1=1", TableName);
            var param = new DynamicParameters();
            if (CompanyId != 0)
            {
                sql += " and Id = @CompanyId";
                param.Add("CompanyId", CompanyId);
            }
            return DBAccess.GetEntityList<CompanyInfo>(sql, param);
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
        public static List<CompanyInfo> GetPagerList(int pagesize, int offset, string name, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = offset, PageSize = pagesize, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[CompanyName],A.[CompanyFullName],A.[Address],A.[Way],A.[Name],A.[Mobile],A.[CreateDate],A.[EndTime],A.[LastPayTime],A.[IsDeleted]";
            if (!string.IsNullOrEmpty(name))
            {
                pager.WhereStr += " and (A.[CompanyName] like @Name or A.[Name] like @Name or  A.[CompanyFullName] like @Name)";
                param.Add("Name", "%" + name + "%");
            }
            var list = PagerDBOperate<CompanyInfo>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion
    }
}
