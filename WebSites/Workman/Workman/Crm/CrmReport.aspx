<%@ Page Language="C#" AutoEventWireup="true" Inherits="CrmReport" Codebehind="CrmReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Báo cáo !</title>
</head>
<body id="Page_Body" runat="server">
    <form id="Page_Form" runat="server">
        <asp:ScriptManager ID="Ajax_ScriptManager" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="Ajax_UpdatePanel" runat="server">
            <ContentTemplate>
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
                                    <tr runat="server" id="Space_Filter_Time_2_tr">
                                        <td style="height: 5px;"></td>
                                    </tr>
                                    <tr runat="server" id="Filter_Time_2_tr">
                                        <td align="right" class="Bold_Blue_Text_css">BUT NOT From:</td>
                                        <td>
                                            <asp:TextBox ID="Time_Start_2_tbx" runat="server" Width="80"></asp:TextBox>
                                            <asp:CalendarExtender ID="Time_Start_2_clde" runat="server" TargetControlID="Time_Start_2_tbx"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Time_Start_2_mee" runat="server" ClearMaskOnLostFocus="false"
                                                ClearTextOnInvalid="true" TargetControlID="Time_Start_2_tbx" Mask="99\/99\/9999"
                                                MaskType="Date" AutoComplete="true">
                                            </asp:MaskedEditExtender>
                                        </td>
                                        <td style="width: 10px;"></td>
                                        <td align="right" class="Bold_Blue_Text_css">To:</td>
                                        <td>
                                            <asp:TextBox ID="Time_Finish_2_tbx" runat="server" Width="80"></asp:TextBox>
                                            <asp:CalendarExtender ID="Time_Finish_2_clde" runat="server" TargetControlID="Time_Finish_2_tbx"
                                                Format="dd/MM/yyyy" FirstDayOfWeek="Monday">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Time_Finish_2_mee" runat="server" ClearMaskOnLostFocus="false"
                                                ClearTextOnInvalid="true" TargetControlID="Time_Finish_2_tbx" Mask="99\/99\/9999"
                                                MaskType="Date" AutoComplete="true">
                                            </asp:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="Space_Filter_Time_3_tr" visible="false">
                                        <td style="height: 5px;"></td>
                                    </tr>
                                    <tr runat="server" id="Filter_Time_3_tr" visible="false">
                                        <td colspan="5" align="center">
                                            <asp:Label ID="Label1" runat="server" Font-Bold="true" ForeColor="Red" Text="Khoảng thời gian, ví dụ: 10-14, 14-18, 18-22, 10-22"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="Filter_Time_3_tbx" runat="server" Width="500" Style="text-align: center" Text="10-14, 14-18, 18-22, 10-22"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="Space_Filter_Name_tr">
                                        <td style="height: 5px;"></td>
                                    </tr>
                                    <tr runat="server" id="Filter_Name_tr">
                                        <td colspan="5" align="center">
                                            <table>
                                                <tr>
                                                    <td runat="server" id="Shop_td" align="right" class="Bold_Blue_Text_css">Shop:</td>
                                                    <td>
                                                        <asp:TextBox ID="Shop_tbx" runat="server" Width="160"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10px;"></td>
                                                    <td align="right" class="Bold_Blue_Text_css">Card:</td>
                                                    <td>
                                                        <asp:TextBox ID="Card_tbx" runat="server" Width="160" CssClass="Red_Text_css"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="Card_tbxe" runat="server" TargetControlID="Card_tbx"
                                                            FilterType="Numbers">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:CheckBox ID="ReActive_Card_cbx" runat="server" Visible="false" Text="Kích hoạt" />
                                                        <br />
                                                        <asp:Button ID="FC_Block_btn" runat="server" Text="Khóa thẻ" ForeColor="Red" Visible="false" OnClick="FC_Block_btn_Click" />
                                                        <asp:Button ID="FC_UnBlock_btn" runat="server" Text="Mở lại" ForeColor="Red" Visible="false" OnClick="FC_UnBlock_btn_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td runat="server" id="Name_td" align="right" class="Bold_Blue_Text_css">Name:</td>
                                                    <td>
                                                        <asp:TextBox ID="Name_tbx" runat="server" Width="160"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10px;"></td>
                                                    <td runat="server" id="Phone_td" align="right" class="Bold_Blue_Text_css">Phone:</td>
                                                    <td>
                                                        <asp:TextBox ID="Phone_tbx" runat="server" Width="160"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td runat="server" id="Email_td" align="right" class="Bold_Blue_Text_css">Email:</td>
                                                    <td>
                                                        <asp:TextBox ID="Email_tbx" runat="server" Width="160"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10px;"></td>
                                                    <td runat="server" id="Address_td" align="right" class="Bold_Blue_Text_css">Address:</td>
                                                    <td>
                                                        <asp:TextBox ID="Address_tbx" runat="server" Width="160"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="Card_Actived_OR_Disabled_tr">
                                                    <td colspan="5" align="center">
                                                        <asp:RadioButtonList ID="Card_Actived_OR_Disabled_rdol" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Chỉ thẻ còn hoạt động" Value="1" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Chỉ thẻ đã khóa" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="Space_Card_FC_Infor_tr">
                                        <td style="height: 5px;"></td>
                                    </tr>
                                    <tr runat="server" id="Card_FC_Infor_tr">
                                        <td colspan="5" align="center">
                                            <asp:Label ID="Money_Current_lbl" runat="server"></asp:Label>
                                            <asp:Label ID="Money_Useable_lbl" runat="server"></asp:Label>
                                            <asp:Label ID="Money_Withdrawal_Able_lbl" runat="server"></asp:Label>
                                            <asp:Label ID="State_lbl" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="Space_SAP_tr">
                                        <td style="height: 5px;"></td>
                                    </tr>
                                    <tr runat="server" id="SAP_tr">
                                        <td colspan="5" align="center">
                                            <span class="Bold_Blue_Text_css">Mã SAP:</span>
                                            <asp:TextBox ID="Ma_SAP_tbx" runat="server" Width="160"></asp:TextBox>
                                            <br />
                                            <br />
                                            <span class="Bold_Blue_Text_css">Mã Hợp đồng:</span>
                                            <asp:TextBox ID="So_Hop_Dong_tbx" runat="server" Width="160"></asp:TextBox>
                                            <br />
                                            <asp:Button ID="SAP_btn" runat="server" Text="OK" ForeColor="Red" Style="margin-left: 5px;" OnClick="SAP_btn_Click" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="Space_Filter_RDOL_1_tr">
                                        <td style="height: 5px;"></td>
                                    </tr>
                                    <tr runat="server" id="Filter_RDOL_1_tr">
                                        <td colspan="2" align="center">                                            
                                            <asp:RadioButtonList ID="Filter_RDOL_1" runat="server">
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td colspan="2" align="center">
                                            <span class="Bold_Blue_Text_css">Người tích điểm:</span><br />
                                            <asp:TextBox ID="Add_By_User_tbx" runat="server" Width="120"></asp:TextBox><br />
                                            <asp:RadioButtonList ID="Filter_RDOL_2" runat="server">
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="Space_Filter_CBXL_1_tr">
                                        <td style="height: 5px;"></td>
                                    </tr>
                                    <tr runat="server" id="Filter_CBXL_1_tr">
                                        <td colspan="5" align="center">
                                            <asp:CheckBoxList ID="Filter_CBXL_1" runat="server"></asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="Submit_tr">
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
                                    <tr runat="server" id="Message_tr">
                                        <td colspan="5" align="center">
                                            <br />
                                            <asp:Label ID="Message_lbl" runat="server" ForeColor="Red" Font-Size="X-Large"></asp:Label>
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
                <asp:HiddenField ID="Is_POS_hdf" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
