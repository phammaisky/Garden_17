// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-report');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-report-upf');
    subMenuItem.addClass('active');

    $("#ErrorMessage").text("");
    $("#error-block").hide();

    $("#btnSearch").click();
    //$('.content-header').hide();
});
// #endregion PageLoaded

$("#btnSearch").click(function startCreate() {

    var reportSearch = new Object();

    reportSearch.CompanyID = $("#CompanyID").val();
    if (reportSearch.CompanyID.length == 0) {
        var companyID = "";
        $("#CompanyID option").each(function () {
            companyID = companyID + $(this).val() + ",";
        });
        reportSearch.CompanyID = companyID;
    }

    reportSearch.DepartmentID = $("#DepartmentID").val();
    if (reportSearch.DepartmentID.length == 0) {
        var departmentIDs = "";
        $("#FromDepartmentID option").each(function() {
            departmentIDs = departmentIDs + $(this).val() + ",";
        });
        reportSearch.DepartmentID = departmentIDs;
    }

    reportSearch.Year = $("#Year").val();
    if (reportSearch.Year.length == 0) {
        var year = "";
        $("#Year option").each(function () {
            year = year + $(this).val() + ",";
        });
        reportSearch.Year = year;
    }

    reportSearch.ScheduleID = $("#ScheduleID").val();
    if (reportSearch.ScheduleID.length == 0) {
        var scheduleIds = "";
        $("#ScheduleID option").each(function() {
            scheduleIds = scheduleIds + $(this).val() + ",";
        });
        reportSearch.ScheduleID = scheduleIds;
    }

    debugger;
    $.ajax({
        type: "POST",
        url: "/Report/UpfMonthSearch",
        data: JSON.stringify(reportSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            if (response != null) {
                debugger;
                $('#upf-month-table').dataTable().fnDestroy();
                $("#upf-month-table").html(response);
                get_ajax_table();

            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            //alert(response.responseText);
            debugger;
            $('#upf-month-table').dataTable().fnDestroy();
            $("#upf-month-table").html(response.responseText);
            get_ajax_table();
        }
    });
});

function get_ajax_table() {
    $('#upf-month-table').dataTable({
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">'
    });
}

$("#btnMonth").click(function startCreate() {

    var reportSearch = new Object();

    reportSearch.CompanyID = $("#CompanyID").val();
    if (reportSearch.CompanyID.length === 0) {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        $("#ErrorMessage").text("Vui lòng chọn công ty!");
        $("#error-block").show();
    }

    reportSearch.Year = $("#Year").val();
    if (reportSearch.Year.length === 0) {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        $("#ErrorMessage").text("Vui lòng chọn năm !");
        $("#error-block").show();
    }

    
    if (reportSearch.CompanyID.length !== 0 & reportSearch.Year.length !== 0) {
        var url = $("#RedirectToMonth").val() + '?company=' + reportSearch.CompanyID + '&year=' + reportSearch.Year;
        location.href = url;
    }
});

$("#btnYear").click(function startCreate() {

    var reportSearch = new Object();

    reportSearch.CompanyID = $("#CompanyID").val();
    if (reportSearch.CompanyID.length === 0) {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        $("#ErrorMessage").text("Vui lòng chọn công ty!");
        $("#error-block").show();
    }

    reportSearch.Year = $("#Year").val();
    if (reportSearch.Year.length === 0) {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        $("#ErrorMessage").text("Vui lòng chọn năm !");
        $("#error-block").show();
    }


    if (reportSearch.CompanyID.length !== 0 & reportSearch.Year.length !== 0) {
        var url = $("#RedirectToYear").val() + '?company=' + reportSearch.CompanyID + '&year=' + reportSearch.Year;
        location.href = url;
    }
});