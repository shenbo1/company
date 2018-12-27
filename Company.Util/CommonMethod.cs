﻿using Company.WX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using ThoughtWorks.QRCode.Codec;

namespace Company.Util
{
    public class CommonMethod
    {
        public const string ProjectName = "eeeYooo";

        #region 数据转换
        /// <summary>
        /// int转换
        /// </summary>
        /// <param name="o">传入值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetInt(object o, int defaultValue)
        {
            int IntValue = defaultValue;
            if (o != null)
            {
                try
                {
                    IntValue = Convert.ToInt32(o);
                }
                catch (Exception ex)
                {

                }
            }

            return IntValue;
        }

        /// <summary>
        /// int转换
        /// </summary>
        /// <param name="o">传入值</param>
        /// <returns></returns>
        public static int GetInt(object o)
        {
            int IntValue = 0;
            if (o != null)
            {
                try
                {
                    IntValue = Convert.ToInt32(o);
                }
                catch (Exception ex)
                {

                }
            }

            return IntValue;
        }

        /// <summary>
        /// string转换
        /// </summary>
        /// <param name="o">传入值</param>
        /// <returns></returns>
        public static string GetString(object o)
        {
            string stringValue = string.Empty;
            if (o != null)
            {
                try
                {
                    stringValue = o.ToString();
                }
                catch (Exception ex)
                {

                }
            }
            return stringValue;
        }
        /// <summary>
        /// string转换
        /// </summary>
        /// <param name="o">传入值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetString(object o, string defaultValue)
        {
            string stringValue = defaultValue;
            if (o != null)
            {
                try
                {
                    stringValue = o.ToString();
                }
                catch (Exception ex)
                {

                }
            }
            return stringValue;
        }

        /// <summary>
        /// datetime转换
        /// </summary>
        /// <param name="o">传入值</param>
        /// <returns></returns>
        public static DateTime GetDateTime(object o)
        {
            DateTime dateTimeValue = DateTime.MinValue;
            if (o != null)
            {
                try
                {
                    dateTimeValue = Convert.ToDateTime(o);
                }
                catch (Exception ex)
                {
                }
            }
            return dateTimeValue;
        }

        /// <summary>
        /// datetime转换
        /// </summary>
        /// <param name="o">传入值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime GetDateTime(object o, DateTime defaultValue)
        {
            DateTime dateTimeValue = defaultValue;
            if (o != null)
            {
                try
                {
                    dateTimeValue = Convert.ToDateTime(o);
                }
                catch (Exception ex)
                {
                }
            }
            return dateTimeValue;
        }

        public static decimal GetDecimal(object o)
        {
            decimal Value = 0;
            if (o != null)
            {
                try
                {
                    Value = Convert.ToDecimal(o);
                }
                catch (Exception ex)
                {

                }
            }

            return Value;
        }

        /// <summary>
        /// 获取工单class
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetWorkListStatusClass(string val)
        {
            string sp = string.Empty;
            switch (val)
            {
                case "Start": sp = "label-warning"; break;
                case "NotStart": sp = "label-danger"; break;
                case "End": sp = "label-primary"; break;
            }
            return sp;
        }
        public static string GetItemStatus(string val) {
            string sp = string.Empty;
            switch (val)
            {
                case "HasPei": sp = "label-info"; break;
                case "InHand": sp = "label-warning"; break;
                case "HasYu": sp = "label-danger"; break;
                case "End": sp = "label-primary"; break;
                case "YuEnd": sp = "label-success"; break;
            }
            return sp;
        }
        public static string GetItemLevel(string val)
        {
            string sp = string.Empty;
            switch (val)
            {
                case "High": sp = "label-danger"; break;
                case "Normal": sp = "label-primary"; break;
            }
            return sp;
        }
        #endregion

        #region json 转换
        public static string ToJson(object t)
        {

            return new JavaScriptSerializer().Serialize(t);
        }

        public static T ScriptDeserialize<T>(string strJson)
        {
            var js = new JavaScriptSerializer();
            return js.Deserialize<T>(strJson);
        }

        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        public static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        #endregion

        #region AES加密
        /// <summary>

        /// AES加密算法

        /// </summary>

        /// <param name="plainText">明文字符串</param>

        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>

