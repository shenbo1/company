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
    public class BillDetailDBOperate
    {
        const string TableName = "BillDetail";

        #region 添加
        public static bool AddBillDetail(BillDetail model)
        {

            string sql = string.Format("insert into {0}([ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[NeedPayDate],[RelPayDate],[NeedPayMoeny],[RelPayMoeny],[PayWay],[PayVouchers],[PayStatus],[Remark],[PayName],[ReceiveName],[BillStartTime],[BillEndTime],[BillContent],[BillGuid])  values(@ProjectId,@ProjectName,@CusMemberId,@CusCompanyId,@Guid,0,getdate(),@CreateBy,@NeedPayDate,@RelPayDate,@NeedPayMoeny,@RelPayMoeny,@PayWay,@PayVouchers,@PayStatus,@Remark,@PayName,@ReceiveName,@BillStartTime,@BillEndTime,@BillContent,@BillGuid)", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        #region 修改
        public static bool ModifyBillDetail(BillDetail model)
        {
            string sql = string.Format(@"update {0} set [ProjectId]=@ProjectId,[ProjectName]=@ProjectName,[CusMemberId]=@CusMemberId,[CusCompanyId]=@CusCompanyId,[Guid]=@Guid,[IsDeleted]=@IsDeleted,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy,[NeedPayDate]=@NeedPayDate,[RelPayDate]=@RelPayDate,[NeedPayMoeny]=@NeedPayMoeny,[RelPayMoeny]=@RelPayMoeny,[PayWay]=@PayWay,[PayVouchers]=@PayVouchers,[PayStatus]=@PayStatus,[Remark]=@Remark,[PayName]=@PayName,[ReceiveName]=@ReceiveName,[BillStartTime]=@BillStartTime,[BillEndTime]=@BillEndTime,[BillContent]=@BillContent,[BillGuid]=@BillGuid
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }
        #endregion

        public static bool PayBillDetail(BillDetail model)
        {
            string sql = string.Format(@"update {0} set [PaySource] = @PaySource ,[ModifyDate]=getdate(),[ModifyBy]=@ModifyBy,[RelPayDate]=@RelPayDate,[RelPayMoeny]=@RelPayMoeny,[PayWay]=@PayWay,[PayVouchers]=@PayVouchers,[PayStatus]=@PayStatus,[Remark]=@Remark,[PayName]=@PayName,[ReceiveName]=@ReceiveName
            where Id=@Id", TableName);
            return DBAccess.ExecuteSqlWithEntity(sql, model);
        }

        #region 获取单个对象
        public static BillDetail GetModelById(int Id)
        {
            string sql = string.Format(@"select [Id],[ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[NeedPayDate],[RelPayDate],[NeedPayMoeny],[RelPayMoeny],[PayWay],[PayVouchers],[PayStatus],[Remark],[PayName],[ReceiveName],[BillStartTime],[BillEndTime],[BillContent],[BillGuid] from {0} (nolock) where Id=@Id", TableName);
            return DBAccess.GetEntityById<BillDetail>(sql, Id);
        }
        #endregion
        #region 获取单个对象
        public static BillDetail GetModelByName(string Name)
        {
            string sql = string.Format(@"select [Id],[ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[NeedPayDate],[RelPayDate],[NeedPayMoeny],[RelPayMoeny],[PayWay],[PayVouchers],[PayStatus],[Remark],[PayName],[ReceiveName],[BillStartTime],[BillEndTime],[BillContent],[BillGuid],[Id],[ProjectId],[ProjectName],[CusMemberId],[CusCompanyId],[Guid],[IsDeleted],[CreateDate],[CreateBy],[ModifyBy],[NeedPayDate],[RelPayDate],[NeedPayMoeny],[RelPayMoeny],[PayWay],[PayVouchers],[PayStatus],[Remark],[PayName],[ReceiveName],[BillStartTime],[BillEndTime],[BillContent],[BillGuid] from {0} (nolock) where Name=@Name", TableName);
            return DBAccess.GetEntityByName<BillDetail>(sql, Name);
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
        public static List<BillDetail> GetPagerList(QueryBase query, out int totalcount)
        {
            var param = new DynamicParameters();
            totalcount = 0;
            Pager pager = new Pager() { TableName = TableName + " A", Offset = query.Offset, PageSize = query.Limit, ColName = "A.[ID]" };
            pager.Columns = @"A.[Id],A.[ProjectId],A.[ProjectName],A.[CusMemberId],A.[CusCompanyId],A.[Guid],A.[IsDeleted],A.[CreateDate],A.[CreateBy],A.[ModifyBy],A.[NeedPayDate],A.[RelPayDate],A.[NeedPayMoeny],A.[RelPayMoeny],A.[PayWay],A.[PayVouchers],A.[PayStatus],A.[Remark],A.[PayName],A.[ReceiveName],A.[BillStartTime],A.[BillEndTime],A.[BillContent],A.[BillGuid],a.[PaySource],a.[BillType]";
            pager.WhereStr += " and A.[IsDeleted]=0";
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                pager.WhereStr += " and A.[Name] like @Name";
                param.Add("Name", "%" + query.KeyWord + "%");
            }
            var list = PagerDBOperate<BillDetail>.Init.GetList(pager, param, out totalcount);
            return list;
        }
        #endregion
    }
}