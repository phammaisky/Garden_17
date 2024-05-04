function Get_Youtube_Embed_Iframe_Path(url) {

    //var regExp = /^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$/;
    var regExp = /^(?:https?\:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v\=))([\w-]{10,12})(?:$|\&|\?\#).*/;
    var match = url.match(regExp);

    var Video_ID = '';

    for (var i = 0; i < match.length; i++) {
        if (match[i].length == 11) {

            Video_ID = match[i];
            break;
        }
    }

    return 'http://www.youtube.com/embed/' + Video_ID + '?rel=0&autoplay=1';
}

function Get_Youtube_Thumbnail(HTML_Editor_ID, TBX_ID) {

    var Youtube_URL = $('#' + TBX_ID).val();

    var regExp = /^(?:https?\:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v\=))([\w-]{10,12})(?:$|\&|\?\#).*/;
    var match = Youtube_URL.match(regExp);

    var Video_ID = '';

    for (var i = 0; i < match.length; i++) {
        if (match[i].length == 11) {

            Video_ID = match[i];
            break;
        }
    }

    //
    $('#Youtube_Thumbnail_div').empty();
    $('#Youtube_Thumbnail_div').append("<img src = 'http://img.youtube.com/vi/" + Video_ID + "/default.jpg' onclick = \"javascript:Add_Title_img(this.src);\">");
}

function Insert_Youtube_Thumbnail_To_Editor(HTML_Editor_ID) {
}

function Insert_Youtube_Embed_Code(HTML_Editor_ID, TBX_ID, Size) {

    var Youtube_URL = $('#' + TBX_ID).val();
    var Youtube_Embed_Iframe_Path = Get_Youtube_Embed_Iframe_Path(Youtube_URL);

    //
    var Embed_Code_x1 = "<center><iframe src='" + Youtube_Embed_Iframe_Path + "' width = '320' height = '240' allowfullscreen='true' border='0' frameborder='0' marginwidth='0' marginheight='0' scrolling='no'></iframe></center>";
    var Embed_Code_x2 = "<center><iframe src='" + Youtube_Embed_Iframe_Path + "' width = '485' height = '320' allowfullscreen='true' border='0' frameborder='0' marginwidth='0' marginheight='0' scrolling='no'></iframe></center>";
    var Embed_Code_x3 = "<center><iframe src='" + Youtube_Embed_Iframe_Path + "' width = '800' height = '600' allowfullscreen='true' border='0' frameborder='0' marginwidth='0' marginheight='0' scrolling='no'></iframe></center>";

    if (Size == 'x1') {
        Insert_HTML_Editor_Content_At_Caret(HTML_Editor_ID, Embed_Code_x1);
    }
    else
        if (Size == 'x2') {
            Insert_HTML_Editor_Content_At_Caret(HTML_Editor_ID, Embed_Code_x2);
        }
        else
            if (Size == 'x3') {
                Insert_HTML_Editor_Content_At_Caret(HTML_Editor_ID, Embed_Code_x3);
            }
}