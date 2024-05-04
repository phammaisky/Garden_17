// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-peformance');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-peformance');
    subMenuItem.addClass('active');

    // Binding On Year Complete Work table
    //FirstLoat or Not
    $("#performance-block").hide();
    $("#manageComment").hide();
    $("#notApproveID").hide();
    $("#approveID").hide();
    if ($("#FirstLoad").val() === "True") {
        $("#ErrorMessage").text("");
        $("#error-block").hide();
        $("#performance-block").hide();
        $("#PerformanceLsfb_ProjectId").attr("disabled", false);
        $("#PerformanceLsfb_TypePerformanceId").attr("disabled", false);
        $("#PerformanceLsfb_Type").attr("disabled", false);
        $("#PerformanceLsfb_Year").attr("disabled", false);
        if ($("#PerformanceLsfb_Type").val() == "0") {
            $("#PerformanceLsfb_Month").attr("disabled", false);
        } else {
            $("#PerformanceLsfb_QuarterId").attr("disabled", false);
        }
        $("#btnStart").attr("disabled", false);
        $("#btnReset").attr("disabled", false);

        $('#performance-fb-id').hide();
        $('#performance-ls-op-id').hide();
        $('#performance-garden-fb-id').hide();
        $('#performance-garden-ls-id').hide();
        $('#performance-manor-id').hide();
        $("#per_ScheduleID").hide();
        $("#per_QuarterID").hide();
        if ($("#PerformanceLsfb_ShowFormByProjType").val() === "3") {
            $("#PerformanceLsfb_OfficeArea").attr("disabled", false);
            $("#PerformanceLsfb_OfficeMonthMoney").attr("disabled", false);
            $("#PerformanceLsfb_OfficeTY").attr("disabled", false);
            $("#PerformanceLsfb_OfficeLY").attr("disabled", false);
            $("#PerformanceLsfb_RetailArea").attr("disabled", false);
            $("#PerformanceLsfb_RetailMonthMoney").attr("disabled", false);
            $("#PerformanceLsfb_RetailTY").attr("disabled", false);
            $("#PerformanceLsfb_RetailLY").attr("disabled", false);
            $("#PerformanceLsfb_NewArea").attr("disabled", false);
            $("#PerformanceLsfb_NewMonthMoney").attr("disabled", false);
            $("#PerformanceLsfb_NewTotalRev").attr("disabled", false);
            $("#PerformanceLsfb_TotalMonthMoney").attr("disabled", false);
            $("#PerformanceLsfb_TotalRevTY").attr("disabled", false);
            $("#PerformanceLsfb_TotalRevLY").attr("disabled", false);
            $("#PerformanceLsfb_TotalGrossLY").attr("disabled", false);
            $("#PerformanceLsfb_Note").attr("disabled", false);
        }
        if ($("#PerformanceLsfb_ShowFormByProjType").val() === "2") {
            $("#PerformanceLsfb_TypeFB").attr("disabled", false);
            $("#PerformanceLsfb_SalesLineToLine").attr("disabled", false);
            $("#PerformanceLsfb_SalesAll").attr("disabled", false);
            $("#PerformanceLsfb_SalesCashFlowTY").attr("disabled", false);
            $("#PerformanceLsfb_SalesCashFlowLY").attr("disabled", false);
            $("#PerformanceLsfb_RevLineTOSNoMG").attr("disabled", false);
            $("#PerformanceLsfb_RevLineTOSWithMG").attr("disabled", false);
            $("#PerformanceLsfb_RevLineNoMG").attr("disabled", false);
            $("#PerformanceLsfb_RevLineWithMG").attr("disabled", false);
            $("#PerformanceLsfb_RevAllTOSNoMG").attr("disabled", false);
            $("#PerformanceLsfb_RevAllTOSWithMG").attr("disabled", false);
            $("#PerformanceLsfb_RevAllNoMG").attr("disabled", false);
            $("#PerformanceLsfb_RevAllWithMG").attr("disabled", false);
            $("#PerformanceLsfb_RevAllLY").attr("disabled", false);
            $("#PerformanceLsfb_RevAllOPMonthMoney").attr("disabled", false);
            $("#PerformanceLsfb_RevTotalNoMG").attr("disabled", false);
            $("#PerformanceLsfb_RevTotalWithMG").attr("disabled", false);
            $("#PerformanceLsfb_RevTotalLY").attr("disabled", false);
            $("#PerformanceLsfb_ComProfitTY").attr("disabled", false);
            $("#PerformanceLsfb_ComProfitLY").attr("disabled", false);
        }
        if ($("#PerformanceLsfb_ShowFormByProjType").val() === "1") {
            $("#PerformanceLsfb_TypeFBLS").attr("disabled", false);
            $("#PerformanceLsfb_SalesLineToLineLS").attr("disabled", false);
            $("#PerformanceLsfb_SalesAllLS").attr("disabled", false);
            $("#PerformanceLsfb_SalesCashFlowTYLS").attr("disabled", false);
            $("#PerformanceLsfb_SalesCashFlowLYLS").attr("disabled", false);
            $("#PerformanceLsfb_RevLSArea").attr("disabled", false);
            $("#PerformanceLsfb_RevLSMonthMoney").attr("disabled", false);
            $("#PerformanceLsfb_RevLSRev").attr("disabled", false);
            $("#PerformanceLsfb_RevLSOPMonthMoney").attr("disabled", false);
            $("#PerformanceLsfb_RevTotalNoMGLS").attr("disabled", false);
            $("#PerformanceLsfb_RevTotalWithMGLS").attr("disabled", false);
            $("#PerformanceLsfb_RevTotalLYLS").attr("disabled", false);
            $("#PerformanceLsfb_ComProfitTYLS").attr("disabled", false);
            $("#PerformanceLsfb_ComProfitLYLS").attr("disabled", false);
        }
    }
    else if ($("#FirstLoad").val() === "False") {
        if ($("#ErrorMessage").text().length != 0) {
            $("#error-block").show();
        } else {
            $("#error-block").hide();
        }
        
        $("#performance-block").show();

        $("#PerformanceLsfb_ApprovedBy").attr("disabled", true);
        $("#PerformanceLsfb_ProjectId").attr("disabled", true);
        $("#PerformanceLsfb_TypePerformanceId").attr("disabled", true);
        $("#PerformanceLsfb_Type").attr("disabled", true);
        $("#PerformanceLsfb_Year").attr("disabled", true);
        if ($("#PerformanceLsfb_Type").val() == "0") {
            $("#PerformanceLsfb_Month").attr("disabled", true);
        } else {
            $("#PerformanceLsfb_QuarterId").attr("disabled", true);
        }

        $("#btnStart").attr("disabled", true);
        if ($("#performanceHiddenId").val() != 0) {
            $("#btnReset").attr("disabled", true);
        }

        if ($("#PerformanceLsfb_Type").val() == "0") {
            $('#per_ScheduleID').show();
            $('#per_QuarterID').hide();
        } else {
            $('#per_ScheduleID').hide();
            $('#per_QuarterID').show();
        }
        if ($("#PerformanceLsfb_ShowFormByProjType").val() === "1") {
            $('#performance-manor-id').hide();
            $('#performance-garden-ls-id').show();
            $('#performance-garden-fb-id').hide();
        }
        if ($("#PerformanceLsfb_ShowFormByProjType").val() === "2") {
            $('#performance-manor-id').hide();
            $('#performance-garden-ls-id').hide();
            $('#performance-garden-fb-id').show();
        }
        if ($("#PerformanceLsfb_ShowFormByProjType").val() === "3") {
            $('#performance-manor-id').show();
            $('#performance-garden-ls-id').hide();
            $('#performance-garden-fb-id').hide();
        }
        if ($("#isApprove").val() == 'True') {
            $("#manageComment").show();
            $("#completeID").hide();
            $("#approveID").show();
            $("#notApproveID").show();
            if ($("#PerformanceLsfb_ShowFormByProjType").val() === "3") {
                $("#PerformanceLsfb_OfficeArea").attr("disabled", true);
                $("#PerformanceLsfb_OfficeMonthMoney").attr("disabled", true);
                $("#PerformanceLsfb_OfficeTY").attr("disabled", true);
                $("#PerformanceLsfb_OfficeLY").attr("disabled", true);
                $("#PerformanceLsfb_RetailArea").attr("disabled", true);
                $("#PerformanceLsfb_RetailMonthMoney").attr("disabled", true);
                $("#PerformanceLsfb_RetailTY").attr("disabled", true);
                $("#PerformanceLsfb_RetailLY").attr("disabled", true);
                $("#PerformanceLsfb_NewArea").attr("disabled", true);
                $("#PerformanceLsfb_NewMonthMoney").attr("disabled", true);
                $("#PerformanceLsfb_NewTotalRev").attr("disabled", true);
                $("#PerformanceLsfb_TotalMonthMoney").attr("disabled", true);
                $("#PerformanceLsfb_TotalRevTY").attr("disabled", true);
                $("#PerformanceLsfb_TotalRevLY").attr("disabled", true);
                $("#PerformanceLsfb_TotalGrossLY").attr("disabled", true);
                $("#PerformanceLsfb_Note").attr("disabled", true);
            }
            if ($("#PerformanceLsfb_ShowFormByProjType").val() === "2") {
                $("#PerformanceLsfb_TypeFB").attr("disabled", true);
                $("#PerformanceLsfb_SalesLineToLine").attr("disabled", true);
                $("#PerformanceLsfb_SalesAll").attr("disabled", true);
                $("#PerformanceLsfb_SalesCashFlowTY").attr("disabled", true);
                $("#PerformanceLsfb_SalesCashFlowLY").attr("disabled", true);
                $("#PerformanceLsfb_RevLineTOSNoMG").attr("disabled", true);
                $("#PerformanceLsfb_RevLineTOSWithMG").attr("disabled", true);
                $("#PerformanceLsfb_RevLineNoMG").attr("disabled", true);
                $("#PerformanceLsfb_RevLineWithMG").attr("disabled", true);
                $("#PerformanceLsfb_RevAllTOSNoMG").attr("disabled", true);
                $("#PerformanceLsfb_RevAllTOSWithMG").attr("disabled", true);
                $("#PerformanceLsfb_RevAllNoMG").attr("disabled", true);
                $("#PerformanceLsfb_RevAllWithMG").attr("disabled", true);
                $("#PerformanceLsfb_RevAllLY").attr("disabled", true);
                $("#PerformanceLsfb_RevAllOPMonthMoney").attr("disabled", true);
                $("#PerformanceLsfb_RevTotalNoMG").attr("disabled", true);
                $("#PerformanceLsfb_RevTotalWithMG").attr("disabled", true);
                $("#PerformanceLsfb_RevTotalLY").attr("disabled", true);
                $("#PerformanceLsfb_ComProfitTY").attr("disabled", true);
                $("#PerformanceLsfb_ComProfitLY").attr("disabled", true);
            }
            if ($("#PerformanceLsfb_ShowFormByProjType").val() === "1") {
                $("#PerformanceLsfb_TypeFBLS").attr("disabled", true);
                $("#PerformanceLsfb_SalesLineToLineLS").attr("disabled", true);
                $("#PerformanceLsfb_SalesAllLS").attr("disabled", true);
                $("#PerformanceLsfb_SalesCashFlowTYLS").attr("disabled", true);
                $("#PerformanceLsfb_SalesCashFlowLYLS").attr("disabled", true);
                $("#PerformanceLsfb_RevLSArea").attr("disabled", true);
                $("#PerformanceLsfb_RevLSMonthMoney").attr("disabled", true);
                $("#PerformanceLsfb_RevLSRev").attr("disabled", true);
                $("#PerformanceLsfb_RevLSOPMonthMoney").attr("disabled", true);
                $("#PerformanceLsfb_RevTotalNoMGLS").attr("disabled", true);
                $("#PerformanceLsfb_RevTotalWithMGLS").attr("disabled", true);
                $("#PerformanceLsfb_RevTotalLYLS").attr("disabled", true);
                $("#PerformanceLsfb_ComProfitTYLS").attr("disabled", true);
                $("#PerformanceLsfb_ComProfitLYLS").attr("disabled", true);
            }
        }
    }
});
// #endregion PageLoaded


