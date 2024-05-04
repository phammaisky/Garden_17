function Read_UserId() {
    return $('#Loggedin_UserId_hdf').val();
}

function Read_Upload_Host() {
    return $('#Upload_Host_hdf').val();
}

function Read_Upload_ID(Upload_For) {
    return $('#Upload_ID_For_' + Upload_For + '_hdf').val();
}

function Read_Upload_Title(Upload_For) {
    return $('#Upload_Title_For_' + Upload_For + '_hdf').val();
}

function Read_File_Type_Filter(Upload_For) {
    return $('#File_Type_Filter_For_' + Upload_For + '_hdf').val();
}

function Read_File_Type_Filter_Name(Upload_For) {
    return $('#File_Type_Filter_Name_For_' + Upload_For + '_hdf').val();
}

function Read_Max_Upload_Allowed(Upload_For) {
    return $('#Max_Upload_Allowed_For_' + Upload_For + '_hdf').val();
}

function Read_Max_File_Size_Allowed(Upload_For) {
    return $('#Max_File_Size_Allowed_For_' + Upload_For + '_hdf').val();
}

function Read_Total_Uploaded(Upload_For) {
    return $('#Total_Uploaded_For_' + Upload_For + '_hdf').val();
}

function Set_Loaded_Uploader(Upload_For) {
    Add_Hidden_Field_For_Uploader('Loaded_Uploader_For_' + Upload_For + '_hdf', '1');
}

function Re_Complete_Upload(Upload_For) {

    var JSON_String = $('#' + Upload_For + '_Complete_Upload_JSON_String').val();
    var Total_Uploaded = $('#Total_Uploaded_For_' + Upload_For + '_hdf').val();

    $('#Upload_List_For_' + Upload_For + '_ul').empty();
    Complete_Upload(Upload_For, JSON_String, Total_Uploaded);
}

function Complete_Upload(Upload_For, JSON_String, Total_Uploaded) {

    //alert('Complete_Upload: ' + Total_Uploaded);

    Add_Hidden_Field_For_Uploader(Upload_For + '_Complete_Upload_JSON_String', JSON_String);

    //
    if (!Check_Loaded_PageMethods('Creat_Uploader')) {
        window.setTimeout(function () { Complete_Upload(Upload_For, JSON_String, Total_Uploaded) }, 0.5 * 1000);
    }
    else {

        //alert(JSON_String);

        if (
            (Check_Element_Is_Not_Null('Total_Uploaded_For_' + Upload_For + '_hdf'))
            && (Check_Element_Is_Not_Null('Upload_List_For_' + Upload_For + '_ul'))
            && (Check_Element_Is_Not_Null('Max_Height_Upload_List_For_' + Upload_For + '_hdf'))
            ) {

            var JSON_Array = Parse_JSON_String_To_Array(JSON_String);
            
            if (JSON_Array != null) {

                for (var i = 0; i < JSON_Array.length; i++) {

                    if (JSON_Array[i].Item_1 == 'ok') {

                        //document.getElementById('Message_lbl').innerHTML = '';                    

                        if (Total_Uploaded == '0') {
                            $('#Total_Uploaded_For_' + Upload_For + '_hdf').val(parseInt($('#Total_Uploaded_For_' + Upload_For + '_hdf').val()) + 1);
                        }
                        else {
                            $('#Total_Uploaded_For_' + Upload_For + '_hdf').val(Total_Uploaded);
                        }

                        //
                        var File_Prefix = JSON_Array[i].Item_3;
                        var File_Extension = JSON_Array[i].Item_4;

                        var File_Width = JSON_Array[i].Item_5;
                        var File_Height = JSON_Array[i].Item_6;

                        //
                        var Need_Wait_Thumbnail_File_Loaded = false;

                        //
                        if ((File_Width == '1') || (File_Height == '1')) {
                            Need_Wait_Thumbnail_File_Loaded = true;
                        }

                        //
                        if ((Upload_For == 'Shop_ADX') || (Upload_For == 'Shop_ADS')) {
                            if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {
                                Need_Wait_Thumbnail_File_Loaded = true;
                            }
                        }

                        //
                        Complete_Upload_Wait_Thumbnail_File_Loaded(Upload_For, File_Prefix, File_Extension, File_Width, File_Height, Need_Wait_Thumbnail_File_Loaded);
                    }
                    else {

                        Write_Message('Lỗi:\n' + JSON_Array[i].Item_1);
                    }

                    //
                    if (i == (JSON_Array.length - 1)) {

                        Check_Total_Uploaded(Upload_For);
                        Enable_Order_List('Upload_List_For_' + Upload_For + '_ul');

                        try {
                            Read_Max_Height_Upload_List(Upload_For);
                        }
                        catch (e) {
                            $('#Upload_List_For_' + Upload_For + '_ul').find('div').each(function () {
                                $(this).css('height', (120 + 24) + 'px');
                            });
                        }
                    }
                }
            }
        }
    }
}

