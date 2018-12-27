using Company.Admin.Fliter;
using Company.DAL.Data;
using Company.Dto;
using Company.Dto.Model;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Company.Util;
using System.Reflection;
using Company.BLL.Data;
using System.Web.Http;
using Company.Dto.Data;
using System.IO;
using Company.DAL;
using Company.Dto;
using Newtonsoft.Json;
using Company.Dto.Enum;

namespace Company.Admin.Controllers
{
    [AdminLoginFilter]
    public class AdminController : Controller
    {
        #region 首页
        public ActionResult Index()
        {
            var user = CookieOperate.MemberCookie;

            ViewBag.Name = user.UserName;
            ViewBag.ID = user.Id;
            ViewBag.Img = string.IsNullOrEmpty(user.WeChatHeadUrl) ? "/content/img/profile_small.jpg" : user.WeChatHeadUrl;
            return View();
        }
        public ActionResult Login()
        {
            CookieOperate.MemberCookie = null;
            return View();
        }
        public ActionResult AdminCenter() {
            var model = CookieOperate.MemberCookie;
            model = CompanyUserDBOperate.GetModelById(model.Id);
            var dep = CompanyDepartMentDBOperate.GetChildOne(model.DepartId, model.CompanyId);
            model.WeChatHeadUrl = string.IsNullOrEmpty(model.WeChatHeadUrl) ? "/content/img/profile_small.jpg" : model.WeChatHeadUrl;
            if (dep != null)
                model.DepartName = dep.Name;
            if (model.HomeAddressType != 0)
                model.HomeAddressTypeTxt = CommonMethod.GetEnumDesc<HuKouEnum>(model.HomeAddressType);// Enum.GetName(typeof(HuKouEnum), model.HomeAddressType);
            if (model.Sex != 0)
                model.SexTxt = CommonMethod.GetEnumDesc<CompanySexEnum>(model.Sex);
            if (!string.IsNullOrEmpty(model.Political))
                model.PoliticalTxt = CommonMethod.GetEnumDesc<PoliticalEnum>(model.Political);
            if (!string.IsNullOrEmpty(model.Education))
                model.EducationTxt = CommonMethod.GetEnumDesc<EducationEnum>(model.Education);
            if (model.EnterDate == null || model.EnterDate == DateTime.MinValue) { model.EnterDate = DateTime.Now; }
            if (string.IsNullOrEmpty(model.CardFront))
                model.CardFront = "";
            if (string.IsNullOrEmpty(model.CardFontBehind))
                model.CardFontBehind = "";
            if (string.IsNullOrEmpty(model.EducationPhoto))
                model.EducationPhoto = "";
            ViewBag.Political = CommonMethod.GetEnumItems<PoliticalEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            ViewBag.Sex = CommonMethod.GetIntEnumItems<CompanySexEnum>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });
            ViewBag.Education = CommonMethod.GetEnumItems<EducationEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            ViewBag.HuKou = CommonMethod.GetIntEnumItems<HuKouEnum>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });

            string url = string.Format("{1}/WeChat/BindWeChat?userName={0}", model.UserName,ConfigSetting.DomainUrl);
            url = string.Format(ConfigSetting.WeChatUrl, url);
            string file = model.UserName + ".jpg";
            file = CommonMethod.CreateCode(url, file);

            ViewBag.Code = file;
            return View(model);
        }
        public ActionResult Welcome()
        {
            return View();
        }

        public JsonResult AdminLogin(string LoginId, string Password)
        {
            ResultInfo info = new ResultInfo() { Message = "用户名或者密码错误" };
            var model = CompanyUserDBOperate.Login(LoginId, CommonMethod.PasswordMD5(Password));
            if (model != null)
            {
                if (model.IsDeleted == 1) {
                    info.Message = "您的账号已被禁用,请联系管理员";
                    return Json(info);
                }
                var company = CompanyInfoDBOperate.GetModelById(model.CompanyId);
                if (company != null) {
                    model.CompanyName = company.CompanyName;
                }
                info.IsSuccess = true;
                CookieOperate.MemberCookie = model;
            }
            return Json(info);
        }
        public ActionResult MenuList()
        {
            string html = "";
            var menuList = UserMenuDBOperate.GetRoleMenuList(CookieOperate.MemberCookie.DepartId);
            var menuParent = menuList.Where(a => a.PID == 0 && a.IsLeft == 1).ToList();
            html = MenuListHtml(menuList, menuParent);
            return Content("document.write('" + html + "')");
        }

        public ActionResult NoPerMission()
        {
            return View();
        }

        #region 菜单
        /// <summary>
        /// 菜单
        /// </summary>
        /// <param name="list">总菜单</param>
        /// <param name="ParentList">父级菜单</param>
        /// <returns></returns>
        public string MenuListHtml(List<UserMenu> list, List<UserMenu> ParentList)
        {
            string html = "";
            int i = 0;
            foreach (var item in ParentList)
            {
                html += "<li class=\"" + (i == 0 ? "active" : "") + "\">";
                html += "<a href=\"#\">";
                html += "<i class=\"fa " + item.MenuIcon + "\"></i>";
                html += "<span class=\"nav-label\">" + item.Name + "</span>";
                html += "<span class=\"fa arrow\"></span>";
                html += "</a>";
                var childList = list.Where(a => a.PID == item.Id && a.IsLeft == 1).ToList();
                html += MenuChlid(childList);
                html += "</li>";
                i++;
            }
            return html;
        }
        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="list">子菜单</param>
        /// <returns></returns>
        public string MenuChlid(List<UserMenu> list)
        {
            if (list == null || list.Count() == 0) { return ""; }
            string html = "";
            html += "<ul class=\"nav nav-second-level\">";
            foreach (var item in list)
            {
                html += "<li>";
                html += "<a class=\"J_menuItem\" href=\"" + item.Url + "\" data-index=\"0\">" + item.Name + "<span class=\"fa arrow\"></span></a>";
                html += "</li>";
            }
            html += "</ul>";
            return html;
        }
        #endregion

        #endregion

        #region 公司管理
        public ActionResult CompanyInfo()
        {
            return View();
        }
        public JsonResult CompanyInfoList()
        {
            int pagesize = CommonMethod.GetInt(Request["limit"]);
            int offset = CommonMethod.GetInt(Request["offset"]);
            string name = CommonMethod.GetString(Request["name"]);
            var totalcount = 0;
            var list = CompanyInfoDBOperate.GetPagerList(pagesize, offset, name, out totalcount);
            PagerList<CompanyInfo> pagerList = new PagerList<CompanyInfo>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        public JsonResult CompanyInfoAdd(CompanyInfo model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            if (model.Id <= 0)
            {
                flag = CompanyInfoDBOperate.AddCompanyInfo(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                flag = CompanyInfoDBOperate.ModifyCompanyInfo(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        public JsonResult CompanyInfoGetOne(int id) {
            ResultInfo<CompanyInfo> result = new ResultInfo<CompanyInfo>();
            result.Data = CompanyInfoDBOperate.GetModelById(id);
            return Json(result);
        }
        #endregion

        #region 角色管理
        public ActionResult RoleInfo()
        {
            return View();
        }
        public JsonResult RoleInfoList()
        {
            int pagesize = CommonMethod.GetInt(Request["limit"]);
            int offset = CommonMethod.GetInt(Request["offset"]);
            string name = CommonMethod.GetString(Request["name"]);
            var totalcount = 0;
            var list = RoleInfoDBOperate.GetPagerList(pagesize, offset, name, out totalcount);
            PagerList<RoleInfo> pagerList = new PagerList<RoleInfo>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        public JsonResult RoleInfoAdd(RoleInfo model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            if (model.Id <= 0)
            {
                flag = RoleInfoDBOperate.AddRoleInfo(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                flag = RoleInfoDBOperate.ModifyRoleInfo(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        public JsonResult RoleInfoGetOne(int id)
        {
            ResultInfo<RoleInfo> result = new ResultInfo<RoleInfo>();
            result.Data = RoleInfoDBOperate.GetModelById(id);
            return Json(result);
        }
        #endregion

        #region 菜单管理
        public ActionResult UserMenuIndex()
        {
            return View();
        }
        #region 菜单
        public JsonResult UserMenuAdd(UserMenu model)
        {
            ResultInfo result = new ResultInfo();
            var flag = false;
            if (model.Id <= 0)
            {
                flag = UserMenuDBOperate.AddUserMenu(model);
                if (flag) { result.IsSuccess = true; result.Message = "菜单添加成功"; }
            }
            else
            {
                flag = UserMenuDBOperate.UpdateUserMenu(model);
                if (flag) { result.IsSuccess = true; result.Message = "菜单修改成功"; }
            }

            return Json(result);
        }

        public JsonResult UserMenuGetOne(int id)
        {
            ResultInfo<UserMenu> result = new ResultInfo<UserMenu>();
            var model = UserMenuDBOperate.GetModelById(id);
            if (model != null)
            {
                result.IsSuccess = true;
                result.Data = model;
            }
            return Json(result);
        }

        public JsonResult UserManagerList()
        {
            var AllList = UserMenuDBOperate.GetAllList();
            var pList = AllList.Where(a => a.PID == 0).ToList();

            foreach (var item in pList)
            {
                var chlidList = AllList.Where(a => a.PID == item.Id).ToList();
                item.ChildList = chlidList;
            }

            return Json(pList);
        }

        public JsonResult UserRoleTreeList()
        {
            var AllList = UserMenuDBOperate.GetAllList();
            var ConventList = AllList.ConvertAll(a => new ZTreeModel()
            {
                id = a.Id,
                name = a.Name,
                pId = a.PID ?? 0,
            });
            return Json(ConventList, JsonRequestBehavior.AllowGet);
        }

        #region 根据角色获取菜单权限
        public JsonResult UserGetPermissionByRole()
        {
            int roleId = CommonMethod.GetInt(Request["RoleId"]);
            var MenuList = UserMenuDBOperate.GetRoleMenuList(roleId);
            ResultInfo<List<int>> result = new ResultInfo<List<int>>();
            result.IsSuccess = true;
            result.Data = MenuList.ConvertAll(a => a.Id).ToList();
            return Json(result);
        }
        #endregion

        #region 根据角色分配菜单权限
        public JsonResult UserPermissionSet()
        {
            int roleId = CommonMethod.GetInt(Request["RoleId"]);
            string PermissionIds = CommonMethod.GetString(Request["PermissionIds"]);
            bool flag = UserMenuDBOperate.SetRoleMenuList(roleId, PermissionIds);
            ResultInfo result = new ResultInfo();
            if (flag) { result.IsSuccess = true; result.Message = "设置成功"; }
            return Json(result);
        }
        #endregion
        #endregion
        #endregion

        #region 人员管理
        public ActionResult AdminInfo()
        {
            int companyId = CookieOperate.MemberCookie.CompanyId;

            ViewBag.CompanyList = CompanyInfoDBOperate.GetList(companyId).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.CompanyName + "--" + a.CompanyFullName
            });

            ViewBag.RoleList = RoleInfoDBOperate.GetList(companyId).Select(p => new SelectListItem()
            {
                Value = p.Id.ToString(),
                Text = p.RoleName
            });
            return View();
        }
        public JsonResult AdminInfoList()
        {
            int pagesize = CommonMethod.GetInt(Request["limit"]);
            int offset = CommonMethod.GetInt(Request["offset"]);
            string name = CommonMethod.GetString(Request["name"]);
            int companyId = CookieOperate.MemberCookie.CompanyId;
            var totalcount = 0;
            var list = AdminInfoDBOperate.GetPagerList(pagesize, offset, name, out totalcount, companyId);
            PagerList<AdminInfo> pagerList = new PagerList<AdminInfo>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        public JsonResult AdminInfoAdd(AdminInfo model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            if (model.Id <= 0)
            {
                if (AdminInfoDBOperate.CheckUserName(model.UserName))
                {
                    info.Message = "用户名已存在";
                    return Json(info);
                }
                model.PassWord = CommonMethod.PasswordMD5(model.PassWord);
                flag = AdminInfoDBOperate.AddAdminInfo(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                flag = AdminInfoDBOperate.ModifyAdminInfo(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        public JsonResult AdminInfoGetOne(int id)
        {
            ResultInfo<AdminInfo> result = new ResultInfo<AdminInfo>();
            result.Data = AdminInfoDBOperate.GetModelById(id);
            return Json(result);
        }
        #endregion

        #region 财务类别配置管理

        public ActionResult FinanceConfigInfo()
        {
            return View();
        }
        public JsonResult FinanceConfigList()
        {
            var list = FinanceConfigDBOperate.GetListChild("Report", CookieOperate.MemberCookie.CompanyId);
            return Json(list);
        }
        public JsonResult FinanceConfigTreeList()
        {
            var AllList = FinanceConfigDBOperate.GetList("Report", CookieOperate.MemberCookie.CompanyId);
            var ConventList = AllList.ConvertAll(a => new ZTreeModel()
            {
                id = a.Id,
                name = a.Value,
                pId = a.PId,
            });
            return Json(ConventList);
        }
        public JsonResult FinanceConfigAdd(FinanceConfig model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            model.CompanyId = CookieOperate.MemberCookie.CompanyId;
            if (model.Id <= 0)
            {
                flag = FinanceConfigDBOperate.AddConfig(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                flag = FinanceConfigDBOperate.ModifyConfig(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        public JsonResult FinanceConfigGetOne(int id)
        {
            ResultInfo<FinanceConfig> result = new ResultInfo<FinanceConfig>() { IsSuccess = true };
            result.Data = FinanceConfigDBOperate.GetModelById(id);
            return Json(result);
        }

        public JsonResult FinancePermissionSet(int DepartId, string Ids)
        {
            ResultInfo result = new ResultInfo();
            result.IsSuccess = FinanceConfigPermissionDBOperate.Set(DepartId, Ids);
            if (result.IsSuccess) { result.Message = "设置成功"; }
            return Json(result);
        }
        public JsonResult FinancePermissionGet(int DepartId)
        {
            ResultInfo<List<int>> result = new ResultInfo<List<int>>() { IsSuccess = true };
            var list = FinanceConfigPermissionDBOperate.GetList(DepartId);
            result.Data = list.Select(a => a.FinanceConfigId).ToList();
            result.IsSuccess = true;
            return Json(result);
        }


        #endregion

        #region 财务管理
        public ActionResult FinanceReportInfo()
        {
            var list = FinanceConfigDBOperate.GetList("Report", CookieOperate.MemberCookie.CompanyId);
            ViewBag.TypeList = list.Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Id + ":" + a.Value });
            return View();
        }
        public JsonResult FinanceReportList(QueryBase model)
        {
            var totalcount = 0;
            model.CompanyId = CookieOperate.MemberCookie.CompanyId;
            var list = FinanceReportDBOperate.GetPagerList(model, out totalcount);
            PagerList<FinanceReport> pagerList = new PagerList<FinanceReport>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        public JsonResult FinanceReportAdd(FinanceReport model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var admin = CookieOperate.MemberCookie;
            model.CreateBy = admin.UserName;
            if (model.Id <= 0)
            {
                flag = FinanceReportDBOperate.AddFinanceReport(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                flag = FinanceReportDBOperate.ModifyFinanceReport(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        public JsonResult FinanceReportGetOne(int id)
        {
            ResultInfo<FinanceReport> result = new ResultInfo<FinanceReport>();
            result.Data = FinanceReportDBOperate.GetModelById(id);
            return Json(result);
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        public JsonResult ImportFinanceReport()
        {
            ResultInfo info = new ResultInfo();
            HttpPostedFileBase file = Request.Files["upfile"];
            var admin = CookieOperate.MemberCookie;
            if (file.FileName.Contains(".xls") || file.FileName.Contains(".xlsx"))
            {
                try
                {
                    DataTable dt = ExportHelper.ExcelToDataTable(file.InputStream, file.FileName, "", true);
                    if (FinanceReportBLL.ImportExcel(dt, admin))
                    {
                        info.Message = "导入成功";
                        info.IsSuccess = true;
                    }
                }
                catch (Exception ex)
                {
                    info.Message = ex.Message;
                }
            }
            return Json(info, "text/html");
        }

        public JsonResult FinanceReportData(QueryBase model)
        {
            ResultInfo info = new ResultInfo() { Message = "", IsSuccess = true };
            info.Message = FinanceReportDBOperate.GetReportData(model);
            return Json(info);
        }
        public JsonResult FinanceReportDel(int Id)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            FinanceReport model = new FinanceReport() { Id = Id, Operater = CookieOperate.MemberCookie.UserName };
            flag = FinanceReportDBOperate.DeleteFinanceReport(model);
            info.IsSuccess = flag;
            if (flag) { info.Message = "删除成功"; }
            return Json(info);
        }
        public JsonResult FinanceReportExport(QueryBase model)
        {
            ResultInfo info = new ResultInfo();

            string fileName = "财务报表";
            if (model.StartTime != DateTime.MinValue) { fileName += "_" + model.StartTime.ToString("yyyy-MM-dd"); }
            if (model.EndTime != DateTime.MinValue) { fileName += "_" + model.EndTime.ToString("yyyy-MM-dd"); }
            var list = FinanceReportBLL.GetExportList(model);
            Company.Util.ExportHelper.Export<ExportFinanceModel>(list, this.Response, fileName);

            return Json(new { });

        }
        #endregion

        #region 部门管理
        public ActionResult CompanyDepartMentInfo()
        {
            var list = CompanyDepartMentDBOperate.GetList(CookieOperate.MemberCookie.CompanyId);
            ViewBag.DepartMent = list.Where(a => a.PId == 0).Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Id + ":" + a.Name });
            var UserList = CompanyUserDBOperate.GetComapnyUserList(CookieOperate.MemberCookie.CompanyId);
            ViewBag.CompanyUserList = UserList.Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.DepartName + ":" + a.Name });
            return View();
        }
        public JsonResult CompanyDepartMentList()
        {
            var list = CompanyDepartMentDBOperate.GetList(CookieOperate.MemberCookie.CompanyId);
            var plist = list.Where(a => a.PId == 0).ToList();
            foreach (var item in plist)
            {
                item.ChildList = list.Where(a => a.PId == item.Id).ToList();
            }
            PagerList<CompanyDepartMent> pagerList = new PagerList<CompanyDepartMent>();
            pagerList.rows = plist;
            return Json(pagerList);
            //int pagesize = CommonMethod.GetInt(Request["limit"]);
            //int offset = CommonMethod.GetInt(Request["offset"]);
            //string name = CommonMethod.GetString(Request["name"]);
            //var totalcount = 0;
            //var list = CompanyDepartMentDBOperate.GetPagerList(pagesize, offset, name,CookieOperate.MemberCookie.CompanyId, out totalcount);
            //var plist = CompanyDepartMentDBOperate.GetList(0, CookieOperate.MemberCookie.CompanyId);
            //list.ForEach(a => {
            //    var pName = plist.FirstOrDefault(b => b.Id == a.PId);
            //    a.PName = pName != null ? pName.Name : "顶级";
            //});
            //PagerList<CompanyDepartMent> pagerList = new PagerList<CompanyDepartMent>();
            //pagerList.rows = list;
            //pagerList.total = totalcount;
            //return Json(pagerList);
        }
        public JsonResult CompanyDepartMentAdd(CompanyDepartMent model)
        {
            model.CompanyId = CookieOperate.MemberCookie.CompanyId;
            model.CompanyName = CookieOperate.MemberCookie.CompanyName;
            ResultInfo info = new ResultInfo();
            var flag = false;
            if (model.Id <= 0)
            {
                flag = CompanyDepartMentDBOperate.AddCompanyDepartMent(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                flag = CompanyDepartMentDBOperate.ModifyCompanyDepartMent(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        public JsonResult CompanyDepartMentGetOne(int id)
        {
            ResultInfo<CompanyDepartMent> result = new ResultInfo<CompanyDepartMent>();
            result.Data = CompanyDepartMentDBOperate.GetModelById(id);
            return Json(result);
        }
        #endregion

        #region 公司人员管理
        public ActionResult CompanyUserInfo()
        {
            var list = CompanyDepartMentDBOperate.GetListChild(CookieOperate.MemberCookie.CompanyId);
            ViewBag.DepartMent = list.Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Id + ":" + a.Name });
            ViewBag.Political = CommonMethod.GetEnumItems<PoliticalEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            ViewBag.Sex = CommonMethod.GetIntEnumItems<CompanySexEnum>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });
            ViewBag.Education = CommonMethod.GetEnumItems<EducationEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            ViewBag.HuKou = CommonMethod.GetIntEnumItems<HuKouEnum>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });
            return View();
        }
        public JsonResult CompanyUserList()
        {
            int pagesize = CommonMethod.GetInt(Request["limit"]);
            int offset = CommonMethod.GetInt(Request["offset"]);
            string name = CommonMethod.GetString(Request["name"]);
            var totalcount = 0;
            var list = CompanyUserDBOperate.GetPagerList(pagesize, offset, name, CookieOperate.MemberCookie.CompanyId, out totalcount);
            var dList = CompanyDepartMentDBOperate.GetListChild(CookieOperate.MemberCookie.CompanyId);
            list.ForEach(a =>
            {
                var pName = dList.FirstOrDefault(b => b.Id == a.DepartId);
                a.DepartName = pName != null ? pName.Name : "";
            });
            PagerList<CompanyUser> pagerList = new PagerList<CompanyUser>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        public JsonResult CompanyUserAdd(CompanyUser model)
        {
            var user = CookieOperate.MemberCookie;
            ResultInfo info = new ResultInfo();
            var flag = false;
            model.CompanyId = user.CompanyId;
            model.CompanyName = user.CompanyName;
            model.Ip = CommonMethod.GetClientIP;
            if (model.Id <= 0)
            {
                if (CompanyUserDBOperate.CheckUserName(model.UserName))
                {
                    info.Message = "登录名已存在";
                    return Json(info);
                }
                model.Password = CommonMethod.PasswordMD5(model.Password);
                model.CreateBy = user.UserName;
                model.CreateDate = DateTime.Now;
                flag = CompanyUserDBOperate.AddCompanyUser(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                var newModel = CompanyUserDBOperate.GetModelById(model.Id);
                if (!string.IsNullOrEmpty(model.Name))
                    newModel.Name = model.Name;
                if (!string.IsNullOrEmpty(model.WeChatHeadUrl)) {
                    newModel.WeChatHeadUrl = model.WeChatHeadUrl;
                    if (user.Id == newModel.Id) {
                        CookieOperate.MemberCookie = newModel;
                    }
                }
                if (!string.IsNullOrEmpty(model.Password)) {
                    if (newModel.Password != CommonMethod.PasswordMD5(model.Password)) {
                        info.Message = "旧密码错误";
                        return Json(info);
                    }
                    newModel.Password = CommonMethod.PasswordMD5(model.ModifyBy);
                }
                if (model.DepartId != 0)
                    newModel.DepartId = model.DepartId;
                if (!string.IsNullOrEmpty(model.Mobile))
                    newModel.Mobile = model.Mobile;
                if (!string.IsNullOrEmpty(model.Email))
                    newModel.Email = model.Email;
                if (!string.IsNullOrEmpty(model.UsedName))
                    newModel.UsedName = model.UsedName;
                if (!string.IsNullOrEmpty(model.SocialNo))
                    newModel.SocialNo = model.SocialNo;
                if (!string.IsNullOrEmpty(model.CardNo))
                    newModel.CardNo = model.CardNo;
                if (!string.IsNullOrEmpty(model.CardFront))
                    newModel.CardFront = model.CardFront;
                if (!string.IsNullOrEmpty(model.CardFontBehind))
                    newModel.CardFontBehind = model.CardFontBehind;
                if (!string.IsNullOrEmpty(model.Nation))
                    newModel.Nation = model.Nation;
                if (!string.IsNullOrEmpty(model.Political))
                    newModel.Political = model.Political;

                if (!string.IsNullOrEmpty(model.Education))
                    newModel.Education = model.Education;
                if (!string.IsNullOrEmpty(model.EducationPhoto))
                    newModel.EducationPhoto = model.EducationPhoto;
                if (!string.IsNullOrEmpty(model.HomeAddress))
                    newModel.HomeAddress = model.HomeAddress;
                if (!string.IsNullOrEmpty(model.Address))
                    newModel.Address = model.Address;
                if (model.EnterDate != DateTime.MinValue)
                    newModel.EnterDate = model.EnterDate;
                if (model.Sex != 0)
                    newModel.Sex = model.Sex;
                if (model.HomeAddressType != 0)
                    newModel.HomeAddressType = model.HomeAddressType;

                newModel.ModifyBy = user.UserName;
                newModel.ModifyDate = DateTime.Now;
                flag = CompanyUserDBOperate.ModifyCompanyUser(newModel);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        public JsonResult CompanyUserGetOne(int id)
        {
            ResultInfo<CompanyUser> result = new ResultInfo<CompanyUser>();
            result.Data = CompanyUserDBOperate.GetModelById(id);
            return Json(result);
        }
        public JsonResult CompanyUserStatus(int id, int isDelete)
        {
            var user = CookieOperate.MemberCookie;
            ResultInfo info = new ResultInfo();
            var flag = CompanyUserDBOperate.ModifyStatus(id, isDelete, user.UserName);
            if (flag) { info.Message = "保存成功"; }
            return Json(info);
        }

        public ActionResult CompanyUserDetail(int id) {
            var model = CompanyUserDBOperate.GetModelById(id);
            var company = CompanyInfoDBOperate.GetModelById(model.CompanyId);
            model.CompanyName = company.CompanyFullName;
            var dep = CompanyDepartMentDBOperate.GetChildOne(model.DepartId, model.CompanyId);
            model.WeChatHeadUrl = string.IsNullOrEmpty(model.WeChatHeadUrl) ? "/content/img/profile_small.jpg" : model.WeChatHeadUrl;
            if (dep != null)
                model.DepartName = dep.Name;
            if (model.HomeAddressType != 0)
                model.HomeAddressTypeTxt = CommonMethod.GetEnumDesc<HuKouEnum>(model.HomeAddressType);// Enum.GetName(typeof(HuKouEnum), model.HomeAddressType);
            if (model.Sex != 0)
                model.SexTxt = CommonMethod.GetEnumDesc<CompanySexEnum>(model.Sex);
            if (!string.IsNullOrEmpty(model.Political))
                model.PoliticalTxt = CommonMethod.GetEnumDesc<PoliticalEnum>(model.Political);
            if (!string.IsNullOrEmpty(model.Education))
                model.EducationTxt = CommonMethod.GetEnumDesc<EducationEnum>(model.Education);
            if (model.EnterDate == null || model.EnterDate == DateTime.MinValue) { model.EnterDate = DateTime.Now; }
            if (string.IsNullOrEmpty(model.CardFront))
                model.CardFront = "";
            if (string.IsNullOrEmpty(model.CardFontBehind))
                model.CardFontBehind = "";
            if (string.IsNullOrEmpty(model.EducationPhoto))
                model.EducationPhoto = "";
            //ViewBag.Political = CommonMethod.GetEnumItems<PoliticalEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            //ViewBag.Sex = CommonMethod.GetIntEnumItems<CompanySexEnum>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });
            //ViewBag.Education = CommonMethod.GetEnumItems<EducationEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            //ViewBag.HuKou = CommonMethod.GetIntEnumItems<HuKouEnum>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });
            return View(model);
        }

        public JsonResult CompanyUserListNotPager() {
            var user = CookieOperate.MemberCookie;
            var list = CompanyUserDBOperate.GetUser(user.CompanyId);
            var result = new ResultInfo<List<CompanyUser>>();
            result.IsSuccess = true;
            result.Data = list;
            return Json(result);
        }

        
        #endregion

        #region 审核步骤管理

        //设置权限
        public JsonResult AuditStepAdd(string Content, int DepartMent)
        {
            ResultInfo result = new ResultInfo();
            result.IsSuccess = AuditStepDBOperate.AuditStepAdd(Content, DepartMent, CookieOperate.MemberCookie.CompanyId,
                CookieOperate.MemberCookie.CompanyName);
            if (result.IsSuccess) { result.Message = "设置成功"; }
            return Json(result);
        }
        public JsonResult AuditStepGet(int DepartMent)
        {
            ResultInfo<List<AuditStep>> result = new ResultInfo<List<AuditStep>>();
            result.IsSuccess = true;
            result.Data = AuditStepDBOperate.GetAuditStepByDepartId(DepartMent);
            if (result.Data.Count == 0) { result.IsSuccess = false; }
            return Json(result);
        }
        #endregion

        #region 客户公司管理 
        //主页
        public ActionResult CustomerCompanyInfo()
        {
            ViewBag.Status = CommonMethod.GetEnumItems<CustomerCompanyStatusEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            return View();
        }
        //主页请求的数据
        public JsonResult CustomerCompanyList(QueryBase query)
        {
            var totalcount = 0;
            query.CompanyId = CookieOperate.MemberCookie.CompanyId;
            var list = CustomerCompanyDBOperate.GetPagerList(query, out totalcount);
            PagerList<CustomerCompany> pagerList = new PagerList<CustomerCompany>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }

        //编辑和新增请求的数据
        public JsonResult CustomerCompanyAdd(CustomerCompany model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            model.CompanyId = user.CompanyId;
            model.CompanyName = user.CompanyName;
            if (model.Id <= 0)
            {
                model.CreateBy = user.UserName;
                model.CreateDate = DateTime.Now;
                model.Source = SourceEnum.Admin.ToString();
                flag = CustomerCompanyDBOperate.AddCustomerCompany(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                model.ModifyBy = user.UserName;
                model.ModifyDate = DateTime.Now;
                flag = CustomerCompanyDBOperate.ModifyCustomerCompany(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        //获取单个对象的数据
        public JsonResult CustomerCompanyGetOne(int id)
        {
            ResultInfo<CustomerCompany> result = new ResultInfo<CustomerCompany>();
            result.Data = CustomerCompanyDBOperate.GetModelById(id);
            return Json(result);
        }

        #endregion


        #region 客户人员管理 
        //主页
        public ActionResult CustomerMemberInfo()
        {
            return View();
        }
        //主页请求的数据
        public JsonResult CustomerMemberList(QueryBase query)
        {
            var totalcount = 0;
            query.CompanyId = CookieOperate.MemberCookie.CompanyId;
            var list = CustomerMemberDBOperate.GetPagerList(query, out totalcount);
            PagerList<CustomerMember> pagerList = new PagerList<CustomerMember>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }

        //编辑和新增请求的数据
        public JsonResult CustomerMemberAdd(CustomerMember model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            model.CompanyId = user.CompanyId;
            model.CompanyName = user.CompanyName;
            if (model.Id <= 0)
            {
                model.CreateBy = user.UserName;
                model.CreateDate = DateTime.Now;
                model.Source = SourceEnum.Admin.ToString();
                flag = CustomerMemberDBOperate.AddCustomerMember(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                model.ModifyBy = user.UserName;
                model.ModifyDate = DateTime.Now;
                flag = CustomerMemberDBOperate.ModifyCustomerMember(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        //获取单个对象的数据
        public JsonResult CustomerMemberGetOne(int id)
        {
            ResultInfo<CustomerMember> result = new ResultInfo<CustomerMember>();
            result.Data = CustomerMemberDBOperate.GetModelById(id);
            return Json(result);
        }

        #endregion

        #region 项目管理 
        //主页
        public ActionResult ProjectManageInfo()
        {
            var user = CookieOperate.MemberCookie;
            ViewBag.PayWay = CommonMethod.GetEnumItems<ProjectPayWayEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            ViewBag.Status = CommonMethod.GetEnumItems<ProjectStatusEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            ViewBag.Member = CustomerMemberDBOperate.GetList(user.CompanyId).Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.CompanyName + "-" + a.Name });
            return View();
        }
        //主页请求的数据
        public JsonResult ProjectManageList(QueryBase query)
        {
            string payway = CommonMethod.GetString(Request["PayWay"]);
            string status = CommonMethod.GetString(Request["Status"]);
            query.CompanyId = CookieOperate.MemberCookie.CompanyId;
            var totalcount = 0;
            var list = ProjectManageDBOperate.GetPagerList(query, payway, status, out totalcount);
            var payways = CommonMethod.GetEnumItems<ProjectPayWayEnum>();
            var statuss = CommonMethod.GetEnumItems<ProjectStatusEnum>();
            list.ForEach(a =>
            {
                a.StatusTxt = statuss.FirstOrDefault(b => b.Key == a.Status).Value;
                a.PayWayTxt = payways.FirstOrDefault(b => b.Key == a.PayWay).Value;
            });
            PagerList<ProjectManage> pagerList = new PagerList<ProjectManage>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }

        //编辑和新增请求的数据
        public JsonResult ProjectManageAdd(ProjectManage model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            var cusMember = CustomerMemberDBOperate.GetModelById(model.CusMemberId);
            model.CusCompanyId = cusMember.CusCompanyId;
            if (model.Id <= 0)
            {
                model.CreateBy = user.UserName;
                model.CreateDate = DateTime.Now;
                model.RelStartTime = model.StartTime;
                model.RelEndTime = model.EndTime;
                model.LastPayDate = model.EndTime;
                model.CompanyId = user.CompanyId;
                model.Source = SourceEnum.Admin.ToString();
                flag = ProjectManageDBOperate.AddProjectManage(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                model.ModifyBy = user.UserName;
                model.ModifyDate = DateTime.Now;
                model.RelStartTime = model.StartTime;
                model.RelEndTime = model.EndTime;
                model.LastPayDate = model.EndTime;
                model.CompanyId = user.CompanyId;
                flag = ProjectManageDBOperate.ModifyProjectManage(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        //获取单个对象的数据
        public JsonResult ProjectManageGetOne(int id)
        {
            ResultInfo<ProjectManage> result = new ResultInfo<ProjectManage>();
            result.Data = ProjectManageDBOperate.GetModelById(id);
            return Json(result);
        }

        public JsonResult ProjectManageDelete(int id, int IsDeleted) {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            flag = ProjectManageDBOperate.DeleteProjectManage(new ProjectManage() { ModifyBy = user.UserName, Id = id, IsDeleted = IsDeleted });
            if (flag) { info.Message = "删除成功"; info.IsSuccess = true; }
            return Json(info);
        }

        public JsonResult ProjectManageModifyDate(int id, DateTime date)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            flag = ProjectManageDBOperate.ProjectManageModifyDate(new ProjectManage() { ModifyBy = user.UserName, Id = id, PayDate = date });
            if (flag) { info.Message = "修改成功"; info.IsSuccess = true; }
            return Json(info);
        }
        #endregion

        #region 工单管理 
        //主页
        public ActionResult WorkListInfo()
        {
            int pid = CommonMethod.GetInt(Request["pid"]);
            var projectName = "";
            if (pid != 0) {
                var model = ProjectManageDBOperate.GetModelById(pid);
                if (model != null)
                    projectName = "->[" + model.Name + "/" + model.FullName + "]";
            }
            ViewBag.ProjectName = projectName;
            ViewBag.ProjectId = pid;
            var user = CookieOperate.MemberCookie;
            ViewBag.Project = ProjectManageDBOperate.GetList(user.CompanyId,0).Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name + "-" + a.FullName });
            ViewBag.Member = CustomerMemberDBOperate.GetList(user.CompanyId).Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name });
            ViewBag.WorkListStatus = CommonMethod.GetEnumItems<WorkListStatusEnum>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });
            return View();
        }
        //主页请求的数据
        public JsonResult WorkListList(WorkListQuery query)
        {
            var totalcount = 0;
            query.CompanyId = CookieOperate.MemberCookie.CompanyId;
            var list = WorkListDBOperate.GetPagerList(query, out totalcount);
            PagerList<WorkList> pagerList = new PagerList<WorkList>();
            var status = CommonMethod.GetEnumItems<WorkListStatusEnum>();
            //var userList = CompanyUserDBOperate.GetUserByUserNames(string.Join(",", list.Select(a => a.Users)).Split(',').ToArray());
            var detailList = WorkListDetailDBOperate.GetList(list.Select(a => a.Guid).ToArray());

            list.ForEach(a =>
            {
                a.StatusTxt = status.FirstOrDefault(b => b.Key == a.Status).Value;
                a.StatusClass = CommonMethod.GetWorkListStatusClass(a.Status);
                a.DetailList = detailList.Where(b=>b.WorkId==a.Guid).ToList();
                //if (string.IsNullOrEmpty(a.Users)) { return; }
                //var thisUserList = new List<CompanyUser>();
                //var thisUserArray = a.Users.Split(',').ToArray();
                //foreach (var item in thisUserArray)
                //{
                //    if (!string.IsNullOrEmpty(item))
                //        thisUserList.Add(userList.FirstOrDefault(b => b.UserName == item));
                //}
                //a.UserList = thisUserList;
            });
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }

        //编辑和新增请求的数据
        public JsonResult WorkListAdd(WorkList model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            model.CompanyId = user.CompanyId;
            model.CompanyName = user.CompanyName;
      
            if (model.Id <= 0)
            {
                var project = ProjectManageDBOperate.GetModelById(model.ProjectId);
                model.ProjectName = project.Name;
                var member = CustomerMemberDBOperate.GetModelById(model.CusMemberId);
                model.CusCompanyId = member.CusCompanyId;
                model.CreateBy = user.UserName;
                model.CreateDate = DateTime.Now;
                flag = WorkListDBOperate.AddWorkList(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                var old = WorkListDBOperate.GetModelById(model.Id);
                if (!string.IsNullOrEmpty(model.UserId))
                    old.UserId = model.UserId;
                if (!string.IsNullOrEmpty(model.UserName)) {
                    old.UserName = model.UserName;
                    WorkListItemsDBOperate.ModifyItemList(model.UserId, model.UserName, model.Id);
                }
                if (model.DoTime != null)
                    old.DoTime = model.DoTime;
                if (model.GetTime != null)
                    old.GetTime = model.GetTime;
                if (model.NeedTime != null)
                    old.NeedTime = model.NeedTime;
                if(!string.IsNullOrEmpty(model.Status))
                    old.Status = model.Status;

                old.ModifyBy = user.UserName;
                old.ModifyDate = DateTime.Now;
                flag = WorkListDBOperate.ModifyWorkList(old);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        //获取单个对象的数据
        public JsonResult WorkListGetOne(int id)
        {
            ResultInfo<WorkList> result = new ResultInfo<WorkList>();
            result.Data = WorkListDBOperate.GetModelById(id);
            return Json(result);
        }
        public JsonResult WorkListDelete(int id, int IsDeleted)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            flag = WorkListDBOperate.DeleteWorkList(new WorkList() { ModifyBy = user.UserName, Id = id, IsDeleted = IsDeleted });
            if (flag) { info.Message = "删除成功"; info.IsSuccess = true; }
            return Json(info);
        }
        public JsonResult ImportWorkListReport(int ProjectId,string status)
        {
            ResultInfo info = new ResultInfo();
            HttpPostedFileBase file = Request.Files["upfile"];
            var admin = CookieOperate.MemberCookie;
            if (file.FileName.Contains(".xls"))
            {
                try
                {
                    DataTable dt = ExportHelper.ExcelToDataTable(file.InputStream, file.FileName, "", true);
                    string message = string.Empty;
                    if (WorkListBLL.ImportExcel(dt, admin, ProjectId, status, out message))
                    {
                        info.Message = "导入成功";
                        info.IsSuccess = true;
                    }
                    else {
                        info.Message = message;
                    }
                }
                catch (Exception ex)
                {
                    info.Message = ex.Message;
                }
            }
            else {
                info.Message = "目前只支持.xls格式";
            }
            return Json(info, "text/html");
        }

        /// <summary>
        /// 编辑详情
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public JsonResult WorkDetailEdit(string str)
        {
            var list = JsonConvert.DeserializeObject<List<WorkListDetail>>(str);

            var result = new ResultInfo();
            var flag = WorkListBLL.ModifyDetail(list, CookieOperate.MemberCookie.UserName);
            if (flag) { result.IsSuccess = true; result.Message = "更新成功"; }
            else {
                result.Message = "更新失败";
            }
            return Json(result);
        }
        public JsonResult WorkDetailDelete(string guid) {
            var result = new ResultInfo();
            var count = WorkListItemsDBOperate.getCountByDetailId(guid);
            if (count >0) { result.Message = "已安排工单，不能删除"; return Json(result); }
            var flag = WorkListDetailDBOperate.DeleteWorkListDetail(guid);
            if (flag) {
                result.IsSuccess = true; result.Message = "删除成功";
            }
            return Json(result);
        }
        #endregion

        #region 工单条目管理 
        public ActionResult WorkListDetail(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public JsonResult WorkDetailById(int id) {
            var result = new ResultInfo<WorkList>();
            try
            {
                var user = CookieOperate.MemberCookie;
                var workDetail = WorkListDBOperate.GetModelById(id);
                //if (!string.IsNullOrEmpty(workDetail.Users))
                //{
                //    var userArray = workDetail.Users.Split(',').ToArray();
                //    if (userArray.Length > 0)
                //    {
                //        workDetail.UserList = new List<CompanyUser>();
                //        var memberList = CompanyUserDBOperate.GetUserByUserNames(userArray); ;
                //        foreach (var item in userArray)
                //        {
                //            if(!string.IsNullOrEmpty(item))
                //                workDetail.UserList.Add(memberList.FirstOrDefault(a => a.UserName == item));
                //        }
                //    }
                //}
                workDetail.StatusClass = CommonMethod.GetWorkListStatusClass(workDetail.Status);
                workDetail.StatusTxt = CommonMethod.GetEnumDesc<WorkListStatusEnum>(workDetail.Status);

                #region 详细条目
                workDetail.ItemList = WorkListItemsDBOperate.GetModelByWorkId(id);

                var level = CommonMethod.GetEnumItems<ItemLevel>();
                var status = CommonMethod.GetEnumItems<ItemStatus>();
                workDetail.ItemList.ForEach(a => {
                    a.StatusName = status.FirstOrDefault(b => b.Key == a.Status).Value;
                    a.StatusClass = CommonMethod.GetItemStatus(a.Status);
                    //a.LevelName = level.FirstOrDefault(b => b.Key == a.Level).Value;
                    //a.LevelClass = CommonMethod.GetItemLevel(a.Level);
                    //a.CanEdit = a.UserId == user.Id && (a.Status != ItemStatus.End.ToString() && a.Status != ItemStatus.YuEnd.ToString());
                });
                #endregion

                if (CookieOperate.MemberCookie.UserName == workDetail.CreateBy) {
                    workDetail.CanEdit = true;
                }
                var createBy = CompanyUserDBOperate.GetModelByName(workDetail.CreateBy);
                if (createBy != null)
                    workDetail.CreateBy = createBy.Name;
                var cusMember = CustomerMemberDBOperate.GetModelById(workDetail.CusMemberId);
                if(cusMember!=null)
                    workDetail.CusMemberName = cusMember.Name;

                result.IsSuccess = true;
                result.Data = workDetail;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 工单团队成员
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="UserName"></param>
        /// <param name="WorkId"></param>
        /// <returns></returns>
        public JsonResult WorkDetailAdminEdit(int Type, string UserName,int WorkId)
        {
            var result = new ResultInfo();

            try
            {
                var workDetail = WorkListDBOperate.GetModelById(WorkId);
                if (Type == 1)
                {
                    workDetail.UserId = workDetail.UserId.Replace(UserName + ",", "");
                    result.Message = "删除成员成功";
                }
                else
                {
                    if (workDetail.UserId.IndexOf(UserName) > -1)
                    {
                        result.Message = "成员已添加";
                        return Json(result);
                    }
                    workDetail.UserId = workDetail.UserId + UserName + ",";
                    result.Message = "添加成员成功";
                }
                if (WorkListDBOperate.ModifyWorkList(workDetail))
                {
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
           
            return Json(result);
        }

        public JsonResult DetialItemsTemplate() {
            var dt = new DataTable();
            dt.Columns.Add("工单编号");
            dt.Columns.Add("条目名称");
            dt.Columns.Add("功能描述");
            dt.Columns.Add("负责人");
            dt.Columns.Add("工单等级");
            dt.Columns.Add("工单耗时");
            dt.Columns.Add("工单类型");
            dt.Columns.Add("工单状态");
            dt.Rows.Add(new object[] {
                "10000", "会员注册","注册需要修改...","沈波,郭贝贝","0","1","后端开发","已分配"
            });
            Company.Util.ExportHelper.ExportByWeb(dt, "头", "工单明细模板");
            return Json(new { });
        }
        public ActionResult WorkItemAdd() {
            var user = CookieOperate.MemberCookie;
            ViewBag.ItemType = CommonMethod.GetEnumItems<ItemType>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });
            ViewBag.ItemStatus = CommonMethod.GetEnumItems<ItemStatus>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });
            ViewBag.Member = CompanyUserDBOperate.GetUser(user.CompanyId).Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name });
            ViewBag.WorkList = WorkListDBOperate.GetListByCompanyId(user.CompanyId).Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name });

            return View();
        }
        public JsonResult ImportDetailItemsReport()
        {
            ResultInfo info = new ResultInfo();
            HttpPostedFileBase file = Request.Files["upfile"];
            var admin = CookieOperate.MemberCookie;
            if (file.FileName.Contains(".xls"))
            {
                try
                {
                    DataTable dt = ExportHelper.ExcelToDataTable(file.InputStream, file.FileName, "", true);
                    string msg = string.Empty;
                    if (WorkItemsBLL.ImportExcel(dt, admin, out msg))
                    {
                        info.Message = "导入成功";
                        info.IsSuccess = true;
                    }
                    else
                    {
                        info.Message = msg;
                    }
                }
                catch (Exception ex)
                {
                    info.Message = ex.Message;
                }
            }
            else
            {
                info.Message = "目前只支持.xls格式";
            }
            return Json(info, "text/html");
        }
        /// <summary>
        /// 工单处理情况
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        public JsonResult WorkDetailStepEdit(int ItemId) {
            var result = new ResultInfo();
            try
            {
                var item = WorkListItemsDBOperate.GetModelById(ItemId);
                //开始处理
                if (item.Status == ItemStatus.HasPei.ToString())
                {
                    item.RelStartDate = DateTime.Now;
                    item.Status = ItemStatus.InHand.ToString();
                }
                //完成
                else if (item.Status == ItemStatus.InHand.ToString())
                {
                    item.RelEndDate = DateTime.Now;
                    item.Status = item.RelEndDate > item.EndDate ? ItemStatus.YuEnd.ToString() : ItemStatus.End.ToString();
                }
                var flag =  WorkListItemsDBOperate.ModifyWorkListItemsStatus(item);
                if (flag) {
                    result.IsSuccess = true;
                    result.Message = "操作成功";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(result);
        }
        //主页
        public ActionResult WorkListItemsInfo()
        {
            var user = CookieOperate.MemberCookie;
            ViewBag.WorkList = WorkListDBOperate.GetListByCompanyId(user.CompanyId).Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name });
            return View();
        }
        //主页请求的数据
        public JsonResult WorkListItemsList(WorkListItemQuery query)
        {
            var totalcount = 0;
            query.CompanyId = CookieOperate.MemberCookie.CompanyId;
            var list = WorkListItemsDBOperate.GetPagerList(query, out totalcount);
            var type = CommonMethod.GetEnumItems<ItemType>();
            var status = CommonMethod.GetEnumItems<ItemStatus>();
            list.ForEach(a => {
                a.StatusName = status.FirstOrDefault(b => b.Key == a.Status).Value;
                a.StatusClass = CommonMethod.GetItemStatus(a.Status);
                a.Type = type.FirstOrDefault(b => b.Key == a.Type).Value;
            });
            PagerList<WorkListItems> pagerList = new PagerList<WorkListItems>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult WorkListItemsDelete(WorkListItems model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            model.ModifyBy = user.UserName;
            model.ModifyDate = DateTime.Now;
            flag = WorkListItemsDBOperate.DeleteWorkListItems(model);
            info.IsSuccess = flag;
            if (flag) { info.Message = "删除成功"; }
            return Json(info);
        }
        //编辑和新增请求的数据
        public JsonResult WorkListItemsAdd(WorkListItems model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            model.CompanyId = user.CompanyId;
            model.CompanyName = user.CompanyName;
            var work = WorkListDBOperate.GetModelById(model.WorkId);
            if (work != null)
                model.ProjectId = work.ProjectId;
            //var users = CompanyUserDBOperate.GetModelById(model.UserId);
            //if (users != null)
            //    model.UserName = users.Name;
            if (model.Id <= 0)
            {
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now.AddDays(model.DayCount);
                model.CreateBy = user.UserName;
                model.CreateDate = DateTime.Now;
                flag = WorkListItemsDBOperate.AddWorkListItems(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                var old = WorkListItemsDBOperate.GetModelById(model.Id);
                if (model.DayCount != 0)
                    old.DayCount = model.DayCount;
                var LevelModify = CommonMethod.GetString(Request["LevelModify"]);//.ToString();
                if (!string.IsNullOrEmpty(LevelModify))
                    old.Level = CommonMethod.GetInt(LevelModify);
                if (!string.IsNullOrEmpty(model.Status))
                    old.Status = model.Status;
                if (!string.IsNullOrEmpty(model.UserId))
                    old.UserId = model.UserId;
                if (!string.IsNullOrEmpty(model.UserName))
                    old.UserName = model.UserName;
                if (!string.IsNullOrEmpty(model.Type))
                    old.Type = model.Type;
                if (!string.IsNullOrEmpty(model.Infos))
                    old.Infos = model.Infos;
                old.ModifyBy = user.UserName;
                old.ModifyDate = DateTime.Now;
                flag = WorkListItemsDBOperate.ModifyWorkListItems(old);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        //获取单个对象的数据
        public JsonResult WorkListItemsGetOne(int id)
        {
            ResultInfo<WorkListItems> result = new ResultInfo<WorkListItems>();
            result.Data = WorkListItemsDBOperate.GetModelById(id);
            return Json(result);
        }

        public JsonResult GetUserDayCount(string user) {
            var result = new ResultInfo<List<WorkListItems>>();
            result.Data = WorkListItemsDBOperate.getUserDayCount(user);
            result.IsSuccess = true;
            return Json(result);
        }

        public JsonResult GetAllUserDayCount() {
            var result = new ResultInfo<List<WorkItemsDayCount>>();
            var user = CookieOperate.MemberCookie;
            var userList = CompanyUserDBOperate.GetComapnyUserList(user.CompanyId).Where(a => a.IsDeleted == 0).OrderBy(a => a.Id).ToList();
            var list = new List<WorkItemsDayCount>();
            var count = new int[] { 0, 1, 2 };
            foreach (var item in userList)
            {
                var model = WorkListItemsDBOperate.getUserDayCount(item.UserName,count);
                var newModel = new WorkItemsDayCount()
                {
                    DepartMent = item.DepartName,
                    Name = item.Name,
                    UserName = item.UserName,
                    One = 0,
                    Three = 0,
                    Two = 0
                };
                var first = model.FirstOrDefault(a => a.Level == 0);
                if  (first!= null)  newModel.One = first.DayCount;
                var two = model.FirstOrDefault(a => a.Level == 1);
                if (two != null) newModel.Two = two.DayCount;
                var three = model.FirstOrDefault(a => a.Level ==2);
                if (three != null) newModel.Three = three.DayCount;
                list.Add(newModel);
            }
            result.Data = list;
            result.IsSuccess = true;
            return Json(result);
        }
        #endregion

        #region 获取枚举
        public JsonResult GetEnum(string type) {
            var dic = new Dictionary<string, string>();
            switch (type)
            {
                case "ItemType":  dic = CommonMethod.GetEnumItems<ItemType>();break;
                case "ItemStatus": dic = CommonMethod.GetEnumItems<ItemStatus>(); break;
                case "WorkStatus": dic = CommonMethod.GetEnumItems<WorkListStatusEnum>(); break;
            }
            return Json(dic);
        }
        #endregion

        #region 新闻管理 
        //主页
        public ActionResult NewsInfoInfo()
        {
            return View();
        }
        public ActionResult NewsInfoInfoViewAdd()
        {
            return View();
        }
        //主页请求的数据
        public JsonResult NewsInfoList(QueryBase query)
        {
            var totalcount = 0;
            query.CompanyId = CookieOperate.MemberCookie.CompanyId;
            var list = NewsInfoDBOperate.GetPagerList(query,false, out totalcount);
            PagerList<NewsInfo> pagerList = new PagerList<NewsInfo>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }

        //编辑和新增请求的数据
        [ValidateInput(false)]
        public JsonResult NewsInfoAdd(NewsInfo model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            //model.CompanyId = user.CompanyId;
            //model.CompanyName = user.CompanyName;
            if (model.Id <= 0)
            {
                model.CreateBy = user.UserName;
                model.CreateDate = DateTime.Now;
                model.GUID = Guid.NewGuid().ToString();
                flag = NewsInfoDBOperate.AddNewsInfo(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                model.ModifyBy = user.UserName;
                model.ModifyDate = DateTime.Now;
                flag = NewsInfoDBOperate.ModifyNewsInfo(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        //获取单个对象的数据
        public JsonResult NewsInfoGetOne(int id)
        {
            ResultInfo<NewsInfo> result = new ResultInfo<NewsInfo>();
            result.Data = NewsInfoDBOperate.GetModelById(id);
            return Json(result);
        }
        public JsonResult NewsInfoStatus(int id, int isDelete)
        {
            var user = CookieOperate.MemberCookie;
            ResultInfo info = new ResultInfo();
            var flag = CompanyUserDBOperate.ModifyStatus(id, isDelete, user.UserName);
            if (flag) { info.Message = "保存成功"; }
            return Json(info);
        }

        #endregion

        #region 管理 
        //主页
        public ActionResult JobListInfo()
        {
            return View();
        }
        //主页请求的数据
        public JsonResult JobListList(QueryBase query)
        {
            var totalcount = 0;
            query.CompanyId = CookieOperate.MemberCookie.CompanyId;
            var list = JobListDBOperate.GetPagerList(query, out totalcount);
            PagerList<JobList> pagerList = new PagerList<JobList>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        public JsonResult JobList(int isparent) {
            var data = JobListDBOperate.GetList(isparent);
            return Json(data);
        }

        //编辑和新增请求的数据
        public JsonResult JobListAdd(JobList model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.MemberCookie;
            if (model.Id <= 0)
            {
                model.CreateBy = user.UserName;
                model.CreateDate = DateTime.Now;
                flag = JobListDBOperate.AddJobList(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                model.ModifyBy = user.UserName;
                model.ModifyDate = DateTime.Now;
                flag = JobListDBOperate.ModifyJobList(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        //获取单个对象的数据
        public JsonResult JobListGetOne(int id)
        {
            ResultInfo<JobList> result = new ResultInfo<JobList>();
            result.Data = JobListDBOperate.GetModelById(id);
            return Json(result);
        }
        public JsonResult JobDeleted(int id) {
            ResultInfo result = new ResultInfo();
            var data = JobListDBOperate.GetList(id);
            if (data != null && data.Count()>0)  { result.Message = "请删除子集,再删除父级"; return Json(result); }
            var flag =  JobListDBOperate.ModifyJobList(id, CookieOperate.MemberCookie.UserName);
            result.IsSuccess = flag;
            return Json(result);
        }

        #endregion
    }
}