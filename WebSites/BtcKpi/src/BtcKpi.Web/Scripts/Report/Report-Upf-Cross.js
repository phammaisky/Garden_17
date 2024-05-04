$(function () {
    var menuItem = $('#left-sidebar-menu-report-upf-cross');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-report-upf-cross');
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
    upfSearch.Month = $("#Month").val();

    $.ajax({
        type: "POST",
        url: "/ReportUpfCross/SearchUpfCrossReport",
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
    upfSearch.Month = $("#Month").val();
    $.ajax({
        type: "POST",
        url: "/ReportUpfCross/ExportExcel",
        data: JSON.stringify(upfSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                window.location = '/ReportUpfCross/Download';
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

$('#report-upf-cross-table').on('click', '.cross-bod-approved-add', function () {
    $('#btnAddUpfCrossSum').show();
    $('#btnUpdateUpfCrossSum').hide();
    $('#divPerforWeightId').hide();
    $('#divPerforPointId').hide();
    var $buttonClicked = $(this);
    var id = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    $("#bodDependWeight").val($row.find('td:eq(1)').text().trim());
    $("#bodDependPoint").val(parseFloat($row.find('td:eq(7)').text().trim()));
    $("#upfCrossId").val(id);
    $("#upfCrossDetailId").val($row.find('td:eq(0)').text().trim());

    var departmentName = $row.find('td:eq(3)').text().trim();
    if (departmentName.toLowerCase().includes("Leasing".toLowerCase()) || departmentName.toLowerCase().includes("FB".toLowerCase()) ||
        departmentName.toLowerCase().includes("Ops".toLowerCase()) || departmentName.toLowerCase().includes("Vận hành".toLowerCase())) {
        $('#divPerforWeightId').show();
        $('#divPerforPointId').show();
        $("#divPerforWeightId").prop("required", true);
        $("#divPerforPointId").prop("required", true);
    } else {
        $('#divPerforWeightId').hide();
        $('#divPerforPointId').hide();
        $("#divPerforWeightId").prop("required", false);
        $("#divPerforPointId").prop("required", false);
    }
});

$("#btnAddUpfCrossSum").click(function () {
    if ($("#bodDependWeight").val() == "") {
        $("#bodDependWeight").focus();
        $("#modalError").show();
        return;
    }
    if ($("#bodDependPoint").val() == "") {
        $("#bodDependPoint").focus();
        $("#modalError").show();
        return;
    }
    var upfCrossSum = new Object();
    upfCrossSum.UpfCrossId = $('#upfCrossId').val();
    upfCrossSum.UpfCrossDetailId = $('#upfCrossDetailId').val();
    upfCrossSum.BodDependWeight = $('#bodDependWeight').val();
    upfCrossSum.BodDependPoint = parseFloat($('#bodDependPoint').val());
    upfCrossSum.BodPerforWeight = $('#bodPerforWeight').val();
    upfCrossSum.BodPerforPoint = parseFloat($('#bodPerforPoint').val());
    $.ajax({
        type: "POST",
        url: "/ReportUpfCross/UpfCrossSumApproved",
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

$('#report-upf-cross-table').on('click', '.cross-bod-approved-edit', function () {
    $('#btnAddUpfCrossSum').hide();
    $('#btnUpdateUpfCrossSum').show();
    var $buttonClicked = $(this);
    var id = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements

    var upfCrossDetailId = $row.find('td:eq(0)').text().trim();

    $.ajax({
        type: "GET",
        url: "/ReportUpfCross/UpdateCrossBodApproved",
        data: { upfCrossId: id, upfCrossDetailId: upfCrossDetailId },
        success: function (response) {
            if (response != null) {
                var departmentName = $row.find('td:eq(3)').text().trim();
                if (departmentName.toLowerCase().includes("Leasing".toLowerCase()) || departmentName.toLowerCase().includes("FB".toLowerCase()) ||
                    departmentName.toLowerCase().includes("Ops".toLowerCase()) || departmentName.toLowerCase().includes("Vận hành".toLowerCase())) {
                    $('#divPerforWeightId').show();
                    $('#divPerforPointId').show();
                } else {
                    $('#divPerforWeightId').hide();
                    $('#divPerforPointId').hide();
                }

                $("#bodDependWeight").val(response.BodDependWeight);
                $("#bodDependPoint").val(response.BodDependPoint);
                $("#bodPerforWeight").val(response.BodPerforWeight);
                $("#bodPerforPoint").val(response.BodPerforPoint);
                $("#upfCrossId").val(response.UpfCrossId);
                $("#upfCrossDetailId").val($row.find('td:eq(0)').text().trim());
                $('#btnUpdateUpfCrossSum').show();
                $('#btnAddUpfCrossSum').hide();
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

$("#btnUpdateUpfCrossSum").click(function () {
    if ($("#bodDependWeight").val() == "") {
        $("#bodDependWeight").focus();
        $("#modalError").show();
        return;
    }
    if ($("#bodDependPoint").val() == "") {
        $("#bodDependPoint").focus();
        $("#modalError").show();
        return;
    }
    var upfCrossSum = new Object();
    upfCrossSum.UpfCrossId = $('#upfCrossId').val();
    upfCrossSum.UpfCrossDetailId = $('#upfCrossDetailId').val();
    upfCrossSum.BodDependWeight = $('#bodDependWeight').val();
    upfCrossSum.BodDependPoint = parseFloat($('#bodDependPoint').val());
    upfCrossSum.BodPerforWeight = $('#bodPerforWeight').val();
    upfCrossSum.BodPerforPoint = parseFloat($('#bodPerforPoint').val());
    $.ajax({
        type: "POST",
        url: "/ReportUpfCross/UpfCrossSumApproved",
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