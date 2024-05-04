// #region PageLoaded
$(function () {
    var menuItem = $('#left-sidebar-menu-report');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-report-upf');
    subMenuItem.addClass('active');

    
    $('#upf-month-table').DataTable({
        "scrollX": true,
        "scrollY": 200
    });
    $('.dataTables_length').addClass('bs-select');

    
});
// #endregion PageLoaded
