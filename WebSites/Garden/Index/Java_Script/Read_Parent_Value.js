function Read_Top_Domain_ID() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Top_Domain_ID_hdf')) {
        Result = $('#Top_Domain_ID_hdf').val();
    }
    else {
        Result = parent.Read_Top_Domain_ID();
    }

    return Result;
}

function Read_Sub_Domain_ID() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Sub_Domain_ID_hdf')) {
        Result = $('#Sub_Domain_ID_hdf').val();
    }
    else {
        Result = parent.Read_Sub_Domain_ID();
    }

    return Result;
}

function Read_Group_Sub_Domain_ID() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Group_Sub_Domain_ID_hdf')) {
        Result = $('#Group_Sub_Domain_ID_hdf').val();
    }
    else {
        Result = parent.Read_Group_Sub_Domain_ID();
    }

    return Result;
}

function Read_Domain_Type() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Domain_Type_hdf')) {
        Result = $('#Domain_Type_hdf').val();
    }
    else {
        Result = parent.Read_Domain_Type();
    }

    return Result;
}

function Read_Domain_Level() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Domain_Level_hdf')) {
        Result = $('#Domain_Level_hdf').val();
    }
    else {
        Result = parent.Read_Domain_Level();
    }

    return Result;
}

function Read_Emotions_List(Emotions_List_hdf_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null(Emotions_List_hdf_ID)) {
        Result = $('#' + Emotions_List_hdf_ID).val();
    }
    else {
        Result = parent.Read_Emotions_List(Emotions_List_hdf_ID);
    }

    return Result;
}


function Read_Web_Domain_AND_Link_Alias() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Web_Domain_AND_Link_Alias_hdf')) {
        Result = $('#Web_Domain_AND_Link_Alias_hdf').val();
    }
    else {
        Result = parent.Read_Web_Domain_AND_Link_Alias();
    }

    return Result;
}

function Read_Manager_UserId_Of_Web_Domain_AND_Link_Alias() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Manager_UserId_Of_Web_Domain_AND_Link_Alias_hdf')) {
        Result = $('#Manager_UserId_Of_Web_Domain_AND_Link_Alias_hdf').val();
    }
    else {
        Result = parent.Read_Manager_UserId_Of_Web_Domain_AND_Link_Alias();
    }

    return Result;
}

function Read_Friend_Index_1() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Friend_Index_1_hdf')) {
        Result = $('#Friend_Index_1_hdf').val();
    }
    else {
        Result = parent.Read_Friend_Index_1();
    }

    return Result;
}

function Read_Friend_Index_2() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Friend_Index_2_hdf')) {
        Result = $('#Friend_Index_2_hdf').val();
    }
    else {
        Result = parent.Read_Friend_Index_2();
    }

    if (Result == '') {
        Result = Read_Default_Friend_Index_2();
    }

    return Result;
}

function Read_Friend_Index_2_ID() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Friend_Index_2_ID_hdf')) {
        Result = $('#Friend_Index_2_ID_hdf').val();
    }
    else {
        Result = parent.Read_Friend_Index_2_ID();
    }

    if (Result == '0') {
        Result = Read_Default_Friend_Index_2_ID();
    }

    return Result;
}

function Read_Default_Friend_Index_2() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Default_Friend_Index_2_hdf')) {
        Result = $('#Default_Friend_Index_2_hdf').val();
    }
    else {
        Result = parent.Read_Default_Friend_Index_2();
    }

    return Result;
}

function Read_Default_Friend_Index_2_ID() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Default_Friend_Index_2_ID_hdf')) {
        Result = $('#Default_Friend_Index_2_ID_hdf').val();
    }
    else {
        Result = parent.Read_Default_Friend_Index_2_ID();
    }

    return Result;
}

function Read_Friend_Index_1_ID() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Friend_Index_1_ID_hdf')) {
        Result = $('#Friend_Index_1_ID_hdf').val();
    }
    else {
        Result = parent.Read_Friend_Index_1_ID();
    }

    return Result;
}

