/*
文件: Bill.cs
描述: Bill类
Copyright: 2019/1/17 15:26:22  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class Bill
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
        /// 需支付金额
        /// </summary>
        public decimal PayMoney { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayWay { get; set; }

        /// <summary>
        /// 付款日
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 支付周期
        /// </summary>
        public int PayPeriod { get; set; }

        /// <summary>
        /// 账单日
        /// </summary>
        public DateTime? BillDate { get; set; }

        #endregion
    }
}