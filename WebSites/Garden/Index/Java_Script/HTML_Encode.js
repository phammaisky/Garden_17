function Replace_Index_Host(Media) {

    var Reg_Ex = new RegExp('http://Upload_Host.com/', 'g');

    return String(Media).replace(Reg_Ex, $('#Index_Host_hdf').val() + '/');
}

function Replace_Upload_Host(Media) {

    var Reg_Ex = new RegExp($('#Index_Host_hdf').val() + '/', 'g');

    return String(Media).replace(Reg_Ex, 'http://Upload_Host.com/');
}

function HTML_Tag_Remove(Html) {

    return String(Html)
            .replace(/</g, '')
            .replace(/>/g, '')

            .replace(/&lt;/g, '')
            .replace(/&gt;/g, '')
            ;
}

function HTML_Tag_Encode(Html) {

    return String(Html)
            .replace(/</g, '@LESS_THAN@')
            .replace(/>/g, '@GREATER_THAN@')

            .replace(/&lt;/g, '@LESS_THAN@')
            .replace(/&gt;/g, '@GREATER_THAN@')
            ;
}

function HTML_Book_Mark_Encode(Html) {

    return String(Html)
            .replace(/#/g, '@BOOK_MARK@')
            ;
}




function HTML_Tag_Decode(Html) {

    return String(Html)
            .replace(/@LESS_THAN@/g, '<img src=\"' + $('#Index_Host_hdf').val() + '/index/Less.png\" border=\"0\" />')
            .replace(/@GREATER_THAN@/g, '<img src=\"' + $('#Index_Host_hdf').val() + '/index/Greater.png\" border=\"0\" />')
            ;
}

function HTML_Tag_Decode_Element(ID) {

    var Element_html = $('#' + ID).html();

    Element_html = Element_html
            .replace(/@LESS_THAN@/g, '<img src=\"' + $('#Index_Host_hdf').val() + '/index/Less.png\" border=\"0\" />')
            .replace(/@GREATER_THAN@/g, '<img src=\"' + $('#Index_Host_hdf').val() + '/index/Greater.png\" border=\"0\" />')
            ;

    $('#' + ID).html(Element_html);
}

function HTML_Tag_Decode_Edit(Html) {

    return String(Html)
            .replace(/@LESS_THAN@/g, '<')
            .replace(/@GREATER_THAN@/g, '>')
            ;
}

function Html_Encode(Html) {

    return String(Html)
    //
            .replace(/&lt;/g, '@LESS_THAN@')//'<img src=\"' + $('#Index_Host_hdf').val() + '/index/Less.png\" border=\"0\" />'
            .replace(/&gt;/g, '@GREATER_THAN@')//<img src=\"' + $('#Index_Host_hdf').val() + '/index/Greater.png\" border=\"0\" />

    //
            .replace(/&/g, '&amp;')

    //
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')

    //
            .replace(/'/g, '"')

    //
            .replace(/href\=\"\/\//g, 'href=\"http://')
            .replace(/src\=\"\/\//g, 'src=\"http://')

    //
            .replace(/"/g, '&quot;')

            ;
    //' --> &#x27;
    /// --> &#x2F; .replace(/\//g, '&#x2F;')
}

function Html_Decode(Html) {

    //return unescape(Html);

    return String(Html)

            .replace(/&amp;/g, '&')
            .replace(/&quot;/g, '\"')
            .replace(/&#39;/g, '\"')

            .replace(/&lt;/g, '<')
            .replace(/&gt;/g, '>')

            .replace(/@LESS_THAN@/g, '<img src=\"' + $('#Index_Host_hdf').val() + '/index/Less.png\" border=\"0\" />')
            .replace(/@GREATER_THAN@/g, '<img src=\"' + $('#Index_Host_hdf').val() + '/index/Greater.png\" border=\"0\" />')
            ;
    /**/
}

function Html_Decode_Edit(Html) {

    //return unescape(Html);

    return String(Html)

            .replace(/&amp;/g, '&')
            .replace(/&quot;/g, '\"')
            .replace(/&#39;/g, '\"')

            .replace(/&lt;/g, '<')
            .replace(/&gt;/g, '>')

            .replace(/@LESS_THAN@/g, '&lt;')
            .replace(/@GREATER_THAN@/g, '&gt;')

            .replace(/&LESS_THAN;/g, '&lt;')
            .replace(/&GREATER_THAN;/g, '&gt;')
            ;
    /**/
}

function Replace_Quote(Html) {
    ////[{""}]
    return String(Html)
            .replace("[{\"", '')
            .replace("\"}]", '');
}