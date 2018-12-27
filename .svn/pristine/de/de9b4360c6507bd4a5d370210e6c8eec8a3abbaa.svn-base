
var Path = {
    Login: "/Member/MemberLogin",
    GetVerify: "/Member/GetVerifyByMobile"
};
var LoginMain = {
    //发送短信
    SendVerify: function () {
        var $this = this;
        var Data = {};
        Data.Mobile = $("#Mobile").val();
        if (!checkMobile(Data.Mobile)) { MyFont.AlertMobile(MyFont.GetFont('MobileError')); return; }
        if (!$this.IsSendVerify) {
            $this.IsSendVerify = true;
            ajaxGet.post(Data, Path.GetVerify, function (data) {
                //发送成功
                if (data.IsSuccess) {
                    $this.ReloadTime(30, $("#GetVerify"));
                    if (data.Code != "0") { MyFont.AlertMobile(MyFont.GetFont('YourCode') + "：" + data.Code); }
                }
                else {
                    $this.IsSendVerify = false;
                    MyFont.AlertMobile(MyFont.GetFont(data.Code));
                }
            }, function (data) { ExceptionDeal(data); });
        } else {
            MyFont.AlertMobile(MyFont.GetFont('HasSend'));
        }
    },
    MemberLogin: function () {
        var $this = this;
        if ($this.IsLogin) { return; }
        var Data = {};
        Data.Mobile = $("#Mobile").val();
        if (!checkMobile(Data.Mobile)) { MyFont.AlertMobile(MyFont.GetFont('MobileError')); return; }
        Data.Code = $("#Code").val();
        if (Data.Code == "") { MyFont.AlertMobile(MyFont.GetFont('CodeError')); return; }
        $("#Login").val(MyFont.GetFont("Logining"));
        $this.IsLogin = true;
        ajaxGet.post(Data, Path.Login, function (data) {
            $("#Login").val(MyFont.GetFont("Login"));
            $this.IsLogin = false;
            //发送成功
            if (data.IsSuccess) {
                var url = GetQueryString("redirtUrl");
                if (isNullOrEmpty(url)) { url = "Index"; }
                window.location.href = url; 
            }
            else {
               
                MyFont.AlertMobile(MyFont.GetFont(data.Code));
            }
        }, function (data) { ExceptionDeal(data); });
    },
    ReloadTime: function (second, obj) {
        var $this = this;
        if (second <= 0) {
            $this.IsSendVerify = false;
            var txt = MyFont.GetFont('GetCode');
            obj.html(txt).val(txt);
        }
        else {
            var txt = second + "(s)";
            obj.html(txt).val(txt);
            setTimeout(function () { $this.ReloadTime(--second, obj); }, 1000);
        }
    },
    IsSendVerify: false,
    IsLogin:false
};
$(function () {
 
    $("#GetVerify").click(function () {
        LoginMain.SendVerify();
    })
    $("#Login").click(function () {
        LoginMain.MemberLogin();
    })
})