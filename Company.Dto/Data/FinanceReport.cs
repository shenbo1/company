/*
文件: FinanceReport.cs
描述: FinanceReport类
Copyright: 2017/8/18 19:52:49  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class FinanceReport
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 报表编号类型
        /// </summary>
        public int ReportTypeId { get; set; }

        /// <summary>
        /// 报表类型名称
        /// </summary>
        public string ReportTypeName { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Blance { get; set; }

        /// <summary>
        /// 1.收入 2.支出
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 录入人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operater { get; set; }

        /// <summary>
        /// 付款人
        /// </summary>
        public string Payer { get; set; }

        public int CompanyId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDeleted { get; set; }
        public DateTime ModifyDate { get; set; }
        #endregion
    }
}