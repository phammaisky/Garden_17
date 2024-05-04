$(function () {
    var menuItem = $('#left-sidebar-menu-ipf');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-ipf-peding');
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
        "paging": false,
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
                $('td:eq(4)', row).css('display', 'none');

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

function configDatepicker() {
    ////Date picker
    $('.complete-date').datepicker({
        format: 'dd/mm/yyyy'
    });

    //Alternativ way
    $('.complete-date').datepicker({
        format: "dd/mm/yyyy"
    }).on('change', function () {
        $('.datepicker').hide();
    });
}

function savePersonalPlan() {
    var careers = new Array();
    $("#ipf-personal-plan-careeer tbody").find('tr').each(function () {
        var carrer = {};
        carrer.Seq = $(this).find("td:eq(0) input[name*='Seq']").val();
        carrer.WishesOfStaff = $(this).find("td:eq(2) textarea").val();
        carrer.RequestOfManager = $(this).find("td:eq(3) textarea").val();
        careers.push(carrer);
    });
    $.ajax({
        type: "POST",
        url: "/Ipf/SavePersonalPlanCareer",
        data: JSON.stringify(careers),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                return;
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function saveCompetency() {
    var competencies = new Array();
    $("#ipf-competency tbody").find('tr').each(function () {
        var competency = {};
        competency.Seq = $(this).find("td:eq(0) input[name*='Seq']").val();
        competency.Weight = $(this).find("td:eq(3) input").val();
        competency.SelfScore = $(this).find("td:eq(4) input").val();
        competency.IsNextYear = 0;
        competencies.push(competency);
    });
    $.ajax({
        type: "POST",
        url: "/Ipf/SaveCompetency",
        data: JSON.stringify(competencies),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $("#lblCompetencyWeightToal").text(response.Weight);
                $("#CompetencyWeightToal").val(response.Weight);
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function saveCompetencyNextYear() {
    var competencies = new Array();
    $("#ipf-competency-ny tbody").find('tr').each(function () {
        var competency = {};
        competency.Seq = $(this).find("td:eq(0) input[name*='Seq']").val();
        competency.Weight = $(this).find("td:eq(3) input").val();
        competency.SelfScore = $(this).find("td:eq(4) input").val();
        competency.IsNextYear = 1;
        competencies.push(competency);
    });
    $.ajax({
        type: "POST",
        url: "/Ipf/SaveCompetency",
        data: JSON.stringify(competencies),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $("#lblCompetencyNextWeightToal").text(response.Weight);
                $("#CompetencyNextWeightToal").val(response.Weight);
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

//// #region Tab
//$("#ipfTabs > li").click(function (e) {    //ul.navbar-nav > li
//    e.preventDefault();
//    var tabId = jQuery(this).attr("id");
//    var currentTabs = $("#ipfTabs > li.active").attr('id')
//    if (currentTabs == 'liPersonalPlan' & tabId != 'liPersonalPlan') {
//        savePersonalPlan();
//    }

//    $.ajax({
//        type: "GET",
//        url: "/Ipf/TabClick",
//        data: { tabID: tabId },
//        success: function (response) {
//            if (response != null) {

//            } else {
//                alert("Lỗi post Tab click!");
//            }
//        },
//        failure: function (response) {
//            alert(response.responseText);
//        },
//        error: function (response) {
//            alert(response.responseText);
//        }
//    });
//});
//// #endregion Tab

function bindingDetailTablesView() {
    // Binding On Year Complete Work table
    var completeWork = $("#ipf-complete-work").DataTable({
        "paging": false,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">',
        createdRow: function (row, data, dataIndex) {
            // If is complete title
            if (data[0] === 'True') {
                // Add COLSPAN attribute
                $('td:eq(1)', row).attr('colspan', 9);

                // Left horizontally
                $('td:eq(1)', row).attr('align', 'left');

                // Hide required number of columns
                // next to the cell with COLSPAN attribute
                $('td:eq(0)', row).css('display', 'none');
                $('td:eq(2)', row).css('display', 'none');
                $('td:eq(3)', row).css('display', 'none');
                $('td:eq(4)', row).css('display', 'none');
                $('td:eq(5)', row).css('display', 'none');
                $('td:eq(6)', row).css('display', 'none');
                $('td:eq(7)', row).css('display', 'none');
                $('td:eq(8)', row).css('display', 'none');

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

    if ($("#Ipf_ScheduleType").val() == "0") {
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
                    $('td:eq(4)', row).css('display', 'none');

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
        $("#ScheduleID option").each(function () {
            scheduleID = scheduleID + $(this).val() + ",";
        });
        ipfSearch.ScheduleID = scheduleID;
    }

    ipfSearch.FullName = $("#FullName").val();

    $.ajax({
        type: "POST",
        url: "/Ipf/SearchPending",
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

//$(".Competencies_Weight").blur(function () {
//    alert($(this).val());
//});

//$(".CompetenciesNextYear_Weight").blur(function () {
//    alert($(this).val());
//});