        public static string AESEncrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;
            //分组加密算法
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            // SymmetricAlgorithm rijndaelCipher = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组 
            //设置密钥及密钥向量
            rijndaelCipher.Key = Convert.FromBase64String(ConfigSetting.AESKey);//加解密双方约定好密钥：AESKey
            rijndaelCipher.GenerateIV();
            byte[] keyIv = rijndaelCipher.IV;
            byte[] cipherBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, rijndaelCipher.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();//得到加密后的字节数组
                    cs.Close();
                    ms.Close();
                }
            }
            return Convert.ToBase64String(keyIv.Concat(cipherBytes).ToArray()).Replace('+', '*').Replace('/', '-').Replace('=', '.'); ;
        }
        /// <summary>

        /// AES解密

        /// </summary>

        /// <param name="cipherString">密文字符串</param>

        /// <returns>返回解密后的明文字符串</returns>

        public static string AESDecrypt(string cipherString)
        {
            if (string.IsNullOrEmpty(cipherString))
                return string.Empty;
            cipherString = cipherString.Replace('.', '=').Replace('*', '+').Replace('-', '/');
            byte[] cipherText = Convert.FromBase64String(cipherString);
            int length = cipherText.Length;
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Convert.FromBase64String(ConfigSetting.AESKey);//加解密双方约定好的密钥
            byte[] iv = new byte[16];
            Buffer.BlockCopy(cipherText, 0, iv, 0, 16);
            des.IV = iv;
            byte[] decryptBytes = new byte[length - 16];
            byte[] passwdText = new byte[length - 16];
            Buffer.BlockCopy(cipherText, 16, passwdText, 0, length - 16);
            using (MemoryStream ms = new MemoryStream(passwdText))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");   ///将字符串后尾的'\0'去掉
        }
        #endregion

        #region 穿过代理服务器取远程用户真实IP地址
        /// <summary>
        /// 穿过代理服务器取远程用户真实IP地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIP
        {
            get
            {
                string result = String.Empty;
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (result != null && result != String.Empty)
                {
                    //可能有代理
                    if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。
                            result = result.Replace(" ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];    //找到不是内网的地址
                                }
                            }
                        }
                        else if (IsIPAddress(result)) //代理即是IP格式 ,IsIPAddress判断是否是IP的方法,
                            return result;
                        else
                            result = null;    //代理中的内容 非IP，取IP
                    }
                }
                string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.UserHostAddress;

                return result;
            }
        }
        #endregion

        #region 检查IP是否合法
        /// <summary>
        /// 检查IP是否合法
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string ip)
        {
            return Regex.IsMatch(ip, @"^(?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))");
        }
        #endregion

        #region 密码加密 2011
        public static string PasswordMD5(string Password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5").Substring(8, 16);
        }
        #endregion

        #region 判断是否是微信
        public static bool IsWeChat(HttpRequestBase request)
        {
            return request.UserAgent != null && request.UserAgent.ToLower().Contains("micromessenger");
        }
        #endregion

        #region 验证手机号
        public static bool CheckMobile(string _mobile)
        {
            return Regex.IsMatch(_mobile, @"^((0?1[34578]\d{9})|((0(10|2[1-3]|[3-9]\d{2}))?[1-9]\d{6,7}))$");
        }
        #endregion

        #region 验证码
        public static string MobileVerify
        {
            get
            {
                string verify = CookieGet("MobileVerify") + "";
                if (string.IsNullOrEmpty(verify))
                {
                    verify = new Random().Next(1000, 9999).ToString();
                    CookieSet("MobileVerify", verify, DateTime.Now.AddMinutes(30));
                }
                return verify;
            }
            set
            {
                CookieSet("MobileVerify", value, DateTime.Now.AddMinutes(-1));
            }
        }
        #endregion

        #region CookieSet
        public static void CookieSet(string key, string value, DateTime time)
        {
            HttpContext.Current.Request.Cookies.Remove(key);
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
            {
                cookie = new HttpCookie(key, CommonMethod.AESEncrypt(value));
                cookie.Expires = time;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Value = CommonMethod.AESDecrypt(cookie.Value);
                cookie.Expires = time;
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
        #endregion

        #region CookieGet
        public static string CookieGet(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                try
                {
                    return CommonMethod.AESDecrypt(cookie.Value);
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 发送
        public static bool SendMessage(string mobile,  string vf,out string message)
        {
            message = "";
            string url = ConfigSetting.JuHeUrl;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("mobile", mobile);
            dic.Add("tpl_id", ConfigSetting.JuHeTplId);
            dic.Add("tpl_value", HttpUtility.UrlEncode("#code#=" + vf + "", Encoding.UTF8));
            dic.Add("key", ConfigSetting.JuHeKey);
            string data = DataCreater(dic);
            data = HttpService.Post(data, url);
            message = data;
            return data.Contains("\"error_code\":0");
        }
        #endregion

        #region 参数
        public static string DataCreater(Dictionary<string, object> parameters)
        {
            string data = "";
            if (parameters.Count == 0)
            {
                return data;
            }
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                data += parameter.Key + "=" + parameter.Value.ToString() + "&";
            }
            data = data.Substring(0,data.Length - 1);
            return data;
        }
        #endregion

        #region 文件操作
        public static bool FolderCreate(string path)
        {
            path = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }
            return false;
        }
        public static string GetMapPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath(path);
        }
        public static bool FileExists(string path)
        {
            return File.Exists(GetMapPath(path));
        }
        public static void FileDel(string _path)
        {
            File.Delete(GetMapPath(_path));
        }
        #endregion

        #region 获取枚举类型
        /// <summary>
        /// 获取枚举类型
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnumItems<T>()
        {
            var result = new Dictionary<string, string>();
            Type t = typeof(T);
            Array arrays = Enum.GetValues(t);
            for (int i = 0; i < arrays.LongLength; i++)
            {
                object test = arrays.GetValue(i);
                FieldInfo fieldInfo = test.GetType().GetField(test.ToString());
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                DescriptionAttribute attrib = (DescriptionAttribute)attribArray[0];
                result.Add(test.ToString(), attrib.Description);
            }
            return result;
        }
        public static Dictionary<int, string> GetIntEnumItems<T>()
        {
            var result = new Dictionary<int, string>();
            Type t = typeof(T);
            Array arrays = Enum.GetValues(t);
            for (int i = 0; i < arrays.LongLength; i++)
            {
                object test = arrays.GetValue(i);
                FieldInfo fieldInfo = test.GetType().GetField(test.ToString());
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                DescriptionAttribute attrib = (DescriptionAttribute)attribArray[0];
                result.Add((int)test, attrib.Description);
            }
            return result;
        }
        public static string GetEnumDesc<T>(int value) {
            var result = new Dictionary<string, string>();
            Type t = typeof(T);
            Array arrays = Enum.GetValues(t);
            for (int i = 0; i < arrays.LongLength; i++)
            {
                object test = arrays.GetValue(i);
                if ((int)test == value) {
                    FieldInfo fieldInfo = test.GetType().GetField(test.ToString());
                    object[] attribArray = fieldInfo.GetCustomAttributes(false);
                    DescriptionAttribute attrib = (DescriptionAttribute)attribArray[0];
                    return attrib.Description;
                }
            }
            return string.Empty;
        }
        public static string GetEnumDesc<T>(string value)
        {
            var result = new Dictionary<string, string>();
            Type t = typeof(T);
            Array arrays = Enum.GetValues(t);
            for (int i = 0; i < arrays.LongLength; i++)
            {
                object test = arrays.GetValue(i);
                if (test.ToString() == value)
                {
                    FieldInfo fieldInfo = test.GetType().GetField(test.ToString());
                    object[] attribArray = fieldInfo.GetCustomAttributes(false);
                    DescriptionAttribute attrib = (DescriptionAttribute)attribArray[0];
                    return attrib.Description;
                }
            }
            return string.Empty;
        }
        #endregion

        public static string CreateCode(string strData, string fileName, string qrEncoding= "Byte", string level="M", int version=8, int scale=4) {
            string filePath = "/upload/code/";
            FolderCreate(filePath);
            filePath += fileName;
            if (FileExists(filePath)) return filePath;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            string encoding = qrEncoding;
            switch (encoding)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = scale;
            qrCodeEncoder.QRCodeVersion = version;
            switch (level)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }
            //文字生成图片
            Image image = qrCodeEncoder.Encode(strData);
            //string filename = DateTime.Now.ToString("yyyymmddhhmmssfff").ToString() + ".jpg";
            //string filepath = Server.MapPath(@"~\Upload") + "\\" + filename;
            //如果文件夹不存在，则创建
            //if (!Directory.Exists(filepath))
            //    Directory.CreateDirectory(filepath);
            System.IO.FileStream fs = new System.IO.FileStream(GetMapPath(filePath), System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();
            return filePath;
        }

    }
}
