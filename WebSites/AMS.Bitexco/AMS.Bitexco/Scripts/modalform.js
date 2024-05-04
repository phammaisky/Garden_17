$(function () {

    $.ajaxSetup({ cache: false });

    $(".create_modal").on("click", function (e) {

        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

        $('#CRUDDialogContent').html(AlertDialog());

        $('#CRUDDialog').modal({
            backdrop: 'static',
            keyboard: true
        }, 'show');

        $('#CRUDDialogContent').load(this.href, function (response, status, xhr) {
            if (status == "error") {               
                $("#CRUDDialogContent").html(AlertErrorDialog());
            }
            else
            {
                //var ele = $("#CRUDDialogContent").find("#AssetsCode").focus();                
                bindForm(this);
            }
        });
        return false;
    });

    $(".create_modal_Upload").on("click", function (e) {

        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

        $('#CRUDDialogContent').html(AlertDialog());

        $('#CRUDDialog').modal({
            backdrop: 'static',
            keyboard: true
        }, 'show');

        $('#CRUDDialogContent').load(this.href, function (response, status, xhr) {
            if (status == "error") {
                $("#CRUDDialogContent").html(AlertErrorDialog());
            }
            else {
                //var ele = $("#CRUDDialogContent").find("#AssetsCode").focus();                
                bindFormUpload(this);
            }
        });
        return false;
    });

    $(".create_sub_modal").on("click", function (e) {
        
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

        $('#CRUDSubDialogContent').html(AlertDialog());

        $('#CRUDSubDialog').modal({
            backdrop: 'static',
            keyboard: true
        }, 'show');

        $('#CRUDSubDialogContent').load(this.href, function (response, status, xhr) {
            if (status == "error") {
                $("#CRUDSubDialogContent").html(AlertErrorDialog());
            }
            else {                            
                bindSubForm(this);
            }
        });
        return false;
    });

    $(".modal").draggable({
        handle: ".modal-header"
    });
});


function opentForm(href) {
    $('[data-toggle="tooltip"]').tooltip();
    $('#CRUDDialogContent').html(AlertDialog());
    $('#CRUDDialog').modal({
        backdrop: 'static',
        keyboard: true
    }, 'show');

    $('#CRUDDialogContent').load(href, function (response, status, xhr) {
        if (status == "error") {
            $("#CRUDDialogContent").html(AlertErrorDialog());
        }
        else {
            bindForm(this);
        }
    });
    return false;

    $(".modal").draggable({
        handle: ".modal-header"
    });
}

function opentSubForm(href) {
    $('[data-toggle="tooltip"]').tooltip();
    $('#CRUDSubDialogContent').html(AlertDialog());
    $('#CRUDSubDialog').modal({
        backdrop: 'static',
        keyboard: true
    }, 'show');

    $('#CRUDSubDialogContent').load(href, function (response, status, xhr) {
        if (status == "error") {
            $("#CRUDSubDialogContent").html(AlertErrorDialog());
        }
        else {
            bindSubForm(this);
        }
    });
    return false;

    $(".modal").draggable({
        handle: ".modal-header"
    });
}

function bindForm(dialog) {

    $('form', dialog).submit(function () {
        $('.submit').button('loading');
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#CRUDDialog').modal('hide');
                    formSubmit();
                } else {
                    $('#CRUDDialogContent').html(result);
                    bindForm();
                }
            },
            error: function (xhr, status, error) {               
                $('#CRUDDialogContent').html(AlertErrorDialog());
            }
        });
        return false;
    });
}

function bindFormUpload(dialog) {

    $('form', dialog).submit(function () {
        $('.submit').button('loading');
        var dataString = new FormData(this);
        $.ajax({
            url: this.action,
            type: this.method,
            data: dataString,
            success: function (result) {
                if (result.success) {
                    $('#CRUDDialog').modal('hide');
                    formSubmit();
                } else {
                    $('#CRUDDialogContent').html(result);
                    formSubmit();
                    bindFormUpload();
                }
            },
            error: function (xhr, status, error) {
                $('#CRUDDialogContent').html(AlertErrorDialog());
            }
        });
        return false;
    });
}

function bindSubForm(dialog) {

    $('form', dialog).submit(function () {
        $('.submit').button('loading');
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#CRUDSubDialog').modal('hide');
                    formSubSubmit();
                } else {
                    $('#CRUDSubDialogContent').html(result);
                    bindSubForm();
                }
            },
            error: function (xhr, status, error) {
                $('#CRUDSubDialogContent').html(AlertErrorDialog());
            }
        });
        return false;
    });
}

function AlertDialog()
{
    var htmlText = "";
    
    htmlText += "<div id='contentProccessing'>";
    htmlText += "<div class='modal-dialog'>";
    htmlText += "<div class='modal-content'>";
    htmlText += "<div class='modal-header'>";
    htmlText += "<button type='button' class='close' data-dismiss='modal'><span aria-hidden='true'>&times;</span><span class='sr-only'>Đóng</span></button>";
    htmlText += "<h4 class='modal-title' id='myModalLabel'>Thông báo</h4>";
    htmlText += "</div>";
    htmlText += "<div class='modal-body' style='min-height:200px;'>";
    htmlText += "<div class='progress-bar progress-bar-striped active' role='progressbar' aria-valuenow='100' aria-valuemin='0' aria-valuemax='100' style='width: 100%'>";
    htmlText += " Đang xử lý dữ liệu! ";
    htmlText += "</div>";
    htmlText += "</div>";
    htmlText += "</div>";
    htmlText += "</div>";
    htmlText += "</div>";
    return htmlText;
}

function AlertErrorDialog(msg) {

    var message = "<h4 class='text-danger'>Chương trình xảy ra lỗi!</h4>";
    message += "<h5 class='text-danger'> - Có thể do máy chủ bị quá tải hoặc kết nối đến máy chủ bị gián đoạn!</h5>";
    message += "<h5 class='text-danger'> - Hãy thử thực hiện lại chức năng. Nếu chương trình vẫn tiếp tục lỗi, xin thông báo lại với quản trị hệ thống.</h5>";
   

    if (msg != null)
        message += msg;
    var htmlText = "";

    htmlText += "<div id='contentProccessing'>";
    htmlText += "<div class='modal-dialog'>";
    htmlText += "<div class='modal-content'>";
    htmlText += "<div class='modal-header'>";
    htmlText += "<button type='button' class='close' data-dismiss='modal'><span aria-hidden='true'>&times;</span><span class='sr-only'>Đóng</span></button>";
    htmlText += "<h4 class='modal-title' id='myModalLabel'>Thông báo</h4>";
    htmlText += "</div>";
    htmlText += "<div class='modal-body' style='min-height:200px;'>";
    htmlText += message;
    htmlText += "</div>";
    htmlText += "</div>";
    htmlText += "</div>";
    htmlText += "</div>";
    return htmlText;
}


jQuery.download = function (url, data, method) {
    //url and data options required
    if (url && data) {
        //data can be string of parameters or array/object
        data = typeof data == 'string' ? data : jQuery.param(data);
        //split params into form inputs
        var inputs = '';
        jQuery.each(data.split('&'), function () {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });
        //send request
        jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>')
		.appendTo('body').submit().remove();
    };
};

