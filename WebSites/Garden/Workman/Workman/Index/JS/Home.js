function Home_Onload() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    //
    Resize_Page_Content_tbl();

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
    $('#Loading_Parent_div').css('top', (window_Height - $('#Loading_Parent_div').height()) / 2 + 'px');
    $('#Loading_Parent_div').css('left', (window_Width - $('#Loading_Parent_div').width()) / 2 + 'px');

    Hide_Element('Loading_Parent_div');

    //
    window.onbeforeunload = function () {
        Home_On_Client_Refresh();
    }

    //
    if (window_Width >= window_Height) {
        $('#Page_Orientation_hdf').val('landscape');
    }
    else {
        $('#Page_Orientation_hdf').val('portrait');
    }

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

            window_Height = Screen_Height();
            window_Width = Screen_Width();

            if (window_Width >= window_Height) {
                $('#Page_Orientation_hdf').val('landscape');
            }
            else {
                $('#Page_Orientation_hdf').val('portrait');
            }

            Resize_Page_Content_tbl();

        }).trigger('resize');
    });

    //
    Disable_TBX_Auto_Complete_All();

    //
    $('#Client_Refresh_hdf').val('');

    //
    setInterval(function () {

        var Slogan_css = $('#Slogan_lbl').attr('class');

        if (Slogan_css == "Slogan_White") {
            $('#Slogan_lbl').attr('class', 'Slogan_Yellow');
        }
        else {
            $('#Slogan_lbl').attr('class', 'Slogan_White');
        }

    }, 500);

    //
    var Platform = $("#Platform_hdf").val();

    if (Platform == 'Software') {

        //$('#Slogan_lbl').css('padding-left', '45px');
    }

    //
    //Read_Job_News();
    GetNewKpiReported();
}

function Resize_Page_Content_tbl() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    $('#Page_Content_tbl').css('width', (window_Width) + 'px');
    $('#Page_Content_tbl').css('maxWidth', (window_Width) + 'px');
    $('#Page_Content_tbl').css('minWidth', (window_Width) + 'px');

    $('#Page_Content_tbl').css('height', (window_Height) + 'px');
    $('#Page_Content_tbl').css('maxHeight', (window_Height) + 'px');
    $('#Page_Content_tbl').css('minHeight', (window_Height) + 'px');

    var Menu_tr_height = 55;

    $('#Menu_tr').css('height', (Menu_tr_height) + 'px');
    $('#Menu_tr').css('maxHeight', (Menu_tr_height) + 'px');
    $('#Menu_tr').css('minHeight', (Menu_tr_height) + 'px');

    $('#Page_Content_ifr').css('height', (window_Height - Menu_tr_height) + 'px');
    $('#Page_Content_ifr').css('maxHeight', (window_Height - Menu_tr_height) + 'px');
    $('#Page_Content_ifr').css('minHeight', (window_Height - Menu_tr_height) + 'px');

    //Always on F5
    //document.getElementById('Page_Content_ifr').src = 'Welcome.aspx?Platform=' + $("#Platform_hdf").val();

    if (!Check_HDF('Page_Is_Loaded_First_Time_hdf')) {
        document.getElementById('Page_Content_ifr').src = 'Welcome.aspx?Platform=' + $("#Platform_hdf").val();
    }

    $('#Page_Is_Loaded_First_Time_hdf').val('1');
}

function Home_On_Client_Refresh() {

    if (!Check_HDF('NOT_Run_Home_On_Client_Refresh_hdf')) {

        //$(document).attr('title', '(^_^) Đợi Chút Xíu... Đang Tải Dữ Liệu...');
        Display_Element('Loading_Parent_div');

        $('#Client_Refresh_hdf').val('1');

        window.location.href = window.location.href;
    }
}

function Menu_On_Click(ID, Menu_Title, URL) {

    if (Check_Element_Is_Not_Null("Menu_" + ID + "_div")) {

        $(".Menu_div").attr('class', 'Menu_div');
        $(".Menu_lbl").attr('class', 'Menu_lbl B White');

        $("#Menu_" + ID + "_div").attr('class', 'Menu_div Menu_div_Active');
        $("#Menu_" + ID + "_lbl").attr('class', 'Menu_lbl B Red');
    }

    if (!Check_String_Contain(URL, "/Kpi/UserInfo"))
        Display_Element('Loading_Parent_div');

    document.getElementById('Page_Content_ifr').src = URL;
}

function Menu_Home_On_Click() {

    $(".Menu_div").attr('class', 'Menu_div');
    $(".Menu_lbl").attr('class', 'Menu_lbl B White');

    //
    Display_Element('Loading_Parent_div');

    document.getElementById('Page_Content_ifr').src = 'Welcome.aspx?Platform=' + $("#Platform_hdf").val();
}