function Read_Max_Height_Upload_List(Upload_For) {

    var Max_Height_Upload_List = 0;

    $('#Upload_List_For_' + Upload_For + '_ul').find('img').each(function () {

        if ($(this).height() > 24) {

            if (Max_Height_Upload_List < $(this).height()) {
                Max_Height_Upload_List = $(this).height();
            }

            $('#Upload_List_For_' + Upload_For + '_ul').find('div').each(function () {
                $(this).css('height', (Max_Height_Upload_List + 24) + 'px');
            });
        }
        else {
            window.setTimeout(function () { Read_Max_Height_Upload_List(Upload_For); }, 0.5 * 1000);
        }
    });
}

function Complete_Upload_Wait_Thumbnail_File_Loaded(Upload_For, File_Prefix, File_Extension, File_Width, File_Height, Need_Wait_Thumbnail_File_Loaded) {

    //
    var File_Prefix_AND_Extension = File_Prefix + File_Extension;

    //
    var Image_Temp = new Image();

    var File_Picture_x1 = '';
    var File_Picture_x2 = '';
    var File_Picture_x3 = '';
    var File_Picture_x4 = '';

    var File_Video = '';
    var File_Video_x1 = '';

    var File_AD = '';
    var File_AD_x1 = '';

    //
    var File_Prefix_Array = File_Prefix.split('/');

    var Prefix = File_Prefix_Array[File_Prefix_Array.length - 1];

    var Iframe_Player_Path = File_Prefix.replace(Prefix, "Player.htm");

    //
    if ((Upload_For == 'Shop_ADX') || (Upload_For == 'Shop_ADS')) {

        File_AD = File_Prefix.replace('_Prefix', '_') + File_Extension;

        if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {

            File_AD_x1 = File_AD;

            Image_Temp.src = File_AD_x1;
        }
    }
    else
        if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {

            File_Picture_x1 = File_Prefix.replace('_Prefix', '_x1' + File_Extension);
            File_Picture_x2 = File_Prefix.replace('_Prefix', '_x2' + File_Extension);
            File_Picture_x3 = File_Prefix.replace('_Prefix', '_x3' + File_Extension);
            File_Picture_x4 = File_Prefix.replace('_Prefix', '_x4' + File_Extension);

            Image_Temp.src = File_Picture_x1;
        }
        else if ((File_Extension == '.swf') || (File_Extension == '.flv')) {

            File_Video = File_Prefix.replace('_Prefix', '_') + File_Extension;
            File_Video_x1 = File_Prefix.replace('_Prefix', '_x1.jpg');

            Image_Temp.src = File_Video_x1;
        }

    //
    if ((Need_Wait_Thumbnail_File_Loaded) && (!Image_Temp.complete)) {
        window.setTimeout(function () { Complete_Upload_Wait_Thumbnail_File_Loaded(Upload_For, File_Prefix, File_Extension, File_Width, File_Height, Need_Wait_Thumbnail_File_Loaded) }, 0.5 * 1000);
    }
    else {

        //
        var UUID = Creat_UUID();

        var Width = 0;
        var Height = 0;

        var Width_Of_Image = parseInt(Image_Temp.width);
        var Height_Of_Image = parseInt(Image_Temp.height);

        var Max_Height_Upload_List = 0;

        var AD_Position = '';
        var AD_Size = 1;

        var AD_Height_Per_Size = 90;
        var AD_Width_Per_Size = 0;

        //
        if (Check_Element_Is_Not_Null('Choice_AD_Position_rdol')) {

            var Choice_AD_Position_Array = $('[name=Choice_AD_Position_rdol]');

            for (var i2 = 0; i2 < Choice_AD_Position_Array.length; i2++) {
                if ((Choice_AD_Position_Array[i2].type == 'radio') && (Choice_AD_Position_Array[i2].checked)) {

                    //
                    AD_Position = Choice_AD_Position_Array[i2].value;

                    //
                    if (AD_Position == '1') {
                        AD_Width_Per_Size = 240;
                    }
                    else
                        if (AD_Position == '2') {
                            AD_Width_Per_Size = 240;
                        }

                    //
                    break;
                }
            }
        }

        //
        if (Check_Element_Is_Not_Null('AD_Size_ddl')) {

            AD_Size = parseInt($('#AD_Size_ddl').val());
        }

        //
        if (File_Width == '0') {

            if ((File_Extension == '.swf') || (File_Extension == '.flv')) {

                Width = AD_Width_Per_Size;
            }
        }
        else
            if (File_Width == '1') {

                Width = Width_Of_Image;
            }
            else {
                Width = File_Width;
            }

        //
        if (File_Height == '0') {

            if ((File_Extension == '.swf') || (File_Extension == '.flv')) {

                if (Check_Element_Is_Not_Null('AD_Size_ddl')) {
                    //
                    Height = AD_Size * AD_Height_Per_Size;
                    Max_Height_Upload_List = (AD_Size * AD_Height_Per_Size);
                }
            }
        }
        else
            if (File_Height == '1') {
                Height = Height_Of_Image;
                Max_Height_Upload_List = Height_Of_Image;
            }
            else {
                Height = File_Height;
                Max_Height_Upload_List = parseInt(File_Height);
            }

        //
        if ((File_Width == '0') || (File_Height == '0')) {
            if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {

                //
                var Max_AD_Width = AD_Width_Per_Size;
                var Max_AD_Height = AD_Size * AD_Height_Per_Size;

                //
                if (Max_AD_Width * Height_Of_Image >= Max_AD_Height * Width_Of_Image) {
                    Width = (Max_AD_Height * Width_Of_Image) / Height_Of_Image;
                    Height = Max_AD_Height;
                }
                else {
                    Height = (Max_AD_Width * Height_Of_Image) / Width_Of_Image;
                    Width = Max_AD_Width;
                }

                Max_Height_Upload_List = Height;
            }
        }

        if ((Upload_For == 'Shop_ADX') || (Upload_For == 'Shop_ADS')) {
            $('#Max_Height_Upload_List_For_' + Upload_For + '_hdf').val(Max_Height_Upload_List);
        }
        else
            if (parseInt($('#Max_Height_Upload_List_For_' + Upload_For + '_hdf').val()) < Max_Height_Upload_List) {
                $('#Max_Height_Upload_List_For_' + Upload_For + '_hdf').val(Max_Height_Upload_List);
            }

        //Write_Message('wait: ' + $('#Max_Height_Upload_List_For_' + Upload_For + '_hdf').val());

        //alert(Width + ' : ' + Height);

        //
        var Upload_List_ID = 'Upload_List_For_' + Upload_For + '_ul';
        var Uploaded_File_Preview_div = '';
        var Uploaded_File_Preview = '';

        //
        if ((Upload_For == 'Shop_ADX') || (Upload_For == 'Shop_ADS')) {

            if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {

                Uploaded_File_Preview = "<img src = '" + File_AD_x1 + "' width = '" + Width + "px' height = '" + Height + "px'>";
            }
            else if ((File_Extension == '.swf') || (File_Extension == '.flv')) {

                Uploaded_File_Preview = "<embed id='" + UUID + "' src='/index/Player.swf?ID=" + UUID + "&Media=" + File_AD_x1 + "&Play=1&Sound=0&Scale=0&Loop=1&Thumb=1&Full=0&X2=0' type='application/x-shockwave-flash' quality='Best' scale='NoBorder' wmode='transparent' bgcolor='#000000' allowfullscreen='true' allowscriptaccess='always' width = '" + Width + "px' height = '" + Height + "px' pluginspage='http://www.macromedia.com/go/getflashplayer'></embed>";
            }
        }
        else
            if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {

                //
                Uploaded_File_Preview = "<img ondblclick = \"javascript:Add_Title_img('" + File_Picture_x1 + "');\" src = '" + File_Picture_x1 + "'>";

                if (Convert_HDF_To_Boolean('Insert_To_Editor_For_' + Upload_For + '_hdf')) {

                    var Embed_Code_x1 = "<center><img src = '" + File_Picture_x1 + "'></center><br/>";
                    var Embed_Code_x2 = "<center><img src = '" + File_Picture_x2 + "'></center><br/>";
                    var Embed_Code_x3 = "<center><img src = '" + File_Picture_x3 + "'></center><br/>";
                    //var Embed_Code_x4 = "<center><img src = '" + File_Picture_x4 + "'></center><br/>";

                    Add_Hidden_Field_For_Uploader('Embed_Code_x1_' + UUID + '_hdf', Embed_Code_x1);
                    Add_Hidden_Field_For_Uploader('Embed_Code_x2_' + UUID + '_hdf', Embed_Code_x2);
                    Add_Hidden_Field_For_Uploader('Embed_Code_x3_' + UUID + '_hdf', Embed_Code_x3);
                    //Add_Hidden_Field_For_Uploader('Embed_Code_x4_' + UUID + '_hdf', Embed_Code_x4);
                }
            }
            else
                if ((File_Extension == '.swf') || (File_Extension == '.flv')) {

                    Uploaded_File_Preview = "<embed id='" + UUID + "' src='/index/Player.swf?ID=" + UUID + "&Media=" + File_Video + "&Play=0&Sound=1&Scale=1&Loop=0&Thumb=0&Full=1&X2=1' type='application/x-shockwave-flash' quality='Best' scale='NoBorder' wmode='transparent' bgcolor='#000000' allowfullscreen='true' allowscriptaccess='always' width = '" + Width + "px' height = '" + Height + "px' pluginspage='http://www.macromedia.com/go/getflashplayer'></embed>";

                    //
                    if (Convert_HDF_To_Boolean('Insert_To_Editor_For_' + Upload_For + '_hdf')) {
                        var Embed_Code_x1 = "<center><iframe src='" + Iframe_Player_Path + "' width = '120' height = '90'></iframe></center><br/>";
                        var Embed_Code_x2 = "<center><iframe src='" + Iframe_Player_Path + "' width = '240' height = '180'></iframe></center><br/>";
                        var Embed_Code_x3 = "<center><iframe src='" + Iframe_Player_Path + "' width = '400' height = '300'></iframe></center><br/>";
                        //var Embed_Code_x4 = "<center><iframe src='" + Iframe_Player_Path + "' width = '800' height = '600'></iframe></center><br/>";

                        Add_Hidden_Field_For_Uploader('Embed_Code_x1_' + UUID + '_hdf', Embed_Code_x1);
                        Add_Hidden_Field_For_Uploader('Embed_Code_x2_' + UUID + '_hdf', Embed_Code_x2);
                        Add_Hidden_Field_For_Uploader('Embed_Code_x3_' + UUID + '_hdf', Embed_Code_x3);
                        //Add_Hidden_Field_For_Uploader('Embed_Code_x4_' + UUID + '_hdf', Embed_Code_x4);
                    }
                }

        //
        if (Convert_HDF_To_Boolean('Insert_To_Editor_For_' + Upload_For + '_hdf')) {
            Uploaded_File_Preview_div = "<DIV><table style='width: 100%; height: 100%;'><tr><td align='center' valign='bottom'>" + Uploaded_File_Preview + "<br/><a href = \"javascript:Insert_HTML_Editor_Content_At_Caret('Content_edt', 'Embed_Code_x1_" + UUID + "_hdf');\">x1</a> <a href = \"javascript:Insert_HTML_Editor_Content_At_Caret('Content_edt', 'Embed_Code_x2_" + UUID + "_hdf');\">x2</a> <a href = \"javascript:Insert_HTML_Editor_Content_At_Caret('Content_edt', 'Embed_Code_x3_" + UUID + "_hdf');\">x3</a> <a href = \"javascript:Delete_Upload('" + Upload_For + "', '" + File_Prefix_AND_Extension + "');\">Xóa</a></td></tr></table></DIV>";
        }
        else {
            Uploaded_File_Preview_div = "<DIV><table style='width: 100%; height: 100%;'><tr><td align='center' valign='bottom'>" + Uploaded_File_Preview + "<br/><a href = \"javascript:Delete_Upload('" + Upload_For + "', '" + File_Prefix_AND_Extension + "');\">Xóa</a></td></tr></table></DIV>";
        }

        Add_Uploaded_File_Preview(Upload_List_ID, File_Prefix_AND_Extension, Uploaded_File_Preview_div);
    }
}

