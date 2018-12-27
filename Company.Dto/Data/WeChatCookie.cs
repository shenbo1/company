using Company.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Company.Dto
{
    public class WeChatCookie
    {
        public CompanyInfo CompanyInfo { get; set; }
        public WXModel WeChat { get; set; }
        public WXUser WXUser { get; set; }
    }
}
