function Add_Point_Onload() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    $('#Client_Refresh_hdf').val('');

    //
    $(document).on('keydown', function (event) {

        if ((Get_Key_Pressed(event) == 116)) {
            Home_On_Client_Refresh();
        }
    });

    //
    $('#Content_tbl').width(window_Width);
    $('#Content_tbl').height(window_Height);

    $('#Shop_tbl').height(window_Height - 25);

    $('#Shop_List_tbl').height(window_Height - 45 - 80);
    $('#Shop_List_tbl').css('maxHeight', window_Height - 45 - 80 + 'px');

    if (Check_HDF('Is_POS_hdf')) {

        $("input[type='text']").keyboard({
            layout: 'custom',
            customLayout: {
                'default': ['{a} {left} {right} {b}', '0 1 2 3 4 5 6 7 8 9', 'Q W E R T Y U I O P', 'A S D F G H J K L', 'Z X C V B N M', '{space}']
            },
            restrictInput: true,
            preventPaste: true,
            autoAccept: true
        });
    }

    $("input[type='text']").css('background-color', 'White');

    //
    Disabled_Element_BG('Point_tbx');

    //
    Creat_Shop_List("0");

    //
    $('#Shop_tbx').bind('change keyup paste cut', function (event) {//mouseup // change paste cut

        var Shop = $('#Shop_tbx').val();

        Shop = Replace_String(Shop, "  ", " ");

        $('#Shop_MaThue_lbl').html('');
        $('#Enable_Add_Point_lbl').html('');
        $('#Shop_ul_1').empty();

        //
        if ((Check_HDF('Is_POS_hdf')) || (Get_Key_Pressed(event) == 13)) {

            if (Shop.length >= 2) {

                Display_TR('Shop_List_tr');
                Creat_Shop_List(Shop);
            }
        }

        //
        Hide_Element('Loading_tr');
        Hide_Element('Add_Point_Result_tr');
    });

    //
    $('#Search_Name_tbx').bind('change keyup paste cut', function (event) {//mouseup // change paste cut

        var Search_Name = $('#Search_Name_tbx').val();

        Search_Name = Replace_String(Search_Name, "  ", " ");

        $('#Shop_ul_1').empty();

        //
        if ((Check_HDF('Is_POS_hdf')) || (Get_Key_Pressed(event) == 13)) {

            if (Search_Name.length >= 2) {

                Display_TR('Shop_List_tr');
                Creat_Search_Name_List(Search_Name);
            }
        }

        //
        Hide_Element('Loading_tr');
        Hide_Element('Add_Point_Result_tr');
    });

    //
    $('#Card_tbx').bind('change keyup paste cut', function () {//mouseup 

        var Card = $('#Card_tbx').val();

        Card = Replace_String(Card, " ", "");

        Hide_Element('Member_Info_tr');
        Hide_Element('Space_Member_Info_tr');

        $('#Member_Info_div').html('');

        //
        if (Card.length == 11) {
            Read_Card_Info(Card);
        }

        //
        Hide_Element('Loading_tr');
        Hide_Element('Add_Point_Result_tr');
    });

    //
    $('#POS_tbx').bind('change keyup paste cut', function (event) {//mouseup 
        Read_Receipt_Info_Onchange(event);
    });

    //
    $('#Buy_Time_Day_tbx').bind('change keyup paste cut', function (event) {//mouseup 
        Read_Receipt_Info_Onchange(event);
    });

    //
    $('#Buy_Time_Month_tbx').bind('change keyup paste cut', function (event) {//mouseup 
        Read_Receipt_Info_Onchange(event);
    });

    //
    $('#Buy_Time_Year_tbx').bind('change keyup paste cut', function (event) {//mouseup 
        Read_Receipt_Info_Onchange(event);
    });

    //
    $('#Receipt_tbx').bind('change keyup paste cut', function (event) {//mouseup 
        Read_Receipt_Info_Onchange(event);
    });

    //
    $('#Money_tbx').bind('change keyup paste cut', function () {//mouseup // change

        if (($('#Enable_Add_Point_lbl').html() == "(Được tích điểm)") || ($('#Enable_Add_Point_lbl').html() == "")) {

            var Money = $('#Money_tbx').val();

            Money = Replace_String(Money, " ", "");
            Money = Replace_String(Money, ".", "");
            $('#Money_tbx').val(Money.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1."));

            var Money_int = parseInt(Money);
            var Point = Money_int / 15000;

            Point = parseInt(Point);

            if (isNaN(Point)) {
                Point = 0;
            }

            $('#Point_tbx').val(Point.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1."));
        }
        else {
            $('#Point_tbx').val('0');
        }

        Hide_Element('Loading_tr');
        Hide_Element('Add_Point_Result_tr');
    });

    //Read_Iam_Info
    window.setInterval(function () {
        Read_Iam_Info();
    }, 10 * 1000);
}

function Read_Receipt_Info_Onchange(event) {

    var POS = $('#POS_tbx').val();
    var Receipt = $('#Receipt_tbx').val();

    var Buy_Time_Day = $('#Buy_Time_Day_tbx').val();
    var Buy_Time_Month = $('#Buy_Time_Month_tbx').val();
    var Buy_Time_Year = $('#Buy_Time_Year_tbx').val();

    if (Buy_Time_Day.length == 1) {
        Buy_Time_Day = "0" + Buy_Time_Day;
    }

    if (Buy_Time_Month.length == 1) {
        Buy_Time_Month = "0" + Buy_Time_Month;
    }

    if (Buy_Time_Year.length == 1) {
        Buy_Time_Year = "0" + Buy_Time_Year;
    }

    var Buy_Time = Buy_Time_Day + "" + Buy_Time_Month + "" + Buy_Time_Year;

    //POS
    if (POS.length == 0) {
        POS = "0000";
    }
    else
        if (POS.length == 1) {
            POS = "000" + POS;
        }
        else
            if (POS.length == 2) {
                POS = "00" + POS;
            }
            else
                if (POS.length == 3) {
                    POS = "0" + POS;
                }

    //POS
    POS = 'AA' + POS;

    //Receipt
    if (Receipt.length == 1) {
        Receipt = "000" + Receipt;
    }
    else
        if (Receipt.length == 2) {
            Receipt = "00" + Receipt;
        }
        else
            if (Receipt.length == 3) {
                Receipt = "0" + Receipt;
            }

    //Receipt_ALL

    var Receipt_ALL = Receipt;

    if (POS != 'AA0000') {
        Receipt_ALL = POS + '' + Buy_Time + '' + Receipt;
    }

    Receipt_ALL = Replace_String(Receipt_ALL, " ", "");

    //
    if (Check_HDF('Enable_Creat_Shop_List_hdf')) {
        $('#Shop_tbx').val('');
        Enabled_Element_BG('Shop_tbx');

        $('#Shop_MaThue_lbl').html('');
        $('#Enable_Add_Point_lbl').html('');
    }

    //    
    $('#Money_tbx').val('');
    $('#Point_tbx').val('');
    Enabled_Element_BG('Money_tbx');

    //
    //if ((Get_Key_Pressed(event) == 13)) {

    if (Receipt_ALL.length == 16) {

        Read_Receipt_Info(Receipt_ALL);
    }
    //}

    //
    Hide_Element('Loading_tr');
    Hide_Element('Add_Point_Result_tr');
}

function Read_Iam_Info() {

    //
    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Read_Iam_Info(Read_Iam_Info_Sucessfull, Read_Iam_Info_Error);

    //
    function Read_Iam_Info_Sucessfull(Response) {

        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                for (var i = 0; i < JSON_Array.length; i++) {

                    var QuayBan = JSON_Array[i].Item_1;
                    var NVBan = JSON_Array[i].Item_2;

                    $('#POS_lbl').html(QuayBan);
                    $('#Cashier_lbl').html(NVBan);
                }
            }
        }
    }

    //
    function Read_Iam_Info_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Read_Card_Info(Card) {

    //
    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Read_Card_Info(Card, Read_Card_Info_Sucessfull, Read_Card_Info_Error);

    //
    function Read_Card_Info_Sucessfull(Response) {

        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                for (var i = 0; i < JSON_Array.length; i++) {

                    var Card_Info = JSON_Array[i].Item_1;
                    var Current_Point = JSON_Array[i].Item_2;

                    $('#Member_Info_div').html(Card_Info);
                    $('#Current_Point_lbl').html(Current_Point);

                    Display_TR('Member_Info_tr');
                    Display_TR('Space_Member_Info_tr');
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

function Read_Receipt_Info(Receipt) {

    //
    if (Check_HDF('Enable_Creat_Shop_List_hdf')) {
        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Read_Receipt_Info(Receipt, Read_Receipt_Info_Sucessfull, Read_Receipt_Info_Error);
    }

    //
    function Read_Receipt_Info_Sucessfull(Response) {

        //alert(Response);

        if (Response == '0') {
            alert('Hóa đơn đã Tích điểm rồi !');
        }
        else {
            var JSON_Array = Parse_JSON_String_To_Array(Response);

            if (JSON_Array != null) {

                if (JSON_Array.length > 0) {

                    for (var i = 0; i < JSON_Array.length; i++) {

                        var MaQuay = JSON_Array[i].Item_1;
                        var MaNV = JSON_Array[i].Item_2;

                        var MaThue = JSON_Array[i].Item_3;
                        var Shop_AND_Code = JSON_Array[i].Item_4;
                        var Tich_Diem = JSON_Array[i].Item_5;

                        var Buy_Time_Day = JSON_Array[i].Item_6;
                        var Buy_Time_Month = JSON_Array[i].Item_7;
                        var Buy_Time_Year = JSON_Array[i].Item_8;

                        var Money = JSON_Array[i].Item_9;
                        var Point = JSON_Array[i].Item_10;

                        $('#Money_tbx').val(Money);
                        $('#Point_tbx').val(Point);

                        Disabled_Element_BG('Money_tbx');
                        //Disabled_Element_BG('Point_tbx');

                        if (Check_HDF('Enable_Creat_Shop_List_hdf')) {

                            $('#Shop_tbx').val(Shop_AND_Code);
                            Disabled_Element_BG('Shop_tbx');

                            $('#Shop_MaThue_lbl').html(MaThue);

                            if (Tich_Diem == '1') {
                                $('#Enable_Add_Point_lbl').html("(Được tích điểm)");
                            }
                            else {
                                $('#Enable_Add_Point_lbl').html("(Không tích điểm)");
                            }

                            $('#Shop_ul_1').empty();
                        }
                    }
                }
            }
        }
    }

    //
    function Read_Receipt_Info_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Creat_Search_Name_List(Search_Name_OR_Phone) {

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Creat_Search_Name_List(Search_Name_OR_Phone, Creat_Search_Name_List_Sucessfull, Creat_Search_Name_List_Error);

    //
    function Creat_Search_Name_List_Sucessfull(Response) {

        //alert(Response);

        var Have_Result = false;

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                Have_Result = true;

                for (var i = 0; i < JSON_Array.length; i++) {

                    var Name = JSON_Array[i].Item_1;
                    var Phone = JSON_Array[i].Item_2;
                    var Card = JSON_Array[i].Item_3;

                    if (!Check_Element_Is_Not_Null(Card + '_li')) {
                        $('#Shop_ul_1').append("<li id='" + Card + "_li'><a href='#' OnClick=\"Search_Name_li_OnClick('" + Card + "'); return false;\"><span class='Red_Text_css' style='font-size: 12pt;'>" + Name + "</span><br/><span class='Blue_Text_css' style='font-size: 12pt;'>" + Phone + "</span></a></li>");
                    }
                }

                //
                if (JSON_Array.length == 1) {
                    Search_Name_li_OnClick(Card);
                }
            }
        }

        //
        if (!Have_Result) {
            if (!Check_Element_Is_Not_Null('0_li')) {
                $('#Shop_ul_1').append("<li id='0_li'><span class='Bold_Red_Text_css' style='font-size: 12pt;'>Không tìm được Khách hàng nào !<br/>Hãy thử Tên hoặc Phone khác !</span></li>");
            }
        }
    }

    //
    function Creat_Search_Name_List_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

//
function Search_Name_li_OnClick(Card) {
    $('#Card_tbx').val(Card);
    Read_Card_Info(Card);
}

function Creat_Shop_List(Shop_Name_OR_Code) {

    if (Check_HDF('Enable_Creat_Shop_List_hdf')) {
        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Creat_Shop_List(Shop_Name_OR_Code, Creat_Shop_List_Sucessfull, Creat_Shop_List_Error);
    }

    //
    function Creat_Shop_List_Sucessfull(Response) {

        //alert(Response);

        var Have_Result = false;

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                Have_Result = true;

                for (var i = 0; i < JSON_Array.length; i++) {

                    var MaThue = JSON_Array[i].Item_1;
                    var Shop_AND_Code = JSON_Array[i].Item_2;
                    var Tich_Diem = JSON_Array[i].Item_3;

                    if (!Check_Element_Is_Not_Null(MaThue + '_li')) {

                        if (Tich_Diem == '1') {
                            $('#Shop_ul_1').append("<li id='" + MaThue + "_li' class='1' value='" + MaThue + "'><a href='#' OnClick=\"Shop_li_OnClick('" + MaThue + "', '" + Shop_AND_Code + "', '" + Tich_Diem + "'); return false;\"><span class='Bold_Red_Text_css' style='font-size: 12pt;'>" + Shop_AND_Code + "</span></a></li>");
                        }
                        else {
                            $('#Shop_ul_1').append("<li id='" + MaThue + "_li' class='0' value='" + MaThue + "'><a href='#' OnClick=\"Shop_li_OnClick('" + MaThue + "', '" + Shop_AND_Code + "', '" + Tich_Diem + "'); return false;\"><span class='Blue_Text_css' style='font-size: 12pt;'>" + Shop_AND_Code + "</span></a></li>");
                        }
                    }
                }

                //
                if (JSON_Array.length == 1) {
                    Shop_li_OnClick(MaThue, Shop_AND_Code, Tich_Diem);
                }
            }
        }

        //
        if (!Have_Result) {
            if (!Check_Element_Is_Not_Null('0_li')) {
                $('#Shop_ul_1').append("<li id='0_li'><span class='Bold_Red_Text_css' style='font-size: 12pt;'>Không tìm được Shop nào !<br/>Hãy thử Shop hoặc Code khác !</span></li>");
            }
        }
    }

    //
    function Creat_Shop_List_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

//
function Shop_li_OnClick(MaThue, Shop_AND_Code, Tich_Diem) {

    if (!Check_Disabled_Element('Shop_tbx')) {

        $('#Shop_tbx').val(Shop_AND_Code);
        $('#Shop_MaThue_lbl').html(MaThue);

        var Reason = Determine_Checked_RDOL('Reason_rdol');

        if (Reason == 'Add') {

            if (Tich_Diem == '1') {
                $('#Enable_Add_Point_lbl').html("(Được tích điểm)");

                var Money = $('#Money_tbx').val();

                Money = Replace_String(Money, " ", "");
                Money = Replace_String(Money, ".", "");
                $('#Money_tbx').val(Money.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1."));

                var Money_int = parseInt(Money);
                var Point = Money_int / 15000;

                Point = parseInt(Point);

                if (isNaN(Point)) {
                    Point = 0;
                }

                $('#Point_tbx').val(Point.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1."));
            }
            else {
                $('#Enable_Add_Point_lbl').html("(Không tích điểm)");
                $('#Point_tbx').val('0');
            }
        }
    }
}

function Submit_Add_Point_btn_OnClientClick() {

    var Valid = true;
    var Message = '';

    //
    var Card = $('#Card_tbx').val();
    var Member_Info_div = $('#Member_Info_div').html();

    var POS = $('#POS_tbx').val();
    var Receipt = $('#Receipt_tbx').val();

    var Buy_Time_Day = $('#Buy_Time_Day_tbx').val();
    var Buy_Time_Month = $('#Buy_Time_Month_tbx').val();
    var Buy_Time_Year = $('#Buy_Time_Year_tbx').val();

    if (Buy_Time_Day.length == 1) {
        Buy_Time_Day = "0" + Buy_Time_Day;
    }

    if (Buy_Time_Month.length == 1) {
        Buy_Time_Month = "0" + Buy_Time_Month;
    }

    if (Buy_Time_Year.length == 1) {
        Buy_Time_Year = "0" + Buy_Time_Year;
    }

    var Buy_Time = Buy_Time_Day + "" + Buy_Time_Month + "" + Buy_Time_Year;

    //POS
    if (POS.length == 0) {
        POS = "0000";
    }
    else
        if (POS.length == 1) {
            POS = "000" + POS;
        }
        else
            if (POS.length == 2) {
                POS = "00" + POS;
            }
            else
                if (POS.length == 3) {
                    POS = "0" + POS;
                }

    //POS
    POS = 'AA' + POS;

    //Receipt
    if (Receipt.length == 1) {
        Receipt = "000" + Receipt;
    }
    else
        if (Receipt.length == 2) {
            Receipt = "00" + Receipt;
        }
        else
            if (Receipt.length == 3) {
                Receipt = "0" + Receipt;
            }

    //Receipt_ALL

    var Receipt_ALL = Receipt;

    if (POS != 'AA0000') {
        Receipt_ALL = POS + '' + Buy_Time + '' + Receipt;
    }

    Receipt_ALL = Replace_String(Receipt_ALL, " ", "");

    Buy_Time = Buy_Time_Day + "/" + Buy_Time_Month + "/20" + Buy_Time_Year;

    var MaThue = $('#Shop_MaThue_lbl').html();

    var Money = $('#Money_tbx').val();
    var Point = $('#Point_tbx').val();
    var Current_Point = $('#Current_Point_lbl').html();

    var Reason = Determine_Checked_RDOL('Reason_rdol');

    Money = Replace_String(Money, ".", "");
    Point = Replace_String(Point, ".", "");
    Current_Point = Replace_String(Current_Point, ".", "");

    //
    if (isNaN(parseInt(Money))) {
        Money = '0';
    }

    if (isNaN(parseInt(Point))) {
        Point = '0';
    }

    if (isNaN(parseInt(Current_Point))) {
        Current_Point = '0';
    }

    var Point_int = parseInt(Point);
    var Current_Point_int = parseInt(Current_Point);

    //
    if ((Card.length != 11) || (Member_Info_div == '')) {

        Valid = false;
        Message += 'Số thẻ: Sai !\n';
    }

    //
    if (Reason == 'Add') {
        if (MaThue == '') {

            Valid = false;
            Message += 'Phải chọn: Shop !\n';
        }

        if (Receipt == '') {

            Valid = false;
            Message += 'Phải nhập: Số hóa đơn !\n';
        }

        if ((Buy_Time_Day == '') || (Buy_Time_Month == '') || (Buy_Time_Year == '')) {

            Valid = false;
            Message += 'Phải nhập: Ngày tháng !\n';
        }
        else {

            if (($('#Enable_Add_Point_lbl').html() == "(Được tích điểm)") || ($('#Enable_Add_Point_lbl').html() == "")) {

                var Money = $('#Money_tbx').val();

                Money = Replace_String(Money, " ", "");
                Money = Replace_String(Money, ".", "");
                $('#Money_tbx').val(Money.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1."));

                var Money_int = parseInt(Money);
                var Point = Money_int / 15000;

                Point = parseInt(Point);

                if (isNaN(Point)) {
                    Point = 0;
                }

                $('#Point_tbx').val(Point.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1."));
            }
            else {
                $('#Point_tbx').val('0');
            }

            //
            var Date = Buy_Time_Month + "/" + Buy_Time_Day + "/20" + Buy_Time_Year;

            if (!Check_Valid_Date(Date, "/")) {
                Valid = false;
                Message += 'Ngày tháng "' + Buy_Time + '": "Không Có Thật Trong Lịch" !\n';
            }
            else
                if (!Check_Date_Less_Than_Today('20' + Buy_Time_Year, Buy_Time_Month, Buy_Time_Day)) {
                    Valid = false;
                    Message += 'Ngày tháng "' + Buy_Time + '": "Lớn Hơn Ngày Hôm Nay" !\n';
                }
        }

        if ((Money == '') || (Money == '0')) {
            Valid = false;
            Message += 'Phải nhập: Số tiền !\n';
        }
    }
    else {

        if ((Point == '') || (Point == '0')) {
            Valid = false;
            Message += 'Phải nhập: Số điểm !\n';
        }

        if ((Reason == 'Mistake') || (Reason == 'Minus') || (Reason == 'Redeem')) {

            if (Point_int > Current_Point_int) {
                Valid = false;
                Message += 'Số điểm TRỪ ĐI phải < hoặc = Tổng điểm Hiện Tại ! (< hoặc = ' + Current_Point_int + ')\n';
            }
        }

        if ((Reason == 'Minus') || (Reason == 'Reward')) {

            if ($('#Reason_tbx').val() != '') {
                Reason += ': ' + $('#Reason_tbx').val();
            }
        }
    }

    //
    if (!Valid) {
        Alert_Message('LỖI:\n' + Message);
    }
    else {

        Hide_Element('Submit_btn');

        Display_TR('Loading_tr');
        Hide_Element('Add_Point_Result_tr');
        Hide_Element('Shop_List_tr');

        //
        var Add_Point_obj = new Object();

        Add_Point_obj.Item_1 = Card;
        Add_Point_obj.Item_2 = Receipt_ALL;
        Add_Point_obj.Item_3 = MaThue;
        Add_Point_obj.Item_4 = Buy_Time;
        Add_Point_obj.Item_5 = Money;
        Add_Point_obj.Item_6 = Point;
        Add_Point_obj.Item_7 = Reason;

        //
        var Add_Point_JSON = Creat_JSON_one(Add_Point_obj);

        //   
        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Submit_Add_Point(Add_Point_JSON, Add_Point_Sucessfull, Add_Point_Error);
    }

    function Add_Point_Sucessfull(Response) {

        //alert(Response);

        if (Response == '0') {
            Alert_Message("LỖI ! Hãy kiểm tra lại các thông số nhập vào !");
        }
        else {
            //Alert_Message("Đã thực hiện: Thành Công !" + "\n\n" + Response);

            Alert_Message("Đã thực hiện: Thành Công !");

            Hide_Element('Loading_tr');
            Display_TR('Add_Point_Result_tr');
            Hide_Element('Shop_List_tr');

            $('#Add_Point_Result_div').html("Đã thực hiện: Thành Công !" + "<br/><br/>" + Replace_String(Response, "\n", "<br/>"));
            Clear_ALL_TBX();

            //
            if (Check_HDF('Is_POS_hdf')) {
                $('#Card_tbx').val('0101');
                $('#Member_Info_div').html('');

                Hide_Element('Member_Info_tr');
                Hide_Element('Space_Member_Info_tr');
            }
        }
    }

    function Add_Point_Error(Response) {

        if (Response != null) {

            //alert(Response.get_message());
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Clear_btn_OnClientClick() {

    Clear_ALL_TBX();

    $('#Card_tbx').val('0101');
    $('#Member_Info_div').html('');
    Hide_Element('Member_Info_tr');
    Hide_Element('Space_Member_Info_tr');

    Hide_Element('Loading_tr');
    Hide_Element('Add_Point_Result_tr');
    Display_TR('Shop_List_tr');

    if (Check_HDF('Enable_Creat_Shop_List_hdf')) {

        $('#Shop_MaThue_lbl').html('');
        $('#Enable_Add_Point_lbl').html('');
        $('#Shop_ul_1').empty();
        Creat_Shop_List('0');
    }
}

function Clear_ALL_TBX() {

    Hide_Element('Loading_tr');
    Display_Element('Submit_btn');

    $('#POS_tbx').val('');
    $('#Receipt_tbx').val('');

    $('#Buy_Time_Day_tbx').val('');
    $('#Buy_Time_Month_tbx').val('');
    $('#Buy_Time_Year_tbx').val('19');

    //
    if (Check_HDF('Enable_Creat_Shop_List_hdf')) {
        $('#Shop_tbx').val('');
        Enabled_Element_BG('Shop_tbx');

        $('#Shop_MaThue_lbl').html('');
        $('#Enable_Add_Point_lbl').html('');
    }

    //    
    $('#Money_tbx').val('');
    $('#Point_tbx').val('');
    $('#Reason_tbx').val('');
    Enabled_Element_BG('Money_tbx');

    $('#Search_Name_tbx').val('');
    $('#Current_Point_lbl').html('');

    $('#Shop_ul_1').empty();

    Re_Choice_RDOL('Reason_rdol', 'Add');
    Reason_rdol_OnClientClick();
}

function Reason_rdol_OnClientClick() {

    $('#Money_tbx').val('');
    $('#Point_tbx').val('');
    $('#Reason_tbx').val('');

    var Reason = Determine_Checked_RDOL('Reason_rdol');

    if (Reason == 'Add') {
        Disabled_Element_BG('Point_tbx');

        Display_TR('POS_tr');
        Display_TR('Buy_Time_tr');
        Display_TR('Receipt_tr');
        Display_TR('Money_tr');
        Hide_Element('Reason_tr');
        Display_TR('Shop_tr');
        Display_TR('Shop_MaThue_tr');

        if (Check_HDF('Enable_Creat_Shop_List_hdf')) {
            $('#Shop_MaThue_lbl').html('');
            $('#Enable_Add_Point_lbl').html('');
            $('#Shop_ul_1').empty();
        }
    }
    else
        if (Reason == 'Mistake') {
            Enabled_Element_BG('Point_tbx');

            Display_TR('POS_tr');
            Display_TR('Buy_Time_tr');
            Display_TR('Receipt_tr');
            Hide_Element('Money_tr');
            Hide_Element('Reason_tr');
            Display_TR('Shop_tr');
            Display_TR('Shop_MaThue_tr');

            if (Check_HDF('Enable_Creat_Shop_List_hdf')) {
                $('#Shop_MaThue_lbl').html('');
                $('#Enable_Add_Point_lbl').html('');
                $('#Shop_ul_1').empty();
            }
        }
        else
            if (Reason == 'Minus') {
                Enabled_Element_BG('Point_tbx');

                Hide_Element('POS_tr');
                Hide_Element('Buy_Time_tr');
                Hide_Element('Receipt_tr');
                Hide_Element('Money_tr');
                Display_Element('Reason_tr');
                Hide_Element('Shop_tr');
                Hide_Element('Shop_MaThue_tr');

                if (Check_HDF('Enable_Creat_Shop_List_hdf')) {
                    $('#Shop_MaThue_lbl').html('');
                    $('#Enable_Add_Point_lbl').html('');
                    $('#Shop_ul_1').empty();
                }
            }
            else
                if (Reason == 'Reward') {
                    Enabled_Element_BG('Point_tbx');

                    Hide_Element('POS_tr');
                    Hide_Element('Buy_Time_tr');
                    Hide_Element('Receipt_tr');
                    Hide_Element('Money_tr');
                    Display_Element('Reason_tr');
                    Hide_Element('Shop_tr');
                    Hide_Element('Shop_MaThue_tr');

                    if (Check_HDF('Enable_Creat_Shop_List_hdf')) {
                        $('#Shop_MaThue_lbl').html('');
                        $('#Enable_Add_Point_lbl').html('');
                        $('#Shop_ul_1').empty();
                    }
                }
                else
                    if (Reason == 'Redeem') {
                        Enabled_Element_BG('Point_tbx');

                        Hide_Element('POS_tr');
                        Hide_Element('Buy_Time_tr');
                        Hide_Element('Receipt_tr');
                        Hide_Element('Money_tr');
                        Hide_Element('Reason_tr');
                        Hide_Element('Shop_tr');
                        Hide_Element('Shop_MaThue_tr');

                        if (Check_HDF('Enable_Creat_Shop_List_hdf')) {
                            $('#Shop_MaThue_lbl').html('');
                            $('#Enable_Add_Point_lbl').html('');
                            $('#Shop_ul_1').empty();
                        }
                    }
}

function Home_On_Client_Refresh() {
    $('#Client_Refresh_hdf').val('1');
}