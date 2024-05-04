function AmsReport() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    //Loading
    $('#Loading_div').css('top', (window_Height - $('#Loading_div').height()) / 2 + 'px');
    $('#Loading_div').css('left', (window_Width - $('#Loading_div').width()) / 2 + 'px');

    try {
        Hide_Loading_Parent();
    }
    catch (ex) {
    }

    Display_Element('Dynamic_Control_Holder_td');

    $("input[type='text']").css('background-color', 'White');
}

function Report_btn_OnClientClick() {

    Refresh_Content(null);
}

function Refresh_Content(Report_Title) {

    Hide_Element('Dynamic_Control_Holder_td');

    try {
        Show_Loading_Parent();
    }
    catch (ex) {
    }

    if (Report_Title != null) {
        $('#Report_lbl').text(Report_Title);
    }
}

function Upload_File() {

    Hide_Element('Dynamic_Control_Holder_td');
    
    try {
        Show_Loading_Parent();
    }
    catch (ex) {
    }

    Disabled_Element('Upload_btn');

    //
    var URL = "Upload.ashx";

    var Form_Data = new FormData(document.getElementById("Page_Form"));

    $.ajax({
        url: URL,
        type: 'POST',
        data: Form_Data,
        async: false,
        cache: false,
        contentType: false,
        enctype: 'multipart/form-data',
        processData: false,

        success: function (Return_Message) {
            Upload_File_Complete(Return_Message);
        }
    });
}

function Upload_File_Complete(Return_Message) {

    if (Return_Message == 'ok') {
        location.href = location.href;
    }
}