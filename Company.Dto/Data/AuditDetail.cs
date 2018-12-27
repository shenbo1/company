/*
文件: AuditDetail.cs
描述: AuditDetail类
Copyright: 2017/12/6 22:00:45  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class AuditDetail
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
        public int Step { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AuditMainId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int OperateBy { get; set; }
        public string OperateName { get; set; }
        public string OperateHeadUrl { get; set; }

        /// <summary>
        /// 1.通过 2.不通过 0.未审核    
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifyDate { get; set; }

        #endregion
    }
}