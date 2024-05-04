function Job_Sended_Onload() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    //
    Resize_Page_Content_tbl_Job_Sended();

    //
    Hide_Loading_Parent();

    //
    Blinking_Notify();
     
    Read_Job_Sended();
}

function Resize_Page_Content_tbl_Job_Sended() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    $('#Page_Content_tbl').css('width', (window_Width - 20) + 'px');
    $('#Page_Content_tbl').css('minWidth', (window_Width - 20) + 'px');

    $('#Page_Content_tbl').css('height', (window_Height) + 'px');
    $('#Page_Content_tbl').css('minHeight', (window_Height) + 'px');
}

function Read_Job_Sended() {

    //$('#Job_List_tbl tbody').empty();

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Read_Job_Sended(Read_Job_Sended_Sucessfull, Read_Job_Sended_Error);

    //
    function Read_Job_Sended_Sucessfull(Response) {

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                for (var i = 0; i < JSON_Array.length; i++) {

                    //
                    var Job_ID = JSON_Array[i].Item_1;
                    var Time = JSON_Array[i].Item_2;

                    //
                    var Sender_UserId = JSON_Array[i].Item_3;
                    var Sender_Name = JSON_Array[i].Item_4;
                    var Sender_Department = JSON_Array[i].Item_5;

                    var Receiver_UserId = JSON_Array[i].Item_6;
                    var Receiver_Name = JSON_Array[i].Item_7;
                    var Receiver_Department = JSON_Array[i].Item_8;

                    var Receiver_Department_All = JSON_Array[i].Item_9;

                    //
                    var Level = JSON_Array[i].Item_10;

                    var Deadline_Hour = JSON_Array[i].Item_11;
                    var Deadline_Minute = JSON_Array[i].Item_12;
                    var Deadline_Date = JSON_Array[i].Item_13;

                    var Deadline_Hour_Request = JSON_Array[i].Item_14;
                    var Deadline_Minute_Request = JSON_Array[i].Item_15;
                    var Deadline_Date_Request = JSON_Array[i].Item_16;

                    //
                    var Job_Type = JSON_Array[i].Item_17;
                    var Title = JSON_Array[i].Item_18;
                    var Content = JSON_Array[i].Item_19;

                    var Request_Change_Deadline = JSON_Array[i].Item_20;
                    var Confirm_Change_Deadline = JSON_Array[i].Item_21;

                    var YES_To_Do = JSON_Array[i].Item_22;
                    var NO_To_Do = JSON_Array[i].Item_23;

                    var Report_Done = JSON_Array[i].Item_24;

                    var Thank = JSON_Array[i].Item_25;
                    var Redo = JSON_Array[i].Item_26;

                    var Redo_Report_Done = JSON_Array[i].Item_27;

                    var Notify = JSON_Array[i].Item_28;

                    //
                    var Sender = Sender_Name + "<br/>" + Sender_Department;

                    var Receiver = "Cả phòng: " + Receiver_Department_All;

                    if (Receiver_UserId != '') {
                        Receiver = Receiver_Name + "<br/>" + Receiver_Department;
                    }

                    //Deadline_Date_td
                    var Deadline_Date_td = "<input id='" + Job_ID + "_Deadline_Date_tbx' name='Deadline_tbx' type='text' value='" + Deadline_Date + "' maxlength='10' class='TBX_Border_White' style='width: 70px; height: 20px; text-align: center;'>";

                    if ((Deadline_Date_Request != '') && (Deadline_Date_Request != Deadline_Date)) {
                        Deadline_Date_td = "<span class='U'>" + Deadline_Date + "</span><br/>"
                            + "<input id='" + Job_ID + "_Deadline_Date_tbx' name='Deadline_tbx' type='text' value='" + Deadline_Date_Request + "' maxlength='10' class='TBX_Border_White' style='width: 70px; height: 20px; color: red; text-align: center;'>";
                    }

                    //Deadline_Hour_td
                    var Deadline_Hour_td = "<input id='" + Job_ID + "_Deadline_Hour_tbx' name='Deadline_tbx' type='text' value='" + Deadline_Hour + "' maxlength='2' class='TBX_Border_White' style='width: 18px; height: 20px; text-align: right;'>h";

                    if ((Deadline_Hour_Request != '') && (Deadline_Hour_Request != Deadline_Hour)) {
                        Deadline_Hour_td = "<span class='U'>" + Deadline_Hour + " h</span><br/>"
                            + "<input id='" + Job_ID + "_Deadline_Hour_tbx' name='Deadline_tbx' type='text' value='" + Deadline_Hour_Request + "' maxlength='2' class='TBX_Border_White' style='width: 18px; height: 20px; color: red; text-align: right;'>h";
                    }

                    //Deadline_Minute_td
                    var Deadline_Minute_td = "<input id='" + Job_ID + "_Deadline_Minute_tbx' name='Deadline_tbx' type='text' value='" + Deadline_Minute + "' maxlength='2' class='TBX_Border_White' style='width: 18px; height: 20px; text-align: right;'>m";

                    if ((Deadline_Minute_Request != '') && (Deadline_Minute_Request != Deadline_Minute)) {
                        Deadline_Minute_td = "<span class='U'>" + Deadline_Minute + " m</span><br/>"
                            + "<input id='" + Job_ID + "_Deadline_Minute_tbx' name='Deadline_tbx' type='text' value='" + Deadline_Minute_Request + "' maxlength='2' class='TBX_Border_White' style='width: 18px; height: 20px; color: red; text-align: right;'>m";
                    }

                    //Job_Content_Full
                    var Job_Content_Full = '';

                    if (Job_Type != '') {
                        Job_Content_Full += Job_Type + '<br/><br/>';
                    }

                    //
                    Job_Content_Full += Title;

                    if (Content != '') {
                        Job_Content_Full += '<br/><br/>' + Content;
                    }

                    //
                    var Job_td =
                        "  <td align='center' valign='middle' style='height: 106px;'><img id='" + Job_ID + "_YES_To_Do_btn' class='Job_btn' src='/Index/IMG/Button/Yes.png'><span id='" + Job_ID + "_YES_To_Do_lbl' class='B Red'><br/>Đã được<br/>Đồng ý</span></td>"
                        + "<td align='center' valign='middle'><img id='" + Job_ID + "_NO_To_Do_btn' class='Job_btn' src='/Index/IMG/Button/No3.png'><span id='" + Job_ID + "_NO_To_Do_lbl' class='B Red'><br/>Đã bị<br/>Từ chối</span></td>"

                        + "<td align='center' valign='middle'><img id='" + Job_ID + "_Report_Done_btn' class='Job_btn' src='/Index/IMG/Button/Done3.png'><span id='" + Job_ID + "_Report_Done_lbl' class='B Red'><br/>Đã<br/>Làm xong</span></td>"

                        + "<td align='center' valign='middle'><img id='" + Job_ID + "_Thank_btn' class='Job_btn' src='/Index/IMG/Button/Thank.png' onclick='Thank(\"" + Job_ID + "\", \"" + Title + "\"); return false;'><span id='" + Job_ID + "_Thank_lbl' class='B Red'><br/>Đã<br/>Cảm ơn</span></td>"
                        + "<td align='center' valign='middle'><img id='" + Job_ID + "_Redo_btn' class='Job_btn' src='/Index/IMG/Button/Redo.png' onclick='Redo(\"" + Job_ID + "\", \"" + Title + "\"); return false;'><span id='" + Job_ID + "_Redo_lbl' class='B Red'><br/>Đã yêu cầu<br/>Làm lại</span></td>"

                        + "<td align='center' valign='middle' class='Job_td_body'><span class='Red'>" + Level + "</span><br/><br/><span class=''>" + Time.replace(" ", "<br/>") + "</span></td>"

                        + "<td align='center' valign='middle' class='Job_td_body' style='width: 72px; min-width: 72px; max-width: 72px;'>" + Deadline_Date_td + "</td>"
                        + "<td align='right' valign='middle' class='Job_td_body' style='width: 35px; min-width: 35px; max-width: 35px;'>" + Deadline_Hour_td + "</td>"
                        + "<td align='right' valign='middle' class='Job_td_body' style='width: 35px; min-width: 35px; max-width: 35px;'>" + Deadline_Minute_td + "</td>"

                        + "<td align='center' valign='middle' class='Job_td_body' style='width: 30px; min-width: 30px; max-width: 30px;'><img id='" + Job_ID + "_Confirm_Change_Deadline_Submit_btn' class='Confirm_Change_Deadline_Submit_btn' onclick='Confirm_Change_Deadline_Submit(\"" + Job_ID + "\", \"" + Title + "\"); return false;' src='/Index/IMG/Button/OK18.jpg' style='display: none;'><img id='" + Job_ID + "_Confirm_Change_Deadline_Loading_img' class='Confirm_Change_Deadline_Loading_img' src='/Index/IMG/Loading/Loading_Red_32.gif' style='display: none'></td>"
                        + "<td align='left' valign='middle' class='Job_td_body'>" + Job_Content_Full + "</td>"
                        + "<td align='center' valign='middle' class='Job_td_body'>" + Receiver + "</td>"
                    ;

                    //
                    Job_td = Replace_Index_Host(Job_td);

                    //
                    if (Check_Element_Is_Not_Null(Job_ID + "_tr")) {

                        if (Job_ID != $("#Editing_Job_ID_hdf").val()) {

                            $('#' + Job_ID + "_tr").html(Job_td);
                        }
                    }
                    else {

                        if ($('#Job_List_tbl tbody tr').length == 0) {
                            $('#Job_List_tbl tbody').append("<tr id='" + Job_ID + "_tr'>" + Job_td + "</tr>");
                        }
                        else {
                            $('#Job_List_tbl > tbody > tr:first').before("<tr id='" + Job_ID + "_tr'>" + Job_td + "</tr>");
                        }
                    }

                    //Have_Request_Change_Deadline
                    var Have_Request_Change_Deadline = false;

                    if ((Request_Change_Deadline == '1') && (Confirm_Change_Deadline != '1')) {
                        Have_Request_Change_Deadline = true;
                    }

                    //
                    if (Have_Request_Change_Deadline) {

                        if (Job_ID != $("#Editing_Job_ID_hdf").val()) {

                            $('#' + Job_ID + '_Confirm_Change_Deadline_Submit_btn').show();
                        }
                    }

                    //Notify
                    if (Notify == 'YES_To_Do') {
                        $("#" + Job_ID + "_YES_To_Do_btn").addClass('Notify');
                    }
                    else
                        if (Notify == 'NO_To_Do') {
                            $("#" + Job_ID + "_NO_To_Do_btn").addClass('Notify');
                        }
                        else
                            if (Notify == 'Report_Done') {
                                $("#" + Job_ID + "_Report_Done_btn").addClass('Notify_Blue');
                                $("#" + Job_ID + "_Thank_btn").addClass('Notify');
                                $("#" + Job_ID + "_Redo_btn").addClass('Notify');
                            }
                            else
                                if (Notify == 'Redo_Report_Done') {
                                    $("#" + Job_ID + "_Report_Done_btn").addClass('Notify_Blue');
                                    $("#" + Job_ID + "_Thank_btn").addClass('Notify');
                                    $("#" + Job_ID + "_Redo_btn").addClass('Notify');
                                }
                                else
                                    if (Notify == 'Request_Change_Deadline') {
                                        $("#" + Job_ID + "_Confirm_Change_Deadline_Submit_btn").addClass('Notify');
                                    }

                    //YES_To_Do
                    if ((YES_To_Do == '') && (NO_To_Do == '')) {

                        $("#" + Job_ID + "_YES_To_Do_btn").hide();
                        $("#" + Job_ID + "_YES_To_Do_lbl").hide();

                        $("#" + Job_ID + "_NO_To_Do_btn").hide();
                        $("#" + Job_ID + "_NO_To_Do_lbl").hide();

                        //
                        $("#" + Job_ID + "_Report_Done_btn").hide();
                        $("#" + Job_ID + "_Report_Done_lbl").hide();

                        $("#" + Job_ID + "_Thank_btn").hide();
                        $("#" + Job_ID + "_Thank_lbl").hide();

                        $("#" + Job_ID + "_Redo_btn").hide();
                        $("#" + Job_ID + "_Redo_lbl").hide();
                    }
                    else
                        if (YES_To_Do == '1') {

                            $("#" + Job_ID + "_YES_To_Do_btn").show();
                            $("#" + Job_ID + "_YES_To_Do_lbl").show();

                            if (Notify == 'YES_To_Do') {
                                $("#" + Job_ID + "_YES_To_Do_btn").attr("onclick", "Clear_Notify('" + Job_ID + "', '" + Notify + "'); return false;");
                            }
                            else {
                                $("#" + Job_ID + "_YES_To_Do_btn").addClass("Job_btn_No_Click");
                            }

                            $("#" + Job_ID + "_NO_To_Do_btn").hide();
                            $("#" + Job_ID + "_NO_To_Do_lbl").hide();

                            //Report_Done
                            if (Report_Done == '1') {

                                $("#" + Job_ID + "_Report_Done_btn").show();
                                $("#" + Job_ID + "_Report_Done_lbl").show();

                                if ((Thank == '') && (Redo == '')) {

                                    $("#" + Job_ID + "_Thank_btn").show();
                                    $("#" + Job_ID + "_Thank_lbl").hide();

                                    $("#" + Job_ID + "_Redo_btn").show();
                                    $("#" + Job_ID + "_Redo_lbl").hide();
                                }
                                else {
                                    if (Thank == '1') {

                                        $("#" + Job_ID + "_Thank_btn").show();
                                        $("#" + Job_ID + "_Thank_lbl").show();
                                        $("#" + Job_ID + "_Thank_btn").removeAttr("onclick");
                                        $("#" + Job_ID + "_Thank_btn").addClass("Job_btn_No_Click");

                                        $("#" + Job_ID + "_Redo_btn").hide();
                                        $("#" + Job_ID + "_Redo_lbl").hide();

                                    } else
                                        if (Redo == '1') {

                                        $("#" + Job_ID + "_Thank_btn").hide();
                                        $("#" + Job_ID + "_Thank_lbl").hide();

                                        $("#" + Job_ID + "_Redo_btn").show();
                                        $("#" + Job_ID + "_Redo_lbl").show();
                                        $("#" + Job_ID + "_Redo_btn").removeAttr("onclick");
                                        $("#" + Job_ID + "_Redo_btn").addClass("Job_btn_No_Click");
                                    }
                                }
                            }
                            else
                                if (Report_Done == '0') {

                                    if (Redo_Report_Done == '') {

                                        $("#" + Job_ID + "_Report_Done_btn").hide();
                                        $("#" + Job_ID + "_Report_Done_lbl").hide();

                                        $("#" + Job_ID + "_Thank_btn").hide();
                                        $("#" + Job_ID + "_Thank_lbl").hide();

                                        $("#" + Job_ID + "_Redo_btn").show();
                                        $("#" + Job_ID + "_Redo_lbl").show();
                                        $("#" + Job_ID + "_Redo_btn").removeAttr("onclick");
                                        $("#" + Job_ID + "_Redo_btn").addClass("Job_btn_No_Click");
                                    }
                                    else
                                        if (Redo_Report_Done == '1') {

                                            $("#" + Job_ID + "_Report_Done_btn").show();
                                            $("#" + Job_ID + "_Report_Done_lbl").show();
                                            $("#" + Job_ID + "_Report_Done_btn").addClass("Job_btn_No_Click");

                                            $("#" + Job_ID + "_Thank_btn").show();
                                            $("#" + Job_ID + "_Thank_lbl").hide();

                                            $("#" + Job_ID + "_Redo_btn").show();
                                            $("#" + Job_ID + "_Redo_lbl").hide();
                                        }
                                }
                                else {
                                    $("#" + Job_ID + "_Report_Done_btn").hide();
                                    $("#" + Job_ID + "_Report_Done_lbl").hide();

                                    $("#" + Job_ID + "_Thank_btn").hide();
                                    $("#" + Job_ID + "_Thank_lbl").hide();

                                    $("#" + Job_ID + "_Redo_btn").hide();
                                    $("#" + Job_ID + "_Redo_lbl").hide();
                                }
                        }
                        else
                            if (NO_To_Do == '1') {

                                $("#" + Job_ID + "_YES_To_Do_btn").hide();
                                $("#" + Job_ID + "_YES_To_Do_lbl").hide();

                                $("#" + Job_ID + "_NO_To_Do_btn").show();
                                $("#" + Job_ID + "_NO_To_Do_lbl").show();

                                if (Notify == 'NO_To_Do') {
                                    $("#" + Job_ID + "_NO_To_Do_btn").attr("onclick", "Clear_Notify('" + Job_ID + "', '" + Notify + "'); return false;");
                                }
                                else {
                                    $("#" + Job_ID + "_NO_To_Do_btn").addClass("Job_btn_No_Click");
                                }

                                //
                                $("#" + Job_ID + "_Report_Done_btn").hide();
                                $("#" + Job_ID + "_Report_Done_lbl").hide();

                                $("#" + Job_ID + "_Thank_btn").hide();
                                $("#" + Job_ID + "_Thank_lbl").hide();

                                $("#" + Job_ID + "_Redo_btn").hide();
                                $("#" + Job_ID + "_Redo_lbl").hide();
                            }
                }
            }
        }

        //
        if ($('#Job_List_tbl tbody tr').length == 0) {

            $('#Job_List_tbl').hide();

            $('#Message_lbl').show();
            $('#Message_lbl').html("Hiện tại chưa có Công việc / Kế hoạch nào...<br/>");
        }
        else {
            $('#Job_List_tbl').show();

            $('#Message_lbl').hide();
        }

        //
        Set_All_onclick_Mouse();

        //
        setTimeout(function () {
            Read_Job_Sended();
        }, 1500);
    }

    //
    function Read_Job_Sended_Error(Response) {

        //
        setTimeout(function () {
            Read_Job_Sended();
        }, 1500);

        //
        if (Response != null) {

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Confirm_Change_Deadline_Submit(Job_ID, Title) {

    Alert_Confirm_Parent("Đồng ý Deadline mới: " + Title + " ?", Confirm_Change_Deadline_Submit_RUN);

    //
    function Confirm_Change_Deadline_Submit_RUN(Confirm) {

        if (Confirm) {

            //ẩn BTN hiện tại
            $('#' + Job_ID + '_Confirm_Change_Deadline_Submit_btn').hide();
            $('#' + Job_ID + '_Confirm_Change_Deadline_Loading_img').show();

            //   
            PageMethods.set_path($('#PageMethods_Path_hdf').val());
            PageMethods.Confirm_Change_Deadline_Submit(Job_ID, Confirm_Change_Deadline_Submit_Sucessfull, Confirm_Change_Deadline_Submit_Error);
        }
    }

    function Confirm_Change_Deadline_Submit_Sucessfull(Response) {

        //Alert_Message_Parent(Response);

        if (Response == '1') {

            //Hiện Edit-BTN hiện tại
            $('#' + Job_ID + '_Confirm_Change_Deadline_Submit_btn').hide();
            $('#' + Job_ID + '_Confirm_Change_Deadline_Loading_img').hide();

            $("#Editing_Job_ID_hdf").val(0);
        }
        else {

            //Hiện OK-BTN hiện tại
            $('#' + Job_ID + '_Confirm_Change_Deadline_Submit_btn').show();
            $('#' + Job_ID + '_Confirm_Change_Deadline_Loading_img').hide();
        }
    }

    //
    function Confirm_Change_Deadline_Submit_Error(Response) {

        if (Response != null) {

            //Hiện OK-BTN hiện tại
            $('#' + Job_ID + '_Confirm_Change_Deadline_Submit_btn').show();
            $('#' + Job_ID + '_Confirm_Change_Deadline_Loading_img').hide();

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Thank(Job_ID, Title) {

    Alert_Confirm_Parent("Cảm ơn: " + Title + " ?", Thank_RUN);

    //
    function Thank_RUN(Confirm) {

        if (Confirm) {

            //Editing HDF
            $("#Editing_Job_ID_hdf").val(Job_ID);

            PageMethods.set_path($('#PageMethods_Path_hdf').val());
            PageMethods.Thank(Job_ID, Thank_Sucessfull, Thank_Error);
        }
    }

    function Thank_Sucessfull(Response) {

        //alert(Response);

        if (Response == '1') {

            $("#Editing_Job_ID_hdf").val(0);

            $("#" + Job_ID + "_Thank_btn").show();
            $("#" + Job_ID + "_Thank_lbl").show();
            $("#" + Job_ID + "_Thank_btn").removeAttr("onclick");
            $("#" + Job_ID + "_Thank_btn").attr("class", "Job_btn_No_Click");

            $("#" + Job_ID + "_Redo_btn").hide();
            $("#" + Job_ID + "_Redo_lbl").hide();
        }
    }

    function Thank_Error(Response) {

        if (Response != null) {

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Redo(Job_ID, Title) {

    Alert_Confirm_Parent("Yêu cầu làm lại: " + Title + " ?", Redo_RUN);

    //
    function Redo_RUN(Confirm) {

        if (Confirm) {

            //Editing HDF
            $("#Editing_Job_ID_hdf").val(Job_ID);

            PageMethods.set_path($('#PageMethods_Path_hdf').val());
            PageMethods.Redo(Job_ID, Redo_Sucessfull, Redo_Error);
        }
    }

    function Redo_Sucessfull(Response) {

        //alert(Response);

        if (Response == '1') {

            $("#Editing_Job_ID_hdf").val(0);

            $("#" + Job_ID + "_Thank_btn").hide();
            $("#" + Job_ID + "_Thank_lbl").hide();

            $("#" + Job_ID + "_Redo_btn").show();
            $("#" + Job_ID + "_Redo_lbl").show();
            $("#" + Job_ID + "_Redo_btn").removeAttr("onclick");
            $("#" + Job_ID + "_Redo_btn").attr("class", "Job_btn_No_Click");
        }
    }

    function Redo_Error(Response) {

        if (Response != null) {

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}