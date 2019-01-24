using Company.BLL.Data;
using Company.DAL.Data;
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

        #region 上传文件
        public JsonResult UploadFile(string url) {
            var result = new ResultInfo<Dto.FileInfo>();
            var files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count <= 0) { result.IsSuccess = false; result.Message = "请上传文件";return Json(result); }
            var file = files[0];
            string fileExt = file.FileName.ToLower().Substring(file.FileName.LastIndexOf('.'));
            string fileTypes = ConfigSetting.FileType;//文件
            if (Array.IndexOf(fileTypes.Split(','), fileExt) == -1)
            {
                result.Message = string.Format("{0}文件类型不正确,允许的文件类型为：{1}", file.FileName,fileExt);
                return Json(result);
            }
            string fileCode = Guid.NewGuid().ToString();
            //先stream到bytes，以便后续复用
            var inputStream = file.InputStream;
            var streamBytes = Stream2Bytes(inputStream);//ProductUtil.
            string path = "/upload";
            //if (CookieOperate.MemberCookie != null) { userId = CookieOperate.MemberCookie.Id; path += "/" + userId; }
            string filename = "/" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExt;
            string smallPath = path + "/file";
            CommonMethod.FolderCreate(smallPath);
            //CommonMethod.FolderCreate(smallPath);
            string savePath = CommonMethod.GetMapPath(smallPath) + filename;
            BytesToFile(streamBytes, savePath);
              var fileModel = new Dto.FileInfo()
            {
                FileName = file.FileName,
                FilePath = smallPath+filename,
                CreateBy = CookieOperate.MemberCookie.Id.ToString(),
                Guid = fileCode,
                TypeCode = 1,
                FileSize = file.ContentLength
            };
            FileInfoDBOperate.AddFileInfo(fileModel);
            result.IsSuccess = true;
            result.Data = fileModel;
            return Json(result);
        }
        public static void BytesToFile(byte[] bytes, string filePath)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    ms.WriteTo(fs);
                }
            }
        }
        public static byte[] Stream2Bytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        #endregion
    }
}