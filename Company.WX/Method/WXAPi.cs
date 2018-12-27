using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.Log.Util;
using System.Web;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace Company.WX
{
    public class WXAPI
    {
        private WXModel WXModel = new WXModel();
        public WXAPI(WXModel wx)
        {
            this.WXModel = wx;
        }

        #region 获取AccessToken
        /// <summary>
        ///  获取AccessToken
        /// </summary>
        /// <returns></returns>
        public AccessToken GetAccessToken()
        {
            try
            {
                WXData data = new WXData();
                data.SetValue("grant_type", "client_credential");
                data.SetValue("appid", WXModel.AppId);
                data.SetValue("secret", WXModel.AppSercet);
                string url = "https://api.weixin.qq.com/cgi-bin/token?" + data.ToUrl();
                string result = HttpService.Get(url);
                return data.FromJson<AccessToken>(result);
            }
            catch (Exception ex)
            {

            }
            return new AccessToken();

        }
        #endregion

        #region 跳转微信链接 微信授权
        /// <summary>
        /// 跳转微信链接  微信授权
        /// </summary>
        /// <param name="_host">主机</param>
        /// <param name="_path">链接</param>
        /// <param name="_scope">snsapi_base snsapi_userinfo </param>
        /// <returns></returns>
        public string GetUrlByAppId(string host, string path, string _scope = "snsapi_userinfo")
        {
            //构造网页授权获取code的URL
            string _redirectUri = HttpUtility.UrlEncode("http://" + host + path);
            WXData data = new WXData();
            data.SetValue("appid", WXModel.AppId);
            data.SetValue("redirect_uri", _redirectUri);
            data.SetValue("response_type", "code");
            data.SetValue("scope", _scope);
            data.SetValue("state", "STATE" + "#wechat_redirect");
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
            Log.Debug("Page", "Will Redirect to URL : " + url);
            return url;
        }
        #endregion

        #region  通过code换取网页授权access_token和openid的返回数据
        /// <summary>
        /// 通过code换取网页授权access_token和openid的返回数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AccessToken GetTokenFromCode(string code)
        {
            AccessToken model = new AccessToken();
            try
            {
                //构造获取openid及access_token的url
                WXData data = new WXData();
                data.SetValue("appid", WXModel.AppId);
                data.SetValue("secret", WXModel.AppSercet);
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();

                //请求url以获取数据
                string result = HttpService.Get(url);
                model = data.FromJson<AccessToken>(result);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
            }
            return model;
        }
        #endregion 

        #region 获取用户信息 通过网页授权
        /// <summary>
        /// 获取用户信息 通过网页授权
        /// </summary>
        /// <param name="_access_token">网页临时accesstoken</param>
        /// <param name="_opendId">用户openid</param>
        /// <returns></returns>
        public WXUser GetUserInfoByWeb(string token, string openid)
        {
            WXUser model = new WXUser();
            try
            {
                WXData data = new WXData();
                data.SetValue("access_token", token);
                data.SetValue("openid", openid);
                data.SetValue("lang", "zh_CN");
                string url = "https://api.weixin.qq.com/sns/userinfo?" + data.ToUrl();
                string result = HttpService.Get(url);
                Log.Info("result", result);
                model = data.FromJson<WXUser>(result);
            }
            catch (Exception _ex)
            {
                Log.Error("GetUserInfo", _ex.Message);
            }
            return model;
        }
        #endregion
    }
}
