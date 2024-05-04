<%@ page language="C#" autoeventwireup="true" inherits="_News_Letter, App_Web_ifosqbh3" enableviewstatemac="false" enableEventValidation="false" smartnavigation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đăng ký nhận tin</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="content-language" content="vi" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <link rel="shortcut icon" type="image/png" href="favicon.ico" />
    <link rel="stylesheet" href="./CSS/thegarden-v2.css" type="text/css" />
    <link href="./CSS/Slider.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="./JS/Jquery/jquery-1.10.2.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery-migrate-1.2.1.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery-ui-1.10.3.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery.easing.1.3.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery.cookie.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery.rd-navbar.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery.rd-styleswitcher.js"></script>

    <script type="text/javascript" src="./JS/Jquery/Rotator.js"></script>

    <script type="text/javascript" src="./JS/Jquery/jquery.dropdownPlain.js"></script>

    <!-- Facebook Pixel Code -->

    <script>
!function(f,b,e,v,n,t,s)
{if(f.fbq)return;n=f.fbq=function(){n.callMethod?
n.callMethod.apply(n,arguments):n.queue.push(arguments)};
if(!f._fbq)f._fbq=n;n.push=n;n.loaded=!0;n.version='2.0';
n.queue=[];t=b.createElement(e);t.async=!0;
t.src=v;s=b.getElementsByTagName(e)[0];
s.parentNode.insertBefore(t,s)}(window,document,'script',
'https://connect.facebook.net/en_US/fbevents.js');
fbq('init', '354498808451564'); 
fbq('track', 'PageView');
    </script>

    <noscript>
        <img height="1" width="1" src="https://www.facebook.com/tr?id=354498808451564&ev=PageView
&noscript=1" />
    </noscript>
    <!-- End Facebook Pixel Code -->
    
</head>
<body runat="server" id="Page_Body">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="Ajax_ScriptManager" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div id="site-content">
        <div class="popup_join contact-bg" style="margin: 30px auto; text-align: left; border: 1px solid #C6C4C5;
            padding: 13px; position: static; width: 503px">
            <div class="contact-header">
                <div class="contact-h">
                    Đăng ký nhận tin khuyến mại
                </div>
                <a class="contact-close" href="./"></a>
            </div>
            <div class="contact-content">
                <div class="contact-input">
                    <table class="contact-input-table">
                        <tbody>
                            <tr>
                                <td colspan="4" style="font-size: 12px; font-weight: bold">
                                    Đăng ký để nhận ưu đãi của các chương trình khuyến mại
                                </td>
                            </tr>
                            <tr>
                                <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                    Họ tên:
                                </td>
                                <td class="t-col2">
                                    <asp:TextBox ID="Name_tbx" runat="server" CssClass="contact-txt" Style="width: 140px"></asp:TextBox>
                                </td>
                                <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                    Giới tính:
                                </td>
                                <td class="t-col2">
                                    <div style="margin-top: 7px;">
                                        <asp:RadioButtonList ID="Sex_rdol" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Nam" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Nữ" Value="0" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                    Email:
                                </td>
                                <td class="t-col2">
                                    <asp:TextBox ID="Email_tbx" runat="server" CssClass="contact-txt" Style="width: 140px"></asp:TextBox>
                                </td>
                                <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                    Ngày sinh:
                                </td>
                                <td class="t-col2">
                                    <asp:TextBox ID="Birthday_tbx" runat="server" CssClass="contact-txt" Style="width: 80px"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="Birthday_mee" runat="server" ClearMaskOnLostFocus="false"
                                        ClearTextOnInvalid="true" TargetControlID="Birthday_tbx" Mask="99\/99\/9999"
                                        MaskType="Date" AutoComplete="true">
                                    </asp:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                    Điện thoại:
                                </td>
                                <td class="t-col2">
                                    <asp:TextBox ID="Phone_tbx" runat="server" CssClass="contact-txt" Style="width: 140px"></asp:TextBox>
                                </td>
                                <td colspan="2" style="font-size: 12px;">
                                    <div align="center">
                                        <asp:CheckBox ID="Agree_cbx" runat="server" Text="Tôi đồng ý nhận tin<br/>từ TTTM The Garden"
                                            Checked="true" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                                <td colspan="2">
                                    <div align="center">
                                        <asp:Button ID="OK_btn" runat="server" Text="Đăng Ký" OnClick="OK_btn_Click" />
                                        <asp:Button ID="Clear_btn" runat="server" Text="Nhập Lại" OnClick="Clear_btn_Click" />
                                    </div>
                                    <div style="font-size: 9px; line-height: 14px; padding: 8px; text-align: justify">
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="c">
                </div>
            </div>
        </div>
        <div class="c">
        </div>
    </div>
    </form>
</body>
</html>
