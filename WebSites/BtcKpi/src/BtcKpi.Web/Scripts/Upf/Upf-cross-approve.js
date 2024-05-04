// #region PageLoaded
$(function () {
    if($("#ErrorMessage").text().length > 0){
        $("#error-block").show();
    } else {
        $("#error-block").hide();
    }

    bindingDetailTablesView();

    if ($("#Upf_ScheduleType").val() == "1") {
        $('#dUpf_ScheduleID').show();
        $("#liPersonalPlan").hide();
        $("#liNextYear").hide();
    } else {
        $('#dUpf_ScheduleID').hide();
        $("#liPersonalPlan").show();
        $("#liNextYear").show();
    }
});
// #endregion PageLoaded



