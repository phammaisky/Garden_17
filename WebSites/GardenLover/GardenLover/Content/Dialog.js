function CreateDialog(title) {

    if ($('#DialogHolder').length == 0)
        $('body').append("<div id='DialogHolder' title='" + title + "' style='display: none; z-index: 999999;'>");

    if ($('#HiddenLink').length == 0)
        $('#DialogHolder').append("<a id='HiddenLink' href='#' onclick='return false;' style='display: none;'>");

    $('#HiddenLink').focus();
}

function ShowDialog(title, content) {

    CreateDialog(title);

    $('#DialogHolder').dialog({
        modal: true
    });

    $('#DialogHolder').html(content.toString());
    $("#DialogHolder").dialog("open");

    ResizeDialogFull();
}

function ResizeDialogFull() {

    var wHeight = $(window).innerHeight();
    var wWidth = $(window).innerWidth();

    $('.ui-dialog-title').addClass('B Red S12');
    $('.ui-dialog-title').css('text-align', 'center');

    $('.ui-button-text').addClass('B Red S16');
    $('.ui-button').css('background-color', 'white !important');
    $('.ui-button').css('border', '1px dashed red');
    $('.ui-button').css('-webkit-border-radius', '10px');
    $('.ui-button').css('-moz-border-radius', '10px');
    $('.ui-button').css('border-radius', '10px');

    if ($("#InsideDialog").attr('title'))
        $('.ui-dialog-title').html($("#InsideDialog").attr('title'));

    ResizeDialogWHTL(wWidth, wHeight);

    window.setTimeout(function () {
        ResizeDialogMin();
    }, 100);
}

function ResizeDialogMin() {
    ResizeDialogWHTL($("#InsideDialog").width() + 60, $("#InsideDialog").height() + 60);
}

function ResizeDialogWHTL(dialogWidth, dialogHeight) {

    var wHeight = $(window).innerHeight();
    var wWidth = $(window).innerWidth();

    var padTop = 20;
    var padBottom = 20;

    var padLeft = 20;
    var padRight = 20;

    if (dialogWidth > wWidth - padLeft - padRight)
        dialogWidth = wWidth - padLeft - padRight;

    if (dialogHeight > wHeight - padTop - padBottom)
        dialogHeight = wHeight - padTop - padBottom;

    $("#DialogHolder").dialog("option", "width", dialogWidth);
    $("#DialogHolder").dialog("option", "height", dialogHeight);

    var dialogTop = (wHeight - $('.ui-dialog').height()) / 2;
    var dialogLeft = (wWidth - $('.ui-dialog').width()) / 2;

    if (dialogTop < padTop)
        dialogTop = padTop;

    if (dialogLeft < padLeft)
        dialogLeft = padLeft;

    $('.ui-dialog').css('z-index', '999999');
    $('.ui-dialog').css('top', dialogTop + 'px');
    $('.ui-dialog').css('left', dialogLeft + 'px');

    if ($("#InsideButton").css('position') == "fixed") {
        var top = parseInt($("#InsideButton").css('top'));
        $("#InsideButton").css('top', top + dialogTop);
    }
}