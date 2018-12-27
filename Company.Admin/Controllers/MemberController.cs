using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Company.Util;
using Company.Admin.Fliter;
using Company.Dto.Model;
using Company.BLL.Data;
using Company.DAL.Data;
using Company.Dto;
using Company.DAL;
using Company.Dto.Enum;
using Newtonsoft.Json;
using Company.Dto.Data;

namespace Company.Admin.Controllers
{
    [AccountLoginFilter]
    public class MemberController : BaseController
    {
        #region 路由
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public ActionResult Index(string cid)
        {
            ViewBag.Name = cid;
            return Redirect("/Content/Member/reim.html" + Request.Url.Query);
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login() 
        {
            return View();
        }
        /// <summary>
        /// 审核页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditIndex() 
        {
         
            return View();
        }
        #endregion

        #region 接口
        public JsonResult GetVerifyByMobile(string Mobile) 
        {
            ResultInfo info = new ResultInfo();
            info = MemberBussiness.GetVerifyByMobile(Mobile);
            return Json(info);
        }
        public JsonResult MemberLogin(string Mobile, string Code) 
        {
            ResultInfo info = new ResultInfo();
            info = MemberBussiness.LoginByMobile(Mobile, Code);
            return Json(info);
        }
        //public JsonResult GetMemberInfo() 
        //{
        //    var user = CookieOperate.MemberCookie;
        //    ResultInfo<Dto.CompanyUser> result = new ResultInfo<Dto.CompanyUser>();
        //    if (user != null) { result.IsSuccess = true; result.Data = user; }
        //    return Json(result);
        //}
        public JsonResult MemberList() 
        {
            var list = CompanyUserDBOperate.GetComapnyUserList(CookieOperate.MemberCookie.CompanyId);
            ResultInfo<List<CompanyUser>> result = new ResultInfo<List<CompanyUser>>() { IsSuccess = true };
            result.Data = list;
            return Json(result);
        }

        #region 审核
        /// <summary>
        /// 发起审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddAuditMain(AuditMain model) 
        {
            ResultInfo info = new ResultInfo();
            string message = "";
            if (model.Id == 0)
            {
                model.DepartId = CookieOperate.MemberCookie.DepartId;
                model.CompanyId = CookieOperate.MemberCookie.CompanyId;
                model.UserId = CookieOperate.MemberCookie.Id;
                model.Status = 1;
                model.OperateId = CookieOperate.MemberCookie.Id;
                model.MainId = Guid.NewGuid().ToString().Replace("-", "");
                info.IsSuccess = AuditBussiness.AddAudit(model,out message);
                info.Code = message;
            }
            else { 
                
            }
            return Json(info);
        }

        /// <summary>
        /// 获取我发起的审核
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyAuditMain() 
        {
            ResultInfo<List<AuditMain>> info = new ResultInfo<List<AuditMain>>(){ IsSuccess = true };
            info.Data = AuditMainDBOperate.GetMyAuditMain(CookieOperate.MemberCookie.Id);
            return Json(info);
        }
        public JsonResult GetAuditMainById(int id)
        {
            string message = "";
            ResultInfo<AuditMain> info = new ResultInfo<AuditMain>() { IsSuccess = true };
            info.Data = AuditBussiness.GetAuditMainById(id, out message);
            if (message == "MainNull") { info.IsSuccess = false; info.Code = message; }
            return Json(info);
        }
        public JsonResult GetFinanceConfig() 
        {
            ResultInfo<List<PickerSelect>> info = new ResultInfo<List<PickerSelect>>() { IsSuccess = false };
            info.Data = FinanceConfigDBOperate.GetConfigDataByDepartId(CookieOperate.MemberCookie.DepartId);
            if (info.Data != null && info.Data.Count > 0) { info.IsSuccess = true; }
            return Json(info);
        }
        #endregion

        #endregion

        #region 个人中心
        public ActionResult Center() {
            var account = CookieOperate.AccountCookie;
            if (account.Role == 1) {
               var model =  CustomerCompanyDBOperate.GetCompanyByMember(account.AccountId);
                ViewBag.Company = model;
            }
            //ViewBag.Account = ;

            return View();
        }
        public ActionResult Welcome() {
            return View();
        }
        public ActionResult ProjectManage() {
            return View();
        }
        public ActionResult ProjectManageAdd() {
            return View();
        }
        public ActionResult MemberInfo() {
            return View();
        }
        public ActionResult CompanyInfo()
        {
            return View();
        }
        #endregion

        #region 个人用户信息
        public JsonResult GetMemberInfo()
        {
            var info = new ResultInfo<CustomerMember>();
            var account = CookieOperate.AccountCookie;
            var id=  account.AccountId;
            var data = CustomerMemberDBOperate.GetModelById(id);
            info.IsSuccess = true;
            info.Data = data;
            return Json(info);
        }
        public JsonResult SaveMemberInfo(CustomerMember member) {
            var info = new ResultInfo();
            var account = CookieOperate.AccountCookie;
            var id = account.AccountId;
            var data = CustomerMemberDBOperate.GetModelById(id);
            if (member.Sex != "") { data.Sex = member.Sex; }
            if (member.Name != "") { data.Name = member.Name; account.AccountName = member.Name; }
            if (member.Job != "") { data.Job = member.Job; }
            if (member.Email != "") { data.Email = member.Email; account.Email = member.Email; }
            if (member.Mobile != "") { data.Mobile = member.Mobile; account.Mobile = member.Mobile; }
           info.IsSuccess =  CustomerMemberDBOperate.ModifyCustomerMember(data);
            AccountDBOperate.ModifyAccount(account);
            if (info.IsSuccess) { info.Message = "修改成功"; }
            else {
                info.Message = "修改失败";
            }
            return Json(info);
        }
        public JsonResult GetCompanyInfo()
        {
            var info = new ResultInfo<CustomerCompany>();
            var account = CookieOperate.AccountCookie;
            var data = CustomerCompanyDBOperate.GetCompanyByMember(account.AccountId);
            info.IsSuccess = true;
            info.Data = data;
            return Json(info);
        }
        public JsonResult SaveCompanyInfo(CustomerCompany model) {
            var info = new ResultInfo();
            var account = CookieOperate.AccountCookie;
            var data = CustomerCompanyDBOperate.GetCompanyByMember(account.AccountId);
            if (model.Name != "") { data.Name = model.Name; }
            if (model.Tax != "") { data.Tax = model.Tax; }
            if (model.BankName != "") { data.BankName = model.BankName; }
            if (model.BankNo != "") { data.BankNo = model.BankNo; }
            if (model.License != "") { data.License = model.License; }
            if (model.ContactName != "") { data.ContactName = model.ContactName; }
            if (model.ContactMobile != "") { data.ContactMobile = model.ContactMobile; }
            if (model.Address != "") { data.Address = model.Address; }
            account.AccountName = model.Name;
            info.IsSuccess = CustomerCompanyDBOperate.ModifyCustomerCompany(data);
            CookieOperate.AccountCookie = account;
            if (info.IsSuccess) { info.Message = "修改成功"; }
            else
            {
                info.Message = "修改失败";
            }
            return Json(info);
        }
        #endregion

        #region 项目管理
        public JsonResult ProjectManageAddModel(ProjectManage model) {
            ResultInfo info = new ResultInfo();
            var flag = false;
            var user = CookieOperate.AccountCookie;
            var cusMember = CustomerMemberDBOperate.GetModelById(user.AccountId);
            model.CusCompanyId = cusMember.CusCompanyId;
            model.FullName = model.Name;
            model.EndTime = DateTime.MaxValue;
            model.CusMemberId = user.AccountId;
            model.PayWay = ProjectPayWayEnum.UnKnown.ToString();
            model.Status = ProjectStatusEnum.NotStart.ToString();
            model.CreateBy = user.Mobile;
            model.CreateDate = DateTime.Now;
            model.RelStartTime = model.StartTime;
            model.RelEndTime = model.EndTime;
            model.LastPayDate = model.EndTime;
            model.CompanyId = CommonMethod.GetInt(ConfigSetting.eeeYoooId);
            model.Source = SourceEnum.PC.ToString();

            if (model.Id <= 0)
            {
                flag = ProjectManageDBOperate.AddProjectManage(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            else
            {
                flag = ProjectManageDBOperate.ModifyProjectManage(model);
                info.IsSuccess = flag;
                if (flag) { info.Message = "保存成功"; }
            }
            return Json(info);
        }
        public JsonResult ProjectManageList(QueryBase query)
        {
            string payway = CommonMethod.GetString(Request["PayWay"]);
            string status = CommonMethod.GetString(Request["Status"]);
            query.CusMemberId = CookieOperate.AccountCookie.AccountId;
            var totalcount = 0;
            var list = ProjectManageDBOperate.GetPagerList(query, payway, status, out totalcount);
            var payways = CommonMethod.GetEnumItems<ProjectPayWayEnum>();
            var statuss = CommonMethod.GetEnumItems<ProjectStatusEnum>();
            var operate = CommonMethod.GetIntEnumItems<OpereateTypeEnum>();
            var dev = CommonMethod.GetIntEnumItems<DevTypeEnum>();
            list.ForEach(a =>
            {
                a.StatusTxt = statuss.FirstOrDefault(b => b.Key == a.Status).Value;
                a.PayWayTxt = payways.FirstOrDefault(b => b.Key == a.PayWay).Value;
                var o = operate.FirstOrDefault(b => b.Key == a.OpereateType);
                if (!string.IsNullOrEmpty( o.Value)) { a.OpereateTypeTxt = o.Value; }
                var d = operate.FirstOrDefault(b => b.Key == a.DevType);
                if (!string.IsNullOrEmpty(d.Value)) { a.DevTypeTxt = d.Value; }
            });
            PagerList<ProjectManage> pagerList = new PagerList<ProjectManage>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        public JsonResult ProjectManageDelete(int id, int IsDeleted)
        {
            ResultInfo info = new ResultInfo();
            var flag = false;
          
            flag = ProjectManageDBOperate.DeleteProjectManage(new ProjectManage() { ModifyBy = CookieOperate.AccountCookie.Mobile,
                Id = id, IsDeleted = IsDeleted });
            if (flag) { info.Message = "删除成功"; info.IsSuccess = true; }
            return Json(info);
        }
        public JsonResult ProjectManageGetModel(int id) {
            ResultInfo<ProjectManage> info = new ResultInfo<ProjectManage>();
            var data = ProjectManageDBOperate.GetModelById(id);
            info.Data = data;
            return Json(info);
        }
        #endregion

        #region 修改密码
        public ActionResult ModifyPwd() {

            return View();
        }
        public JsonResult ModifyAccountPwd()
        {
            ResultInfo info = new ResultInfo();
            var flag = false;

            string old = CommonMethod.GetString(Request["OldPwd"]);
            string newPwd = CommonMethod.GetString(Request["NewPwd"]);
            if (CookieOperate.AccountCookie.Password != CommonMethod.PasswordMD5(old))
            {
                info.Message = "旧密码错误";
            }
            var account = CookieOperate.AccountCookie;
            account.Password = CommonMethod.PasswordMD5(newPwd);
            flag = AccountDBOperate.ModifyAccount(account);
            if (flag) {
                CookieOperate.AccountCookie = null;
                info.Message = "修改成功,请重新登录"; info.IsSuccess = true; }
            return Json(info);
        }
        #endregion

        #region 工单
        public ActionResult WorkListInfo() {
            int pid = CommonMethod.GetInt(Request["pid"]);
            var projectName = "";
            if (pid != 0)
            {
                var model = ProjectManageDBOperate.GetModelById(pid);
                if (model != null)
                    projectName = "->[" + model.Name + "/" + model.FullName + "]";
            }
            ViewBag.ProjectName = projectName;
            ViewBag.ProjectId = pid;
            var user = CookieOperate.AccountCookie;
            ViewBag.Project = ProjectManageDBOperate.GetList(0,user.AccountId).Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name + "-" + a.FullName });
            //ViewBag.Member = CustomerMemberDBOperate.GetList(user.CompanyId).Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name });
            //ViewBag.WorkListStatus = CommonMethod.GetEnumItems<WorkListStatusEnum>().Select(a => new SelectListItem() { Value = a.Key.ToString(), Text = a.Value });

            return View();
        }
        //主页请求的数据
        public JsonResult WorkListList(WorkListQuery query)
        {
            var totalcount = 0;
            query.CusMemberId = CookieOperate.AccountCookie.AccountId;
            var list = WorkListDBOperate.GetPagerList(query, out totalcount);
            PagerList<WorkList> pagerList = new PagerList<WorkList>();
            var status = CommonMethod.GetEnumItems<WorkListStatusEnum>();
            //var userList = CompanyUserDBOperate.GetUserByUserNames(string.Join(",", list.Select(a => a.Users)).Split(',').ToArray());
            var detailList = WorkListDetailDBOperate.GetList(list.Select(a => a.Guid).ToArray());

            list.ForEach(a =>
            {
                a.StatusTxt = status.FirstOrDefault(b => b.Key == a.Status).Value;
                a.StatusClass = CommonMethod.GetWorkListStatusClass(a.Status);
                a.DetailList = detailList.Where(b => b.WorkId == a.Guid).ToList();
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
        public ActionResult WorkListInfoAdd() {
            return View();
        }
        public JsonResult WorkListInfoEdit(string str,int projectId,string name,int id) {
            ResultInfo result = new ResultInfo();
            var account = CookieOperate.AccountCookie;
            var list = JsonConvert.DeserializeObject<List<WorkListEdit>>(str);
            var project = ProjectManageDBOperate.GetModelById(projectId);
            var cusMember = CustomerMemberDBOperate.GetModelById(account.AccountId);
            var workList = new WorkList() {
                ProjectId = projectId,
                ProjectName = project.Name,
                CompanyId = CommonMethod.GetInt( ConfigSetting.eeeYoooId),
                CompanyName = ConfigSetting.eeeYoooName,
                CusMemberId = account.AccountId,
                CusCompanyId = cusMember.CusCompanyId,
                Name= name,
                Id = id,
                Status = WorkListStatusEnum.NotStart.ToString(),
                Guid = Guid.NewGuid().ToString(),
                CreateBy = account.AccountName
            };
           var flag =   WorkListBLL.AddWorkList(workList, list);
            if (flag) { result.Message = "操作成功"; result.IsSuccess = true; }
            else {
                result.Message = "操作成功";
            }
            return Json(result);
        }
        public JsonResult GetWorkDetailById(int projectId) {
            var result = new ResultInfo<WorkList>();
            var model = WorkListDBOperate.GetModelById(projectId);
            var detail = WorkListDetailDBOperate.GetList(new string[] { model.Guid });
            var guid =  detail.Select(a => a.Guid).ToArray();
            var items = WorkListItemsDBOperate.GetList(guid);
            model.DetailList = detail;
            model.ItemList = items;
            result.Data = model;
            result.IsSuccess = true;
            return Json(result);
        }
        #endregion
    }
}