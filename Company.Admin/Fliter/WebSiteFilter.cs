using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Company.Util;
using System.Web.Routing;
using Company.DAL.Data;
using Company.Dto;
using Company.WX;
using Company.DAL;

namespace Company.Admin.Fliter
{
    public class WebSiteFilter : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)//protected 只能被子类访问
        {
            base.OnActionExecuted(filterContext);
            var account = CookieOperate.AccountCookie;
            var name = "个人中心";
            if (account != null) name = account.AccountName;
            ViewBag.AccountName = name;
        }
    }
}