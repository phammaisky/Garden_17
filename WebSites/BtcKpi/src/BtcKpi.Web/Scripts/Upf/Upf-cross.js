$(function () {
    var menuItem = $('#left-sidebar-menu-upf');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-upf-cross');
    subMenuItem.addClass('active');

    var upfCross = $("#upf-cross-table").DataTable({
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    $("#btnSearch").click();

});

function companySearchChange(val) {
    $.ajax({
        type: "POST",
        url: "/Upf/CompanyChange",
        data: "companyId=" + JSON.stringify(val), //'{companyId: "' + val + '" }',
        dataType: "html",
        success: function (response) {
            if (response != null) {
                $("#FromDepartmentID").html(response);
                $("#ToDepartmentID").html(response);
            } else {
                alert("Lỗi post Tab click!");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            //alert(response.responseText);
            $("#FromDepartmentID").html(response.responseText);
            $("#ToDepartmentID").html(response.responseText);
        }
    });
}

$("#btnSearch").click(function startCreate() {
    
    var upfSearch = new Object();
    upfSearch.Status = $("#Status").val();
    upfSearch.UserDepartmentID = $("#UserDepartmentID").val();

    upfSearch.CompanyID = $("#CompanyID").val();
    if (upfSearch.CompanyID.length == 0) {
        var companyID = "";
        $("#CompanyID option").each(function () {
            companyID = companyID + $(this).val() + ",";
        });
        upfSearch.CompanyID = companyID;
    }

    upfSearch.FromDepartmentID = $("#FromDepartmentID").val();
    if (upfSearch.FromDepartmentID.length == 0) {
        var departmentID = "";
        $("#FromDepartmentID option").each(function () {
            departmentID = departmentID + $(this).val() + ",";
        });
        upfSearch.FromDepartmentID = departmentID;
    }

    upfSearch.ToDepartmentID = $("#ToDepartmentID").val();
    if (upfSearch.ToDepartmentID.length == 0) {
        var toDepartmentID = "";
        $("#ToDepartmentID option").each(function () {
            toDepartmentID = toDepartmentID + $(this).val() + ",";
        });
        upfSearch.ToDepartmentID = toDepartmentID;
    }

    upfSearch.Year = $("#Year").val();
    if (upfSearch.Year.length == 0) {
        var year = "";
        $("#Year option").each(function () {
            year = year + $(this).val() + ",";
        });
        upfSearch.Year = year;
    }

    upfSearch.Month = $("#Month").val();
    if (upfSearch.Month.length == 0) {
        var month = "";
        $("#Month option").each(function () {
            month = month + $(this).val() + ",";
        });
        upfSearch.Month = month;
    }

    upfSearch.Status = $("#Status").val();
    

    $.ajax({
        type: "POST",
        url: "/Upf/Search",
        data: JSON.stringify(upfSearch),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {
                $('#upf-cross-table').dataTable().fnDestroy();
                $("#upf-cross-table").html(response);
                get_ajax_table();

            } else {
                alert("Kiểm tra lại dữ liệu");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            //alert(response.responseText);
            $('#upf-cross-table').dataTable().fnDestroy();
            $("#upf-cross-table").html(response.responseText);
            get_ajax_table();
        }
    });
});

function get_ajax_table() {
    $('#upf-cross-table').dataTable({
        //"bRetrieve": true,
        //"bDestroy": true,
        //"aoColumns": [{ "bSortable": false }, { "bSortable": false }, { "bSortable": false }, { "bSortable": false }, { "bSortable": false }, null, null, null, { "bSortable": false }, null, { "bSortable": false }],
        //"bJQueryUI": false,
        //"bPaginate": true,
        //"sPaginationType": "full_numbers",
        //"bFilter": false,
        //"bAutoWidth": false,
        //"bInfo": true,
        //"bLengthChange": false,
        //"aaSorting": [[9, "asc"]],
        //"oLanguage": {
        //    "sUrl": Y_R + "/js/dataTablesTranslations/" + LANGUAGE + ".txt"
        //}
        "paging": true,
        "ordering": false,
        "searching": false,
        "info": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">'
    });

}
