function Check_IMG_Loaded(ID) {

    var Image_Temp = new Image();
    Image_Temp.src = $('#' + ID).attr('src');

    return Image_Temp.complete;
}

function Display_AD(Holer_ID, JSON_String) {

    //Write_Message(JSON_String);

    var JSON_Array = Parse_JSON_String_To_Array(JSON_String);

    if (JSON_Array != null) {

        for (var i = 0; i < JSON_Array.length; i++) {

            var File_Prefix = JSON_Array[i].Item_1;
            var File_Extension = JSON_Array[i].Item_2;

            var Position = JSON_Array[i].Item_3;
            var Size = parseInt(JSON_Array[i].Item_4);
            var Link = JSON_Array[i].Item_5;

            //
            var Need_Wait_Thumbnail_File_Loaded = false;

            //
            if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {
                Need_Wait_Thumbnail_File_Loaded = true;
            }

            //
            Display_AD_Wait_Thumbnail_File_Loaded(Holer_ID, File_Prefix, File_Extension, Position, Size, Link, Need_Wait_Thumbnail_File_Loaded);
        }
    }
}

function Display_AD_Wait_Thumbnail_File_Loaded(Holer_ID, File_Prefix, File_Extension, Position, Size, Link, Need_Wait_Thumbnail_File_Loaded) {

    //
    var Thumbnail_File_URL = '';
    var Real_File_URL = '';

    //
    Real_File_URL = File_Prefix.replace('_Prefix', '_') + File_Extension;

    if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {

        Thumbnail_File_URL = Real_File_URL;
    }

    //
    var Image_Temp = new Image();
    Image_Temp.src = Thumbnail_File_URL;

    if ((Need_Wait_Thumbnail_File_Loaded) && (!Image_Temp.complete)) {
        window.setTimeout(function () { Display_AD_Wait_Thumbnail_File_Loaded(Holer_ID, File_Prefix, File_Extension, Position, Size, Link, Need_Wait_Thumbnail_File_Loaded) }, 0.5 * 1000);
    }
    else {

        var Width = 0;
        var Height = 0;

        var AD_Height_Per_Size = 90;
        var AD_Width_Per_Size = 0;

        var Width_Of_Image = parseInt(Image_Temp.width);
        var Height_Of_Image = parseInt(Image_Temp.height);

        //
        if (Position == '1') {
            AD_Width_Per_Size = 240;
        }
        else
            if (Position == "2") {
                AD_Width_Per_Size = 240;
            }

        //
        if ((File_Extension == '.swf') || (File_Extension == '.flv')) {

            Width = AD_Width_Per_Size;
            Height = Size * AD_Height_Per_Size;
        }
        else
            if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {
                //
                var Max_AD_Width = AD_Width_Per_Size;
                var Max_AD_Height = Size * AD_Height_Per_Size;

                //
                if (Max_AD_Width * Height_Of_Image >= Max_AD_Height * Width_Of_Image) {
                    Width = (Max_AD_Height * Width_Of_Image) / Height_Of_Image;
                    Height = Max_AD_Height;
                }
                else {
                    Height = (Max_AD_Width * Height_Of_Image) / Width_Of_Image;
                    Width = Max_AD_Width;
                }
            }

        //
        if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {

            if (Link != '') {
                Add_Row_To_Table(Holer_ID, "<a href='" + Link + "' target='_blank'><img src = '" + Real_File_URL + "' width = '" + Width + "px' height = '" + Height + "px'></a>");
            }
            else {
                Add_Row_To_Table(Holer_ID, "<img src = '" + Real_File_URL + "' width = '" + Width + "px' height = '" + Height + "px'>");
            }
        }
        else if ((File_Extension == '.swf') || (File_Extension == '.flv')) {

            //
            var UUID = Creat_UUID();

            Add_Row_To_Table(Holer_ID, "<embed id='" + UUID + "' src='/index/Player.swf?ID=" + UUID + "&Media=" + Real_File_URL + "&Play=1&Sound=0&Scale=0&Loop=1&Thumb=1&Full=0&X2=0' type='application/x-shockwave-flash' quality='Best' scale='NoBorder' wmode='transparent' bgcolor='#000000' allowfullscreen='true' allowscriptaccess='always' width = '" + Width + "px' height = '" + Height + "px' pluginspage='http://www.macromedia.com/go/getflashplayer'></embed>");
        }
    }
}

