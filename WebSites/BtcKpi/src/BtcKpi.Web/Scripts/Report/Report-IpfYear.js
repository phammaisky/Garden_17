// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-report');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-report-ipf-year');
    subMenuItem.addClass('active');

    $("#ErrorMessage").text("");
    $("#error-block").hide();

    $("#btnSearch").click();
    

});
// #endregion PageLoaded
$("#btnSearch").click(function startCreate() {
    var reportSearch = new Object();
    reportSearch.Year = $("#Year").val();
    if (reportSearch.Year.length === 0) {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        $("#ErrorMessage").text("Vui lòng chọn năm !");
        $("#error-block").show();
        return;
    } else {
        $("#ErrorMessage").text("");
        $("#error-block").hide();
    }

    reportSearch.CompanyID = $("#CompanyID").val();
    if (reportSearch.CompanyID.length === 0) {
        var companyID = "";
        $("#CompanyID option").each(function () {
            companyID = companyID + $(this).val() + ",";
        });
        reportSearch.CompanyID = companyID;
    }

    reportSearch.DepartmentID = $("#DepartmentID").val();
    if (reportSearch.DepartmentID.length === 0) {
        var departmentIDs = "";
        $("#FromDepartmentID option").each(function () {
            departmentIDs = departmentIDs + $(this).val() + ",";
        });
        reportSearch.DepartmentID = departmentIDs;
    }

    if (reportSearch.Year.length === 0) {
        var year = "";
        $("#Year option").each(function () {
            year = year + $(this).val() + ",";
        });
        reportSearch.Year = year;
    }
    $.ajax({
        type: "POST",
        url: "/Report/IpfYearSearch",
        data: JSON.stringify(reportSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            if (response !== null) {
                if ($("#ipf-year-table").html().trim().length !== 0) {
                    $('#ipf-year-table').dataTable().fnDestroy();
                }
                $("#ipf-year-table").html(response);
                get_ajax_table();
                set_caculate_table();

            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            //alert(response.responseText);
            if ($("#ipf-year-table").html().trim().length !== 0) {
                $('#ipf-year-table').dataTable().fnDestroy();
            }
            $("#ipf-year-table").html(response.responseText);
            get_ajax_table();
        }
    });
});

function get_ajax_table() {
    $('#ipf-year-table').dataTable({
        "paging": false,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">',
        createdRow: function (row, data, dataIndex) {
            // If is Work Title
            if (data[2] === 'True') {
                // Add COLSPAN attribute
                $('td:eq(1)', row).attr('colspan', 28);

                // Left horizontally
                $('td:eq(1)', row).attr('align', 'left');

                // Hide required number of columns
                // next to the cell with COLSPAN attribute
                $('td:eq(2)', row).css('display', 'none');
                $('td:eq(3)', row).css('display', 'none');
                $('td:eq(4)', row).css('display', 'none');
                $('td:eq(5)', row).css('display', 'none');
                $('td:eq(6)', row).css('display', 'none');
                $('td:eq(7)', row).css('display', 'none');
                $('td:eq(8)', row).css('display', 'none');
                $('td:eq(9)', row).css('display', 'none');
                $('td:eq(10)', row).css('display', 'none');
                $('td:eq(11)', row).css('display', 'none');
                $('td:eq(12)', row).css('display', 'none');
                $('td:eq(13)', row).css('display', 'none');
                $('td:eq(14)', row).css('display', 'none');
                $('td:eq(15)', row).css('display', 'none');
                $('td:eq(16)', row).css('display', 'none');
                $('td:eq(17)', row).css('display', 'none');
                $('td:eq(18)', row).css('display', 'none');
                $('td:eq(19)', row).css('display', 'none');
                $('td:eq(20)', row).css('display', 'none');
                $('td:eq(21)', row).css('display', 'none');
                $('td:eq(22)', row).css('display', 'none');
                $('td:eq(23)', row).css('display', 'none');
                $('td:eq(24)', row).css('display', 'none');
                $('td:eq(25)', row).css('display', 'none');
                $('td:eq(26)', row).css('display', 'none');
                $('td:eq(27)', row).css('display', 'none');
                $('td:eq(28)', row).css('display', 'none');

                // Update cell data
            }
        }
    });
}

function set_caculate_table() {

    var $bodscore_fields = $('input[type="number"][id*="_BodScore"]');
    $bodscore_fields.each(function() {
        $(this).on('input',function (e) {
            var nextCell = $(this).parent().next();
            var value = $(this).val();
            if (!isNaN(value) && value > 0) {
                if (parseFloat(value) >= 10) {
                    nextCell.html('A+');
                }
                if (parseFloat(value) < 10 & parseFloat(value) >= 8) {
                    nextCell.html('A');
                }
                if (parseFloat(value) < 8 & parseFloat(value) >= 6) {
                    nextCell.html('B+');
                }
                if (parseFloat(value) < 6 & parseFloat(value) >= 4) {
                    nextCell.html('B');
                }
                if (parseFloat(value) <= 4) {
                    nextCell.html('C');
                } 
            }
        });
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

// #endregion PageLoaded
$("#btnExcel").click(function startCreate() {
    var reportSearch = new Object();
    reportSearch.Year = $("#Year").val();
    if (reportSearch.Year.length === 0) {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        $("#ErrorMessage").text("Vui lòng chọn năm !");
        $("#error-block").show();
        return;
    } else {
        $("#ErrorMessage").text("");
        $("#error-block").hide();
    }

    reportSearch.CompanyID = $("#CompanyID").val();
    if (reportSearch.CompanyID.length === 0) {
        var companyID = "";
        $("#CompanyID option").each(function () {
            companyID = companyID + $(this).val() + ",";
        });
        reportSearch.CompanyID = companyID;
    }

    reportSearch.DepartmentID = $("#DepartmentID").val();
    if (reportSearch.DepartmentID.length === 0) {
        var departmentIDs = "";
        $("#FromDepartmentID option").each(function () {
            departmentIDs = departmentIDs + $(this).val() + ",";
        });
        reportSearch.DepartmentID = departmentIDs;
    }

    if (reportSearch.Year.length === 0) {
        var year = "";
        $("#Year option").each(function () {
            year = year + $(this).val() + ",";
        });
        reportSearch.Year = year;
    }
    $.ajax({
        type: "POST",
        url: "/Report/ExportToExcel",
        data: JSON.stringify(reportSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response !== null) {
                //use window.location.href for redirect to download action for download the file
                // ReSharper disable once QualifiedExpressionMaybeNull
                window.location.href = "/Report/Download?file=" + response.fileName;
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



