$(function(){
  // swiper
  var mySwiper = new Swiper('.swiper-container', {
    direction: 'horizontal',
    loop: true,
    prevButton:'.swiper-button-prev',
    nextButton:'.swiper-button-next',
    pagination: '.swiper-pagination'
  });

  /*------------ Service Process效果 -----------*/
  $(".img_txt_list a").hover(function(){
    $(this).children('.icon_txt').find('img').attr('src', 'img/icon/clock_yellow.png');
  },function(){
    $(this).children('.icon_txt').find('img').attr('src', 'img/icon/clock.png');
  })


  /*------------ 合作商二维码显示 -----------*/
  $(".column .j-show-code").mouseover(function(){
    $(this).find(".item_code").stop().animate({right: '0'},300);
    $(this).find(".item_code").stop().animate({right: '0'},300);
  });

  $(".column .j-close-code").mouseout(function(){
    $(this).find(".item_code").stop().animate({right: '-100%'},300);
  });

  // banner轮播图
  $('.index_nav_box .item').click(function(){
    var _this=$(this);
    _this.addClass('s_checked').siblings().removeClass('s_checked');
    var int=_this.index();
    $('.swiper-box').find(".swiper-item").stop(false,true).fadeOut().eq(int).fadeIn();
  });

  $('.column_nav').on('click','.column_item', function () {
    $(this).addClass('s_checked').siblings().removeClass('s_checked');
    var idx = $(this).index();
    $('.column_nav_main .column_main').eq(idx).show().siblings().hide();
  });
});
