using Company.DAL;
using Company.DAL.Data;
using Company.Dto.Model;
using Company.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Data
{
    public class MemberBussiness
    {
        #region 发送验证码
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public static ResultInfo GetVerifyByMobile(string Mobile) 
        {
            ResultInfo info = new ResultInfo();
            if (CommonMethod.CheckMobile(Mobile))
            {
                try
                {
                    string vf = CommonMethod.MobileVerify;
                    string message = "";

                    #region 是否是调试环境
                    if (ConfigSetting.IsOnline == "0")
                    {
                        info.IsSuccess = true;
                        info.Message = "发送成功";
                        info.Code = vf;
                        return info;
                    }
                    #endregion

                    if (CommonMethod.SendMessage(Mobile, vf, out message))
                    {
                        info.IsSuccess = true;
                        info.Message = "发送成功";
                       LogBussiness.AddLog("SendMessage", Mobile, "发送成功", LogLevel.Success);
                    }
                    else
                    {
                        LogBussiness.AddLog("SendMessage", Mobile, "发送失败:" + message, LogLevel.Success);
                    }
                }
                catch (Exception ex)
                {
                    LogBussiness.AddLog("SendMessage", Mobile, ex.Message, LogLevel.Error);
                }
            }
            else
            {
                info.Code = "MobileError";
                info.Message = "手机号错误";
            }
            return info;
        }
        #endregion

        #region 登录
        /// <summary>
        /// 通过手机号登录
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static ResultInfo LoginByMobile(string Mobile, string Code) 
        {
            ResultInfo info = new ResultInfo();
            if (!Code.Equals(CommonMethod.MobileVerify)) { info.Code = "CodeError"; return info; }
            var user = CompanyUserDBOperate.GetModelByMobile(Mobile);
            if (user == null) { info.Code = "MemberMobileNotExist"; return info; }
            var depart = CompanyDepartMentDBOperate.GetDepartMentById(user.DepartId);
            user.DepartName = depart.Name;
            CookieOperate.MemberCookie = user;
            info.IsSuccess = true;
            CommonMethod.MobileVerify = "";
            return info;
        }
        #endregion
    }
}
