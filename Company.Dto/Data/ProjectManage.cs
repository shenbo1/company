/*
文件: ProjectManage.cs
描述: ProjectManage类
Copyright: 2018/7/4 18:50:57  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class ProjectManage
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 项目简称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目全名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 项目开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 项目结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 项目实际开始时间
        /// </summary>
        public DateTime RelStartTime { get; set; }

        /// <summary>
        /// 项目实际开始时间
        /// </summary>
        public DateTime RelEndTime { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayWay { get; set; }
        public string PayWayTxt { get; set; }
        /// <summary>
        /// 最后一次付款时间
        /// </summary>
        public DateTime LastPayDate { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        public string StatusTxt { get; set; }

        /// <summary>
        /// 项目简介
        /// </summary>
        public string Infos { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyBy { get; set; }
        public int IsDeleted { get; set; }
        public int CusMemberId { get; set; }
        public string CusMemberName { get; set; }
        public int CusCompanyId { get; set; }
        public string CusCompanyName { get; set; }
        public string Source { get; set; }
        /// <summary>
        /// 经营分类
        /// </summary>
        public int OpereateType { get; set; }
        public string OpereateTypeTxt { get; set; }
        /// <summary>
        /// 开发分类
        /// </summary>
        public int DevType { get; set; }
        public string DevTypeTxt { get; set; }
        /// <summary>
        /// 项目目前状态
        /// </summary>
        public int NowStatus { get; set; }
        public string NowStatusTxt { get; set; }
        /// <summary>
        /// 项目开发语言
        /// </summary>
        public string DevLanguate { get; set; }
        /// <summary>
        /// 市场前景
        /// </summary>
        public string MarketProspect { get; set; }
        public int Month { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayDate { get; set; }
        #endregion
    }
}