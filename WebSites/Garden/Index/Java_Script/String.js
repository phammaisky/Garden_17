function Check_ID(object) {

    var Result = false;

    if (Check_Object_NOT_Null_Or_Empty(object) && (object != '0')) {
        Result = true;
    }

    return Result;
}

function Check_Object_NOT_Null_Or_Empty(object) {

    var Result = false;

    if ((object != null) && (object != '') && (object != 'undefined')) {
        Result = true;
    }

    return Result;
}

function Crop_http_From_URL(URL) {

    return String(URL)
            .replace(/http:\/\/www./g, '')
            .replace(/https:\/\/www./g, '')

            .replace(/http:\/\//g, '')
            .replace(/https:\/\//g, '')
            ;
}

function Crop_String_Length(String_Input, Max_Length, Add_3_dot) {

    var Result = String_Input;

    if (String_Input != null) {

        if (String_Input.length > Max_Length) {

            Result = String_Input.substring(0, Max_Length - 1);

            if (Add_3_dot) {
                Result += '...';
            }

            if (Check_String_End_With(String_Input, ')')) {
                Result += ')';
            }
            else
                if (Check_String_End_With(String_Input, ' !')) {
                    Result += ' !';
                }
                else
                    if (Check_String_End_With(String_Input, '!')) {
                        Result += ')';
                    }
        }
    }

    return Result;
}

function Remove_From_Index_To(Input, Start_Text_Index, End_Text_Index) {

    Input = Input.substr(0, Start_Text_Index) + Input.substr(End_Text_Index);

    return Input;
}

function Remove_From_String_To_End(Input, End_Text) {

    if (Check_String_Contain(Input, End_Text)) {

        var End_Text_Index = Input.indexOf(End_Text);
        Input = Input.substr(0, End_Text_Index); // Remove_From_Index_To(Input, End_Text_Index, Input.length);
    }

    return Input;
}

function Remove_String_Last(Input, Last_Text) {

    if (Check_String_End_With(Input, Last_Text)) {
        Input = Input.substr(0, Input.length - Last_Text.length);
    }

    return Input;
}

function Check_String_End_With(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

function Get_From_String_To(Input, Start_Text, End_Text) {

    var Result = '';

    if (Check_String_Contain(Input, Start_Text)) {

        var Start_Text_Index = Input.indexOf(Start_Text);

        Input = Remove_From_Index_To(Input, 0, Start_Text_Index);

        if (Check_String_Contain(Input, End_Text)) {

            Start_Text_Index = Input.indexOf(Start_Text);
            var End_Text_Index = Input.indexOf(End_Text);

            var Start = Start_Text_Index + Start_Text.length;
            var End = End_Text_Index - Start_Text_Index - Start_Text.length;

            //
            Result = Input.substr(Start, End);
        }
    }

    return Result;
}

function Replace_String(Input, Replace_From, Replace_To) {
    while (Check_String_Contain(Input, Replace_From)) {
        Input = Input.replace(Replace_From, Replace_To);
    }

    return Input;
}

function Check_String_Contain(Input, Contain) {
    var Result = (Input.indexOf(Contain) !== -1);

    //Write_Message(Input + ' : ' + Contain);

    return Result;
}

function Convert_Link_Element(ID) {
    
    var str = $('#' + ID).html();
    
    // Set the regex string
    var regex = /(https?:\/\/([-\w\.]+)+(:\d+)?(\/([\w\/_\.]*(\?\S+)?)?)?)/ig
    
    // Replace plain text links by hyperlinks
    var replaced_text = str.replace(regex, "<a href='$1' target='_blank'>$1</a>");
    
    // Echo link
    $('#' + ID).html(replaced_text);
}