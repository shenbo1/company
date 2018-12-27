using Company.BLL.Data;
using Company.Dto.Model;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Company.Admin.Controllers
{
    public class UploadController : Controller
    {
        #region 上传图片
        public JsonResult UploadImage(string url) 
        {
            ResultInfo info = new ResultInfo();
            string startStr = "data:image/";
            string endStr = ";base64,";
            int startIndex = url.IndexOf(startStr);
            int endIndex = url.IndexOf(endStr);
            string type = url.Substring(startIndex + startStr.Length, endIndex - startIndex - startStr.Length);
            url = url.Substring(url.IndexOf("base64,")).Replace("base64,", "");
            int userId = 0;
            try
            {
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(url));
                Bitmap img = new Bitmap(stream);
                string path = "/upload";
                if (CookieOperate.MemberCookie != null) {userId =CookieOperate.MemberCookie.Id; path += "/" +userId;  }
                string filename = "/" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + type;
                string smallPath = path + "/small";
                path = path+ "/large";
                CommonMethod.FolderCreate(path);
                CommonMethod.FolderCreate(smallPath);
                path = path + filename;//大图
                smallPath = smallPath + filename;//缩略图
                ImageHelper.ZoomAuto(stream, CommonMethod.GetMapPath(path), 500, 500);
                ImageHelper.ZoomAuto(stream, CommonMethod.GetMapPath(smallPath), 100, 100);
                info.IsSuccess = true;
                info.Message = smallPath;
            }
            catch (Exception ex)
            {
                LogBussiness.AddLog("UploadError", userId + "", ex.Message, LogLevel.Error);
            }
            return Json(info);
        }
        #endregion

        #region 图片删除
        public JsonResult DelImage(string path) 
        {
            ResultInfo info = new ResultInfo();
            try
            {
                if (CommonMethod.FileExists(path))
                {
                    CommonMethod.FileDel(path);
                    path = path.Replace("small", "large");
                    CommonMethod.FileDel(path);
                    info.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                LogBussiness.AddLog("DelImage", path, ex.Message, LogLevel.Error);
            }
            return Json(info);
        }
        #endregion
    }
}