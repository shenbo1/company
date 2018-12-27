$(function(){
  var haveNum = false;
  /*------------ 导航切换 -----------*/
  $('.j_top_nav').on('click', function () {
    $(this).addClass('s_cehcked').siblings().removeClass('s_cehcked');
    var idx = $(this).index();
    $('.nav_main_bottom .nav_bottom_list').eq(idx).show().siblings().hide();
    $('.page_main_box .page_main_item').eq(idx).show().siblings().hide();   
    if($('.nav_bottom_list2').css('display') == 'block' &&  haveNum == false){
      for(var i=0; i<navLi3.length; i++) {
        var ele = $('.page_main_item2 .item_box').eq(i);
        var topNum = $(ele).offset().top - 140;
        topNumArr3.push(topNum);
      }
      navPart2();      
      haveNum = true;
    }
  });

  function changeClass(ele, idx){
    ele.eq(idx).siblings().removeClass('active');
    ele.eq(idx).addClass('active');
  }
  /*------------ 点击导航滚动到相应位置1 -----------*/
  $('.m_banner_img .nav_bottom_list1').on('click', '.nav_bottom_item', function() {
    var ele = $('.page_main_item1 .item_box').eq($(this).index());
    var topNum = $(ele).offset().top  - 139;
    $('html,body').animate({scrollTop:topNum});
  });

  var navLi2 = $(".nav_bottom_list1 .nav_bottom_item"),
      topNumArr2 = [];
  for(var i=0; i<navLi2.length; i++) {
      var ele = $('.page_main_item1 .item_box').eq(i);
      
      var topNum = $(ele).offset().top - 140;
      
      topNumArr2.push(topNum);
  }

  function navPart(){
    var navTop = 533,
        scrollH = $(document).scrollTop();
    if (scrollH > navTop) {
      $('.m_banner_img .nav_box_bg').css({
        'position':'fixed',
        'top':'0'
      });
      $('.m_banner_img .nav_box_bg').addClass('s-fixed');
    } else {
      $('.m_banner_img .nav_box_bg').css({
        'position': 'absolute',
        'top': 'auto',
      });
      $('.m_banner_img .nav_box_bg').removeClass('s-fixed');
    }

    var topNowNum = $(document).scrollTop();

    if(0 <= topNowNum && topNowNum < topNumArr2[0]){
      $('.nav_bottom_list1 .nav_bottom_item').removeClass('active');
    } else if (topNumArr2[0] <= topNowNum && topNowNum < topNumArr2[1]) {
        changeClass(navLi2, 0);
    } else if (topNumArr2[1] <= topNowNum && topNowNum < topNumArr2[2]) {
        changeClass(navLi2, 1);
    } else if (topNumArr2[2] <= topNowNum && topNowNum < topNumArr2[3]) {
        changeClass(navLi2, 2);
    } else if (!topNumArr2[4] && topNumArr2[3] <= topNowNum) {
      changeClass(navLi2, 3);
    } else if (topNumArr2[3] <= topNowNum && topNowNum < topNumArr2[4]) {
        changeClass(navLi2, 3);
    } else if (topNumArr2[4] <= topNowNum && topNowNum < topNumArr2[5]) {
        changeClass(navLi2, 4);
    } else if (!topNumArr2[5] && topNumArr2[4] <= topNowNum) {
      changeClass(navLi2, 4);
    } else if (topNumArr2[5] <= topNowNum && topNowNum < topNumArr2[6]) {
        changeClass(navLi2, 5);
    } else if (topNumArr2[6] <= topNowNum) {
        changeClass(navLi2, 6);
    };
  }
  
  /*------------ 点击导航滚动到相应位置2 -----------*/

  $('.m_banner_img .nav_bottom_list2').on('click', '.nav_bottom_item', function() {
    var ele = $('.page_main_item2 .item_box').eq($(this).index());
    var topNum = $(ele).offset().top  - 139;
    $('html,body').animate({scrollTop:topNum});
  });

  function changeClass(ele, idx){
    ele.eq(idx).siblings().removeClass('active');
    ele.eq(idx).addClass('active');
  }

  var navLi3 = $(".nav_bottom_list2 .nav_bottom_item"),
      topNumArr3 = [];
  // for(var i=0; i<navLi3.length; i++) {
  //   var ele = $('.page_main_item2 .item_box').eq(i);
    
  //   var topNum = $(ele).offset().top - 140;
    
  //   topNumArr3.push(topNum);
  // }

  $('.j_top_nav2').on('click', function () {

  });

  function navPart2(){
    var navTop = 533,
        scrollH = $(document).scrollTop();
    if (scrollH > navTop) {
      $('.m_banner_img .nav_box_bg').css({
        'position':'fixed',
        'top':'0'
      });
      $('.m_banner_img .nav_box_bg').addClass('s-fixed');
    } else {
      $('.m_banner_img .nav_box_bg').css({
        'position': 'absolute',
        'top': 'auto',
      });
      $('.m_banner_img .nav_box_bg').removeClass('s-fixed');
    }

    var topNowNum = $(document).scrollTop();
    // console.log(topNowNum)
    // console.log(topNumArr3[0])
    // console.log(topNumArr2[0])    
    
    if(0 <= topNowNum && topNowNum < topNumArr3[0]){
      $('.nav_bottom_list2 .nav_bottom_item').removeClass('active');
    } else if (topNumArr3[0] <= topNowNum && topNowNum < topNumArr3[1]) {
        changeClass(navLi3, 0);
    } else if (topNumArr3[1] <= topNowNum && topNowNum < topNumArr3[2]) {
        changeClass(navLi3, 1);
    } else if (topNumArr3[2] <= topNowNum && topNowNum < topNumArr3[3]) {
        changeClass(navLi3, 2);
    } else if (!topNumArr3[4] && topNumArr3[3] <= topNowNum) {
      changeClass(navLi3, 3);
    } else if (topNumArr3[3] <= topNowNum && topNowNum < topNumArr3[4]) {
        changeClass(navLi3, 3);
    } else if (topNumArr3[4] <= topNowNum && topNowNum < topNumArr3[5]) {
        changeClass(navLi3, 4);
    } else if (!topNumArr3[5] && topNumArr3[4] <= topNowNum) {
      changeClass(navLi3, 4);
    } else if (topNumArr3[5] <= topNowNum && topNowNum < topNumArr3[6]) {
        changeClass(navLi3, 5);
    } else if (topNumArr3[6] <= topNowNum) {
        changeClass(navLi3, 6);
    };
  }


  $(document).on('scroll', function(){
    navPart();
    navPart2();
  });

  navPart();
  navPart2();
})
