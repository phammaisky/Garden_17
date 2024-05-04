// #region PageLoaded
$(function () {
    if($("#ErrorMessage").text().length > 0){
        $("#error-block").show();
    } else {
        $("#error-block").hide();
    }

    bindingDetailTablesView();

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

//View
$(".complete-work-view").click(function () {

    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements

    $.ajax({
        type: "GET",
        url: "/Ipf/ViewCompleteWork",
        data: { seq: seq },
        success: function (response) {
            if (response != null) {
                $("#v_completeWorkType").val(response.WorkCompleteID);
                $("#v_objective").val(response.Objective);
                $("#v_target").val(response.Target);
                $("#v_weight").val(response.Weight);
                $("#v_result").val(response.Result);
                $("#v_self-score").val(response.SelfScore);
                $("#v_manager-score").val(response.ManagerScore);
                $("#v_bod-score").val(response.BodScore);

                $('#view-detail-modal').modal('show');

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



