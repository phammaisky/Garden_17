function ViewKpi_Onload() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    Resize_Page_Content_tbl_ViewKpi();
    Hide_Loading_Parent();

    //IsReport
    if (Check_HDF("hdfIsReport"))
        ViewReportKpi();
    else//View: All or MyKpi
        ViewAllOrMyKpi();
}
function Resize_Page_Content_tbl_ViewKpi() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    $('#Page_Content_tbl').css('width', (window_Width) + 'px');
    $('#Page_Content_tbl').css('maxWidth', (window_Width) + 'px');
    $('#Page_Content_tbl').css('minWidth', (window_Width) + 'px');

    $('#Page_Content_tbl').css('height', (window_Height) + 'px');
    $('#Page_Content_tbl').css('maxHeight', (window_Height) + 'px');
    $('#Page_Content_tbl').css('minHeight', (window_Height) + 'px');
}

function ViewAllOrMyKpi() {

    $("#tblViewKpi").hide();
    $('#tbodyViewKpi').empty();

    var year = $('#ddlYear').val();

    if (year != null) {

        var MyDepartment_ID = $('#Department_ID_hdf').val();
        var MyUserId = $('#UserId_hdf').val();

        var month = $('#ddlMonth').val();

        var department_ID = $('#ddlDepartment').val();
        var userId = $('#ddlPersonnel').val();

        var positionId = $('#ddlPosition').val();
        var stateId = $('#ddlState').val();

        var orderId = $('#ddlOrder').val();

        Show_Loading_Parent();

        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.ViewAllOrMyKpi(Check_HDF("hdfIsMyKpi"), year, month, department_ID, userId, positionId, stateId, orderId, ViewAllOrMyKpi_Sucessfull, ViewAllOrMyKpi_Error);
    }

    function ViewAllOrMyKpi_Sucessfull(Response) {

        //alert(Response);
        Hide_Loading_Parent();

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            for (var i = 0; i < JSON_Array.length; i++) {

                var KpiReportId = JSON_Array[i].Item_1;
                var UserId = JSON_Array[i].Item_2;

                var Month = JSON_Array[i].Item_3;
                var Year = JSON_Array[i].Item_4;

                var TotalResultMarkKpi = JSON_Array[i].Item_5;
                var TotalResultMarkCompe = JSON_Array[i].Item_6;

                var FinalResultMarkKpi = JSON_Array[i].Item_7;
                var FinalResultMarkCompe = JSON_Array[i].Item_8;

                var FinalResultMarkKpiAndCompe = JSON_Array[i].Item_9;

                var FirstName = JSON_Array[i].Item_10;
                var LastName = JSON_Array[i].Item_11;

                var Department = JSON_Array[i].Item_12;
                var Rank = JSON_Array[i].Item_13;

                var KpiPositionId = JSON_Array[i].Item_14;
                var KpiPosition = JSON_Array[i].Item_15;

                var KpiStateId = JSON_Array[i].Item_16;
                var KpiState = JSON_Array[i].Item_17;

                var Time = JSON_Array[i].Item_18;

                var ValidRole_Mark = JSON_Array[i].Item_19;
                var ValidRole_Confirm = JSON_Array[i].Item_20;

                var Url = "/Kpi/UploadKpi.aspx?KpiReportId=" + KpiReportId;

                var PositionCss = "Blue";

                if (ValidRole_Mark == 1 || ValidRole_Confirm == 1)
                    if (KpiPositionId == MyDepartment_ID && KpiStateId == 1)
                        PositionCss = "Red";

                var tr =
                    "<tr id='Kpi_" + KpiReportId + "'>"
                    + "    <td class='SeqKpi' align='center' valign='middle'>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <span>" + Month + "/" + Year + "</span>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <span>" + Department + "</span>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <span>" + Rank + "</span>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <a href='" + Url + "'><span class='" + PositionCss + "'>" + LastName + " " + FirstName + "</span></a>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <a href='" + Url + "'><span class='" + PositionCss + "'>" + KpiPosition + "</span></a>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <a href='" + Url + "'><span class='" + PositionCss + "'>" + KpiState + "</span></a>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <span>" + FinalResultMarkKpi + "</span>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <span>" + FinalResultMarkCompe + "</span>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <span class='B Blue'>" + FinalResultMarkKpiAndCompe + "</span>"
                    + "    </td>"
                    + "    <td align='center' valign='middle'>"
                    + "        <span>" + Time + "</span>"
                    + "    </td>"
                    + "</tr>"
                ;

                $("#tbodyViewKpi").append(tr);
                AddSeqKpi();
            }
        }

        $("#tblViewKpi").show();
    }

    function ViewAllOrMyKpi_Error(Response) {

        if (Response != null) {

            Alert_Message_PageMethods_Error(Response);
            //Alert_Message_Parent("Lỗi hệ thống, vui lòng thử lại sau vài phút...");
        }
    }
}
function ViewReportKpi() {

    $("#tblReportKpi").hide();
    $('#tbodyReportKpi').empty();

    var year = $('#ddlYear').val();

    if (year != null) {

        Show_Loading_Parent();

        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.ViewReportKpi(year, ViewReportKpi_Sucessfull, ViewReportKpi_Error);
    }

    function ViewReportKpi_Sucessfull(Response) {

        //alert(Response);
        Hide_Loading_Parent();

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            for (var i = 0; i < JSON_Array.length; i++) {

                var KpiReportId = JSON_Array[i].Item_1;
                var UserId = JSON_Array[i].Item_2;

                var Month = JSON_Array[i].Item_3;
                var Year = JSON_Array[i].Item_4;

                var FinalResultMarkKpiAndCompe = JSON_Array[i].Item_5;

                var FirstName = JSON_Array[i].Item_6;
                var LastName = JSON_Array[i].Item_7;

                var Department = JSON_Array[i].Item_8;
                var Rank = JSON_Array[i].Item_9;

                var KpiAverage = JSON_Array[i].Item_10;
                var KpiRate = JSON_Array[i].Item_11;

                var Url = "/Kpi/UploadKpi.aspx?KpiReportId=" + KpiReportId;

                var tdMonth12 = "";
                for (var x = 1; x <= 12; x++)
                    tdMonth12 += "<td class='" + UserId + "_" + Year + "_" + x + "' align='center' valign='middle'></td>";

                if ($("#" + UserId + "_" + Year).length == 0) {

                    var tr =
                        "<tr id='" + UserId + "_" + Year + "'>"
                        + "    <td class='SeqKpi' align='center' valign='middle'>"
                        + "    </td>"
                        + "    <td align='center' valign='middle'>"
                        + "        <span>" + Department + "</span>"
                        + "    </td>"
                        + "    <td align='center' valign='middle'>"
                        + "        <span>" + Rank + "</span>"
                        + "    </td>"
                        + "    <td align='center' valign='middle'>"
                        + "        <span class='Blue'>" + LastName + " " + FirstName + "</span>"
                        + "    </td>"
                        + tdMonth12
                        + "    <td align='center' valign='middle'>"
                        + "        <span class='Red'>" + KpiAverage + "</span>"
                        + "    </td>"
                        + "    <td align='center' valign='middle'>"
                        + "        <span class='Red'>" + KpiRate + "</span>"
                        + "    </td>"
                        + "</tr>"
                    ;

                    $("#tbodyReportKpi").append(tr);
                    AddSeqKpi();
                }

                $("." + UserId + "_" + Year + "_" + Month).html("<a href='" + Url + "'>" + FinalResultMarkKpiAndCompe + "</a>");
            }
        }

        $("#tblReportKpi").show();
    }

    function ViewReportKpi_Error(Response) {

        if (Response != null) {

            Alert_Message_PageMethods_Error(Response);
            //Alert_Message_Parent("Lỗi hệ thống, vui lòng thử lại sau vài phút...");
        }
    }
}

