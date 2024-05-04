<%@ Page Language="C#" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home !</title>
</head>
<body>
    <script type="text/javascript">

        $(document).ready(function () {

            //Resize
            $(window).resize(function () {

                var window_Height = $(window).innerHeight();
                var window_Width = $(window).innerWidth();

                $('#Picture_tbl').width(window_Width);
                $('#Picture_tbl').height(window_Height);

                //
                var Platform = $("#Platform_hdf").val();

                var Platform_div_ID = '';

                if (Platform == 'Software') {
                    Platform_div_ID = '_Software';
                }

                if (window_Height <= 768) {

                    if (window_Width > 1024) {

                        $("#Slider_1360_768" + Platform_div_ID).wtRotator({
                            width: window_Width, height: window_Height, button_width: 24, button_height: 24, button_margin: 5, auto_start: true, delay: 5000, transition: "slide",
                            transition_speed: 700, block_size: 75, vert_size: 55, horz_size: 50, cpanel_align: "BR", timer_align: "top", display_thumbs: false, display_dbuttons: true,
                            display_playbutton: true, tooltip_type: "image", display_numbers: false, display_timer: true, mouseover_pause: false, cpanel_mouseover: false,
                            text_mouseover: false, text_effect: "fade", text_sync: true, shuffle: false, block_delay: 25, vstripe_delay: 73, hstripe_delay: 183
                        });

                        $("#yw1").animate({ opacity: 1.0 }, 5000).fadeOut("slow");

                        //
                        $("#Slider_1360_768" + Platform_div_ID).show();
                    }
                    else {

                        $("#Slider_1024_768" + Platform_div_ID).wtRotator({
                            width: window_Width, height: window_Height, button_width: 24, button_height: 24, button_margin: 5, auto_start: true, delay: 5000, transition: "slide",
                            transition_speed: 700, block_size: 75, vert_size: 55, horz_size: 50, cpanel_align: "BR", timer_align: "top", display_thumbs: false, display_dbuttons: true,
                            display_playbutton: true, tooltip_type: "image", display_numbers: false, display_timer: true, mouseover_pause: false, cpanel_mouseover: false,
                            text_mouseover: false, text_effect: "fade", text_sync: true, shuffle: false, block_delay: 25, vstripe_delay: 73, hstripe_delay: 183
                        });

                        $("#yw1").animate({ opacity: 1.0 }, 5000).fadeOut("slow");

                        //
                        $("#Slider_1024_768" + Platform_div_ID).show();
                    }
                }
                else {
                    $("#Slider_1280_1024" + Platform_div_ID).wtRotator({
                        width: window_Width, height: window_Height, button_width: 24, button_height: 24, button_margin: 5, auto_start: true, delay: 5000, transition: "slide",
                        transition_speed: 700, block_size: 75, vert_size: 55, horz_size: 50, cpanel_align: "BR", timer_align: "top", display_thumbs: false, display_dbuttons: true,
                        display_playbutton: true, tooltip_type: "image", display_numbers: false, display_timer: true, mouseover_pause: false, cpanel_mouseover: false,
                        text_mouseover: false, text_effect: "fade", text_sync: true, shuffle: false, block_delay: 25, vstripe_delay: 73, hstripe_delay: 183
                    });

                    $("#yw1").animate({ opacity: 1.0 }, 5000).fadeOut("slow");

                    //
                    $("#Slider_1280_1024" + Platform_div_ID).show();
                }

            }).trigger("resize");
        });
    </script>
    <form id="Page_Form" runat="server">
        <table id="Picture_tbl" border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center" valign="middle">
                    <div id="Slider_1360_768" class="slider" style="display: none;">
                        <div class="wt-rotator">
                            <div class="screen">
                                <div style="margin: 5px 0px; top: 264px; right: 0px; height: 26px; visibility: visible;"
                                    class="c-panel">
                                    <div style="float: right;" class="buttons">
                                        <div class="prev-btn">
                                        </div>
                                        <div class="play-btn">
                                        </div>
                                        <div class="next-btn">
                                        </div>
                                    </div>
                                    <div style="height: 26px; float: right;" class="thumbnails">
                                        <ul>
                                            <li><a href='/index/Slider/Garden/1360x768/1.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1360x768/2.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1360x768/3.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1360x768/4.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1360x768/5.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1360x768/6.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1360x768/7.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1360x768/8.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1360x768/9.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1360x768/10.jpg'></a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Slider_1280_1024" class="slider" style="display: none;">
                        <div class="wt-rotator">
                            <div class="screen">
                                <div style="margin: 5px 0px; top: 264px; right: 0px; height: 26px; visibility: visible;"
                                    class="c-panel">
                                    <div style="float: right;" class="buttons">
                                        <div class="prev-btn">
                                        </div>
                                        <div class="play-btn">
                                        </div>
                                        <div class="next-btn">
                                        </div>
                                    </div>
                                    <div style="height: 26px; float: right;" class="thumbnails">
                                        <ul>
                                            <li><a href='/index/Slider/Garden/1280x1024/1.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1280x1024/2.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1280x1024/3.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1280x1024/4.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1280x1024/5.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1280x1024/6.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1280x1024/7.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1280x1024/8.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1280x1024/9.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1280x1024/10.jpg'></a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Slider_1024_768" class="slider" style="display: none;">
                        <div class="wt-rotator">
                            <div class="screen">
                                <div style="margin: 5px 0px; top: 264px; right: 0px; height: 26px; visibility: visible;"
                                    class="c-panel">
                                    <div style="float: right;" class="buttons">
                                        <div class="prev-btn">
                                        </div>
                                        <div class="play-btn">
                                        </div>
                                        <div class="next-btn">
                                        </div>
                                    </div>
                                    <div style="height: 26px; float: right;" class="thumbnails">
                                        <ul>
                                            <li><a href='/index/Slider/Garden/1024x768/1.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1024x768/2.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1024x768/3.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1024x768/4.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1024x768/5.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1024x768/6.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1024x768/7.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1024x768/8.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1024x768/9.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/1024x768/10.jpg'></a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Slider_1360_768_Software" class="slider" style="display: none;">
                        <div class="wt-rotator">
                            <div class="screen">
                                <div style="margin: 5px 0px; top: 264px; right: 0px; height: 26px; visibility: visible;"
                                    class="c-panel">
                                    <div style="float: right;" class="buttons">
                                        <div class="prev-btn">
                                        </div>
                                        <div class="play-btn">
                                        </div>
                                        <div class="next-btn">
                                        </div>
                                    </div>
                                    <div style="height: 26px; float: right;" class="thumbnails">
                                        <ul>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/1.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/2.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/3.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/4.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/5.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/6.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/7.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/8.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/9.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1360x768/10.jpg'></a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Slider_1280_1024_Software" class="slider" style="display: none;">
                        <div class="wt-rotator">
                            <div class="screen">
                                <div style="margin: 5px 0px; top: 264px; right: 0px; height: 26px; visibility: visible;"
                                    class="c-panel">
                                    <div style="float: right;" class="buttons">
                                        <div class="prev-btn">
                                        </div>
                                        <div class="play-btn">
                                        </div>
                                        <div class="next-btn">
                                        </div>
                                    </div>
                                    <div style="height: 26px; float: right;" class="thumbnails">
                                        <ul>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/1.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/2.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/3.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/4.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/5.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/6.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/7.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/8.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/9.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1280x1024/10.jpg'></a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Slider_1024_768_Software" class="slider" style="display: none;">
                        <div class="wt-rotator">
                            <div class="screen">
                                <div style="margin: 5px 0px; top: 264px; right: 0px; height: 26px; visibility: visible;"
                                    class="c-panel">
                                    <div style="float: right;" class="buttons">
                                        <div class="prev-btn">
                                        </div>
                                        <div class="play-btn">
                                        </div>
                                        <div class="next-btn">
                                        </div>
                                    </div>
                                    <div style="height: 26px; float: right;" class="thumbnails">
                                        <ul>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/1.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/2.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/3.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/4.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/5.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/6.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/7.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/8.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/9.jpg'></a></li>
                                            <li><a href='/index/Slider/Garden/Software/1024x768/10.jpg'></a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="Index_Host_hdf" runat="server" />
        <asp:HiddenField ID="Platform_hdf" runat="server" Value="Website" />
    </form>
</body>
</html>
