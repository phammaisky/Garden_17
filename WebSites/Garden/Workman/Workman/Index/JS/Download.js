function Download_Onload() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    //
    Resize_Page_Content_tbl_Download();

    //
    Hide_Loading_Parent();
}

function Resize_Page_Content_tbl_Download() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    $('#Page_Content_tbl').css('width', (window_Width) + 'px');
    $('#Page_Content_tbl').css('maxWidth', (window_Width) + 'px');
    $('#Page_Content_tbl').css('minWidth', (window_Width) + 'px');

    $('#Page_Content_tbl').css('height', (window_Height) + 'px');
    $('#Page_Content_tbl').css('maxHeight', (window_Height) + 'px');
    $('#Page_Content_tbl').css('minHeight', (window_Height) + 'px');
}