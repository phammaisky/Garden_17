// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-upf');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-upf-cross');
    subMenuItem.addClass('active');

    if ($("#ErrorMessage").text().length == 0) {
        $("#ErrorMessage").text("");
        $("#error-block").hide();
    } else {
        $("#error-block").show();
    }

    

});
// #endregion PageLoaded

function departmentChange(val) {
    debugger;
    $("#Detail_ToName").val($("#Detail_ToDepartment option:selected").text());
}


