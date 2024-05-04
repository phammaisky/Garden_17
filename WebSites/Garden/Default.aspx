<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>The Garden !</title>
</head>
<body id="Page_Body" runat="server">
    <div id="Page_Content_div">
        <form id="Page_Form" runat="server">
            <div id="Loading_div" style="display: block; position: fixed; z-index: 9; text-align: center; top: 200px; left: 400px;">
                <asp:Image ID="Loading_img" runat="server" ImageUrl="/index/Loading/Loading.gif" />
                <br />
                <span class="Bold_Red_Text_css">Loading data... Please wait a moment... Thanks...</span>
            </div>
            <table id="Page_Content_tbl" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr id="Menu_tr">
                    <td style="background-color: #4169E1;">
                        <div runat="server" id="Menu_div" style="background-color: #4169E1;">
                        </div>
                    </td>
                </tr>
                <tr id="Page_Content_tr">
                    <td>
                        <iframe id='Page_Content_ifr' runat="server" width='100%' frameborder='0' marginwidth='0' marginheight='0'
                            scrolling="auto"></iframe>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="Index_Host_hdf" runat="server" />
            <asp:HiddenField ID="Page_Is_Loaded_First_Time_hdf" runat="server" />
        </form>
    </div>
</body>
</html>