function Replace_Media_From_ID(Size, JSON_String) {

    var JSON_Array = Parse_JSON_String_To_Array(JSON_String);

    if (JSON_Array != null) {

        for (var i = 0; i < JSON_Array.length; i++) {

            var Media_ID = JSON_Array[i].Item_1;
            var Media = JSON_Array[i].Item_2;

            if (Check_Element_Is_Not_Null(Media_ID)) {
                $('#' + Media_ID).attr('src', Convert_Picture_File_Prefix_To_Real(Media, Size));
            }
        }
    }
}

function Display_Media(Holder_ID, Media_For, Size, JSON_String) {

    //alert(JSON_String);

    var JSON_Array = Parse_JSON_String_To_Array(JSON_String);

    if (JSON_Array != null) {

        for (var i = 0; i < JSON_Array.length; i++) {
            
            var File_Prefix = JSON_Array[i].Item_2;
            var File_Extension = JSON_Array[i].Item_3;

            //
            Display_Media_Wait_Thumbnail_File_Loaded(Holder_ID, Media_For, Size, File_Prefix, File_Extension);
        }

        //
        if (Media_For == 'Shop_X_Picture_Size_1') {

            if (JSON_String != '') {
                $('#' + Holder_ID).html("(Click từng <font color='red'>Ảnh</span> để xem ảnh phóng to)<br/><br/>" + $('#' + Holder_ID).html());
            }
        }
    }
}

function Display_Media_Wait_Thumbnail_File_Loaded(Holder_ID, Media_For, Size, File_Prefix, File_Extension) {
    //
    var File_Prefix_AND_Extension = File_Prefix + File_Extension;

    var Thumbnail_File_URL = '';
    var Real_File_URL = '';
    var Real_File_Big_URL = '';

    var Need_Wait_Thumbnail_File_Loaded = false;
    var Need_Wait_Real_File_Loaded = false;

    //
    if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {

        Need_Wait_Thumbnail_File_Loaded = true;
        Need_Wait_Real_File_Loaded = true;

        Thumbnail_File_URL = File_Prefix.replace('_Prefix', '_x1') + File_Extension;
        Real_File_URL = File_Prefix.replace('_Prefix', '_x4') + File_Extension;
        Real_File_Big_URL = File_Prefix.replace('_Prefix', '_x4') + File_Extension;
    }
    else {
        //Thumbnail_File_URL = File_Prefix.replace('_Prefix', '_x1.jpg');
        Real_File_URL = File_Prefix.replace('_Prefix', '_') + File_Extension;
    }

    //
    var Image_Temp = new Image();
    Image_Temp.src = Thumbnail_File_URL;

    //
    var Real_Image_Temp = new Image();
    Real_Image_Temp.src = Real_File_URL;

    //
    if (((!Real_Image_Temp.complete) && (Need_Wait_Real_File_Loaded)) || ((!Image_Temp.complete) && (Need_Wait_Thumbnail_File_Loaded))) {
        window.setTimeout(function () { Display_Media_Wait_Thumbnail_File_Loaded(Holder_ID, Media_For, Size, File_Prefix, File_Extension) }, 0.5 * 1000);
    }
    else {

        //
        var Width = '';
        var Height = '';

        //
        var Width_Of_Image = Image_Temp.width;
        var Height_Of_Image = Image_Temp.height;

        //
        if (Need_Wait_Thumbnail_File_Loaded) {
            Width = Width_Of_Image;
            Height = Height_Of_Image;
        }
        else {
            Width = 120;
            Height = 120;
        }

        //
        var Media = '';

        //
        if ((File_Extension == '.jpg') || (File_Extension == '.jpeg') || (File_Extension == '.bmp') || (File_Extension == '.gif') || (File_Extension == '.png')) {

            if (Size == 'x1') {
                Media = "<a href = '" + Real_File_URL + "' class='highslide' onclick=\"return hs.expand(this, {wrapperClassName: 'borderless floating-caption', dimmingOpacity: 0.75, align: 'center'})\"><img src = '" + Thumbnail_File_URL + "' width = '" + Width + "px' height = '" + Height + "px' style='border-width: 0'></a> ";
            }
            else {

                if (Size == 'x2') {
                    Media = "<img width ='100%' src='" + Real_File_URL + "' style='border-width: 0'><br/><br/>";
                }
                else
                    if (Size == 'x3') {
                        Media = "<img width ='100%' src='" + Real_File_Big_URL + "' style='border-width: 0'><br/><br/>";
                    }
            }
        }
        else
            if ((File_Extension == '.swf') || (File_Extension == '.flv')) {

                var UUID = Creat_UUID();

                Media = "<embed id='" + UUID + "' src='/index/Player.swf?ID=" + UUID + "&Media=" + Real_File_URL + "&Play=0&Sound=1&Scale=1&Loop=0&Thumb=0&Full=1&X2=1' type='application/x-shockwave-flash' quality='Best' scale='NoBorder' wmode='transparent' bgcolor='#000000' allowfullscreen='true' allowscriptaccess='always' width = '" + Width + "px' height = '" + Height + "px' pluginspage='http://www.macromedia.com/go/getflashplayer'></embed> ";
            }

        //
        Add_innerHTML_To_Current(Holder_ID, Media);
    }
}

