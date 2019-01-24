/*
文件: BillDetail.cs
描述: BillDetail类
Copyright: 2019/1/12 16:36:09  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class BillDetail
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public int CusMemberId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CusCompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 需支付
        /// </summary>
        public string ModifyBy { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? NeedPayDate { get; set; }

        /// <summary>
        /// 实际支付时间
        /// </summary>
        public DateTime? RelPayDate { get; set; }

        /// <summary>
        /// 需支付金额
        /// </summary>
        public decimal NeedPayMoeny { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        public decimal RelPayMoeny { get; set; }

        /// <summary>
        /// 支付方式 月付
        /// </summary>
        public int PayWay { get; set; }

        /// <summary>
        /// 支付凭证
        /// </summary>
        public string PayVouchers { get; set; }

        /// <summary>
        /// 0 未支付 1 部分支付 2 支付完成
        /// </summary>
        public int PayStatus { get; set; }
        public string PayStatusTxt { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 付款方名字
        /// </summary>
        public string PayName { get; set; }

        /// <summary>
        /// 收款方名字
        /// </summary>
        public string ReceiveName { get; set; }

        /// <summary>
        /// 账单开始日期
        /// </summary>
        public DateTime? BillStartTime { get; set; }

        /// <summary>
        /// 账单结束日期
        /// </summary>
        public DateTime? BillEndTime { get; set; }

        /// <summary>
        /// 账单内容
        /// </summary>
        public string BillContent { get; set; }

        /// <summary>
        /// 账单关联编号
        /// </summary>
        public string BillGuid { get; set; }

        public string PayWayTxt { get; set; }
        public int BillType { get; set; }
        public string BillTypeName { get; set; }
        public string PaySource { get; set; }
        #endregion
    }
}
