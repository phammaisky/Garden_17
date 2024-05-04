function UploadKpi_Onload() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    Resize_Page_Content_tbl_UploadKpi();
    Hide_Loading_Parent();

    $("#btnRejectKpi").attr("src", "/Index/IMG/DisLike.png");

    //OnlyNumber
    OnlyNumber("txtMonth");
    OnlyNumber("txtYear");

    //Compe
    for (var i1 = 1; i1 <= 4; i1++) {

        $("#txtPercentCompe_" + i1).watermark('%');

        OnlyNumber("txtPercentCompe_" + i1);
        OnlyNumber("txtSelfMarkCompe_" + i1);
        OnlyNumber("txtManagerMarkCompe_" + i1);

        MaxLength("txtPercentCompe_" + i1, 3);
        MaxLength("txtResultCompe_" + i1, 10240);
        MaxLength("txtSelfMarkCompe_" + i1, 4);
        MaxLength("txtManagerMarkCompe_" + i1, 4);
    }

    $(".PercentCompe").on('keyup keydown', function (event) {
        CountPercentCompe();
    });

    //
    var KpiReportId = $("#hdfKpiReportId").val();

    if (Check_ID(KpiReportId)) {
        ReadKpi(KpiReportId, true);
    }
    else {
        for (var i1 = 1; i1 <= 3; i1++) {

            //AddKpi
            var id = Creat_UUID();
            AddKpiBlank(id);

            for (var i2 = 1; i2 <= 1; i2++) {

                var subId = Creat_UUID();
                AddSubKpiBlank(id, subId);
            }

            //AddNextPlan
            id = Creat_UUID();
            AddNextPlan(id, null, '', '', '');

            for (var i2 = 1; i2 <= 1; i2++) {

                var subId = Creat_UUID();
                AddSubNextPlan(id, subId, '');
            }
        }

        ShowControl();
    }
}
function Resize_Page_Content_tbl_UploadKpi() {

    var window_Height = Screen_Height();
    var window_Width = Screen_Width();

    $('#Page_Content_tbl').css('width', (window_Width) + 'px');
    $('#Page_Content_tbl').css('maxWidth', (window_Width) + 'px');
    $('#Page_Content_tbl').css('minWidth', (window_Width) + 'px');

    $('#Page_Content_tbl').css('height', (window_Height) + 'px');
    $('#Page_Content_tbl').css('maxHeight', (window_Height) + 'px');
    $('#Page_Content_tbl').css('minHeight', (window_Height) + 'px');
}

function AddKpiBlank(id) {

    if (id == null)
        id = Creat_UUID();

    AddKpi(id, null, '', '', '', '', '', '', '', '');
}
function AddSubKpiBlank(id, subId) {

    if (id == null)
        id = Creat_UUID();

    if (subId == null)
        subId = Creat_UUID();

    AddSubKpi(id, subId, '', '', '');
}

