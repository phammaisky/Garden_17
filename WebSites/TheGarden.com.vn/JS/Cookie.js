function Set_Cookie(c_name, value, exdays) {

    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);

    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());

    document.cookie = c_name + "=" + c_value;
}

function Add_Cookie(c_name, value, exdays) {

    var Current_Cookie = Get_Cookie(c_name);

    var New_Cookie = value;

    if ((Current_Cookie != null) && (Current_Cookie != '')) {
        New_Cookie = Current_Cookie + value;
    }

    Set_Cookie(c_name, New_Cookie, exdays);
}

function Delete_Cookie(c_name) {

    var exdate = new Date();
    exdate.setDate(exdate.getDate() - 1);

    document.cookie = c_name + "=" + ";expires=" + exdate.toUTCString();
}

function Get_Cookie_Not_Null(c_name) {

    var Result = Get_Cookie(c_name);

    if (!Check_Object_NOT_Null_Or_Empty(Result)) {
        
        Set_Cookie(c_name, '', 1);
        
        Result = '';
    }

    return Result;
}

function Get_Cookie(c_name) {

    var c_value = document.cookie;
    var c_start = c_value.indexOf(" " + c_name + "=");

    if (c_start == -1) {
        c_start = c_value.indexOf(c_name + "=");
    }

    if (c_start == -1) {
        c_value = null;
    }
    else {

        c_start = c_value.indexOf("=", c_start) + 1;

        var c_end = c_value.indexOf(";", c_start);

        if (c_end == -1) {
            c_end = c_value.length;
        }

        c_value = unescape(c_value.substring(c_start, c_end));
    }

    //
    var Result = c_value; //.toString()

    return Result;
}