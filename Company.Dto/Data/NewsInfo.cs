/*
文件: NewsInfo.cs
描述: NewsInfo类
Copyright: 2018/11/28 20:57:57  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class NewsInfo
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MainImg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Infos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PublishName { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public string GUID { get; set; }
        public string IsPublish { get; set; }
        public int Index { get; set; }
        public string Desc { get; set; }

        #endregion
    }
}