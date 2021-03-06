﻿/*
文件: WorkListDetail.cs
描述: WorkListDetail类
Copyright: 2018/7/22 16:30:37  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class WorkListDetail
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Infos { get; set; }

        /// <summary>
        /// 1.消费者 2.管理员
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsDeleted { get; set; }
        public int Index { get; set; }
        public string Guid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModifyBy { get; set; }

        #endregion
    }
}