/*
文件: CustomerCompany.cs
描述: CustomerCompany类
Copyright: 2018/9/9 17:51:16  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class CustomerCompany
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
        /// 公司名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 具体地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 合作关系
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 公司简介
        /// </summary>
        public string Infos { get; set; }

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
        /// 开户行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankNo { get; set; }

        /// <summary>
        /// 税号
        /// </summary>
        public string Tax { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public string License { get; set; }
        public string Source { get; set; }
        public string TeamDesc { get; set; }
        /// <summary>
        /// 公司状态
        /// </summary>
        public int CompanyStatus { get; set; }
        /// <summary>
        /// 合伙人简介
        /// </summary>
        public string PartInfo { get; set; }

        #endregion
    }
}