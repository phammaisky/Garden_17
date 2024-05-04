function Setup_jScrollPane_Vertical(jScrollPane_ID) {

    try {

        if (Check_Element_Is_Not_Null(jScrollPane_ID)) {

            var jScrollPane_width = parseInt($('#' + jScrollPane_ID).width());
            var jScrollPane_height = parseInt($('#' + jScrollPane_ID).height());

            if ((jScrollPane_width > 20) && (jScrollPane_height > 20)) {

                $('#' + jScrollPane_ID).jScrollPane();
                $('#' + jScrollPane_ID).attr('class', 'jspScrollable');

                $('#' + jScrollPane_ID).find('div').each(function () {

                    if ($(this).parent().parent().attr('id') == jScrollPane_ID) {

                        if ($(this).attr('class') == 'jspVerticalBar') {
                            $(this).show();
                        }

                        if ($(this).attr('class') == 'jspHorizontalBar') {
                            $(this).hide();
                        }
                    }
                });

                Resize_jScrollPane_Vertical(jScrollPane_ID);
            }
            else {
                Remove_jScrollPane(jScrollPane_ID);
            }
        }
    } catch (e) {
    }
}

function Resize_jScrollPane_Vertical(jScrollPane_ID) {

    try {
        if (Check_Element_Is_Not_Null(jScrollPane_ID)) {

            var jScrollPane_width = parseInt($('#' + jScrollPane_ID).width());
            var jScrollPane_height = parseInt($('#' + jScrollPane_ID).height());

            if ((jScrollPane_width > 20) && (jScrollPane_height > 20)) {

                $('#' + jScrollPane_ID).find('div').each(function () {

                    if ($(this).parent().attr('id') == jScrollPane_ID) {

                        if ($(this).attr('class') == 'jspContainer') {
                            $(this).width(jScrollPane_width);
                            $(this).height(jScrollPane_height);
                        }
                    }

                    if ($(this).parent().parent().attr('id') == jScrollPane_ID) {

                        if ($(this).attr('class') == 'jspPane') {

                            $(this).width(jScrollPane_width);//$(this).width(jScrollPane_width - 20);
                            $(this).css('left', '');
                        }
                    }

                    if ($(this).parent().parent().parent().attr('id') == jScrollPane_ID) {

                        if ($(this).attr('class') == 'jspTrack') {
                            $(this).height(jScrollPane_height);
                        }
                    }
                });
            }
            else {
                Remove_jScrollPane(jScrollPane_ID);
            }
        }
    } catch (e) {
    }
}

function Setup_jScrollPane_Horizontal(jScrollPane_ID) {

    try {
        if (Check_Element_Is_Not_Null(jScrollPane_ID)) {

            var jScrollPane_width = parseInt($('#' + jScrollPane_ID).width());
            var jScrollPane_height = parseInt($('#' + jScrollPane_ID).height());

            if ((jScrollPane_width > 20) && (jScrollPane_height > 20)) {

                $('#' + jScrollPane_ID).jScrollPane();
                $('#' + jScrollPane_ID).attr('class', 'jspScrollable');

                $('#' + jScrollPane_ID).find('div').each(function () {

                    if ($(this).parent().parent().attr('id') == jScrollPane_ID) {

                        if ($(this).attr('class') == 'jspHorizontalBar') {
                            $(this).show();
                        }

                        if ($(this).attr('class') == 'jspVerticalBar') {
                            $(this).hide();
                        }
                    }
                });
            }
            else {
                Remove_jScrollPane(jScrollPane_ID);
            }
        }
    } catch (e) {
    }
}

function Setup_jScrollPane_Vertical_On_Hover(jScrollPane_ID) {

    try {
        if (Check_Element_Is_Not_Null(jScrollPane_ID)) {

            $('#' + jScrollPane_ID).hover(
        function () {
            window.setTimeout(function () { Setup_jScrollPane_Vertical(jScrollPane_ID); }, 0.5 * 1000);
        }, function () {
            Hide_jScrollPane(jScrollPane_ID);
        }
    );
        }
    } catch (e) {
    }
}

