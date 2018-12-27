
var v = new Vue({
    el: "#main",
    data: {
        User: "",
        List: "",
        nothing: false
    },
    methods: {
        GetMemberInfo: function () {
            $.ajax({
                type: "post",
                url: MyUrl.GetMemberInfo,
                dataType: "json",
                success: function (data) {
                    if (data.IsSuccess) {
                        v.User = data.Data;
                    }
                }
            });
        },
        GetList: function () {
            Loading.Show();
            $.ajax({
                type: "post",
                url:MyUrl.GetMyAuditMain,
                dataType: "json",
                success: function (data) {
                    v.nothing = !data.IsSuccess;
                    if (data.IsSuccess) {                        
                        v.List = data.Data;
                        Loading.Hide();
                    }
                }
            });
            
        }
    },
    filters: {
        FormatDate: function (value) {
            return ChangeDt(value);
        },
        FormatStatus: function (status, name) {
            var txt = name;
            if (status == 1) { txt = name + "审批中"; }
            else if (status == 2) { txt = '审批未通过'; }
            else if (status == 3) { txt = '审批通过'; }
            return txt;
        }
    }
});
v.GetMemberInfo(); //加载事件
v.GetList();

