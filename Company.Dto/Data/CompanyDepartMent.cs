﻿/*
文件: CompanyDepartMent.cs
描述: CompanyDepartMent类
Copyright: 2017/10/31 15:06:33  by 沈波
*/
using System;
using System.Collections.Generic;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class CompanyDepartMent 
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级
        /// </summary>
        public int PId { get; set; }
        public string PName { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

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
        /// 审核步骤
        /// </summary>
        public string AuditStep { get; set; }
        public List<CompanyDepartMent> ChildList { get; set; }
        #endregion
    }
}