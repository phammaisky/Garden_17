function KeyCode(event) {
    return event.keyCode ? event.keyCode : event.which;
}

function OnlyNumber(selector) {
    //Number: >=48 && <=57 | backspace: 8 | delete: 46 | <-: 37 | ->: 39 | dot.: 190 | dotnumpad.: 110 | comma,: 188
    $(selector).on('keypress', function (event) {
        var key = KeyCode(event);
        return (key >= 48 && key <= 57) || key == 8 || key == 46 || key == 37 || key == 39;
    });
}
function OnlyNumberDot(selector) {
    $(selector).on('keypress', function (event) {
        var key = KeyCode(event);
        return (key >= 48 && key <= 57) || key == 8 || key == 46 || key == 37 || key == 39 || key == 190 || key == 110;
    });
}