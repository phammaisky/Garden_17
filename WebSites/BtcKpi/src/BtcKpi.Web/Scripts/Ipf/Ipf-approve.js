// #region PageLoaded
$(function () {
    if ($("#ErrorMessage").text().length > 0) {
        $("#error-block").show();
    } else {
        $("#error-block").hide();
    }

    if ($("#Ipf_ScheduleType").val() == "1") {
        $('#dIpf_ScheduleID').show();
        $("#liPersonalPlan").hide();
        $("#liNextYear").hide();
    } else {
        $('#dIpf_ScheduleID').hide();
        $("#liPersonalPlan").show();
        $("#liNextYear").show();
    }

    // Binding On Year Complete Work table
    debugger;
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
                $('td:eq(1)', row).attr('colspan', 7);

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



