function Home_Onload() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    //
    Home_resize();

    ////
    //$(document).ready(function () {

    //    $(window).resize(function () {
    //        Home_resize();
    //    }).trigger('resize');
    //});

    $('#News_div').find('img').each(function () {
        $(this).css('maxWidth', '100%');
        $(this).css('height', 'auto');
        $(this).css('width', 'auto');
    });

    //
    Setup_jScrollPane_Vertical('Shop_List_div');
    Setup_jScrollPane_Vertical('Shop_Index_List_div');

    $('#Search_Shop_tbx').watermark('Tìm kiếm');
}