function ALL_Setup_jScrollPane_Vertical_Childen(Parent_ID) {

    try {
        $('#' + Parent_ID).find('div').each(function () {

            if ($(this).attr('class') == 'jspScrollable') {
                if ($(this).attr('id')) {
                    Setup_jScrollPane_Vertical($(this).attr('id'));
                }
            }
        });
    } catch (e) {
    }
}

function Setup_Wait_jScrollPane_Vertical(jScrollPane_ID) {

    try {
        if (Check_Element_Is_Not_Null(jScrollPane_ID)) {

            if (($('#' + jScrollPane_ID).attr('id')) && ($('#' + jScrollPane_ID).attr('class') == 'Wait_Setup_jScrollPane_Vertical')) {

                $('#' + jScrollPane_ID).attr('class', '');
                Setup_jScrollPane_Vertical($('#' + jScrollPane_ID).attr('id'));
            }
        }
    } catch (e) {
    }
}

function ALL_Setup_Wait_jScrollPane_Vertical_Childen(Parent_ID) {

    try {
        $('#' + Parent_ID).find('div').each(function () {

            if ($(this).attr('class') == 'jspScrollable') {

                var jScrollPane_width = parseInt($(this).width());
                var jScrollPane_height = parseInt($(this).height());

                if ((jScrollPane_width < 20) || (jScrollPane_height < 20)) {

                    Remove_jScrollPane($(this).attr('id'));
                }
            }

            //
            if (($(this).attr('id')) && ($(this).attr('class') == 'Wait_Setup_jScrollPane_Vertical')) {

                $(this).attr('class', '');
                Setup_jScrollPane_Vertical($(this).attr('id'));
            }
        });
    } catch (e) {
    }
}

function Remove_jScrollPane(jScrollPane_ID) {

    try {
        if (Check_Element_Is_Not_Null(jScrollPane_ID)) {

            var jScrollPane = $('#' + jScrollPane_ID).jScrollPane();

            var api = jScrollPane.data('jsp');

            if (api != null) {
                api.destroy();
            }

            $('#' + jScrollPane_ID).attr('class', 'Wait_Setup_jScrollPane_Vertical');
        }
    } catch (e) {
    }
}

function ALL_Remove_jScrollPane(Parent_ID) {

    try {
        $('#' + Parent_ID).find('div').each(function () {

            if ($(this).attr('class') == 'jspScrollable') {
                if ($(this).attr('id')) {
                    Remove_jScrollPane($(this).attr('id'));
                }
            }
        });
    } catch (e) {
    }
}

function Hide_jScrollPane(jScrollPane_ID) {

    try {
        if (Check_Element_Is_Not_Null(jScrollPane_ID)) {

            $('#' + jScrollPane_ID).find('div').each(function () {

                if ($(this).parent().parent().attr('id') == jScrollPane_ID) {

                    if (($(this).attr('class') == 'jspVerticalBar') || ($(this).attr('class') == 'jspHorizontalBar')) {
                        $(this).hide();
                    }
                }
            });
        }
    } catch (e) {
    }
}

function Display_jScrollPane(jScrollPane_ID) {

    try {
        if (Check_Element_Is_Not_Null(jScrollPane_ID)) {

            $('#' + jScrollPane_ID).find('div').each(function () {

                if ($(this).parent().parent().attr('id') == jScrollPane_ID) {

                    if ($(this).attr('class') == 'jspVerticalBar') {
                        $(this).show();
                    }
                }
            });
        }
    } catch (e) {
    }
}

