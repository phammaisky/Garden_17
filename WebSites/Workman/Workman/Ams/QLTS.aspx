<%@ Page Language="C#" AutoEventWireup="true" Inherits="AmsReport" CodeBehind="QLTS.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QLTS !</title>
</head>
<body id="Page_Body" runat="server">
    <form id="Page_Form" runat="server" method="post" enctype="multipart/form-data">
        <asp:ScriptManager ID="Ajax_ScriptManager" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="Ajax_UpdatePanel" runat="server">
            <ContentTemplate>
                <div id="Page_Content_div" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="height: 20px;"></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Report_lbl" runat="server" Text="Kiểm kê Tài sản" class="Bold_Red_Text_css" Font-Size="XX-Large" Style="margin: 10px;"></asp:Label>
                                <br />
                                <br />
                                <table style="vertical-align: bottom; display: block;">
                                    <tr runat="server" id="Submit_tr">
                                        <td colspan="5" align="center">
                                            <table border="0">
                                                <tr>
                                                    <td colspan="2" align="center" style="vertical-align: middle; padding: 5px;">
                                                        <asp:DropDownList ID="Cong_Ty_ddl" runat="server" Width="150" OnSelectedIndexChanged="Report_btn_Click" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="Phong_Ban_ddl" runat="server" Width="150" OnSelectedIndexChanged="Report_btn_Click" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="Loai_Thiet_Bi_ddl" runat="server" Width="150" OnSelectedIndexChanged="Report_btn_Click" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="CheckedBarcode_ddl" runat="server" Width="150" OnSelectedIndexChanged="Report_btn_Click" AutoPostBack="true">
                                                            <asp:ListItem Selected="True" Text="Tất cả barcode" Value="all"></asp:ListItem>
                                                            <asp:ListItem Text="Tổng hợp số lượng" Value="x"></asp:ListItem>
                                                            <asp:ListItem Text="Có" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Không" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center" style="vertical-align: middle; padding: 5px;">From:
                                                        <asp:TextBox ID="Time_Start_tbx" runat="server" Width="80"></asp:TextBox>
                                                        <asp:CalendarExtender ID="Time_Start_clde" runat="server" TargetControlID="Time_Start_tbx"
                                                            Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                                        </asp:CalendarExtender>
                                                        <asp:MaskedEditExtender ID="Time_Start_mee" runat="server" ClearMaskOnLostFocus="false"
                                                            ClearTextOnInvalid="true" TargetControlID="Time_Start_tbx" Mask="99\/99\/9999"
                                                            MaskType="Date" AutoComplete="true">
                                                        </asp:MaskedEditExtender>
                                                        --- To:
                                                        <asp:TextBox ID="Time_Finish_tbx" runat="server" Width="80"></asp:TextBox>
                                                        <asp:CalendarExtender ID="Time_Finish_clde" runat="server" TargetControlID="Time_Finish_tbx"
                                                            Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                                        </asp:CalendarExtender>
                                                        <asp:MaskedEditExtender ID="Time_Finish_mee" runat="server" ClearMaskOnLostFocus="false"
                                                            ClearTextOnInvalid="true" TargetControlID="Time_Finish_tbx" Mask="99\/99\/9999"
                                                            MaskType="Date" AutoComplete="true">
                                                        </asp:MaskedEditExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center" style="vertical-align: middle; padding: 5px;">
                                                        <asp:CheckBox ID="cbxFilterName" runat="server" Text="Tìm theo Mã TS" />
                                                        <asp:TextBox ID="Filter_ID_List_tbx" runat="server" Width="500" BorderStyle="Solid" BorderWidth="1" BorderColor="Black"></asp:TextBox>
                                                        <asp:Button ID="OK_btn" runat="server" Text="OK !" ForeColor="Red" Font-Bold="true" OnClick="Report_btn_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 135px; padding: 5px;"><span style="font-weight: bold; color: black;">Tải lên Barcode:</span></td>
                                                    <td style="padding: 5px;">
                                                        <table border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:FileUpload ID="Upload_upl" runat="server" Name="PostedFile" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="Upload_btn" runat="server" Text="Upload !" ForeColor="Red" Font-Bold="true" OnClientClick="Upload_File(); return false;" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="middle" style="padding: 5px;"><span style="font-weight: bold; color: black;">Tải xuống Báo cáo:</span></td>
                                                    <td style="padding: 5px;" valign="middle">
                                                        <table>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <asp:ImageButton ID="Download_Excel_All_btn" runat="server" ImageUrl="/Index/IMG/Button/Excel.png" OnClientClick="Report_btn_OnClientClick();" OnClick="Download_Excel_All_btn_Click" /></td>
                                                                <td valign="middle">
                                                                    <asp:Button ID="Print_Barcode_Page_btn" runat="server" OnClick="Print_Barcode_Page_btn_Click" Text="Tạo Barcode trang hiện tại" Font-Bold="true" OnClientClick="Report_btn_OnClientClick();" /></td>
                                                                <td valign="middle">
                                                                    <asp:Button ID="Print_Barcode_All_btn" runat="server" OnClick="Print_Barcode_All_btn_Click" Text="Tạo tất cả Barcode" Font-Bold="true" OnClientClick="Report_btn_OnClientClick();" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="Message_tr">
                                        <td colspan="5" align="center">
                                            <asp:Label ID="Message_lbl" runat="server" ForeColor="Red" Font-Size="X-Large"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
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

                <asp:HiddenField ID="Index_Host_hdf" runat="server" />
                <asp:HiddenField ID="PageMethods_Path_hdf" runat="server" />
                <asp:HiddenField ID="Client_Refresh_hdf" runat="server" />
                <asp:HiddenField ID="Is_Iframe_hdf" runat="server" Value="1" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
