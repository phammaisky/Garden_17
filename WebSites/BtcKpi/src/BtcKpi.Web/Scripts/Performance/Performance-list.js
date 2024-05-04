// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-peformance');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-peformance');
    subMenuItem.addClass('active');

    $("#error-block").hide();
    if ($('#ProjectID option:selected').text().indexOf("Manor") != -1) {
        $("#performance-table-header-ls").hide();
        $("#performance-table-header-ls-row2").hide();
        $("#performance-table-header-ls-row3").hide();
        $("#performance-table-header-ls-row4").hide();
        $("#performance-table-header-ls-row5").hide();
        $("#performance-table-header-fb").hide();
        $("#performance-table-header-fb-row2").hide();
        $("#performance-table-header-fb-row3").hide();
        $("#performance-table-header-fb-row4").hide();
        $("#performance-table-header-fb-row5").hide();
        $("#performance-table-header-manor").show();
        $("#performance-table-header-manor-row2").show();
        $("#performance-table-header-manor-row3").show();
    } else if ($('#ProjectID option:selected').text().indexOf("Garden") != -1 && $('#TypePerformanceID option:selected').text().indexOf("Leasing") != -1) {
        $("#performance-table-header-ls").show();
        $("#performance-table-header-ls-row2").show();
        $("#performance-table-header-ls-row3").show();
        $("#performance-table-header-ls-row4").show();
        $("#performance-table-header-ls-row5").show();
        $("#performance-table-header-fb").hide();
        $("#performance-table-header-fb-row2").hide();
        $("#performance-table-header-fb-row3").hide();
        $("#performance-table-header-fb-row4").hide();
        $("#performance-table-header-fb-row5").hide();
        $("#performance-table-header-manor").hide();
        $("#performance-table-header-manor-row2").hide();
        $("#performance-table-header-manor-row3").hide();
    } else if ($('#ProjectID option:selected').text().indexOf("Garden") != -1 && $('#TypePerformanceID option:selected').text().indexOf("F&B") != -1) {
        $("#performance-table-header-ls").hide();
        $("#performance-table-header-ls-row2").hide();
        $("#performance-table-header-ls-row3").hide();
        $("#performance-table-header-ls-row4").hide();
        $("#performance-table-header-ls-row5").hide();
        $("#performance-table-header-fb").show();
        $("#performance-table-header-fb-row2").show();
        $("#performance-table-header-fb-row3").show();
        $("#performance-table-header-fb-row4").show();
        $("#performance-table-header-fb-row5").show();
        $("#performance-table-header-manor").hide();
        $("#performance-table-header-manor-row2").hide();
        $("#performance-table-header-manor-row3").hide();
        if ($('#TypeFBID option:selected').text().indexOf("FB") != -1) {
            $("#table-header-fb-row3-fb").show();
            $("#table-header-fb-row3-line-fb").show();
            $("#table-header-fb-row3-all-fb").show();
            $("#table-header-fb-row3-none-fb").hide();
            $("#table-header-fb-row3-line-none-fb").hide();
            $("#table-header-fb-row3-all-none-fb").hide();
        } else {
            $("#table-header-fb-row3-fb").hide();
            $("#table-header-fb-row3-line-fb").hide();
            $("#table-header-fb-row3-all-fb").hide();
            $("#table-header-fb-row3-none-fb").show();
            $("#table-header-fb-row3-line-none-fb").show();
            $("#table-header-fb-row3-all-none-fb").show();
        }
    }
    if ($("#TypePerformanceID option:selected").text() === "F&B") {
        $("#performance-type-fb").show();
    } else {
        $("#performance-type-fb").hide();
    }

    $("#btnSearch").click();
});

