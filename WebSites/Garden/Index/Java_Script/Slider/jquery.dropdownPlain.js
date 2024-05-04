$(function () {

    $("ul.mainnav li").hover(function () {

        $(this).find('ul:first').css({ visibility: "visible", display: "none" }).show(400);
    }, function () {
        $(this).find('ul:first').css({ visibility: "hidden" });

    });

    $("ul.mainnav li ul li:has(ul)").find("a:first").append("");

});

$(function () {

    $("ul.listnav li").hover(function () {

        $(this).addClass("hover");
        $('ul:first', this).css('visibility', 'visible');

    }, function () {

        $(this).removeClass("hover");
        $('ul:first', this).css('visibility', 'hidden');

    });

    $("ul.listnav li ul li:has(ul)").find("a:first").append(" &raquo; ");

});