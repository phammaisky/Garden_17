function Login_Submit_btn_OnClientClick() {

    var Valid = true;
    var Message = '';

    //
    var UserName = document.getElementById('UserName_tbx').value;
    var Password = document.getElementById('Password_tbx').value;

    if ((UserName.length < 6) || (UserName.length > 128) || (UserName == 'Email...') || (Password.length < 6) || (Password.length > 32) || (Password == '***')) {
        Valid = false;
        Message += 'Phải Nhập: Email và Password.\n';
    } else
        if (!Check_Valid_Email(UserName)) {
            Valid = false;
            Message += 'Email không hợp lệ.\n';
        }

    //Check_Valid_UserName
    if (!Valid) {
        Alert_Message('Lỗi:\n\n' + Message);
        //Display_Element('Loading_Login_div');
        //document.getElementById('Loading_Login_div').innerHTML = "<img src = '" + $('#Index_Host_hdf').val() + "/index/Loading/Loading.gif'> ĐANG ĐĂNG NHẬP...";
    }
    else {
        Hide_Element('Login_Input_tbl');
        Hide_Element('Login_Submit_tbl');

        Display_Element('Loading_Login_div');

        //
        var Login_obj = new Object();

        Login_obj.Item_1 = UserName;
        Login_obj.Item_2 = Password;

        //
        var Login_JSON = Creat_JSON_one(Login_obj);

        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Login(Login_JSON, Login_Sucessfull, Login_Error);
    }

    function Login_Sucessfull(Response) {

        Response = Replace_Index_Host(Response);
        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            var Sucessfull = JSON_Array[0].Item_1;

            var InCorrect_UserName = JSON_Array[0].Item_2;

            var InCorrect_Password_Time = JSON_Array[0].Item_3;
            var InCorrect_Key_Pay_Money_Times = JSON_Array[0].Item_4;

            var IsLockedOut = JSON_Array[0].Item_5;
            var IsLockedOut_Because_Max_InCorrect_Password_Time = JSON_Array[0].Item_6;
            var IsLockedOut_Because_Max_InCorrect_Key_Pay_Money_Times = JSON_Array[0].Item_7;

            var Max_InCorrect_Password_Time = JSON_Array[0].Item_8;

            var Loggedin_UserId = JSON_Array[0].Item_9;
            var Loggedin_Role = JSON_Array[0].Item_10;
            var Loggedin_Role_Type = JSON_Array[0].Item_11;

            var Loggedin_Name = JSON_Array[0].Item_12;
            var Loggedin_Link_Alias = JSON_Array[0].Item_13;
            var Loggedin_Domain_AND_Link_Alias = JSON_Array[0].Item_14;
            var Loggedin_Picture = JSON_Array[0].Item_15;
            var Loggedin_Chat_Background_Picture_List = JSON_Array[0].Item_16;
            var Loggedin_Icon = JSON_Array[0].Item_17;

            if (Sucessfull == '1') {

                //
                Hide_Element('Loading_Login_div');

                Alert_Message('Đã ĐĂNG NHẬP THÀNH CÔNG !<br/><br/>Bạn có thể Bắt Đầu Viết Bài, hoặc Đăng Bán Sản Phẩm !');

                $('#Loggedin_UserId_hdf').val(Loggedin_UserId);
                $('#Loggedin_Role_hdf').val(Loggedin_Role);
                $('#Loggedin_Role_Type_hdf').val(Loggedin_Role_Type);

                $('#Loggedin_Name_hdf').val(Loggedin_Name);
                $('#Loggedin_Picture_hdf').val(Loggedin_Picture);
                $('#Loggedin_Chat_Background_Picture_List_hdf').val(Loggedin_Chat_Background_Picture_List);

                $('#Loggedin_Link_Alias_hdf').val(Loggedin_Link_Alias);
                $('#Loggedin_Domain_AND_Link_Alias_hdf').val(Loggedin_Domain_AND_Link_Alias);

                //
                if (Loggedin_Icon != '') {
                    $('#Loggedin_Icon_hdf').val(Loggedin_Icon);
                }
                else {
                    $('#Loggedin_Icon_hdf').val($('#Default_Icon_hdf').val());
                }

                try {
                    $('#Information_ifr')[0].contentWindow.Set_Domain_AND_Link_Alias_lbl(Loggedin_Link_Alias, Loggedin_Domain_AND_Link_Alias);
                } catch (e) {
                }

                //
                if (($('#Friend_Index_1_ID_hdf').val() != '0') && ($('#Friend_Index_1_ID_hdf').val() != '')) {
                    Clear_Search_Friend_News_Result();
                }
            }
            else {
                Display_Table('Login_Input_tbl');
                Display_Table('Login_Submit_tbl');

                Hide_Element('Loading_Login_div');

                if (InCorrect_UserName == '1') {
                    Alert_Message('Sai địa chỉ Email !');
                }
                else
                    if (IsLockedOut == '1') {
                        if (IsLockedOut_Because_Max_InCorrect_Password_Time == '1') {
                            Alert_Message('Tài Khoản của bạn đã bị KHÓA !<br/><br/>Vì bạn đã nhập sai MẬT KHẨU quá nhiều lần !<br/><br/>Hãy liên hệ Ban Quản Trị, để Kích Hoạt tài khoản trở lại !');
                        }
                        else
                            if (IsLockedOut_Because_Max_InCorrect_Key_Pay_Money_Times == '1') {
                                Alert_Message('Tài Khoản của bạn đã bị KHÓA !<br/><br/>Vì bạn đã nhập sai MÃ NẠP TIỀN quá nhiều lần !<br/><br/>Hãy liên hệ Ban Quản Trị, để Kích Hoạt tài khoản trở lại !');
                            }
                    }
                    else {
                        Alert_Message('Sai Mật Khẩu !<br/><br/>Chú ý: Bạn đã nhập sai ' + InCorrect_Password_Time + ' lần !<br/><br/>Nếu nhập sai quá: ' + Max_InCorrect_Password_Time + ' lần, tài khoản sẽ bị Tạm Khóa !');
                    }
            }
        }

        //
        Display_Welcome_Loggedin();

        //
        if (JSON_Array != null) {

            var Sucessfull = JSON_Array[0].Item_1;

            if (Sucessfull == '1') {
                Active_View_Frame_1_Content_tab_By_Loggedin_Role_Type();
            }
        }
    }

    function Login_Error(Response) {

        if (Response != null) {

            Enabled_Element('Login_Input_tbl');
            Display_Table('Login_Submit_tbl');

            Hide_Element('Loading_Login_div');

            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Active_View_Frame_1_Content_tab_By_Loggedin_Role_Type() {

    var Domain_Type = $('#Domain_Type_hdf').val();

    if (Domain_Type == 'web') {
        Active_View_Frame_1_Content_tab(0);
    }
    else {
        var Loggedin_Role_Type = $('#Loggedin_Role_Type_hdf').val();

        if (Loggedin_Role_Type == 'shop') {

            if (!Check_View_Frame_1_Content_tab_Is_Disabled(1)) {
                Active_View_Frame_1_Content_tab(1);

                if (!Check_View_Frame_1_Content_tab_Is_Disabled(2)) {

                    if (Check_ID(Read_Friend_Index_1_ID())) {
                        Clear_Search_Friend_News_Result();
                        Search_Friend_News('', false);
                    }
                }
            }
            else
                if (!Check_View_Frame_1_Content_tab_Is_Disabled(2)) {

                    if ($('#View_Frame_1_Content_tab_div').tabs('option', 'active') != 2) {
                        Active_View_Frame_1_Content_tab(2);
                    }

                    if (Check_ID(Read_Friend_Index_1_ID())) {
                        Clear_Search_Friend_News_Result();
                        Search_Friend_News('', false);
                    }
                }
                else
                    if (!Check_View_Frame_1_Content_tab_Is_Disabled(0)) {
                        Active_View_Frame_1_Content_tab(0);
                    }
        }
        else {
            if (!Check_View_Frame_1_Content_tab_Is_Disabled(2)) {

                if ($('#View_Frame_1_Content_tab_div').tabs('option', 'active') != 2) {
                    Active_View_Frame_1_Content_tab(2);
                }

                if (Check_ID(Read_Friend_Index_1_ID())) {
                    Clear_Search_Friend_News_Result();
                    Search_Friend_News('', false);
                }
            }
            else
                if (!Check_View_Frame_1_Content_tab_Is_Disabled(1)) {
                    Active_View_Frame_1_Content_tab(1);
                }
                else
                    if (!Check_View_Frame_1_Content_tab_Is_Disabled(0)) {
                        Active_View_Frame_1_Content_tab(0);
                    }
        }
    }
}

function Logout_lnk_OnClientClick() {

    Alert_Message('Đã THOÁT THÀNH CÔNG !<br/><br/>Hẹn Sớm Gặp Lại Bạn !');

    document.cookie = null;

    $('#Loggedin_Onload_hdf').val('');

    $('#Firt_Time_Load_For_Chat_Message_hdf').val('');
    $('#Firt_Time_Load_For_Form_Submit_Test_Drive_hdf').val('');

    $('#Loggedin_UserId_hdf').val('');
    Set_Editing_Friend_Index_1_ID_AND_Friend_News_ID('');

    //
    Clear_Watermarked_TBX('UserName_tbx');
    Clear_Watermarked_TBX('Password_tbx');

    Hide_Welcome_Loggedin();

    //
    //Hide_Element('Chat_With_Friend_Web_Friend_Domain_AND_Link_Alias_img');
    //Hide_Element('Friend_Ship_Status_Web_Friend_Domain_AND_Link_Alias_btn');

    Hide_Element('Chat_With_Friend_Web_Shop_Domain_AND_Link_Alias_img');
    Hide_Element('Friend_Ship_Status_Web_Shop_Domain_AND_Link_Alias_btn');

    $(document).find('img, button').each(function () {

        if ($(this).attr('class')) {

            if (Check_String_Contain($(this).attr('class'), 'Chat_With_Friend_img_')) {
                $(this).hide();
            }

            if (Check_String_Contain($(this).attr('class'), 'Friend_Ship_Status_btn_')) {
                $(this).hide();
            }
        }
    });

    //
    Disable_ALL_Manager_BTN();

    Active_View_Frame_1_Content_tab(1);
    $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('option', 'active', 1);

    Clear_Watermarked_TBX('Search_Friend_News_tbx');
    Hide_Element('Search_Friend_News_Filter_tbl');
    Clear_Search_Friend_News_Filter();

    //
    $('#Search_My_Friend_Result_tbl').empty();

    //
    $('#My_New_Friend_tbl').empty();
    $('#My_New_Friend_tbl').append('<tbody></tbody>');

    Hide_Element('My_New_Friend_tbl');
    Display_Total_My_New_Friend();

    //
    $('#Friend_Request_Received_tbl').empty();
    $('#Friend_Request_Received_tbl').append('<tbody></tbody>');
    Display_Total_Friend_Request_Received();

    //
    $('#ALL_Chat_With_Friend_div').empty();
    $('#ALL_Chat_With_Friend_div').append("<div id = 'Loading_ALL_My_Life_div'><img src = '" + $('#Index_Host_hdf').val() + "/index/Loading/Loading.gif'><br/><br/>ĐỢI CHÚT XÍU...<br/>ĐANG ĐĂNG BÀI VIẾT...<br/></div>");

    //
    $('#Chat_With_Friend_div').empty();

    //
    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Logout(Logout_Sucessfull, Logout_Error);

    function Logout_Sucessfull(Response) {
        //alert(Response);                

        //Write_Message('2: ' + document.cookie);
        //alert('Logout Sucessfull !');
    }

    function Logout_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Read_Information_Of_User_Loggedin() {

    Add_Hidden_Field('Loaded_PageMethods_Read_Information_Of_User_Loggedin_hdf', '');

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Read_Information_Of_User_Loggedin(Read_Information_Of_User_Loggedin_Sucessfull, Read_Information_Of_User_Loggedin_Error);

    function Read_Information_Of_User_Loggedin_Sucessfull(Response) {

        Response = Replace_Index_Host(Response);
        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            var Loggedin_UserId = JSON_Array[0].Item_1;
            var Loggedin_Role = JSON_Array[0].Item_2;
            var Loggedin_Role_Type = JSON_Array[0].Item_3;

            var Loggedin_Name = JSON_Array[0].Item_4;
            var Loggedin_Link_Alias = JSON_Array[0].Item_5;
            var Loggedin_Domain_AND_Link_Alias = JSON_Array[0].Item_6;
            var Loggedin_Picture = JSON_Array[0].Item_7;
            var Loggedin_Chat_Background_Picture_List = JSON_Array[0].Item_8;
            var Loggedin_Icon = JSON_Array[0].Item_9;

            //
            $('#Loggedin_UserId_hdf').val(Loggedin_UserId);
            $('#Loggedin_Role_hdf').val(Loggedin_Role);
            $('#Loggedin_Role_Type_hdf').val(Loggedin_Role_Type);

            $('#Loggedin_Name_hdf').val(Loggedin_Name);
            $('#Loggedin_Picture_hdf').val(Loggedin_Picture);
            $('#Loggedin_Chat_Background_Picture_List_hdf').val(Loggedin_Chat_Background_Picture_List);

            $('#Loggedin_Link_Alias_hdf').val(Loggedin_Link_Alias);
            $('#Loggedin_Domain_AND_Link_Alias_hdf').val(Loggedin_Domain_AND_Link_Alias);

            //
            if (Loggedin_Icon != '') {
                $('#Loggedin_Icon_hdf').val(Loggedin_Icon);
            }
            else {
                $('#Loggedin_Icon_hdf').val($('#Default_Icon_hdf').val());
            }

            //
            try {
                $('#Information_ifr')[0].contentWindow.Set_Domain_AND_Link_Alias_lbl(Loggedin_Link_Alias, Loggedin_Domain_AND_Link_Alias);
            } catch (e) {
            }
        }

        //            
        Display_Welcome_Loggedin();

        Add_Hidden_Field('Loaded_PageMethods_Read_Information_Of_User_Loggedin_hdf', '1');
    }

    function Read_Information_Of_User_Loggedin_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Display_Welcome_Loggedin() {

    var Loggedin_Role_Type = $('#Loggedin_Role_Type_hdf').val();

    if (Check_Loggedin()) {

        $('#Loggedin_Onload_hdf').val('1');

        $('#Firt_Time_Load_For_Chat_Message_hdf').val('1');
        $('#Firt_Time_Load_For_Form_Submit_Test_Drive_hdf').val('1');

        if (Loggedin_Role_Type != 'temp_username') {

            Hide_Element('Login_Input_tbl');
            Hide_Element('Login_Submit_tbl');

            Display_Table('Loggedin_tbl');
        }
        else {
            Display_Table('Login_Input_tbl');
            Display_Table('Login_Submit_tbl');
        }

        //
        var Loggedin_Picture = $('#Loggedin_Picture_hdf').val();
        var Loggedin_Name = $('#Loggedin_Name_hdf').val();
        var Loggedin_Domain_AND_Link_Alias = $('#Loggedin_Domain_AND_Link_Alias_hdf').val();

        //
        if (Loggedin_Picture == '') {
            Loggedin_Picture = $('#Default_Friend_Avatar_hdf').val();
        }

        Loggedin_Picture = Convert_Picture_File_Prefix_To_Real(Loggedin_Picture, 'x24');

        //
        //$('#Loggedin_Picture_lbl').html("<a target = '_blank' href = '" + Loggedin_Domain_AND_Link_Alias + "'><img src = '" + Loggedin_Picture + "' style='max-width: 24px; max-height: 24px;'></a>");
        $('#Loggedin_Name_lbl').html("<a target = '_blank' href = '" + Loggedin_Domain_AND_Link_Alias + "' style = 'color: red;'>" + Loggedin_Name + "</a>");

        //
        if (Loggedin_Role_Type != 'temp_username') {

            Enable_View_Frame_1_Content_tab(3);
            Enable_View_Frame_1_Content_tab(4);
            Disable_View_Frame_1_Content_tab(5);
            Enable_View_Frame_1_Content_tab(6);
            Enable_View_Frame_1_Content_tab(7);
            Enable_View_Frame_1_Content_tab(8);

            $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('enable', 2);
            $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('enable', 3);
            $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('enable', 4);
        }
        else {
            Enable_View_Frame_1_Content_tab(3);
            Disable_View_Frame_1_Content_tab(4);
            Enable_View_Frame_1_Content_tab(5);
            Disable_View_Frame_1_Content_tab(6);
            Disable_View_Frame_1_Content_tab(7);
            Disable_View_Frame_1_Content_tab(8);

            $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('disable', 2);
            $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('disable', 3);
            $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('disable', 4);
        }

        //
        if ($('#Manager_UserId_Of_Web_Domain_AND_Link_Alias_hdf').val() == $('#Loggedin_UserId_hdf').val()) {

            $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('disable', 0);
            Hide_Element('Chat_With_Support_div');
        }
        else {
            $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('enable', 0);

            if (Check_HDF('Enable_Chat_With_Support_hdf')) {
                Display_Element('Chat_With_Support_div');
            }
        }

        //
        if (Loggedin_Role_Type != 'temp_username') {

            $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('option', 'active', 2);

            $('#View_Frame_1_Content_MemberShip_Tab_Panel_Search_My_Friend_Tab_Header_img').attr('src', Loggedin_Picture);
            $('#View_Frame_1_Content_MemberShip_Tab_Panel_Search_My_Friend_Tab_Header_lbl').html('Bạn Của<br/>' + $('#Loggedin_Name_hdf').val());

            //
            Clear_Watermarked_TBX('Search_Friend_News_tbx');

            if ((Read_Friend_Index_1_ID() != '') && (Read_Friend_Index_1_ID() != '0')) {
                Display_Table('Search_Friend_News_Filter_tbl');
            }

            Clear_Search_Friend_News_Filter();

            //
            if (Loggedin_Role_Type == 'taoday') {

                $('#View_Frame_1_Content_Creat_New_Shop_X_Tab_Header_lbl').html("<span class='Red_Text_css'>Quản Lý Hệ Thống");
                $('#View_Frame_1_Content_Information_Tab_Header_lbl').html("<span class='Red_Text_css'>Sửa GIAO DIỆN</span><br/>Đổi Email, Mật Khẩu");
            }
            else
                if (Loggedin_Role_Type == 'shop') {

                    if (!Check_Element_Is_Not_Null('Y_' + $('#Loggedin_UserId_hdf').val() + '_tbl')) {

                        Display_TR('Creat_New_Shop_X_tr');
                        Display_TR('Space_Creat_New_Shop_X_tr');
                    }

                    //
                    $('#View_Frame_1_Content_Creat_New_Shop_X_Tab_Header_lbl').html("<span class='Red_Text_css'>Đăng Bán Sản Phẩm</span><br/>Quản Lý Tài Khoản");
                    $('#View_Frame_1_Content_Information_Tab_Header_lbl').html("<span class='Red_Text_css'>Sửa GIAO DIỆN</span><br/>Đổi Email, Mật Khẩu");
                }
                else {

                    $('#View_Frame_1_Content_Creat_New_Shop_X_Tab_Header_lbl').html("<span class='Red_Text_css'>Đăng Bán Sản Phẩm</span><br/>Quản Lý Tài Khoản");
                    $('#View_Frame_1_Content_Information_Tab_Header_lbl').html("<span class='Red_Text_css'>Sửa GIAO DIỆN</span><br/>Đổi Email, Mật Khẩu");
                }
        }

        //
        if ($('#Manager_UserId_Of_Web_Domain_AND_Link_Alias_hdf').val() != $('#Loggedin_UserId_hdf').val()) {
            //Display_Element('Chat_With_Friend_Web_Friend_Domain_AND_Link_Alias_img');

            if (Loggedin_Role_Type != 'temp_username') {
                //Display_Element('Friend_Ship_Status_Web_Friend_Domain_AND_Link_Alias_btn');
            }
        }

        if ($('#Manager_UserId_Of_Web_Shop_Domain_AND_Link_Alias_hdf').val() != $('#Loggedin_UserId_hdf').val()) {
            Display_Element('Chat_With_Friend_Web_Shop_Domain_AND_Link_Alias_img');

            if (Loggedin_Role_Type != 'temp_username') {
                Display_Element('Friend_Ship_Status_Web_Shop_Domain_AND_Link_Alias_btn');
            }
        }

        //
        Add_Hidden_Field('Loaded_PageMethods_Read_ALL_My_Life_hdf', '');
        Add_Hidden_Field('Loaded_PageMethods_Update_My_Life_hdf', '');
        Add_Hidden_Field('Loaded_PageMethods_Check_User_Is_Online_AND_Friend_Ship_Status_hdf', '');

        //
        Read_ALL_My_Life();
        Update_My_Life();

        Check_User_Is_Online_AND_Friend_Ship_Status(true);

        Update_Form_Submit_Test_Drive();
    }
    else {
        Hide_Welcome_Loggedin();
    }
}

function Hide_Welcome_Loggedin() {

    Display_Table('Login_Input_tbl');
    Display_Table('Login_Submit_tbl');

    Hide_Element('Loggedin_tbl');

    Hide_Element('Creat_New_Shop_X_tr');
    Hide_Element('Space_Creat_New_Shop_X_tr');

    Disable_View_Frame_1_Content_tab(3);
    Disable_View_Frame_1_Content_tab(4);
    Enable_View_Frame_1_Content_tab(5);
    Disable_View_Frame_1_Content_tab(6);
    Disable_View_Frame_1_Content_tab(7);
    Disable_View_Frame_1_Content_tab(8);

    $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('enable', 0);
    $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('disable', 2);
    $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('disable', 3);
    $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('disable', 4);
    $('#View_Frame_1_Content_MemberShip_Tab_Panel_div').tabs('disable', 5);

    //
    if (Check_HDF('Enable_Chat_With_Support_hdf')) {
        //Display_Element('Chat_With_Support_div');
    }

    //
    var Domain_Type = $('#Domain_Type_hdf').val();

    if (Domain_Type == 'friend') {
        $('#View_Frame_1_Content_Register_Tab_Header_lbl').html("<span class='Red_Text_css'>Đăng Ký Tài Khoản</span><br/>Lấy Lại Mật Khẩu");
    }
    else
        if ((Domain_Type == 'shop') || (Domain_Type == 'web')) {
            $('#View_Frame_1_Content_Register_Tab_Header_lbl').html("<span class='Red_Text_css'>Đăng Ký Tài Khoản</span><br/>Lấy Lại Mật Khẩu");
        }
}

function Check_Loggedin() {

    var Result = false;

    if (Check_Element_Is_Not_Null('Loggedin_UserId_hdf')) {
        if ($('#Loggedin_UserId_hdf').val() != '') {
            Result = true;
        }
    }

    return Result;
}