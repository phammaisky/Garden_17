<%@ page language="C#" autoeventwireup="true" inherits="Report, App_Web_1njdqebw" enableviewstatemac="false" enableEventValidation="false" smartnavigation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quản lý !</title>
    <link href="/CSS/ALL.css" type="text/css" rel="stylesheet" />
</head>
<body id="Page_Body" runat="server">
    <form id="Page_Form" runat="server">
        <asp:ScriptManager ID="Ajax_ScriptManager" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="Ajax_UpdatePanel" runat="server">
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="Page_Content_div" runat="server">
            <div id="Loading_div" style="display: block; position: fixed; z-index: 9; text-align: center; top: 200px; left: 200px;">
                <asp:Image ID="Loading_img" runat="server" ImageUrl="/index/Loading/Loading.gif" />
                <br />
                <span class="Bold_Red_Text_css" style="font-size: x-large">Loading data... Please wait a moment... Thanks...</span>
            </div>
            <table width="100%">
                <tr>
                    <td style="height: 10px;"></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Report_lbl" runat="server" Text="Report !" class="Bold_Red_Text_css" Font-Size="Larger"></asp:Label>
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
                            <tr id="Space_Banner_tr" runat="server" visible="false">
                                <td style="height: 10px;"></td>
                            </tr>
                            <tr id="Banner_tr" runat="server" visible="false">
                                <td colspan="5" align="center">
                                    <table>
                                        <tr>
                                            <td align="right">Danh mục:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="Index_1_ddl" runat="server" BorderColor="Black" BorderWidth="1" AutoPostBack="true" OnSelectedIndexChanged="Index_1_ddl_SelectedIndexChanged"></asp:DropDownList>
                                                Danh mục con:
                                                    <asp:DropDownList ID="Index_2_ddl" runat="server" BorderColor="Black" BorderWidth="1" AutoPostBack="true" OnSelectedIndexChanged="Index_2_ddl_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td align="right">URL:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="URL_tbx" runat="server" Width="98%" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="Submit_Banner_btn" runat="server" Text="Tạo mới" Width="100" OnClick="Submit_Banner_btn_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Image ID="Picture_img" runat="server" Visible="false" Style="max-width: 120px; max-height: 120px;" />
                                                <asp:HiddenField ID="Picture_hdf" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="Picture_upl" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Cancel_btn" runat="server" Text="Hủy" Width="100" OnClick="Cancel_btn_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">Sắp xếp ID:</td>
                                            <td align="center">
                                                <asp:TextBox ID="Or_Der_tbx" runat="server" Width="99%" TextMode="MultiLine" Rows="4" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="Or_Der_btn" runat="server" Text="Lưu thứ tự sắp xếp" Width="135" OnClick="Or_Der_btn_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Space_Shop_tr" runat="server" visible="false">
                                <td style="height: 10px;"></td>
                            </tr>
                            <tr id="Shop_tr" runat="server" visible="false">
                                <td colspan="5" align="center">
                                    <table>
                                        <tr>
                                            <td>Tầng:
                                                <asp:DropDownList ID="Floor_ddl" runat="server" BorderColor="Black" BorderWidth="1"></asp:DropDownList>
                                            </td>
                                            <td>Shop:
                                                <asp:TextBox ID="Shop_tbx" runat="server" Width="200" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="Submit_Shop_btn" runat="server" Text="Tạo mới" OnClick="Submit_Shop_btn_Click" />
                                                <asp:Button ID="Cancel_Shop_btn" runat="server" Text="Hủy" OnClick="Cancel_btn_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Image ID="Picture_Shop_img" runat="server" Visible="false" Style="max-width: 120px; max-height: 120px;" />
                                                <asp:HiddenField ID="Picture_Shop_hdf" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="Picture_Shop_upl" runat="server" />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Space_Shop_Index_tr" runat="server" visible="false">
                                <td style="height: 10px;"></td>
                            </tr>
                            <tr id="Shop_Index_tr" runat="server" visible="false">
                                <td colspan="5" align="center">
                                    <table>
                                        <tr>
                                            <td runat="server" id="Shop_Index_1_td" visible="false">Danh mục:
                                                <asp:DropDownList ID="Shop_Index_1_ddl" runat="server" BorderColor="Black" BorderWidth="1"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Shop_Index_lbl" runat="server"></asp:Label>
                                                <asp:TextBox ID="Shop_Index_tbx" runat="server" Width="200" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="Submit_Shop_Index_btn" runat="server" Text="Tạo mới" OnClick="Submit_Shop_Index_btn_Click" />
                                                <asp:Button ID="Cancel_Shop_Index_btn" runat="server" Text="Hủy" OnClick="Cancel_btn_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Report_tr" runat="server">
                                <td colspan="5" align="center">
                                    <table>
                                        <tr>
                                            <td style="vertical-align: middle;">
                                                <asp:Button ID="Report_btn" runat="server" Text="OK !" OnClick="Report_btn_Click" OnClientClick="Report_btn_OnClientClick();" />
                                            </td>
                                            <td style="vertical-align: middle;">
                                                <asp:ImageButton ID="Download_Excel_All_btn" runat="server" ImageUrl="~/index/Excel.png" OnClientClick="Report_btn_OnClientClick();" OnClick="Download_Excel_All_btn_Click" />
                                            </td>
                                            <td style="vertical-align: middle;" id="Switch_Point_td" runat="server" visible="false">
                                                <asp:Button ID="Switch_Point_btn" runat="server" Text="Chuyển điểm !" OnClientClick="Submit_Switch_Multi_Point_btn_OnClientClick(); return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;"></td>
                </tr>
                <tr>
                    <td id="Dynamic_Control_Holder_td" align="center" style="display: none;">
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
    </form>
</body>
</html>
