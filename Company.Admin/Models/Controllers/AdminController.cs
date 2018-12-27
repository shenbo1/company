using Company.Admin.Common;
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

using Company.Dto.Data;

using System.IO;

namespace Company.Admin.Controllers
{
    [AdminLoginFilter]
    public class AdminController : Controller
    {
        #region 首页
        public ActionResult Index()
        {
            ViewBag.Name = CookieOperate.UserAdminCookie.Name;
            ViewBag.ID = CookieOperate.UserAdminCookie.Id;
            return View();
        }
        public ActionResult Login() 
        {
            CookieOperate.UserAdminCookie = null;
            return View();
        }
        public ActionResult Welcome()
        {
            return View();
        }
        public JsonResult AdminLogin(string LoginId, string Password)
        {
            ResultInfo info = new ResultInfo() { Message = "用户名或者密码错误" };
            var model = AdminInfoDBOperate.Login(LoginId, CommonMethod.PasswordMD5( Password));
            if (model != null)
            {
                info.IsSuccess = true;
                CookieOperate.UserAdminCookie = model;
            }
            return Json(info);
        }
        public ActionResult MenuList()
        {
            string html = "";
            var menuList = UserMenuDBOperate.GetRoleMenuList(CookieOperate.UserAdminCookie.RoleId);
            var menuParent = menuList.Where(a => a.PID == 0).ToList();
            html = MenuListHtml(menuList, menuParent);
            return Content("document.write('" + html + "')");
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
                var childList = list.Where(a => a.PID == item.Id).ToList();
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
            return Json(ConventList);
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
            int companyId = CookieOperate.UserAdminCookie.CompanyId;

            ViewBag.CompanyList = CompanyInfoDBOperate.GetList(companyId).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.CompanyName+"--"+ a.CompanyFullName
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
            int companyId = CookieOperate.UserAdminCookie.CompanyId;
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

        #region 配置管理
        #region 管理
        public ActionResult ConfigInfo()
        {
            return View();
        }
        public JsonResult ConfigList()
        {
            int pagesize = CommonMethod.GetInt(Request["limit"]);
            int offset = CommonMethod.GetInt(Request["offset"]);
            string name = CommonMethod.GetString(Request["name"]);
            var totalcount = 0;
            var list = ConfigDBOperate.GetPagerList(pagesize, offset, name, out totalcount);
            PagerList<Config> pagerList = new PagerList<Config>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        public JsonResult ConfigAdd(Config model)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            if (model.Id <= 0)
            {
                flag = ConfigDBOperate.AddConfig(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                flag = ConfigDBOperate.ModifyConfig(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        public JsonResult ConfigGetOne(int id)
        {
            ResultInfo<Config> result = new ResultInfo<Config>();
            result.Data = ConfigDBOperate.GetModelById(id);
            return Json(result);
        }
        #endregion
        #endregion

        #region 财务管理
        public ActionResult FinanceReportInfo()
        {
            var list = ConfigDBOperate.GetList("Report", CookieOperate.UserAdminCookie.CompanyId);
            ViewBag.TypeList = list.Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Id + ":" + a.Value });
            return View();
        }
        public JsonResult FinanceReportList(QueryBase model)
        {
            var totalcount = 0;
            model.CompanyId = CookieOperate.UserAdminCookie.CompanyId;
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
            AdminInfo admin = CookieOperate.UserAdminCookie;
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
            AdminInfo admin = CookieOperate.UserAdminCookie;
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
            ResultInfo info = new ResultInfo() { Message = "",IsSuccess = true };
            info.Message = FinanceReportDBOperate.GetReportData(model);
            return Json(info);
        }
        public JsonResult FinanceReportDel(int Id)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
            FinanceReport model = new FinanceReport() { Id = Id, Operater = CookieOperate.UserAdminCookie.UserName };
            flag = FinanceReportDBOperate.DeleteFinanceReport(model);
            info.IsSuccess = flag;
            if (flag) { info.Message = "删除成功"; }
            return Json(info);
        }
        public ActionResult FinanceReportExport(QueryBase model) 
        {
            ResultInfo info = new ResultInfo();

            string fileName = "财务报表";
            if (model.StartTime != DateTime.MinValue) { fileName += "_" + model.StartTime.ToString("yyyy-MM-dd"); }
            if (model.EndTime != DateTime.MinValue) { fileName += "_" + model.EndTime.ToString("yyyy-MM-dd"); }
            var list = FinanceReportBLL.GetExportList(model);
            Company.Util.ExportHelper.Export<ExportFinanceModel>(list, this.Response, fileName);

            return View();

        }
        #endregion
    }
}