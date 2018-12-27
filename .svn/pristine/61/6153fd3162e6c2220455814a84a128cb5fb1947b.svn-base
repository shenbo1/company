using Company.DAL.Common;
using Company.Dto;
using Company.Dto.Model;
using Company.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data
{
    public class FinanceReportDBOperate
    {
        const string TableName = "FinanceReport";

        #region 添加
        public static bool AddFinanceReport(FinanceReport model)
        {
            string sql = string.Format("insert into {0}(ReportTypeId,ReportTypeName,Abstract,Blance,Type,Remark,CreateBy,DateTime,Operater,Payer,CreateDate,IsDeleted)  values(@ReportTypeId,@ReportTypeName,@Abstract,@Blance,@Type,@Remark,@CreateBy,@DateTime,@Operater,@Payer,getdate(),0)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyFinanceReport(FinanceReport model)
        {
            string sql = string.Format(@"update {0} set [ReportTypeId]=@ReportTypeId,[ReportTypeName]=@ReportTypeName,[Abstract]=@Abstract,[Blance]=@Blance,[Type]=@Type,[Remark]=@Remark,[DateTime]=@DateTime,[Operater]=@Operater,[Payer]=@Payer,[IsDeleted]=@IsDeleted,[ModifyDate]=getdate()
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 删除
        public static bool DeleteFinanceReport(FinanceReport model)
        {
            string sql = string.Format(@"update {0} set [Operater]=@Operater,[IsDeleted]=1,[ModifyDate]=getdate()
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static FinanceReport GetModelById(int Id)
        {
            string sql = string.Format(@"select Id,ReportTypeId,ReportTypeName,Abstract,Blance,Type,Remark,CreateBy,DateTime,Operater,Payer,CreateDate,IsDeleted from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<FinanceReport>(sql, Id);
        }
        #endregion

        #region 获取所有数据
        public static List<FinanceReport> GetList(QueryBase model)
        {

            string whereStr = "";
            var param = GetWhereStr(model, out whereStr);
            string sql = string.Format(@"select Id,ReportTypeId,ReportTypeName,Abstract,Blance,Type,Remark,CreateBy,DateTime,Operater,Payer,CreateDate,IsDeleted from {0} (nolock) " + whereStr +" order by DateTime", TableName);
            return DBAccess.GetEntityList<FinanceReport>(sql, param);

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
        public static List<FinanceReport> GetPagerList(QueryBase model, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = model.Offset, PageSize = model.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[ReportTypeId],A.[ReportTypeName],A.[Abstract],A.[Blance],A.[Type],A.[Remark],A.[CreateBy],A.[DateTime],A.[Operater],A.[Payer],A.[CreateDate],A.[IsDeleted]";
            pager.WhereStr += " and A.[IsDeleted]= 0 ";
            if (!string.IsNullOrEmpty(model.KeyWord))
            {
                pager.WhereStr += " and (A.[Abstract] like @Name or A.[Remark] like @Name )";
                param.Add("Name", "%" + model.KeyWord.Trim() + "%");
            }
            if (model.TypeId != 0) {
                pager.WhereStr += " and A.[ReportTypeId]=@TypeId";
                param.Add("TypeId", model.TypeId);
            }
            if (model.CompanyId != 0)
            {
                pager.WhereStr += " and A.[CompanyId]=@CompanyId";
                param.Add("CompanyId", model.CompanyId);
            }
            if (model.StartTime != DateTime.MinValue) 
            {
                pager.WhereStr += " and A.[DateTime]>=@StartTime";
                param.Add("StartTime", model.StartTime);    
            }
            if (model.EndTime != DateTime.MinValue)
            {
                pager.WhereStr += " and A.[DateTime]<@EndTime";
                param.Add("EndTime", model.EndTime);
            }


            var list = PagerDBOperate<FinanceReport>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion

        #region 获取统计数据
        public static string GetReportData(QueryBase model)
        {
            string whereStr = "";
            var param = GetWhereStr(model, out whereStr);
            string sql = string.Format("select sum(Blance) from {0}(nolock)", TableName);
            string sqlShouRu = sql + whereStr + " and Type = 1";
            string sqlZhiChu = sql + whereStr + " and Type = 2";

            decimal shouru = DBAccess.ExecuteSql<decimal>(sqlShouRu, param);
            decimal zhichu = DBAccess.ExecuteSql<decimal>(sqlZhiChu, param);
            //decimal all = shouru - zhichu;
            return shouru + "|" + zhichu;
        }
        #endregion

        #region 根据条件获取Where Sql语句
        /// <summary>
        /// 根据条件获取Where Sql语句
        /// </summary>
        /// <param name="model"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public static DynamicParameters GetWhereStr(QueryBase model,out string whereStr)
        {
            whereStr = " where [IsDeleted] =0 ";
            var param = new DynamicParameters();
            if (!string.IsNullOrEmpty(model.KeyWord))
            {
                whereStr += " and ([Abstract] like @Name or [Remark] like @Name )";
                param.Add("Name", "%" + model.KeyWord.Trim() + "%");
            }
            if (model.TypeId != 0)
            {
                whereStr += " and [ReportTypeId]=@TypeId";
                param.Add("TypeId", model.TypeId);
            }
            if (model.CompanyId != 0)
            {
                whereStr += " and [CompanyId]=@CompanyId";
                param.Add("CompanyId", model.CompanyId);
            }
            if (model.StartTime != DateTime.MinValue)
            {
                whereStr += " and [DateTime]>=@StartTime";
                param.Add("StartTime", model.StartTime);
            }
            if (model.EndTime != DateTime.MinValue)
            {
                whereStr += " and [DateTime]<@EndTime";
                param.Add("EndTime", model.EndTime);
            }
            return param;
        }
        #endregion

        #region 报表导入
        public static bool ImportExcel(DataTable dt)
        {
            using (SqlBulkCopy sqlBC = new SqlBulkCopy(ConfigSetting.DataConnection))
            {
                //一次批量的插入的数据量
                sqlBC.BatchSize = dt.Rows.Count;
                //超时之前操作完成所允许的秒数，如果超时则事务不会提交 ，数据将回滚，所有已复制的行都会从目标表中移除
                sqlBC.BulkCopyTimeout = 600;
                //设置要批量写入的表
                sqlBC.DestinationTableName = TableName;

                var objType = typeof(FinanceReport);
                PropertyInfo[] properties = objType.GetProperties();
                string[] outColName = new string[] { "Id", "ModifyDate" };
                foreach (var item in properties)
                {
                    var name = item.Name;
                    if (outColName.Contains(name)) { continue; }
                    sqlBC.ColumnMappings.Add(item.Name, item.Name);
                }

                //批量写入
                if (dt.Rows.Count != 0 && dt != null)
                {
                    sqlBC.WriteToServer(dt);
                    dt.Dispose();
                }
            }
            return true;
        }

        #endregion 

    }
}