using Company.DAL.Data;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Company.Admin.Fliter
{
    public class AdminLoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var context = HttpContext.Current;
            var path = context.Request.Url.AbsolutePath.ToLower();
            var user = CookieOperate.MemberCookie;
            string[] outPath = new string[] { "/admin/login", "/admin/adminlogin", "/admin/nopermission","/admin/welcome" };
              //非登录模块 登录限制
            if (!outPath.Contains(path))
            {
                //Cookie不存在
                if (user == null)
                {
                    filterContext.Result = new RedirectResult("/admin/login");
                    return;
                }
                else
                {
                    var mypath = "index";
                    if (path != "/")
                        mypath = context.Request.Url.AbsolutePath.Split('/')[2];
                    else
                        path = "/admin/index";

                        var action = filterContext.Controller.GetType().GetMethods().FirstOrDefault(
                        a => a.ReturnType.Name == "ActionResult" && a.Name.ToLower() == mypath.ToLower() );
                    if (path.IndexOf("admin/index") < 0 && path.IndexOf("admin/menulist") < 0 && action != null)
                    {
                        var flag = UserMenuDBOperate.HasPerMission(user.DepartId, path);
                        if (!flag)
                        {
                            filterContext.Result = new RedirectResult("/admin/NoPerMission");
                            return;
                        }
                    }
                }
            }
        }
    }
}