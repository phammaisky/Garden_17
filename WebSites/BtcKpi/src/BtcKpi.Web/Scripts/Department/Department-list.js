$(function () {
    var menuItem = $('#left-sidebar-menu-upf-self');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-upf-self');
    subMenuItem.addClass('active');

    //First time -> Get All
    $("#btnSearch").click();

});

function configDatepicker() {
    ////Date picker
    $('.complete-date').datepicker({
        format: 'dd/mm/yyyy'
    });

    //Alternativ way
    $('.complete-date').datepicker({
        format: "dd/mm/yyyy"
    }).on('change', function () {
        $('.datepicker').hide();
    });
}

function companySearchChange(val) {
    $.ajax({
        type: "POST",
        url: "/Department/CompanyChange",
        data: "companyId=" + JSON.stringify(val), //'{companyId: "' + val + '" }',
        success: function (response) {
            if (response != null) {
                $("#DepartmentID").html(response);
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            //alert(response.responseText);
            $("#DepartmentID").html(response.responseText);
        }
    });
}

function scheduleSearchChange(val) {
    if (val == "0") {
        $('#depart_ScheduleID').hide();
    } else {
        $('#depart_ScheduleID').show();
    }
}

$("#btnSearch").click(function startCreate() {
    
    var upfSearch = new Object();
    upfSearch.CompanyID = $("#CompanyID").val();
    if (upfSearch.CompanyID.length == 0) {
        var companyID = "";
        $("#CompanyID option").each(function () {
            companyID = companyID + $(this).val() + ",";
        });
        upfSearch.CompanyID = companyID;
    }

    upfSearch.DepartmentID = $("#DepartmentID").val();
    if (upfSearch.DepartmentID.length == 0) {
        var departmentID = "";
        $("#DepartmentID option").each(function () {
            departmentID = departmentID + $(this).val() + ",";
        });
        upfSearch.DepartmentID = departmentID;
    }

    upfSearch.ScheduleType = $("#ScheduleType").val();

    upfSearch.Year = $("#Year").val();

    upfSearch.ScheduleID = $("#ScheduleID").val();

    upfSearch.StatusID = $("#StatusID").val();

    $.ajax({
        type: "POST",
        url: "/Department/SearchDepartmentList",
        data: JSON.stringify(upfSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $("#depart-list-table").dataTable().fnDestroy();
                $("#depart-list-table").html(response);
                get_ajax_table();
            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            $("#depart-list-table").dataTable().fnDestroy();
            $("#depart-list-table").html(response.responseText);
            get_ajax_table();
        }
    });


});

$('#depart-list-table').on('click', '.depart-delete', function () {
    if (confirm("Bạn có chắc chắn muốn xóa không?")) {
        var $buttonClicked = $(this);
        var departId = $buttonClicked.attr('data-id');
        var depart = new Object();
        depart.ID = departId;
        $.ajax({
            type: "POST",
            url: "/Department/DepartKPIsDelete",
            data: JSON.stringify(depart),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(response) {
                if (response != null) {
                    $("#depart-list-table").html(response);
                } else {
                    alert("Kiểm tra lại dữ liệu");
                }
            },
            failure: function(response) {
                alert(response.responseText);
            },
            error: function(response) {
                //alert(response.responseText);
                $("#depart-list-table").html(response.responseText);
            }
        });
    }
});

function get_ajax_table() {
    $('#depart-list-table').dataTable({
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">'
    });
}
