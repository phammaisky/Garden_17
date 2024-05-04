function Admin_Onload() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    $('#Page_Content_tbl').css('width', (window_Width) + 'px');
    $('#Page_Content_tbl').css('maxWidth', (window_Width) + 'px');
    $('#Page_Content_tbl').css('minWidth', (window_Width) + 'px');

    $('#Page_Content_tbl').css('height', (window_Height) + 'px');
    $('#Page_Content_tbl').css('maxHeight', (window_Height) + 'px');
    $('#Page_Content_tbl').css('minHeight', (window_Height) + 'px');

    $('#Menu_tr').css('height', (30) + 'px');
    $('#Menu_tr').css('maxHeight', (30) + 'px');
    $('#Menu_tr').css('minHeight', (30) + 'px');

    $('#Page_Content_ifr').css('height', (window_Height - 50) + 'px');
    $('#Page_Content_ifr').css('maxHeight', (window_Height - 50) + 'px');
    $('#Page_Content_ifr').css('minHeight', (window_Height - 50) + 'px');
}

function Menu_Admin_On_Click(URL) {

    document.getElementById('Page_Content_ifr').src = URL;
    return false;
}
