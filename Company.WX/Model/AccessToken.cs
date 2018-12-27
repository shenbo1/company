using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.WX
{
    public class AccessToken
    {
        public int errcode { get; set; }
        public string access_token { get; set; }
        public string errmsg { get; set; }
        public int expires_in { get; set; }
        public string openid { get; set; }
    }
    public class WXUser
    {
        public int errcode { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string headimgurl { get; set; }
        public string unionid { get; set; }
    }
}
