<%@ Page Language="C#" AutoEventWireup="true" Inherits="Switch_Point" Codebehind="Switch_Point.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chuyển điểm !</title>
</head>
<body id="Page_Body" runat="server">
    <div id="Page_Content_div">
        <form id="Page_Form" runat="server">
            <asp:ScriptManager ID="Ajax_ScriptManager" runat="server" EnablePageMethods="true">
            </asp:ScriptManager>
            <div id="Loading_div" style="display: block; position: fixed; z-index: 9; text-align: center; top: 200px; left: 200px;">
                <asp:Image ID="Loading_img" runat="server" ImageUrl="/index/Loading/Loading.gif" />
                <br />
                <span class="Bold_Red_Text_css" style="font-size: x-large">Loading data... Please wait a moment... Thanks...</span>
            </div>
            <table id="Content_tbl" runat="server" width="100%">
                <tr>
                    <td colspan="99" align="center" style="height: 10px"></td>
                </tr>
                <tr>
                    <td colspan="99" align="center">
                        <table>
                            <tr style="height: 20px;">
                                <td width="20px"></td>
                                <td id="Current_Point_1_span" align="center" valign="top" class="Bold_Red_Text_css"></td>
                                <td width="20px"></td>
                                <td></td>
                                <td width="20px"></td>
                                <td id="Current_Point_2_span" align="center" valign="top" class="Bold_Red_Text_css"></td>
                                <td width="20px"></td>
                            </tr>
                            <tr>
                                <td width="20px"></td>
                                <td align="center" valign="top" style="width: 350px">
                                    <asp:TextBox ID="Card_1_tbx" runat="server" Width="95%" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Blue_Text_css" BorderColor="Red" BorderWidth="1" BorderStyle="Solid">0101</asp:TextBox>
                                </td>
                                <td width="20px"></td>
                                <td>
                                    <img src="../index/Mui_Ten_02.jpg" onclick="Submit_Switch_Point_btn_OnClientClick(); return false;" style="cursor: pointer;" /></td>
                                <td width="20px"></td>
                                <td align="center" valign="top" style="width: 350px">
                                    <asp:TextBox ID="Card_2_tbx" runat="server" Width="95%" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Blue_Text_css" BorderColor="Red" BorderWidth="1" BorderStyle="Solid">0101</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="Card_2_tbxe" runat="server" TargetControlID="Card_2_tbx"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td width="20px"></td>
                            </tr>
                            <tr>
                                <td width="20px"></td>
                                <td id="Card_1_Infor_td" valign="top"></td>
                                <td width="20px"></td>
                                <td></td>
                                <td width="20px"></td>
                                <td id="Card_2_Infor_td" valign="top"></td>
                                <td width="20px"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="99" align="center" style="height: 10px"></td>
                </tr>
            </table>
            <asp:HiddenField ID="Loggedin_UserId_hdf" runat="server" />
            <asp:HiddenField ID="Index_Host_hdf" runat="server" />
            <asp:HiddenField ID="PageMethods_Path_hdf" runat="server" />
            <asp:HiddenField ID="Client_Refresh_hdf" runat="server" />
        </form>
    </div>
</body>
</html>
