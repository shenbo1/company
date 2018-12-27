/*
文件: CompanyUser.cs
描述: CompanyUser类
Copyright: 2017/10/31 17:37:52  by 沈波
*/
using System;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class CompanyUser 
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool Checked { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string WeChatOpenId { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WeChatNickName { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string WeChatHeadUrl { get; set; }

        /// <summary>
        /// 是否绑定微信 1绑定0未绑定
        /// </summary>
        public int IsBindWeChat { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartId { get; set; }
        public string DepartName { get; set; }
        public int RoleId { get; set; }
        public string Ip { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        /// <summary>
        /// 是否上线
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 曾用名
        /// </summary>
        public string UsedName { get; set; }

        /// <summary>
        /// 社保编号
        /// </summary>
        public string SocialNo { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 身份证正面
        /// </summary>
        public string CardFront { get; set; }

        /// <summary>
        /// 身份证反面
        /// </summary>
        public string CardFontBehind { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
        public string SexTxt { get; set; }

        /// <summary>
        /// 名族
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        public string Political { get; set; }
        public string PoliticalTxt { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }
        public string EducationTxt { get; set; }
        public string EducationPhoto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int HomeAddressType { get; set; }
        public string HomeAddressTypeTxt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime EnterDate { get; set; }

        #endregion
    }
}