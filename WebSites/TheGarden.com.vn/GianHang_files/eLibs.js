if (typeof eLibs == 'undefined')
{
    var eLibs = { version:"1.0.0",
        date:"03-11-2011",
        author:"Ngannv",
        conf:function ()
        {
            var doc = document, win = window, n = navigator, ua = n.userAgent;
            this.isOpera = win.opera && opera.buildNumber;
            this.isWebKit = /WebKit/.test(ua);
            this.isIE = !this.isWebKit && !this.isOpera && (/MSIE/gi).test(ua) && (/Explorer/gi).test(n.appName);
            this.isIE6 = this.isIE && /MSIE [56]/.test(ua);
            this.isIE7 = this.isIE && /MSIE [7]/.test(ua);
            this.isGecko = !this.isWebKit && /Gecko/.test(ua);
            this.isMac = ua.indexOf("Mac") != -1;
            this.isAir = /adobeair/i.test(ua);
            this.isChrome = ua.toLowerCase().indexOf("chrome") != -1;
            this.isFireFox = ua.toLowerCase().indexOf('firefox') != -1;
            this.isSafari = ua.toLowerCase().indexOf('safari') != -1;
            this.screenHeight = jQuery(window).height();
            this.screenWidth = jQuery(document).width();

        }, isEmail:function (email)
        {
            var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            return reg.test(email);
        }, checkFileExt:function (fileName, fileTypes)
        {
            if (!fileName) return false;
            var dots = fileName.split(".");
            var fileType = dots[dots.length - 1];
            for (var i in fileTypes)
            {
                if (fileType.toLowerCase() == fileTypes[i].toLowerCase())
                {
                    return true;
                }
            }
            return false;
        } };
    window.eLibs = eLibs;
    eLibs.conf();
}
eLibs.date_format = function (Argument, separator)
{
    if (separator == undefined)
    {
        separator = '-';
    }
    if ((Argument.length == 2) || (Argument.length == 5))
    {
        Argument = Argument + separator
    }
    return Argument;
};
eLibs.date_format_v2 = function (d, e, separator)
{
    if (separator == undefined)
    {
        separator = '-';
    }
    var pK = e ? e.which : window.event.keyCode;
    if (pK == 8)
    {
        d.value = substr(0, d.value.length - 1);
        return;
    }
    var dt = d.value;
    var da = dt.split(separator);
    for (var a = 0; a < da.length; a++)
    {
        if (da[a] != +da[a]) da[a] = da[a].substr(0, da[a].length - 1);
    }
    if (da[0] > 31)
    {
        da[1] = da[0].substr(da[0].length - 1, 1);
        da[0] = '0' + da[0].substr(0, da[0].length - 1);
    }
    if (da[1] > 12)
    {
        da[2] = da[1].substr(da[1].length - 1, 1);
        da[1] = '0' + da[1].substr(0, da[1].length - 1);
    }
    if (da[2] > 9999) da[1] = da[2].substr(0, da[2].length - 1);
    dt = da.join(separator);
    if (dt.length == 2 || dt.length == 5) dt += separator;
    d.value = dt;
};

eLibs.number_format = function (Num)
{
    Num = Num.toString().replace(/^0+/, "").replace(/\./g, "").replace(/,/g, "");
    Num = "" + parseInt(Num);
    var temp1 = "";
    var temp2 = "";
    if (Num == 0 || Num == undefined || Num == '0' || Num == '' || isNaN(Num))
    {
        return '';
    }
    else
    {
        var count = 0;
        for (var k = Num.length - 1; k >= 0; k--)
        {
            var oneChar = Num.charAt(k);
            if (count == 3)
            {
                temp1 += ".";
                temp1 += oneChar;
                count = 1;
                continue;
            }
            else
            {
                temp1 += oneChar;
                count++;
            }
        }
        for (var k = temp1.length - 1; k >= 0; k--)
        {
            var oneChar = temp1.charAt(k);
            temp2 += oneChar;
        }
        return temp2;
    }
};
eLibs.runFunc = function (funcName)
{
    try
    {
        return eval(funcName);
    } catch (exception)
    {
        console.log(exception);
    }

};
eLibs.ajaxLoading = function (is_show)
{
    alert(1)
    if (is_show == undefined || is_show == 'show')
    {
        var container = '<div id="loadding-overlay" style="display: none;"></div>';
        jQuery('body').append(container);
        jQuery('#loadding-overlay').fadeIn(0);
    }
    else
    {
        jQuery('#loadding-overlay').fadeOut('slow').remove();
    }
    return false;

};
eLibs.scrollTo = function (elementId, scrollTime)
{
    if (elementId == undefined || elementId=='')
    {
        jQuery('html,body').animate({scrollTop:0}, 300);
        return false;
    }
    if (!scrollTime || scrollTime == undefined)
    {
        scrollTime = 1000;
    }
    var target = jQuery(elementId);
    target = target.length && target || jQuery('[name=' + elementId.slice(1) + ']');
    if (target.length)
    {
        var targetOffset = target.offset().top;
        jQuery('html,body').animate({scrollTop:targetOffset}, scrollTime);
        return false;
    }
};
eLibs.getScreenSize=function(){
    this.screenHeight = jQuery(window).height();
    this.screenWidth = jQuery(document).width();
};

eLibs.goTopStart = function ()
{
    var strFB = '<iframe src="//www.facebook.com/plugins/like.php?href=https%3A%2F%2Fwww.facebook.com%2Fthegardenhanoi&amp;width=100&amp;height=21&amp;colorscheme=light&amp;layout=button_count&amp;action=like&amp;show_faces=false&amp;send=false" scrolling="no" frameborder="0" style="border:none; overflow:hidden; position: absolute; left: -2px; top: 50px;" allowTransparency="true"></iframe>';
    jQuery('body').append('<a href="javascript:void(0)" onclick="jQuery(\'html,body\').animate({scrollTop: 0},300);" class="go_top _tipsy" title="Lên đầu trang" style="display:none">'+strFB+'</a>');
    var height = jQuery(window).height();
    jQuery(window).scroll(function ()
    {
        var top = jQuery(window).scrollTop();

        if (top > 100)
        {
            if (this.isIE6 || this.isIE7)
            {
                top = top + height - 60;
                jQuery('.go_top').css('top', top);
            }
            jQuery('.go_top').show();
        }
        else
        {
            jQuery('.go_top').hide()
        }
    });
};