$(function () {
    var menuItem = $('#left-sidebar-menu-report-upf-cross-year');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-report-upf-cross-year');
    subMenuItem.addClass('active');

    //First time -> Get All
    $("#btnSearch").click();

});

function companySearchChange(val) {
    $.ajax({
        type: "POST",
        url: "/ReportUpfCross/CompanyChange",
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

    upfSearch.Year = $("#Year").val();

    $.ajax({
        type: "POST",
        url: "/ReportUpfCross/SearchUpfCrossYearReport",
        data: JSON.stringify(upfSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $("#report-upf-cross-table").dataTable().fnDestroy();
                $("#report-upf-cross-table").html(response);
                get_ajax_table_cross();
            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            $("#report-upf-cross-table").dataTable().fnDestroy();
            $("#report-upf-cross-table").html(response.responseText);
            get_ajax_table_cross();
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

    upfSearch.Year = $("#Year").val();
    $.ajax({
        type: "POST",
        url: "/ReportUpfCross/ExportExcelYear",
        data: JSON.stringify(upfSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                window.location = '/ReportUpfCross/DownloadYear';
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

function get_ajax_table_cross() {
    $('#report-upf-cross-table').dataTable({
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">'
    });
}

$('#report-upf-cross-table').on('click', '.cross-approved-year-add', function () {
    $('#btnAddUpfCrossYear').show();
    $('#btnUpdateUpfCrossYear').hide();
    var $buttonClicked = $(this);
    var id = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    $("#totalYearPoint").val(parseFloat($row.find('td:eq(4)').text().trim()));
    $("#upfCrossId").val(id);
    $("#upfCrossDetailId").val($row.find('td:eq(0)').text().trim());
});

$("#btnAddUpfCrossYear").click(function () {
    if ($("#totalYearPoint").val() == "") {
        $("#totalYearPoint").focus();
        $("#modalError").show();
        return;
    }
    var upfCrossSum = new Object();
    upfCrossSum.UpfCrossId = $('#upfCrossId').val();
    upfCrossSum.UpfCrossDetailId = $('#upfCrossDetailId').val();
    upfCrossSum.TotalYearPoint = parseFloat($('#totalYearPoint').val());
    $.ajax({
        type: "POST",
        url: "/ReportUpfCross/UpfCrossApprovedYear",
        data: JSON.stringify(upfCrossSum),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $('#add-cross-bod-approved-modal').modal('hide');
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

$('#report-upf-cross-table').on('click', '.cross-approved-year-edit', function () {
    var $buttonClicked = $(this);
    var id = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements

    var upfCrossDetailId = $row.find('td:eq(0)').text().trim();

    $.ajax({
        type: "GET",
        url: "/ReportUpfCross/UpdateCrossApprovedYear",
        data: { upfCrossId: id, upfCrossDetailId: upfCrossDetailId },
        success: function (response) {
            if (response != null) {
                $("#totalYearPoint").val(response.TotalYearPoint);
                $("#upfCrossId").val(response.UpfCrossId);
                $("#upfCrossDetailId").val(response.UpfCrossDetailId);
                $('#btnAddUpfCrossYear').hide();
                $('#btnUpdateUpfCrossYear').show();
                $('#add-cross-bod-approved-modal').modal('show');
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

$("#btnUpdateUpfCrossYear").click(function () {
    if ($("#totalYearPoint").val() == "") {
        $("#totalYearPoint").focus();
        $("#modalError").show();
        return;
    }
    var upfCrossSum = new Object();
    upfCrossSum.UpfCrossId = $('#upfCrossId').val();
    upfCrossSum.UpfCrossDetailId = $('#upfCrossDetailId').val();
    upfCrossSum.TotalYearPoint = parseFloat($('#totalYearPoint').val());
    $.ajax({
        type: "POST",
        url: "/ReportUpfCross/UpfCrossApprovedYear",
        data: JSON.stringify(upfCrossSum),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $('#add-cross-bod-approved-modal').modal('hide');
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