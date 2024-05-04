$(function () {
    $.validator.addMethod('date',
    function (value, element) {
        if (this.optional(element)) {
            return true;
        }
        var valid = true;
        try {
            $.datepicker.parseDate('mm/dd/yy', value);
        }
        catch (err) {
            valid = false;
        }
        return valid;
    }, "Vui lòng nhập ngày theo định dạng mm/dd/yyyy.");
    //$(".datetype").datepicker({ dateFormat: 'dd/mm/yy' });
    $(".datetype").datepicker({ dateFormat: 'mm/dd/yy', changeMonth: true, changeYear: true, yearRange: '-100:+0' });
});
