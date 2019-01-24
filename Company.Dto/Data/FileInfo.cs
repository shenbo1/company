/*
文件: FileInfo.cs
描述: FileInfo类
Copyright: 2019/1/23 20:38:09  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class FileInfo
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
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
        /// 需支付
        /// </summary>
        public string ModifyBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 类型 ： 项目
        /// </summary>
        public int TypeCode { get; set; }

        /// <summary>
        /// 类型编号
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize { get; set; }

        #endregion
    }
}