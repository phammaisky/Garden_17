function Report_Onload() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    //Loading
    $('#Loading_div').css('top', (window_Height - $('#Loading_div').height()) / 2 + 'px');
    $('#Loading_div').css('left', (window_Width - $('#Loading_div').width()) / 2 + 'px');

    Hide_Element('Loading_div');
    Display_Element('Dynamic_Control_Holder_td');

    if (Check_HDF('Is_POS_hdf')) {

        $("#Card_tbx").keyboard({
            layout: 'custom',
            customLayout: {
                'default': ['{a} {left} {right} {b}', '1 2 3', '4 5 6', '7 8 9', '0']
            },
            restrictInput: false,
            preventPaste: true,
            autoAccept: true
        });
    }

    $("input[type='text']").css('background-color', 'White');
}

function Report_btn_OnClientClick() {

    Refresh_Content(null);
}

function Refresh_Content(Report_Title) {

    Hide_Element('Dynamic_Control_Holder_td');
    Display_Element('Loading_div');

    if (Report_Title != null) {
        $('#Report_lbl').text(Report_Title);
    }
}

function Scroll_To_Top_img_OnClient_Click() {

    //Scroll_To_Element('Page_Content_div');

    try {
        $("html, body").animate({ scrollTop: 0 }, "slow");
    }
    catch (e) {
        window.scrollTo(0, 0);
    }
}