// #region Info
function projectSearchChange(val) {
    $.ajax({
        type: "POST",
        url: "/Performance/ProjectChange",
        data: "projectId=" + JSON.stringify(val),
        success: function (response) {
            if (response != null) {
                $("#PerformanceLsfb_TypePerformanceId").html(response);
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            $("#PerformanceLsfb_TypePerformanceId").html(response.responseText);
        }
    });
}

function scheduleChange(val) {
    if (val == "0") {
        $('#per_ScheduleID').show();
        $('#per_QuarterID').hide();
    } else if (val == "1") {
        $('#per_ScheduleID').hide();
        $('#per_QuarterID').show();
    } else {
        $('#per_ScheduleID').hide();
        $('#per_QuarterID').hide();
    }
}

$("#btnStart").click(function startCreate() {
    if ($("#PerformanceLsfb_ProjectId").val() == "") {
        $("#PerformanceLsfb_ProjectId").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Dự án");
        $("#error-block").show();
        return;
    }
    if ($("#PerformanceLsfb_TypePerformanceId").val() == "") {
        $("#PerformanceLsfb_TypePerformanceId").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Loại hiệu suất");
        $("#error-block").show();
        return;
    }
    if ($("#PerformanceLsfb_Type").val() == "") {
        $("#PerformanceLsfb_Type").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Loại");
        $("#error-block").show();
        return;
    }
    if ($("#PerformanceLsfb_Year").val() == "") {
        $("#PerformanceLsfb_Year").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Năm");
        $("#error-block").show();
        return;
    }
    if ($("#PerformanceLsfb_Type").val() == "0" && $("#PerformanceLsfb_Month").val() == "") {
        $("#PerformanceLsfb_Month").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Tháng");
        $("#error-block").show();
        return;
    }
    if ($("#PerformanceLsfb_Type").val() == "1" && $("#PerformanceLsfb_QuarterId").val() == "") {
        $("#PerformanceLsfb_QuarterId").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Quý");
        $("#error-block").show();
        return;
    }

    var performanceLsfb = new Object();
    performanceLsfb.ProjectId = $("#PerformanceLsfb_ProjectId").val();
    performanceLsfb.TypePerformanceId = $("#PerformanceLsfb_TypePerformanceId").val();
    performanceLsfb.Type = $("#PerformanceLsfb_Type").val();
    performanceLsfb.Year = $("#PerformanceLsfb_Year").val();
    if ($("#PerformanceLsfb_Type").val() == "0") {
        performanceLsfb.Month = $("#PerformanceLsfb_Month").val();
    } else {
        performanceLsfb.QuarterId = $("#PerformanceLsfb_QuarterId").val();
    }

    $.ajax({
        type: "POST",
        url: "/Performance/Start",
        data: JSON.stringify(performanceLsfb),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                if (response.CheckExistsPerformance == false) {
                    var error = "Bạn chỉ tạo được bản ghi một tháng một lần trong năm.";
                    $("#ErrorMessage").text(error);
                    $("#error-block").show();
                } else {
                    $("#ErrorMessage").text("");
                    $("#error-block").hide();
                    $("#performance-block").show();
                    $("#PerformanceLsfb_ApprovedBy").attr("disabled", true);
                    $("#PerformanceLsfb_ProjectId").attr("disabled", true);
                    $("#PerformanceLsfb_TypePerformanceId").attr("disabled", true);
                    $("#PerformanceLsfb_Type").attr("disabled", true);
                    $("#PerformanceLsfb_Year").attr("disabled", true);
                    if ($("#PerformanceLsfb_Type").val() == "0") {
                        $("#PerformanceLsfb_Month").attr("disabled", true);
                    } else {
                        $("#PerformanceLsfb_QuarterId").attr("disabled", true);
                    }
                    $("#btnStart").attr("disabled", true);
                    $("#PerformanceLsfb_ShowFormByProjType").val(response.ShowFormByProjType);
                    if (response.ShowFormByProjType === 1) {
                        $('#performance-manor-id').hide();
                        $('#performance-garden-ls-id').show();
                        $('#performance-garden-fb-id').hide();
                    }
                    if (response.ShowFormByProjType === 2) {
                        $('#performance-manor-id').hide();
                        $('#performance-garden-ls-id').hide();
                        $('#performance-garden-fb-id').show();
                    }
                    if (response.ShowFormByProjType === 3) {
                        $('#performance-manor-id').show();
                        $('#performance-garden-ls-id').hide();
                        $('#performance-garden-fb-id').hide();
                    }

                }
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

$("#btnReset").click(function resetCreate() {
    $.ajax({
        type: "GET",
        url: "/Performance/Reset",
        data: null,
        success: function (response) {
            if (response != null) {
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

function onChangeTypeFB(val) {
    if (val == "0") {
        $('#performance-fb-id').hide();
    } else {
        $('#performance-fb-id').show();
    }
}

function onChangeTypeLS(val) {
    if (val == "0") {
        $('#performance-ls-op-id').hide();
    } else {
        $('#performance-ls-op-id').show();
    }
}
// #endregion Info

// #region Modal
function validateForm() {
    var showFormByProjType = $("#PerformanceLsfb_ShowFormByProjType").val();
    if (showFormByProjType == "1" && $("#PerformanceLsfb_TypeFBLS").val() == "") {
        $("#PerformanceLsfb_TypeFBLS").focus();
        $("#ErrorMessage").text("Loại FB không được để trống!");
        $("#error-block").show();
        return false;
    }
    if (showFormByProjType == "2" && $("#PerformanceLsfb_TypeFB").val() == "") {
        $("#PerformanceLsfb_TypeFB").focus();
        $("#ErrorMessage").text("Loại FB không được để trống!");
        $("#error-block").show();
        return false;
    }

    return true;
}
// #endregion Modal