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
    public class AccountLoginFilter : ActionFilterAttribute
    {
        string[] OutPath = new string[] { "/Member/Login", "/Member/GetVerifyByMobile", "/Member/MemberLogin" };
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var context = HttpContext.Current;
            var path = context.Request.Url.AbsolutePath;
            var member = CookieOperate.AccountCookie;
            if (member == null)
            {
                filterContext.Result = new RedirectResult("/WebSite/Login?redirtUrl=" + HttpUtility.UrlEncode(context.Request.Url.ToString()));
                //var action = filterContext.Controller.GetType().GetMethods().FirstOrDefault();

                //if (OutPath.Contains(path)) { return; }
                ////不使用微信账号
                //if (ConfigSetting.IsUseWeChat == "0")
                //{
                //    filterContext.Result = new RedirectResult("/Member/Login?redirtUrl=" + HttpUtility.UrlEncode(context.Request.Url.ToString()));
                //    return;
                //}

                //WeChatCookie wechat = new WeChatCookie();

                //#region 获取公司信息
                //var cid = CommonMethod.GetString(HttpContext.Current.Request.RequestContext.RouteData.Values["cid"]);

                //if (string.IsNullOrEmpty(cid))
                //{
                //    filterContext.Result = new RedirectResult("/Common/Error/1");
                //}
                //var company = CompanyInfoDBOperate.GetModelByCompanyName(cid);
                //if (company == null)
                //{
                //    filterContext.Result = new RedirectResult("/Common/Error/2");
                //}
                //wechat.CompanyInfo = company;
                //CookieOperate.WeChatCookie = wechat;
                //#endregion

                ////非微信
                //if (!CommonMethod.IsWeChat(filterContext.HttpContext.Request)) 
                //{
                //    return;
                //}

                //#region 微信环境

                //WeChatInfo wxInfo = WeChatInfoDBOperate.GetWeChatInfoByProjectName(cid);
                //WXModel wxModel = new WXModel() { AppId = wxInfo.AppId,    AppSercet = wxInfo.AppSecret };
                //WXAPI wxApi = new WXAPI(wxModel);
                //string code = CommonMethod.GetString(context.Request["code"]);
                //if (string.IsNullOrEmpty(code))
                //{
                //    string url = wxApi.GetUrlByAppId(context.Request.UserHostAddress, context.Request.Path);
                //    filterContext.Result = new RedirectResult(url);
                //    return;
                //}
                //AccessToken token = wxApi.GetTokenFromCode(code);
                //WXUser user = wxApi.GetUserInfoByWeb(token.access_token, token.openid);
                //wechat.WXUser = user;
                //wechat.WeChat = wxModel;
                //#endregion


            }
        }
    }
}