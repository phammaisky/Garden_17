<%@ Page Language="C#" AutoEventWireup="true" Inherits="History" Codebehind="History.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lịch sử Giao dịch !</title>
    <style type="text/css">
        * {
            margin: 0 !important;
            padding: 0 !important;
            font-family: 'Times New Roman' !important;
            font-size: 11.5pt !important;
            color: black !important;
            font-style: normal;
            font-weight:normal;
        }
    </style>
</head>
<body id="Page_Body" runat="server">
    <form id="Page_Form" runat="server">
        <div id="Page_Content_div" runat="server" style="text-align: center; width: 100%;">
            <asp:Panel ID="Dynamic_Control_Holder_pnl" runat="server" Width="100%">
            </asp:Panel>
        </div>
    </form>
</body>
</html>
