<%@ page language="C#" autoeventwireup="true" inherits="Admin, App_Web_1njdqebw" enableviewstatemac="false" enableEventValidation="false" smartnavigation="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin !</title>
    <link href="/CSS/ALL.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        html, body {
            overflow: hidden !important;
        }

        a {
            cursor: pointer;
            text-decoration: none !important;
        }
    </style>
</head>
<body runat="server" id="Page_Body">
    <form id="form1" runat="server">
        <table id="Page_Content_tbl" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr id="Menu_tr">
                <td align="center" style="background-color: #4169E1;">
                    <div runat="server" id="Menu_div" style="background-color: #4169E1; color: white !important; font-weight: bold;">
                        <a href="#" onclick="return Menu_Admin_On_Click('./Post_News.aspx');"><span class="White">Đăng bài</span></a>
                        --- <a href="#" onclick="return Menu_Admin_On_Click('./Manager_News.aspx');"><span class="White">Bài đã đăng</span></a>
                        --- <a href="/GD/Report.aspx?R=Banner" target="_blank"><span class="White">Banner</span></a>
                        --- <a href="#" onclick="return Menu_Admin_On_Click('./Report.aspx?R=Shop');"><span class="White">Shop</span></a>
                        --- <a href="#" onclick="return Menu_Admin_On_Click('./Report.aspx?R=Shop_Index_1');"><span class="White">Danh mục</span></a>
                        --- <a href="#" onclick="return Menu_Admin_On_Click('./Report.aspx?R=Shop_Index_2');"><span class="White">Danh mục con</span></a>
                        --- <a href="#" onclick="return Menu_Admin_On_Click('./Report.aspx?R=Reg_Card');"><span class="White">Báo cáo Đăng ký Thẻ</span></a>
                        --- <a href="#" onclick="return Menu_Admin_On_Click('./Report.aspx?R=News_Letter');"><span class="White">Đăng ký Thành viên</span></a>
                    </div>
                </td>
            </tr>
            <tr id="Page_Content_tr">
                <td>
                    <iframe id='Page_Content_ifr' width='100%' frameborder='0' marginwidth='0' marginheight='0'
                        scrolling="auto"></iframe>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
