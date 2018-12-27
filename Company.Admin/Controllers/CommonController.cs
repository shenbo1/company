using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Company.Util;

namespace Company.Admin.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/
        public ActionResult Error(int id)
        {
            ViewBag.ErrorMessage = CommonDictionary.ErrorDic[id];
            return View();
        }
	}
}