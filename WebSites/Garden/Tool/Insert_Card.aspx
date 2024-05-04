<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Insert_Card.aspx.cs" Inherits="Insert_Card" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Insert Card !</title>
</head>
<body id="Page_Body" runat="server">
    <div id="Page_Content_div">
        <form id="Page_Form" runat="server">
            <div id="Loading_div" style="display: block; position: fixed; z-index: 9; text-align: center; top: 200px; left: 200px;">
                <asp:Image ID="Loading_img" runat="server" ImageUrl="/index/Loading/Loading.gif" />
                <br />
                <span class="Bold_Red_Text_css" style="font-size: x-large">Loading data... Please wait a moment... Thanks...</span>
            </div>
            <table id="All_tbl" border="0" style="width: 100%; height: 100%;">
                <tr>
                    <td align="center" valign="middle">
                        <table border="0" style="width: 100%;">
                            <tr>
                                <td align="right">
                                    <asp:TextBox ID="From_Card_tbx" runat="server" Width="300px" BorderColor="Red" BorderWidth="1" Style="text-align:center;"></asp:TextBox></td>
                                <td style="width: 20px;"></td>
                                <td style="width: 50px;">
                                    <asp:Button ID="OK_btn" runat="server" Text="OK !" Width="48" OnClick="OK_btn_Click" OnClientClick="Insert_Card_btn_OnClientClick();" ForeColor="Red" Font-Bold="true" /></td>
                                <td style="width: 20px;"></td>
                                <td align="left">
                                    <asp:TextBox ID="To_Card_tbx" runat="server" Width="300px" BorderColor="Red" BorderWidth="1" Style="text-align:center;"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="5" style="height: 50px;"></td>
                            </tr>
                            <tr>
                                <td colspan="5" align="center" valign="middle">
                                    <asp:Label ID="Message_lbl" runat="server" Font-Bold="true" Font-Size="20" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="Index_Host_hdf" runat="server" />
        </form>
    </div>
</body>
</html>
