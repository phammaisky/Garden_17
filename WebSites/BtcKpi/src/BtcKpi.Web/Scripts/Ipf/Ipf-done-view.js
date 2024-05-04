// #region PageLoaded
$(function () {
    $("#ErrorMessage").text("");
    $("#error-block").hide();

    bindingDetailTables();

    if ($("#Ipf_ScheduleType").val() == "1") {
        $('#dIpf_ScheduleID').show();
        $("#liPersonalPlan").hide();
        $("#liNextYear").hide();
    } else {
        $('#dIpf_ScheduleID').hide();
        $("#liPersonalPlan").show();
        $("#liNextYear").show();
    }
});
// #endregion PageLoaded



