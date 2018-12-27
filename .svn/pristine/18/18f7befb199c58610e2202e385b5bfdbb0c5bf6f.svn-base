/*
文件: Config.cs
描述: Config类
Copyright: 2017/8/17 22:20:17  by 沈波
*/
using System;
using System.Collections.Generic;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class FinanceConfig
    {
        #region 属性

        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 类别(Report 报表)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDeleted { get; set; }
        public int PId { get; set; }
        public int Index { get; set; }
        public List<FinanceConfig> ChildList { get; set; }
        #endregion
    }

    public class PickerSelect {
        public int id { get; set; }
        public int value { get; set; }
        public string text { get; set; }
        public List<PickerSelect> list { get; set; }
    }
}