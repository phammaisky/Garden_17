<%@ page language="C#" autoeventwireup="true" inherits="_Email_To_Friend, App_Web_pm5ajuum" enableviewstatemac="false" enableEventValidation="false" smartnavigation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gửi bài viết qua email</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="content-language" content="vi" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <link href="GUI%20EMAIL_files/tooltip.css" rel="stylesheet" type="text/css" />
    <link href="GUI%20EMAIL_files/default.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="GUI%20EMAIL_files/thegarden-v2.css" type="text/css" />

    <script src="GUI%20EMAIL_files/522926.js" id="hs-analytics"></script>

    <script src="GUI%20EMAIL_files/fbds.js" async=""></script>

    <script src="GUI%20EMAIL_files/ga.js" async="" type="text/javascript"></script>

    <script type="text/javascript" src="GUI%20EMAIL_files/jquery-1.js"></script>

    <script type="text/javascript" src="GUI%20EMAIL_files/jquery_002.js"></script>

    <script type="text/javascript" src="GUI%20EMAIL_files/jquery.js"></script>

    <script type="text/javascript" src="GUI%20EMAIL_files/library.js"></script>

    <script type="text/javascript" src="GUI%20EMAIL_files/jquery_003.js"></script>

    <script type="text/javascript" src="GUI%20EMAIL_files/mudim.js"></script>

    <script type="text/javascript" src="GUI%20EMAIL_files/tooltip.js"></script>

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
    <div class="popup_join contact-bg" style="margin: 0 auto; text-align: left; border: 1px solid #C6C4C5;
        padding: 13px">
        <div class="contact-header-e">
            <div class="contact-h">
                Gửi cho bạn bè bài viết này
            </div>
        </div>
        <div class="contact-content">
            <div class="contact-input">
                <table class="contact-input-table">
                    <tbody>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                Tên của bạn:
                            </td>
                            <td class="t-col2">
                                <asp:TextBox ID="Name_tbx" runat="server" CssClass="contact-txt" Style="width: 300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                Email của bạn:
                            </td>
                            <td class="t-col2">
                                <asp:TextBox ID="Email_tbx" runat="server" CssClass="contact-txt" Style="width: 300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="t-col1" style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                Tên người nhận:
                            </td>
                            <td class="t-col2">
                                <asp:TextBox ID="To_Name_tbx" runat="server" CssClass="contact-txt" Style="width: 300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                Email gửi đến:
                            </td>
                            <td class="t-col2">
                                <asp:TextBox ID="To_Email_tbx" runat="server" CssClass="contact-txt" Style="width: 300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="t-col1" style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                Tiêu đề:
                            </td>
                            <td class="t-col2">
                                <asp:TextBox ID="Subject_tbx" runat="server" CssClass="contact-txt" Style="width: 300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="t-col1" style="font-size: 14px; font-weight: bold;">
                                Thông điệp:
                            </td>
                            <td class="t-col2">
                                <asp:TextBox ID="Message_tbx" runat="server" CssClass="contact-txt" Style="width: 300px"
                                    TextMode="MultiLine" Rows="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="t-col1" style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="Email_To_Friend_btn" runat="server" Text="Send" OnClick="Email_To_Friend_btn_Click" />
                                <asp:Button ID="Reset_btn" runat="server" Text="Reset" OnClick="Reset_btn_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="c">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
