<%@ Page Language="C#" AutoEventWireup="true" Inherits="Add_Point" Codebehind="Add_Point.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tích điểm !</title>
    <style type="text/css">
        html, body {
            overflow: hidden !important;
        }

        .Shop_List li {
            list-style-type: none;
            font-size: 12pt;
            margin: 5px;
        }
    </style>
</head>
<body id="Page_Body" runat="server">
    <div id="Page_Content_div">
        <form id="Page_Form" runat="server">
            <asp:ScriptManager ID="Ajax_ScriptManager" runat="server" EnablePageMethods="true">
            </asp:ScriptManager>
            <table id="Content_tbl" runat="server" width="100%">
                <tr>
                    <td colspan="99" align="center" style="height: 10px"></td>
                </tr>
                <tr>
                    <td width="20px"></td>
                    <td align="center" valign="top">
                        <table border="1" width="350px" style="border-collapse: collapse">
                            <tr>
                                <td colspan="2" align="center">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="POS_lbl" runat="server" Text="Tên Quầy" CssClass="Bold_Blue_Text_css"></asp:Label>
                                            </td>
                                            <td width="20px"></td>
                                            <td>
                                                <asp:Label ID="Cashier_lbl" runat="server" Text="Thu Ngân" CssClass="Bold_Blue_Text_css"></asp:Label>
                                            </td>
                                            <td width="20px"></td>
                                            <td>
                                                <asp:Button ID="Submit_btn" runat="server" UseSubmitBehavior="false" Text="OK !" OnClientClick="Submit_Add_Point_btn_OnClientClick(); return false;" Font-Size="12pt" CssClass="Bold_Red_Text_css" />
                                            </td>
                                            <td width="20px"></td>
                                            <td>
                                                <asp:Button ID="Clear_btn" runat="server" UseSubmitBehavior="false" Text="Xóa trắng..." OnClientClick="Clear_btn_OnClientClick(); return false;" Font-Size="12pt" CssClass="Bold_Red_Text_css" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="80px"><span class="Bold_Blue_Text_css">Công việc:</span></td>
                                <td align="left" style="text-align: left;">
                                    <asp:RadioButtonList ID="Reason_rdol" runat="server" onclick="Reason_rdol_OnClientClick();">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="80px"><span class="Bold_Blue_Text_css">Số thẻ:</span></td>
                                <td>
                                    <asp:TextBox ID="Card_tbx" runat="server" Width="95%" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Red_Text_css" BorderColor="Transparent" BorderWidth="1" BorderStyle="Solid">0101</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="Card_tbxe" runat="server" TargetControlID="Card_tbx"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr id="POS_tr">
                                <td align="right" width="80px"><span class="Bold_Blue_Text_css">Quầy:</span></td>
                                <td>
                                    <span class="Bold_Blue_Text_css" style="font-size: 12pt; margin-left: 5px;">AA</span><asp:TextBox ID="POS_tbx" runat="server" Width="80%" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Red_Text_css" BorderColor="Transparent" BorderWidth="1" BorderStyle="Solid"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="POS_tbxe" runat="server" TargetControlID="POS_tbx"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr id="Buy_Time_tr">
                                <td align="right" width="80px"><span class="Bold_Blue_Text_css">Ngày mua:</span></td>
                                <td>
                                    <asp:TextBox ID="Buy_Time_Day_tbx" runat="server" Width="40" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Red_Text_css" MaxLength="2" BorderColor="blue" BorderWidth="1" BorderStyle="Solid"></asp:TextBox>
                                    <span class="Bold_Blue_Text_css" style="font-size: 12pt;">- </span>
                                    <asp:TextBox ID="Buy_Time_Month_tbx" runat="server" Width="40" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Red_Text_css" MaxLength="2" BorderColor="blue" BorderWidth="1" BorderStyle="Solid"></asp:TextBox>
                                    <span class="Bold_Blue_Text_css" style="font-size: 12pt;">- 20</span><asp:TextBox ID="Buy_Time_Year_tbx" runat="server" Width="40" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Red_Text_css" MaxLength="4" BorderColor="blue" BorderWidth="1" BorderStyle="Solid">19</asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="Buy_Time_Day_tbxe" runat="server" TargetControlID="Buy_Time_Day_tbx"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:FilteredTextBoxExtender ID="Buy_Time_Month_tbxe" runat="server" TargetControlID="Buy_Time_Month_tbx"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:FilteredTextBoxExtender ID="Buy_Time_Year_tbxe" runat="server" TargetControlID="Buy_Time_Year_tbx"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr id="Receipt_tr">
                                <td align="right" width="80px"><span class="Bold_Blue_Text_css">Hóa đơn:</span></td>
                                <td>
                                    <asp:TextBox ID="Receipt_tbx" runat="server" Width="95%" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Red_Text_css" BorderColor="Transparent" BorderWidth="1" BorderStyle="Solid"></asp:TextBox>
                                    <%--<asp:FilteredTextBoxExtender ID="Receipt_tbxe" runat="server" TargetControlID="Receipt_tbx"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 10px;"></td>
                            </tr>
                            <tr id="Money_tr">
                                <td align="right" width="80px"><span class="Bold_Blue_Text_css">Số tiền:</span></td>
                                <td>
                                    <asp:TextBox ID="Money_tbx" runat="server" Width="95%" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Red_Text_css" BorderColor="Transparent" BorderWidth="1" BorderStyle="Solid"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="Money_tbxe" runat="server" TargetControlID="Money_tbx"
                                        FilterType="Custom, Numbers" ValidChars=".,">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr id="Point_tr">
                                <td align="right" width="80px"><span class="Bold_Blue_Text_css">Điểm:</span></td>
                                <td valign="middle">
                                    <asp:TextBox ID="Point_tbx" runat="server" Width="30%" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Red_Text_css" BorderColor="Transparent" BorderWidth="1" BorderStyle="Solid"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="Point_tbxe" runat="server" TargetControlID="Point_tbx"
                                        FilterType="Custom, Numbers" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <span class="Bold_Red_Text_css">Điểm hiện có:</span>
                                    <asp:Label ID="Current_Point_lbl" runat="server" Font-Size="12pt" CssClass="Red_Text_css"></asp:Label>
                                </td>
                            </tr>
                            <tr id="Reason_tr" style="display: none;">
                                <td align="right" width="80px"><span class="Bold_Blue_Text_css">Ghi chú:</span></td>
                                <td valign="middle">
                                    <asp:TextBox ID="Reason_tbx" runat="server" Width="95%" Height="20px" Style="margin: 5px;" Font-Size="12pt" CssClass="Red_Text_css" BorderColor="Transparent" BorderWidth="1" BorderStyle="Solid"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="Space_Member_Info_tr" style="display: none;">
                                <td colspan="2" style="height: 10px;"></td>
                            </tr>
                            <tr id="Member_Info_tr" style="display: none;">
                                <td colspan="2" align="left">
                                    <div id="Member_Info_div" class="Blue_Text_css" style="margin: 5px;"></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="20px"></td>
                    <td align="center" valign="top">
                        <table id="Shop_tbl" border="1" width="350px" style="overflow: hidden; border-collapse: collapse">
                            <tr id="Shop_tr">
                                <td align="right">
                                    <span class="Bold_Red_Text_css" style="font-size: 12pt;">Shop:</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="Shop_tbx" runat="server" Width="95%" Height="20px" Style="margin: 5px; text-align: center;" Font-Size="12pt" CssClass="Red_Text_css" BorderColor="Transparent" BorderWidth="1" BorderStyle="Solid"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="Shop_MaThue_tr">
                                <td colspan="2" align="center">
                                    <asp:Label ID="Shop_MaThue_lbl" runat="server" CssClass="Blue_Text_css"></asp:Label>&nbsp;<asp:Label ID="Enable_Add_Point_lbl" runat="server" CssClass="Blue_Text_css"></asp:Label>
                                </td>
                            </tr>
                            <tr id="Search_Name_tr">
                                <td align="right">
                                    <span class="Bold_Red_Text_css" style="font-size: 12pt;">Khách:</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="Search_Name_tbx" runat="server" Width="95%" Height="20px" Style="margin: 5px; text-align: center;" Font-Size="12pt" CssClass="Red_Text_css" BorderColor="Transparent" BorderWidth="1" BorderStyle="Solid"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top" style="overflow: hidden;">
                                    <table id="Shop_List_tbl" width="350px" style="overflow: hidden;">
                                        <tr id="Loading_tr" style="display: none;">
                                            <td valign="top">
                                                <div id="Loading_div" style="display: block; z-index: 9; text-align: center;">
                                                    <asp:Image ID="Loading_img" runat="server" ImageUrl="/index/Loading/Loading.gif" />
                                                    <br />
                                                    <span class="Bold_Red_Text_css" style="font-size: 12pt">Đang Tích Điểm...<br />
                                                        Đợi Chút Xíu...</span>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="Add_Point_Result_tr" style="display: none;">
                                            <td valign="top">
                                                <div id="Add_Point_Result_div" class="Red_Text_css">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="Shop_List_tr" style="overflow: hidden;">
                                            <td valign="top" style="overflow: hidden;">
                                                <div style="overflow: hidden;">
                                                    <ul id="Shop_ul_1" class="Shop_List">
                                                    </ul>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="20px"></td>
                </tr>
                <tr>
                    <td colspan="99" align="center" style="height: 10px"></td>
                </tr>
            </table>
            <asp:HiddenField ID="Loggedin_UserId_hdf" runat="server" />
            <asp:HiddenField ID="Index_Host_hdf" runat="server" />
            <asp:HiddenField ID="PageMethods_Path_hdf" runat="server" />
            <asp:HiddenField ID="Client_Refresh_hdf" runat="server" />
            <asp:HiddenField ID="Today_Year_hdf" runat="server" />
            <asp:HiddenField ID="Today_Month_hdf" runat="server" />
            <asp:HiddenField ID="Today_Day_hdf" runat="server" />
            <asp:HiddenField ID="Enable_Creat_Shop_List_hdf" runat="server" Value="1" />

            <asp:HiddenField ID="Is_POS_hdf" runat="server" />
        </form>
    </div>
</body>
</html>