function Hide_ALL_Uploaded_File_Preview_Link(Upload_For) {

    var Upload_List_ID = 'Upload_List_For_' + Upload_For + '_ul';

    if (Check_Element_Is_Not_Null(Upload_List_ID)) {

        $('#' + Upload_List_ID).sortable('disable');
        $('#' + Upload_List_ID).find('a').css('display', 'none');
    }
}

function Add_Uploaded_File_Preview(Parent_Holder, ID, Element) {

    var Element_To_Add = $('<li/>', {
        id: ID,
        html: Element
    });

    $('#' + Parent_Holder).append(Element_To_Add);
}

function Delete_Upload(Upload_For, ID) {

    //
    var Upload_List_ID = 'Upload_List_For_' + Upload_For + '_ul';
    Remove_Element(Upload_List_ID, ID);

    //
    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Delete_Upload(Read_Upload_ID(Upload_For), Delete_Upload_Sucessfull, Delete_Upload_Error);

    function Delete_Upload_Sucessfull(Response) {

        if (Response == '1') {

            $('#Total_Uploaded_For_' + Upload_For + '_hdf').val(parseInt($('#Total_Uploaded_For_' + Upload_For + '_hdf').val()) - 1);

            var Uploader = document.getElementById('Uploader_For_' + Upload_For + '_swf');
            Uploader.Delete_Upload();

            Check_Total_Uploaded(Upload_For);
        }
        else {
            Alert_Message('Bạn không thể Xóa Upload của người khác !');
        }
    }

    function Delete_Upload_Error(Response) {
        
        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Complete_Order_Upload_List_ALL() {

    //
    var Upload_Order_Json = '[';

    //
    var Upload_Order_obj = new Object();

    //
    Upload_Order_obj.Item_1 = 'Shop_X_Picture';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Shop_X_Picture');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Shop_ADX';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Shop_ADX');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Shop_ADS';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Shop_ADS');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Friend_News_Picture';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Friend_News_Picture');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Friend_News_Video';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Friend_News_Video');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Friend_News_Music';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Friend_News_Music');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Friend_Avatar';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Friend_Avatar');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Shop_Logo';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Shop_Logo');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Page_Background_Picture';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Page_Background_Picture');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Chat_Background_Picture';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Chat_Background_Picture');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Upload_Order_obj.Item_1 = 'Thumbnail';
    Upload_Order_obj.Item_2 = Complete_Order_Upload_List('Thumbnail');
    Upload_Order_Json += Creat_JSON_one(Upload_Order_obj) + ',';

    //
    Remove_String_Last(Upload_Order_Json, ',');
    Upload_Order_Json += ']';

    //
    Add_Hidden_Field('Upload_Order_hdf', Upload_Order_Json);

    //alert($('#Upload_Order_hdf').val());
}

function Complete_Order_Upload_List(Upload_For) {

    var Upload_Order = '';

    if (Check_Element_Is_Not_Null('Upload_List_For_' + Upload_For + '_ul')) {

        $('#Upload_List_For_' + Upload_For + '_ul').find('li').each(function () {
            Upload_Order += '#' + $(this).attr('id') + '#';
        });

        Add_Hidden_Field_For_Uploader('Upload_Order_For_' + Upload_For + '_hdf', Upload_Order);
    }

    return Upload_Order;
}

function Check_Loaded_Uploader(Upload_For) {
    return Convert_HDF_To_Boolean('Loaded_Uploader_For_' + Upload_For + '_hdf');
}


function Check_Total_Uploaded(Upload_For) {

    var Uploader = document.getElementById('Uploader_For_' + Upload_For + '_swf');

    if ((Check_Element_Is_Not_Null('Uploader_For_' + Upload_For + '_swf')) && ($('#Uploader_For_' + Upload_For + '_tr').css('display') != 'none')) {
    
        if (Check_Loaded_Uploader(Upload_For)) {
            Uploader.Check_Total_Uploaded($('#Total_Uploaded_For_' + Upload_For + '_hdf').val());
        }
        else {
            window.setTimeout(function () { Check_Total_Uploaded(Upload_For) }, 0.5 * 1000);
        }
    }
}

function Cancel_ALL_X_Shop_Uploader() {
    Cancel_ALL_File_Uploading("Shop_X_Picture");
}

function Cancel_ALL_AD_Shop_Uploader() {
    Cancel_ALL_File_Uploading("Shop_ADX");
    Cancel_ALL_File_Uploading("Shop_ADS");
}

function Cancel_ALL_Friend_News_Uploader() {

    Cancel_ALL_File_Uploading('Friend_News_Picture');
    Cancel_ALL_File_Uploading('Friend_News_Video');
    Cancel_ALL_File_Uploading('Friend_News_Music');
}

function Cancel_ALL_Friend_Information_Uploader() {

    Cancel_ALL_File_Uploading('Friend_Avatar');

    Cancel_ALL_File_Uploading('Page_Background_Picture');
    Cancel_ALL_File_Uploading('Chat_Background_Picture');

    Cancel_ALL_File_Uploading('Thumbnail');
}

function Cancel_ALL_Shop_Information_Uploader() {

    Cancel_ALL_File_Uploading('Shop_Logo');

    Cancel_ALL_File_Uploading('Page_Background_Picture');
    Cancel_ALL_File_Uploading('Chat_Background_Picture');

    Cancel_ALL_File_Uploading('Thumbnail');
}


function Cancel_ALL_File_Uploading(Upload_For) {

    var Uploader = document.getElementById('Uploader_For_' + Upload_For + '_swf');

    if ((Check_Element_Is_Not_Null('Uploader_For_' + Upload_For + '_swf')) && ($('#Uploader_For_' + Upload_For + '_tr').css('display') != 'none')) {
        //alert(Upload_For);

        if (Check_Loaded_Uploader(Upload_For)) {
            $('#Uploader_For_' + Upload_For + '_swf').css('height', '48px');
            Uploader.Cancel_ALL_File_Uploading();
        }
        else {
            window.setTimeout(function () { Cancel_ALL_File_Uploading(Upload_For) }, 0.5 * 1000);
        }
    }
}

function Creat_Uploader(Upload_For_List) {

    Add_Hidden_Field_For_Uploader('Loaded_PageMethods_Creat_Uploader_hdf', '');

    //
    PageMethods.set_path($('#PageMethods_Path_hdf').val());
    PageMethods.Creat_Uploader(Upload_For_List, Creat_Uploader_Sucessfull, Creat_Uploader_Error);

    function Creat_Uploader_Sucessfull(Response) {

        var JSON_String = Response;

        //alert('Creat_Upload: ' + Upload_For_List + " : " + Response);

        var JSON_Array = Parse_JSON_String_To_Array(JSON_String);

        if (JSON_Array != null) {

            for (var i = 0; i < JSON_Array.length; i++) {

                //
                var Upload_For = JSON_Array[i].Item_1;
                var Upload_ID = JSON_Array[i].Item_2;

                var Max_Upload_Allowed = JSON_Array[i].Item_3;
                var Max_File_Size_Allowed = JSON_Array[i].Item_4;

                var File_Type_Filter = JSON_Array[i].Item_5;
                var File_Type_Filter_Name = JSON_Array[i].Item_6;
                var Select_File_Title = JSON_Array[i].Item_7;

                var Insert_To_Editor = JSON_Array[i].Item_8;

                //
                if (Check_Element_Is_Not_Null('Uploader_For_' + Upload_For + '_tr')) {

                    //alert('Uploader_For_' + Upload_For + '_tr');

                    Add_Hidden_Field_For_Uploader('Loaded_Uploader_For_' + Upload_For + '_hdf', '');

                    //
                    Display_TR('Uploader_For_' + Upload_For + '_tr');
                    Display_TR('Upload_List_For_' + Upload_For + '_tr');

                    //
                    $('#Uploader_For_' + Upload_For + '_swf').css('height', '48px');

                    //
                    Add_Hidden_Field_For_Uploader('Upload_ID_For_' + Upload_For + '_hdf', Upload_ID);
                    Add_Hidden_Field_For_Uploader('Upload_Title_For_' + Upload_For + '_hdf', Select_File_Title);

                    Add_Hidden_Field_For_Uploader('Max_Upload_Allowed_For_' + Upload_For + '_hdf', Max_Upload_Allowed);
                    Add_Hidden_Field_For_Uploader('Max_File_Size_Allowed_For_' + Upload_For + '_hdf', Max_File_Size_Allowed);

                    Add_Hidden_Field_For_Uploader('File_Type_Filter_For_' + Upload_For + '_hdf', File_Type_Filter);
                    Add_Hidden_Field_For_Uploader('File_Type_Filter_Name_For_' + Upload_For + '_hdf', File_Type_Filter_Name);

                    Add_Hidden_Field_For_Uploader('Insert_To_Editor_For_' + Upload_For + '_hdf', Insert_To_Editor);

                    Add_Hidden_Field_For_Uploader('Total_Uploaded_For_' + Upload_For + '_hdf', '0');

                    Add_Hidden_Field_For_Uploader('Upload_Order_For_' + Upload_For + '_hdf', '');
                    Add_Hidden_Field_For_Uploader('Max_Height_Upload_List_For_' + Upload_For + '_hdf', '0');
                }
            }
        }

        //
        Add_Hidden_Field_For_Uploader('Loaded_PageMethods_Creat_Uploader_hdf', '1');
    }

    function Creat_Uploader_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}

function Start_Upload(Upload_For) {
    //Disabled_Submit_BTN();
}

function Error_Upload(Upload_For) {
    //Enabled_Submit_BTN();
}
