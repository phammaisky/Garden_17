function Check_Element_Is_Not_Null(ID) {
    
    var Result = false;

    try {
        if ($('#' + ID).length) {
            Result = true;
        }
    } catch (e) {
        if (document.getElementById(ID) != null) {
            Result = true;
        }
    }

    return Result;
}

function Check_Element_Visible(ID) {

    var Result = false;

    if (Check_Element_Is_Not_Null(ID)) {

        if ($('#' + ID).css('display') != 'none') {
            Result = true;
        }
    }

    return Result;
}

function Hide_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).css('display', 'none');
    }
}

function Invisible_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).css('visibility', 'hidden');
    }
}

function Display_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).show(); // css('display', 'block');
    }
}

function Display_Table(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).show(); // css('display', 'table');
    }
}

function Display_inline_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).css('display', 'inline');
    }
}

function Visible_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).css('visibility', 'visible');
    }
}

function Display_TR(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).show(); // css('display', 'table-row');
    }
}

function Display_TD(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).show(); // css('display', 'table-cell');
    }

}

function Check_Disabled_Element(ID) {

    var Result = false;

    if (Check_Element_Is_Not_Null(ID)) {

        if ($('#' + ID).attr('disabled')) {
            Result = $('#' + ID).attr('disabled');
        }
    }

    return Result;
}

function Disabled_Element(ID) {

    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).find('*').attr('disabled', true);
        $('#' + ID).attr('disabled', true);        
    }
}

function Enabled_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).find('*').attr('disabled', false);
        $('#' + ID).attr('disabled', false);
    }
}

function Disabled_Element_BG(ID) {

    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).find('*').attr('disabled', true);
        $('#' + ID).attr('disabled', true);

        $('#' + ID).css('background-color', 'RoyalBlue');
        $('#' + ID).css('color', 'White');
    }
}

function Enabled_Element_BG(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).find('*').attr('disabled', false);
        $('#' + ID).attr('disabled', false);

        $('#' + ID).css('background-color', 'White');
        $('#' + ID).css('color', 'Red');
    }
}

function Remove_Element(Parent_Holder_ID, Element_ID) {

    try {
        if (Check_Element_Is_Not_Null(Element_ID)) {
            $('#' + Element_ID).remove();
        }
    } catch (e) {

        var Parent_Holder = document.getElementById(Parent_Holder_ID);

        var Element_To_Remove = document.getElementById(Element_ID);

        if ((Parent_Holder != null) && (Element_To_Remove != null)) {
            Parent_Holder.removeChild(Element_To_Remove);
        }
    }
}

function Strike_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).css('textDecoration', 'line-through');
    }
}

function Un_Strike_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).css('textDecoration', 'none');
    }
}

function Italic_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).css('fontStyle', 'italic');
    }
}

function Un_Italic_Element(ID) {
    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).css('fontStyle', 'normal');
    }
}

function Change_Element_Size(Element_ID, width, height) {
    if (Check_Element_Is_Not_Null(Element_ID)) {
        $('#' + Element_ID).css('width', width + 'px');
        $('#' + Element_ID).css('height', height + 'px');
    }
}

function Change_Element_Background_Color(Element_ID, Color) {
    if (Check_Element_Is_Not_Null(Element_ID)) {
        $('#' + Element_ID).css('bgcolor', Color); //transparent //bgcolor='#000000' 
    }
}