function Read_Job_News() {

    if (Check_HDF_Not_Empty('UserIdd_hdf')) {

        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Read_Job_News(Read_Job_News_Sucessfull, Read_Job_News_Error);
    }
    else {

        setTimeout(function () {
            Read_Job_News();
        }, 1500);
    }

    //
    function Read_Job_News_Sucessfull(Response) {

        //Job_Received
        var Job_Received_Count = 0;
        var Job_Received_Confirm_Change_Deadline_Count = 0;

        var Job_Received_Thank_Count = 0;
        var Job_Received_Redo_Count = 0;

        //Job_Sended
        var Job_Sended_Request_Change_Deadline_Count = 0;

        var Job_Sended_YES_To_Do_Count = 0;
        var Job_Sended_NO_To_Do_Count = 0;

        var Job_Sended_Report_Done_Count = 0;
        var Job_Sended_Redo_Report_Done_Count = 0;

        //
        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                for (var i = 0; i < JSON_Array.length; i++) {

                    Job_Received_Count = JSON_Array[i].Item_1;
                    Job_Received_Confirm_Change_Deadline_Count = JSON_Array[i].Item_2;

                    Job_Received_Thank_Count = JSON_Array[i].Item_3;
                    Job_Received_Redo_Count = JSON_Array[i].Item_4;

                    Job_Sended_Request_Change_Deadline_Count = JSON_Array[i].Item_5;

                    Job_Sended_YES_To_Do_Count = JSON_Array[i].Item_6;
                    Job_Sended_NO_To_Do_Count = JSON_Array[i].Item_7;

                    Job_Sended_Report_Done_Count = JSON_Array[i].Item_8;
                    Job_Sended_Redo_Report_Done_Count = JSON_Array[i].Item_9;
                }
            }
        }

        //
        Job_Received_Count = parseInt(Job_Received_Count);
        Job_Received_Confirm_Change_Deadline_Count = parseInt(Job_Received_Confirm_Change_Deadline_Count);

        Job_Received_Thank_Count = parseInt(Job_Received_Thank_Count);
        Job_Received_Redo_Count = parseInt(Job_Received_Redo_Count);

        //
        Job_Sended_Request_Change_Deadline_Count = parseInt(Job_Sended_Request_Change_Deadline_Count);

        Job_Sended_YES_To_Do_Count = parseInt(Job_Sended_YES_To_Do_Count);
        Job_Sended_NO_To_Do_Count = parseInt(Job_Sended_NO_To_Do_Count);

        Job_Sended_Report_Done_Count = parseInt(Job_Sended_Report_Done_Count);
        Job_Sended_Redo_Report_Done_Count = parseInt(Job_Sended_Redo_Report_Done_Count);

        //
        if (isNaN(Job_Received_Count)) {
            Job_Received_Count = 0;
        }

        if (isNaN(Job_Received_Confirm_Change_Deadline_Count)) {
            Job_Received_Confirm_Change_Deadline_Count = 0;
        }

        if (isNaN(Job_Received_Thank_Count)) {
            Job_Received_Thank_Count = 0;
        }

        if (isNaN(Job_Received_Redo_Count)) {
            Job_Received_Redo_Count = 0;
        }

        //
        if (isNaN(Job_Sended_Request_Change_Deadline_Count)) {
            Job_Sended_Request_Change_Deadline_Count = 0;
        }

        if (isNaN(Job_Sended_YES_To_Do_Count)) {
            Job_Sended_YES_To_Do_Count = 0;
        }

        if (isNaN(Job_Sended_NO_To_Do_Count)) {
            Job_Sended_NO_To_Do_Count = 0;
        }

        if (isNaN(Job_Sended_Report_Done_Count)) {
            Job_Sended_Report_Done_Count = 0;
        }

        if (isNaN(Job_Sended_Redo_Report_Done_Count)) {
            Job_Sended_Redo_Report_Done_Count = 0;
        }

        //
        var Job_Received_Total_Count = Job_Received_Count + Job_Received_Confirm_Change_Deadline_Count + Job_Received_Thank_Count + Job_Received_Redo_Count;

        var Job_Sended_Total_Count = Job_Sended_Request_Change_Deadline_Count + Job_Sended_YES_To_Do_Count + Job_Sended_NO_To_Do_Count + Job_Sended_Report_Done_Count + Job_Sended_Redo_Report_Done_Count;

        var Job_Sended_Report_Done_Count_All = Job_Sended_Report_Done_Count + Job_Sended_Redo_Report_Done_Count;

        var Job_News_Count = Job_Received_Count + Job_Received_Thank_Count + Job_Sended_Report_Done_Count_All;

        //
        var Job_News_List = Job_Received_Count + "#"

            + Job_Received_Thank_Count + "#"

            + Job_Sended_Report_Done_Count_All
        ;

        //
        var Platform = $("#Platform_hdf").val();

        if (Platform == 'Software') {

            if (Job_News_Count == 0) {
                alert("Message=0");
            }
            else {
                alert("Message=" + Job_News_Count + "Job_News_List=" + Job_News_List);
            }
        }

        //Menu
        if (Job_Received_Total_Count > 0) {
            $("#Menu_3_lbl").html("Received (" + Job_Received_Total_Count + ")");
        }
        else {
            $("#Menu_3_lbl").html("Received");
        }

        if (Job_Sended_Total_Count > 0) {
            $("#Menu_2_lbl").html("Sent (" + Job_Sended_Total_Count + ")");
        }
        else {
            $("#Menu_2_lbl").html("Sent");
        }

        //Page Title

        if (Job_News_Count > 0) {
            $(document).prop('title', '(' + Job_News_Count + ') Workman : Quản lý công việc tốt hơn !');
        }
        else {
            $(document).prop('title', 'Workman : Quản lý công việc tốt hơn !');
        }

        //Loopback
        setTimeout(function () {
            Read_Job_News();
        }, 1500);
    }

    //
    function Read_Job_News_Error(Response) {

        //
        setTimeout(function () {
            Read_Job_News();
        }, 1500);

        //
        if (Response != null) {

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function GetNewKpiReported() {

    if (Check_HDF_Not_Empty('UserIdd_hdf')) {

        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.GetNewKpiReported(GetNewKpiReported_Sucessfull, GetNewKpiReported_Error);
    }
    else {

        setTimeout(function () {
            GetNewKpiReported();
        }, 1500);
    }

    function GetNewKpiReported_Sucessfull(Response) {

        //alert(Response);

        var totalNew = parseInt(Response);

        if (isNaN(totalNew)) {
            totalNew = 0;
        }

        if (totalNew > 0) {
            $("#Menu_1_lbl").html("View All (" + totalNew + ")");
        }
        else {
            $("#Menu_1_lbl").html("View All");
        }

        if (totalNew > 0) {
            $(document).prop('title', '(' + totalNew + ') Workman : Quản lý công việc tốt hơn !');
        }
        else {
            $(document).prop('title', 'Workman : Quản lý công việc tốt hơn !');
        }

        setTimeout(function () {
            GetNewKpiReported();
        }, 1500);
    }

    function GetNewKpiReported_Error(Response) {

        setTimeout(function () {
            GetNewKpiReported();
        }, 1500);

        if (Response != null) {
            //Alert_Message_PageMethods_Error(Response);
            //Alert_Message_Parent("Lỗi hệ thống, vui lòng thử lại sau vài phút...");
        }
    }
}

function Blinking_Notify() {

    //
    setInterval(function () {

        var Blinking = false;

        if ($('.Notify').css('border') == '1px dashed red') {
            Blinking = true;
        }

        if (Blinking) {
            $('.Notify').css('border', '1px dashed white');
            $('.Notify_Blue').css('border', '1px dashed white');
        }
        else {
            $('.Notify').css('border', '1px dashed red');
            $('.Notify_Blue').css('border', '1px dashed blue');
        }

        //
        var Blinking_Text = false;

        if ($('.Notify_Text').css('color') == 'red') {
            Blinking_Text = true;
        }

        if (Blinking_Text) {
            $('.Notify_Text').css('color', 'black');
            $('.Notify_Text').css('font-weight', 'normal');

            $('.Notify_Text').css('border', '1px dashed white');
        }
        else {
            $('.Notify_Text').css('color', 'red');
            $('.Notify_Text').css('font-weight', 'bold');

            $('.Notify_Text').css('border', '1px dashed red');
        }

    }, 1500);
}

function Clear_Notify(Job_ID, Notify) {

    //Editing HDF
    $("#Editing_Job_ID_hdf").val(Job_ID);

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Clear_Notify(Job_ID, Notify, Clear_Notify_Sucessfull, Clear_Notify_Error);

    function Clear_Notify_Sucessfull(Response) {

        //alert(Response);

        if (Response == '1') {

            $("#Editing_Job_ID_hdf").val(0);

            //Received
            if (Notify == 'Confirm_Change_Deadline') {

                $("#" + Job_ID + "_Deadline_Date_tbx").removeClass('Notify_Text');
                $("#" + Job_ID + "_Deadline_Hour_tbx").removeClass('Notify_Text');
                $("#" + Job_ID + "_Deadline_Minute_tbx").removeClass('Notify_Text');
            }
            else
                if (Notify == 'Thank') {

                    $("#" + Job_ID + "_Thank_btn").removeClass('Notify');
                }
                else
                    if (Notify == 'Redo') {

                        $("#" + Job_ID + "_Redo_btn").removeClass('Notify');
                    }
                    else//Sended
                        if (Notify == 'YES_To_Do') {
                            $("#" + Job_ID + "_YES_To_Do_btn").removeClass('Notify');
                        }
                        else
                            if (Notify == 'NO_To_Do') {
                                $("#" + Job_ID + "_NO_To_Do_btn").removeClass('Notify');
                            }
        }
    }

    function Clear_Notify_Error(Response) {

        if (Response != null) {

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}