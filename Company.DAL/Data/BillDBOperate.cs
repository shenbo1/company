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
    public class BillDBOperate
    {
        const string TableName = "Bill";
        const string BillDetailTable = "BillDetail";

        #region 添加
        public static bool AddBill(Bill model,List<BillDetail> billDetail)
        {
         
            using (SqlConnection con = new SqlConnection(ConfigSetting.DataConnection))
            {
                con.Open();
                var transaction = con.BeginTransaction();
                try
                {
                    string modifySql = string.Format("update {0} set isdeleted = 1,ModifyDate = getdate(), ModifyBy = @ModifyBy where ProjectId = @ProjectId and BillStartTime >= @BillStartTime and BillEndTime <= @BillEndTime and isdeleted = 0 and PayStatus = 1;", BillDetailTable);
                    string sql = string.Format("insert into {0}([ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[PayMoney],[PayWay],[PayDate],[PayPeriod],[BillDate])  values(@ProjectId,@ProjectName,@CusMemberId,@CusCompanyId,@Guid,0,getdate(),@CreateBy,@PayMoney,@PayWay,@PayDate,@PayPeriod,@BillDate)", TableName);
                    string sql2 = string.Format("insert into {0}([ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[NeedPayDate],[RelPayDate],[NeedPayMoeny],[RelPayMoeny],[PayWay],[PayVouchers],[PayStatus],[Remark],[PayName],[ReceiveName],[BillStartTime],[BillEndTime],[BillContent],[BillGuid],[BillType])  values(@ProjectId,@ProjectName,@CusMemberId,@CusCompanyId,@Guid,0,getdate(),@CreateBy,@NeedPayDate,@RelPayDate,@NeedPayMoeny,@RelPayMoeny,@PayWay,@PayVouchers,@PayStatus,@Remark,@PayName,@ReceiveName,@BillStartTime,@BillEndTime,@BillContent,@BillGuid,@BillType)", BillDetailTable);
                    con.Execute(modifySql, new { ModifyBy = model.CreateBy, ProjectId = model.ProjectId, BillStartTime = model.BillDate, BillEndTime = billDetail.Max(a => a.BillEndTime) }, transaction);
                    con.Execute(sql, model, transaction);
                    con.Execute(sql2, billDetail, transaction);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region 添加
        public static bool AddBill(Bill model)
        {
            string sql = string.Format("insert into {0}([ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[PayMoney],[PayWay],[PayDate],[PayPeriod],[BillDate])  values(@ProjectId,@ProjectName,@CusMemberId,@CusCompanyId,@Guid,0,getdate(),@CreateBy,@PayMoney,@PayWay,@PayDate,@PayPeriod,@BillDate)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyBill(Bill model)
        {
            string sql = string.Format(@"update {0} set [ProjectId]=@ProjectId,[ProjectName]=@ProjectName,[CusMemberId]=@CusMemberId,[CusCompanyId]=@CusCompanyId,[Guid]=@Guid,[IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy,[PayMoney]=@PayMoney,[PayWay]=@PayWay,[PayDate]=@PayDate,[PayPeriod]=@PayPeriod,[BillDate]=@BillDate
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 获取单个对象
        public static Bill GetModelById(int Id)
        {
            string sql = string.Format(@"select [Id],[ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[PayMoney],[PayWay],[PayDate],[PayPeriod],[BillDate] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<Bill>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static Bill GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[PayMoney],[PayWay],[PayDate],[PayPeriod],[BillDate],[Id],[ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[PayMoney],[PayWay],[PayDate],[PayPeriod],[BillDate] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<Bill>(sql, Name);
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
        public static List<Bill> GetPagerList(QueryBase query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[ProjectId],A.[ProjectName],A.[CusMemberId],A.[CusCompanyId],A.[Guid],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[ModifyBy],A.[PayMoney],A.[PayWay],A.[PayDate],A.[PayPeriod],A.[BillDate]";
            pager.WhereStr += " and A.[IsDeleted]=0 ";
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            var list = PagerDBOperate<Bill>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion
    }
}