function Hide_jScrollPane_AND_Resize_Container(jScrollPane_ID) {

    try {
        if (Check_Element_Is_Not_Null(jScrollPane_ID)) {

            var jScrollPane_width = parseInt($('#' + jScrollPane_ID).width());
            var jScrollPane_height = parseInt($('#' + jScrollPane_ID).height());

            $('#' + jScrollPane_ID).find('div').each(function () {

                if ($(this).parent().parent().attr('id') == jScrollPane_ID) {

                    if (($(this).attr('class') == 'jspVerticalBar') || ($(this).attr('class') == 'jspHorizontalBar')) {
                        $(this).hide();
                    }
                }

                if ($(this).parent().attr('id') == jScrollPane_ID) {

                    if ($(this).attr('class') == 'jspContainer') {

                        $(this).width(jScrollPane_width);
                        $(this).height(jScrollPane_height);
                    }
                }

                if ($(this).parent().parent().attr('id') == jScrollPane_ID) {

                    if ($(this).attr('class') == 'jspPane') {

                        $(this).width(jScrollPane_width);
                        $(this).css('left', '');
                    }
                }

                if ($(this).parent().parent().parent().attr('id') == jScrollPane_ID) {

                    if ($(this).attr('class') == 'jspTrack') {
                        $(this).height(jScrollPane_height);
                    }
                }
            });
        }
    } catch (e) {
    }
}

function ALL_Hide_jScrollPane(Parent_ID) {

    try {
        $('#' + Parent_ID).find('div').each(function () {

            if ($(this).attr('class') == 'jspScrollable') {
                if ($(this).attr('id')) {
                    Hide_jScrollPane($(this).attr('id'));
                }
            }
        });
    } catch (e) {
    }
}

function ALL_Resize_jScrollPane_Vertical_Childen(Parent_ID) {

    try {
        $('#' + Parent_ID).find('div').each(function () {

            if ($(this).attr('class') == 'jspScrollable') {
                if ($(this).attr('id')) {
                    Resize_jScrollPane_Vertical($(this).attr('id'));
                }
            }
        });
    } catch (e) {
    }
}

function Scroll_To_jScrollPane_Vertical(jScrollPane_ID, Element_ID) {

    try {
        var Scrolled = false;

        var jScrollPane_width = parseInt($('#' + jScrollPane_ID).width());
        var jScrollPane_height = parseInt($('#' + jScrollPane_ID).height());

        if ((jScrollPane_width > 20) && (jScrollPane_height > 20)) {

            var jScrollPane = $('#' + jScrollPane_ID).jScrollPane();

            var api = jScrollPane.data('jsp');

            if (api != null) {

                Setup_jScrollPane_Vertical(jScrollPane_ID);
                api.scrollToElement('#' + Element_ID);
                Hide_jScrollPane(jScrollPane_ID);

                Scrolled = true;
            }
        }

        if (!Scrolled) {
            Scroll_To_Element(Element_ID);
        }
    } catch (e) {
    }
}

function Scroll_To_jScrollPane_Vertical_END(jScrollPane_ID) {

    try {
        var jScrollPane_width = parseInt($('#' + jScrollPane_ID).width());
        var jScrollPane_height = parseInt($('#' + jScrollPane_ID).height());

        if ((jScrollPane_width > 20) && (jScrollPane_height > 20)) {

            var jScrollPane = $('#' + jScrollPane_ID).jScrollPane();

            var api = jScrollPane.data('jsp');

            if (api != null) {
                Setup_jScrollPane_Vertical(jScrollPane_ID);
                api.scrollToPercentY(100);
                Hide_jScrollPane(jScrollPane_ID);
            }
        }
    } catch (e) {
    }
}

function ALL_Scroll_To_jScrollPane_Vertical_Childen_END(Parent_ID) {

    try {
        $('#' + Parent_ID).find('div').each(function () {

            if ($(this).attr('class') == 'jspScrollable') {
                if ($(this).attr('id')) {
                    Scroll_To_jScrollPane_Vertical_END($(this).attr('id'));
                }
            }
        });
    } catch (e) {
    }
}