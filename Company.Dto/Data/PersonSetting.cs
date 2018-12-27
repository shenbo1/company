/*
文件: PersonSetting.cs
描述: PersonSetting类
Copyright: 2018/12/17 14:12:07  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class PersonSetting
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

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
        public string WorkGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int DepartId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Infos { get; set; }

        /// <summary>
        /// 月数
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 总价格
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public int Discount { get; set; }
        public int ProjectId { get; set; }

        #endregion
    }
}