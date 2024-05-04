function Job_Received_Onload() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    //
    Resize_Page_Content_tbl_Job_Received();

    //
    Hide_Loading_Parent(); 

    //
    Blinking_Notify();

    Read_Job_Received();
}

function Resize_Page_Content_tbl_Job_Received() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    $('#Page_Content_tbl').css('width', (window_Width - 20) + 'px');
    $('#Page_Content_tbl').css('minWidth', (window_Width - 20) + 'px');

    $('#Page_Content_tbl').css('height', (window_Height) + 'px');
    $('#Page_Content_tbl').css('minHeight', (window_Height) + 'px');
}

function Read_Job_Received() {

    //$('#Job_List_tbl tbody').empty();

    var Job_Myself = Determine_Checked_RDOL('Job_Myself_rdol');

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Read_Job_Received(Job_Myself, Read_Job_Received_Sucessfull, Read_Job_Received_Error);

    //
    function Read_Job_Received_Sucessfull(Response) {

        Job_Myself = Determine_Checked_RDOL('Job_Myself_rdol');

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
                    var Job_Myself_Ask = JSON_Array[i].Item_29;

                    //
                    if (Job_Myself_Ask != Job_Myself) {

                        $('#Job_List_tbl tbody').empty();
                    }
                    else {
                        var Sender = Sender_Name + "<br/>" + Sender_Department;

                        var Receiver = "Cả phòng: " + Receiver_Department_All;

                        if (Receiver_UserId != '') {
                            Receiver = Receiver_Name;
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
                        var Report_Done_lbl = "Đã<br/>Báo cáo";

                        if (Sender_UserId == Receiver_UserId) {
                            Report_Done_lbl = "Đã<br/>Làm xong";
                        }

                        //
                        var Job_td =
                            "  <td align='center' valign='middle' class='NOT_Job_Myself_td' style='height: 106px;'><img id='" + Job_ID + "_YES_To_Do_btn' class='Job_btn' onclick='YES_or_NO_To_Do(\"" + Job_ID + "\", \"" + Title + "\", 1); return false;' src='/Index/IMG/Button/Yes.png'><span id='" + Job_ID + "_YES_To_Do_lbl' class='B Red'><br/>Đã<br/>Đồng ý</span></td>"
                            + "<td align='center' valign='middle' class='NOT_Job_Myself_td'><img id='" + Job_ID + "_NO_To_Do_btn' class='Job_btn' onclick='YES_or_NO_To_Do(\"" + Job_ID + "\", \"" + Title + "\", 0); return false;' src='/Index/IMG/Button/No3.png'><span id='" + Job_ID + "_NO_To_Do_lbl' class='B Red'><br/>Đã<br/>Từ chối</span></td>"

                            + "<td align='center' valign='middle'><img id='" + Job_ID + "_Report_Done_btn' class='Job_btn' onclick='Report_Done(\"" + Job_ID + "\", \"" + Title + "\", \"" + Redo + "\"); return false;' src='/Index/IMG/Button/Done3.png'><span id='" + Job_ID + "_Report_Done_lbl' class='B Red'><br/>" + Report_Done_lbl + "</span></td>"

                            + "<td align='center' valign='middle' class='NOT_Job_Myself_td'><img id='" + Job_ID + "_Thank_btn' class='Job_btn' src='/Index/IMG/Button/Thank.png'><span id='" + Job_ID + "_Thank_lbl' class='B Red'><br/>Được<br/>Cảm ơn</span></td>"
                            + "<td align='center' valign='middle' class='NOT_Job_Myself_td'><img id='" + Job_ID + "_Redo_btn' class='Job_btn' src='/Index/IMG/Button/Redo.png'><span id='" + Job_ID + "_Redo_lbl' class='B Red'><br/>Phải<br/>Làm lại</span></td>"

                            + "<td align='center' valign='middle' class='Job_td_body'><span class='Red'>" + Level + "</span><br/><br/><span class=''>" + Time.replace(" ", "<br/>") + "</span></td>"

                            + "<td align='center' valign='middle' class='Job_td_body' style='width: 72px; min-width: 72px; max-width: 72px;'>" + Deadline_Date_td + "</td>"
                            + "<td align='right' valign='middle' class='Job_td_body' style='width: 35px; min-width: 35px; max-width: 35px;'>" + Deadline_Hour_td + "</td>"
                            + "<td align='right' valign='middle' class='Job_td_body' style='width: 35px; min-width: 35px; max-width: 35px;'>" + Deadline_Minute_td + "</td>"

                            + "<td align='center' valign='middle' class='Job_td_body' style='width: 30px; min-width: 30px; max-width: 30px;'><img id='" + Job_ID + "_Request_Change_Deadline_Edit_btn' class='Request_Change_Deadline_Edit_btn' src='/Index/IMG/Button/Edit18.jpg' onclick='Request_Change_Deadline_Edit(\"" + Job_ID + "\"); return false;'><img id='" + Job_ID + "_Request_Change_Deadline_Submit_btn' class='Request_Change_Deadline_Submit_btn' onclick='Request_Change_Deadline_Submit(\"" + Job_ID + "\"); return false;' src='/Index/IMG/Button/OK18.jpg' style='display: none;'><span id='" + Job_ID + "_Request_Change_Deadline_Submit_BR' style='display: none;'><br/><br/></span><img id='" + Job_ID + "_Request_Change_Deadline_Cancel_btn' class='Request_Change_Deadline_Cancel_btn' onclick='Request_Change_Deadline_Cancel(\"" + Job_ID + "\"); return false;' src='/Index/IMG/Button/Delete18.jpg' style='display: none'><img id='" + Job_ID + "_Request_Change_Deadline_Loading_img' class='Request_Change_Deadline_Loading_img' src='/Index/IMG/Loading/Loading_Red_32.gif' style='display: none'></td>"
                            + "<td align='left' valign='middle' class='Job_td_body'>" + Job_Content_Full + "</td>"
                            + "<td align='center' valign='middle' class='Job_td_body NOT_Job_Myself_td'>" + Sender + "<br/><img src='/Index/IMG/Button/Arrow_Down.jpg' style='padding: 10px;'><br/>" + Receiver + "</td>"
                        ;

                        //
                        Job_td = Replace_Index_Host(Job_td);

                        //append
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

                        //NOT_Job_Myself_td
                        if (Sender_UserId == Receiver_UserId) {

                            $(".NOT_Job_Myself_td").hide();

                            $("#Report_Done_lbl_Header").html("Đã<br/>Làm xong");
                        }
                        else {
                            $(".NOT_Job_Myself_td").show();

                            $("#Report_Done_lbl_Header").html("Báo cáo<br/>Đã xong");
                        }

                        //Have_Request_Change_Deadline
                        var Have_Request_Change_Deadline = false;

                        if ((Request_Change_Deadline == '1') && (Confirm_Change_Deadline != '1')) {
                            Have_Request_Change_Deadline = true;
                        }

                        //Notify
                        if (Notify == 'New') {//OK

                            $("#" + Job_ID + "_YES_To_Do_btn").addClass('Notify');
                            $("#" + Job_ID + "_NO_To_Do_btn").addClass('Notify');
                        }
                        else
                            if (Notify == 'YES_To_Do') {//OK
                                $("#" + Job_ID + "_Report_Done_btn").addClass('Notify');
                            }
                            else
                                if (Notify == 'Thank') {

                                    $("#" + Job_ID + "_Thank_btn").addClass('Notify');
                                }
                                else
                                    if (Notify == 'Redo') {

                                        $("#" + Job_ID + "_Redo_btn").addClass('Notify_Blue');
                                        $("#" + Job_ID + "_Report_Done_btn").addClass('Notify');
                                    }
                                    else
                                        if (Notify == 'Request_Change_Deadline') {

                                            if (Job_ID != $("#Editing_Job_ID_hdf").val()) {
                                                $("#" + Job_ID + "_Request_Change_Deadline_Edit_btn").addClass('Notify');
                                            }
                                        }
                                        else
                                            if (Notify == 'Confirm_Change_Deadline') {

                                                if (Job_ID != $("#Editing_Job_ID_hdf").val()) {

                                                    $("#" + Job_ID + "_Deadline_Date_tbx").addClass('Notify_Text');
                                                    $("#" + Job_ID + "_Deadline_Hour_tbx").addClass('Notify_Text');
                                                    $("#" + Job_ID + "_Deadline_Minute_tbx").addClass('Notify_Text');

                                                    $("#" + Job_ID + "_Deadline_Date_tbx").attr("onclick", "Clear_Notify('" + Job_ID + "', '" + Notify + "'); return false;");
                                                    $("#" + Job_ID + "_Deadline_Hour_tbx").attr("onclick", "Clear_Notify('" + Job_ID + "', '" + Notify + "'); return false;");
                                                    $("#" + Job_ID + "_Deadline_Minute_tbx").attr("onclick", "Clear_Notify('" + Job_ID + "', '" + Notify + "'); return false;");

                                                    //
                                                    if ((YES_To_Do == '') && (NO_To_Do == '')) {
                                                        $("#" + Job_ID + "_YES_To_Do_btn").addClass('Notify');
                                                        $("#" + Job_ID + "_NO_To_Do_btn").addClass('Notify');
                                                    }
                                                }
                                            }
                                            else
                                                if (Notify == '') {

                                                    if ((YES_To_Do == '') && (NO_To_Do == '')) {
                                                        $("#" + Job_ID + "_YES_To_Do_btn").addClass('Notify');
                                                        $("#" + Job_ID + "_NO_To_Do_btn").addClass('Notify');
                                                    }
                                                }

                        //
                        if (Sender_UserId == Receiver_UserId) {

                            //Report_Done
                            if (Report_Done == '1') {

                                $("#" + Job_ID + "_Report_Done_btn").show();
                                $("#" + Job_ID + "_Report_Done_lbl").show();
                                $("#" + Job_ID + "_Report_Done_btn").removeAttr("onclick");
                                $("#" + Job_ID + "_Report_Done_btn").addClass("Job_btn_No_Click");
                            }
                            else {
                                $("#" + Job_ID + "_Report_Done_btn").show();
                                $("#" + Job_ID + "_Report_Done_lbl").hide();
                            }
                        }
                        else {

                            //YES_To_Do
                            if ((YES_To_Do == '') && (NO_To_Do == '')) {

                                //(Notify == 'Request_Change_Deadline')
                                if (Have_Request_Change_Deadline) {

                                    $("#" + Job_ID + "_YES_To_Do_btn").hide();
                                    $("#" + Job_ID + "_YES_To_Do_lbl").hide();

                                    $("#" + Job_ID + "_NO_To_Do_btn").hide();
                                    $("#" + Job_ID + "_NO_To_Do_lbl").hide();
                                }
                                else {
                                    $("#" + Job_ID + "_YES_To_Do_btn").show();
                                    $("#" + Job_ID + "_YES_To_Do_lbl").hide();

                                    if (Receiver_UserId != '') {
                                        $("#" + Job_ID + "_NO_To_Do_btn").show();
                                        $("#" + Job_ID + "_NO_To_Do_lbl").hide();
                                    }
                                    else {
                                        $("#" + Job_ID + "_NO_To_Do_btn").hide();
                                        $("#" + Job_ID + "_NO_To_Do_lbl").hide();
                                    }
                                }

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
                                    $("#" + Job_ID + "_YES_To_Do_btn").removeAttr("onclick");
                                    $("#" + Job_ID + "_YES_To_Do_btn").addClass("Job_btn_No_Click");

                                    $("#" + Job_ID + "_NO_To_Do_btn").hide();
                                    $("#" + Job_ID + "_NO_To_Do_lbl").hide();

                                    //
                                    $('#' + Job_ID + '_Request_Change_Deadline_Edit_btn').hide();

                                    //Report_Done
                                    if (Report_Done == '1') {

                                        $("#" + Job_ID + "_Report_Done_btn").show();
                                        $("#" + Job_ID + "_Report_Done_lbl").show();
                                        $("#" + Job_ID + "_Report_Done_btn").removeAttr("onclick");
                                        $("#" + Job_ID + "_Report_Done_btn").addClass("Job_btn_No_Click");

                                        if ((Thank == '') && (Redo == '')) {

                                            $("#" + Job_ID + "_Thank_btn").hide();
                                            $("#" + Job_ID + "_Thank_lbl").hide();

                                            $("#" + Job_ID + "_Redo_btn").hide();
                                            $("#" + Job_ID + "_Redo_lbl").hide();
                                        }
                                        else {
                                            if (Thank == '1') {

                                                $("#" + Job_ID + "_Thank_btn").show();
                                                $("#" + Job_ID + "_Thank_lbl").show();

                                                $("#" + Job_ID + "_Redo_btn").hide();
                                                $("#" + Job_ID + "_Redo_lbl").hide();

                                                if (Notify == 'Thank') {
                                                    $("#" + Job_ID + "_Thank_btn").attr("onclick", "Clear_Notify('" + Job_ID + "', '" + Notify + "'); return false;");
                                                }
                                                else {
                                                    $("#" + Job_ID + "_Thank_btn").addClass("Job_btn_No_Click");
                                                }
                                            } else
                                                if (Redo == '1') {

                                                $("#" + Job_ID + "_Thank_btn").hide();
                                                $("#" + Job_ID + "_Thank_lbl").hide();

                                                $("#" + Job_ID + "_Redo_btn").show();
                                                $("#" + Job_ID + "_Redo_lbl").show();
                                            }
                                        }
                                    }
                                    else
                                        if (Report_Done == '0') {

                                            if (Redo_Report_Done == '') {

                                                $("#" + Job_ID + "_Report_Done_btn").show();
                                                $("#" + Job_ID + "_Report_Done_lbl").hide();
                                                $("#" + Job_ID + "_Report_Done_btn").addClass("Notify");

                                                $("#" + Job_ID + "_Thank_btn").hide();
                                                $("#" + Job_ID + "_Thank_lbl").hide();

                                                $("#" + Job_ID + "_Redo_btn").show();
                                                $("#" + Job_ID + "_Redo_lbl").show();
                                            }
                                            else
                                                if (Redo_Report_Done == '1') {

                                                    $("#" + Job_ID + "_Report_Done_btn").show();
                                                    $("#" + Job_ID + "_Report_Done_lbl").show();
                                                    $("#" + Job_ID + "_Report_Done_btn").removeAttr("onclick");
                                                    $("#" + Job_ID + "_Report_Done_btn").addClass("Job_btn_No_Click");

                                                    $("#" + Job_ID + "_Thank_btn").hide();
                                                    $("#" + Job_ID + "_Thank_lbl").hide();

                                                    $("#" + Job_ID + "_Redo_btn").hide();
                                                    $("#" + Job_ID + "_Redo_lbl").hide();
                                                }
                                        }
                                        else {
                                            $("#" + Job_ID + "_Report_Done_btn").show();
                                            $("#" + Job_ID + "_Report_Done_lbl").hide();
                                            $("#" + Job_ID + "_Report_Done_btn").addClass("Notify");

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
                                        $("#" + Job_ID + "_NO_To_Do_btn").removeAttr("onclick");
                                        $("#" + Job_ID + "_NO_To_Do_btn").addClass("Job_btn_No_Click");

                                        //
                                        $('#' + Job_ID + '_Request_Change_Deadline_Edit_btn').hide();

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
            Read_Job_Received();
        }, 1500);
    }

    //
    function Read_Job_Received_Error(Response) {

        //
        setTimeout(function () {
            Read_Job_Received();
        }, 1500);

        //
        if (Response != null) {

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function YES_or_NO_To_Do(Job_ID, Title, YES_or_NO) {

    var YES_or_NO_Title = "ĐỒNG Ý";

    if (YES_or_NO == 0) {
        YES_or_NO_Title = "TỪ CHỐI";
    }

    Alert_Confirm_Parent(YES_or_NO_Title + " công việc: " + Title + " ?", YES_or_NO_To_Do_RUN);

    //
    function YES_or_NO_To_Do_RUN(Confirm) {

        if (Confirm) {

            //Editing HDF        
            $("#Editing_Job_ID_hdf").val(Job_ID);

            PageMethods.set_path($('#PageMethods_Path_hdf').val());
            PageMethods.YES_or_NO_To_Do(Job_ID, YES_or_NO, YES_or_NO_To_Do_Sucessfull, YES_or_NO_To_Do_Error);
        }
    }

    //
    function YES_or_NO_To_Do_Sucessfull(Response) {

        //alert(Response);

        if (Response == '1') {

            //
            if (YES_or_NO == 1) {

                $("#" + Job_ID + "_YES_To_Do_btn").show();
                $("#" + Job_ID + "_YES_To_Do_lbl").show();
                $("#" + Job_ID + "_YES_To_Do_btn").removeAttr("onclick");
                $("#" + Job_ID + "_YES_To_Do_btn").attr("class", "Job_btn_No_Click");

                $("#" + Job_ID + "_NO_To_Do_btn").hide();
                $("#" + Job_ID + "_NO_To_Do_lbl").hide();
            }
            else
                if (YES_or_NO == 0) {

                    $("#" + Job_ID + "_YES_To_Do_btn").hide();
                    $("#" + Job_ID + "_YES_To_Do_lbl").hide();

                    $("#" + Job_ID + "_NO_To_Do_btn").show();
                    $("#" + Job_ID + "_NO_To_Do_lbl").show();
                    $("#" + Job_ID + "_NO_To_Do_btn").removeAttr("onclick");
                    $("#" + Job_ID + "_NO_To_Do_btn").attr("class", "Job_btn_No_Click");
                }

            //
            $("#Editing_Job_ID_hdf").val(0);
        }
    }

    //
    function YES_or_NO_To_Do_Error(Response) {

        if (Response != null) {

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Request_Change_Deadline_Edit(Job_ID) {

    //Editing HDF
    $("#Editing_Job_ID_hdf").val(Job_ID);

    //Ẩn hết TBX
    $('input[name="Deadline_tbx"]').attr('class', 'TBX_Border_White');

    //Ẩn hết BTN
    $('.Request_Change_Deadline_Edit_btn').hide();
    $('.Request_Change_Deadline_Submit_btn').hide();
    $('.Request_Change_Deadline_Cancel_btn').hide();

    //Chỉ hiện TBX hiện tại
    $('#' + Job_ID + '_Deadline_Date_tbx').attr('class', 'TBX_Border_Red');
    $('#' + Job_ID + '_Deadline_Hour_tbx').attr('class', 'TBX_Border_Red');
    $('#' + Job_ID + '_Deadline_Minute_tbx').attr('class', 'TBX_Border_Red');

    $("#" + Job_ID + "_Deadline_Date_tbx").removeClass('Notify_Text');
    $("#" + Job_ID + "_Deadline_Hour_tbx").removeClass('Notify_Text');
    $("#" + Job_ID + "_Deadline_Minute_tbx").removeClass('Notify_Text');

    $("#" + Job_ID + "_Deadline_Date_tbx").removeAttr("onclick");
    $("#" + Job_ID + "_Deadline_Hour_tbx").removeAttr("onclick");
    $("#" + Job_ID + "_Deadline_Minute_tbx").removeAttr("onclick");

    //Chỉ hiện BTN hiện tại
    $('#' + Job_ID + '_Request_Change_Deadline_Edit_btn').hide();
    $('#' + Job_ID + '_Request_Change_Deadline_Submit_btn').show();
    $('#' + Job_ID + '_Request_Change_Deadline_Cancel_btn').show();
    $('#' + Job_ID + '_Request_Change_Deadline_Submit_BR').show();
}

function Request_Change_Deadline_Cancel(Job_ID) {

    //Editing HDF
    $("#Editing_Job_ID_hdf").val(0);

    //Ẩn hết TBX
    $('input[name="Deadline_tbx"]').attr('class', 'TBX_Border_White');

    //Ẩn hết BTN
    $('.Request_Change_Deadline_Edit_btn').hide();
    $('.Request_Change_Deadline_Submit_btn').hide();
    $('.Request_Change_Deadline_Cancel_btn').hide();
}

function Request_Change_Deadline_Submit(Job_ID) {

    //Check valid
    var Valid = true;
    var Message = '';

    var Deadline_Hour = parseInt($('#' + Job_ID + '_Deadline_Hour_tbx').val());
    var Deadline_Minute = parseInt($('#' + Job_ID + '_Deadline_Minute_tbx').val());
    var Deadline_Date = $('#' + Job_ID + '_Deadline_Date_tbx').val();

    //Check_Valid_Date
    if (!Check_Valid_Date(Deadline_Date, "/")) {

        Valid = false;
        Message += 'Ngày tháng "' + Deadline_Date + '": "Không có thật trong lịch"\n';
    }
    else
        if (Check_Date_Less_Than_Today_one(Deadline_Date)) {

            Valid = false;
            Message += 'Ngày tháng "' + Deadline_Date + '": "Nhỏ hơn ngày hôm nay"\n';
        }

    //
    if ((Deadline_Hour < 0) || (Deadline_Hour > 23)) {

        Valid = false;
        Message += 'Chỉ được nhập Giờ từ 0 đến 23\n';
    }

    if ((Deadline_Minute < 0) || (Deadline_Minute > 59)) {

        Valid = false;
        Message += 'Chỉ được nhập Phút từ 0 đến 59\n';
    }

    //
    if (!Valid) {
        Alert_Message_Parent('LỖI:\n' + Message);
    }
    else {

        //ẩn BTN hiện tại
        $('#' + Job_ID + '_Request_Change_Deadline_Edit_btn').hide();
        $('#' + Job_ID + '_Request_Change_Deadline_Submit_btn').hide();
        $('#' + Job_ID + '_Request_Change_Deadline_Cancel_btn').hide();
        $('#' + Job_ID + '_Request_Change_Deadline_Submit_BR').hide();

        $('#' + Job_ID + '_Request_Change_Deadline_Loading_img').show();

        //   
        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Request_Change_Deadline_Submit(Job_ID, Deadline_Date, Deadline_Hour, Deadline_Minute, Request_Change_Deadline_Submit_Sucessfull, Request_Change_Deadline_Submit_Error);
    }

    function Request_Change_Deadline_Submit_Sucessfull(Response) {

        //Alert_Message_Parent(Response);

        if (Response == '1') {

            //Hiện Edit-BTN hiện tại
            $('#' + Job_ID + '_Request_Change_Deadline_Edit_btn').show();
            $('#' + Job_ID + '_Request_Change_Deadline_Submit_btn').hide();
            $('#' + Job_ID + '_Request_Change_Deadline_Cancel_btn').hide();
            $('#' + Job_ID + '_Request_Change_Deadline_Submit_BR').hide();

            $('#' + Job_ID + '_Request_Change_Deadline_Loading_img').hide();

            //Ẩn TBX hiện tại
            $('#' + Job_ID + '_Deadline_Date_tbx').attr('class', 'TBX_Border_White');
            $('#' + Job_ID + '_Deadline_Hour_tbx').attr('class', 'TBX_Border_White');
            $('#' + Job_ID + '_Deadline_Minute_tbx').attr('class', 'TBX_Border_White');

            $("#Editing_Job_ID_hdf").val(0);
        }
        else {

            //Hiện OK-BTN hiện tại
            $('#' + Job_ID + '_Request_Change_Deadline_Edit_btn').hide();
            $('#' + Job_ID + '_Request_Change_Deadline_Submit_btn').show();
            $('#' + Job_ID + '_Request_Change_Deadline_Cancel_btn').show();
            $('#' + Job_ID + '_Request_Change_Deadline_Submit_BR').show();

            $('#' + Job_ID + '_Request_Change_Deadline_Loading_img').hide();
        }
    }

    //
    function Request_Change_Deadline_Submit_Error(Response) {

        if (Response != null) {

            //Hiện OK-BTN hiện tại
            $('#' + Job_ID + '_Request_Change_Deadline_Edit_btn').hide();
            $('#' + Job_ID + '_Request_Change_Deadline_Submit_btn').show();
            $('#' + Job_ID + '_Request_Change_Deadline_Cancel_btn').show();
            $('#' + Job_ID + '_Request_Change_Deadline_Submit_BR').show();

            $('#' + Job_ID + '_Request_Change_Deadline_Loading_img').hide();

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Report_Done(Job_ID, Title, Redo) {

    Alert_Confirm_Parent("Đã làm xong công việc: " + Title + " ?", Report_Done_RUN);

    //
    function Report_Done_RUN(Confirm) {

        if (Confirm) {

            //Editing HDF
            $("#Editing_Job_ID_hdf").val(Job_ID);

            PageMethods.set_path($('#PageMethods_Path_hdf').val());
            PageMethods.Report_Done(Job_ID, Redo, Report_Done_Sucessfull, Report_Done_Error);
        }
    }

    function Report_Done_Sucessfull(Response) {

        //alert(Response);

        if (Response == '1') {

            $("#Editing_Job_ID_hdf").val(0);

            $("#" + Job_ID + "_Report_Done_btn").show();
            $("#" + Job_ID + "_Report_Done_lbl").show();
            $("#" + Job_ID + "_Report_Done_btn").removeAttr("onclick");
            $("#" + Job_ID + "_Report_Done_btn").attr("class", "Job_btn_No_Click");
        }
    }


    function Report_Done_Error(Response) {

        if (Response != null) {

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Job_Myself_rdol_OnClick_Received() {

    $('#Job_List_tbl tbody').empty();
}