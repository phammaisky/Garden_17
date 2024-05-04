// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-upf');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-upf-cross');
    subMenuItem.addClass('active');

    $("#ErrorMessage").text("");
    $("#error-block").hide();

    SetEnableByFromTo();

});
// #endregion PageLoaded