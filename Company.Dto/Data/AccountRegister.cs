using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Dto.Data
{
    public class AccountRegister
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyInfo { get; set; }
        public string PassWord { get; set; }
        public int Role { get; set; }
    }
    public class ProjectRegister : AccountRegister
    {
        /// <summary>
        /// 角色
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 合同月数
        /// </summary>
        public int Month { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 硬件
        /// </summary>
        public string HardWare { get; set; }
        public string Info { get; set; }
        /// <summary>
        /// 合伙人简介
        /// </summary>
        public string PartInfo { get; set; }
        /// <summary>
        /// 公司状态
        /// </summary>
        public int CompanyStatus { get; set; }
        /// <summary>
        /// 团队简介
        /// </summary>
        public string TeamDesc { get; set; }
        public string MemberAddress { get; set; }
        /// <summary>
        /// 经营分类
        /// </summary>
        public int OpereateType { get; set; }
        /// <summary>
        /// 开发分类
        /// </summary>
        public int DevType { get; set; }
        /// <summary>
        /// 项目目前状态
        /// </summary>
        public int NowStatus { get; set; }
        /// <summary>
        /// 项目开发语言
        /// </summary>
        public string DevLanguate { get; set; }
        /// <summary>
        /// 市场前景
        /// </summary>
        public string MarketProspect { get; set; }
    }
}
