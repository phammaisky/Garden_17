function Home_Onload() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    //
    Resize_View_Frame_1();
    Hide_Friend_News_Index_1_li_BR();

    //
    $('.dropdown').hover(
        function () {

            $(this).children('.sub-menu').stop(true, true);
            $(this).children('.sub-menu').slideDown(200);
        },
        function () {

            $(this).children('.sub-menu').stop(true, true);
            $(this).children('.sub-menu').slideUp(200);
        }
    );

    //Loading
    $('#Loading_div').css('top', (window_Height - $('#Loading_div').height()) / 2 + 'px');
    $('#Loading_div').css('left', (window_Width - $('#Loading_div').width()) / 2 + 'px');

    Hide_Element('Loading_div');

    //
    window.onbeforeunload = function () {
        Home_On_Client_Refresh();
    }

    //
    Home_Onload_After();
}

function Home_Onload_After() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    //
    var UUID = Creat_UUID();
    $('#Page_ID_hdf').val(UUID);

    Set_Cookie('Last_Active_Page_ID', $('#Page_ID_hdf').val(), 1);

    //
    if (window_Width >= window_Height) {
        $('#Page_Orientation_hdf').val('landscape');
    }
    else {
        $('#Page_Orientation_hdf').val('portrait');
    }

    //
    $(window).focus(function () {
        $('#Window_Is_In_Focus_hdf').val('1');
        Set_Cookie('Last_Active_Page_ID', $('#Page_ID_hdf').val(), 1);
    });

    $(window).blur(function () {
        $('#Window_Is_In_Focus_hdf').val('');
    });

    //
    $(document).on('keydown', function (event) {

        if ((Get_Key_Pressed(event) == 116)) {
            Home_On_Client_Refresh();
        }
    });

    $(document).on('keyup', function (event) {
    });

    $(document).on('mousemove', function (event) {
    });

    //
    $(document).ready(function () {

        $(window).resize(function () {

            window_Height = $(window).innerHeight();
            window_Width = $(window).innerWidth();

            if (window_Width >= window_Height) {
                $('#Page_Orientation_hdf').val('landscape');
            }
            else {
                $('#Page_Orientation_hdf').val('portrait');
            }

            //
            Resize_View_Frame_1();
            Hide_Friend_News_Index_1_li_BR();

        }).trigger('resize');
    });

    //
    Setup_Tooltip();
    Config_Highslide();

    //
    $('input[type=text]').attr('autocomplete', 'off');
    $('input[type=text]').attr('autocorrect', 'off');
    $('input[type=text]').attr('autocapitalize', 'off');
    $('input[type=text]').attr('spellcheck', 'false');

    $('input[type=password]').attr('autocomplete', 'off');
    $('input[type=password]').attr('autocorrect', 'off');
    $('input[type=password]').attr('autocapitalize', 'off');
    $('input[type=password]').attr('spellcheck', 'false');

    //
    $('#Client_Refresh_hdf').val('');
}

function Resize_View_Frame_1() {

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

    //
    if (!Check_HDF('Page_Is_Loaded_First_Time_hdf')) {
        document.getElementById('Page_Content_ifr').src = 'About.aspx';
    }

    $('#Page_Is_Loaded_First_Time_hdf').val('1');
}

function Home_On_Client_Refresh() {

    if (!Check_HDF('NOT_Run_Home_On_Client_Refresh_hdf')) {

        //$(document).attr('title', '(^_^) Đợi Chút Xíu... Đang Tải Dữ Liệu...');
        Display_Element('Loading_div');

        $('#Client_Refresh_hdf').val('1');

        window.location.href = window.location.href;
    }
}

function Check_Is_Active_Page() {

    var Result = false;

    var Last_Active_Page_ID = Get_Cookie('Last_Active_Page_ID');

    if (!Check_Object_NOT_Null_Or_Empty(Last_Active_Page_ID)) {
        Set_Cookie('Last_Active_Page_ID', $('#Page_ID_hdf').val(), 1);
    }

    if (Get_Cookie('Last_Active_Page_ID') == $('#Page_ID_hdf').val()) {
        Result = true;
    }

    return Result;
}

function Hide_Friend_News_Index_1_li_BR() {

    /*
    var First_Friend_News_Index_1_li_offset_top = $('#First_Friend_News_Index_1_li').offset().top;

    $('#News_Index_1').find('li').each(function () {

    if ($(this).attr('class')) {
    if ($(this).attr('class') == 'dropdown') {

    $(this).show();

    if ($(this).offset().top != First_Friend_News_Index_1_li_offset_top) {
    $(this).hide();
    }
    }
    }
    });
    */
}

function Auto_Scroll_To_Content(Must_Scroll) {

    if ((Must_Scroll) || (Check_HDF('Auto_Scroll_To_Content_hdf'))) {
        Scroll_To_Element('View_Frame_1_Content_tab_div');
    }

    //
    $('#Auto_Scroll_To_Content_hdf').val('1');
}

function Scroll_To_Element(ID) {

    try {
        if (Check_Element_Is_Not_Null(ID)) {
            document.getElementById(ID).scrollIntoView();
        }
    }
    catch (e) {
    }
}

function Menu_On_Click(Menu_Title, URL) {

    if (Check_Object_NOT_Null_Or_Empty(URL)) {

        try {

            if (!Check_String_Contain(document.getElementById('Page_Content_ifr').src, 'About.aspx')) {
                document.getElementById('Page_Content_ifr').contentWindow.Refresh_Content("Loading data for Report: " + Menu_Title + "...");
            }
        }
        catch (e) {
        }

        document.getElementById('Page_Content_ifr').src = URL;
    }
    else {
        Alert_Message("This feature:'" + Menu_Title + "' is developing...<br/>Plz choice any feature have title is started with ' >> ' !");
    }
}

function Menu_Home_On_Click() {

    document.getElementById('Page_Content_ifr').src = 'About.aspx';
}