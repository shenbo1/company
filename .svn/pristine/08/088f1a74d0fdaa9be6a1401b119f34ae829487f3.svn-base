//function imgChange(obj1, obj2) {
//  //获取点击的文本框
//  var file = document.getElementById("file");
//  //存放图片的父级元素
//  var imgContainer = document.getElementsByClassName(obj1)[0];
//  //获取的图片文件
//  var fileList = file.files;
//  console.log(fileList);
//  //文本框的父级元素
//  var input = document.getElementsByClassName(obj2)[0];
//  var imgArr = [];
  
//  //遍历获取到得图片文件
//  for (var i = 0; i < fileList.length; i++) {
//      var imgUrl = window.URL.createObjectURL(file.files[i]);
//      imgArr.push(imgUrl);
//      var img = document.createElement("img");
//      img.setAttribute("src", imgArr[i]);
//      var imgAdd = document.createElement("div");
//      imgAdd.setAttribute("class", "z_addImg");
//      imgAdd.appendChild(img);
//      imgContainer.appendChild(imgAdd);
//  };
//};
var upload = new UploadImages();
upload.Init();
$(function () {
    //console.log( MemberInfo());
    
  //删除图片
  //$('.new-reim').on('click','.z_addImg',function(){
  //    var t = this;
  //    MyFont.AlertMobileConfirm('您确定要删除此照片吗？', function (index) {
  //        layer.closeAll();
  //        t.remove();
  //    })
    //});
    NewReim.GetMemberInfo();
    NewReim.GetFinanceConfig();
  $("#submit").click(function () { NewReim.Add(); })
});

var NewReim = {
    GetMemberInfo: function () {
        $.ajax({
            type: "post",
            url: MyUrl.GetMemberInfo,
            dataType: "json",
            success: function (data) {
                if (data.IsSuccess) {
                  $("#depart").val(data.Data.DepartName);
                }
            }
        });
    },
    GetFinanceConfig: function () {
        Loading.Show();
        ajaxGet.post({}, MyUrl.GetFinanceConfig, function (json) {
            if (!json.IsSuccess) { MyFont.AlertMobile("报销类别出错,请联系管理员"); return; }
            var data = json.Data;
            $("#pickermock").val(data[0].text + ' ' + data[0].list[0].text)
            $("#ConfigId").val(data[0].list[0].id);
            
            //插件
            var picker = new Picker({
                data: [data, data[0].list],
                selectedIndex: [0, 0],
                title: '报销类别'
            });
            picker.on('picker.select', function (selectedVal, selectedIndex) {
                var text1 = data[selectedIndex[0]].text;
                var text2 = data[selectedIndex[0]].list[selectedIndex[1]].text;
                var short_text = text1 + ' ' + text2;
                var id = data[selectedIndex[0]].list[selectedIndex[1]].id;
                $("#pickermock").val(short_text);
                $("#ConfigId").val(id);
            });
            picker.on('picker.change', function (index, selectIndex) {
                if (index === 0) {
                    picker.refillColumn(1, data[selectIndex].list);
                }
                //当一列滚动停止的时候，会派发picker.change事件，同时会传递列序号index及滚动停止的位置selectIndex。
            });
            pickermock.addEventListener('click', function () {
                picker.show();
            });
            Loading.Hide();
        }, function (data) { ExceptionDeal(data); })
    },
    Add: function () {
        var Data = {};
        var Money = $("#Money").val();//
        Data.Money = Money;
        var ConfigId = $("#ConfigId").val();//
        Data.ConfigId = ConfigId;
        var Remark = $("#Remark").val();//
        Data.Remark = Remark;
        var Images = upload.GetImg();//$("#Images").val();//
        Data.Images = Images;
        var url = MyUrl.ReimAdd;
        if (Money == 0) { MyFont.AlertMobile("请填写报销金额"); return; }
        if (Remark == "") { MyFont.AlertMobile("请填写报销明细"); return; }
        if (Images == "") { MyFont.AlertMobile("请至少上传一张图片凭证"); return; }
        MyFont.AlertMobileConfirm('提交后无法修改,确认提交？', function (index) {
            layer.closeAll();
            Loading.Show();
            ajaxGet.post(Data, url, function (data) {
                Loading.Hide();
                if (data.IsSuccess) {
                    MyFont.AlertMobileSuccess("提交成功", function () {
                        window.location.href = "/Member/Index";
                    });
                }
                else {
                    if (data.Code == "StepNull") { MyFont.AlertMobile("无审核步骤,请联系管理员"); return; }
                }
                console.log(data);
            }, function (data) { ExceptionDeal(data); })

        })

      
    }
};