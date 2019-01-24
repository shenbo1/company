$(function () {
    $('.imgview').hover(function () {
        if ($(this).val() != null && $(this).val() != undefined && $(this).val() != '') {
            var html = '<div class="divimgview" style=" position: absolute;left: ' + ($(this).width()+50) + 'px; top: 0;z-index: 10;border: 1px solid #ddd;">';
            html += '<img src = "' + $(this).val()+'" ></div >';
            $(this).css('position', 'relative');
            $(this).parent().append(html);
        }

    }, function () {
        $(this).siblings('.divimgview').remove();
    });
})
function UploadImages() {
    this.UploadType = 1;
    this.MaxSize = 1024 * 5;
    this.MaxCount = 9;
    this.EditObj = $("#upload");//前端显示
    this.FileObj = $("#file_upload");//input 框
    this.ImagesClass = "img";
    this.ImageContainer = ".z_photo";//图片容器 需和input file 平级
    this.Html = function (url) {
        var $this = this;
        var html = '<div class="z_addImg"><img src="' + url + '" class="' + $this.ImagesClass + '" /></div>';
        return html;
    }
    this.Success = function (url) {
        var $this = this;
        $this.FileObj.siblings($this.ImageContainer).append($this.Html(url));
        console.log(url + '');
        $this.ClearImg();
    };
    this.ClearImg = function () {
        var imgs = this.FileFindImgs();
        $(imgs).unbind("click");
        $(imgs).bind("click", function (event) {
            event.stopPropagation();
            var now = $(this);
            MyFont.AlertMobileConfirm('您确定要删除此照片吗？', function (index) {
                $.post("/Upload/DelImage", { path: now.attr("src") }, function (json) {
                    if (json.IsSuccess) {
                        now.parent().remove();
                        layer.closeAll();
                    }
                });
            })
        });
    },
    this.SetImg = function (url) {
        var $this = this;
        var maxCount = $this.MaxCount;
        if (maxCount > 1) { url = url.substring(0, url.length - 1); }
        var urls = url.split('|');
        for (var i = 0; i < urls.length; i++) {
            $this.FileObj.siblings($this.ImageContainer).append($this.Html(urls[i]));
        }
    },
    this.GetImg = function () {
        var $this = this;
        var imgs = this.GetFiles();
        var imgurl = imgs.join('|');
        //var maxCount = $this.MaxCount;
        //if (maxCount > 1) { imgurl = imgurl + "|"; }
        console.log(imgurl);
        return imgurl;
    },
    this.GetLength = function () {
        var array = this.GetFiles();
        return array.length;
    },
    this.GetFiles = function () {
        var imgs = this.FileFindImgs();
        var srcArray = [];
        $(imgs).each(function () {
            srcArray.push($(this).attr("src"));
        });
        return srcArray;
    };
    this.FileFindImgs = function () {
        var $this = this;
        return $this.FileObj.siblings($this.ImageContainer).find('img');
    };
    this.Init = function () {
        var $this = this;
        var fileObj = $this.FileObj;
        //监听上传图片click
        $this.EditObj.bind('click', function () {
            fileObj.click();
        });
        // 监听file的change
        fileObj.bind("change", function () {
            var reader = new FileReader();
            reader.readAsDataURL(this.files[0]);
            var file = this.files[0];
            var size = file.size;
            var isture = true;
            reader.onloadstart = function () {
                if (size > 1024 * $this.MaxSize) {
                    MyFont.AlertMobile("最大尺寸为： " + $this.MaxSize + "KB");
                    isture = false;
                }
            };
            reader.onload = function () {
                if (!isture) { return; }
                if ($this.GetLength() >= $this.MaxCount) {//Type为2的时候，只能上传一张图片
                    MyFont.AlertMobile("最多上传" + $this.MaxCount + "张");
                    return;
                }
              
                if ($this.UploadType === 1) {
                    $.ajax({
                        type: "post",
                        data: { url: this.result },
                        url: "/Upload/UploadImage",
                        beforeSend: function () {
                            Loading.Show();
                        },
                        success: function (json) {
                            Loading.Hide();
                            if (json.IsSuccess) {
                                $this.Success(json.Message);
                                //$this.ClearImg();
                            } else {
                                MyFont.AlertMobile("请上传正确的图片格式")
                            }
                        },
                        error: function (data) {

                        }
                    });
                }
                else {
                    var formData = new FormData();
                    formData.append('file', file);
                     $.ajax({
                        type: "post",
                         data: formData,
                         url: "/Upload/UploadFile",
                         processData: false,
                         contentType: false,
                        beforeSend: function () {
                            Loading.Show();
                        },
                        success: function (json) {
                            Loading.Hide();
                            if (json.IsSuccess) {
                                $this.Success(json);
                                //$this.ClearImg();
                            } else {
                                MyFont.AlertMobile(json.Message)
                            }
                        },
                        error: function (data) {

                        }
                    });
                }
            }
        });
    }
}

function UploadFiles() {

}