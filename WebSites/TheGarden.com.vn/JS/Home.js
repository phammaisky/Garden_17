function Menu_Home_On_Click(URL) {

    Hide_Element('Slider_div');
    Hide_Element('News_div');
    Hide_Element('News_List_div');

    Hide_Element('Shop_div');
    Hide_Element('Search_div');

    Display_Element('Register_div');
    document.getElementById('Register_ifr').src = URL;

    jQuery('html,body').animate({ scrollTop: 0 }, 300);

    return false;
}

function Home_resize() {

    var window_Height = $(window).innerHeight();
    var window_Width = $(window).innerWidth();

    var Need_Mobile = false;

    //Desktop
    if (window_Width < 1000) {
        Need_Mobile = true;
    }

    if ((Need_Mobile) && ($('#Is_Mobile_hdf').val() == 'D')) {
        window.location = $('#Redirect_Home_hdf').val();
    }

    if ((!Need_Mobile) && ($('#Is_Mobile_hdf').val() == 'M')) {
        window.location = $('#Redirect_Home_hdf').val();
    }
}

function Email_To_Friend(News_ID) {

    Display_Element('Email_To_Friend_div');
    document.getElementById('Email_To_Friend_ifr').src = "/Email_To_Friend.aspx?News_ID=" + News_ID;

    return false;
}

function Close_Email_To_Friend() {

    Hide_Element('Email_To_Friend_div');

    return false;
}