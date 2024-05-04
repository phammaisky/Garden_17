function Creat_Job_Onload() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    //
    Resize_Page_Content_tbl_Creat_Job();

    //
    Hide_Loading_Parent();

    //
    Department_ddl_Onchange();
}

function Resize_Page_Content_tbl_Creat_Job() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    $('#Page_Content_tbl').css('width', (window_Width) + 'px');
    $('#Page_Content_tbl').css('maxWidth', (window_Width) + 'px');
    $('#Page_Content_tbl').css('minWidth', (window_Width) + 'px');

    $('#Page_Content_tbl').css('height', (window_Height) + 'px');
    $('#Page_Content_tbl').css('maxHeight', (window_Height) + 'px');
    $('#Page_Content_tbl').css('minHeight', (window_Height) + 'px');
}

function Department_ddl_Onchange() {

    Creat_Receiver_ddl();
}

function Creat_Receiver_ddl() {

    $('#Receiver_ddl').empty();
    Add_Item_To_DDL("Receiver_ddl", "Cả phòng", "0");

    var Department_ID = $('#Department_ddl').val();

    if (Check_Object_NOT_Null_Or_Empty(Department_ID)) {

        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Creat_Receiver_ddl(Department_ID, Creat_Receiver_ddl_Sucessfull, Creat_Receiver_ddl_Error);
    }

    //
    function Creat_Receiver_ddl_Sucessfull(Response) {

        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                for (var i = 0; i < JSON_Array.length; i++) {

                    var UserId = JSON_Array[i].Item_1;
                    var Name = JSON_Array[i].Item_2;

                    Add_Item_To_DDL("Receiver_ddl", Name, UserId);
                }
            }
        }
    }

    //
    function Creat_Receiver_ddl_Error(Response) {

        if (Response != null) {

            alert(Response.get_message());
            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Creat_Receiver_ddl_Job_Myself() {

    $('#Receiver_ddl').empty();
    Add_Item_To_DDL("Receiver_ddl", "Cả phòng", "0");

    var Department_ID = $('#Department_ddl').val();

    if (Check_Object_NOT_Null_Or_Empty(Department_ID)) {

        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Creat_Receiver_ddl(Department_ID, Creat_Receiver_ddl_Sucessfull, Creat_Receiver_ddl_Error);
    }

    //
    function Creat_Receiver_ddl_Sucessfull(Response) {

        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                for (var i = 0; i < JSON_Array.length; i++) {

                    var UserId = JSON_Array[i].Item_1;
                    var Name = JSON_Array[i].Item_2;

                    Add_Item_To_DDL("Receiver_ddl", Name, UserId);
                }

                //
                var My_UserId = $('#UserId_hdf').val();

                Re_Choice_DDL('Receiver_ddl', My_UserId);

                Disabled_Element('Department_ddl');
                Disabled_Element('Receiver_ddl');
            }
        }
    }

    //
    function Creat_Receiver_ddl_Error(Response) {

        if (Response != null) {

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Submit_Job() {

    //Check valid
    var Valid = true;
    var Message = '';

    //
    var Job_Myself = Determine_Checked_RDOL('Job_Myself_rdol');

    var Department_ID = '';
    var Receiver_ID = '';

    var Receiver_FULL = '';
    var Department = '';
    var Receiver = '';

    if (Job_Myself == '1') {

        Department_ID = $('#Department_ID_hdf').val();
        Receiver_ID = $('#UserId_hdf').val();

        Receiver_FULL = "Chính mình";
    }
    else {
        Department_ID = $('#Department_ddl').val();
        Receiver_ID = $('#Receiver_ddl').val();

        Department = $("#Department_ddl option:selected").text();
        Receiver = $("#Receiver_ddl option:selected").text();

        Receiver_FULL = Receiver + " (" + Department + ")";
    }

    //
    var Level_ID = $('#Level_ddl').val();

    var Deadline_Hour = $('#Deadline_Hour_ddl').val();
    var Deadline_Minute = $('#Deadline_Minute_ddl').val();
    var Deadline_Date = $('#Deadline_Date_tbx').val();

    //
    var Job_Type = $('#Job_Type_tbx').val();

    var Title = $('#Title_tbx').val();
    var Content = $('#Content_tbx').val();

    //Check_Valid_Date
    if (!Check_Valid_Date(Deadline_Date, "/")) {

        Valid = false;
        Message += 'Ngày tháng "' + Deadline_Date + '": "Không có thật trong lịch" \n';
    }
    else
        if (Check_Date_Less_Than_Today_one(Deadline_Date)) {

            Valid = false;
            Message += 'Ngày tháng "' + Deadline_Date + '": "Nhỏ hơn ngày hôm nay" \n';
        }

    //
    if (Job_Type.length > 100) {

        Valid = false;
        Message += 'Phải nhập Loại công việc: Ít hơn 100 ký tự\n';
    }

    //
    if ((Title.length < 5) || (Title.length > 100)) {

        Valid = false;
        Message += 'Phải nhập Tiêu đề: Từ 5 -> 100 ký tự\n';
    }

    //
    if (Content.length > 10000) {

        Valid = false;
        Message += 'Phải nhập Nội dung: Ít hơn 10.000 ký tự\n';
    }

    //
    if (!Valid) {
        Alert_Message_Parent('LỖI:\n' + Message);
    }
    else {

        //
        if (Job_Myself == '1') {
            Alert_Confirm_Parent("Bạn có chắc chắn muốn Tự tạo Kế hoạch cho: Chính mình ?<br/><br/>Tiêu đề:<br/>" + Title, Submit_Job_RUN);
        }
        else {
            Alert_Confirm_Parent("Bạn có chắc chắn muốn Gửi công việc cho: " + Receiver_FULL + " ?<br/><br/>Tiêu đề:<br/>" + Title, Submit_Job_RUN);
        }
    }

    //
    function Submit_Job_RUN(Confirm) {

        if (Confirm) {

            Disabled_Element('OK_btn');

            Disabled_Element('Job_Type_tbx');
            Disabled_Element('Title_tbx');
            Disabled_Element('Content_tbx');

            Show_Loading_Parent();

            //
            var Submit_Job_obj = new Object();

            Submit_Job_obj.Item_1 = Level_ID;

            Submit_Job_obj.Item_2 = Deadline_Hour;
            Submit_Job_obj.Item_3 = Deadline_Minute;
            Submit_Job_obj.Item_4 = Deadline_Date;

            Submit_Job_obj.Item_5 = Department_ID;
            Submit_Job_obj.Item_6 = Receiver_ID;

            Submit_Job_obj.Item_7 = Job_Type;

            Submit_Job_obj.Item_8 = Title;
            Submit_Job_obj.Item_9 = Content;

            //
            var Submit_Job_JSON = Creat_JSON_one(Submit_Job_obj);

            //   
            PageMethods.set_path($('#PageMethods_Path_hdf').val());
            PageMethods.Submit_Job(Submit_Job_JSON, Submit_Job_Sucessfull, Submit_Job_Error);
        }
    }

    function Submit_Job_Sucessfull(Response) {

        Enabled_Element('OK_btn');

        Enabled_Element('Job_Type_tbx');
        Enabled_Element('Title_tbx');
        Enabled_Element('Content_tbx');

        Hide_Loading_Parent();

        if (Response == "1") {

            $('#Job_Type_tbx').val('');
            $('#Title_tbx').val('');
            $('#Content_tbx').val('');

            Alert_Message_Parent("Đã gửi yêu cầu công việc !");
        }
        else {
            Alert_Message_Parent(Response);
        }
    }

    //
    function Submit_Job_Error(Response) {

        if (Response != null) {

            Enabled_Element('OK_btn');

            Enabled_Element('Job_Type_tbx');
            Enabled_Element('Title_tbx');
            Enabled_Element('Content_tbx');

            Hide_Loading_Parent();

            //Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Job_Myself_rdol_OnClick() {

    var Job_Myself = Determine_Checked_RDOL('Job_Myself_rdol');

    if (Job_Myself == '1') {

        var My_Department_ID = $('#Department_ID_hdf').val();
        var My_UserId = $('#UserId_hdf').val();

        Re_Choice_DDL('Department_ddl', My_Department_ID);
        Creat_Receiver_ddl_Job_Myself();
    }
    else {
        Enabled_Element('Department_ddl');
        Enabled_Element('Receiver_ddl');
    }
}