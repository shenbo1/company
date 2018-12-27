var Base = {
    setRem: function () {
        var html = document.getElementsByTagName('html')[0];
        var pageWidth = html.getBoundingClientRect().width;
        html.style.fontSize = pageWidth / 10 + "px";
    },
    t_img: null,
    isLoad: false,
    isImgLoad: function (ele) {
        var Base = this;
        var imgArr = $('.j-img-ratio'),
				imgLen = imgArr.length;

        imgArr.each(function () {
            var innerImg = $(this).find('img');
            var innerImgH = innerImg.height();
            if (innerImgH === 0) {
                Base.isLoad = false;
                return false;
            }
        });
        if (Base.isLoad) {
            clearTimeout(Base.t_img);
            imgArr.each(function () {
                var innerImg = $(this).find('img');
                if (innerImg) {
                    var w = $(this).width(),
							h = $(this).height();
                    var innerImgW = innerImg.width(),
							innerImgH = innerImg.height();
                    if (w / h >= innerImgW / innerImgH) {
                        innerImg.css({
                            'width': '100%',
                            'height': 'auto'
                        });
                    } else {
                        innerImg.css({
                            'width': 'auto',
                            'height': '100%'
                        });
                    };
                };
            });
        } else {
            Base.isLoad = true;
            Base.t_img = setTimeout(function () {
                Base.isImgLoad();
            }, 500);
        }
    },
}

Base.setRem();

$(function(){
  FastClick.attach(document.body);
	window.addEventListener('resize',function(){
		Base.setRem();
	});

	$('.title_logo').text('飞马旅');

	$('.j-back-page').on('click', function() {
		window.history.back(-1);location.reload();
	});

	/**********   动态计算图片比例  **********/
	Base.isImgLoad();
});