function projectSearchChange(val) {
    $.ajax({
        type: "POST",
        url: "/Performance/ProjectChange",
        data: "projectId=" + JSON.stringify(val),
        success: function (response) {
            if (response != null) {
                $("#TypePerformanceID").html(response);
                if ($('#ProjectID option:selected').text().indexOf("Manor") != -1) {
                    $("#performance-table-header-ls").hide();
                    $("#performance-table-header-ls-row2").hide();
                    $("#performance-table-header-ls-row3").hide();
                    $("#performance-table-header-ls-row4").hide();
                    $("#performance-table-header-ls-row5").hide();
                    $("#performance-table-header-fb").hide();
                    $("#performance-table-header-fb-row2").hide();
                    $("#performance-table-header-fb-row3").hide();
                    $("#performance-table-header-fb-row4").hide();
                    $("#performance-table-header-fb-row5").hide();
                    $("#performance-type-fb").hide();
                    $("#performance-table-header-manor").show();
                    $("#performance-table-header-manor-row2").show();
                    $("#performance-table-header-manor-row3").show();
                } else if ($('#ProjectID option:selected').text().indexOf("Garden") != -1 && $('#TypePerformanceID option:selected').text().indexOf("Leasing") != -1) {
                    $("#performance-table-header-ls").show();
                    $("#performance-table-header-ls-row2").show();
                    $("#performance-table-header-ls-row3").show();
                    $("#performance-table-header-ls-row4").show();
                    $("#performance-table-header-ls-row5").show();
                    $("#performance-type-fb").hide();
                    $("#performance-table-header-fb").hide();
                    $("#performance-table-header-fb-row2").hide();
                    $("#performance-table-header-fb-row3").hide();
                    $("#performance-table-header-fb-row4").hide();
                    $("#performance-table-header-fb-row5").hide();
                    $("#performance-table-header-manor").hide();
                    $("#performance-table-header-manor-row2").hide();
                    $("#performance-table-header-manor-row3").hide();
                } else if ($('#ProjectID option:selected').text().indexOf("Garden") != -1 && $('#TypePerformanceID option:selected').text().indexOf("F&B") != -1) {
                    $("#performance-table-header-ls").hide();
                    $("#performance-table-header-ls-row2").hide();
                    $("#performance-table-header-ls-row3").hide();
                    $("#performance-table-header-ls-row4").hide();
                    $("#performance-table-header-ls-row5").hide();
                    $("#performance-table-header-fb").show();
                    $("#performance-table-header-fb-row2").show();
                    $("#performance-table-header-fb-row3").show();
                    $("#performance-table-header-fb-row4").show();
                    $("#performance-table-header-fb-row5").show();
                    $("#performance-type-fb").show();
                    $("#performance-table-header-manor").hide();
                    $("#performance-table-header-manor-row2").hide();
                    $("#performance-table-header-manor-row3").hide();
                    if ($('#TypeFBID option:selected').text() === "FB") {
                        $("#table-header-fb-fb").show();
                        $("#table-header-fb-row2-rev-fb").show();
                        $("#table-header-fb-row3-fb").show();
                        $("#table-header-fb-row3-line-fb").show();
                        $("#table-header-fb-row3-all-fb").show();
                        $("#table-header-fb-row4-fb").show();
                        $("#table-header-fb-none-fb").hide();
                        $("#table-header-fb-row2-rev-none-fb").hide();
                        $("#table-header-fb-row3-none-fb").hide();
                        $("#table-header-fb-row3-line-none-fb").hide();
                        $("#table-header-fb-row3-all-none-fb").hide();
                        $("#table-header-fb-row4-none-fb").hide();
                    } else {
                        $("#table-header-fb-none-fb").show();
                        $("#table-header-fb-row2-rev-none-fb").show();
                        $("#table-header-fb-row3-none-fb").show();
                        $("#table-header-fb-row3-line-none-fb").show();
                        $("#table-header-fb-row3-all-none-fb").show();
                        $("#table-header-fb-row4-none-fb").show();
                        $("#table-header-fb-fb").hide();
                        $("#table-header-fb-row2-rev-fb").hide();
                        $("#table-header-fb-row3-fb").hide();
                        $("#table-header-fb-row3-line-fb").hide();
                        $("#table-header-fb-row3-all-fb").hide();
                        $("#table-header-fb-row4-fb").hide();
                    }
                }
                $("#btnSearch").click();
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            $("#TypePerformanceID").html(response.responseText);
        }
    });
}

function onChangeType(val) {
    if ($('#ProjectID option:selected').text().indexOf("Manor") != -1) {
        $("#performance-table-header-ls").hide();
        $("#performance-table-header-ls-row2").hide();
        $("#performance-table-header-ls-row3").hide();
        $("#performance-table-header-ls-row4").hide();
        $("#performance-table-header-ls-row5").hide();
        $("#performance-table-header-fb").hide();
        $("#performance-table-header-fb-row2").hide();
        $("#performance-table-header-fb-row3").hide();
        $("#performance-table-header-fb-row4").hide();
        $("#performance-table-header-fb-row5").hide();
        $("#performance-type-fb").hide();
        $("#performance-table-header-manor").show();
        $("#performance-table-header-manor-row2").show();
        $("#performance-table-header-manor-row3").show();
    }
    else if ($('#ProjectID option:selected').text().indexOf("Garden") != -1 && $('#TypePerformanceID option:selected').text().indexOf("Leasing") != -1) {
        $("#performance-table-header-ls").show();
        $("#performance-table-header-ls-row2").show();
        $("#performance-table-header-ls-row3").show();
        $("#performance-table-header-ls-row4").show();
        $("#performance-table-header-ls-row5").show();
        $("#performance-table-header-fb").hide();
        $("#performance-table-header-fb-row2").hide();
        $("#performance-table-header-fb-row3").hide();
        $("#performance-table-header-fb-row4").hide();
        $("#performance-table-header-fb-row5").hide();
        $("#performance-type-fb").hide();
        $("#performance-table-header-manor").hide();
        $("#performance-table-header-manor-row2").hide();
        $("#performance-table-header-manor-row3").hide();
    }
    else if ($('#ProjectID option:selected').text().indexOf("Garden") != -1 && $('#TypePerformanceID option:selected').text().indexOf("F&B") != -1) {
        $("#performance-table-header-ls").hide();
        $("#performance-table-header-ls-row2").hide();
        $("#performance-table-header-ls-row3").hide();
        $("#performance-table-header-ls-row4").hide();
        $("#performance-table-header-ls-row5").hide();
        $("#performance-table-header-fb").show();
        $("#performance-table-header-fb-row2").show();
        $("#performance-table-header-fb-row3").show();
        $("#performance-table-header-fb-row4").show();
        $("#performance-table-header-fb-row5").show();
        $("#performance-type-fb").show();
        $("#performance-table-header-manor").hide();
        $("#performance-table-header-manor-row2").hide();
        $("#performance-table-header-manor-row3").hide();
        if ($('#TypeFBID option:selected').text() === "FB") {
            $("#table-header-fb-fb").show();
            $("#table-header-fb-row2-rev-fb").show();
            $("#table-header-fb-row3-fb").show();
            $("#table-header-fb-row3-line-fb").show();
            $("#table-header-fb-row3-all-fb").show();
            $("#table-header-fb-row4-fb").show();
            $("#table-header-fb-none-fb").hide();
            $("#table-header-fb-row2-rev-none-fb").hide();
            $("#table-header-fb-row3-none-fb").hide();
            $("#table-header-fb-row3-line-none-fb").hide();
            $("#table-header-fb-row3-all-none-fb").hide();
            $("#table-header-fb-row4-none-fb").hide();
        } else {
            $("#table-header-fb-none-fb").show();
            $("#table-header-fb-row2-rev-none-fb").show();
            $("#table-header-fb-row3-none-fb").show();
            $("#table-header-fb-row3-line-none-fb").show();
            $("#table-header-fb-row3-all-none-fb").show();
            $("#table-header-fb-row4-none-fb").show();
            $("#table-header-fb-fb").hide();
            $("#table-header-fb-row2-rev-fb").hide();
            $("#table-header-fb-row3-fb").hide();
            $("#table-header-fb-row3-line-fb").hide();
            $("#table-header-fb-row3-all-fb").hide();
            $("#table-header-fb-row4-fb").hide();
        }
    }

    $("#btnSearch").click();
}

function onChangeTypeFB(val) {
    if ($('#TypeFBID option:selected').text() === "FB") {
        $("#table-header-fb-fb").show();
        $("#table-header-fb-row2-rev-fb").show();
        $("#table-header-fb-row3-fb").show();
        $("#table-header-fb-row3-line-fb").show();
        $("#table-header-fb-row3-all-fb").show();
        $("#table-header-fb-row4-fb").show();
        $("#table-header-fb-none-fb").hide();
        $("#table-header-fb-row2-rev-none-fb").hide();
        $("#table-header-fb-row3-none-fb").hide();
        $("#table-header-fb-row3-line-none-fb").hide();
        $("#table-header-fb-row3-all-none-fb").hide();
        $("#table-header-fb-row4-none-fb").hide();
    }
    else {
        $("#table-header-fb-none-fb").show();
        $("#table-header-fb-row2-rev-none-fb").show();
        $("#table-header-fb-row3-none-fb").show();
        $("#table-header-fb-row3-line-none-fb").show();
        $("#table-header-fb-row3-all-none-fb").show();
        $("#table-header-fb-row4-none-fb").show();
        $("#table-header-fb-fb").hide();
        $("#table-header-fb-row2-rev-fb").hide();
        $("#table-header-fb-row3-fb").hide();
        $("#table-header-fb-row3-line-fb").hide();
        $("#table-header-fb-row3-all-fb").hide();
        $("#table-header-fb-row4-fb").hide();
    }

    $("#btnSearch").click();
}

$("#btnSearch").click(function startCreate() {
    if ($("#ProjectID").val() == "") {
        $("#ProjectID").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Dự án");
        $("#error-block").show();
        return;
    }
    if ($("#TypePerformanceID").val() == "") {
        $("#TypePerformanceID").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Loại hiệu suất");
        $("#error-block").show();
        return;
    }
    if ($("#Year").val() == "") {
        $("#Year").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Năm");
        $("#error-block").show();
        return;
    }
    if ($('#TypePerformanceID option:selected').text().indexOf("F&B") != -1 && $("#TypeFBID").val() == "") {
        $("#TypeFBID").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Loại FB");
        $("#error-block").show();
        return;
    }

    var performanceSearch = new Object();
    performanceSearch.ProjectID = $("#ProjectID").val();
    performanceSearch.TypePerformanceID = $("#TypePerformanceID").val();
    performanceSearch.Year = $("#Year").val();
    performanceSearch.TypeFBID = $("#TypeFBID").val();

    $.ajax({
        type: "POST",
        url: "/Performance/SearchPerformance",
        data: JSON.stringify(performanceSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            if (response != null) {
                $("#performance-table-id tbody").html(response);
                $("#error-block").hide();
                if (response.ShowShowFormByProjType === 1) {
                    $("#performance-table-id #performance-table-header-ls").show();
                    $("#performance-table-id #performance-table-header-ls-row2").show();
                    $("#performance-table-id #performance-table-header-ls-row3").show();
                    $("#performance-table-id #performance-table-header-ls-row4").show();
                    $("#performance-table-id #performance-table-header-ls-row5").show();
                    $("#performance-table-id #performance-table-header-fb").hide();
                    $("#performance-table-id #performance-table-header-fb-row2").hide();
                    $("#performance-table-id #performance-table-header-fb-row3").hide();
                    $("#performance-table-id #performance-table-header-fb-row4").hide();
                    $("#performance-table-id #performance-table-header-fb-row5").hide();
                    $("#performance-table-id #performance-table-header-manor").hide();
                    $("#performance-table-id #performance-table-header-manor-row2").hide();
                    $("#performance-table-id #performance-table-header-manor-row3").hide();
                } else if (response.ShowShowFormByProjType === 2) {
                    $("#performance-table-header-ls").hide();
                    $("#performance-table-header-ls-row2").hide();
                    $("#performance-table-header-ls-row3").hide();
                    $("#performance-table-header-ls-row4").hide();
                    $("#performance-table-header-ls-row5").hide();
                    $("#performance-table-header-fb").show();
                    $("#performance-table-header-fb-row2").show();
                    $("#performance-table-header-fb-row3").show();
                    $("#performance-table-header-fb-row4").show();
                    $("#performance-table-header-fb-row5").show();
                    if (response.TypeFB === 1) {
                        $("#table-header-fb-row4-none-fb").show();
                        $("#table-header-fb-row3-none-fb").show();
                        $("#table-header-fb-row3-line-none-fb").show();
                        $("#table-header-fb-row3-all-none-fb").show();
                        $("#table-header-fb-row3-fb").hide();
                        $("#table-header-fb-row3-line-fb").hide();
                        $("#table-header-fb-row3-all-fb").hide();
                    } else {
                        $("#table-header-fb-row4-none-fb").hide();
                        $("#table-header-fb-row3-none-fb").hide();
                        $("#table-header-fb-row3-line-none-fb").hide();
                        $("#table-header-fb-row3-all-none-fb").hide();
                        $("#table-header-fb-row3-fb").show();
                        $("#table-header-fb-row3-line-fb").show();
                        $("#table-header-fb-row3-all-fb").show();
                    }
                    $("#performance-table-header-manor").hide();
                    $("#performance-table-header-manor-row2").hide();
                    $("#performance-table-header-manor-row3").hide();
                } else if (response.ShowShowFormByProjType === 3) {
                    $("#performance-table-header-ls").hide();
                    $("#performance-table-header-ls-row2").hide();
                    $("#performance-table-header-ls-row3").hide();
                    $("#performance-table-header-ls-row4").hide();
                    $("#performance-table-header-ls-row5").hide();
                    $("#performance-table-header-fb").hide();
                    $("#performance-table-header-fb-row2").hide();
                    $("#performance-table-header-fb-row3").hide();
                    $("#performance-table-header-fb-row4").hide();
                    $("#performance-table-header-fb-row5").hide();
                    $("#performance-table-header-manor").show();
                    $("#performance-table-header-manor-row2").show();
                    $("#performance-table-header-manor-row3").show();
                }
            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            $("#error-block").hide();
            $("#performance-table-id").html(response.responseText);
        }
    });
});
// #endregion PageLoaded