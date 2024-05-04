// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-ipf');
    menuItem.addClass('active');

    var oldSubMenuItem = menuItem.find('#left-sidebar-menu-ipf-peding');
    oldSubMenuItem.removeClass('active');

    var subMenuItem = menuItem.find('#left-sidebar-menu-ipf-pending-create');
    subMenuItem.addClass('active');

    // Binding On Year Complete Work table
    var completeWork = $("#ipf-complete-work").DataTable({
        "paging": false,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">',
        createdRow: function (row, data, dataIndex) {
            // If is Work Title
            if (data[0] === 'True') {
                // Add COLSPAN attribute
                $('td:eq(1)', row).attr('colspan', 6);

                // Left horizontally
                $('td:eq(1)', row).attr('align', 'left');

                // Hide required number of columns
                // next to the cell with COLSPAN attribute
                $('td:eq(0)', row).css('display', 'none');
                $('td:eq(2)', row).css('display', 'none');
                $('td:eq(3)', row).css('display', 'none');
                $('td:eq(4)', row).css('display', 'none');
                $('td:eq(5)', row).css('display', 'none');

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
            // If is Work Title
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

    //FirstLoat or Not
    if ($("#FirstLoad").val() == 'True') {

        $("#ErrorMessage").text("");
        $("#error-block").hide();
        $("#kpi-block").hide();

        $("#Ipf_ScheduleType").attr("disabled", false);
        $("#Ipf_Year").attr("disabled", false);
        $("#Ipf_ScheduleID").attr("disabled", false);

        $("#btnStart").attr("disabled", false);

        $('#dIpf_ScheduleID').hide();
        $("#liPersonalPlan").hide();
        $("#liNextYear").hide();
    }
    else {

        if ($("#ErrorMessage").text().length != 0) {
            $("#error-block").show();
        } else {
            $("#error-block").hide();
        }
        
        $("#kpi-block").show();

        $("#Ipf_ScheduleType").attr("disabled", true);
        $("#Ipf_Year").attr("disabled", true);
        $("#Ipf_ScheduleID").attr("disabled", true);

        $("#btnStart").attr("disabled", true);

        if ($("#Ipf_ScheduleType").val() == "1") {
            $('#dIpf_ScheduleID').show();
            $("#liPersonalPlan").hide();
            $("#liNextYear").hide();
        } else {
            $('#dIpf_ScheduleID').hide();
            $("#liPersonalPlan").show();
            $("#liNextYear").show();
        }
    }

    configDatepicker();

    var $amount_fields = $('input[class="Competencies_Weight"][type="number"][id*="Competencies_"]');
    $total_amount = $('#lblCompetencyWeightToal');

    $amount_fields.on('input', function (e) {
        var final_value = 0;
        $amount_fields.each(function () {
            var value = $(this).val();
            if (!isNaN(value) && value > 0) final_value += parseInt(value);
        });
        $total_amount.text(final_value);
    });
    
});
// #endregion PageLoaded


// #region Info
function scheduleChange(val) {
    if (val == "0") {
        $('#dIpf_ScheduleID').hide();
        $("#liPersonalPlan").show();
        $("#liNextYear").show();
    } else {
        $('#dIpf_ScheduleID').show();
        $("#liPersonalPlan").hide();
        $("#liNextYear").hide();
    }
}

$("#btnStart").click(function startCreate() {
    if ($("#Ipf_ScheduleType").val() == "") {
        $("#ErrorMessage").text("Bạn chưa chọn Loại");
        $("#error-block").show();
        return;
    } 

    if ($("#Ipf_Year").val() == "") {
        $("#ErrorMessage").text("Bạn chưa chọn Năm");
        $("#error-block").show();
        return;
    }

    if ($("#Ipf_ScheduleType").val() == "1" & $("#Ipf_ScheduleID").val() == "") {
        $("#ErrorMessage").text("Bạn chưa chọn Kỳ");
        $("#error-block").show();
        return;
    }

    if ($("#Ipf_BodId").val() == "") {
        $("#ErrorMessage").text("Bạn chưa chọn CEO/BOD/Final");
        $("#error-block").show();
        return;
    }

    $("#ErrorMessage").text("");
    $("#error-block").hide();

    var ipf = new Object();
    ipf.ScheduleType = $("#Ipf_ScheduleType").val();
    ipf.Year = $("#Ipf_Year").val();
    ipf.ScheduleID = $("#Ipf_ScheduleID").val();
    ipf.BodId = $("#Ipf_BodId").val();

    $.ajax({
        type: "POST",
        url: "/Ipf/Start",
        data: JSON.stringify(ipf),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                if (response.ErrorMessage.length === 0) {
                    $("#ErrorMessage").text("");
                    $("#error-block").hide();
                    $("#kpi-block").show();

                    $("#Ipf_ScheduleType").attr("disabled", true);
                    $("#Ipf_Year").attr("disabled", true);
                    $("#Ipf_ScheduleID").attr("disabled", true);

                    $("#btnStart").attr("disabled", true);
                } else {
                    $("#ErrorMessage").text(response.ErrorMessage);
                    $("#error-block").show();
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
        type: "POST",
        url: "/Ipf/Reset",
        data: null,
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
});
// #endregion Info

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

    //$("#completeWorkType").val(completeWorkId);

    //$("#objective").val($row.find('td:eq(1)').text())
    //$("#target").val($row.find('td:eq(2)').text())
    //$("#weight").val($row.find('td:eq(3)').text())
    //$("#result").val($row.find('td:eq(4)').text())
    //$("#score").val($row.find('td:eq(5)').text())
    
    $.ajax({
        type: "GET",
        url: "/Ipf/UpdateCompleteWork",
        data: { seq: seq },
        success: function (response) {
            if (response != null) {
                $("#u_completeWorkType").val(response.WorkCompleteID);
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

    //$("#completeWorkType").val(completeWorkId);

    //$("#objective").val($row.find('td:eq(1)').text())
    //$("#target").val($row.find('td:eq(2)').text())
    //$("#weight").val($row.find('td:eq(3)').text())
    //$("#result").val($row.find('td:eq(4)').text())
    //$("#score").val($row.find('td:eq(5)').text())

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
        return;
    }
    
    var completeWork = new Object();
    completeWork.WorkCompleteID = $("#completeWorkType option:selected").val();
    completeWork.Objective = $('#objective').val();
    completeWork.Target = $('#target').val();
    completeWork.Weight = $('#weight').val();
    completeWork.Result = $('#result').val();
    completeWork.SelfScore = $('#self-score').val();
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
    debugger;
    var completeWork = new Object();
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
    const [day, month, year] = $('#complete-date').val().split("/")
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
    personalPlan.ID = $('#u_hdPersonalPlanID').val();
    personalPlan.Seq = $('#u_hdPersonalPlanSeq').val();

    if (personalPlan != null) {
        $.ajax({
            type: "POST",
            url: "/Ipf/DeletePersonalPlanCompetencyPost",
            data: JSON.stringify(personalPlan),
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

