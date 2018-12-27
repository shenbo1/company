$(function () {
    $('.reim-detail').on('click', '.j-open-mask', function () {
        var imgSrc = $(this).find('img').attr('src');
        imgSrc = imgSrc.replace('/small', '/large');
        $('.img-mask').find('img').attr('src', imgSrc);
        $('.img-mask').show();
    });
    $('.img-mask .mask').on('click', function () {
        $('.img-mask').hide();
    });
    v.GetMemberInfo(); //加载事件
    v.GetReimDetail();
})

var v = new Vue({
    el: "#main",
    data: {
        User: "",
        Detail: "",
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
        GetReimDetail: function () {
            Loading.Show()
            $.ajax({
                type: "post",
                url: MyUrl.GetAuditMainDetail,
                data: { id: GetQueryString("Id")},
                dataType: "json",
                success: function (data) {
                    if (data.IsSuccess) {
                        v.Detail = data.Data;
                        var images = data.Data.Images;
                        var myimage = images.split('|');

                        for (var i = 0; i < myimage.length; i++) {
                            var html = '<div class="img_box j-open-mask j-img-ratio">'
                            html += '<img class="cc" src="' + myimage[i] + '">'
                            html += '</div>';
                            $(".img_group").append(html);
                        }
                        Loading.Hide()
                    }
                }
            }); 
        }
    },
    filters: {
        FormatDate: function (value) {
            if (!isNullOrEmpty(value)) {
                return ChangeDt(value);
            }
        },
        FormatStatus: function (status, name) {
            var txt = name;
            if (status == 1) { txt = name + "审批中"; }
            else if (status == 2) { txt = '审批未通过'; }
            else if (status == 3) { txt = '审批通过'; }
            return txt;
        },
        StatusFilter: function (status, name) {
            if (isNullOrEmpty(status)) { return; }
            if (status == 1) {
                return "待" + name + "审批";
            }
            else if (status == 2) {
                return "审批被" + name + "驳回";
            }
            else {
                return "审批通过";
            }
        },
        DetailStatus: function (status, step) {
            if (step == 0) { return '发起了审核'; }
            var txt = "发起了审核";
            switch (status) {
                case -1: txt = "审批中"; break;
                case 1: txt = "审核通过"; break;
                case 2: txt = "审核未通过"; break;
            }
            return txt;
        }
    }
});
