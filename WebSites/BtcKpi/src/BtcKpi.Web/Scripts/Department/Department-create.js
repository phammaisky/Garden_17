// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-upf-self');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-upf-self');
    subMenuItem.addClass('active');

    // Binding On Year Complete Work table
    //FirstLoat or Not
    $("#kpi-block").hide();
    $("#manageComment").hide();
    $("#approveID").hide();
    $("#notApproveID").hide();
    $("#saveID").hide();
    if ($("#FirstLoad").val() == 'True') {
        $("#ErrorMessage").text("");
        $("#error-block").hide();
        $("#kpi-block").hide();
        $("#Upf_ApproveBy").attr("disabled", false);
        $("#Upf_BodApproved").attr("disabled", false);
        $("#Upf_ScheduleType").attr("disabled", false);
        $("#Upf_Year").attr("disabled", false);
        $("#Upf_ScheduleID").attr("disabled", false);
        $("#btnStart").attr("disabled", false);
        $('#dUpf_ScheduleID').hide();
    }
    else {

        if ($("#ErrorMessage").text().length != 0) {
            $("#error-block").show();
        } else {
            $("#error-block").hide();
        }
        
        $("#kpi-block").show();
        $("#Upf_ApproveBy").attr("disabled", true);
        $("#Upf_BodApproved").attr("disabled", true);
        $("#Upf_ScheduleType").attr("disabled", true);
        $("#Upf_Year").attr("disabled", true);
        $("#Upf_ScheduleID").attr("disabled", true);

        $("#btnStart").attr("disabled", true);
        if ($("#upfHiddenId").val() != 0) {
            $("#btnReset").attr("disabled", true);
        }

        if ($("#Upf_ScheduleType").val() == "1") {
            $('#dUpf_ScheduleID').show();
        } else {
            $('#dUpf_ScheduleID').hide();
        }
        if ($("#isApprove").val() == 'True' && $("#isComment").val() == 'False') {
            $("#manageComment").show();
            $("#completeID").hide();
            $("#draftID").hide();
            $("#approveID").show();
            $("#notApproveID").show();
            $("#outsAchievID").attr("disabled", true);
            $("#selfRatingID").attr("disabled", true);
        }
        if ($("#isBODApprove").val() == 'True' && $("#isComment").val() == 'False') {
            $("#manageComment").show();
            $("#completeID").hide();
            $("#draftID").hide();
            $("#approveID").show();
            $("#notApproveID").show();
            $("#outsAchievID").attr("disabled", true);
            $("#selfRatingID").attr("disabled", true);
        }
        if ($("#isComment").val() == 'True' && $("#isApprove").val() == 'False') {
            $("#manageComment").show();
            $("#completeID").hide();
            $("#draftID").hide();
            $("#approveID").hide();
            $("#notApproveID").hide();
            $("#saveID").show();
            $("#outsAchievID").attr("disabled", true);
            $("#selfRatingID").attr("disabled", true);
        }
    }
    configDatepicker();
});
// #endregion PageLoaded


// #region Info
function scheduleChange(val) {
    if (val == "0") {
        $('#dUpf_ScheduleID').hide();
    } else {
        $('#dUpf_ScheduleID').show();
    }
}

function onChangeOutsAchiev(outsAchiev) {
    var jobObject = new Object();
    jobObject.OutsAchiev = outsAchiev;
    $.ajax({
        type: "POST",
        url: "/Department/OutsAchievChange",
        data: JSON.stringify(jobObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                //$("#DepartmentID").html(response);
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            //alert(response.responseText);
            //$("#DepartmentID").html(response.responseText);
        }
    });
}

function onChangeSelfRating(selfRating) {
    var jobObject = new Object();
    jobObject.SelfRating = selfRating;
    $.ajax({
        type: "POST",
        url: "/Department/SelfRatingChange",
        data: JSON.stringify(jobObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                //$("#DepartmentID").html(response);
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            //alert(response.responseText);
            //$("#DepartmentID").html(response.responseText);
        }
    });
}

