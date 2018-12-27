$(function(){
  /*------------ 顶部导航显示 -----------*/
  $('.nav_list .nav_item').on('mouseover',function(){
    var left = $(this).position().left,
        width = $(this).width();
    $('.line').css({
      'left': left,
      'width': width
    });
  })

  $('.j_show_sideslip').on('click', function () {
    $('.sideslip').toggleClass('s_show');
  });
  $('.header_bottom .nav_list').on('mouseleave', function() {
    $('.line').css('width', 0);
  });

  $('.nav_list .s-open-box').hover(function(){
    var idx = $(this).index()-1;
    $('.nav_list .box_main').hide();
    $('.nav_list .box_main').eq(idx).show();
  })

  // $(".nav_list .j-open-box").mouseover(function(){
  //   $(this).addClass('active');
  //   if($('.suspend_box').css('display') =='none'){
  //       $(this).parents('.nav_list').siblings('.suspend_box').slideDown();
  //   } else {
  //     $(this).parents('.nav_list').siblings('.suspend_box').slideDown();
  //   }
  // });
  //
  // $(".header_bottom .j-open-box").mouseout(function(){
  //   $('.suspend_box').slideUp();
  // });


  /*------------ 邮件显示 -----------*/
  $(".m_suspend_btn .j-show-email").mouseover(function(){
    $(".bottom_btn_main").show();
  });
  $(".m_suspend_btn").mouseout(function(){
    $(".bottom_btn_main").hide();
  });

  /*------------ 语言选择 -----------*/
  $('.header_top .j-show-language').on({
    mouseover : function(){
      $('.down_drop_box').show();
    },
    mouseout : function(){
      $('.down_drop_box').hide();
    }
  })

  // 底部点击返回顶部
  $(".j-back-top").on('click',function(){
    var backTopNum = $('html,body').offset().top;
    $('body,html').animate({scrollTop:backTopNum},300);
  });
  //滚动出现top按钮
  $(window).on('scroll',function(){
    var topNowNum = $(window).scrollTop();
    if(topNowNum>500){
      $('.m_suspend_btn .top_btn').show();
    } else {
      $('.m_suspend_btn .top_btn').hide();
    }
  })


  /*------------ 点击导航滚动到相应位置 -----------*/
  $('.m_banner_img').on('click', '.nav_item', function() {
		var ele = $('.m_main_group .item_box').eq($(this).index());
		var topNum = $(ele).offset().top  - 59;
		// var topNum = document.querySelector(eleName).getBoundingClientRect().top;//点击两次出错
		// var topNum = document.querySelector(eleName).offsetTop-'85';

		$('html,body').animate({scrollTop:topNum});
	});

  function changeClass(ele, idx){
    ele.eq(idx).siblings().removeClass('active');
    ele.eq(idx).addClass('active');
  }

  var navLi = $(".m_banner_img .nav_item"),
      topNumArr = [];
  for(var i=0; i<navLi.length; i++) {
      var ele = $('.m_main_group .item_box').eq(i);
      var topNum = $(ele).offset().top - 60;
      topNumArr.push(topNum);
  }

  function navPart(){
    var navTop = 533,
        scrollH = $(document).scrollTop();
    if (scrollH > navTop) {
      $('.m_banner_img .nav_box').css({
        'position':'fixed',
        'top':'0'
      });
      $('.m_banner_img .nav_box').addClass('s-fixed');
    } else {
      $('.m_banner_img .nav_box').css({
        'position': 'absolute',
        'top': 'auto',
      });
      $('.m_banner_img .nav_box').removeClass('s-fixed');
    }

    var topNowNum = $(document).scrollTop();

    if(0 <= topNowNum && topNowNum < topNumArr[0]){
      $('.m_banner_img .nav_item').removeClass('active');
    } else if (topNumArr[0] <= topNowNum && topNowNum < topNumArr[1]) {
        changeClass(navLi, 0);
    } else if (topNumArr[1] <= topNowNum && topNowNum < topNumArr[2]) {
        changeClass(navLi, 1);
    } else if (topNumArr[2] <= topNowNum && topNowNum < topNumArr[3]) {
        changeClass(navLi, 2);
    } else if (!topNumArr[4] && topNumArr[3] <= topNowNum) {
      changeClass(navLi, 3);
    } else if (topNumArr[3] <= topNowNum && topNowNum < topNumArr[4]) {
        changeClass(navLi, 3);
    } else if (topNumArr[4] <= topNowNum && topNowNum < topNumArr[5]) {
        changeClass(navLi, 4);
    } else if (!topNumArr[5] && topNumArr[4] <= topNowNum) {
      changeClass(navLi, 4);
    } else if (topNumArr[5] <= topNowNum && topNowNum < topNumArr[6]) {
        changeClass(navLi, 5);
    } else if (topNumArr[6] <= topNowNum) {
        changeClass(navLi, 6);
    };
  }

  $(document).on('scroll', function(){
    navPart();
	});

  navPart();

  /*------------ 弹窗 -----------*/

  var showMsgIdx = 0;
  function showMsg(ele, msg, btnText, bgColor){
    showMsgIdx += 1;
    var showMsgBlk = "showMsgBlk" + showMsgIdx;
    var showMsgBtn = "showMsgBtn" + showMsgIdx;
    var html = '<div id="'+ showMsgBlk +'" style="display:none;position:fixed;z-index:999;top:30%;left:50%;transform:translateX(-50%);width:300px;padding:12px;border-radius:6px;background-color:#fff;box-shadow:0 0 5px;">'+
                 '<p style="font-size:16px;margin-top:20px;">'+ msg +'</p>' +
                 '<button id="'+ showMsgBtn +'" style="width:100%;height:40px;text-align:center;line-height:40px;border-radius:4px;border: none;color: #fff;font-size: 20px;margin-top: 20px;cursor:pointer;background-color:'+ bgColor + ';">' + btnText +'</button>'+
               '</div>';
    $('body').append(html);
    var MsgBlkId = '#' + showMsgBlk,
        MsgBtnId = '#' + showMsgBtn;
    $(ele).on('click',function(){
      $(MsgBlkId).show();
    })
    $(MsgBtnId).on('click',function(){
      $(MsgBlkId).hide();
    });
  }
  showMsg('.show_layer', 'Coming soon!', 'O K', '#f7e465');

  $('.show_code').on('click',function(){
    $('.layer_code').toggle();
  })

})
