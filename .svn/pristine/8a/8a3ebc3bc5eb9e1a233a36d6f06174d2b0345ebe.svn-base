$(function () {
    MyFont.SetFont();
})
var MyUrl = {
    GetFinanceConfig:"/member/GetFinanceConfig",
    ReimAdd: "/member/AddAuditMain",
    GetMemberInfo: "/Member/GetMemberInfo",
    GetMyAuditMain: "/Member/GetMyAuditMain",
    GetAuditMainDetail: "/Member/GetAuditMainById",
};
var MemberInfo = function () {
    var member = null;
    //if (!localStorage.MemberInfo) {
        $.ajax({
            type: "post",
            url: MyUrl.GetMemberInfo,
            dataType: "json",
            success: function (data) {
                if (data.IsSuccess) {
                    // v.User = data.Data;
                    member = data.Data;
                    console.log(data.Data);
                    localStorage.MemberInfo = member;
                }
            }
        });
    //}
    //else {

    //}
    return member;
};
var MyFont = {
    //判断是否是中文
    IsChinese: function () {
        var lang = (navigator.systemLanguage ? navigator.systemLanguage : navigator.language);
        var lang = lang.substr(0, 2);//获取lang字符串的前两位
        return lang.indexOf('zh') > -1;
    },
    //当前语言
    Language: function () {
        var language = this.IsChinese() ? 'cn' : 'en';
       return language;
        // return 'en';
    },
    SetFont: function () {
        var $this = this;
        var language = $this.Language();
       // Loading.Show();
        $("[lang]").each(function () {
            var type = $(this).attr("type");//类型 如果是text就是placeholder,其他的则是text
            var lang = $(this).attr("lang");//获取 语言类型
            if (fonts[lang] == undefined) { return; }
            var name = fonts[lang][language];
            $(this).attr("placeholder", name).text(name).val(name);
        });
       // Loading.Hide();
    },
    GetFont: function (font) {
        var $this = this;
        var language = $this.Language();
        if (fonts[font] == undefined) { return ""; }
        return fonts[font][language];
    },
    AlertMobile: function (font) {
        var $this = this;
        layer.open({
            content: font
           , btn: $this.GetFont('LangConfirm')
        });
    },
    AlertMobileSuccess: function (font, Success) {
        var $this = this;
        layer.open({
            content: font
           , btn: $this.GetFont('LangConfirm'),
            yes: function (index) {
               Success(index);
           }
        });
    },
    AlertMobileConfirm: function (font, Success) {
        var $this = this;
        layer.open({
            content: font
           , btn: [$this.GetFont("LangConfirm"), $this.GetFont("LangCancel")]
           , skin: 'footer'
           , yes: function (index) {
               Success(index);
           }
        });
    },
};
var fonts = {
    LangSave: { cn: "保存", en: "Save" },
    LangCancel: { cn: "取消", en: "Cancel" },
    LangConfirm: { cn: "确认", en: "Confrim" },
    LangYes: { cn: "确定", en: "Yes" },
    LangNo: { cn: "否", en: "No" },  
    /*登陆页面*/
    GetCode:{cn:'获取验证码',en:'Get Code'},
    YourCode: { cn: "你的验证码", en: 'Your Code' },
    HasSend: { cn: '验证码已发送', en: 'Code HasSend' },
    Mobile: { cn: '手机号', en: 'Mobile' },
    MobileError: { cn: '手机号格式错误', en: 'Mobile Error' },
    Code: { cn: '验证码', en: 'Code' },
    CodeError: { cn: '验证码错误', en: 'Code Error' },
    Login: { cn: "登录", en: "Login" },
    Logining: { cn: "登录中...", en: "Logining..." },
    MemberMobileNotExist: { cn: "用户手机号不存在,请联系管理员", en: 'MemberMobile Not Exist,Please Contact Manager' }
}