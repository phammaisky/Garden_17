using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Drawing;
using System.Data.SqlClient;
using System.Security.AccessControl;
using AjaxControlToolkit;
using _4u4m;
using System.Web.Services;
using System.Collections.Generic;

using HtmlAgilityPack;
using System.Runtime.InteropServices;
using OfficeOpenXml;
using System.Net;
using System.Text;

public partial class Trinh_Ky_Duyet : System.Web.UI.Page
{
    SqlConnection Sql_Connection;
    SqlCommand Sql_Command;

    //
    string Message = string.Empty;
    string Time = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss:fff");

    string On_Page_Load = string.Empty;

    string Menu_List = string.Empty;

    //OK+OK
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!new _4e().Check_PageMethods_Is_undefined())
        {
            Sql_Connection = new SqlConnection(Application["Sql_Connection_String"].ToString());

            if (Sql_Connection.State != ConnectionState.Open)
            {
                Sql_Connection.Open();
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //Download_Chu_Ky();
        //Chuyen_Doi_Excel_Trinh_Ky_Duyet();

        Creat_Code();
    }

    //OK+OK
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!new _4e().Check_PageMethods_Is_undefined())
        {
            new _4e().Add_ALL_JavaScript_AND_CSS_File_To_Header(Index_Host_hdf.Value);
            //new _4e().Add_CSS_File_To_Header(Index_Host_hdf.Value + "/index/CSS/4u4m4e_Home.css?" + Guid.NewGuid().ToString());
            new _4e().Add_ALL_Index_Host_To_IMG(Index_Host_hdf.Value);

            //
            if (!IsPostBack)
            {
                Page_Body.Attributes.Add("onload", On_Page_Load);
            }
            else
            {
                new _4e().Run_JavaScript(On_Page_Load);
            }
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        if (!new _4e().Check_PageMethods_Is_undefined())
        {
            if (Sql_Connection != null)
            {
                Sql_Connection.Close(); Sql_Connection.Dispose();
            }
        }
    }

    //OK+OK
    protected void Chuyen_Doi_Excel_Trinh_Ky_Duyet()
    {
        //
        FileInfo File_Info = new FileInfo(@"D:\Websites\Garden\File_Upload\User-Trinh-ky-Duyet.xlsx");
        ExcelPackage Excel_Package = new ExcelPackage(File_Info);

        //
        var Work_sheets = Excel_Package.Workbook.Worksheets[1];

        //Read Data
        for (int Row = 6; Row <= Work_sheets.Dimension.End.Row; Row++)
        {
            //Mã phòng ban	Phòng ban	Họ và tên	Tài khoản	Email

            string Ma_Phong_Ban = new _4e().Object_ToString(Work_sheets.Cells[Row, 1].Value);
            string Phong_Ban = new _4e().Object_ToString(Work_sheets.Cells[Row, 2].Value);
            string Ho_Va_Ten = new _4e().Object_ToString(Work_sheets.Cells[Row, 3].Value);
            string UserName_AND_Domain = new _4e().Object_ToString(Work_sheets.Cells[Row, 4].Value);
            string Email = new _4e().Object_ToString(Work_sheets.Cells[Row, 5].Value);

            //
            Ma_Phong_Ban = new _4e().Remove_Space_String(Ma_Phong_Ban);
            Phong_Ban = new _4e().Remove_Space_String(Phong_Ban);
            Ho_Va_Ten = new _4e().Remove_Space_String(Ho_Va_Ten);
            UserName_AND_Domain = new _4e().Remove_Space_String(UserName_AND_Domain);
            Email = new _4e().Remove_Space_String(Email);

            //
            if (UserName_AND_Domain != string.Empty)
            {
                string Domain = "Garden";
                string UserName = UserName_AND_Domain;

                string[] UserName_AND_Domain_Array = UserName_AND_Domain.Split('\\');

                if (UserName_AND_Domain_Array.Length == 2)
                {
                    Domain = UserName_AND_Domain_Array[0];
                    UserName = UserName_AND_Domain_Array[1];
                }

                string Picture = @"D:\Websites\Garden\File_Upload\Chu_Ky\" + Domain + "\\" + UserName + ".jpg";

                if (!File.Exists(Picture))
                {
                    Response.Write("Chưa có chữ ký: " + Phong_Ban + " : " + UserName + " --- " + Picture + "<br/>");
                }

                //
                string Sql_Query =
                        "INSERT INTO Test.DBO.UserInfo"
                        + " (DeptId, UserName, FullName, Email, ImageSignature)"
                        + " VALUES"
                        + " (@DeptId, @UserName, @FullName, @Email, @ImageSignature)"
                        ;

                Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                Sql_Command.Parameters.Add("@DeptId", Ma_Phong_Ban);
                Sql_Command.Parameters.Add("@UserName", Domain + "\\" + UserName);
                Sql_Command.Parameters.Add("@FullName", Ho_Va_Ten);
                Sql_Command.Parameters.Add("@Email", Email);

                if (!File.Exists(Picture))
                {
                    Sql_Command.Parameters.Add("@ImageSignature", new byte[] { });
                }
                else
                {
                    Sql_Command.Parameters.Add("@ImageSignature", File.ReadAllBytes(Picture));
                }

                Sql_Command.ExecuteNonQuery();
            }
        }

        //Save
        Excel_Package.Save();
    }

    protected void Download_Chu_Ky()
    {
        for (int i1 = 1; i1 <= 500; i1++)
        {
            try
            {
                string Template_Directory_Path = @"D:\Websites\Garden\Temp";
                string Template_URL = "http://10.1.3.6:88/sites/home/ProposalSignature/Forms/DispForm.aspx?ID=" + i1.ToString();

                //
                using (WebClient Web_Client = new WebClient())
                {
                    new _4e().Creat_Directory(Template_Directory_Path);

                    Web_Client.UseDefaultCredentials = true;
                    Web_Client.Credentials = new NetworkCredential(@"Garden\QuyenNv", "HAICONTHANLANCON");

                    Web_Client.DownloadFile(Template_URL, Template_Directory_Path + "\\" + i1.ToString() + ".html");
                }

                //id=webImgShrinked (IMG)
                //id=SPFieldUser (td)

                //
                string index_html = File.ReadAllText(Template_Directory_Path + "\\" + i1.ToString() + ".html", Encoding.UTF8);

                //
                HtmlAgilityPack.HtmlDocument Html_Document = new HtmlAgilityPack.HtmlDocument();
                Html_Document.LoadHtml(index_html);

                //
                string UserName_AND_Domain = string.Empty;

                //td
                var td_Array = Html_Document.DocumentNode.SelectNodes(@"//td[@id]");

                if (td_Array != null)
                {
                    foreach (HtmlNode td_one in td_Array)
                    {
                        if (td_one.Attributes["id"].Value == "SPFieldUser")
                        {
                            UserName_AND_Domain = td_one.InnerText.ToLower();
                        }
                    }
                }

                UserName_AND_Domain = new _4e().Remove_From_String_To(UserName_AND_Domain, "<!--", "-->");

                UserName_AND_Domain = UserName_AND_Domain.Replace(Environment.NewLine, string.Empty);
                UserName_AND_Domain = new _4e().Remove_Space_String(UserName_AND_Domain);

                string Domain = string.Empty;
                string UserName = string.Empty;

                string[] UserName_AND_Domain_Array = UserName_AND_Domain.Split('\\');

                if (UserName_AND_Domain_Array.Length == 2)
                {
                    Domain = UserName_AND_Domain_Array[0];
                    UserName = UserName_AND_Domain_Array[1];

                    //IMG
                    var Img_Array = Html_Document.DocumentNode.SelectNodes(@"//img[@id]");

                    if (Img_Array != null)
                    {
                        foreach (HtmlNode Img_one in Img_Array)
                        {
                            if (Img_one.Attributes["id"].Value == "webImgShrinked")
                            {
                                string Img_one_URL = Img_one.Attributes["src"].Value.ToLower();

                                string File_Extension = Path.GetExtension(Img_one_URL).ToLower();
                                string File_Name = Path.GetFileName(Img_one_URL);

                                try
                                {
                                    using (WebClient Web_Client = new WebClient())
                                    {
                                        new _4e().Creat_Directory(Template_Directory_Path + "\\" + Domain + "\\");

                                        Web_Client.UseDefaultCredentials = true;
                                        Web_Client.Credentials = new NetworkCredential(@"Garden\QuyenNv", "HAICONTHANLANCON");

                                        Web_Client.DownloadFile(Img_one_URL, Template_Directory_Path + "\\" + Domain + "\\" + UserName + File_Extension);
                                    }
                                }
                                catch (Exception EX)
                                {
                                    new _4e().Write_To_File_Temp("\\EX.txt", EX.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception EX)
            {
                new _4e().Write_To_File_Temp("\\EX.txt", EX.ToString());
            }
        }
    }

    protected void Creat_Code()
    {
        for (int Year_int = 2026; Year_int <= 2027; Year_int++)
        {
            for (int Month_int = 1; Month_int <= 12; Month_int++)
            {
                //
                string Data_Table_Hoa_Don = "HoaDon";

                //
                if (Month_int < 10)
                {
                    Data_Table_Hoa_Don += "0" + Month_int.ToString();
                }
                else
                {
                    Data_Table_Hoa_Don += Month_int.ToString();
                }

                Data_Table_Hoa_Don += Year_int.ToString();

                //
                new _4e().Write_To_File_Temp("\\SQL.sql", "exec sp_addmergearticle @publication = N'TOPOS_DB', @article = N'" + Data_Table_Hoa_Don + "', @source_owner = N'dbo', @source_object = N'" + Data_Table_Hoa_Don + "', @type = N'table', @description = null, @creation_script = null, @pre_creation_cmd = N'drop', @schema_option = 0x000000010C034FD1, @identityrangemanagementoption = N'manual', @destination_owner = N'dbo', @force_reinit_subscription = 1, @column_tracking = N'false', @subset_filterclause = null, @vertical_partition = N'false', @verify_resolver_signature = 1, @allow_interactive_resolver = N'false', @fast_multicol_updateproc = N'true', @check_permissions = 0, @subscriber_upload_options = 0, @delete_tracking = N'true', @compensate_for_errors = N'false', @stream_blob_columns = N'false', @partition_options = 0");
                new _4e().Write_To_File_Temp("\\SQL.sql", "exec sp_addmergearticle @publication = N'TOPOS_DB', @article = N'ThanhToan" + Data_Table_Hoa_Don + "', @source_owner = N'dbo', @source_object = N'ThanhToan" + Data_Table_Hoa_Don + "', @type = N'table', @description = null, @creation_script = null, @pre_creation_cmd = N'drop', @schema_option = 0x000000010C034FD1, @identityrangemanagementoption = N'manual', @destination_owner = N'dbo', @force_reinit_subscription = 1, @column_tracking = N'false', @subset_filterclause = null, @vertical_partition = N'false', @verify_resolver_signature = 1, @allow_interactive_resolver = N'false', @fast_multicol_updateproc = N'true', @check_permissions = 0, @subscriber_upload_options = 0, @delete_tracking = N'true', @compensate_for_errors = N'false', @stream_blob_columns = N'false', @partition_options = 0");
                new _4e().Write_To_File_Temp("\\SQL.sql", "exec sp_addmergearticle @publication = N'TOPOS_DB', @article = N'CT" + Data_Table_Hoa_Don + "', @source_owner = N'dbo', @source_object = N'CT" + Data_Table_Hoa_Don + "', @type = N'table', @description = null, @creation_script = null, @pre_creation_cmd = N'drop', @schema_option = 0x000000010C034FD1, @identityrangemanagementoption = N'manual', @destination_owner = N'dbo', @force_reinit_subscription = 1, @column_tracking = N'false', @subset_filterclause = null, @vertical_partition = N'false', @verify_resolver_signature = 1, @allow_interactive_resolver = N'false', @fast_multicol_updateproc = N'true', @check_permissions = 0, @subscriber_upload_options = 0, @delete_tracking = N'true', @compensate_for_errors = N'false', @stream_blob_columns = N'false', @partition_options = 0");

                new _4e().Write_To_File_Temp("\\SQL.sql", Environment.NewLine);
            }
        }
    }
}