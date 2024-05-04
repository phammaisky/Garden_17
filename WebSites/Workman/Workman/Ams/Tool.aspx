<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tool" CodeBehind="Tool.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quản lý tài sản !</title>
</head>
<body id="Page_Body" runat="server">
    <div id="Page_Content_div">
        <form id="Page_Form" runat="server">
            <asp:HiddenField ID="Index_Host_hdf" runat="server" />
            <asp:HiddenField ID="PageMethods_Path_hdf" runat="server" />
            <asp:HiddenField ID="Client_Refresh_hdf" runat="server" />
            <asp:HiddenField ID="Is_Iframe_hdf" runat="server" Value="1" />
        </form>
    </div>
</body>
</html>
