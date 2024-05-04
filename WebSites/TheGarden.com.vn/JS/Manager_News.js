function Delete_News(News_ID) {

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Delete_News(News_ID, Delete_News_Sucessfull, Delete_News_Error);

    //
    function Delete_News_Sucessfull(Response) {

        Hide_Element(News_ID + '_tr');
    }

    //
    function Delete_News_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Delete_Shop_News(Shop_News_ID) {

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Delete_Shop_News(Shop_News_ID, Delete_Shop_News_Sucessfull, Delete_Shop_News_Error);

    //
    function Delete_Shop_News_Sucessfull(Response) {

        Hide_Element(Shop_News_ID + '_tr');
    }

    //
    function Delete_Shop_News_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Display_News(News_ID, Is_Display) {

    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Display_News(News_ID, Is_Display, Display_News_Sucessfull, Display_News_Error);

    //
    function Display_News_Sucessfull(Response) {
                
        if (Is_Display == 1) {

            $("#Display_lnk_" + News_ID).text("Ẩn");
            $("#Display_lnk_" + News_ID).attr('onclick', "Display_News(" + News_ID + ", 0); return false;");
        }
        else
        {
            $("#Display_lnk_" + News_ID).text("Hiện");
            $("#Display_lnk_" + News_ID).attr('onclick', "Display_News(" + News_ID + ", 1); return false;");
        }
    }

    //
    function Display_News_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}