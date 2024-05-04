// #region PageLoaded
$(function () {
    // Binding table
    //var detailTable = $("#tbl-detail").DataTable({
    //    "paging": false,
    //    "ordering": false,
    //    "searching": false,
    //    "info": false
    //});

    if ($("#ErrorMessage").text().length != 0) {
        $("#error-block").show();
    } else {
        $("#error-block").hide();
    }
});
// #endregion PageLoaded



// #region Info
$("#btnStart").click(function startCreate() {
    if ($("#UpfCross_Year").val() == "" | $("#UpfCross_Month").val() == "") {
        $("#ErrorMessage").text("Bạn chưa chọn Năm hoặc Tháng");
        $("#error-block").show();
        return;
    }

    $("#ErrorMessage").text("");
    $("#error-block").hide();

    var upf = new Object();
    upf.Year = $("#UpfCross_Year").val();
    upf.Month = $("#UpfCross_Month").val();
    $.ajax({
        type: "POST",
        url: "/Upf/StartCreateCross",
        data: JSON.stringify(upf),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            if (response != null) {
                $("#ErrorMessage").text("");
                $("#error-block").hide();
                $("#kpi-block").show();

                $("#UpfCross_Year").attr("disabled", true);
                $("#UpfCross_Month").attr("disabled", true);

                $("#btnStart").attr("disabled", true);

                $("#tbl-detail").html(response);

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
    location.reload();
});
// #endregion Info



// #region CRUD modal
//Add
$('#tbl-detail').on('click', '.btn-add', function () {
    //ResetModalValues();
    debugger;
    $("#Detail_FromDepartment").val($("#UserInfo_DepartmentID").val());
    $("#Detail_FromName").val($("#UserInfo_DepartmentName").val());
    $("#lDetail_FromName").text($("#UserInfo_DepartmentName").val());
    

    var actionName = 'Add';
    $('#DetailAction').val(actionName);
    SetButtonStateByActionName(actionName);
    SetModalStateByActionName(actionName);
    $('#edit-detail-modal').modal('show');
});
//View
$('#tbl-detail').on('click', '.btn-detail-view', function () {
    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    var rowItem = {};
    GetRowValues($row, rowItem);
    SetModalValues(rowItem);
    var actionName = 'View';
    $('#DetailAction').val(actionName);
    SetButtonStateByActionName(actionName);
    SetModalStateByActionName(actionName);
    $('#edit-detail-modal').modal('show');
});
//Edit
$('#tbl-detail').on('click', '.btn-detail-edit', function () {
    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    var rowItem = {};
    GetRowValues($row, rowItem);
    SetModalValues(rowItem);
    var actionName = 'Edit';
    $('#DetailAction').val(actionName);
    SetButtonStateByActionName(actionName);
    SetModalStateByActionName(actionName);
    $('#edit-detail-modal').modal('show');

});

//Delete
$('#tbl-detail').on('click', '.btn-detail-delete', function () {
    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    var rowItem = {};
    GetRowValues($row, rowItem);
    SetModalValues(rowItem);
    var actionName = 'Delete';
    $('#DetailAction').val(actionName);
    SetButtonStateByActionName(actionName);
    SetModalStateByActionName(actionName);
    $('#edit-detail-modal').modal('show');

});
// #endregion CompleteWork table

function GetRowValues(row, item) {
    item.ID = row.find("td:eq(0) input[name*='.ID']").val();
    item.UpfCrossID = row.find("td:eq(0) input[name*='.UpfCrossID']").val();
    item.Objective = row.find("td:eq(1) input[name*='Objective']").val();
    item.FromDepartment = row.find("td:eq(2) input[name*='FromDepartment']").val();
    item.FromName = row.find("td:eq(2) input[name*='FromName']").val();
    item.ContentsRequested = row.find("td:eq(3) input[name*='ContentsRequested']").val();
    item.ExpectedTimeOfCompletion = row.find("td:eq(3) input[name*='ExpectedTimeOfCompletion']").val();
    item.ExpectedResult = row.find("td:eq(3) input[name*='ExpectedResult']").val();
    item.FromWeight = row.find("td:eq(5) input[name*='FromWeight']").val();
    item.ToDepartment = row.find("td:eq(6) input[name*='ToDepartment']").val();
    item.ToName = row.find("td:eq(6) input[name*='ToName']").val();
    item.TimeOfCompletion = row.find("td:eq(4) input[name*='TimeOfCompletion']").val();
    item.Result = row.find("td:eq(4) input[name*='Result']").val();
    item.FromScore = row.find("td:eq(6) input[name*='FromScore']").val();
    item.PlanToDo = row.find("td:eq(6) input[name*='PlanToDo']").val();
    item.ExplainationForResults = row.find("td:eq(6) input[name*='ExplainationForResults']").val();
    item.Solutions = row.find("td:eq(6) input[name*='Solutions']").val();
    item.Timeline = row.find("td:eq(6) input[name*='Timeline']").val();
    item.ToWeight = row.find("td:eq(6) input[name*='ToWeight']").val();
    item.ToScore = row.find("td:eq(6) input[name*='ToScore']").val();
    item.AssessmentByCouncil = row.find("td:eq(6) input[name*='AssessmentByCouncil']").val();
    item.TotalScore = row.find("td:eq(6) input[name*='TotalScore']").val();
    item.Status = row.find("td:eq(6) input[name*='Status']").val();
    item.Created = row.find("td:eq(6) input[name*='Created']").val();
    item.CreatedBy = row.find("td:eq(6) input[name*='CreatedBy']").val();
    item.DeleteFlg = row.find("td:eq(6) input[name*='DeleteFlg']").val();
    item.Deleted = row.find("td:eq(6) input[name*='Deleted']").val();
    item.DeletedBy = row.find("td:eq(6) input[name*='DeletedBy']").val();
    item.Seq = row.find("td:eq(6) input[name*='Seq']").val();
};

function GetModalValues(item) {
    item.ID = $("#Detail_ID").val();
    item.UpfCrossID = $("#Detail_UpfCrossID").val();
    item.Objective = $("#Detail_Objective").val();
    item.FromDepartment = $("#Detail_FromDepartment").val();
    item.FromName = $("#Detail_FromName").val();
    item.ContentsRequested = $("#Detail_ContentsRequested").val();
    item.ExpectedTimeOfCompletion = $("#Detail_ExpectedTimeOfCompletion").val();
    item.ExpectedResult = $("#Detail_ExpectedResult").val();
    item.FromWeight = $("#Detail_FromWeight").val();
    item.ToDepartment = $("#Detail_ToDepartment").val();
    item.ToName = $("#Detail_ToName").val();
    item.TimeOfCompletion = $("#Detail_TimeOfCompletion").val();
    item.Result = $("#Detail_Result").val();
    item.FromScore = $("#Detail_FromScore").val();
    item.PlanToDo = $("#Detail_PlanToDo").val();
    item.ExplainationForResults = $("#Detail_ExplainationForResults").val();
    item.Solutions = $("#Detail_Solutions").val();
    item.Timeline = $("#Detail_Timeline").val();
    item.ToWeight = $("#Detail_ToWeight").val();
    item.ToScore = $("#Detail_ToScore").val();
    item.AssessmentByCouncil = $("#Detail_AssessmentByCouncil").val();
    item.TotalScore = $("#Detail_TotalScore").val();
    item.Status = $("#Detail_Status").val();
    item.Created = $("#Detail_Created").val();
    item.CreatedBy = $("#Detail_CreatedBy").val();
    item.DeleteFlg = $("#Detail_DeleteFlg").val();
    item.Deleted = $("#Detail_Deleted").val();
    item.DeletedBy = $("#Detail_DeletedBy").val();
    item.Seq = $("#Detail_Seq").val();
};

function SetModalValues(item) {
    $("#Detail_ID").val(item.ID);
    $("#Detail_UpfCrossID").val(item.UpfCrossID);
    $("#Detail_Objective").val(item.Objective);
    $("#Detail_FromDepartment").val(item.FromDepartment);
    $("#Detail_FromName").val(item.FromName);
    $("#Detail_ContentsRequested").val(item.ContentsRequested);
    $("#Detail_ExpectedTimeOfCompletion").val(item.ExpectedTimeOfCompletion);
    $("#Detail_ExpectedResult").val(item.ExpectedResult);
    $("#Detail_FromWeight").val(item.FromWeight);
    $("#Detail_ToDepartment").val(item.ToDepartment);
    $("#Detail_ToName").val(item.ToName);
    $("#Detail_TimeOfCompletion").val(item.TimeOfCompletion);
    $("#Detail_Result").val(item.Result);
    $("#Detail_FromScore").val(item.FromScore);
    $("#Detail_PlanToDo").val(item.PlanToDo);
    $("#Detail_ExplainationForResults").val(item.ExplainationForResults);
    $("#Detail_Solutions").val(item.Solutions);
    $("#Detail_Timeline").val(item.Timeline);
    $("#Detail_ToWeight").val(item.ToWeight);
    $("#Detail_ToScore").val(item.ToScore);
    $("#Detail_AssessmentByCouncil").val(item.AssessmentByCouncil);
    $("#Detail_TotalScore").val(item.TotalScore);
    $("#Detail_Status").val(item.Status);
    $("#Detail_Created").val(item.Created);
    $("#Detail_CreatedBy").val(item.CreatedBy);
    $("#Detail_DeleteFlg").val(item.DeleteFlg);
    $("#Detail_Deleted").val(item.Deleted);
    $("#Detail_DeletedBy").val(item.DeletedBy);
    $("#Detail_Seq").val(item.Seq);
};

function ResetModalValues() {
    $("#Detail_ID").val("");
    $("#Detail_UpfCrossID").val("");
    $("#Detail_Objective").val("");
    $("#Detail_FromDepartment").val("");
    $("#Detail_FromName").val("");
    $("#Detail_ContentsRequested").val("");
    $("#Detail_ExpectedTimeOfCompletion").val("");
    $("#Detail_ExpectedResult").val("");
    $("#Detail_FromWeight").val("");
    $("#Detail_ToDepartment").val("");
    $("#Detail_ToName").val("");
    $("#Detail_TimeOfCompletion").val("");
    $("#Detail_Result").val("");
    $("#Detail_FromScore").val("");
    $("#Detail_PlanToDo").val("");
    $("#Detail_ExplainationForResults").val("");
    $("#Detail_Solutions").val("");
    $("#Detail_Timeline").val("");
    $("#Detail_ToWeight").val("");
    $("#Detail_ToScore").val("");
    $("#Detail_AssessmentByCouncil").val("");
    $("#Detail_TotalScore").val("");
    $("#Detail_Status").val("");
    $("#Detail_Created").val("");
    $("#Detail_CreatedBy").val("");
    $("#Detail_DeleteFlg").val("");
    $("#Detail_Deleted").val("");
    $("#Detail_DeletedBy").val("");
    $("#Detail_Seq").val("");
};

function SetButtonStateByActionName(actionName) {
    if (actionName == 'View') {
        $("#modalTitle").text("Xem đánh giá");
        $("#btnAddDetailModal").hide();
        $("#btnCloseDetailModal").show();
        $("#btnUpdateDetailModal").hide();
        $("#btnDeleteDetailModal").hide();
    };
    if (actionName == 'Add') {
        $("#modalTitle").text("Thêm mới đánh giá");
        $("#btnAddDetailModal").show();
        $("#btnCloseDetailModal").hide();
        $("#btnUpdateDetailModal").hide();
        $("#btnDeleteDetailModal").hide();
    };
    if (actionName == 'Edit') {
        $("#modalTitle").text("Chỉnh sửa đánh giá");
        $("#btnAddDetailModal").hide();
        $("#btnCloseDetailModal").hide();
        $("#btnUpdateDetailModal").show();
        $("#btnDeleteDetailModal").hide();
    };
    if (actionName == 'Delete') {
        $("#modalTitle").text("Xóa đánh giá");
        $("#btnAddDetailModal").hide();
        $("#btnCloseDetailModal").hide();
        $("#btnUpdateDetailModal").hide();
        $("#btnDeleteDetailModal").show();
    };
};

function SetModalStateByActionName(actionName) {
    if (actionName == 'View' | actionName == 'Delete') {
        $("#Detail_ID").attr("disabled", true);
        $("#Detail_UpfCrossID").attr("disabled", true);
        $("#Detail_Objective").attr("disabled", true);
        $("#Detail_FromDepartment").attr("disabled", true);
        $("#Detail_FromName").attr("disabled", true);
        $("#Detail_ContentsRequested").attr("disabled", true);
        $("#Detail_ExpectedTimeOfCompletion").attr("disabled", true);
        $("#Detail_ExpectedResult").attr("disabled", true);
        $("#Detail_FromWeight").attr("disabled", true);
        $("#Detail_ToDepartment").attr("disabled", true);
        $("#Detail_ToName").attr("disabled", true);
        $("#Detail_TimeOfCompletion").attr("disabled", true);
        $("#Detail_Result").attr("disabled", true);
        $("#Detail_FromScore").attr("disabled", true);
        $("#Detail_PlanToDo").attr("disabled", true);
        $("#Detail_ExplainationForResults").attr("disabled", true);
        $("#Detail_Solutions").attr("disabled", true);
        $("#Detail_Timeline").attr("disabled", true);
        $("#Detail_ToWeight").attr("disabled", true);
        $("#Detail_ToScore").attr("disabled", true);
        $("#Detail_AssessmentByCouncil").attr("disabled", true);
        $("#Detail_TotalScore").attr("disabled", true);
        $("#Detail_Status").attr("disabled", true);
        $("#Detail_Created").attr("disabled", true);
        $("#Detail_CreatedBy").attr("disabled", true);
        $("#Detail_DeleteFlg").attr("disabled", true);
        $("#Detail_Deleted").attr("disabled", true);
        $("#Detail_DeletedBy").attr("disabled", true);
        $("#Detail_Seq").attr("disabled", true);
    }
    else {
        $("#Detail_ID").attr("disabled", false);
        $("#Detail_UpfCrossID").attr("disabled", false);
        $("#Detail_Objective").attr("disabled", false);
        $("#Detail_FromDepartment").attr("disabled", false);
        $("#Detail_FromName").attr("disabled", false);
        $("#Detail_ContentsRequested").attr("disabled", false);
        $("#Detail_ExpectedTimeOfCompletion").attr("disabled", false);
        $("#Detail_ExpectedResult").attr("disabled", false);
        $("#Detail_FromWeight").attr("disabled", false);
        $("#Detail_ToDepartment").attr("disabled", false);
        $("#Detail_ToName").attr("disabled", false);
        $("#Detail_TimeOfCompletion").attr("disabled", false);
        $("#Detail_Result").attr("disabled", false);
        $("#Detail_FromScore").attr("disabled", false);
        $("#Detail_PlanToDo").attr("disabled", false);
        $("#Detail_ExplainationForResults").attr("disabled", false);
        $("#Detail_Solutions").attr("disabled", false);
        $("#Detail_Timeline").attr("disabled", false);
        $("#Detail_ToWeight").attr("disabled", false);
        $("#Detail_ToScore").attr("disabled", false);
        $("#Detail_AssessmentByCouncil").attr("disabled", false);
        $("#Detail_TotalScore").attr("disabled", false);
        $("#Detail_Status").attr("disabled", false);
        $("#Detail_Created").attr("disabled", false);
        $("#Detail_CreatedBy").attr("disabled", false);
        $("#Detail_DeleteFlg").attr("disabled", false);
        $("#Detail_Deleted").attr("disabled", false);
        $("#Detail_DeletedBy").attr("disabled", false);
        $("#Detail_Seq").attr("disabled", false);
    };
};

//Add
$("#btnAddDetailModal").click(function () {

    var items = new Array();
    $("#tbl-detail tbody").find('tr').each(function () {

        var item = {};
        GetRowValues($(this), item);
        items.push(item);
    });

    var newItem = {};
    GetModalValues(newItem);
    items.push(newItem);

    $.ajax({
        type: "POST",
        url: "/Upf/AddUpfCrossDetail",
        data: JSON.stringify(items), //$("#tbl-detail").serialize(), //JSON.stringify(completeWork),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            if (response != null) {
                $('#edit-detail-modal').modal('hide');
                $("#tbl-detail").html(response);
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

//View
$("#btnCloseDetailModal").click(function () {
    $('#edit-detail-modal').modal('hide');
});

//Update
$("#btnUpdateDetailModal").click(function () {

    var items = new Array();
    var newItem = {};
    GetModalValues(newItem);
    $("#tbl-detail tbody").find('tr').each(function () {

        var item = {};
        GetRowValues($(this), item);
        if (item.Seq == newItem.Seq) {
            items.push(newItem);
        } else {
            items.push(item);
        };
    });

    $.ajax({
        type: "POST",
        url: "/Upf/UpdateUpfCrossDetail",
        data: JSON.stringify(items), //$("#tbl-detail").serialize(), //JSON.stringify(completeWork),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            if (response != null) {
                $('#edit-detail-modal').modal('hide');
                $("#tbl-detail").html(response);
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

//Delete
$("#btnDeleteDetailModal").click(function () {

    var items = new Array();
    var newItem = {};
    GetModalValues(newItem);
    $("#tbl-detail tbody").find('tr').each(function () {

        var item = {};
        GetRowValues($(this), item);
        if (item.Seq != newItem.Seq) {
            items.push(item);
        }
    });

    $.ajax({
        type: "POST",
        url: "/Upf/DeleteUpfCrossDetail",
        data: JSON.stringify(items), //$("#tbl-detail").serialize(), //JSON.stringify(completeWork),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            if (response != null) {
                $('#edit-detail-modal').modal('hide');
                $("#tbl-detail").html(response);
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





