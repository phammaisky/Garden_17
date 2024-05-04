<%@ page language="C#" autoeventwireup="true" inherits="Post_News, App_Web_1njdqebw" validaterequest="false" enableviewstatemac="false" enableEventValidation="false" smartnavigation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đăng tin bài !</title>
    <link href="/CSS/ALL.css" type="text/css" rel="stylesheet" />
</head>
<body runat="server" id="Page_Body">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Ajax_ScriptManager" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div>
            <table border="0" width="100%">
                <tr>
                    <td align="center" valign="top" style="width: 600px;">
                        <table border="0" cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td>
                                    <asp:Button ID="OK_btn" runat="server" Text="Lưu bài viết" OnClick="OK_btn_Click" CssClass="Red B S14" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><span class="Red">Danh mục:</span><br />
                                    <asp:DropDownList ID="Index_1_ddl" runat="server" Width="250" BorderColor="Black" BorderWidth="1" AutoPostBack="true" OnSelectedIndexChanged="Index_1_ddl_SelectedIndexChanged"></asp:DropDownList></td>
                                <td>
                                    <asp:Label ID="Index_2_lbl" runat="server" CssClass="Red" Text="Danh mục con:<br/>"></asp:Label>
                                    <asp:DropDownList ID="Index_2_ddl" runat="server" Width="250" BorderColor="Black" BorderWidth="1" AutoPostBack="false"></asp:DropDownList></td>
                            </tr>

                            <tr>
                                <td colspan="2"><span class="Red">Tiêu đề:</span><br />
                                    <asp:TextBox ID="Title_tbx" runat="server" Width="702" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><span class="Red">Tóm tắt:</span><br />
                                    <asp:TextBox ID="Summary_tbx" runat="server" Width="702" BorderColor="Black" BorderWidth="1" TextMode="multiline" Rows="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><span class="Red">Nội dung:</span><br />
                                    <asp:TextBox ID="Contents_tbx" runat="server" Width="702" BorderColor="Black" BorderWidth="1" TextMode="multiline" Rows="50"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <table border="0" width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="Download_Picture_cbx" runat="server" Text="Tự động tải ảnh từ nguồn khác" Checked="true" />
                                </td>
                            </tr>
                            <tr runat="server" id="Tin_Hot_tr">
                                <td colspan="2">
                                    <asp:CheckBox ID="Tin_Hot_cbx" runat="server" Text="Tin hot" />
                                    <asp:CheckBox ID="Tin_Noi_Bat_cbx" runat="server" Text="Tin nổi bật" />
                                    <asp:CheckBox ID="Tin_Khuyen_Mai_cbx" runat="server" Text="Tin khuyến mại" />
                                </td>
                            </tr>
                            <tr runat="server" id="Enable_Request_Gift_tr">
                                <td colspan="2">
                                    <asp:CheckBox ID="Enable_Request_Gift_cbx" runat="server" Text="Cho phép Đăng ký nhận quà" Checked="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="Display_cbx" runat="server" Text="Hiển thị ngoài trang chủ" Checked="true" Visible="false" />
                                    Thời hạn tin:
                                    <asp:TextBox ID="End_Time_tbx" runat="server" CssClass="contact-txt" Style="width: 80px"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="End_Time_mee" runat="server" ClearMaskOnLostFocus="false"
                                        ClearTextOnInvalid="true" TargetControlID="End_Time_tbx" Mask="99\/99\/9999"
                                        MaskType="Date" AutoComplete="true">
                                    </asp:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                </td>
                            </tr>
                            <tr runat="server" id="Picture_thumb_tr">
                                <td>
                                    <span class="Red">Hình ảnh Thu nhỏ:</span><br />
                                    <asp:FileUpload ID="Picture_thumb_Upl" runat="server" />
                                </td>
                                <td>
                                    <asp:Image ID="Picture_thumb_img" runat="server" Visible="false" Style="max-width: 120px; max-height: 120px;" />
                                    <asp:HiddenField ID="Picture_thumb_hdf" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="Red">Hình ảnh Phóng to (700 x 425):</span><br />
                                    <asp:FileUpload ID="Picture_Upl" runat="server" />
                                </td>
                                <td>
                                    <asp:Image ID="Picture_img" runat="server" Visible="false" Style="max-width: 120px; max-height: 120px;" />
                                    <asp:HiddenField ID="Picture_hdf" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                </td>
                            </tr>
                            <tr runat="server" id="Shop_Index_tr">
                                <td colspan="2">
                                    <asp:TreeView ID="Shop_Index_trv" runat="server" ShowCheckBoxes="All"></asp:TreeView>
                                </td>
                            </tr>
                            <tr runat="server" id="Source_tr">
                                <td colspan="2"><span class="Red">Nguồn tin:</span><br />
                                    <asp:TextBox ID="Source_tbx" runat="server" Width="100%" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><span class="Red">Tiêu đề trang:</span><br />
                                    <asp:TextBox ID="Page_Title_tbx" runat="server" Width="100%" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><span class="Red">Description:</span><br />
                                    <asp:TextBox ID="Description_tbx" runat="server" Width="100%" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><span class="Red">Keyword:</span><br />
                                    <asp:TextBox ID="Keyword_tbx" runat="server" Width="100%" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><span class="Red">Tags:</span><br />
                                    <asp:TextBox ID="Tags_tbx" runat="server" Width="100%" BorderColor="Black" BorderWidth="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <iframe src="./Upload.aspx" width="100%" height="600px" frameborder="0"></iframe>
                        
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