function AddKpi(id, subId, Kpi, Percent, Detail, Result, Status, SelfMark, ManagerMark, ResultMark) {

    if ((id != '') && (subId != '')) {

        if (id == null) {
            id = Creat_UUID();
        }

        if (subId == null) {
            subId = Creat_UUID();
        }

        var tr =
            "<tr id='Kpi_" + id + "_" + subId + "' class='Kpi Kpi_" + id + "'>"
            + "    <td class='SeqKpi' id='tdSeqKpi_" + id + "_" + subId + "' align='center' valign='middle'>"
            + "    </td>"
            + "    <td class='MainKpi_" + id + "' rowspan='1' align='center' valign='middle'>"
            + "        <textarea rows='2' id='txtKpi_" + id + "' disabled='true' style='width: 100%; border: 1px solid gray;'>" + Kpi + "</textarea>"
            + "    </td>"
            + "    <td class='MainKpi_" + id + "' rowspan='1' align='center' valign='middle'>"
            + "        <input type='text' id='txtPercentKpi_" + id + "' class='PercentKpi' disabled='true' style='width: 24px; border: 1px solid gray;' value='" + Percent + "' />"
            + "    </td>"
            + "    <td class='MainKpi_" + id + "' rowspan='1' align='center' valign='middle' style='width: 20px;'>"
            + "        <input type='button' value='+' id='btnAddSubKpi_" + id + "' disabled='true' onclick='AddSubKpiBlank(\"" + id + "\", null); return false;' style='width: 20px;' />"
            + "    </td>"
            + "    <td align='center' valign='middle' style='width: 20px;'>"
            + "        <input type='button' value='-' id='btnRemoveSubKpi_" + id + "_" + subId + "' disabled='true' onclick='RemoveSubKpi(true, \"" + id + "\", \"" + id + "_" + subId + "\"); return false;' style='width: 20px;' />"
            + "    </td>"
            + "    <td id='SubKpi_" + id + "_" + subId + "' align='center' valign='middle'>"
            + "        <textarea rows='2' id='txtDetailKpi_" + id + "_" + subId + "' disabled='true' style='width: 100%; border: none;'>" + Detail + "</textarea>"
            + "    </td>"
            + "    <td align='center' valign='middle'>"
            + "        <textarea rows='2' id='txtResultKpi_" + id + "_" + subId + "' disabled='true' style='width: 100%; border: none;'>" + Result + "</textarea>"
            + "    </td>"
            + "    <td align='center' valign='middle'>"
            + "        <input type='text' id='txtStatusKpi_" + id + "_" + subId + "' disabled='true' style='width: 100%; border: 1px solid gray;' value='" + Status + "' />"
            + "    </td>"
            + "    <td class='MainKpi_" + id + "' align='center' valign='middle'>"
            + "        <input type='text' id='txtSelfMarkKpi_" + id + "' disabled='true' style='width: 30px; border: 1px solid gray;' value='" + SelfMark + "' />"
            + "    </td>"
            + "    <td class='MainKpi_" + id + "' align='center' valign='middle'>"
            + "        <input type='text' id='txtManagerMarkKpi_" + id + "' disabled='true' style='width: 30px; border: 1px solid gray;' value='" + ManagerMark + "' />"
            + "    </td>"
            + "    <td class='MainKpi_" + id + "' align='center' valign='middle'>"
            + "        <span id='lblResultMarkKpi_" + id + "'>" + ResultMark + "</span>"
            + "    </td>"
            + "</tr>"
        ;

        $("#tbodyKpi").append(tr);
        AddSeqKpi();
        CountPercentKpi();

        //watermark
        $("#txtKpi_" + id).watermark('Nhóm công việc chính...');
        $("#txtPercentKpi_" + id).watermark('%');
        $("#txtDetailKpi_" + id + "_" + subId + "").watermark('Các công việc cụ thể...');

        //OnlyNumber
        OnlyNumber("txtPercentKpi_" + id);
        OnlyNumber("txtSelfMarkKpi_" + id);
        OnlyNumber("txtManagerMarkKpi_" + id);

        $(".PercentKpi").on('keyup keydown', function (event) {
            CountPercentKpi();
        });

        //MaxLength
        MaxLength("txtKpi_" + id, 10240);
        MaxLength("txtPercentKpi_" + id, 3);

        MaxLength("txtDetailKpi_" + id + "_" + subId + "", 10240);
        MaxLength("txtResultKpi_" + id + "_" + subId + "", 10240);
        MaxLength("txtStatusKpi_" + id + "_" + subId + "", 10240);

        MaxLength("txtSelfMarkKpi_" + id, 4);
        MaxLength("txtManagerMarkKpi_" + id, 4);

        //hdf
        if (Check_HDF("hdfIsReporter")) {

            Enabled_Element("txtKpi_" + id);
            Enabled_Element("txtPercentKpi_" + id);
            Enabled_Element("btnAddSubKpi_" + id);

            Enabled_Element("btnRemoveSubKpi_" + id + "_" + subId);
            Enabled_Element("txtDetailKpi_" + id + "_" + subId);
            Enabled_Element("txtResultKpi_" + id + "_" + subId);
            Enabled_Element("txtStatusKpi_" + id + "_" + subId);
            Enabled_Element("txtSelfMarkKpi_" + id);
        }
        else if (Check_HDF("hdfValidRole_Mark")) {

            Enabled_Element("txtManagerMarkKpi_" + id);
        }
    }
}
function AddSubKpi(id, subId, Detail, Result, Status) {

    if ((id != '') && (subId != '')) {

        var valid = true;

        if (valid) {

            if (subId == null) {
                subId = Creat_UUID();
            }

            var tr =
                "<tr id='Kpi_" + id + "_" + subId + "' class='Kpi Kpi_" + id + "'>"
                + "    <td class='SeqKpi' id='tdSeqKpi_" + id + "_" + subId + "' align='center' valign='middle'>"
                + "    </td>"
                + "    <td align='center' valign='middle' style='width: 20px;'>"
                + "        <input type='button' value='-' id='btnRemoveSubKpi_" + id + "_" + subId + "' disabled='true' onclick='RemoveSubKpi(false, \"" + id + "\", \"" + id + "_" + subId + "\"); return false;' style='width: 20px;' />"
                + "    </td>"
                + "    <td id='SubKpi_" + id + "_" + subId + "' align='center' valign='middle'>"
                + "        <textarea rows='2' id='txtDetailKpi_" + id + "_" + subId + "' disabled='true' style='width: 100%; border: none;'>" + Detail + "</textarea>"
                + "    </td>"
                + "    <td align='center' valign='middle'>"
                + "        <textarea rows='2' id='txtResultKpi_" + id + "_" + subId + "' disabled='true' style='width: 100%; border: none;'>" + Result + "</textarea>"
                + "    </td>"
                + "    <td align='center' valign='middle'>"
                + "        <input type='text' id='txtStatusKpi_" + id + "_" + subId + "' disabled='true' style='width: 100%; border: 1px solid gray;' value='" + Status + "' />"
                + "    </td>"
                + "</tr>"
            ;

            //Find Last
            var Last;

            $('tr').each(function () {

                if ($(this).attr('id')) {

                    var idTemp = $(this).attr('id');

                    if (Check_String_Start_With(idTemp, "Kpi_" + id + "_")) {

                        Last = $(this);
                    }
                }
            });

            //insertAfter
            $(tr).insertAfter(Last);

            //rowspan
            $(".MainKpi_" + id).attr('rowspan', $('.Kpi_' + id).length);
            AddSeqKpi();
            CountPercentKpi();

            //watermark
            $("#txtDetailKpi_" + id + "_" + subId).watermark('Các công việc cụ thể...');

            //MaxLength
            MaxLength("txtDetailKpi_" + id + "_" + subId, 10240);
            MaxLength("txtResultKpi_" + id + "_" + subId, 10240);
            MaxLength("txtStatusKpi_" + id + "_" + subId, 10240);

            //hdf
            if (Check_HDF("hdfIsReporter")) {

                Enabled_Element("btnRemoveSubKpi_" + id + "_" + subId);
                Enabled_Element("txtDetailKpi_" + id + "_" + subId);
                Enabled_Element("txtResultKpi_" + id + "_" + subId);
                Enabled_Element("txtStatusKpi_" + id + "_" + subId);
            }
        }
    }
}

