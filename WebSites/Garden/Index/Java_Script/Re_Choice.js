function Re_Choice_RDOL(ID, Value) {

    var RDOL_Item_Array = document.getElementsByName(ID);

    for (var i = 0; i < RDOL_Item_Array.length; i++) {

        if (RDOL_Item_Array[i].value == Value) {

            RDOL_Item_Array[i].checked = true;

            break;
        }
    }
}

function Re_Choice_DDL(ID, Value) {

    /*
    var Options = $('#' + ID)[0].options;

    var DLL_Item_Array = $.map(Options, function (elem) {
    return (elem.value || elem.text);
    });

    for (var i = 0; i < DLL_Item_Array.length; i++) {

    if (DLL_Item_Array[i].value == Value) {
    $('#' + ID).prop('selectedIndex', i);
    break;
    }
    }
    */

    var DDL = document.getElementById(ID);

    var DLL_Item_Array = DDL.options;

    for (var i = 0; i < DLL_Item_Array.length; i++) {

        if (DLL_Item_Array[i].value == Value) {
            DDL.selectedIndex = i;
            break;
        }
    }
}


function Re_Choice_TRV(Tree_View_ID, Value_List) {

    $('#' + Tree_View_ID + ' input[type=checkbox]').each(function () {

        if (Check_String_Contain(Value_List, '#' + $(this).val() + '#')) {
            $(this).attr('checked', true);
        }
    });
}