$("#btnStart").click(function startCreate() {

    if ($("#Upf_ApproveBy").val() == "") {
        $("#Upf_ApproveBy").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Người duyệt");
        $("#error-block").show();
        return;
    }
    if ($("#Upf_BodApproved").val() == "") {
        $("#Upf_BodApproved").focus();
        $("#ErrorMessage").text("Bạn chưa chọn BOD duyệt");
        $("#error-block").show();
        return;
    }
    if ($("#Upf_ScheduleType").val() == "") {
        $("#Upf_ScheduleType").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Loại");
        $("#error-block").show();
        return;
    }
    if($("#Upf_Year").val() == "") {
        $("#Upf_Year").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Năm");
        $("#error-block").show();
        return;
    }
    if($("#Upf_ScheduleType").val() == "1" && $("#Upf_ScheduleID").val() == "") {
        $("#Upf_ScheduleID").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Kỳ");
        $("#error-block").show();
        return;
    }

    var department = new Object();
    department.ScheduleType = $("#Upf_ScheduleType").val();
    department.Year = $("#Upf_Year").val();
    department.ScheduleID = $("#Upf_ScheduleID").val();
    department.ApproveBy = $("#Upf_ApproveBy").val();
    department.BodApproved = $("#Upf_BodApproved").val();

    $.ajax({
        type: "POST",
        url: "/Department/Start",
        data: JSON.stringify(department),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                if (response.CheckExistsDepartment == false) {
                    var error = "Bạn chỉ được tạo 1 bản ghi KPIs phòng ban năm hoặc định kỳ ";
                    $("#ErrorMessage").text(error);
                    $("#error-block").show();
                } else {
                    $("#ErrorMessage").text("");
                    $("#error-block").hide();
                    $("#kpi-block").show();
                    $("#Upf_ApproveBy").attr("disabled", true);
                    $("#Upf_BodApproved").attr("disabled", true);
                    $("#Upf_ScheduleType").attr("disabled", true);
                    $("#Upf_Year").attr("disabled", true);
                    $("#Upf_ScheduleID").attr("disabled", true);
                    $("#btnStart").attr("disabled", true);
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
        url: "/Department/Reset",
        data: null,
        success: function (response) {
            if (response != null) {
                $('#add-detail-modal').modal('hide');
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
// #endregion Info

// #region Modal
//Add Name Detail Modal

$(".name-detail-add").click(function () {
    $('#btnAddDetailModal').show();
    $('#btnEditDetailModal').hide();
});

$("#btnAddDetailModal").click(function () {
    if ($("#nameKPI").val() == "") {
        $("#nameKPI").focus();
        $("#modalError").show();
        return;
    }

    var jobObject = new Object();
    jobObject.NameKPI = $('#nameKPI').val();
    if (jobObject != null) {
        $.ajax({
            type: "POST",
            url: "/Department/AddNameKPIDetail",
            data: JSON.stringify(jobObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#add-detail-modal').modal('hide');
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
    }
});

$(".name-detail-edit").click(function () {
    $('#btnAddDetailModal').hide();
    $('#btnEditDetailModal').show();
    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    $("#nameKPI").val($row.find('td:eq(2)').text().trim());
    $("#nameDetailEditID").val($row.find('td:eq(0)').text().trim());
});

$("#btnEditDetailModal").click(function () {
    if ($("#nameKPI").val() == "") {
        $("#nameKPI").focus();
        $("#modalError").show();
        return;
    }

    var jobObject = new Object();
    jobObject.NameKPI = $('#nameKPI').val();
    jobObject.Order = $('#nameDetailEditID').val();
    if (jobObject != null) {
        $.ajax({
            type: "POST",
            url: "/Department/EditNameKPIDetail",
            data: JSON.stringify(jobObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#add-detail-modal').modal('hide');
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
    }
});

$(".job-obj-add").click(function () {
    $('#btnUpdateJobDetailModal').hide();
    $('#btnAddJobDetailModal').show();
    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    $("#nameKPIView").val($row.find('td:eq(2)').text().trim());
    $("#nameDetailID").val($row.find('td:eq(0)').text().trim());
    $("#completeWorkType").val("");
    $("#jobName").val("");
    $("#scheduledTime").val("");
    $("#numberPlan").val("");
    $("#performResults").val("");
    $("#weight").val("");
    $("#point").val("");
});

//Add Job Detail Modal
$("#btnAddJobDetailModal").click(function () {
    if ($("#completeWorkType").val() == "") {
        $("#completeWorkType").focus();
        $("#modalErrorDetail").show();
        return;
    }
    if ($("#jobName").val() == "") {
        $("#jobName").focus();
        $("#modalErrorDetail").show();
        return;
    }
    if ($("#scheduledTime").val() == "") {
        $("#scheduledTime").focus();
        $("#modalErrorDetail").show();
        return;
    }
    if ($("#weight").val() == "") {
        $("#weight").focus();
        $("#modalErrorDetail").show();
        return;
    }
    var jobObject = new Object();
    jobObject.JobName = $('#jobName').val();
    jobObject.ScheduledTime = $('#scheduledTime').val();
    jobObject.NumberPlan = $('#numberPlan').val();
    jobObject.PerformResults = $('#performResults').val();
    jobObject.Weight = $('#weight').val();
    jobObject.Point = parseFloat($('#point').val());
    jobObject.NameDetailID = $('#completeWorkType').val();
    jobObject.NameKPIEdit = $('#nameKPIView').val();
    if (jobObject != null) {
        $.ajax({
            type: "POST",
            url: "/Department/AddJobObject",
            data: JSON.stringify(jobObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#add-job-detail-modal').modal('hide');
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
    }
});

//Update job object
$(".job-obj-edit").click(function () {
    var $buttonClicked = $(this);
    var order = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    var nameDetailID = $row.find('td:eq(0)').text().trim();
    $("#nameKPIView").val($row.find('td:eq(2)').text().trim());
    $("#nameDetailID").val(nameDetailID);
    $.ajax({
        type: "GET",
        url: "/Department/UpdateJobObjPre",
        data: { Order: order, NameDetailID: nameDetailID },
        success: function (response) {
            if (response != null) {
                $("#jobName").val(response.JobName);
                $("#scheduledTime").val(response.ScheduleTimeString);
                $("#numberPlan").val(response.NumberPlan);
                $("#performResults").val(response.PerformResults);
                $("#weight").val(response.Weight);
                $("#point").val(response.Point);
                $("#upfJobObjOrderID").val(response.Order);
                $("#jobObjOrderID").val(response.ID);
                $("#upfNameDetailID").val(response.UpfNameDetailID);
                $('#btnUpdateJobDetailModal').show();
                $('#btnAddJobDetailModal').hide();
                $('#add-job-detail-modal').modal('show');

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

//Delete job object
$(".job-obj-delete").click(function () {
    var $buttonClicked = $(this);
    var order = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    var nameDetailID = $row.find('td:eq(0)').text().trim();
    if(confirm("Bạn có chắc chắn muốn xóa không?"))
    {
        $.ajax({
            type: "GET",
            url: "/Department/DeleteJobObject",
            data: { Order: order, NameDetailID: nameDetailID },
            success: function(response) {
                if (response != null) {
                    $('#add-job-detail-modal').modal('hide');
                    location.reload();
                } else {
                    alert("Kiểm tra lại dữ liệu");
                }
            },
            failure: function(response) {
                alert(response.responseText);
            },
            error: function(response) {
                alert(response.responseText);
            }
        });
    }
});

//Update Modal
$("#btnUpdateJobDetailModal").click(function () {
    if ($("#jobName").val() == "") {
        $("#jobName").focus();
        $("#modalErrorDetail").show();
        return;
    }
    if ($("#scheduledTime").val() == "") {
        $("#scheduledTime").focus();
        $("#modalErrorDetail").show();
        return;
    }
    if ($("#weight").val() == "") {
        $("#weight").focus();
        $("#modalErrorDetail").show();
        return;
    }
    var updateJobObject = new Object();
    updateJobObject.JobName = $('#jobName').val();
    updateJobObject.ScheduledTime = $('#scheduledTime').val();
    updateJobObject.NumberPlan = $('#numberPlan').val();
    updateJobObject.PerformResults = $('#performResults').val();
    updateJobObject.Weight = $('#weight').val();
    updateJobObject.Point = parseFloat($('#point').val());
    updateJobObject.Order = $('#upfJobObjOrderID').val();
    updateJobObject.ID = $('#jobObjOrderID').val();
    updateJobObject.UpfNameDetailID = $('#upfNameDetailID').val();
    updateJobObject.NameDetailID = $('#nameDetailID').val();
    if (updateJobObject != null) {
        $.ajax({
            type: "POST",
            url: "/Department/UpdateJobObject",
            data: JSON.stringify(updateJobObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#add-job-detail-modal').modal('hide');
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
    }
});

$(".pers-rew-prop-add").click(function() {
    $('#btnAddPersRewPropModal').show();
    $('#btnUpdatePersRewPropModal').hide();
    $("#employeeName").val("");
    $("#persOutsAchiev").val("");
});

$("#btnAddPersRewPropModal").click(function () {
    if ($("#employeeName").val() == "") {
        $("#employeeName").focus();
        $("#modalError").show();
        return;
    }
    if ($("#persOutsAchiev").val() == "") {
        $("#persOutsAchiev").focus();
        $("#modalError").show();
        return;
    }

    var persRewProp = new Object();
    persRewProp.EmployeeName = $('#employeeName').val();
    persRewProp.PersOutsAchiev = $('#persOutsAchiev').val();
    if (persRewProp != null) {
        $.ajax({
            type: "POST",
            url: "/Department/AddPersRewProp",
            data: JSON.stringify(persRewProp),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#add-persrewpro-modal').modal('hide');
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
    }
});

//Update job object
$(".pers-raw-prop-edit").click(function () {
    var $buttonClicked = $(this);
    var order = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    $.ajax({
        type: "GET",
        url: "/Department/UpdatePersRawPropPre",
        data: { Order: order },
        success: function (response) {
            if (response != null) {
                $("#employeeName").val(response.EmployeeName);
                $("#persOutsAchiev").val(response.PersOutsAchiev);
                $("#upfPersRawPropOrderID").val(response.Order);
                $("#persRawPropID").val(response.ID);
                $('#persRawPropUpfID').val(response.UpfID);
                $('#btnUpdatePersRewPropModal').show();
                $('#btnAddPersRewPropModal').hide();
                $('#add-persrewpro-modal').modal('show');

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

$("#btnUpdatePersRewPropModal").click(function () {
    if ($("#employeeName").val() == "") {
        $("#employeeName").focus();
        $("#modalErrorPers").show();
        return;
    }
    if ($("#persOutsAchiev").val() == "") {
        $("#persOutsAchiev").focus();
        $("#modalErrorPers").show();
        return;
    }
    var updatePersRawProp = new Object();
    updatePersRawProp.EmployeeName = $('#employeeName').val();
    updatePersRawProp.PersOutsAchiev = $('#persOutsAchiev').val();
    updatePersRawProp.Order = $('#upfPersRawPropOrderID').val();
    updatePersRawProp.ID = $('#persRawPropID').val();
    updatePersRawProp.UpfID = $('#persRawPropUpfID').val();
    if (updatePersRawProp != null) {
        $.ajax({
            type: "POST",
            url: "/Department/UpdatePersRawProp",
            data: JSON.stringify(updatePersRawProp),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#add-persrewpro-modal').modal('hide');
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
    }
});

//Delete job object
$(".pers-raw-prop-delete").click(function () {

    var $buttonClicked = $(this);
    var order = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    if(confirm("Bạn có chắc chắn muốn xóa không?"))
    {
        $.ajax({
            type: "GET",
            url: "/Department/DeletePersRawProp",
            data: { Order: order },
            success: function(response) {
                if (response != null) {
                    $('#add-persrewpro-modal').modal('hide');
                    location.reload();
                } else {
                    alert("Kiểm tra lại dữ liệu");
                }
            },
            failure: function(response) {
                alert(response.responseText);
            },
            error: function(response) {
                alert(response.responseText);
            }
        });
    }
});


function validateForm() {
    if ($("#weightTotalID").val() == "0") {
        $("#ErrorMessage").text("Mục tiêu công việc không được để trống!");
        $("#error-block").show();
        return false;
    }

    if ($("#weightTotalID").val() > 0 && $("#weightTotalID").val() != "100") {
        $("#weightTotalID").focus();
        $("#ErrorMessage").text("Tổng tỷ trọng mục tiêu hoàn thành công việc trong năm/kỳ phải bằng 100%.");
        $("#error-block").show();
        return false;
    }
    if ($("#selfRatingID").val() == "") {
        $("#selfRatingID").focus();
        $("#ErrorMessage").text("Bạn chưa chọn Đánh giá xếp loại của bộ phận!");
        $("#error-block").show();
        return false;
    }

    return true;
}

function onUpdateManagePoint(managePoint, jobDetailId) {
    var jobObject = new Object();
    jobObject.ManagePoint = parseFloat(managePoint);
    jobObject.ID = jobDetailId;
    $.ajax({
        type: "POST",
        url: "/Department/UpdateManagePoint",
        data: JSON.stringify(jobObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                location.reload();
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
        }
    });
}

function onUpdateBODPoint(bodPoint, jobDetailId) {
    var jobObject = new Object();
    jobObject.BodPoint = parseFloat(bodPoint);
    jobObject.ID = jobDetailId;
    $.ajax({
        type: "POST",
        url: "/Department/UpdateBODPoint",
        data: JSON.stringify(jobObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                location.reload();
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
        }
    });
}