function RemoveSubKpi(isMain, id, fullSubId) {

    var valid = false;

    if (isMain) {

        var kpi = $("#txtKpi_" + id).val();
        var subKpi = $("#txtDetailKpi_" + fullSubId).val();

        var message = "";

        if (subKpi != "") {
            message = "Xóa: \"" + subKpi + "\" ?\r\n"
        }

        if (kpi == "") {
            message += "Chú ý: Nếu xóa Chi tiết công việc Đầu tiên, sẽ xóa cả nhóm Kpi này ?!";
        }
        else {
            message += "Chú ý: Nếu xóa Chi tiết công việc Đầu tiên, sẽ xóa cả nhóm Kpi: \"" + kpi + "\" ?!";
        }

        valid = confirm(message);
    }
    else {

        var subKpi = $("#txtDetailKpi_" + fullSubId).val();

        if (subKpi == "")
            valid = true;
        else
            valid = confirm("Xóa: \"" + subKpi + "\" ?");
    }

    if (valid) {

        if (!isMain) {
            $("#Kpi_" + fullSubId).remove();
            $(".MainKpi_" + id).attr('rowspan', $('.Kpi_' + id).length);
        }
        else {
            $(".Kpi_" + id).remove();
        }

        AddSeqKpi();
        CountPercentKpi();
    }
}
function AddSeqKpi() {

    $(".SeqKpi").each(function (index) {
        $(this).html("<span>" + (index + 1) + "</span>");
    });
}

function CountPercentKpi() {

    var total = 0;

    $(".PercentKpi").each(function (index) {

        var percent = parseInt($(this).val());

        if (isNaN(percent))
            percent = 0;

        total += percent;
    });

    if (isNaN(total))
        total = 0;

    $("#lblTotalPercentKpi").text(total);
    return parseInt(total);
}
function CountPercentNextPlan() {

    var total = 0;

    $(".PercentNextPlan").each(function (index) {

        var percent = parseInt($(this).val());

        if (isNaN(percent))
            percent = 0;

        total += percent;
    });

    if (isNaN(total))
        total = 0;

    $("#lblTotalPercentNextPlan").text(total);
    return parseInt(total);
}
function CountPercentCompe() {

    var total = 0;

    $(".PercentCompe").each(function (index) {

        var percent = parseInt($(this).val());

        if (isNaN(percent))
            percent = 0;

        total += percent;
    });

    if (isNaN(total))
        total = 0;

    $("#lblTotalPercentCompe").text(total);
    return parseInt(total);
}