function Read_Friend_Index_1_ID_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Friend_Index_1_ID_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Friend_Index_1_ID_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_Index_1_ID_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_Index_2_ID_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Friend_Index_2_ID_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Friend_Index_2_ID_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_Index_2_ID_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_News_ID_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Friend_News_ID_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Friend_News_ID_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_News_ID_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_Index_1_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Friend_Index_1_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Friend_Index_1_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_Index_1_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_Index_2_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Friend_Index_2_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Friend_Index_2_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_Index_2_From_FULL_Friend_News_ID(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Editing_Friend_Index_1_ID_AND_Friend_News_ID() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Editing_Friend_Index_1_ID_AND_Friend_News_ID_hdf')) {
        Result = $('#Editing_Friend_Index_1_ID_AND_Friend_News_ID_hdf').val();
    }
    else {
        Result = parent.Read_Editing_Friend_Index_1_ID_AND_Friend_News_ID();
    }

    return Result;
}

function Read_Loggedin_Icon() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Loggedin_Icon_hdf')) {
        Result = $('#Loggedin_Icon_hdf').val();
    }
    else {
        Result = parent.Read_Loggedin_Icon();
    }

    return Result;
}

function Read_Loggedin_Picture() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Loggedin_Picture_hdf')) {
        Result = $('#Loggedin_Picture_hdf').val();
    }
    else {
        Result = parent.Read_Loggedin_Picture();
    }

    return Result;
}

function Read_Friend_News_Title_hdf(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''
    
    if (Check_Element_Is_Not_Null('Title_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Title_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_News_Title_hdf(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_News_Content_hdf(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Content_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Content_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_News_Content_hdf(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_News_Primary_Picture_hdf(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''    

    if (Check_Element_Is_Not_Null('Primary_Picture_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Primary_Picture_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_News_Primary_Picture_hdf(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_News_Other_Picture_hdf(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Other_Picture_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Other_Picture_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_News_Other_Picture_hdf(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_News_Primary_Video_hdf(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''
    
    if (Check_Element_Is_Not_Null('Primary_Video_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Primary_Video_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_News_Primary_Video_hdf(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_News_Other_Video_hdf(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Other_Video_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Other_Video_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_News_Other_Video_hdf(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_News_Primary_Music_hdf(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Primary_Music_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Primary_Music_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_News_Primary_Music_hdf(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Friend_News_Other_Music_hdf(Friend_Index_1_ID_AND_Friend_News_ID) {

    var Result = ''

    if (Check_Element_Is_Not_Null('Other_Music_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf')) {
        Result = $('#Other_Music_' + Friend_Index_1_ID_AND_Friend_News_ID + '_hdf').val();
    }
    else {
        Result = parent.Read_Friend_News_Other_Music_hdf(Friend_Index_1_ID_AND_Friend_News_ID);
    }

    return Result;
}

function Read_Today_Year() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Today_Year_hdf')) {
        Result = $('#Today_Year_hdf').val();
    }
    else {
        Result = parent.Read_Today_Year();
    }

    return Result;
}

function Read_Today_Month() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Today_Month_hdf')) {
        Result = $('#Today_Month_hdf').val();
    }
    else {
        Result = parent.Read_Today_Month();
    }

    return Result;
}

function Read_Today_Day() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Today_Day_hdf')) {
        Result = $('#Today_Day_hdf').val();
    }
    else {
        Result = parent.Read_Today_Day();
    }

    return Result;
}

function Read_Enable_Friend_News_Emotion() {

    var Result = ''

    if (Check_Element_Is_Not_Null('Enable_Friend_News_Emotion_hdf')) {
        Result = $('#Enable_Friend_News_Emotion_hdf').val();
    }
    else {
        Result = parent.Read_Enable_Friend_News_Emotion();
    }

    return Result;
}