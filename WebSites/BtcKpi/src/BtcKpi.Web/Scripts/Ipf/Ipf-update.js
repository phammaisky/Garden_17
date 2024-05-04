// #region PageLoaded
$(function () {
    

    bindingDetailTables();

    configDatepicker();

    debugger;
    //FirstLoat or Not
    if ($("#FirstLoad").val() == 'True') {
        $("#ErrorMessage").text("");
        $("#error-block").hide();
    }
    else {
        if ($("#ErrorMessage").text().length != 0) {
            $("#error-block").show();
        } else {
            $("#error-block").hide();
        }
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

    
});
// #endregion PageLoaded

// #region Tab
$("#ipfTabs > li").click(function (e) {    //ul.navbar-nav > li
    e.preventDefault();
    var tabId = jQuery(this).attr("id");
    var currentTabs = $("#ipfTabs > li.active").attr('id')
    if (currentTabs == 'liPersonalPlan' & tabId != 'liPersonalPlan') {
        savePersonalPlan();
    }

    if (currentTabs == 'liOnYear' & tabId != 'liOnYear') {
        saveCompetency();
    }

    if (currentTabs == 'liNextYear' & tabId != 'liNextYear') {
        saveCompetencyNextYear();
    }

    $.ajax({
        type: "GET",
        url: "/Ipf/TabClick",
        data: { tabID: tabId },
        success: function (response) {
            if (response != null) {

            } else {
                alert("Lỗi post Tab click!");
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
// #endregion Tab

// #region CompleteWork table
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

//Edit
$(".complete-work-edit").click(function () {

    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements
    
    $.ajax({
        type: "GET",
        url: "/Ipf/UpdateCompleteWork",
        data: { seq: seq },
        success: function (response) {
            if (response != null) {
                $("#u_completeWorkType").val(response.WorkCompleteID);
                $('#u_hdIpfDetailID').val(response.ID);
                $('#u_hdIpfDetailSeq').val(response.Seq);
                $("#u_objective").val(response.Objective);
                $("#u_target").val(response.Target);
                $("#u_weight").val(response.Weight);
                $("#u_result").val(response.Result);
                $("#u_self-score").val(response.SelfScore);
                $("#u_manager-score").val(response.ManagerScore);

                $('#update-detail-modal').modal('show');
                
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
$(".complete-work-delete").click(function () {

    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements

    $.ajax({
        type: "GET",
        url: "/Ipf/DeleteCompleteWork",
        data: { seq: seq },
        success: function (response) {
            if (response != null) {
                $("#d_completeWorkType").val(response.WorkCompleteID);
                $('#d_hdIpfDetailSeq').val(response.Seq);
                $("#d_objective").val(response.Objective);
                $("#d_target").val(response.Target);
                $("#d_weight").val(response.Weight);
                $("#d_result").val(response.Result);
                $("#d_self-score").val(response.SelfScore);
                $("#d_manager-score").val(response.ManagerScore);

                $('#delete-detail-modal').modal('show');

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
// #endregion CompleteWork table

// #region Tab
$("#add-personal-plan-competency").click(function (e) {    //ul.navbar-nav > li
    savePersonalPlan();
});

//Edit Personal Competency
$(".personal-plan-competency-edit").click(function () {
    savePersonalPlan();

    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements

    $.ajax({
        type: "GET",
        url: "/Ipf/UpdatePersonalPlanCompetency",
        data: { seq: seq },
        success: function (response) {
            if (response != null) {
                $('#u_hdPersonalPlanID').val(response.ID);
                $('#u_hdPersonalPlanSeq').val(response.Seq);
                $("#u_activity").val(response.Activity);
                $("#u_complete-date").val(response.CompleteDateString);
                $("#u_remark").val(response.Remark);

                $('#update-personal-plan-modal').modal('show');

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

//Delete Personal Competency
$(".personal-plan-competency-delete").click(function () {
    savePersonalPlan();

    var $buttonClicked = $(this);
    var seq = $buttonClicked.attr('data-id');
    var $row = $(this).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements

    $.ajax({
        type: "GET",
        url: "/Ipf/DeletePersonalPlanCompetency",
        data: { seq: seq },
        success: function (response) {
            if (response != null) {
                $('#d_hdPersonalPlanID').val(response.ID);
                $('#d_hdPersonalPlanSeq').val(response.Seq);
                $("#d_activity").val(response.Activity);
                $("#d_complete-date").val(response.CompleteDateString);
                $("#d_remark").val(response.Remark);

                $('#delete-personal-plan-modal').modal('show');
            } else {
                alert("Lỗi get kế hoạch phát triển bản thân - năng lực");
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

// #region Modal
//Add Modal
$("#btnAddDetailModal").click(function () {
    if ($("#completeWorkType option:selected").val() == null |
        $("#completeWorkType option:selected").val() == "" |
        $("#objective").val() == "" |
        $("#target").val() == "" |
        $("#weight").val() == "" |
        $("#weight").val() == "0") {
        $("#modalError").show();
        return
    }

    var completeWork = new Object();
    completeWork.WorkCompleteID = $("#completeWorkType option:selected").val();
    completeWork.Objective = $('#objective').val();
    completeWork.Target = $('#target').val();
    completeWork.Weight = $('#weight').val();
    completeWork.Result = $('#result').val();
    completeWork.SelfScore = $('#score').val();
    completeWork.ManagerScore = $('#manager-score').val();
    if (completeWork != null) {
        $.ajax({
            type: "POST",
            url: "/Ipf/AddCompleteWork",
            data: JSON.stringify(completeWork),
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

//Update Modal
$("#btnUpdateDetailModal").click(function () {
    if ($("#u_completeWorkType option:selected").val() == null |
        $("#u_completeWorkType option:selected").val() == "" |
        $("#u_objective").val() == "" |
        $("#u_target").val() == "" |
        $("#u_weight").val() == "" |
        $("#u_result").val() == "0") {
        $("#u_modalError").show();
        return
    }
    var completeWork = new Object();
    completeWork.IpfID = $('#Ipf_ID').val();
    completeWork.ID = $('#u_hdIpfDetailID').val();
    completeWork.WorkCompleteID = $("#u_completeWorkType option:selected").val();
    completeWork.Seq = $('#u_hdIpfDetailSeq').val();
    completeWork.Objective = $('#u_objective').val();
    completeWork.Target = $('#u_target').val();
    completeWork.Weight = $('#u_weight').val();
    completeWork.Result = $('#u_result').val();
    completeWork.SelfScore = $('#u_self-score').val();
    completeWork.ManagerScore = $('#u_manager-score').val();

    if (completeWork != null) {
        $.ajax({
            type: "POST",
            url: "/Ipf/UpdateCompleteWorkPost",
            data: JSON.stringify(completeWork),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#update-detail-modal').modal('hide');
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

//Delete Modal
$("#btnDeleteDetailModal").click(function () {
    var completeWork = new Object();
    completeWork.ID = $('#d_hdIpfDetailID').val();
    completeWork.WorkCompleteID = $("#d_completeWorkType option:selected").val();
    completeWork.Seq = $('#d_hdIpfDetailSeq').val();

    if (completeWork != null) {
        $.ajax({
            type: "POST",
            url: "/Ipf/DeleteCompleteWorkPost",
            data: JSON.stringify(completeWork),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#delete-detail-modal').modal('hide');
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
// #endgion Modal

// #region Modal
//Add Modal
$("#btnAddPersonalPlanModal").click(function () {
    if ($("#activity").val() == "" |
        $("#complete-date").val() == "" |
        $("#remark").val() == "") {
        $("#modalError_pp").show();
        return
    }

    var personalPlan = new Object();
    personalPlan.Activity = $('#activity').val();
    const [day, month, year] = $('#complete-date').val().split("/");
    personalPlan.CompleteDate = new Date(year, month - 1, day);
    personalPlan.Remark = $('#remark').val();
    if (personalPlan != null) {
        $.ajax({
            type: "POST",
            url: "/Ipf/AddPersonalPlanCompetency",
            data: JSON.stringify(personalPlan),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#add-personal-plan-modal').modal('hide');
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

//Update Modal
$("#btnUpdatePersonalPlanModal").click(function () {
    if ($("#u_activity").val() == "" |
        $("#u_complete-date").val() == "" |
        $("#u_remark").val() == "") {
        $("#u_modalError_pp").show();
        return
    }
    var personalPlan = new Object();
    personalPlan.IpfID = $('#Ipf_ID').val();
    personalPlan.ID = $('#u_hdPersonalPlanID').val();
    personalPlan.Seq = $('#u_hdPersonalPlanSeq').val();
    personalPlan.Activity = $('#u_activity').val();
    const [day, month, year] = $('#u_complete-date').val().split("/");
    personalPlan.CompleteDate = new Date(year, month - 1, day);
    personalPlan.Remark = $('#u_remark').val();

    if (personalPlan != null) {
        $.ajax({
            type: "POST",
            url: "/Ipf/UpdatePersonalPlanCompetencyPost",
            data: JSON.stringify(personalPlan),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#update-personal-plan-modal').modal('hide');
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

//Delete Modal
$("#btnDeletePersonalPlanModal").click(function () {
    var personalPlan = new Object();
    personalPlan.ID = $('#d_hdPersonalPlanID').val();
    personalPlan.Seq = $('#d_hdPersonalPlanSeq').val();

    if (personalPlan != null) {
        $.ajax({
            type: "GET",
            url: "/Ipf/DeletePersonalPlanCompetencyPost",
            data: {seq:personalPlan.Seq} ,
            traditional: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    $('#delete-personal-plan-modal').modal('hide');
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
// #endgion Modal

