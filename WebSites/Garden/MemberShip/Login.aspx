<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome to The Garden !</title>
    <link rel="stylesheet" href="Login.css">
    <script language="JavaScript" type="text/javascript">

        function Onload() {

            var window_Height = $(window).innerHeight();
            var window_Width = $(window).innerWidth();

            if (document.getElementById('Page_Content_tbl') != null) {
                document.getElementById('Page_Content_tbl').style.width = window_Width + 'px';
                document.getElementById('Page_Content_tbl').style.height = window_Height + 'px';

                document.getElementById('Page_Content_one_tbl').style.width = window_Width + 'px';
            }
        }
    </script>
</head>
<body id="Page_Body" runat="server">
    <div class="wrapper">
        <div class="container">
            <form id="Page_Form" runat="server" class="form">
            <table id="Page_Content_tbl">
                <tr>
                    <td align="center">
                        <table id="Page_Content_one_tbl">
                            <tr>
                                <td align="center">
                                    <h1>
                                        Welcome to The Garden !</h1>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px;">
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="UserName_tbx" placeholder="Username" runat="server" Width="200"
                                        MaxLength="128"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="Password_tbx" runat="server" Width="200" TextMode="Password" MaxLength="32"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="Login_btn" runat="server" Text="Login !"
                                        OnClick="Login_btn_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Message_lbl" runat="server" Visible="true" CssClass="Red_Text_css"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="Index_Host_hdf" runat="server"  />
            </form>
        </div>
        <ul class="bg-bubbles">
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
        </ul>
    </div>
</body>
</html>
