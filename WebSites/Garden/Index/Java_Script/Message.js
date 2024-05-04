function Write_Message(Message_Object) {

    var Message = Message_Object.toString();

    Message = Message.replace(/\n/g, '<br/>');

    if (Check_Element_Is_Not_Null('Message_lbl')) {
        Add_innerHTML_To_Current('Message_lbl', Message + '<br/>');
    }
    else {
        parent.Add_innerHTML_To_Current('Message_lbl', Message + '<br/>');
    }
}

function Creat_Dialog_Message_div() {

    if (!Check_Element_Is_Not_Null('Dialog_Message_div')) {
        $('#Page_Body').append("<div id='Dialog_Message_div' title='Thông Báo' style='display: none; font-family: Verdana; font-size: 8pt; color: red; width: 500px;'>");
    }

    if (!Check_Element_Is_Not_Null('Hidden_link')) {
        $('#Dialog_Message_div').append("<a id='Hidden_link' href='#' onclick='return false;' style='display: none;'>");
    }

    $('#Hidden_link').focus();
}

function Alert_Message(Message) {

    if (window.self !== window.top) {
        parent.Alert_Message(Message);
    }
    else {
        Creat_Dialog_Message_div();

        $('#Dialog_Message_div').dialog({
            modal: true,
            width: 500,
            buttons: {
                OK: function () {
                    $(this).dialog('close');
                }
            }
        });

        Message = Message.replace(/\n/g, '<br/>');

        $('#Dialog_Message_div').html(Message);
        $('#Dialog_Message_div').show();
    }
}

function Alert_Message_Iframe(Message) {

    Creat_Dialog_Message_div();

    $('#Dialog_Message_div').dialog({
        modal: true,
        width: 500,
        buttons: {
            OK: function () {
                $(this).dialog('close');
            }
        }
    });

    Message = Message.replace(/\n/g, '<br/>');

    $('#Dialog_Message_div').html(Message);
    $('#Dialog_Message_div').show();
}

function Alert_Message_PageMethods_Error(Response) {

    //var Message = 'Lỗi hệ thống, liên lạc với Ban Quản Trị:\n\n' + Response.get_message();

    //if (!Check_HDF('Client_Refresh_hdf')) {
    //    Alert_Message(Message);
    //}

    //if ((!Check_HDF('Client_Refresh_hdf')) && (Check_HDF('Is_Local_hdf'))) {
    //    Alert_Message(Message);
    //}
}

function Alert_Message_AND_By_Loading_div(Loading_For, Message) {

    Alert_Message(Message);

    try {
        Message = Message.replace(/\n/g, '<br/>');
        Display_Element('Loading_' + Loading_For + '_div');
        $('#Loading_' + Loading_For + '_div').html(Message);
    } catch (e) {
    }
}

function Alert_Message_PageMethods_Error_AND_By_Loading_div(Loading_For, Response) {

    //
    //var Message = 'Lỗi hệ thống, liên lạc với Ban Quản Trị:\n\n' + Response.get_message();

    //if (!Check_HDF('Client_Refresh_hdf')) {
    //    Alert_Message(Message);
    //}

    //try {
    //    Message = Message.replace(/\n/g, '<br/>');
    //    Display_Element('Loading_' + Loading_For + '_div');
    //    $('#Loading_' + Loading_For + '_div').html(Message);
    //} catch (e) {
    //}
}

function Alert_Message_AND_Redirect(Message, URL) {

    Creat_Dialog_Message_div();

    $('#Dialog_Message_div').dialog({
        modal: true,
        width: 500,
        buttons: {
            OK: function () {

                $(this).dialog('close');

                $('#NOT_Run_Home_On_Client_Refresh_hdf').val('1');

                Display_Element('Loading_div');

                window.location.href = URL;
            }
        }
    });

    Message = Message.replace(/\n/g, '<br/>');

    $('#Dialog_Message_div').html(Message);
    $('#Dialog_Message_div').show();
}

function Alert_Message_AND_Reload_Home(Message) {

    Creat_Dialog_Message_div();

    $('#Dialog_Message_div').dialog({
        modal: true,
        width: 500,
        buttons: {
            OK: function () {

                $(this).dialog('close');
                Home_On_Client_Refresh();
            }
        }
    });

    Message = Message.replace(/\n/g, '<br/>');

    $('#Dialog_Message_div').html(Message);
    $('#Dialog_Message_div').show();
}

function Alert_Message_AND_Open_New_Page(Message, URL) {

    Creat_Dialog_Message_div();

    $('#Dialog_Message_div').dialog({
        modal: true,
        width: 500,
        buttons: {
            OK: function () {
                window.open(URL, '_blank');
                $(this).dialog('close');
            }
        }
    });

    Message = Message.replace(/\n/g, '<br/>');

    $('#Dialog_Message_div').html(Message);
    $('#Dialog_Message_div').show();
}

function Check_Dialog_Message_div_Is_Active() {

    var Result = false;

    if (Check_Element_Is_Not_Null('Dialog_Message_div')) {

        if ($('#Dialog_Message_div').css('display') != 'none') {
            Result = true;
        }
    }

    return Result;
}