function ddlYear_onchange() {

    var year = $('#ddlYear').val();
    Set_Cookie('Year', $('#ddlYear').val(), 1);

    if (Check_HDF("hdfIsReport")) {
        ViewReportKpi();
    }
    else {

        $('#ddlMonth').empty();

        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.CreatKpiMonth(Check_HDF("hdfIsMyKpi"), year, CreatKpiMonth_Sucessfull, CreatKpiMonth_Error);
    }

    function CreatKpiMonth_Sucessfull(Response) {
        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);
        if (JSON_Array != null) {
            for (var i = 0; i < JSON_Array.length; i++) {

                var Month = JSON_Array[i].Item_1;
                Add_Item_To_DDL("ddlMonth", Month, Month);
            }
        }

        Add_First_Item_To_DDL_If2("ddlMonth", "Tháng", "0");
        ddlMonth_onchange();
    }

    function CreatKpiMonth_Error(Response) {

        if (Response != null) {
            //Alert_Message_PageMethods_Error(Response);
            Alert_Message_Parent("Lỗi hệ thống, vui lòng thử lại sau vài phút...");
        }
    }
}
function ddlMonth_onchange() {

    $('#ddlDepartment').empty();

    var year = $('#ddlYear').val();
    var month = $('#ddlMonth').val();
    Set_Cookie('Month', $('#ddlMonth').val(), 1);

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.CreatKpiDepartment(Check_HDF("hdfIsMyKpi"), year, month, CreatKpiDepartment_Sucessfull, CreatKpiDepartment_Error);

    function CreatKpiDepartment_Sucessfull(Response) {
        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);
        if (JSON_Array != null) {
            for (var i = 0; i < JSON_Array.length; i++) {

                var Department_ID = JSON_Array[i].Item_1;
                var Department = JSON_Array[i].Item_2;

                Add_Item_To_DDL("ddlDepartment", Department, Department_ID);
            }
        }

        Add_First_Item_To_DDL_If2("ddlDepartment", "Phòng ban", "0");
        ddlDepartment_onchange();
    }

    function CreatKpiDepartment_Error(Response) {

        if (Response != null) {
            //Alert_Message_PageMethods_Error(Response);
            Alert_Message_Parent("Lỗi hệ thống, vui lòng thử lại sau vài phút...");
        }
    }
}