function Convert_Picture_File_Prefix_To_Real(Picture_File_Prefix, Size) {

    var Result = Picture_File_Prefix;

    if (Check_String_Contain(Picture_File_Prefix, '_Prefix')) {

        var File_Name_Extension = Read_File_Name_Extension(Picture_File_Prefix);

        Result = Remove_From_String_To_End(Picture_File_Prefix, '_Prefix') + '_' + Size + File_Name_Extension;
    }

    return Result;
}

function Convert_Picture_File_To_Big(Picture_File, Size) {

    var Result = Picture_File;

    var File_Name_Extension = Read_File_Name_Extension(Picture_File);

    var Start_Text_Index = Picture_File.length - File_Name_Extension.length - 2;
    var End_Text_Index = Picture_File.length;

    //
    Result = Remove_From_Index_To(Picture_File, Start_Text_Index, End_Text_Index);

    Result += Size + File_Name_Extension;

    return Result;
}

function Get_IMG_Size_Wait_Loaded(IMG_src) {

    var Image_Temp = new Image();

    Image_Temp.src = IMG_src;
}

function Enable_Expand_IMG(UUID) {

    //Write_Message('start: ' + UUID);

    if (Check_Element_Is_Not_Null('Expand_IMG_' + UUID + '_lnk')) {

        $('#Expand_IMG_' + UUID + '_lnk').attr('onclick', "return hs.expand(this, {wrapperClassName: 'borderless floating-caption', dimmingOpacity: 0.75, align: 'center'})");
        $('#Expand_IMG_' + UUID + '_lnk').attr('class', 'highslide');

        //document.getElementById('Expand_IMG_' + UUID + '_lnk').setAttribute("title", "Click để xem ảnh Phóng To !');
        //Write_Message('setAttribute: ' + UUID);

        //$("#Expand_IMG_" + UUID + "_lnk").tooltip("option", "content", "Click để xem ảnh Phóng To !');
        //Write_Message('content: ' + UUID);

        //Write_Message('end: ' + UUID);

        $('#Expand_IMG_' + UUID + '_lnk').removeAttr('onload');
    }
    else {
        //Write_Message('NULL: ' + UUID);
    }
}