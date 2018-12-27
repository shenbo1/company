using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace Company.Util
{
    public class ConfigSetting
    {
        public static string DataConnection = ConfigurationManager.AppSettings["DataConnection"];
        public static string EeeYoocompany = ConfigurationManager.AppSettings["eeeYoooCompany"];
        public static string AESKey = ConfigurationManager.AppSettings["AESKey"];
        //是否使用微信 控制微信自动登录和获取微信信息
        public static string IsUseWeChat = ConfigurationManager.AppSettings["IsUseWeChat"];
        //是否线上 非线上控制短信验证码
        public static string IsOnline = ConfigurationManager.AppSettings["IsOnline"];

        public static string JuHeUrl = ConfigurationManager.AppSettings["JuHeUrl"];     
        public static string JuHeTplId = ConfigurationManager.AppSettings["JuHeTplId"];
        public static string JuHeKey = ConfigurationManager.AppSettings["JuHeKey"];


        public static string WeChatUrl = ConfigurationManager.AppSettings["WeChatUrl"];
        public static string DomainUrl = ConfigurationManager.AppSettings["DomainUrl"];

        public static string eeeYoooId = ConfigurationManager.AppSettings["eeeYoooId"];
        public static string eeeYoooName = ConfigurationManager.AppSettings["eeeYoooName"];

        public static string FirstRoute = ConfigurationManager.AppSettings["FirstRoute"];

    }
}


