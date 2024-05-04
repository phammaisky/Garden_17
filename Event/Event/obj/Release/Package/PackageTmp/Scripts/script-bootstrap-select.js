$(document).ready(function () 
{
    // Enable Live Search.
    $('#MeetingRoomList').attr('data-live-search', true);

    $('.selectMeetingRoom').selectpicker(
    {
        width: '100%',
        title: '- [Choose Country] -',
        style: 'btn-warning',
        size: 6
    });
});  

