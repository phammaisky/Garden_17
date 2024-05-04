function Insert_Card_Onload() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    //Loading
    $('#Loading_div').css('top', (window_Height - $('#Loading_div').height()) / 2 + 'px');
    $('#Loading_div').css('left', (window_Width - $('#Loading_div').width()) / 2 + 'px');

    Hide_Element('Loading_div');

    $('#All_tbl').height(window_Height);
    $('#All_tbl').width(window_Width);
}

function Insert_Card_btn_OnClientClick() {

    $('#Message_lbl').html('');

    Display_Element('Loading_div');
}