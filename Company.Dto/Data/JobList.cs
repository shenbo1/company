/*
文件: JobList.cs
描述: JobList类
Copyright: 2018/12/9 15:14:13  by 沈波
*/
using System;
using System.Collections.Generic;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class JobList
    {
        #region 属性

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        public string ParentName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsParent { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string ModifyBy { get; set; }

        /// <summary>
        /// 月价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 图片URL
        /// </summary>
        public string Ico { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public int IsPublish { get; set; }
        public string Info { get; set; }
        public string Level { get; set; }
        public List<JobList> List { get; set; }

        #endregion
    }
}