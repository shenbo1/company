using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Dto
{
    /// <summary>
    /// 备注:
    /// </summary>
    [Serializable]
    public class WeChatInfo
    {
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 微信公众号名称
        /// </summary>
        public string WechatPublicName { get; set; }

        /// <summary>
        /// 微信公众号AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 微信支付开通状态1已开通  2未开通
        /// </summary>
        public int WehatPaymentStatus { get; set; }

        /// <summary>
        /// 公众号注册的邮箱即微信公众号登录账号
        /// </summary>
        public string WechatRegisteredMailbox { get; set; }

        /// <summary>
        /// 微信公众号登录密码
        /// </summary>
        public string WechatLoginPassword { get; set; }

        /// <summary>
        /// 微信公众号类型   1服务号  2订阅号 3其他
        /// </summary>
        public int WechatPublicType { get; set; }

        /// <summary>
        /// 微信支付的类型  1 JSAPI 2 NATIVE 3 APP 4 MICROPAY
        /// </summary>
        public int WehatPaymentType { get; set; }

        /// <summary>
        /// 商户支付账号
        /// </summary>
        public string PaymentAccount { get; set; }

        /// <summary>
        /// 支付秘钥
        /// </summary>
        public string PaymentKey { get; set; }

        /// <summary>
        /// 支付平台登录账号
        /// </summary>
        public string PaymentPlatformAccount { get; set; }

        /// <summary>
        /// 支付平台登录密码
        /// </summary>
        public string PaymentPlatformPassword { get; set; }

        /// <summary>
        /// 微信二维码地址
        /// </summary>
        public string WeChatQrCodeUrl { get; set; }

        /// <summary>
        /// 微信Token
        /// </summary>
        public string WeChatToken { get; set; }

        /// <summary>
        /// Token有效期
        /// </summary>
        public DateTime TokenTime { get; set; }

        /// <summary>
        /// 公众号到期时间
        /// </summary>
        public DateTime WechatEndTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改资料时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeleteDate { get; set; }

        #endregion
    }
}
