function Setup_Editor(Editor_ID) {

    if (Check_Element_Is_Not_Null(Editor_ID)) {

        try {
            tinymce.init({
                selector: '#' + Editor_ID,
                theme: 'modern',
                width: 702,
                height: 700,

                theme_advanced_resizing: true,

                menubar: false,
                statusbar: false,

                forced_root_block: false,

                plugins: [
                 'link lists charmap print',
                 'searchreplace code media',
                 'table directionality emoticons paste textcolor'
            ],
                
                body_class: '/JS/tinymce/HTML_Editor.css',
                content_css: '/JS/tinymce/HTML_Editor.css',
                editor_css: '/JS/tinymce/HTML_Editor.css',

                theme_advanced_default_foreground_color: '#FF0000',
                theme_advanced_default_background_color: '#FF0000',


                toolbar: 'media link unlink charmap | pastetext removeformat undo redo | bold italic underline strikethrough | fontsizeselect | forecolor backcolor | alignleft aligncenter alignright alignjustify | subscript superscript | code | table bullist numlist | searchreplace'//emoticons preview
            //,

            //    valid_styles: {
            //        '*': 'text-decoration,color,background-color,text-align,max-width'
            //    }
            //,

            //    valid_elements: 'a[href|target:_blank],img[src|border:0],iframe[src|style|width:400|height:300|border:0|frameborder:0|marginwidth:0|marginheight:0|scrolling:no],center,strong,em,i,u,strike,div[style],span[style],sub,sup,br,table[border|width|height|style],tbody[border|width|height|style],tr[border|width|height|style],td[border|width|height|style],ul[style],li[style],ol[style]',
            //    extended_valid_elements: 'iframe[src|style|width|height|border:0|frameborder:0|marginwidth:0|marginheight:0|scrolling:no],embed[id|src|width|height|pluginspage:http://www.macromedia.com/go/getflashplayer|allowscriptaccess:always|allowfullscreen:true|bgcolor:#000000|wmode:transparent|scale:NoBorder|quality:Best|type:application/x-shockwave-flash],video[width|height|controls|style],audio[width|height|controls|style],source[src|type]'
            });

            //
            var Active_Editor = tinymce.activeEditor;
            var Active_Editor_ifr = tinymce.DOM.get(Active_Editor.id + '_ifr');
            Active_Editor.dom.setAttrib(Active_Editor_ifr, 'title', '');
        }
        catch (e) {
        }
    }
}

function Set_HTML_Editor_Content(Editor_ID, Content) {

    try {
        tinyMCE.get(Editor_ID).setContent(Content);
    }
    catch (e) {
        window.setTimeout(function () { Set_HTML_Editor_Content(Editor_ID, Content) }, 1 * 1000);
    }
}

function Get_HTML_Editor_Content(Editor_ID) {

    var Result = '';

    try {
        Result = tinyMCE.get(Editor_ID).getContent();
    }
    catch (e) {
    }

    return Result;
}

function Insert_HTML_Editor_Content_At_Caret(ID, Content) {

    if (Check_Element_Is_Not_Null(Content)) {
        Content = $('#' + Content).val();
    }

    try {
        tinyMCE.activeEditor.selection.setContent(Content);
    }
    catch (e) {
    }
}