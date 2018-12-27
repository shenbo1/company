using Company.DAL.Data;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Company.Admin.Controllers
{
    public class WeChatController : Controller
    {
        // GET: WeChat
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult CreateCode(string userName)
        {
            string url = string.Format("http://company.eeeyooo.com/WeChat/BindWeChat?userName={0}", userName);
            url = string.Format(ConfigSetting.WeChatUrl, url);
            string file =  userName + ".jpg";
            file = CommonMethod.CreateCode(url, file);
            return Json(new { file = file }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindWeChat(string openid, string headUrl, string name, string userName)
        {
            var model = CompanyUserDBOperate.GetModelByName(userName);
            //model.IsBindWeChat = 1;
            model.WeChatOpenId = openid;
            model.WeChatHeadUrl = headUrl;
            model.WeChatNickName = name;
            
            //ViewBag.OpenId = openid;
            //ViewBag.headUrl = headUrl;
            //ViewBag.name = name;
            //ViewBag.userName = userName;
            return View(model);
        }
        public JsonResult Bind(string openid, string headUrl, string name, string userName) {
            var model = CompanyUserDBOperate.GetModelByName(userName);
            model.IsBindWeChat = 1;
            model.WeChatOpenId = openid;
            model.WeChatHeadUrl = headUrl;
            model.WeChatNickName = name;
            CompanyUserDBOperate.ModifyCompanyUser(model);
            return Json(new { code =1 });
        }
    }
}