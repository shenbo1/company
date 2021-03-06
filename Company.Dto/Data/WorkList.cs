﻿/*
文件: WorkList.cs
描述: WorkList类
Copyright: 2018/7/8 14:48:24  by 沈波
*/
using System;
using System.Collections.Generic;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class WorkList
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
        /// 
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public int CusMemberId { get; set; }
        public string CusMemberName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CusCompanyId { get; set; }

        /// <summary>
        /// 工单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工单简介
        /// </summary>
        public string Infos { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime? GetTime { get; set; }

        /// <summary>
        /// 需求分析时间
        /// </summary>
        public DateTime? NeedTime { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        public DateTime? DoTime { get; set; }

        /// <summary>
        /// 参与人员
        /// </summary>
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<CompanyUser> UserList { get; set; }
        public List<WorkListDetail> DetailList { get; set; }
        public List<WorkListItems> ItemList { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string Status { get; set; }
        public string StatusTxt { get; set; }
        public string StatusClass { get; set; }

        /// <summary>
        /// 项目进度
        /// </summary>
        public int Length { get; set; }
        public string Guid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsDeleted { get; set; }

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
        public bool CanEdit { get; set; }
  
        #endregion
    }
}