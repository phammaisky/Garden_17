$(function () {
    var menuItem = $('#left-sidebar-menu-ipf');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-ipf-done');
    subMenuItem.addClass('active');

    var ipfPending = $("#ipf-pending-table").DataTable({
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    //First time -> Get All
    $("#btnSearch").click();

});

function bindingDetailTables() {
    // Binding On Year Complete Work table
    var completeWork = $("#ipf-complete-work").DataTable({
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">',
        createdRow: function (row, data, dataIndex) {
            // If is complete title
            if (data[0] === 'True') {
                // Add COLSPAN attribute
                $('td:eq(1)', row).attr('colspan', 6);

                // Left horizontally
                $('td:eq(1)', row).attr('align', 'left');

                // Hide required number of columns
                // next to the cell with COLSPAN attribute
                $('td:eq(0)', row).css('display', 'none');
                $('td:eq(2)', row).css('display', 'none');
                $('td:eq(3)', row).css('display', 'none');
                $('td:eq(4)', row).css('display', 'none');
                $('td:eq(5)', row).css('display', 'none');
                // Update cell data
            }
        }
    });

    // Binding On Year Competency table
    var competency = $("#ipf-competency").DataTable({
        "paging": false,
        "ordering": false,
        "searching": false,
        "info": false
    });

    // Binding Personal Plan Competency
    var personalPlanCompetency = $("#ipf-personal-plan-competency").DataTable({
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    // Binding Personal Plan Career
    var personalPlanCareer = $("#ipf-personal-plan-careeer").DataTable({
        "paging": false,
        "ordering": false,
        "searching": false,
        "info": false
    });

    // Binding Next Year Complete Work table
    var completeWorkNextYear = $("#ipf-complete-work-ny").DataTable({
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">',
        createdRow: function (row, data, dataIndex) {
            // If is complete work title
            if (data[0] === 'True') {
                // Add COLSPAN attribute
                $('td:eq(1)', row).attr('colspan', 5);

                // Left horizontally
                $('td:eq(1)', row).attr('align', 'left');

                // Hide required number of columns
                // next to the cell with COLSPAN attribute
                $('td:eq(0)', row).css('display', 'none');
                $('td:eq(2)', row).css('display', 'none');
                $('td:eq(3)', row).css('display', 'none');

                // Update cell data
            }
        }
    });

    // Binding Next Year Competency table
    var competencyNextYear = $("#ipf-competency-ny").DataTable({
        "paging": false,
        "ordering": false,
        "searching": false,
        "info": false
    });
}

function companySearchChange(val) {
    $.ajax({
        type: "POST",
        url: "/Ipf/CompanyChange",
        data: "companyId=" + JSON.stringify(val), //'{companyId: "' + val + '" }',
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
            //alert(response.responseText);
            $("#DepartmentID").html(response.responseText);
        }
    });
}

function scheduleSearchChange(val) {
    if (val == "0") {
        $('#dIpf_ScheduleID').hide();
    } else {
        $('#dIpf_ScheduleID').show();
    }
}

$("#btnSearch").click(function startCreate() {

    var ipfSearch = new Object();
    ipfSearch.Status = $("#Status").val();
    ipfSearch.CompanyID = $("#CompanyID").val();
    if (ipfSearch.CompanyID.length == 0) {
        var companyID = "";
        $("#CompanyID option").each(function () {
            companyID = companyID + $(this).val() + ",";
        });
        ipfSearch.CompanyID = companyID;
    }

    ipfSearch.DepartmentID = $("#DepartmentID").val();
    if (ipfSearch.DepartmentID.length == 0) {
        var departmentID = "";
        $("#DepartmentID option").each(function () {
            departmentID = departmentID + $(this).val() + ",";
        });
        ipfSearch.DepartmentID = departmentID;
    }

    ipfSearch.ScheduleType = $("#ScheduleType").val();
    if (ipfSearch.ScheduleType.length == 0) {
        var scheduleType = "";
        $("#ScheduleType option").each(function () {
            scheduleType = scheduleType + $(this).val() + ",";
        });
        ipfSearch.ScheduleType = scheduleType;
    }

    ipfSearch.Year = $("#Year").val();
    if (ipfSearch.Year.length == 0) {
        var year = "";
        $("#Year option").each(function () {
            year = year + $(this).val() + ",";
        });
        ipfSearch.Year = year;
    }

    ipfSearch.ScheduleID = $("#ScheduleID").val();
    if (ipfSearch.ScheduleID.length == 0) {
        var scheduleID = "";
        $("#ScheduleType option").each(function () {
            scheduleID = scheduleID + $(this).val() + ",";
        });
        ipfSearch.ScheduleID = scheduleID;
    }

    ipfSearch.FullName = $("#FullName").val();

    $.ajax({
        type: "POST",
        url: "/Ipf/SearchDone",
        data: JSON.stringify(ipfSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $("#ipf-pending-table").html(response);

            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            //alert(response.responseText);
            $("#ipf-pending-table").html(response.responseText);
        }
    });


});
