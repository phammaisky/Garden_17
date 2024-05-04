function Report_Onload() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    //Loading
    $('#Loading_div').css('top', (window_Height - $('#Loading_div').height()) / 2 + 'px');
    $('#Loading_div').css('left', (window_Width - $('#Loading_div').width()) / 2 + 'px');

    Hide_Element('Loading_div');
    Display_Element('Dynamic_Control_Holder_td');
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

function Delete_Banner(Banner_ID) {

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Delete_Banner(Banner_ID, Delete_Banner_Sucessfull, Delete_Banner_Error);

    //
    function Delete_Banner_Sucessfull(Response) {

        Hide_Element(Banner_ID + '_tr');
    }

    //
    function Delete_Banner_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Delete_Shop(Shop_ID) {

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Delete_Shop(Shop_ID, Delete_Shop_Sucessfull, Delete_Shop_Error);

    //
    function Delete_Shop_Sucessfull(Response) {

        Hide_Element(Shop_ID + '_tr');
    }

    //
    function Delete_Shop_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Delete_Shop_Index_1(Shop_Index_1_ID) {

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Delete_Shop_Index_1(Shop_Index_1_ID, Delete_Shop_Index_1_Sucessfull, Delete_Shop_Index_1_Error);

    //
    function Delete_Shop_Index_1_Sucessfull(Response) {

        Hide_Element(Shop_Index_1_ID + '_tr');
    }

    //
    function Delete_Shop_Index_1_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Delete_Shop_Index_2(Shop_Index_2_ID) {

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Delete_Shop_Index_2(Shop_Index_2_ID, Delete_Shop_Index_2_Sucessfull, Delete_Shop_Index_2_Error);

    //
    function Delete_Shop_Index_2_Sucessfull(Response) {

        Hide_Element(Shop_Index_2_ID + '_tr');
    }

    //
    function Delete_Shop_Index_2_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}
