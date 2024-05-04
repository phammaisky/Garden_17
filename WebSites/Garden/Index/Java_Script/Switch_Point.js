function Switch_Point_Onload() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    //Loading
    $('#Loading_div').css('top', (window_Height - $('#Loading_div').height()) / 2 + 'px');
    $('#Loading_div').css('left', (window_Width - $('#Loading_div').width()) / 2 + 'px');

    Hide_Element('Loading_div');

    $('#Client_Refresh_hdf').val('');

    //
    $(document).on('keydown', function (event) {

        if ((Get_Key_Pressed(event) == 116)) {
            Home_On_Client_Refresh();
        }
    });

    //
    $('#Card_1_tbx').bind('change keyup paste cut', function () {//mouseup 

        var Card = $('#Card_1_tbx').val();
        Card = Replace_String(Card, " ", "");

        $('#Card_1_Infor_td').html('');
        $('#Current_Point_1_span').html('');

        //
        if (Card.length == 11) {
            Read_Card_Info_Switch_Point(Card, '1');
        }
    });

    $('#Card_2_tbx').bind('change keyup paste cut', function () {//mouseup 

        var Card = $('#Card_2_tbx').val();
        Card = Replace_String(Card, " ", "");

        $('#Card_2_Infor_td').html('');
        $('#Current_Point_2_span').html('');

        //
        if (Card.length == 11) {
            Read_Card_Info_Switch_Point(Card, '2');
        }
    });
}

function Read_Card_Info_Switch_Point(Card, Card_Order) {

    //
    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Read_Card_Info(Card, Read_Card_Info_Sucessfull, Read_Card_Info_Error);

    //
    function Read_Card_Info_Sucessfull(Response) {

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                for (var i = 0; i < JSON_Array.length; i++) {

                    var Card_Info = JSON_Array[i].Item_1;
                    var Current_Point = JSON_Array[i].Item_2;

                    $('#Card_' + Card_Order + '_Infor_td').html(Card_Info);
                    $('#Current_Point_' + Card_Order + '_span').html(Current_Point);
                }
            }
        }
    }

    //
    function Read_Card_Info_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Submit_Switch_Point_btn_OnClientClick() {

    var Valid = true;
    var Message = '';

    //
    var Card_1 = $('#Card_1_tbx').val();
    var Member_Info_1 = $('#Card_1_Infor_td').html();

    var Card_2 = $('#Card_2_tbx').val();
    var Member_Info_2 = $('#Card_2_Infor_td').html();

    //
    if ((Card_1.length != 11) || (Member_Info_1 == '')) {

        Valid = false;
        Message += 'Số thẻ 1: Sai !\n';
    }

    //
    if ((Card_2.length != 11) || (Member_Info_2 == '')) {

        Valid = false;
        Message += 'Số thẻ 2: Sai !\n';
    }

    if (Valid) {

        if (Card_1 == Card_2) {
            Valid = false;
            Message += '2 số thẻ phải khác nhau !\n';
        }
    }

    //
    if (!Valid) {
        Alert_Message('LỖI:\n' + Message);
    }
    else {

        //
        Display_Element('Loading_div');

        //   
        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Submit_Switch_Point(Card_1, Card_2, Switch_Point_Sucessfull, Switch_Point_Error);
    }

    function Switch_Point_Sucessfull(Response) {

        Hide_Element('Loading_div');

        if (Response == '0') {
            Alert_Message("LỖI ! Hãy kiểm tra lại các thông số nhập vào !");
        }
        else {

            Alert_Message("Đã thực hiện: Thành Công !");

            $('#Current_Point_1_span').html($('#Current_Point_1_span').html() + ' + ' + $('#Current_Point_2_span').html());
            $('#Card_1_tbx').val('-' + Card_1);

            $('#Card_1_Infor_td').html('');
            $('#Card_2_Infor_td').html('');
            Read_Card_Info_Switch_Point(Card_2, '2');
        }
    }

    function Switch_Point_Error(Response) {

        Hide_Element('Loading_div');

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Submit_Switch_Multi_Point_btn_OnClientClick() {

    var Valid = true;
    var Message = '';

    var Card_1_List = Get_Checked_CBXL_To_List('Switch_Point_cbxl');
    var Card_2 = Determine_Checked_RDOL('Switch_Point_rdol');

    Card_1_List = Replace_String(Card_1_List, '#5#', '');

    if (!Check_Object_NOT_Null_Or_Empty(Card_1_List)) {
        Valid = false;
        Message += 'Phải chọn Thẻ sẽ chuyển Đi !\n';
    }

    if (!Check_Object_NOT_Null_Or_Empty(Card_2)) {
        Valid = false;
        Message += 'Phải chọn Thẻ sẽ chuyển Đến !\n';
    }

    if (Card_1_List == '#' + Card_2 + '#') {
        Valid = false;
        Message += 'Phải chọn Thẻ chuyển Đi \"khác\" với Thẻ chuyển Đến !\n';
    }

    //
    if (!Valid) {
        Alert_Message('LỖI:\n' + Message);
    }
    else {

        //
        Display_Element('Loading_div');

        //   
        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Submit_Switch_Multi_Point(Card_1_List, Card_2, Switch_Multi_Point_Sucessfull, Switch_Multi_Point_Error);
    }

    function Switch_Multi_Point_Sucessfull(Response) {

        Hide_Element('Loading_div');

        if (Response == '0') {
            Alert_Message("LỖI ! Hãy kiểm tra lại các thông số nhập vào !");
        }
        else {

            var URL = "/Tool/Report.aspx?R=Add_point&Time_Start_1=01%252f01%252f2000&Time_Finish_1=31%252f12%252f2099&Card=" + Card_2;

            Alert_Message_AND_Redirect("Đã Chuyển điểm vào Thẻ '" + Card_2 + "': Thành Công !", URL);
        }
    }

    function Switch_Multi_Point_Error(Response) {

        Hide_Element('Loading_div');

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}