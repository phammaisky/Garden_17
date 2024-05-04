<%@ page language="C#" autoeventwireup="true" inherits="Upload, App_Web_1njdqebw" validaterequest="false" enableviewstatemac="false" enableEventValidation="false" smartnavigation="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload !</title>
    <link href="/CSS/ALL.css" type="text/css" rel="stylesheet" />
</head>
<body runat="server" id="Page_Body">
    <form id="form1" runat="server">
        <div>
            <table border="0" width="100%">
                <tr>
                    <td colspan="2"><span class="Red">Chọn Hình ảnh bất kỳ:</span><br />
                        <asp:FileUpload ID="Piture_Upl" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><span class="Red">Max Size (Mặc định: 700x700. Cỡ nhỏ: 120x120. Hoặc tùy ý...):</span><br />
                        <asp:TextBox ID="Width_tbx" runat="server" Text="700" Width="30"></asp:TextBox>
                        X
                        <asp:TextBox ID="Height_tbx" runat="server" Text="700" Width="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="OK_btn" runat="server" Text="Upload" OnClick="OK_btn_Click" CssClass="Red B S14" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" style="height: 480px;"><span class="Red">Rồi Copy vào Bài viết:</span><br />
                        <asp:Image ID="Picture_img" runat="server" Style="max-width: 96%; max-height: 96%;" Visible="false" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
