<%@ page language="C#" autoeventwireup="true" inherits="Manager_News, App_Web_1njdqebw" enableviewstatemac="false" enableEventValidation="false" smartnavigation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manager_News !</title>
    <link href="/CSS/ALL.css" type="text/css" rel="stylesheet" />
</head>
<body id="Page_Body" runat="server">
    <form id="Page_Form" runat="server">
        <asp:ScriptManager ID="Ajax_ScriptManager" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="Ajax_UpdatePanel" runat="server">
            <ContentTemplate>
                <div id="Page_Content_div" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="height: 10px;"></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Manager_News_lbl" runat="server" Text="Manager_News !" class="Bold_Red_Text_css" Font-Size="Larger"></asp:Label>
                                <br />
                                <table style="vertical-align: bottom;">
                                    <tr id="Space_Filter_Time_tr" runat="server">
                                        <td style="height: 10px;"></td>
                                    </tr>
                                    <tr id="Filter_Time_tr" runat="server">
                                        <td align="right" class="Bold_Blue_Text_css">From:</td>
                                        <td>
                                            <asp:TextBox ID="Time_Start_1_tbx" runat="server" Width="80"></asp:TextBox>
                                            <asp:CalendarExtender ID="Time_Start_1_clde" runat="server" TargetControlID="Time_Start_1_tbx"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Time_Start_1_mee" runat="server" ClearMaskOnLostFocus="false"
                                                ClearTextOnInvalid="true" TargetControlID="Time_Start_1_tbx" Mask="99\/99\/9999"
                                                MaskType="Date" AutoComplete="true">
                                            </asp:MaskedEditExtender>
                                        </td>
                                        <td style="width: 10px;"></td>
                                        <td align="right" class="Bold_Blue_Text_css">To:</td>
                                        <td>
                                            <asp:TextBox ID="Time_Finish_1_tbx" runat="server" Width="80"></asp:TextBox>
                                            <asp:CalendarExtender ID="Time_Finish_1_clde" runat="server" TargetControlID="Time_Finish_1_tbx"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Time_Finish_1_mee" runat="server" ClearMaskOnLostFocus="false"
                                                ClearTextOnInvalid="true" TargetControlID="Time_Finish_1_tbx" Mask="99\/99\/9999"
                                                MaskType="Date" AutoComplete="true">
                                            </asp:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Danh mục:</td>
                                        <td colspan="3" align="center">
                                            <asp:DropDownList ID="Index_1_ddl" runat="server" BorderColor="Black" BorderWidth="1" AutoPostBack="true" OnSelectedIndexChanged="Index_1_ddl_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:Label ID="Index_2_lbl" runat="server" Text="Danh mục con:"></asp:Label>
                                            <asp:DropDownList ID="Index_2_ddl" runat="server" BorderColor="Black" BorderWidth="1" AutoPostBack="true" OnSelectedIndexChanged="Index_2_ddl_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="Manager_News_btn" runat="server" Text="Danh sách Bài viết" Width="135" OnClick="Manager_News_btn_Click" OnClientClick="Manager_News_btn_OnClientClick();" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="Or_Der_tr">
                                        <td align="right">Sắp xếp ID:</td>
                                        <td colspan="3" align="center">
                                            <asp:TextBox ID="Or_Der_tbx" runat="server" Width="99%" TextMode="MultiLine" Rows="4" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="Or_Der_btn" runat="server" Text="Lưu thứ tự sắp xếp" Width="135" OnClick="Or_Der_btn_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;"></td>
                        </tr>
                        <tr>
                            <td id="Dynamic_Control_Holder_td" align="center" style="display: block;">
                                <div id="Change_Page_1_div" runat="server" visible="False">
                                    <asp:Label ID="Total_Result_1_lbl" runat="server" CssClass="Blue_Text_css"></asp:Label>
                                    <br />
                                    <asp:Label ID="Page_1_lbl" runat="server" Visible="False" CssClass="Blue_Text_css">Page:</asp:Label>
                                    <asp:Label ID="Change_Page_1_lbl" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                </div>
                                <asp:Panel ID="Dynamic_Control_Holder_pnl" runat="server" Width="100%">
                                </asp:Panel>
                                <div id="Change_Page_2_div" runat="server" visible="False">
                                    <br />
                                    <asp:Label ID="Total_Result_2_lbl" runat="server" CssClass="Blue_Text_css"></asp:Label>
                                    <br />
                                    <asp:Label ID="Page_2_lbl" runat="server" Visible="False" CssClass="Blue_Text_css">Page:</asp:Label>
                                    <asp:Label ID="Change_Page_2_lbl" runat="server"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;"></td>
                        </tr>
                    </table>
                </div>
                <asp:Image ID="Scroll_To_Top_img" runat="server" onclick="Scroll_To_Top_img_OnClient_Click(); return false;"
                    ImageUrl="/index/Scroll_To_Top.png" Style="position: fixed; bottom: 10px; right: 20px; z-index: 1; cursor: pointer;" />
                <asp:HiddenField ID="Index_Host_hdf" runat="server" />
                <asp:HiddenField ID="PageMethods_Path_hdf" runat="server" />
                <asp:HiddenField ID="Client_Refresh_hdf" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