function AddNextPlan(id, subId, NextPlan, Percent, Detail) {

    if ((id != '') && (subId != '')) {

        if (id == null) {
            id = Creat_UUID();
        }

        if (subId == null) {
            subId = Creat_UUID();
        }

        var tr =
            "<tr id='NextPlan_" + id + "_" + subId + "' class='NextPlan NextPlan_" + id + "'>"
            + "    <td class='SeqNextPlan' id='tdSeqNextPlan_" + id + "_" + subId + "' align='center' valign='middle'>"
            + "    </td>"
            + "    <td class='MainNextPlan_" + id + "' rowspan='1' align='center' valign='middle'>"
            + "        <textarea rows='2' id='txtNextPlan_" + id + "' disabled='true' style='width: 100%; border: 1px solid gray;'>" + NextPlan + "</textarea>"
            + "    </td>"
            + "    <td class='MainNextPlan_" + id + "' rowspan='1' align='center' valign='middle'>"
            + "        <input type='text' id='txtPercentNextPlan_" + id + "' class='PercentNextPlan' disabled='true' style='width: 24px; border: 1px solid gray;' value='" + Percent + "' />"
            + "    </td>"
            + "    <td class='MainNextPlan_" + id + "' rowspan='1' align='center' valign='middle' style='width: 20px;'>"
            + "        <input type='button' value='+' id='btnAddSubNextPlan_" + id + "' disabled='true' onclick='AddSubNextPlan(\"" + id + "\", null, \"\"); return false;' style='width: 20px;' />"
            + "    </td>"
            + "    <td align='center' valign='middle' style='width: 20px;'>"
            + "        <input type='button' value='-' id='btnRemoveSubNextPlan_" + id + "_" + subId + "' disabled='true' onclick='RemoveSubNextPlan(true, \"" + id + "\", \"" + id + "_" + subId + "\"); return false;' style='width: 20px;' />"
            + "    </td>"
            + "    <td id='SubNextPlan_" + id + "_" + subId + "' align='center' valign='middle'>"
            + "        <textarea rows='2' id='txtDetailNextPlan_" + id + "_" + subId + "' disabled='true' style='width: 100%; border: none;'>" + Detail + "</textarea>"
            + "    </td>"
            + "</tr>"
        ;

        $("#tbodyNextPlan").append(tr);
        AddSeqNextPlan();
        CountPercentNextPlan();

        //watermark
        $("#txtNextPlan_" + id).watermark('Nhóm kế hoạch chính...');
        $("#txtPercentNextPlan_" + id).watermark('%');
        $("#txtDetailNextPlan_" + id + "_" + subId + "").watermark('Các kế hoạch cụ thể...');

        //OnlyNumber
        OnlyNumber("txtPercentNextPlan_" + id);

        $(".PercentNextPlan").on('keyup keydown', function (event) {
            CountPercentNextPlan();
        });

        //MaxLength
        MaxLength("txtNextPlan_" + id, 10240);
        MaxLength("txtPercentNextPlan_" + id, 3);
        MaxLength("txtDetailNextPlan_" + id + "_" + subId + "", 10240);

        //hdf
        if (Check_HDF("hdfIsReporter")) {

            Enabled_Element("txtNextPlan_" + id);
            Enabled_Element("txtPercentNextPlan_" + id);
            Enabled_Element("btnAddSubNextPlan_" + id);

            Enabled_Element("btnRemoveSubNextPlan_" + id + "_" + subId);
            Enabled_Element("txtDetailNextPlan_" + id + "_" + subId);
        }
    }
}
function AddSubNextPlan(id, subId, Detail) {

    if ((id != '') && (subId != '')) {

        var valid = true;

        if (valid) {

            if (subId == null) {
                subId = Creat_UUID();
            }

            var tr =
                "<tr id='NextPlan_" + id + "_" + subId + "' class='NextPlan NextPlan_" + id + "'>"
                + "    <td class='SeqNextPlan' id='tdSeqNextPlan_" + id + "_" + subId + "' align='center' valign='middle'>"
                + "    </td>"
                + "    <td align='center' valign='middle' style='width: 20px;'>"
                + "        <input type='button' value='-' id='btnRemoveSubNextPlan_" + id + "_" + subId + "' disabled='true' onclick='RemoveSubNextPlan(false, \"" + id + "\", \"" + id + "_" + subId + "\"); return false;' style='width: 20px;' />"
                + "    </td>"
                + "    <td id='SubNextPlan_" + id + "_" + subId + "' align='center' valign='middle'>"
                + "        <textarea rows='2' id='txtDetailNextPlan_" + id + "_" + subId + "' disabled='true' style='width: 100%; border: none;'>" + Detail + "</textarea>"
                + "    </td>"
                + "</tr>"
            ;

            //Find Last
            var Last;

            $('tr').each(function () {

                if ($(this).attr('id')) {

                    var idTemp = $(this).attr('id');

                    if (Check_String_Start_With(idTemp, "NextPlan_" + id + "_")) {

                        Last = $(this);
                    }
                }
            });

            //insertAfter
            $(tr).insertAfter(Last);

            //rowspan
            $(".MainNextPlan_" + id).attr('rowspan', $('.NextPlan_' + id).length);
            AddSeqNextPlan();
            CountPercentNextPlan();

            //watermark
            $("#txtDetailNextPlan_" + id + "_" + subId).watermark('Các kế hoạch cụ thể...');

            //MaxLength
            MaxLength("txtDetailNextPlan_" + id + "_" + subId, 10240);

            //hdf
            if (Check_HDF("hdfIsReporter")) {

                Enabled_Element("btnRemoveSubNextPlan_" + id + "_" + subId);
                Enabled_Element("txtDetailNextPlan_" + id + "_" + subId);
            }
        }
    }
}

function RemoveSubNextPlan(isMain, id, fullSubId) {

    var valid = false;

    if (isMain) {

        var NextPlan = $("#txtNextPlan_" + id).val();
        var subNextPlan = $("#txtDetailNextPlan_" + fullSubId).val();

        var message = "";

        if (subNextPlan != "") {
            message = "Xóa: \"" + subNextPlan + "\" ?\r\n"
        }

        if (NextPlan == "") {
            message += "Chú ý: Nếu xóa Chi tiết công việc Đầu tiên, sẽ xóa cả nhóm Kế hoạch này ?!";
        }
        else {
            message += "Chú ý: Nếu xóa Chi tiết công việc Đầu tiên, sẽ xóa cả nhóm Kế hoạch: \"" + NextPlan + "\" ?!";
        }

        valid = confirm(message);
    }
    else {

        var subNextPlan = $("#txtDetailNextPlan_" + fullSubId).val();

        if (subNextPlan == "")
            valid = true;
        else
            valid = confirm("Xóa: \"" + subNextPlan + "\" ?");
    }

    if (valid) {

        if (!isMain) {
            $("#NextPlan_" + fullSubId).remove();
            $(".MainNextPlan_" + id).attr('rowspan', $('.NextPlan_' + id).length);
        }
        else {
            $(".NextPlan_" + id).remove();
        }

        AddSeqNextPlan();
        CountPercentNextPlan();
    }
}
function AddSeqNextPlan() {

    $(".SeqNextPlan").each(function (index) {
        $(this).html("<span>" + (index + 1) + "</span>");
    });
}

