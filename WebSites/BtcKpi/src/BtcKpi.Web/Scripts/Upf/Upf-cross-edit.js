// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-upf');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-upf-cross');
    subMenuItem.addClass('active');

    debugger;
    $("#ErrorMessage").text("");
    $("#error-block").hide();

    SetEnableByFromTo();

});
// #endregion PageLoaded

function SetEnableByFromTo() {
    if ($('#UserInfo_DepartmentID').val() === $('#Detail_FromDepartment').val()) {
        $("#Detail_Objective").prop("readonly", false);
        $("#Detail_ContentsRequested").prop("readonly", false);
        $("#Detail_ExpectedTimeOfCompletion").prop("readonly", false);
        $("#Detail_ExpectedResult").prop("readonly", false);
        $("#Detail_FromWeight").prop("readonly", false);
        $("#Detail_ToDepartment").prop("readonly", false);
        $("#Detail_TimeOfCompletion").prop("readonly", false);
        $("#Detail_Result").prop("readonly", false);
        $("#Detail_FromScore").prop("readonly", false);

        $("#Detail_PlanToDo").prop("readonly", true);
        $("#Detail_ExplainationForResults").prop("readonly", true);
        $("#Detail_Solutions").prop("readonly", true);
        $("#Detail_Timeline").prop("readonly", true);
        $("#Detail_ToWeight").prop("readonly", true);
        $("#Detail_ToScore").prop("readonly", true);

        $("#Detail_AssessmentByCouncil").prop("readonly", true);
        $("#Detail_TotalScore").prop("readonly", true);
    }
    if ($('#UserInfo_DepartmentID').val() === $('#Detail_ToDepartment').val()) {
        $("#Detail_Objective").prop("readonly", true);
        $("#Detail_ContentsRequested").prop("readonly", true);
        $("#Detail_ExpectedTimeOfCompletion").prop("readonly", true);
        $("#Detail_ExpectedResult").prop("readonly", true);
        $("#Detail_FromWeight").prop("readonly", true);
        $("#Detail_ToDepartment").prop("readonly", true);
        $("#Detail_TimeOfCompletion").prop("readonly", true);
        $("#Detail_Result").prop("readonly", true);
        $("#Detail_FromScore").prop("readonly", true);

        $("#Detail_PlanToDo").prop("readonly", false);
        $("#Detail_ExplainationForResults").prop("readonly", false);
        $("#Detail_Solutions").prop("readonly", false);
        $("#Detail_Timeline").prop("readonly", false);
        $("#Detail_ToWeight").prop("readonly", false);
        $("#Detail_ToScore").prop("readonly", false);

        $("#Detail_AssessmentByCouncil").prop("readonly", true);
        $("#Detail_TotalScore").prop("readonly", true);
    }
}


