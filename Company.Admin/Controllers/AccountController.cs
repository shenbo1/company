using Company.DAL;
using Company.DAL.Data;
using Company.Dto;
using Company.Dto.Data;
using Company.Dto.Enum;
using Company.Dto.Model;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Company.Admin.Controllers
{
    public class AccountController : Controller
    {
        #region 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public JsonResult Register(AccountRegister account) {
            ResultInfo info = new ResultInfo();
            int companyId = CommonMethod.GetInt( ConfigSetting.eeeYoooId);
            string companyName = ConfigSetting.eeeYoooName;

            account.PassWord = CommonMethod.PasswordMD5(account.PassWord);
            if (AccountDBOperate.GetAccountByMobile(account.Telephone, account.Role)) {
                info.Message = "手机号已注册";
                return Json(info);
            }
            if (AccountDBOperate.GetAccountByEmail(account.Email, account.Role))
            {
                info.Message = "邮箱已注册";
                return Json(info);
            }
            info.IsSuccess =  AccountDBOperate.AddAccount(new Dto.CustomerCompany()
            {
                CompanyId = companyId,
                CompanyName = companyName,
                Address = account.CompanyAddress,
                City = account.City,
                Infos = account.CompanyInfo,
                Name = account.CompanyName,
                Status = CustomerCompanyStatusEnum.NotStart.ToString(),
                CreateBy = account.Name,

            }, new CustomerMember()
            {
                CompanyId = companyId,
                CompanyName = companyName,
                Email = account.Email,
                Mobile = account.Telephone,
                Name = account.Name,
                CreateBy = account.Name,
            }, new Account()
            {
                AccountName = account.Name,
                Mobile = account.Telephone,
                Password = account.PassWord,
                Email = account.Email,
                Role = account.Role,
                CreateBy = account.Name,
            });
            return Json(info);
        }

        #endregion

        #region 注册并立项
        public JsonResult AddProject(ProjectRegister account) {
            ResultInfo info = new ResultInfo();
            int companyId = CommonMethod.GetInt(ConfigSetting.eeeYoooId);
            string companyName = ConfigSetting.eeeYoooName;

            account.PassWord = CommonMethod.PasswordMD5(account.PassWord);
            if (AccountDBOperate.GetAccountByMobile(account.Telephone, account.Role))
            {
                info.Message = "手机号已注册";
                return Json(info);
            }
            if (AccountDBOperate.GetAccountByEmail(account.Email, account.Role))
            {
                info.Message = "邮箱已注册";
                return Json(info);
            }
            var list = new List<PersonSetting>() { };
            var roles = account.RoleId.Split(',');
            var joblist = JobListDBOperate.GetPublishList();
            foreach (var item in roles)
            {
                var job = joblist.FirstOrDefault(a => a.Id == Convert.ToInt32(item));
                if (job == null) { continue; }
                var personSetting = new PersonSetting() { CreateBy = account.Name, DepartId = Convert.ToInt32(item), DepartName = job.Name, Discount = 100, Month = account.Month, Price = job.Price, TotalPrice = job.Price * account.Month };
                list.Add(personSetting);
            }


            info.IsSuccess = AccountDBOperate.AddAccount(new Dto.CustomerCompany()
            {
                CompanyId = companyId,
                CompanyName = companyName,
                Address = account.CompanyAddress,
                City = account.City,
                Infos = account.CompanyInfo,
                Name = account.CompanyName,
                Status = CustomerCompanyStatusEnum.NotStart.ToString(),
                CreateBy = account.Name,
                TeamDesc = account.TeamDesc,
                CompanyStatus = account.CompanyStatus,
                PartInfo = account.PartInfo
            }, new CustomerMember()
            {
                CompanyId = companyId,
                CompanyName = companyName,
                Email = account.Email,
                Mobile = account.Telephone,
                Name = account.Name,
                CreateBy = account.Name,
                Address = account.MemberAddress
            }, new Account()
            {
                AccountName = account.Name,
                Mobile = account.Telephone,
                Password = account.PassWord,
                Email = account.Email,
                Role = account.Role,
                CreateBy = account.Name,
            }, new ProjectManage()
            {
                CompanyId = companyId,
                FullName = account.ProjectName,
                Name = account.ProjectName,
                CusCompanyName = account.CompanyName,
                CusMemberName = account.Name,
                //LastPayDate = account.EndTime,
                Infos = account.Info,
                IsDeleted = 0,
                Money = 0,
                PayWay = ProjectPayWayEnum.UnKnown.ToString(),
                //RelStartTime = account.StartTime,
                //StartTime = account.StartTime,
                EndTime = account.EndTime,
                Source = SourceEnum.PC.ToString(),
                //RelEndTime = account.EndTime,
                Status = CustomerCompanyStatusEnum.NotStart.ToString(),
                OpereateType = account.OpereateType,
                DevLanguate = account.DevLanguate,
                DevType = account.DevType,
                NowStatus = account.NowStatus,
                MarketProspect = account.MarketProspect,
                Month = account.Month
            }, new HardWare()
            {

                CreateBy = account.Name,
                Infos = account.HardWare
            }, list);
             //AccountDBOperate.LoginByMobileOrEmail(account.Telephone);
            return Json(info);
        }
        #endregion

        #region 登陆
        public JsonResult Login(string name, string pwd)
        {
            string txt =  CommonMethod.PasswordMD5("123456");
            ResultInfo info = new ResultInfo();
            var model = AccountDBOperate.LoginByMobileOrEmail(name);
            if (model == null) { info.Message = "该用户未注册"; return Json(info); }
            //var txt = CommonMethod.PasswordMD5("123456");
            if (model.Password != CommonMethod.PasswordMD5(pwd))
            {
                info.Message = "密码错误";
                return Json(info);
            }
            info.IsSuccess = true;
            //model.AccountId
            if (model.Role == 1)
            {
                var model1 = CustomerCompanyDBOperate.GetCompanyByMember(model.AccountId);
                model.AccountName = model1.Name;
            }
            CookieOperate.AccountCookie = model;
            return Json(info);
        }
        #endregion

        #region 退出
        public ActionResult LogOut() {
            CookieOperate.AccountCookie = null;
            return Redirect("/website/index");
        }
        #endregion
    }
}