function SubmitKpi(Reject) {

    var messageTitle = "";

    if (Check_HDF("hdfIsReporter")) {
        messageTitle = "Báo cáo";
    }
    else if (Check_HDF("hdfValidRole_Confirm")) {
        messageTitle = Reject ? "Từ chối" : "Xác nhận Hoàn thành";
    }
    else if (Check_HDF("hdfValidRole_Mark")) {
        messageTitle = Reject ? "Từ chối" : "Đánh giá";
    }
    else if (Check_HDF("hdfIsBOD")) {
        messageTitle = "Thêm Nhận Xét";
    }

    //jsonKpi
    var jsonKpi = "[";

    $('textarea').each(function () {

        //id
        if ($(this).attr('id')) {

            var idAll = $(this).attr('id');

            if (Check_String_Start_With(idAll, "txtKpi_")) {

                var id = Remove_String_First(idAll, "txtKpi_");

                var Kpi = $("#txtKpi_" + id).val();
                var Percent = $("#txtPercentKpi_" + id).val();

                var SelfMark = $("#txtSelfMarkKpi_" + id).val();
                var ManagerMark = $("#txtManagerMarkKpi_" + id).val();

                var invalid = true;

                if (Kpi != "") {

                    //subId
                    $('textarea').each(function () {

                        if ($(this).attr('id')) {

                            var subIdAll = $(this).attr('id');

                            if (Check_String_Start_With(subIdAll, "txtDetailKpi_" + id + "_")) {

                                var subId = Remove_String_First(subIdAll, "txtDetailKpi_" + id + "_");
                                var Seq = $("#tdSeqKpi_" + id + "_" + subId).text();

                                var Detail = $("#txtDetailKpi_" + id + "_" + subId).val();
                                var Result = $("#txtResultKpi_" + id + "_" + subId).val();
                                var Status = $("#txtStatusKpi_" + id + "_" + subId).val();

                                if (Detail != "") {

                                    invalid = false;
                                    var obj = new Object();

                                    obj.Item_1 = id;
                                    obj.Item_2 = subId;
                                    obj.Item_3 = Seq;

                                    obj.Item_4 = Kpi;
                                    obj.Item_5 = Percent;

                                    obj.Item_6 = Detail;
                                    obj.Item_7 = Result;
                                    obj.Item_8 = Status;

                                    obj.Item_9 = SelfMark;
                                    obj.Item_10 = ManagerMark;

                                    var subJSON = Creat_JSON_one(obj);
                                    jsonKpi += subJSON + ",";
                                }
                            }
                        }
                    });
                }

                if (invalid) {
                    $("#txtPercentKpi_" + id).val(0);
                }
            }
        }
    });

    jsonKpi = Remove_String_Last(jsonKpi, ",") + "]";

    //jsonNextPlan
    var jsonNextPlan = "[";

    $('textarea').each(function () {

        //id
        if ($(this).attr('id')) {

            var idAll = $(this).attr('id');

            if (Check_String_Start_With(idAll, "txtNextPlan_")) {

                var id = Remove_String_First(idAll, "txtNextPlan_");

                var NextPlan = $("#txtNextPlan_" + id).val();
                var Percent = $("#txtPercentNextPlan_" + id).val();

                if (NextPlan != "") {

                    //subId
                    $('textarea').each(function () {

                        if ($(this).attr('id')) {

                            var subIdAll = $(this).attr('id');

                            if (Check_String_Start_With(subIdAll, "txtDetailNextPlan_" + id + "_")) {

                                var subId = Remove_String_First(subIdAll, "txtDetailNextPlan_" + id + "_");
                                var Seq = $("#tdSeqNextPlan_" + id + "_" + subId).text();

                                var Detail = $("#txtDetailNextPlan_" + id + "_" + subId).val();

                                if (Detail != "") {

                                    var obj = new Object();

                                    obj.Item_1 = id;
                                    obj.Item_2 = subId;

                                    obj.Item_3 = Seq;

                                    obj.Item_4 = NextPlan;
                                    obj.Item_5 = Percent;
                                    obj.Item_6 = Detail;

                                    var subJSON = Creat_JSON_one(obj);
                                    jsonNextPlan += subJSON + ",";
                                }
                            }
                        }
                    });
                }
            }
        }
    });

    jsonNextPlan = Remove_String_Last(jsonNextPlan, ",") + "]";

    //jsonCompe
    var jsonCompe = "[";

    $('input[type=text]').each(function () {

        //id
        if ($(this).attr('id')) {

            var idAll = $(this).attr('id');

            if (Check_String_Start_With(idAll, "txtPercentCompe_")) {

                var id = Remove_String_First(idAll, "txtPercentCompe_");
                var Seq = $("#tdSeqCompe_" + id).text();

                var Percent = $("#txtPercentCompe_" + id).val();
                var Result = $("#txtResultCompe_" + id).val();

                var SelfMark = $("#txtSelfMarkCompe_" + id).val();
                var ManagerMark = $("#txtManagerMarkCompe_" + id).val();

                if (Percent != "") {

                    var obj = new Object();

                    obj.Item_1 = Seq;

                    obj.Item_2 = Percent;
                    obj.Item_3 = Result;

                    obj.Item_4 = SelfMark;
                    obj.Item_5 = ManagerMark;

                    var subJSON = Creat_JSON_one(obj);
                    jsonCompe += subJSON + ",";
                }
            }
        }
    });

    jsonCompe = Remove_String_Last(jsonCompe, ",") + "]";

    //valid
    var valid = true;
    var message = '';

    var KpiReportId = $("#hdfKpiReportId").val();
    var Month = $("#txtMonth").val();
    var Year = $("#txtYear").val();

    if (Month == '') {
        valid = false;
        message += 'Phải nhập: Tháng\n';
    }

    if (Year == '') {
        valid = false;
        message += 'Phải nhập: Năm\n';
    }

    if (CountPercentKpi() != 100) {
        valid = false;
        message += 'Tổng tỷ trọng "Mục tiêu Công việc" phải = 100%\n';
    }

    if (CountPercentCompe() != 100) {
        valid = false;
        message += 'Tổng tỷ trọng "Mục tiêu Năng lực" phải = 100%\n';
    }

    //Comment
    var Comment = $("#txtComment").val();
    var ManagerComment = $("#txtManagerComment").val();
    var BodComment = $("#txtBodComment").val();

    if (!valid) {
        Alert_Message_Parent('LỖI:\n' + message);
    }
    else {
        Alert_Confirm_Parent("Bạn có chắc là muốn \"" + messageTitle + "\" ?", SubmitKpi_RUN);
    }

    function SubmitKpi_RUN(Confirm) {

        if (Confirm) {

            Disabled_btnSubmitKpi();
            Disabled_btnRejectKpi();

            Show_Loading_Parent();

            var obj = new Object();

            obj.Item_1 = KpiReportId;

            obj.Item_2 = Month;
            obj.Item_3 = Year;

            obj.Item_4 = jsonKpi;
            obj.Item_5 = jsonCompe;
            obj.Item_6 = jsonNextPlan;

            obj.Item_7 = Comment;
            obj.Item_8 = ManagerComment;
            obj.Item_9 = BodComment;

            obj.Item_10 = Reject;

            var JSON = Creat_JSON_one(obj);

            PageMethods.set_path($('#PageMethods_Path_hdf').val());
            PageMethods.SubmitKpi(JSON, SubmitKpi_Sucessfull, SubmitKpi_Error);
        }
    }

    function SubmitKpi_Sucessfull(Response) {

        Hide_Loading_Parent();

        if (Response == "1") {

            if (Check_ID(KpiReportId))
                ReadKpi(KpiReportId, false);

            Alert_Message_Parent("Đã \"" + messageTitle + "\" xong !");

            //tbx
            $('input[type=text]').each(function () {
                $(this).prop('disabled', true);
            });

            //textarea
            $('textarea').each(function () {
                $(this).prop('disabled', true);
            });

            //button
            $('input[type=button]').each(function () {
                $(this).prop('disabled', true);
            });
        }
        else {
            Alert_Message_Parent('LỖI:\n' + Response);

            Enabled_btnSubmitKpi();

            if (Check_HDF("hdfValidRole_Mark") || Check_HDF("hdfValidRole_Confirm") || Check_HDF("hdfIsBOD")) {
                Enabled_btnRejectKpi();
            }
        }
    }

    function SubmitKpi_Error(Response) {

        if (Response != null) {

            Enabled_btnSubmitKpi();

            if (Check_HDF("hdfValidRole_Mark") || Check_HDF("hdfValidRole_Confirm") || Check_HDF("hdfIsBOD")) {
                Enabled_btnRejectKpi();
            }

            Hide_Loading_Parent();

            //Alert_Message_PageMethods_Error(Response);
            Alert_Message_Parent("Lỗi hệ thống, vui lòng thử bấm lại \"Nút OK\" sau vài phút...");
        }
    }
}
function ReadKpi(KpiReportId, updateAll) {

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.ReadKpi(KpiReportId, ReadKpi_Sucessfull, ReadKpi_Error);

    function ReadKpi_Sucessfull(Response) {

        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            for (var i = 0; i < JSON_Array.length; i++) {

                var KpiReportId = JSON_Array[i].Item_1;
                var Time = JSON_Array[i].Item_2;
                var UserId = JSON_Array[i].Item_3;

                var Month = JSON_Array[i].Item_4;
                var Year = JSON_Array[i].Item_5;

                var KpiId = JSON_Array[i].Item_6;
                var Kpi = JSON_Array[i].Item_7;
                var PercentKpi = JSON_Array[i].Item_8;
                var SelfMarkKpi = JSON_Array[i].Item_9;
                var ManagerMarkKpi = JSON_Array[i].Item_10;
                var ResultMarkKpi = JSON_Array[i].Item_11;

                var KpiDetailId = JSON_Array[i].Item_12;
                var DetailKpi = JSON_Array[i].Item_13;
                var ResultKpi = JSON_Array[i].Item_14;
                var StatusKpi = JSON_Array[i].Item_15;

                var NextPlanId = JSON_Array[i].Item_16;
                var NextPlan = JSON_Array[i].Item_17;
                var PercentNextPlan = JSON_Array[i].Item_18;
                var NextPlanDetailId = JSON_Array[i].Item_19;
                var DetailNextPlan = JSON_Array[i].Item_20;

                var SeqCompe = JSON_Array[i].Item_21;
                var PercentCompe = JSON_Array[i].Item_22;
                var ResultCompe = JSON_Array[i].Item_23;
                var SelfMarkCompe = JSON_Array[i].Item_24;
                var ManagerMarkCompe = JSON_Array[i].Item_25;
                var ResultMarkCompe = JSON_Array[i].Item_26;

                var TotalResultMarkKpi = JSON_Array[i].Item_27;
                var TotalResultMarkCompe = JSON_Array[i].Item_28;
                var FinalResultMarkKpi = JSON_Array[i].Item_29;
                var FinalResultMarkCompe = JSON_Array[i].Item_30;
                var FinalResultMarkKpiAndCompe = JSON_Array[i].Item_31;

                var Comment = JSON_Array[i].Item_32;
                var ManagerComment = JSON_Array[i].Item_33;
                var BodComment = JSON_Array[i].Item_34;

                var ValidRole_Mark = JSON_Array[i].Item_35;
                var ValidRole_Comment = JSON_Array[i].Item_36;
                var ValidRole_Confirm = JSON_Array[i].Item_37;

                var IsBOD = JSON_Array[i].Item_38;
                var IsReporter = JSON_Array[i].Item_39;

                var KpiStateId = JSON_Array[i].Item_40;

                var FullName = JSON_Array[i].Item_41;
                var Department = JSON_Array[i].Item_42;
                var Rank = JSON_Array[i].Item_43;
                var Grade = JSON_Array[i].Item_44;
                var StartDate = JSON_Array[i].Item_45;
                var CurrentStartDate = JSON_Array[i].Item_46;

                var FullNameManager = JSON_Array[i].Item_47;
                var RankManager = JSON_Array[i].Item_48;

                if (SelfMarkKpi == '0.00')
                    SelfMarkKpi = '';

                if (ManagerMarkKpi == '0.00')
                    ManagerMarkKpi = '';

                if (SelfMarkCompe == '0.00')
                    SelfMarkCompe = '';

                if (ManagerMarkCompe == '0.00')
                    ManagerMarkCompe = '';

                //updateAll
                if (updateAll) {

                    //Month
                    $("#txtMonth").val(Month);
                    $("#txtYear").val(Year);

                    //Name
                    $("#lblName").text(FullName);
                    $("#lblDepartment").text(Department);
                    $("#lblRank").text(Rank);
                    $("#lblGrade").text(Grade);
                    $("#lblStartDate").text(StartDate);
                    $("#lblCurrentStartDate").text(CurrentStartDate);

                    //hdf
                    $("#hdfValidRole_Mark").val(ValidRole_Mark);
                    $("#hdfValidRole_Comment").val(ValidRole_Comment);
                    $("#hdfValidRole_Confirm").val(ValidRole_Confirm);

                    $("#hdfIsBOD").val(IsBOD);
                    $("#hdfIsReporter").val(IsReporter);

                    $("#hdfKpiStateId").val(KpiStateId);

                    //Kpi
                    if (!Check_Element_Is_Not_Null("txtKpi_" + KpiId)) {
                        AddKpi(KpiId, KpiDetailId, Kpi, PercentKpi, DetailKpi, ResultKpi, StatusKpi, SelfMarkKpi, ManagerMarkKpi, ResultMarkKpi);
                    }
                    else
                        if (!Check_Element_Is_Not_Null("Kpi_" + KpiId + "_" + KpiDetailId)) {
                            AddSubKpi(KpiId, KpiDetailId, DetailKpi, ResultKpi, StatusKpi);
                        }

                    //NextPlan
                    if (!Check_Element_Is_Not_Null("txtNextPlan_" + NextPlanId)) {
                        AddNextPlan(NextPlanId, NextPlanDetailId, NextPlan, PercentNextPlan, DetailNextPlan);
                    }
                    else
                        if (!Check_Element_Is_Not_Null("NextPlan_" + NextPlanId + "_" + NextPlanDetailId)) {
                            AddSubNextPlan(NextPlanId, NextPlanDetailId, DetailNextPlan);
                        }

                    //Compe
                    $("#txtPercentCompe_" + SeqCompe).val(PercentCompe);
                    $("#txtResultCompe_" + SeqCompe).val(ResultCompe);

                    $("#txtSelfMarkCompe_" + SeqCompe).val(SelfMarkCompe);
                    $("#txtManagerMarkCompe_" + SeqCompe).val(ManagerMarkCompe);

                    //CountPercentCompe
                    CountPercentCompe();

                    //ManagerName
                    $("#lblManagerName").text(FullNameManager);
                    $("#lblManagerRank").text(RankManager);

                    //Comment
                    $("#txtComment").val(Comment);
                    $("#txtManagerComment").val(ManagerComment);
                    $("#txtBodComment").val(BodComment);

                    //ShowControl
                    ShowControl();
                }

                //ResultMarkCompe
                $("#lblResultMarkCompe_" + SeqCompe).text(ResultMarkCompe);

                //TotalResult
                $("#lblTotalResultMarkKpi").text(TotalResultMarkKpi);
                $("#lblTotalResultMarkCompe").text(TotalResultMarkCompe);

                //FinalResult
                $("#lblFinalResultMarkKpi").text(FinalResultMarkKpi);
                $("#lblFinalResultMarkCompe").text(FinalResultMarkCompe);
                $("#lblFinalResultMarkKpiAndCompe").text(FinalResultMarkKpiAndCompe);
            }
        }
    }

    function ReadKpi_Error(Response) {

        if (Response != null) {

            Alert_Message_PageMethods_Error(Response);
            //Alert_Message_Parent("Lỗi hệ thống, vui lòng thử lại sau vài phút...");
        }
    }
}

