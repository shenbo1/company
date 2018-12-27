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
    public class RoleInfoDBOperate
    {
        const string TableName = "RoleInfo";

        #region 添加
        public static bool AddRoleInfo(RoleInfo model)
        {

            string sql = string.Format("insert into {0}(RoleName,CreateDate)  values(@RoleName,getdate())", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyRoleInfo(RoleInfo model)
        {
            string sql = string.Format(@"update {0} set [RoleName]=@RoleName
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static RoleInfo GetModelById(int Id)
        {
            string sql = string.Format(@"select Id,RoleName,CreateDate from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<RoleInfo>(sql, Id);
        }
        #endregion

        #region 获取所有数据
        public static List<RoleInfo> GetList(int companyId)
        {
            string sql = string.Format(@"select Id,RoleName,CreateDate from {0} (nolock) where 1= 1", TableName);
            var param = new DynamicParameters();
            if (companyId != 0)
            {
                sql += " and Id != 10006 ";
            }
            return DBAccess.GetEntityList<RoleInfo>(sql, new { });
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
        public static List<RoleInfo> GetPagerList(int pagesize, int offset, string name, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = offset, PageSize = pagesize, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[RoleName],A.[CreateDate]";
            pager.Direction = Direction.ASC;
            if (!string.IsNullOrEmpty(name))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + name + "%");
            }
            var list = PagerDBOperate<RoleInfo>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

    }
}