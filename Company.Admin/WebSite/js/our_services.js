$(function(){
  // $('.m_sm_nav .sm_nav_main').hover(function(){
  //   $(this).find('.icon_wrap').addClass('active');
  //   $(this).find('p').addClass('active');
  // },function(){
  //   $(this).find('.icon_wrap').removeClass('active');
  //   $(this).find('p').removeClass('active');
  // })

  $('.m_sm_nav li').on('click',function(){
    $('.sm_nav_main .icon_wrap').removeClass('active');
    $('.sm_nav_main p').removeClass('active');

    $(this).find('.icon_wrap').addClass('active');
    $(this).addClass('active').siblings().removeClass('active');
  })

  $('.m_sm_nav .sm_nav_main').on('click',function(){
    var left = $(this).parent('li').position().left+121.5,
        idx = $(this).parent('li').index();

    $('.sm_nav_triangle').css('left', left);
    $('.sm_nav_detail').hide();
    $('.sm_nav_detail').eq(idx).show();

  })
})