function ShowControl() {

    $("#btnSubmitKpi").attr("src", "/Index/IMG/Submit.png");
    $("#btnSubmitKpi").hide();
    $("#btnRejectKpi").hide();

    var KpiStateId = $("#hdfKpiStateId").val();
    $("#imgState").hide();

    if (Check_HDF("hdfIsReporter")) {

        if (KpiStateId != '4') {

            Enabled_Element("txtMonth");
            Enabled_Element("txtYear");

            Enabled_Element("btnAddKpi");
            Enabled_Element("btnAddNextPlan");

            Enabled_Element("txtComment");

            Show_btnSubmitKpi();
            Enabled_btnSubmitKpi();

            //Compe
            for (var i1 = 1; i1 <= 4; i1++) {
                Enabled_Element("txtPercentCompe_" + i1);
                Enabled_Element("txtResultCompe_" + i1);
                Enabled_Element("txtSelfMarkCompe_" + i1);
            }
        }
    }
    else if (Check_HDF("hdfValidRole_Mark")) {

        Enabled_Element("txtManagerComment");

        Show_btnSubmitKpi();
        Show_btnRejectKpi();

        Enabled_btnSubmitKpi();
        Enabled_btnRejectKpi();

        //Compe
        for (var i1 = 1; i1 <= 4; i1++) {
            Enabled_Element("txtManagerMarkCompe_" + i1);
        }
    }

    if (Check_HDF("hdfValidRole_Confirm")) {

        Show_btnSubmitKpi();
        Show_btnRejectKpi();

        Enabled_btnSubmitKpi();
        Enabled_btnRejectKpi();
    }

    //IsBOD
    if (Check_HDF("hdfIsBOD")) {

        Enabled_Element("txtBodComment");

        Show_btnSubmitKpi();
        Enabled_btnSubmitKpi();

        $("#btnSubmitKpi").attr("src", "/Index/IMG/Like.png");
    }

    //imgState
    if (KpiStateId == '1') {
        $("#imgState").attr("src", "/Index/IMG/Wait.png");
        $("#imgState").show();
    }
    else
        if (KpiStateId == '2') {
            $("#imgState").attr("src", "/Index/IMG/Approved.png");
            $("#imgState").show();
        }
        else
            if (KpiStateId == '3') {
                $("#imgState").attr("src", "/Index/IMG/Rejected.png");
                $("#imgState").show();
            }
            else
                if (KpiStateId == '4') {
                    $("#imgState").attr("src", "/Index/IMG/Done.png");
                    $("#imgState").show();
                }

    //tbx
    $('input[type=text]').each(function () {
        if ($(this).prop('disabled')) {
            $(this).css('font-style', 'italic');
        }
    });

    //textarea
    $('textarea').each(function () {
        if ($(this).prop('disabled')) {
            $(this).css('font-style', 'italic');
        }
    });

    //button
    $('input[type=button]').each(function () {
        if ($(this).prop('disabled')) {
            $(this).hide();
        }
    });
}

function Disabled_btnSubmitKpi() {
    $("#btnSubmitKpi").attr("onclick", "return false;");
    $("#btnSubmitKpi").addClass("Disabled");
}
function Enabled_btnSubmitKpi() {
    $("#btnSubmitKpi").attr("onclick", "SubmitKpi(false); return false;");
    $("#btnSubmitKpi").removeClass("Disabled");
}

function Disabled_btnRejectKpi() {
    $("#btnRejectKpi").attr("onclick", "return false;");
    $("#btnRejectKpi").addClass("Disabled");
}
function Enabled_btnRejectKpi() {
    $("#btnRejectKpi").attr("onclick", "SubmitKpi(true); return false;");
    $("#btnRejectKpi").removeClass("Disabled");
}

function Show_btnSubmitKpi() {
    $("#btnSubmitKpi").show();
}
function Show_btnRejectKpi() {
    $("#btnSubmitKpi").attr("src", "/Index/IMG/Like.png");
    $("#btnRejectKpi").show();
}
