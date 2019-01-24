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
    public class FinanceConfigPermissionDBOperate
    {
        const string TableName = "FinanceConfigPermission";

        #region 添加
        public static bool AddFinanceConfigPermission(FinanceConfigPermission model)
        {

            string sql = string.Format("insert into {0}([DepartId],[FinanceConfigId])  values(@DepartId,@FinanceConfigId)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyFinanceConfigPermission(FinanceConfigPermission model)
        {
            string sql = string.Format(@"update {0} set [DepartId]=@DepartId,[FinanceConfigId]=@FinanceConfigId
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static FinanceConfigPermission GetModelById(int Id)
        {
            string sql = string.Format(@"select [Id],[DepartId],[FinanceConfigId] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<FinanceConfigPermission>(sql, Id);
        }
        #endregion

        #region 获取单个对象
        public static List<FinanceConfigPermission> GetList(int DepartId)
        {
            string sql = string.Format(@"select [Id],[DepartId],[FinanceConfigId] from {0} (nolock) where DepartId=@DepartId", TableName);
            return DBAccess.GetEntityList<FinanceConfigPermission>(sql, new { DepartId = DepartId });
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
        public static List<FinanceConfigPermission> GetPagerList(int pagesize, int offset, string name, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = offset, PageSize = pagesize, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[DepartId],A.[FinanceConfigId]";
            pager.WhereStr += " and A.[Deleted]=@Deleted";
            param.Add("Deleted", 0);
            if (!string.IsNullOrEmpty(name))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + name + "%");
            }
            var list = PagerDBOperate<FinanceConfigPermission>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        #region 设定
        public static bool Set(int departId, string IDs) 
        {
            string[] myIds = IDs.Split(',');
            List<FinanceConfigPermission> list = new List<FinanceConfigPermission>();
            foreach (var item in myIds)
            {
                list.Add(new FinanceConfigPermission()
                {
                    DepartId = departId,
                    FinanceConfigId = Convert.ToInt32(item)
                });
            }
            using (SqlConnection con = new SqlConnection(ConfigSetting.DataConnection))
            {
                con.Open();
                var transaction = con.BeginTransaction();   
                try
                {
                    //清除之前记录
                    string deleteSql = "delete from [FinanceConfigPermission] where DepartId=@DepartId";
                    con.Execute(deleteSql, new { DepartId = departId }, transaction);
                    //添加审核流程
                    string insertSql = string.Format("insert into {0}([DepartId],[FinanceConfigId])  values(@DepartId,@FinanceConfigId)", TableName);
                    con.Execute(insertSql, list, transaction);
                   
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

     

    }
}