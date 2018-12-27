using Company.Admin.Fliter;
using Company.DAL.Data;
using Company.Dto;
using Company.Dto.Model;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Company.Admin.Controllers
{
    public class WebSiteController : WebSiteFilter
    {
        // GET: WebSite
        public ActionResult Index()
        {
            //var account = CookieOperate.AccountCookie;
            //var name = "个人中心";
            //if (account != null) name = account.AccountName;
            //ViewBag.AccountName = name;
            return View();
        }

        public ActionResult Login()
        {
            //var account = CookieOperate.AccountCookie;
            //var name = "个人中心";
            //if (account != null) name = account.AccountName;
            //ViewBag.AccountName = name;
            return View();
        }
        public ActionResult Register()
        {
            //var account = CookieOperate.AccountCookie;
            //var name = "个人中心";
            //if (account != null) name = account.AccountName;
            //ViewBag.AccountName = name;
            return View();
        }

        #region eeeYooo服务
        /// <summary>
        /// 技术孵化
        /// </summary>
        /// <returns></returns>
        public ActionResult Techatch() {
            return View();
        }
        /// <summary>
        /// 外包开发
        /// </summary>
        /// <returns></returns>
        public ActionResult OutDev()
        {
            return View();
        }
        /// <summary>
        /// 成熟的项目运维
        /// </summary>
        /// <returns></returns>
        public ActionResult Peration()
        {
            return View();
        }
        /// <summary>
        /// 项目开发
        /// </summary>
        /// <returns></returns>
        public ActionResult Dev()
        {
            return View();
        }
        #endregion

        #region 开发模式
        /// <summary>
        /// 内场开发
        /// </summary>
        /// <returns></returns>
        public ActionResult InnerDev()
        {
            return View();
        }
        /// <summary>
        /// 外派开发
        /// </summary>
        /// <returns></returns>
        public ActionResult OutOtherDev()
        {
            return View();
        }
        /// <summary>
        /// 排期开发
        /// </summary>
        /// <returns></returns>
        public ActionResult PaiDev()
        {
            return View();
        }

        #endregion

        #region 计费模式
        /// <summary>
        /// 内场计费
        /// </summary>
        /// <returns></returns>
        public ActionResult InnerCharge()
        {
            return View();
        }
        /// <summary>
        /// 外场计费
        /// </summary>
        /// <returns></returns>
        public ActionResult OutCharge()
        {
            return View();
        }
        /// <summary>
        /// 排期计费
        /// </summary>
        /// <returns></returns>
        public ActionResult PaiCharge()
        {
            return View();
        }
        /// <summary>
        /// 硬件计费
        /// </summary>
        /// <returns></returns>
        public ActionResult HardCharge()
        {
            return View();
        }
        /// <summary>
        /// eeeYooo服务费
        /// </summary>
        /// <returns></returns>
        public ActionResult EeeyoooCharge()
        {
            return View();
        }
        /// <summary>
        /// 交付方式计费模式
        /// </summary>
        /// <returns></returns>
        public ActionResult PayCharge()
        {
            return View();
        }
        /// <summary>
        /// 源码计费模式
        /// </summary>
        /// <returns></returns>
        public ActionResult OutSourceCharge()
        {
            return View();
        }
        #endregion

        #region 开发流程
        /// <summary>
        /// 开发流程图
        /// </summary>
        /// <returns></returns>
        public ActionResult DevProcess()
        {
            return View();
        }
        /// <summary>
        /// 开发管理说明
        /// </summary>
        /// <returns></returns>
        public ActionResult DevManage()
        {
            return View();
        }
        /// <summary>
        /// 上线管理
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineManage()
        {
            return View();
        }
        #endregion

        #region 上线流程
        /// <summary>
        /// 上线方式
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineWay()
        {
            return View();
        }
        /// <summary>
        /// 上线说明
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineInfo()
        {
            return View();
        }
        #endregion

        #region 交付流程
        /// <summary>
        /// 交付方式
        /// </summary>
        /// <returns></returns>
        public ActionResult PayWay()
        {
            return View();
        }
        /// <summary>
        /// 交付说明
        /// </summary>
        /// <returns></returns>
        public ActionResult PayWayInfo()
        {
            return View();
        }
        #endregion

        #region 首次
        public ActionResult FirstSign() {
            ViewBag.Operate = CommonMethod.GetEnumItems<OpereateTypeEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            ViewBag.Dev = CommonMethod.GetEnumItems<DevTypeEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            ViewBag.CompanyStatus = CommonMethod.GetEnumItems<CompanyStatusEnum>().Select(a => new SelectListItem() { Value = a.Key, Text = a.Value });
            return View();
        }
        #endregion

        #region 查看新闻
        public ActionResult NewsInfoList()
        {
            ViewBag.Count = NewsInfoDBOperate.GetPublishCount();
            return View();
        }
        public JsonResult NewsInfoListPager(QueryBase query)
        {
            var totalcount = 0;
            var list = NewsInfoDBOperate.GetPagerList(query,true, out totalcount);
            PagerList<NewsInfo> pagerList = new PagerList<NewsInfo>();
            pagerList.rows = list;
            pagerList.total = totalcount;
            return Json(pagerList);
        }
        public ActionResult NewsInfo(int id) {
            return View();
        }
        public JsonResult NewDetail(int id) {
            var list = NewsInfoDBOperate.GetModelById(id);
            return Json(list);
        }
        #endregion

        #region GetJobList
        public JsonResult GetJobList() {
            var list = JobListDBOperate.GetPublishList();
            var res  = list.Where(a => a.IsParent == 0).ToList();
            foreach (var item in res)
            {
                var child = list.Where(a => a.IsParent == item.Id).ToList();
                if (child != null && child.Count() > 0) { item.List = child; } else {
                    item.List = new List<JobList>();
                }
            }
            return Json(res);
        }
        #endregion
    }
}