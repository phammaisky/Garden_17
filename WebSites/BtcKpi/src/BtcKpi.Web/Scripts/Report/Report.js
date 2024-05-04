function companySearchChange(val) {
    $.ajax({
        type: "POST",
        url: "/Report/CompanyChange",
        data: "companyId=" + JSON.stringify(val), //'{companyId: "' + val + '" }',
        dataType: "html",
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