function ddlDepartment_onchange() {

    $('#ddlPersonnel').empty();

    var year = $('#ddlYear').val();
    var month = $('#ddlMonth').val();
    var Department_ID = $('#ddlDepartment').val();
    Set_Cookie('Department', $('#ddlDepartment').val(), 1);

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.CreatKpiPersonnel(Check_HDF("hdfIsMyKpi"), year, month, Department_ID, CreatKpiPersonnel_Sucessfull, CreatKpiPersonnel_Error);

    function CreatKpiPersonnel_Sucessfull(Response) {
        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);
        if (JSON_Array != null) {
            for (var i = 0; i < JSON_Array.length; i++) {

                var UserId = JSON_Array[i].Item_1;
                var FullName = JSON_Array[i].Item_2;

                Add_Item_To_DDL("ddlPersonnel", FullName, UserId);
            }
        }

        Add_First_Item_To_DDL_If2("ddlPersonnel", "Nhân viên", "0");
        ddlPersonnel_onchange();
    }

    function CreatKpiPersonnel_Error(Response) {

        if (Response != null) {
            //Alert_Message_PageMethods_Error(Response);
            Alert_Message_Parent("Lỗi hệ thống, vui lòng thử lại sau vài phút...");
        }
    }
}
function ddlPersonnel_onchange() {

    $('#ddlPosition').empty();

    var year = $('#ddlYear').val();
    var month = $('#ddlMonth').val();

    var Department_ID = $('#ddlDepartment').val();
    var UserId = $('#ddlPersonnel').val();
    Set_Cookie('Personnel', $('#ddlPersonnel').val(), 1);

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.CreatKpiPosition(Check_HDF("hdfIsMyKpi"), year, month, Department_ID, UserId, CreatKpiPosition_Sucessfull, CreatKpiPosition_Error);

    function CreatKpiPosition_Sucessfull(Response) {
        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);
        if (JSON_Array != null) {
            for (var i = 0; i < JSON_Array.length; i++) {

                var PositionId = JSON_Array[i].Item_1;
                var Position = JSON_Array[i].Item_2;

                Add_Item_To_DDL("ddlPosition", Position, PositionId);
            }
        }

        Add_First_Item_To_DDL_If2("ddlPosition", "Level", "0");
        ddlPosition_onchange();
    }

    function CreatKpiPosition_Error(Response) {

        if (Response != null) {
            //Alert_Message_PageMethods_Error(Response);
            Alert_Message_Parent("Lỗi hệ thống, vui lòng thử lại sau vài phút...");
        }
    }
}

function ddlPosition_onchange() {

    $('#ddlState').empty();

    var year = $('#ddlYear').val();
    var month = $('#ddlMonth').val();

    var Department_ID = $('#ddlDepartment').val();
    var UserId = $('#ddlPersonnel').val();

    var PositionId = $('#ddlPosition').val();
    Set_Cookie('Position', $('#ddlPosition').val(), 1);

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.CreatKpiState(Check_HDF("hdfIsMyKpi"), year, month, Department_ID, UserId, PositionId, CreatKpiState_Sucessfull, CreatKpiState_Error);

    function CreatKpiState_Sucessfull(Response) {
        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);
        if (JSON_Array != null) {
            for (var i = 0; i < JSON_Array.length; i++) {

                var StateId = JSON_Array[i].Item_1;
                var State = JSON_Array[i].Item_2;

                Add_Item_To_DDL("ddlState", State, StateId);
            }
        }

        Add_First_Item_To_DDL_If2("ddlState", "Tình trạng", "0");
        ddlState_onchange();
    }

    function CreatKpiState_Error(Response) {

        if (Response != null) {
            //Alert_Message_PageMethods_Error(Response);
            Alert_Message_Parent("Lỗi hệ thống, vui lòng thử lại sau vài phút...");
        }
    }
}
function ddlState_onchange() {
    Set_Cookie('State', $('#ddlState').val(), 1);
    ViewAllOrMyKpi();
}

function ddlOrder_onchange() {
    Set_Cookie('Order', $('#ddlOrder').val(), 1);
    ViewAllOrMyKpi();
}

function ddlAutoSelectIf2(id) {
    if ($("#" + id + " > option").length == 2)
        $("#" + id).prop('selectedIndex', 1);
}
function Add_First_Item_To_DDL(DDL_ID, Text, Value) {
    $('#' + DDL_ID).prepend(new Option(Text, Value));
}
function Add_First_Item_To_DDL_If2(DDL_ID, Text, Value) {

    if ($("#" + DDL_ID + " > option").length > 1) {

        Add_First_Item_To_DDL(DDL_ID, Text, Value);
        $("#" + DDL_ID).prop('selectedIndex', 0);
    }
}