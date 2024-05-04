<%@ page language="C#" autoeventwireup="true" inherits="_Default, App_Web_pm5ajuum" enableviewstatemac="false" enableEventValidation="false" smartnavigation="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>The Garden Shopping Center</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="content-language" content="vi" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <meta content="0" http-equiv="EXPIRES">
    <meta content="DOCUMENT" name="RESOURCE-TYPE">
    <meta content="GLOBAL" name="DISTRIBUTION">
    <meta content="Thegarden" name="AUTHOR">
    <meta content="Copyright (c) by Thegarden" name="COPYRIGHT">
    <meta content="INDEX, FOLLOW" name="ROBOTS">
    <meta content="index,follow,archive" name="Googlebot">
    <meta content="4LXwegZxdz27rvvAJ22lrHvC9A67Z9jB0px76pn96bI" name="google-site-verification">
    <meta content="https://www.facebook.com/thegardenhanoi" property="article:author">
    <meta content="GENERAL" name="RATING">
    <meta content="Thegarden" name="GENERATOR">
    <meta name="description" runat="server" id="Description_meta" />
    <meta name="keywords" runat="server" id="Keywords_meta" />
    <link rel="shortcut icon" type="image/png" href="favicon.ico" />
    <link href="./CSS/thegarden-v2.css" type="text/css" rel="stylesheet" />
    <link href="./CSS/ALL.css" type="text/css" rel="stylesheet" />
    <link href="./CSS/Slider.css" type="text/css" rel="stylesheet" />
    <link href="./CSS/popup.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="./JS/Jquery/jquery-1.10.2.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery-migrate-1.2.1.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery-ui-1.10.3.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery.easing.1.3.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery.cookie.js"></script>

    <script src="./JS/Jquery/jssor.slider-23.1.6.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        jQuery(document).ready(function($) {

            if ($('#Enable_Slider_hdf').val() == '1') {

                var jssor_1_SlideshowTransitions = [
                  { $Duration: 1200, x: -0.3, $During: { $Left: [0.3, 0.7] }, $Easing: { $Left: $Jease$.$InCubic, $Opacity: $Jease$.$Linear }, $Opacity: 0 }
                ];

                var jssor_1_options = {
                    $AutoPlay: 1,
                    $SlideshowOptions: {
                        $Class: $JssorSlideshowRunner$,
                        $Transitions: jssor_1_SlideshowTransitions,
                        $TransitionsOrder: 1
                    },
                    $ArrowNavigatorOptions: {
                        $Class: $JssorArrowNavigator$
                    },
                    $BulletNavigatorOptions: {
                        $Class: $JssorBulletNavigator$
                    },
                    $ThumbnailNavigatorOptions: {
                        $Class: $JssorThumbnailNavigator$,
                        $Cols: 1,
                        $Align: 0,
                        $NoDrag: true
                    }
                };

                var jssor_1_slider = new $JssorSlider$("jssor_1", jssor_1_options);

                /*responsive code begin*/
                /*remove responsive code if you don't want the slider scales while window resizing*/
                function ScaleSlider() {
                    var refSize = jssor_1_slider.$Elmt.parentNode.clientWidth;
                    if (refSize) {
                        refSize = Math.min(refSize, 700);
                        jssor_1_slider.$ScaleWidth(refSize);
                    }
                    else {
                        window.setTimeout(ScaleSlider, 30);
                    }
                }
                ScaleSlider();
                $(window).bind("load", ScaleSlider);
                $(window).bind("resize", ScaleSlider);
                $(window).bind("orientationchange", ScaleSlider);
                /*responsive code end*/
            }
            else {
                Hide_Element('jssor_1');
            }
        });
    </script>

    <style>
        /* jssor slider bullet navigator skin 01 css *//*
        .jssorb01 div           (normal)
        .jssorb01 div:hover     (normal mouseover)
        .jssorb01 .av           (active)
        .jssorb01 .av:hover     (active mouseover)
        .jssorb01 .dn           (mousedown)
        */.jssorb01
        {
            position: absolute;
        }
        .jssorb01 div, .jssorb01 div:hover, .jssorb01 .av
        {
            position: absolute; /* size of bullet elment */
            width: 12px;
            height: 12px;
            filter: alpha(opacity=70);
            opacity: .7;
            overflow: hidden;
            cursor: pointer;
            border: #000 1px solid;
        }
        .jssorb01 div
        {
            background-color: gray;
        }
        .jssorb01 div:hover, .jssorb01 .av:hover
        {
            background-color: #d3d3d3;
        }
        .jssorb01 .av
        {
            background-color: #fff;
        }
        .jssorb01 .dn, .jssorb01 .dn:hover
        {
            background-color: #555555;
        }
        /* jssor slider arrow navigator skin 05 css *//*
        .jssora05l                  (normal)
        .jssora05r                  (normal)
        .jssora05l:hover            (normal mouseover)
        .jssora05r:hover            (normal mouseover)
        .jssora05l.jssora05ldn      (mousedown)
        .jssora05r.jssora05rdn      (mousedown)
        .jssora05l.jssora05lds      (disabled)
        .jssora05r.jssora05rds      (disabled)
        */.jssora05l, .jssora05r
        {
            display: block;
            position: absolute; /* size of arrow element */
            width: 40px;
            height: 40px;
            cursor: pointer;
            background: url('img/a17.png') no-repeat;
            overflow: hidden;
        }
        .jssora05l
        {
            background-position: -10px -40px;
        }
        .jssora05r
        {
            background-position: -70px -40px;
        }
        .jssora05l:hover
        {
            background-position: -130px -40px;
        }
        .jssora05r:hover
        {
            background-position: -190px -40px;
        }
        .jssora05l.jssora05ldn
        {
            background-position: -250px -40px;
        }
        .jssora05r.jssora05rdn
        {
            background-position: -310px -40px;
        }
        .jssora05l.jssora05lds
        {
            background-position: -10px -40px;
            opacity: .3;
            pointer-events: none;
        }
        .jssora05r.jssora05rds
        {
            background-position: -70px -40px;
            opacity: .3;
            pointer-events: none;
        }
        /* jssor slider thumbnail navigator skin 09 css */.jssort09-600-45 .p
        {
            position: absolute;
            top: 0;
            left: 0;
            width: 700px;
            height: 45px;
        }
        .jssort09-600-45 .t
        {
            font-family: verdana;
            font-weight: normal;
            position: absolute;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            color: #fff;
            line-height: 45px;
            font-size: 20px;
            padding-left: 10px;
        }
    </style>

    <script>
        (function(i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function() {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', 'https://www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-103835490-1', 'auto');
        ga('send', 'pageview');

    </script>

    <!-- Facebook Pixel Code -->
<script>
    !function(f, b, e, v, n, t, s) {
        if (f.fbq) return; n = f.fbq = function() {
            n.callMethod ?
  n.callMethod.apply(n, arguments) : n.queue.push(arguments)
        };
        if (!f._fbq) f._fbq = n; n.push = n; n.loaded = !0; n.version = '2.0';
        n.queue = []; t = b.createElement(e); t.async = !0;
        t.src = v; s = b.getElementsByTagName(e)[0];
        s.parentNode.insertBefore(t, s)
    } (window, document, 'script',
  'https://connect.facebook.net/en_US/fbevents.js');
    fbq('init', '366589777542955');
    fbq('track', 'PageView');
</script>
<noscript><img height="1" width="1" style="display:none"
  src="https://www.facebook.com/tr?id=366589777542955&ev=PageView&noscript=1"
/></noscript>
<!-- End Facebook Pixel Code -->

    
</head>
<body runat="server" id="Page_Body" style="background-color: white;">
    <form id="form1" runat="server">
    <div class="site-bd">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tbody>
                <tr>
                    <td valign="top">
                        <div id="site-left" class="fl">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td valign="top">
                                            <div class="bd-left">
                                                <div class="bd-left-top">
                                                    <div class="header-slogan">
                                                        <a id="logo" href="/" class="fl">
                                                            <img class="logo" src="Home_files/1467259424.jpg" alt="TTTM TheGarden" /></a>
                                                    </div>
                                                    <div class="clear-both">
                                                    </div>
                                                    <ul class="left-menu-top">
                                                        <li><a href="/">Trang chủ </a></li>
                                                        <li style="display: none;"><a href="/?I1=000">ABC</a> </li>
                                                        <li><a href="/?G=Su-kien-Khuyen-mai&I1=1">Sự kiện &amp; khuyến mại</a>
                                                            <ul id="Index_1_ul_1" runat="server" style="display: none;">
                                                            </ul>
                                                        </li>
                                                        <li><a href="/?G=Gian-hang&I1=2">Gian hàng</a> </li>
                                                        <li><a href="/?G=The-Garden-Club&I1=3">The Garden Club</a>
                                                            <ul id="Index_1_ul_3" runat="server" style="display: none;">
                                                                <li><a href="#" onclick="return Menu_Home_On_Click('/Reg_Card.aspx');">Đăng ký thành
                                                                    viên</a></li>
                                                            </ul>
                                                        </li>
                                                        <li><a href="/?I1=30">Ưu đãi ngân hàng</a> </li>
                                                        <li><a href="#" onclick="return Menu_Home_On_Click('/News_Letter.aspx');">Nhận bản tin</a>
                                                        </li>
                                                        <li><a href="/?G=Lien-he&I1=5">Liên hệ</a> </li>
                                                        <li><a href="/?G=Tuyen-dung&I1=12">Tuyển dụng</a> </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <div class="bd-left mt10">
                                                <div class="e-left-bottom">
                                                    <div class="e-left">
                                                        <ul class="left-icon-text">
                                                            <li><a href="https://www.facebook.com/thegardenhanoi" target="_blank" class="upcase">
                                                                <img src="Home_files/20150904101909_20130903171619_fb1.jpg" />
                                                            </a></li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <div class="fb-like" data-href="https://www.facebook.com/thegardenhanoi" data-width="120"
                                                data-layout="standard" data-action="like" data-size="small" data-show-faces="true"
                                                data-share="true">
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>
                    <td valign="top">
                        <div id="site-right" class="fl">
                            <div class="bd-right">
                                <div id="site-header">
                                    <div class="header-region">
                                        <div id="jssor_1" style="position: relative; margin: 0 auto; top: 0px; left: 0px;
                                            width: 700px; height: 425px; overflow: hidden; visibility: hidden;">
                                            <!-- Loading Screen -->
                                            <div data-u="loading" style="position: absolute; top: 0px; left: 0px; background-color: rgba(0,0,0,0.7);">
                                                <div style="filter: alpha(opacity=70); opacity: 0.7; position: absolute; display: block;
                                                    top: 0px; left: 0px; width: 100%; height: 100%;">
                                                </div>
                                                <div style="position: absolute; display: block; background: url('img/loading.gif') no-repeat center center;
                                                    top: 0px; left: 0px; width: 100%; height: 100%;">
                                                </div>
                                            </div>
                                            <div id="Slider" runat="server" data-u="slides" style="cursor: default; position: relative;
                                                top: 0px; left: 0px; width: 700px; height: 425px; overflow: hidden;">
                                            </div>
                                            <!-- Thumbnail Navigator -->
                                            <div data-u="thumbnavigator" class="jssort09-600-45" style="position: absolute; bottom: 0px;
                                                left: 0px; width: 700px; height: 45px;">
                                                <div style="position: absolute; top: 0; left: 0; width: 100%; height: 100%; background-color: transparent;
                                                    filter: alpha(opacity=40.0); opacity: 0.4;">
                                                </div>
                                                <!-- Thumbnail Item Skin Begin -->
                                                <div data-u="slides" style="cursor: default;">
                                                    <div data-u="prototype" class="p">
                                                        <div data-u="thumbnailtemplate" class="t">
                                                        </div>
                                                    </div>
                                                </div>
                                                <!-- Thumbnail Item Skin End -->
                                            </div>
                                            <!-- Bullet Navigator -->
                                            <div data-u="navigator" class="jssorb01" style="bottom: 16px; right: 16px;">
                                                <div data-u="prototype" style="width: 12px; height: 12px;">
                                                </div>
                                            </div>
                                            <!-- Arrow Navigator -->
                                            <span data-u="arrowleft" class="jssora05l" style="top: 0px; left: 8px; width: 40px;
                                                height: 40px;" data-autocenter="2"></span><span data-u="arrowright" class="jssora05r"
                                                    style="top: 0px; right: 8px; width: 40px; height: 40px;" data-autocenter="2">
                                            </span>
                                        </div>
                                    </div>
                                    <!--end header-region-->
                                    <div class="clear-both">
                                    </div>
                                    <div runat="server" id="Search_div" style="position: relative; display: none;">
                                        <div class="header-menu-bottom in-store-guide">
                                            <div class="h-key-search">
                                                <asp:TextBox ID="Search_Shop_tbx" runat="server" class="keys-txt-2" BackColor="White"></asp:TextBox>
                                                <asp:ImageButton ID="Search_Shop_btn" runat="server" class="search-btn" Height="18"
                                                    Width="18" ImageUrl="~/Picture/1px.png" OnClick="Search_Shop_btn_Click" />
                                            </div>
                                        </div>
                                        <div class="clear-both">
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                                <div id="site-content">
                                    <div class="e-news-list" style="background: #fff;">
                                        <div id="Shop_div" runat="server" style="display: none; height: 540px;">
                                            <div class="store-guide-box">
                                                <div class="store-list-box">
                                                    <div class="all-title">
                                                        Tất cả
                                                    </div>
                                                    <ul class="store-list">
                                                        <li class="store-list-headding" style="border-bottom: 1px solid #aaa;">
                                                            <label class="store-headding-col" style="width: 225px;">
                                                                Gian hàng
                                                            </label>
                                                            <label class="store-headding-col" style="margin-left: 35px;">
                                                                Tầng
                                                            </label>
                                                        </li>
                                                        <div id="Shop_List_div" runat="server" tabindex="0" style="overflow: hidden; width: 333px;
                                                            height: 400px;">
                                                        </div>
                                                    </ul>
                                                </div>
                                                <div class="show_cat_estore">
                                                    <div class="store-list-headding" style="border-left: 1px solid #aaa; border-right: 1px solid #aaa;
                                                        border-top: 1px solid #aaa;">
                                                        <label class="store-headding-col all-title-e">
                                                            Danh mục</label>
                                                    </div>
                                                    <div>
                                                        <ul class="store-list store-list-e">
                                                            <div id="Shop_Index_List_div" runat="server" tabindex="0" style="overflow: hidden;
                                                                width: 313px; height: 400px;">
                                                                <li><a class="all" href="/?G=Gian-hang&I1=2">Tất cả</a></li>
                                                            </div>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div id='Register_div' style="display: none;">
                                            <iframe id='Register_ifr' width='100%' frameborder='0' marginwidth='0' marginheight='0'
                                                scrolling="auto" style="height: 600px;"></iframe>
                                        </div>
                                        <div id="News_div" runat="server" style="display: none;">
                                            <asp:Label ID="News_img" runat="server"></asp:Label>
                                            <div style="margin-left: 80px; margin-right: 80px; margin-top: 20px;">
                                                <asp:Label ID="News_lbl" runat="server"></asp:Label>
                                                <div style="clear: both">
                                                </div>
                                                <div id="fb-root">
                                                </div>

                                                <script>
                                                    (function(d, s, id) {
                                                        var js, fjs = d.getElementsByTagName(s)[0];
                                                        if (d.getElementById(id)) return;
                                                        js = d.createElement(s); js.id = id;
                                                        js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.8";
                                                        fjs.parentNode.insertBefore(js, fjs);
                                                    } (document, 'script', 'facebook-jssdk'));

                                                </script>

                                                <script src="https://apis.google.com/js/platform.js" async defer></script>

                                                <div style="float: right;">
                                                    <div style="text-align: right; width: 100%;">
                                                        <div class="fb-follow" data-href="https://www.facebook.com/thegardenhanoi" data-layout="button_count"
                                                            data-size="small" data-show-faces="true">
                                                        </div>
                                                        <div id="Share_This_FB_div" runat="server" class="fb-like" data-href="" data-layout="button_count"
                                                            data-action="like" data-size="small" data-show-faces="true" data-share="true">
                                                        </div>
                                                        <div class="g-plusone" data-annotation="inline" data-width="100">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div style="clear: both">
                                                </div>
                                                <div class="news_tags">
                                                    <b>Tags:</b>
                                                    <asp:Label ID="Tags_lbl" runat="server"></asp:Label>
                                                </div>
                                                <div style="clear: both">
                                                </div>
                                                <div id='Email_To_Friend_div' style="display: none;">
                                                    <iframe id='Email_To_Friend_ifr' width='100%' frameborder='0' marginwidth='0' marginheight='0'
                                                        scrolling="auto" style="height: 420px;"></iframe>
                                                </div>
                                                <div style="clear: both">
                                                </div>
                                                <div class="footer-detail-news">
                                                    <asp:HyperLink ID="Back_To_Index_lnk" runat="server"></asp:HyperLink>
                                                    <span class="spr">|</span> <a href="/?G=Lien-he&I1=5">Đăng ký thuê</a> <span class="spr">
                                                        |</span> <a id="Email_To_Friend_lnk" runat="server" href="#">Gửi email cho bạn bè</a>
                                                    <span id="Space_Edit_News_lnk" runat="server" visible="false" class="spr">|</span>
                                                    <a id="Edit_News_lnk" runat="server" visible="false" target="_blank" href="#">Sửa bài
                                                        viết</a>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="News_List_div" runat="server" style="display: none;">
                                            <table id="News_List_tbl" class="news-list" border="0" cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="Change_Page_1_lbl" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <asp:Label ID="News_List_lbl" runat="server"></asp:Label>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="Change_Page_2_lbl" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <!--end.news-list-->
                                    </div>
                                </div>
                            </div>
                            <div class="clear-both">
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="clear-both">
        </div>
    </div>
    <div style="clear: both;">
    </div>
    <div class="footer">
        <div class="e-footer">
            <div class="e-footer-content">
                <div class="e-f-logo">
                </div>
                <div class="e-footer-menu">
                    <ul>
                        <li><a href="/">Trang chủ</a></li>
                        <li><a href="/?G=Gioi-Thieu&I1=6">Về chúng tôi</a></li>
                        <li><a href="#" onclick="return Menu_Home_On_Click('https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3724.438353752566!2d105.77560281426626!3d21.015139393620586!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x313454aa0fed56c7%3A0x1ba275bf03d4c1a9!2zVHJ1bmcgdMOibSB0aMawxqFuZyBt4bqhaSBUaGUgR2FyZGVu!5e0!3m2!1svi!2s!4v1488078997682');">
                            Bản đồ</a></li>
                    </ul>
                    <div class="clear-both">
                    </div>
                    <div class="e-footer-contact">
                        TTTM The Garden, khu đô thị The Manor, đường Mễ Trì, phường Mỹ Đình 1, quận Nam
                        Từ Liêm, Hà Nội<br />
                        ĐT: 024 3787 5500 | Fax: 024 3787 5511<br />
                        Giờ mở cửa: 10h00 - 22h00 (ngày thường) | 9h00 - 22h00 (Thứ 7, Chủ nhật &amp; ngày
                        lễ)
                    </div>
                </div>
            </div>
            <div class="clear-both">
            </div>
            <a style="display: block; position: fixed; bottom: 50px; right: 20px;" title="Lên đầu trang"
                class="go_top _tipsy" onclick="jQuery('html,body').animate({scrollTop: 0},300);"
                href="javascript:void(0)">
                <div style="text-align: center;">
                    <img src="Picture/gotop.gif" />
                    <br />
                    <br />
                    <div class="fb-like" data-href="https://www.facebook.com/thegardenhanoi" data-layout="button"
                        data-action="like" data-size="small" data-show-faces="false" data-share="false">
                    </div>
                </div>
            </a>
        </div>
    </div>
    <asp:HiddenField ID="Enable_Slider_hdf" runat="server" Value="1" />
    <asp:HiddenField ID="Is_Mobile_hdf" runat="server" Value="D" />
    <asp:HiddenField ID="Redirect_Home_hdf" runat="server" Value="0" />
    </form>
</body>
</html>
