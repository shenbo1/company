/*
文件: AuditMain.cs
描述: AuditMain类
Copyright: 2017/12/6 22:00:22  by 沈波
*/
using System;
using System.Collections.Generic;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class AuditMain
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 财务类别
        /// </summary>
        public int ConfigId { get; set; }
        public string ConfigName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 1.审批中 2.审批未通过 3.流程完成
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 当前操作人
        /// </summary>
        public int OperateId { get; set; }
        public string OperateName { get; set; }
        public int NextOperateId { get; set; }
        public string NextOperateName { get; set; }
        /// <summary>
        /// 当前进度
        /// </summary>
        public int NowCount { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MainId { get; set; }
        public string Images { get; set; }
        public int DepartId { get; set; }
        public List<AuditDetail> DetailList { get; set; }
        #endregion
    }
}