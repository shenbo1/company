
using System;
using System.Collections.Generic;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class UserMenu 
    {
        #region 属性
        public int Id { get; set; }
        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string MenuIcon { get; set; }

        /// <summary>
        /// 父级编号
        /// </summary>
        public int? PID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderIndex { get; set; }

        /// <summary>
        /// 是否显示在左侧菜单(1.是 0.否)
        /// </summary>
        public int? IsLeft { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public List<UserMenu> ChildList { get; set; }
        #endregion
    }
}