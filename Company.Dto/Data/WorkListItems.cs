/*
文件: WorkListItems.cs
描述: WorkListItems类
Copyright: 2018/7/10 23:02:56  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class WorkListItems
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 工单Id
        /// </summary>
        public int WorkId { get; set; }
        public string WorkName { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目优先级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string StatusClass { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        public string UserName { get; set; }
        public double DayCount { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RelStartDate { get; set; }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? RelEndDate { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Infos { get; set; }
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
        public int DetailId { get; set; }
        public string DetailGuid { get; set; }
        #endregion
    }
    public class WorkListItemCal{
        public int Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Infos { get; set; }
        public string WorkName { get; set; }
    }
}
