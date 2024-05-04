$(function () {
    var menuItem = $('#left-sidebar-menu-report-upf-sum');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-report-upf-sum');
    subMenuItem.addClass('active');

    //First time -> Get All
    $("#btnSearch").click();

});

function companySearchChange(val) {
    $.ajax({
        type: "POST",
        url: "/ReportUpf/CompanyChange",
        data: "companyId=" + JSON.stringify(val),
        dataType: "html",
        success: function (response) {
            if (response != null) {
                $("#DepartmentID").html(response);
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            $("#DepartmentID").html(response.responseText);
        }
    });
}

function scheduleSearchChange(val) {
    if (val == "0") {
        $('#ScheduleID').hide();
    } else {
        $('#ScheduleID').show();
    }
}


$("#btnSearch").click(function startCreate() {
    var upfSearch = new Object();
    upfSearch.CompanyID = $("#CompanyID").val();
    if (upfSearch.CompanyID.length == 0) {
        var companyID = "";
        $("#CompanyID option").each(function () {
            companyID = companyID + $(this).val() + ",";
        });
        upfSearch.CompanyID = companyID;
    }

    upfSearch.DepartmentID = $("#DepartmentID").val();
    if (upfSearch.DepartmentID.length == 0) {
        var departmentID = "";
        $("#DepartmentID option").each(function () {
            departmentID = departmentID + $(this).val() + ",";
        });
        upfSearch.DepartmentID = departmentID;
    }

    upfSearch.ScheduleType = $("#ScheduleType").val();

    upfSearch.Year = $("#Year").val();

    upfSearch.ScheduleID = $("#ScheduleID").val();

    upfSearch.StatusID = $("#StatusID").val();

    $.ajax({
        type: "POST",
        url: "/ReportUpf/SearchReportUpfList",
        data: JSON.stringify(upfSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $("#report-upf-table").dataTable().fnDestroy();
                $("#report-upf-table").html(response);
                get_ajax_table();
            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            $("#report-upf-table").dataTable().fnDestroy();
            $("#report-upf-table").html(response.responseText);
            get_ajax_table();
        }
    });
});

$("#btnExportExcel").click(function startCreate() {
    var upfSearch = new Object();
    upfSearch.CompanyID = $("#CompanyID").val();
    if (upfSearch.CompanyID.length == 0) {
        var companyID = "";
        $("#CompanyID option").each(function () {
            companyID = companyID + $(this).val() + ",";
        });
        upfSearch.CompanyID = companyID;
    }

    upfSearch.DepartmentID = $("#DepartmentID").val();
    if (upfSearch.DepartmentID.length == 0) {
        var departmentID = "";
        $("#DepartmentID option").each(function () {
            departmentID = departmentID + $(this).val() + ",";
        });
        upfSearch.DepartmentID = departmentID;
    }

    upfSearch.ScheduleType = $("#ScheduleType").val();

    upfSearch.Year = $("#Year").val();

    upfSearch.ScheduleID = $("#ScheduleID").val();

    upfSearch.StatusID = $("#StatusID").val();

    $.ajax({
        type: "POST",
        url: "/ReportUpf/ExportExcel",
        data: JSON.stringify(upfSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                window.location = '/ReportUpf/Download';
            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
        }
    });
});

function get_ajax_table() {
    $('#report-upf-table').dataTable({
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">'
    });
}

$('#report-upf-table').on('click', '.bod-approved-add', function () {
    $('#btnAddUpfSum').show();
    $('#btnUpdateUpfSum').hide();
    var $buttonClicked = $(this);
    var id = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    $("#averagePoint").val($row.find('td:eq(15)').text().trim());
    $("#sumBODPoint").val(parseFloat($row.find('td:eq(15)').text().trim()));
    $("#upfId").val(id);
    $("#note").val("");
});

$("#btnAddUpfSum").click(function () {
    if ($("#sumBODPoint").val() == "") {
        $("#sumBODPoint").focus();
        $("#modalError").show();
        return;
    }
    var upfSum = new Object();
    upfSum.UpfId = $('#upfId').val();
    upfSum.SumBODPoint = parseFloat($('#sumBODPoint').val());
    upfSum.Note = $('#note').val();
    $.ajax({
        type: "POST",
        url: "/ReportUpf/UpfSumApproved",
        data: JSON.stringify(upfSum),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $('#add-bod-approved-modal').modal('hide');
                location.reload();
            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});

$('#report-upf-table').on('click', '.bod-approved-edit', function () {
    $('#btnAddUpfSum').hide();
    $('#btnUpdateUpfSum').show();
    var $buttonClicked = $(this);
    var id = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    $.ajax({
        type: "GET",
        url: "/ReportUpf/UpdateBodApproved",
        data: { upfId: id },
        success: function (response) {
            if (response != null) {
                $("#averagePoint").val($row.find('td:eq(15)').text().trim());
                $("#sumBODPoint").val(response.SumBODPoint);
                $("#note").val(response.Note);
                $("#upfId").val(response.UpfId);
                $('#btnUpdateUpfSum').show();
                $('#btnAddUpfSum').hide();
                $('#add-bod-approved-modal').modal('show');

            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});

$("#btnUpdateUpfSum").click(function () {
    if ($("#sumBODPoint").val() == "") {
        $("#sumBODPoint").focus();
        $("#modalError").show();
        return;
    }
    var upfSum = new Object();
    upfSum.UpfId = $('#upfId').val();
    upfSum.SumBODPoint = parseFloat($('#sumBODPoint').val());
    upfSum.Note = $('#note').val();
    $.ajax({
        type: "POST",
        url: "/ReportUpf/UpfSumApproved",
        data: JSON.stringify(upfSum),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $('#add-bod-approved-modal').modal('hide');
                location.reload();
            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});