using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OfficeOpenXml;

namespace _4u4m
{
    public class _4e : System.Web.UI.Page
    {
        SqlConnection Sql_Connection;
        SqlCommand Sql_Command;

        public void Get_Sql_Connection_DB()
        {
            Sql_Connection = new SqlConnection(HttpContext.Current.Application["Sql_Connection_String_DB"].ToString());

            if (Sql_Connection.State != ConnectionState.Open)
            {
                Sql_Connection.Open();
            }
        }

        public string Read_DataBase_Source()
        {
            string DataBase_Source = string.Empty;

            if (HttpContext.Current.Request.IsLocal)
            {
                DataBase_Source = "127.0.0.1";
            }
            else
            {
                DataBase_Source = "127.0.0.1";
            }

            DataBase_Source = "10.15.40.17";

            return DataBase_Source;
        }

        string Message = string.Empty;
        string Time = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss:fff");

        public void Set_Index_Host(HiddenField Index_Host_hdf, HiddenField Upload_Host_hdf)
        {
            Index_Host_hdf.Value = Add_http_To_URL(Read_Domain(string.Empty));

            if (Upload_Host_hdf != null)
            {
                Upload_Host_hdf.Value = Add_http_To_URL(Read_Domain(string.Empty)) + "/upload.ashx";
            }
        }

        //
        public string Add_Alert_Message_To_On_Page_Load(string String_Input)
        {
            return " parent.Alert_Message('" + String_Input + "');";
        }

        //
        public void Run_JavaScript(string JavaScript)
        {
            System.Web.UI.Page Current_Page = (System.Web.UI.Page)HttpContext.Current.Handler;

            ScriptManager.RegisterClientScriptBlock(Current_Page, Current_Page.GetType(), new Random().Next(999).ToString(), JavaScript, true);
        }

        //
        public void Add_CSS_File_To_Header(string File_Path)
        {
            System.Web.UI.Page Current_Page = (System.Web.UI.Page)HttpContext.Current.Handler;

            HtmlGenericControl Html_Generic_Control = new HtmlGenericControl("link");
            Html_Generic_Control.Attributes.Add("rel", "stylesheet");
            Html_Generic_Control.Attributes.Add("type", "text/css");
            Html_Generic_Control.Attributes.Add("href", File_Path);

            Current_Page.Page.Header.Controls.Add(Html_Generic_Control);
        }
        public void Add_JavaScript_File_To_Header(string File_Path)
        {
            System.Web.UI.Page Current_Page = (System.Web.UI.Page)HttpContext.Current.Handler;

            HtmlGenericControl Html_Generic_Control = new HtmlGenericControl("script");
            Html_Generic_Control.Attributes.Add("type", "text/javascript");
            Html_Generic_Control.Attributes.Add("src", File_Path);

            Current_Page.Page.Header.Controls.Add(Html_Generic_Control);
        }
        public void Add_JavaScript_File_To_Body(Control Page_Body, string File_Path)
        {
            System.Web.UI.Page Current_Page = (System.Web.UI.Page)HttpContext.Current.Handler;

            HtmlGenericControl Html_Generic_Control = new HtmlGenericControl("script");
            Html_Generic_Control.Attributes.Add("type", "text/javascript");
            Html_Generic_Control.Attributes.Add("src", File_Path);

            Page_Body.Controls.Add(Html_Generic_Control);
        }

        //?????
        public void Add_ALL_JavaScript_AND_CSS_File_To_Header_Basic(string Index_Host)
        {
            //OK
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/jquery-1.10.2.js");
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/jquery-migrate-1.2.1.js");

            //OK
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/jquery.easing.1.3.js");
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/jquery-ui.js");
            Add_CSS_File_To_Header(Index_Host + "/index/Java_Script/Jquery/jquery-ui.css");

            //Not Used
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/jquery.marquee.js");
            //Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/jquery.viewport.js");

            //OK
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/tag-it.js");
            Add_CSS_File_To_Header(Index_Host + "/index/Java_Script/Jquery/tagit.css");

            //OK
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/jquery.zoomooz.js");
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/jquery.watermark.js");

            //OK
            Add_CSS_File_To_Header(Index_Host + "/index/Java_Script/Highslide/highslide.css");
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Highslide/highslide-full.js");

            //OK
            Add_CSS_File_To_Header(Index_Host + "/index/Java_Script/jScrollPane/jquery.jscrollpane.css");
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/jScrollPane/jquery.jscrollpane.js");
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/jScrollPane/jquery.mousewheel.js");
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/jScrollPane/mwheelIntent.js");

            //OK
            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Jquery/json2.js");

            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Slider/Rotator.js");

            Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/Keyboard/js/keyboard.js");
            Add_CSS_File_To_Header(Index_Host + "/index/Java_Script/Keyboard/css/keyboard.css");
            Add_CSS_File_To_Header(Index_Host + "/index/Java_Script/Keyboard/css/jquery-ui.min.css");
            Add_CSS_File_To_Header(Index_Host + "/index/Java_Script/Keyboard/css/font-awesome.min.css");

            //
            Add_CSS_File_To_Header(Index_Host + "/index/CSS/4u4m4e.css?" + Guid.NewGuid().ToString());
            Add_CSS_File_To_Header(Index_Host + "/index/CSS/4u4m4e_Plugin.css?" + Guid.NewGuid().ToString());
        }

        public void Add_ALL_JavaScript_AND_CSS_File_To_Header(string Index_Host)
        {
            //OK
            Add_ALL_JavaScript_AND_CSS_File_To_Header_Basic(Index_Host);

            //
            string Directory_Path = HttpContext.Current.Server.MapPath("~/index/Java_Script/");

            DirectoryInfo Directory_Info = new DirectoryInfo(Directory_Path);
            FileInfo[] File_Info_Array = Directory_Info.GetFiles();

            foreach (FileInfo File_Info in File_Info_Array)
            {
                string File_Name = Path.GetFileName(File_Info.FullName);

                if (File_Name.ToLower().EndsWith(".js"))
                {
                    Add_JavaScript_File_To_Header(Index_Host + "/index/Java_Script/" + File_Name + "?" + File_Info.CreationTime.ToString().Replace(" ", "_").Replace(":", "_"));
                }
            }
        }

        public void Add_ALL_Index_Host_To_IMG(string Index_Host)
        {
            System.Web.UI.Page Current_Page = (System.Web.UI.Page)HttpContext.Current.Handler;

            //IMG
            List<System.Web.UI.WebControls.Image> ALL_IMG = new List<System.Web.UI.WebControls.Image>();
            Get_Control_List<System.Web.UI.WebControls.Image>(Current_Page.Controls, ALL_IMG);

            foreach (var IMG in ALL_IMG)
            {
                if (IMG.ImageUrl.StartsWith("/index/"))
                {
                    IMG.ImageUrl = Index_Host + IMG.ImageUrl;
                }
            }

            //LNK
            List<System.Web.UI.WebControls.HyperLink> ALL_LNK = new List<System.Web.UI.WebControls.HyperLink>();
            Get_Control_List<System.Web.UI.WebControls.HyperLink>(Current_Page.Controls, ALL_LNK);

            foreach (var LNK in ALL_LNK)
            {
                if (LNK.ImageUrl.StartsWith("/index/"))
                {
                    LNK.ImageUrl = Index_Host + LNK.ImageUrl;
                }
            }

            //TBL
            List<Table> ALL_TBL = new List<Table>();
            Get_Control_List<Table>(Current_Page.Controls, ALL_TBL);

            foreach (var TBL in ALL_TBL)
            {
                if (TBL.BackImageUrl.StartsWith("/index/"))
                {
                    TBL.BackImageUrl = Index_Host + TBL.BackImageUrl;
                }
            }
        }

        //
        public void Response_Write_Only_Message(string String_Input)
        {
            string Message = "<table style='height: 100%; width: 100%;'><tr><td align='center'><a href = 'http://" + Read_Domain(string.Empty) + "' style = 'text-decoration: none; color: Red; font-family: Verdana; font-size: 16;'>" + String_Input + "</a></td></tr></table>";

            HttpContext.Current.Response.Write(Message);
        }

        //
        public void Global_asax_Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Application["Schema_Data_Base"] == null)
            {
                HttpContext.Current.Application["Schema_Data_Base"] = "DBO";
            }
        }
        public void Global_asax_Application_Error(object sender, EventArgs e)
        {
            string IP = HttpContext.Current.Request.UserHostAddress;
            string Domain = Read_Domain(string.Empty);

            int Http_Runtime_Max_Request_Length = 15 * 1024 * 1024;
            int Current_Request_Content_Length = HttpContext.Current.Request.ContentLength;

            Exception Ex = HttpContext.Current.Server.GetLastError();

            bool Re_Load_Page = false;
            bool Log_Error = true;

            //System.Web.HttpCompileException
            //HttpCompileException
            //HttpException
            //HttpParseException 
            //HttpRequestValidationException

            //Vượt dung lượng tối đa - maxWaitChangeNotification="" maxQueryStringLength="" maxUrlLength=""
            if (Current_Request_Content_Length > Http_Runtime_Max_Request_Length)
            {
                Write_To_File("\\Temp\\Re_Set_Is_Bot_IP.txt", IP + " Because: Vượt quá dung lượng tối đa !");
            }

            if (Ex is System.Web.HttpRequestValidationException)
            {
                Write_To_File("\\Temp\\Re_Set_Is_Bot_IP.txt", IP + " Because: Có mã nguy hiểm !");
            }

            //
            if (Ex is InvalidOperationException)
            {
            }

            //            
            if (Ex is ArgumentException)
            {
            }

            //
            if (Ex is ArgumentNullException)
            {
            }

            //
            if (Ex is ArgumentOutOfRangeException)
            {
            }

            //
            if (Ex is HttpUnhandledException)
            {
            }

            //Từng Lỗi Cụ Thể
            if (
                (Ex.ToString().Contains("Object reference not set to an instance of an object"))
                || (Ex.ToString().Contains("Timeout expired"))
                )
            {
                Re_Load_Page = true;
            }

            //
            if (
                (Ex.ToString().Contains("A potentially dangerous"))
                || (Ex.ToString().Contains("does not exist"))

                || (Ex.ToString().Contains("This is an invalid script resource request"))
                || (Ex.ToString().Contains("This is an invalid webresource request"))

                || ((Ex.ToString().Contains("Could not find a part of the path '")) && (Ex.ToString().Contains(".htm'.")))
                || ((Ex.ToString().Contains("The process cannot access the file '")) && (Ex.ToString().Contains(".txt' because it is being used by another process.")))

                || (HttpContext.Current.Application["Sql_Connection_String"] == null)

                || (Ex.ToString().Contains("cannot be autostarted during server shutdown or startup"))
                || (Ex.ToString().Contains("SHUTDOWN is in progress."))

                || (Ex.ToString().Contains("Waiting until recovery is finished."))

                || (Ex.ToString().Contains("error: 40 - Could not open a connection to SQL Server"))
                || (Ex.ToString().Contains("error: 0 - An existing connection was forcibly closed by the remote host"))

                || (Check_ID(Domain.Replace(".", string.Empty)))
                )
            {
                Log_Error = false;
            }

            //Always log
            //Log_Error = true;

            //
            if (Log_Error)
            {
                Write_To_File("\\Error\\" + Get_Time_Name_Code() + ".txt", "Type: " + Ex.GetType().ToString() + Environment.NewLine + Environment.NewLine + "Message: " + Ex.Message + Environment.NewLine + Environment.NewLine + "ALL: " + Ex.ToString());
            }

            //
            if (!HttpContext.Current.Request.IsLocal)
            {
                HttpContext.Current.Server.ClearError();

                Message = "Hệ Thống Đang CẬP NHẬT ! Vui Lòng Liên Hệ Với Ban Quản Trị !<br/><br/>Click Vào Đây Để Trở Về TRANG CHỦ !";

                //
                if (Re_Load_Page)
                {
                    string URL = HttpContext.Current.Request.Url.ToString();

                    if (HttpContext.Current.Session["Times_Reload_Of_" + HttpUtility.UrlEncode(URL)] == null)
                    {
                        HttpContext.Current.Session["Times_Reload_Of_" + HttpUtility.UrlEncode(URL)] = 0;
                    }

                    int Reload = (int)HttpContext.Current.Session["Times_Reload_Of_" + HttpUtility.UrlEncode(URL)] + 1;

                    if (Reload <= 5)
                    {
                        System.Threading.Thread.Sleep(1000 * 3);
                        HttpContext.Current.Response.Redirect(URL);
                    }
                    else
                    {
                        Response_Write_Only_Message(Message);
                    }
                }
                else
                {
                    Response_Write_Only_Message(Message);
                }
            }
        }

        //
        public void Write_To_File(string File_Path, string Content)
        {
            if (File_Path.StartsWith("\\"))
            {
                File_Path = HttpContext.Current.Server.MapPath("~").Replace(Remove_String_Last(Path.GetPathRoot(HttpContext.Current.Server.MapPath("~")), "\\"), "D:") + File_Path;
            }

            Creat_Directory(File_Path);

            using (StreamWriter Stream_Writer = new StreamWriter(File_Path, true, Encoding.Unicode))
            {
                Stream_Writer.WriteLine(Content);
            }
        }
        public void Write_To_File_Temp(string File_Name, string Content)
        {
            string File_Path = "\\Temp\\" + File_Name;

            Write_To_File(File_Path, Content);
        }
        public void Delete_File(string File_Item)
        {
            if (File_Item.StartsWith("\\"))
            {
                File_Item = HttpContext.Current.Server.MapPath("~").Replace(Remove_String_Last(Path.GetPathRoot(HttpContext.Current.Server.MapPath("~")), "\\"), "D:") + File_Item;
            }

            if (File.Exists(File_Item))
            {
                if (!Check_File_Is_In_Use(File_Item))
                {
                    try
                    {
                        File.Delete(File_Item);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        public void Creat_Directory(string Directory_Path)
        {
            //Directory_Path = Remove_String_Last(Directory_Path, "\\");
            Directory_Path = Path.GetDirectoryName(Directory_Path);

            if (!Directory.Exists(Directory_Path))
            {
                Directory.CreateDirectory(Directory_Path);
            }
        }
        public bool Check_File_Is_In_Use(string File_Path)
        {
            if (File.Exists(File_Path))
            {
                FileStream File_Stream = null;

                try
                {
                    File_Stream = File.Open(File_Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (Exception Ex)
                {
                    return true;
                }
                finally
                {
                    if (File_Stream != null)
                    {
                        File_Stream.Close();
                    }
                }
            }

            return false;
        }

        public void Add_SQL_Parameters(ref SqlCommand Sql_Command, string[] SQL_Parameters_Array, string[] SQL_Value_Array)
        {
            if (SQL_Parameters_Array != null)
            {
                for (int i1 = 0; i1 < SQL_Parameters_Array.Length; i1++)
                {
                    if (Check_String_Is_Not_Null_AND_Not_Space(SQL_Value_Array[i1]))
                    {
                        Sql_Command.Parameters.Add(SQL_Parameters_Array[i1], SQL_Value_Array[i1]);
                    }
                    else
                    {
                        Sql_Command.Parameters.Add(SQL_Parameters_Array[i1], DBNull.Value);
                    }
                }
            }
        }

        public Literal Creat_Label_ltr(string String_Input)
        {
            Literal String_Input_ltr = new Literal();
            String_Input_ltr.Text = String_Input;

            return String_Input_ltr;
        }

        public HtmlTable Creat_HTBL(string ID, string Width, string Height, int Border, int CellPadding, int CellSpacing, string Style)
        {
            if (Width == "1")
            {
                Width = "100%";
            }

            if (Height == "1")
            {
                Height = "100%";
            }

            HtmlTable TBL = new HtmlTable();

            if (ID != string.Empty)
            {
                TBL.ID = ID + "_htbl";
            }

            if (Width != "0")
            {
                TBL.Width = Width;
            }

            if (Height != "0")
            {
                TBL.Width = Height;
            }

            TBL.CellPadding = CellPadding;
            TBL.CellSpacing = CellSpacing;
            TBL.Border = Border;

            TBL.Attributes.CssStyle.Add("style", Style);

            return TBL;
        }
        public HtmlTableRow Creat_HTR(string ID)
        {
            HtmlTableRow TR = new HtmlTableRow();

            if (ID != string.Empty)
            {
                TR.ID = ID + "_tr";
            }

            return TR;
        }
        public HtmlTableCell Creat_HTD(string ID, string Width, string Height, string Align_Position, string Valign_Position, int Column_Span, string InnerHtml, bool F_B, bool F_I, string F_Color)
        {
            if (Width == "1")
            {
                Width = "100%";
            }

            if (Height == "1")
            {
                Height = "100%";
            }

            HtmlTableCell TD = new HtmlTableCell();

            if (ID != string.Empty)
            {
                TD.ID = ID + "_td";
            }

            if (Width != "0")
            {
                TD.Width = Width;
                TD.Style.Add("min-width", Width + "px");
                //TD.Style.Add("max-width", Width);
            }

            if (Height != "0")
            {
                TD.Height = Height;
            }

            if (Column_Span != 0)
            {
                TD.ColSpan = Column_Span;
            }

            if (InnerHtml != string.Empty)
            {
                if (F_B)
                {
                    InnerHtml = "<b>" + InnerHtml + "</b>";
                }

                if (F_I)
                {
                    InnerHtml = "<i>" + InnerHtml + "</i>";
                }

                if (F_Color != string.Empty)
                {
                    InnerHtml = "<span style = 'color: " + F_Color + "'>" + InnerHtml + "</span>";
                }

                TD.InnerHtml = InnerHtml;
            }

            TD.Align = Align_Position;
            TD.VAlign = Valign_Position;

            return TD;
        }

        public HtmlTableCell Creat_HTH(string ID, string Width, string Height, string Align_Position, string Valign_Position, int Column_Span, string InnerHtml, bool F_B, bool F_I, string F_Color)
        {
            if (Width == "1")
            {
                Width = "100%";
            }

            if (Height == "1")
            {
                Height = "100%";
            }

            HtmlTableCell TD = new HtmlTableCell("th");

            if (ID != string.Empty)
            {
                TD.ID = ID + "_td";
            }

            if (Width != "0")
            {
                TD.Width = Width;
                TD.Style.Add("min-width", Width + "px");
                //TD.Style.Add("max-width", Width);
            }

            if (Height != "0")
            {
                TD.Height = Height;
            }

            if (Column_Span != 0)
            {
                TD.ColSpan = Column_Span;
            }

            if (InnerHtml != string.Empty)
            {
                if (F_B)
                {
                    InnerHtml = "<b>" + InnerHtml + "</b>";
                }

                if (F_I)
                {
                    InnerHtml = "<i>" + InnerHtml + "</i>";
                }

                if (F_Color != string.Empty)
                {
                    InnerHtml = "<span style = 'color: " + F_Color + "'>" + InnerHtml + "</span>";
                }

                TD.InnerHtml = InnerHtml;
            }

            TD.Align = Align_Position;
            TD.VAlign = Valign_Position;

            return TD;
        }

        //Kiểm tra số (Trả về True, False)
        public bool Check_Int(string String_Input)
        {
            //Kiem tra la so int 32 <= 2147483647                                       
            bool Valid = true;

            if (Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                if ((String_Input.Length < 1) || (String_Input.Length > 10) || (String_Input.Contains(" ")))
                {
                    Valid = false;
                }
                else
                {
                    char[] String_Input_Array = String_Input.ToCharArray();

                    for (int i1 = 0; i1 < String_Input_Array.Length; i1++)
                    {
                        if (!char.IsNumber(String_Input_Array[i1]))
                        {
                            Valid = false;
                        }
                    }

                    if (Valid)
                    {
                        if (Convert.ToInt64(String_Input) > 2147483647)
                        {
                            Valid = false;
                        }
                    }
                }
            }
            else
                Valid = false;

            return Valid;
        }
        public bool Check_BigInt(string String_Input)
        {
            //9,223,372,036,854,775,807

            //Kiem tra la so int 32 <= 2147483647                                       
            bool Valid = true;

            if (Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                if ((String_Input.Length < 1) || (String_Input.Length > 19) || (String_Input.Contains(" ")))
                {
                    Valid = false;
                }
                else
                {
                    char[] String_Input_Array = String_Input.ToCharArray();

                    for (int i1 = 0; i1 < String_Input_Array.Length; i1++)
                    {
                        if (!char.IsNumber(String_Input_Array[i1]))
                        {
                            Valid = false;
                        }
                    }

                    if (Valid)
                    {
                        if ((Convert.ToInt64(String_Input) < -9223372036854775807) || (Convert.ToInt64(String_Input) > 9223372036854775807))
                        {
                            Valid = false;
                        }
                    }
                }
            }
            else
                Valid = false;

            return Valid;
        }
        public bool Check_Date(string String_Input)
        {
            bool Valid = false;

            if (Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                string Date_String_Result = string.Empty;
                Check_AND_Convert_Date_String_To_String(String_Input, out Valid, out Date_String_Result);
            }

            return Valid;
        }
        public bool Check_Date_Less_Than_Today(string String_Input)
        {
            bool Valid = false;

            if (Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                DateTime Date_Temp = Convert_Date_String_To_Date_Time(String_Input, new DateTime());

                if (Date_Temp != new DateTime())
                {
                    if (Date_Temp <= DateTime.Now)
                    {
                        Valid = true;
                    }
                }
            }

            return Valid;
        }

        public bool Check_ID(string String_Input)
        {
            return Check_BigInt(String_Input);
        }

        //
        public int Convert_String_To_Int(string String_Input, int If_Is_False)
        {
            int Result = If_Is_False;

            if (Check_Int(String_Input))
            {
                Result = Convert.ToInt32(String_Input);
            }

            return Result;
        }
        public Int64 Convert_String_To_BigInt(string String_Input, int If_Is_False)
        {
            Int64 Result = If_Is_False;

            try
            {
                Result = Int64.Parse(String_Input);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return Result;
        }

        public string Convert_Boolean_To_Bit_String(bool Boolean)
        {
            string Result = "0";

            if (Boolean.ToString().ToLower() == "true")
            {
                Result = "1";
            }

            return Result;
        }
        public string Convert_String_To_Bit_String(string String_Input)
        {
            string Result = "0";

            if (Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                if ((String_Input.ToString().ToLower() == "true") || (String_Input.ToString().ToLower() == "1"))
                {
                    Result = "1";
                }
            }

            return Result;
        }
        public bool Convert_String_To_Boolean(string String_Input)
        {
            bool Result = false;

            if (Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                if ((String_Input.ToLower() == "true") || (String_Input == "1"))
                {
                    Result = true;
                }
            }

            return Result;
        }

        public void Check_AND_Convert_Date_String_To_String(string String_Input, out bool Valid, out string Date_String_Result)
        {
            Valid = false;
            Date_String_Result = string.Empty;

            if (Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                string Day = "0";
                string Month = "0";
                string Year = "0000";

                //__/__/____
                String_Input = String_Input.Replace("-", "/");
                String_Input = Remove_String_Duplicate(String_Input, "/");

                //
                string[] Input_Array = String_Input.Split('/');

                int Day_Temp = 1;
                int Month_Temp = 1;
                int Year_Temp = 0001;

                if (Input_Array.Length == 3)
                {
                    Day_Temp = Convert_String_To_Int(Input_Array[0], 1);
                    Month_Temp = Convert_String_To_Int(Input_Array[1], 1);
                    Year_Temp = Convert_String_To_Int(Input_Array[2], 1);
                }

                //
                if (Day_Temp <= 31)
                {
                    Day = Day_Temp.ToString();
                }

                if (Month_Temp <= 12)
                {
                    Month = Month_Temp.ToString();
                }

                Year = Year_Temp.ToString();

                //
                if (Day.Length == 1)
                {
                    Day = "0" + Day;
                }

                if (Month.Length == 1)
                {
                    Month = "0" + Month;
                }

                for (int i1 = 0; i1 < (4 - Year.Length); i1++)
                {
                    Year = "0" + Year;
                }

                String_Input = Day + "/" + Month + "/" + Year;

                //Alert_Message("Input : " + Input);

                //
                DateTime New_Date_Time = new DateTime();

                //
                try
                {
                    DateTime.TryParseExact(String_Input, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out New_Date_Time);
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }

                if (New_Date_Time != new DateTime())
                {
                    Valid = true;
                    Date_String_Result = New_Date_Time.ToShortDateString();

                    //Alert_Message("OK : " + Date_String_Result);
                }
                else
                {
                    //Alert_Message("NO : " + Input);
                }
            }
        }
        public string Convert_Date_String_To_String(string String_Input, string If_InValid)
        {
            bool Valid = false;
            string Date_String_Result = string.Empty;

            Check_AND_Convert_Date_String_To_String(String_Input, out Valid, out Date_String_Result);

            if (!Valid)
            {
                Date_String_Result = If_InValid;
            }

            return Date_String_Result;
        }

        public void Check_AND_Convert_Date_String_To_Date_Time(string Input, out bool Valid, out DateTime Date_Result)
        {
            Valid = false;
            Date_Result = new DateTime();

            if (Check_String_Is_Not_Null_AND_Not_Space(Input))
            {
                string Day = "0";
                string Month = "0";
                string Year = "0000";

                //
                Input = Input.Replace("-", "/");
                Input = Remove_String_Duplicate(Input, "/");

                //
                string[] Input_Array = Input.Split('/');

                int Day_Temp = 1;
                int Month_Temp = 1;
                int Year_Temp = 0001;

                if (Input_Array.Length == 3)
                {
                    Day_Temp = Convert_String_To_Int(Input_Array[0], 1);
                    Month_Temp = Convert_String_To_Int(Input_Array[1], 1);
                    Year_Temp = Convert_String_To_Int(Input_Array[2], 1);
                }

                //
                if (Day_Temp <= 31)
                {
                    Day = Day_Temp.ToString();
                }

                if (Month_Temp <= 12)
                {
                    Month = Month_Temp.ToString();
                }

                Year = Year_Temp.ToString();

                //
                if (Day.Length == 1)
                {
                    Day = "0" + Day;
                }

                if (Month.Length == 1)
                {
                    Month = "0" + Month;
                }

                for (int i1 = 0; i1 < (4 - Year.Length); i1++)
                {
                    Year = "0" + Year;
                }

                Input = Day + "/" + Month + "/" + Year;

                //
                DateTime New_Date_Time = new DateTime();

                //
                try
                {
                    DateTime.TryParseExact(Input, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out New_Date_Time);
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }

                if (New_Date_Time != new DateTime())
                {
                    Valid = true;
                    Date_Result = New_Date_Time;
                }
            }
        }
        public DateTime Convert_Date_String_To_Date_Time(string Input, DateTime If_InValid)
        {
            bool Valid = false;
            DateTime Date_Time_Result = new DateTime();

            //
            Check_AND_Convert_Date_String_To_Date_Time(Input, out Valid, out Date_Time_Result);

            if (!Valid)
            {
                Date_Time_Result = If_InValid;
            }

            return Date_Time_Result;
        }

        public bool Check_String_Is_Not_Null_OR_Space(string String_Input)
        {
            bool Result = false;

            if (String_Input != null)
            {
                if (String_Input != string.Empty)
                {
                    Result = true;
                }
            }

            return Result;
        }
        public bool Check_String_Is_Not_Null_AND_Not_Space(string String_Input)
        {
            bool Result = false;

            String_Input = Remove_Space_String(String_Input);

            if (String_Input != null)
            {
                if (String_Input != string.Empty)
                {
                    Result = true;
                }
            }

            return Result;
        }

        public bool Check_String_Length(string String_Input, int Min_Length, int Max_Length)
        {
            bool Valid = false;

            if (String_Input != null)
            {
                if ((String_Input.Length >= Min_Length) && (String_Input.Length <= Max_Length))
                {
                    Valid = true;
                }
            }

            return Valid;
        }

        public string Remove_String_Last(string Text, string Last_Text)
        {
            if ((Check_String_Is_Not_Null_OR_Space(Text)) && (Check_String_Is_Not_Null_OR_Space(Last_Text)))
            {
                if (Text.Length >= Last_Text.Length)
                {
                    while ((Text.EndsWith(Last_Text)) || (Text.EndsWith(" ")))
                    {
                        if (Text.EndsWith(Last_Text))
                        {
                            Text = Text.Remove(Text.Length - Last_Text.Length, Last_Text.Length);
                        }
                        else
                        {
                            Text = Text.Remove(Text.Length - 1, 1);
                        }
                    }
                }
            }

            return Text;
        }
        public string Remove_String_First(string Text, string First_Text)
        {
            if ((Check_String_Is_Not_Null_OR_Space(Text)) && (Check_String_Is_Not_Null_OR_Space(First_Text)))
            {
                while ((Text.StartsWith(First_Text)) || (Text.StartsWith(" ")))
                {
                    if (Text.StartsWith(First_Text))
                    {
                        Text = Text.Remove(0, First_Text.Length);
                    }
                    else
                    {
                        Text = Text.Remove(0, 1);
                    }
                }
            }

            return Text;
        }

        public string Remove_Space_String(string Text)
        {
            if (Check_String_Is_Not_Null_OR_Space(Text))
            {
                //Convert tabs - space
                Text = Text.Replace("\t", " ");

                //Lọc khoảng cách ở giữa
                while (Text.Contains("  "))
                {
                    Text = Text.Replace("  ", " ");
                }

                //Lọc khoảng cách ở đầu
                Text = Remove_Space_String_First(Text);

                //Lọc khoảng cách ở cuối
                Text = Remove_Space_String_Last(Text);
            }

            return Text;
        }
        public string Remove_Space_String_Last(string Text)
        {
            if (Check_String_Is_Not_Null_OR_Space(Text))
            {
                while (Text.EndsWith(" "))
                {
                    Text = Text.Remove(Text.Length - 1, 1);
                }
            }

            return Text;
        }
        public string Remove_Space_String_First(string Text)
        {
            if (Check_String_Is_Not_Null_OR_Space(Text))
            {
                while (Text.StartsWith(" "))
                {
                    Text = Text.Remove(0, 1);
                }
            }

            return Text;
        }

        public string Remove_0_Before(string String_Input)
        {
            if (Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                while (String_Input.StartsWith("0"))
                {
                    String_Input = String_Input.Remove(0, 1);
                }
            }

            return String_Input;
        }
        public string Remove_String_Duplicate(string Input, string String_Duplicate)
        {
            if ((Check_String_Is_Not_Null_OR_Space(Input)) && (Check_String_Is_Not_Null_OR_Space(String_Duplicate)))
            {
                while (Input.Contains(String_Duplicate + String_Duplicate))
                {
                    Input = Input.Replace(String_Duplicate + String_Duplicate, String_Duplicate);
                }
            }

            return Input;
        }

        public string Remove_From_String_To(string Text, string Start_Text, string End_Text)
        {
            if ((Check_String_Is_Not_Null_OR_Space(Text)) && (Check_String_Is_Not_Null_OR_Space(Start_Text)) && (Check_String_Is_Not_Null_OR_Space(End_Text)))
            {
                if ((Text.Contains(Start_Text)) && (Text.Contains(End_Text)))
                {
                    int Start_Text_Index = Text.IndexOf(Start_Text);
                    int Last_Text_Index = Text.IndexOf(End_Text);

                    Text = Text.Remove(Start_Text_Index, Last_Text_Index - Start_Text_Index + End_Text.Length);
                }
            }

            return Text;
        }
        public string Remove_From_String_To_End(string Text, string Last_Text)
        {
            if ((Check_String_Is_Not_Null_OR_Space(Text)) && (Check_String_Is_Not_Null_OR_Space(Last_Text)))
            {
                if (Text.Contains(Last_Text))
                {
                    int Index = Text.IndexOf(Last_Text);

                    Text = Text.Remove(Index, Text.Length - Index);
                }
            }

            return Text;
        }
        public string Remove_From_String_To_End_Last(string Text, string Last_Text)
        {
            if ((Check_String_Is_Not_Null_OR_Space(Text)) && (Check_String_Is_Not_Null_OR_Space(Last_Text)))
            {
                if (Text.Contains(Last_Text))
                {
                    int Index = Text.LastIndexOf(Last_Text);

                    Text = Text.Remove(Index, Text.Length - Index);
                }
            }

            return Text;
        }

        public string Get_From_String_To(string Text, string Start_Text, string End_Text)
        {
            string Result = string.Empty;

            if ((Text.Contains(Start_Text)) && (Text.Contains(End_Text)))
            {
                try
                {
                    var Start_Text_index = Text.IndexOf(Start_Text) + Start_Text.Length;
                    Result = Text.Substring(Start_Text_index, Text.IndexOf(End_Text) - Start_Text_index);
                }
                catch
                {
                    string Regex_ruler = Start_Text + "(.*?)" + End_Text;
                    Regex Re_gex = new Regex(Regex_ruler, RegexOptions.IgnoreCase);

                    Match New_Match = Re_gex.Match(Text);

                    if (New_Match.Success)
                    {
                        Result = New_Match.Groups[1].Value;
                    }
                }
            }

            return Result;
        }

        public string UpperCase_First_Character(string String_Input)
        {
            string Result = string.Empty;

            //Nếu Văn bản null

            if (!string.IsNullOrEmpty(String_Input))
            {
                //Viết hoa chữ cái đầu tiên trong tất cả các từ

                //String_Input = String_Input.ToLower();

                char[] Character_Array = String_Input.ToCharArray();

                // Handle the first letter in the string.

                if (Character_Array.Length >= 1)
                {
                    if (char.IsLower(Character_Array[0]))
                    {
                        Character_Array[0] = char.ToUpper(Character_Array[0]);
                    }
                }

                // Scan through the letters, checking for spaces.
                // ... Uppercase the lowercase letters following spaces.

                for (int i = 1; i < Character_Array.Length; i++)
                {
                    if (Character_Array[i - 1] == ' ')
                    {
                        if (char.IsLower(Character_Array[i]))
                        {
                            Character_Array[i] = char.ToUpper(Character_Array[i]);
                        }
                    }
                }

                Result = new string(Character_Array);
            }

            return Result;
        }

        public string Add_http_To_URL(string URL)
        {
            if ((!URL.StartsWith("http://")) && (!URL.StartsWith("https://")))
            {
                URL = "http://" + URL;
            }

            return URL;
        }

        public string Remove_Non_Latinh_String(string String_Input)
        {
            if (Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                string[] Non_Latinh_Character_Array = new string[] { "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ", "ÝỲỴỶỸ" };

                for (int i = 1; i < Non_Latinh_Character_Array.Length; i++)
                {
                    for (int j = 0; j < Non_Latinh_Character_Array[i].Length; j++)
                    {
                        String_Input = String_Input.Replace(Non_Latinh_Character_Array[i][j], Non_Latinh_Character_Array[0][i - 1]);
                    }
                }
            }

            return String_Input;
        }


        //
        public string Remove_Danger_String(string String_Input)
        {
            //Lọc nếu Chỉ toàn khoảng trắng
            if (!Check_String_Is_Not_Null_AND_Not_Space(String_Input))
            {
                String_Input = string.Empty;
            }
            else
            {
                //String_Input = String_Input.Replace(";", ",");

                //Lọc từ nếu có trong mảng ko hợp lệ (LowerCase)

                string[] InValid_String_Array = new string[] { 
            
                //Từ nguy hiểm
                //";", "*", "'", "--", "+", "/*", "*/", "xp_", "<", ">", 
            
                //Từ trùng với từ khóa SQL, .Net
                //"select", "insert", "update", "delete",

                //Từ pậy pạ
                "địt", "đụ", "lồn", "buồi", "dái", "chó", "cứt" };

                foreach (string InValid_String in InValid_String_Array)
                {
                    if (String_Input.ToLower().Contains(InValid_String))
                    {
                        String_Input = String_Input.Replace(InValid_String, string.Empty);
                        String_Input = String_Input.Replace(InValid_String.ToUpper(), string.Empty);
                    }
                }
            }

            //
            String_Input = HTML_Tag_Remove(String_Input);
            //String_Input = Remove_Space_String(String_Input);

            return String_Input;
        }
        public string Remove_InValid_Character(string Input, string Regex_Patent)
        {
            if (Check_String_Is_Not_Null_AND_Not_Space(Input))
            {
                Regex New_Regex = new Regex(Regex_Patent);
                Input = New_Regex.Replace(Input, string.Empty);
            }

            return Input;
        }

        public string HTML_Tag_Remove(string String_Input)
        {
            String_Input = String_Input.Replace("<", string.Empty);
            String_Input = String_Input.Replace(">", string.Empty);

            String_Input = String_Input.Replace("&lt;", string.Empty);
            String_Input = String_Input.Replace("&gt;", string.Empty);

            return String_Input;
        }

        public string Get_Input_String_From_Encoded_HTML_To_SQL(string Input_String, bool Multiline)
        {
            //Input_String = Input_String.Replace("\n", "' + CHAR(13) + CHAR(10) + N'");
            //

            //Input_String = Input_String.Replace("\n", "<br/>");
            //Input_String = Remove_String_Last(Input_String, "<br/>");

            //Input_String = HttpUtility.HtmlEncode(Input_String);
            //Input_String = Remove_Danger_String(Input_String);

            if (Check_String_Is_Not_Null_AND_Not_Space(Input_String))
            {
                if (Multiline)
                {
                    Input_String = Input_String.Replace("\n", System.Environment.NewLine);
                    //Input_String = Remove_String_Last(Input_String, " + CHAR(13) + CHAR(10) + N'");
                }
            }

            return Input_String;
        }

        public string Get_Time_Name_Code()
        {
            string Time = DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "h_" + DateTime.Now.Minute.ToString() + "m_" + DateTime.Now.Second.ToString() + "s_" + DateTime.Now.Millisecond.ToString() + "ms";

            return Time;
        }

        //
        public string Check_Sql_Query(string Sql_Query)
        {
            if (Check_String_Is_Not_Null_AND_Not_Space(Sql_Query))
            {
                //Tach Space
                Sql_Query = Sql_Query.Replace(",", " , ");
                Sql_Query = Sql_Query.Replace("(", " ( ");
                Sql_Query = Sql_Query.Replace(")", " ) ");

                //Trung Space
                while (Sql_Query.Contains("  "))
                {
                    Sql_Query = Sql_Query.Replace("  ", " ");
                }

                //Nhap Space
                Sql_Query = Sql_Query.Replace(" ,", ",");
                Sql_Query = Sql_Query.Replace("( ", "(");
                Sql_Query = Sql_Query.Replace(" )", ")");

                //N
                Sql_Query = Sql_Query.Replace("LIKE '", "LIKE N'");

                //,     
                Sql_Query = Sql_Query.Replace(",,", ",");
                Sql_Query = Sql_Query.Replace("(,", "(");
                Sql_Query = Sql_Query.Replace(",)", ")");

                Sql_Query = Sql_Query.Replace(", FROM", " FROM");
                Sql_Query = Sql_Query.Replace(", WHERE", " WHERE");
                Sql_Query = Sql_Query.Replace(", ORDER BY", " ORDER BY");
                Sql_Query = Sql_Query.Replace(", END", " END");

                //(
                Sql_Query = Sql_Query.Replace("(AND ", "(");
                Sql_Query = Sql_Query.Replace("(OR ", "(");

                //()
                Sql_Query = Sql_Query.Replace(" WHERE ()", string.Empty);
                Sql_Query = Sql_Query.Replace(" WHERE (())", string.Empty);

                Sql_Query = Sql_Query.Replace("AND ()", string.Empty);
                Sql_Query = Sql_Query.Replace("OR ()", string.Empty);

                //Update
                Sql_Query = Sql_Query.Replace("SET,", "SET");
                Sql_Query = Sql_Query.Replace("SET WHERE", "WHERE");
            }

            return Sql_Query;
        }
        public string Check_Sql_Query_Result(object Sql_Query_Result)
        {
            string Result = string.Empty;

            if (Sql_Query_Result == null)
            {
                Result = string.Empty;
            }

            if (Sql_Query_Result != null)
            {
                if ((Sql_Query_Result.ToString() == string.Empty) || (Sql_Query_Result.ToString() == "0"))
                {
                    Result = string.Empty;
                }
                else
                {
                    Result = Sql_Query_Result.ToString();
                }
            }

            //
            if (Sql_Connection != null)
            {
                Sql_Connection.Close(); Sql_Connection.Dispose();
            }

            return Result;
        }

        //
        public bool Check_Exists_Data_Base_POS(string Data_Base)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            //
            Sql_Query =
                "IF EXISTS (SELECT DB_ID(N'" + Data_Base + "'))"
                + " SELECT 'true'"

                + " ELSE"
                + " SELECT 'false'"
                ;

            Sql_Query = Check_Sql_Query(Sql_Query);
            Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            return Convert_String_To_Boolean(Check_Sql_Query_Result(Sql_Command.ExecuteScalar()));
        }

        public bool Check_Exists_Table_POS(string Data_Base, string Data_Table)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            //
            bool Result = false;

            //
            if (Check_Exists_Data_Base_POS(Data_Base))
            {
                Sql_Query =
                    "IF EXISTS (SELECT * FROM " + Data_Base + ".INFORMATION_SCHEMA.TABLES WHERE (TABLE_NAME = N'" + Data_Table + "'))"
                    + " SELECT 'true'"

                    + " ELSE"
                    + " SELECT 'false'"
                    ;

                Sql_Query = Check_Sql_Query(Sql_Query);
                Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                Result = Convert_String_To_Boolean(Check_Sql_Query_Result(Sql_Command.ExecuteScalar()));
            }

            return Result;
        }

        public void Get_Control_List<T>(ControlCollection Control_Collection, List<T> Result_Collection) where T : Control
        {
            foreach (Control Control_one in Control_Collection)
            {
                if (Control_one is T)
                {
                    Result_Collection.Add((T)Control_one);
                }

                if (Control_one.HasControls())
                {
                    Get_Control_List(Control_one.Controls, Result_Collection);
                }
            }
        }
        public string Determine_Control_Call_Postback()
        {

            System.Web.UI.Page Current_Page = (System.Web.UI.Page)HttpContext.Current.Handler;

            string Result = string.Empty;

            Control Control_Call_Postback = null;

            if ((HttpContext.Current.Request.Params["__EVENTTARGET"] != null) && (HttpContext.Current.Request.Params["__EVENTTARGET"] != String.Empty))
            {
                Control_Call_Postback = Current_Page.FindControl(HttpContext.Current.Request.Params["__EVENTTARGET"]);
            }
            else
            {
                string ctrlStr = String.Empty;

                Control Web_Control = null;

                foreach (string Web_Control_Name in HttpContext.Current.Request.Form)
                {
                    if (Web_Control_Name.EndsWith(".x") || Web_Control_Name.EndsWith(".y"))
                    {
                        ctrlStr = Web_Control_Name.Substring(0, Web_Control_Name.Length - 2);

                        Web_Control = Current_Page.FindControl(ctrlStr);
                    }
                    else
                    {
                        Web_Control = Current_Page.FindControl(Web_Control_Name);
                    }

                    if ((Web_Control is System.Web.UI.WebControls.Button) || (Web_Control is System.Web.UI.WebControls.ImageButton) || (Web_Control is System.Web.UI.WebControls.LinkButton))
                    {
                        Control_Call_Postback = Web_Control;

                        break;
                    }
                }
            }

            if (Control_Call_Postback == null)
            {
                Result = string.Empty;
            }
            else
            {
                if (Control_Call_Postback.ID != null)
                {
                    Result = Control_Call_Postback.ID.ToString();
                }
            }

            return Result;
        }

        public string Determine_Query_String_ID(string URL, string Query_String)
        {
            string Result = string.Empty;

            string Value = string.Empty;

            //
            if (URL == string.Empty)
            {
                if (HttpContext.Current.Request.QueryString[Query_String] != null)
                {
                    Value = HttpContext.Current.Request.QueryString[Query_String].ToString();
                }
            }
            else
            {
                URL = Add_http_To_URL(URL);

                Value = HttpUtility.ParseQueryString(new Uri(URL).Query).Get(Query_String);
            }

            //
            if (Check_ID(Value))
            {
                Result = Value;
            }

            Result = Value;

            return Result;
        }
        public string Determine_Query_String_Text(string URL, string Query_String)
        {
            string Result = string.Empty;

            string Value = string.Empty;

            if (URL == string.Empty)
            {
                if (HttpContext.Current.Request.QueryString[Query_String] != null)
                {
                    Value = HttpContext.Current.Request.QueryString[Query_String].ToString();
                }
            }
            else
            {
                URL = Add_http_To_URL(URL);

                Value = HttpUtility.ParseQueryString(new Uri(URL).Query).Get(Query_String);
            }

            //
            Value = HttpUtility.UrlDecode(Value);
            Value = Remove_Danger_String(Value);
            Value = Remove_Space_String(Value);

            Result = Value;

            return Result;
        }

        public string Update_Query_String(string URL, string Query_String_Name, string Query_String_Value)
        {
            if (Check_String_Is_Not_Null_AND_Not_Space(URL))
            {
                URL = Add_http_To_URL(URL);

                Uri URI = new Uri(URL);
                URL = "http://" + URI.Host + URI.AbsolutePath;

                var Parse_Query_String = HttpUtility.ParseQueryString(URI.Query);
                Parse_Query_String.Set(Query_String_Name, Query_String_Value);

                if (Parse_Query_String != null)
                {
                    URL += "?" + Parse_Query_String.ToString();
                }
            }

            URL = Remove_String_Last(URL, "?");
            URL = Remove_String_Last(URL, "/");

            return URL;
        }
        public string Remove_Query_String(string URL, string Query_String_Name)
        {
            if (Check_String_Is_Not_Null_AND_Not_Space(URL))
            {
                URL = Add_http_To_URL(URL);

                Uri URI = new Uri(URL);
                URL = "http://" + URI.Host + URI.AbsolutePath;

                var Parse_Query_String = HttpUtility.ParseQueryString(URI.Query);
                Parse_Query_String.Remove(Query_String_Name);

                if (Parse_Query_String != null)
                {
                    URL += "?" + Parse_Query_String.ToString();
                }
            }

            URL = Remove_String_Last(URL, "?");
            URL = Remove_String_Last(URL, "/");

            return URL;
        }

        public string Find_Value_In_Two_Array_Start_With(string[] Key_Array, string[] Value_Array, string Key)
        {
            string Result = null;

            if (Key_Array != null)
            {
                for (int i1 = 0; i1 < Key_Array.Length; i1++)
                {
                    if ((Key_Array[i1].ToLower().StartsWith(Key.ToLower())) || (Key.ToLower().StartsWith(Key_Array[i1].ToLower())))
                    {
                        Result = Value_Array[i1];

                        break;
                    }
                }
            }

            return Result;
        }
        public bool Check_Exists_In_Array(string[] Array_To_Check, string String_To_Check)
        {
            bool Result = false;

            if (Array_To_Check != null)
            {
                for (int i1 = 0; i1 < Array_To_Check.Length; i1++)
                {
                    if (Array_To_Check[i1].ToLower() == String_To_Check.ToLower())
                    {
                        Result = true;
                        break;
                    }
                }
            }

            return Result;
        }

        public string[] Add_Value_To_Array_String(string[] Array_To_Add, string Value)
        {
            if (Array_To_Add == null)
            {
                Array_To_Add = new string[0];
            }

            int Old_Size = Array_To_Add.Length;

            Array.Resize(ref Array_To_Add, Old_Size + 1);
            Array_To_Add[Old_Size] = Value;

            return Array_To_Add;
        }

        public string[] Split_List_To_Array_ID(string List_Input)
        {
            string[] Valid_Array = new string[0];

            int j1 = 0;

            string[] List_Input_Array = List_Input.Split('#');

            for (int i1 = 0; i1 < List_Input_Array.Length; i1++)
            {
                if (Check_ID(List_Input_Array[i1]))
                {
                    Array.Resize(ref Valid_Array, j1 + 1);
                    Valid_Array[j1] = List_Input_Array[i1];

                    j1++;
                }
            }

            return Valid_Array;
        }

        public string Determine_CBXL_Selected(System.Web.UI.WebControls.CheckBoxList CBXL, string Ruler)
        {
            string Result = string.Empty;

            for (int i1 = 0; i1 < CBXL.Items.Count; i1++)
            {
                if (CBXL.Items[i1].Selected)
                {
                    if (Ruler == "text")
                    {
                        Result += CBXL.Items[i1].Text + ", ";
                    }
                    else
                    {
                        if (Ruler == "value")
                        {
                            Result += "#" + CBXL.Items[i1].Value + "#";
                        }
                    }
                }
            }

            return Result;
        }

        public void Re_Choice_RDOL(RadioButtonList RDOL, string Value)
        {
            //Mã gửi vào là 1, nên ko cần tách mảng

            for (int i1 = 0; i1 < RDOL.Items.Count; i1++)
            {
                if (
                    (RDOL.Items[i1].Value == Value)
                    || (RDOL.Items[i1].Value.Contains("#" + Value + "#"))
                    )
                {
                    RDOL.SelectedIndex = i1;

                    break;
                }
            }
        }
        public void Re_Choice_CBXL(System.Web.UI.WebControls.CheckBoxList CBXL, string Value)
        {
            CBXL.ClearSelection();

            string[] Value_Array = Split_List_To_Array_ID(Value);

            for (int i1 = 0; i1 < Value_Array.Length; i1++)
            {
                for (int i2 = 0; i2 < CBXL.Items.Count; i2++)
                {
                    if (
                        (CBXL.Items[i2].Value == Value_Array[i1])
                        || (CBXL.Items[i2].Value.Contains("#" + Value_Array[i1] + "#"))
                        )
                    {
                        CBXL.Items[i2].Selected = true;
                    }
                }
            }
        }

        public string Object_ToString(object Object)
        {
            string Result = string.Empty;

            if (Object != null)
            {
                Result = Object.ToString();
            }

            return Result;
        }

        public void Logout()
        {
            //
            //Global_asax_Session_End();

            //
            MembershipUser User = Membership.GetUser(false);

            /*
            HttpCookie Http_Cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            Http_Cookie.Domain = Read_Top_Domain(string.Empty);
            Http_Cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.AppendCookie(Http_Cookie);
            */

            /*
            HttpCookie Cookie_To_Delete = new HttpCookie(FormsAuthentication.FormsCookieName);
            Cookie_To_Delete.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(Cookie_To_Delete);
            */

            //???
            string[] ALL_Cookies = HttpContext.Current.Request.Cookies.AllKeys;

            foreach (string Cookies_Key in ALL_Cookies)
            {
                HttpContext.Current.Response.Cookies[Cookies_Key].Expires = DateTime.Now.AddDays(-1);
            }

            //
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Request.Cookies.Clear();
            FormsAuthentication.SignOut();

            if (User != null)
            {
                User.LastActivityDate = DateTime.UtcNow.AddMinutes(-10);
                Membership.UpdateUser(User);
            }
        }
        public string Read_UserName()
        {
            string UserName = string.Empty;

            if (HttpContext.Current.User != null)
            {
                UserName = HttpContext.Current.User.Identity.Name;
            }

            //
            string[] UserName_Array = UserName.Split('\\');

            if (UserName_Array.Length == 2)
            {
                UserName = UserName_Array[1];
            }

            UserName = UpperCase_First_Character(UserName);

            return UserName;
        }
        public string Creat_Name_Code(string Input)
        {
            if (Check_String_Is_Not_Null_AND_Not_Space(Input))
            {
                Input = Remove_Non_Latinh_String(Input);

                Input = Remove_InValid_Character(Input, "[^a-zA-Z0-9-_/– ]");
                Input = Remove_Space_String(Input);

                Input = Input.Replace(" ", "_").Replace("-", "_").Replace("–", "_").Replace("/", "_");
                Input = Remove_String_Duplicate(Input, "_");

                if (Check_ID(Input[0].ToString()))
                {
                    Input = "_" + Input;
                }
            }

            return Input;
        }

        public string Read_Domain(string Domain)
        {
            if (Domain == string.Empty)
            {
                int Port = HttpContext.Current.Request.Url.Port;

                if (Port == 80)
                {
                    Domain = HttpContext.Current.Request.Url.Host.ToLower().Replace("www.", string.Empty);
                }
                else
                {
                    Domain = HttpContext.Current.Request.Url.Host.ToLower().Replace("www.", string.Empty) + ":" + Port;
                }
            }
            else
            {
                Domain = Add_http_To_URL(Domain);

                Uri Domain_URI = new Uri(Domain);

                int Port = Domain_URI.Port;

                if (Port == 80)
                {
                    Domain = Domain_URI.Host.ToLower().Replace("www.", string.Empty);
                }
                else
                {
                    Domain = Domain_URI.Host.ToLower().Replace("www.", string.Empty) + ":" + Port;
                }
            }

            return Domain;
        }

        public bool Check_PageMethods_Is_undefined()
        {
            bool Result = false;

            if (HttpContext.Current.Request.Url.ToString().ToLower().EndsWith("/undefined"))
            {
                Result = true;
            }

            return Result;
        }

        #region JSON
        public class List_2_Item
        {
            public string Item_1 { get; set; }
            public string Item_2 { get; set; }
        }

        //
        public class List_3_Item
        {
            public string Item_1 { get; set; }
            public string Item_2 { get; set; }
            public string Item_3 { get; set; }
        }

        //
        public class List_7_Item
        {
            public string Item_1 { get; set; }
            public string Item_2 { get; set; }
            public string Item_3 { get; set; }
            public string Item_4 { get; set; }
            public string Item_5 { get; set; }
            public string Item_6 { get; set; }
            public string Item_7 { get; set; }
        }

        //
        public class List_10_Item
        {
            public string Item_1 { get; set; }
            public string Item_2 { get; set; }
            public string Item_3 { get; set; }
            public string Item_4 { get; set; }
            public string Item_5 { get; set; }
            public string Item_6 { get; set; }
            public string Item_7 { get; set; }
            public string Item_8 { get; set; }
            public string Item_9 { get; set; }
            public string Item_10 { get; set; }
        }

        public string Creat_JSON_From_List_2_Item(string[] Item_1_Array, string[] Item_2_Array)
        {
            List<List_2_Item> List_2_Item = new List<List_2_Item>();

            for (int i1 = 0; i1 < Item_1_Array.Length; i1++)
            {
                List_2_Item List_2_Item_one = new List_2_Item();

                List_2_Item_one.Item_1 = Item_1_Array[i1];
                List_2_Item_one.Item_2 = Item_2_Array[i1];

                List_2_Item.Add(List_2_Item_one);
            }

            //
            JavaScriptSerializer Java_Script_Serializer = new JavaScriptSerializer();

            return Java_Script_Serializer.Serialize(List_2_Item);
        }
        public string Creat_JSON_From_List_3_Item(string[] Item_1_Array, string[] Item_2_Array, string[] Item_3_Array)
        {
            List<List_3_Item> List_3_Item = new List<List_3_Item>();

            for (int i1 = 0; i1 < Item_1_Array.Length; i1++)
            {
                List_3_Item List_3_Item_one = new List_3_Item();

                List_3_Item_one.Item_1 = Item_1_Array[i1];
                List_3_Item_one.Item_2 = Item_2_Array[i1];
                List_3_Item_one.Item_3 = Item_3_Array[i1];

                List_3_Item.Add(List_3_Item_one);
            }

            //
            JavaScriptSerializer Java_Script_Serializer = new JavaScriptSerializer();

            return Java_Script_Serializer.Serialize(List_3_Item);
        }
        public string Creat_JSON_From_List_10_Item(string[] Item_1_Array, string[] Item_2_Array, string[] Item_3_Array, string[] Item_4_Array, string[] Item_5_Array, string[] Item_6_Array, string[] Item_7_Array, string[] Item_8_Array, string[] Item_9_Array, string[] Item_10_Array)
        {
            List<List_10_Item> List_10_Item = new List<List_10_Item>();

            for (int i1 = 0; i1 < Item_1_Array.Length; i1++)
            {
                List_10_Item List_10_Item_one = new List_10_Item();

                List_10_Item_one.Item_1 = Item_1_Array[i1];
                List_10_Item_one.Item_2 = Item_2_Array[i1];
                List_10_Item_one.Item_3 = Item_3_Array[i1];
                List_10_Item_one.Item_4 = Item_4_Array[i1];
                List_10_Item_one.Item_5 = Item_5_Array[i1];
                List_10_Item_one.Item_6 = Item_6_Array[i1];
                List_10_Item_one.Item_7 = Item_7_Array[i1];
                List_10_Item_one.Item_8 = Item_8_Array[i1];
                List_10_Item_one.Item_9 = Item_9_Array[i1];
                List_10_Item_one.Item_10 = Item_10_Array[i1];

                List_10_Item.Add(List_10_Item_one);
            }

            //
            JavaScriptSerializer Java_Script_Serializer = new JavaScriptSerializer();

            return Java_Script_Serializer.Serialize(List_10_Item);
        }

        public List_7_Item Convert_JSON_7(string JSON)
        {
            //
            JavaScriptSerializer Java_Script_Serializer = new JavaScriptSerializer();

            return Java_Script_Serializer.Deserialize<List_7_Item>(JSON);
        }
        #endregion

        public string Split_Thousand(string String_Input)
        {
            string Result = string.Empty;

            String_Input = String_Input.Replace(".", string.Empty);

            //Số 64bit
            if (Check_ID(String_Input))
            {
                Result = Remove_0_Before(String.Format("{0:0,0}", Convert.ToInt64(String_Input)).Replace(",", "."));
            }
            else
            {
                if (String_Input.StartsWith("-"))
                {
                    String_Input = Remove_String_First(String_Input, "-");

                    Result = "-" + Remove_0_Before(String.Format("{0:0,0}", Convert.ToInt64(String_Input)).Replace(",", "."));
                }
            }

            if (Result == string.Empty)
            {
                Result = "0";
            }

            return Result;
        }

        public void Read_ALL_Shop_Code_Array(out string[] ALL_Shop_Code_Array, out string[] ALL_Shop_Name_Array)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            //
            ALL_Shop_Code_Array = new string[0];
            ALL_Shop_Name_Array = new string[0];

            //
            Sql_Query = " SELECT SoHopDong, TenGianHang FROM TOPOS_DB.dbo.ThueGianHang";

            //
            Sql_Query = Check_Sql_Query(Sql_Query);
            Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

            try
            {
                while (Sql_Data_Reader.Read())
                {
                    string SoHopDong = Sql_Data_Reader["SoHopDong"].ToString();
                    string TenGianHang = Sql_Data_Reader["TenGianHang"].ToString();

                    SoHopDong = Remove_Space_String(SoHopDong);
                    SoHopDong = Remove_From_String_To_End(SoHopDong, " ");

                    TenGianHang = Remove_Space_String(TenGianHang);

                    if ((TenGianHang != string.Empty) && (SoHopDong != string.Empty))
                    {
                        ALL_Shop_Code_Array = Add_Value_To_Array_String(ALL_Shop_Code_Array, SoHopDong);
                        ALL_Shop_Name_Array = Add_Value_To_Array_String(ALL_Shop_Name_Array, TenGianHang);
                    }
                }
            }
            catch (SqlException Sql_Exception)
            {
            }

            if (!Sql_Data_Reader.IsClosed)
            {
                Sql_Data_Reader.Dispose(); Sql_Command.Dispose(); Sql_Connection.Close(); Sql_Connection.Dispose();
            }

            //
            HttpContext.Current.Application["ALL_Shop_Code_Array"] = ALL_Shop_Code_Array;
            HttpContext.Current.Application["ALL_Shop_Name_Array"] = ALL_Shop_Name_Array;
        }

        public void Delete_Time_Out_File(string Directory_Path, int Minute)
        {
            Directory_Path = Remove_String_Last(Directory_Path, "\\");

            if (Directory.Exists(Directory_Path))
            {
                DirectoryInfo Directory_Info = new DirectoryInfo(Directory_Path);
                FileInfo[] File_Info_Array = Directory_Info.GetFiles();

                foreach (FileInfo File_Info in File_Info_Array)
                {
                    if (Minute > 0)
                    {
                        if (File_Info.CreationTime.CompareTo(DateTime.Now.AddSeconds(-(Minute * 60))) < 0)
                        {
                            Delete_File(File_Info.FullName);
                        }
                    }
                    else
                    {
                        Delete_File(File_Info.FullName);
                    }
                }

                //
                string[] Child_Directory_Array = Directory.GetDirectories(Directory_Path);

                if (Child_Directory_Array.Length > 0)
                {
                    foreach (string Child_Directory in Child_Directory_Array)
                    {
                        if (Directory.Exists(Child_Directory))
                        {
                            if ((File.GetAttributes(Child_Directory) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                            {
                                Delete_Time_Out_File(Child_Directory, Minute);
                            }
                        }
                    }
                }
            }
        }
        public void Delete_All_Empty_Directory(string Root_Directory_Path, string Directory_Path)
        {
            Root_Directory_Path = Remove_String_Last(Root_Directory_Path, "\\");
            Directory_Path = Remove_String_Last(Directory_Path, "\\");

            if (Directory.Exists(Directory_Path))
            {
                string[] Child_Directory_Array = Directory.GetDirectories(Directory_Path);

                //
                if (Child_Directory_Array.Length > 0)
                {
                    foreach (string Child_Directory in Child_Directory_Array)
                    {
                        if (Directory.Exists(Child_Directory))
                        {
                            if ((File.GetAttributes(Child_Directory) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                            {
                                Delete_All_Empty_Directory(Root_Directory_Path, Child_Directory);
                            }
                        }
                    }
                }
                else
                {
                    DirectoryInfo Directory_Info = new DirectoryInfo(Directory_Path);
                    FileInfo[] File_Info_Array = Directory_Info.GetFiles();

                    foreach (FileInfo File_Info in File_Info_Array)
                    {
                        if (File_Info.Name.ToLower() == "thumbs.db")
                        {
                            Delete_File(File_Info.FullName);
                        }
                    }

                    File_Info_Array = Directory_Info.GetFiles();

                    if (File_Info_Array.Length == 0)
                    {
                        if (Directory.Exists(Directory_Path))
                        {
                            if (Directory_Path != Root_Directory_Path)
                            {
                                Directory.Delete(Directory_Path);

                                //
                                Directory_Path = Directory.GetParent(Directory_Path).FullName;

                                if (Directory_Path != Root_Directory_Path)
                                {
                                    Delete_All_Empty_Directory(Root_Directory_Path, Directory_Path);
                                }
                            }
                        }
                    }
                }
            }
        }

        public string Creat_Shop_List(string Shop_Name_OR_Code)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            Shop_Name_OR_Code = Remove_Space_String(Shop_Name_OR_Code);

            if (Shop_Name_OR_Code != "0")
            {
                Sql_Where = " AND ((SoHopDong LIKE '%' + @Shop_Name_OR_Code + '%') OR (TenGianHang LIKE '%' + @Shop_Name_OR_Code + '%'))";
            }

            //UserName
            string UserName = new _4e().Read_UserName();

            string Ho_Va_Ten = string.Empty;
            string Phong_Ban = string.Empty;

            bool Duoc_Xem_Bao_Cao = false;
            bool Duoc_Tich_Diem = false;
            bool Duoc_Tru_Diem = false;
            bool Duoc_Doi_Diem = false;

            new _4e().Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

            //
            string[] MaThue_Array = new string[0];
            string[] Shop_AND_Code_Array = new string[0];
            string[] Tich_Diem_Array = new string[0];

            Sql_Query =
                "SELECT TOP 10 MaThue, SoHopDong, TenGianHang, Tich_Diem"
                + " FROM TOPOS_DB.DBO.ThueGianHang"

                + " WHERE ((Hien_Thi = '1') AND (TenGianHang NOT IN ('0')) AND (TenGianHang IS NOT NULL) AND (TenGianHang NOT LIKE '') AND (NgayKetThuc >= GETDATE())" + Sql_Where + ")"// AND (NgayKetThuc >= GETDATE())

                + " ORDER BY TenGianHang";

            //
            Sql_Query = Check_Sql_Query(Sql_Query);
            Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@Shop_Name_OR_Code", Shop_Name_OR_Code);

            SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

            try
            {
                while (Sql_Data_Reader.Read())
                {
                    MaThue_Array = Add_Value_To_Array_String(MaThue_Array, Sql_Data_Reader["MaThue"].ToString());
                    Shop_AND_Code_Array = Add_Value_To_Array_String(Shop_AND_Code_Array, Sql_Data_Reader["TenGianHang"].ToString() + " (" + Sql_Data_Reader["SoHopDong"].ToString() + ")");
                    Tich_Diem_Array = Add_Value_To_Array_String(Tich_Diem_Array, Convert_String_To_Bit_String(Sql_Data_Reader["Tich_Diem"].ToString()));
                }
            }
            catch (SqlException Sql_Exception)
            {
            }

            if (!Sql_Data_Reader.IsClosed)
            {
                Sql_Data_Reader.Dispose(); Sql_Command.Dispose(); Sql_Connection.Close(); Sql_Connection.Dispose();
            }

            //
            if (("RO0035".ToLower().Contains(Shop_Name_OR_Code.ToLower())) || ("Enteroil HT".ToLower().Contains(Shop_Name_OR_Code.ToLower())))
            {
                MaThue_Array = Add_Value_To_Array_String(MaThue_Array, "Enteroil HT");
                Shop_AND_Code_Array = Add_Value_To_Array_String(Shop_AND_Code_Array, "RO0035");
                Tich_Diem_Array = Add_Value_To_Array_String(Tich_Diem_Array, "1");
            }

            //
            if (Phong_Ban.ToLower() == "CRM".ToLower())
            {
                if (("StarFitness".ToLower().Contains(Shop_Name_OR_Code.ToLower())) || ("Star Fitness".ToLower().Contains(Shop_Name_OR_Code.ToLower())))
                {
                    //MaThue_Array = Add_Value_To_Array_String(MaThue_Array, "AA-StarFitness");
                    //Shop_AND_Code_Array = Add_Value_To_Array_String(Shop_AND_Code_Array, "Star Fitness");
                    //Tich_Diem_Array = Add_Value_To_Array_String(Tich_Diem_Array, "1");
                }
            }

            //
            return Creat_JSON_From_List_3_Item(MaThue_Array, Shop_AND_Code_Array, Tich_Diem_Array);
        }

        public string Read_Receipt_Info(string Receipt)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            //
            bool Have_Result = false;
            string Result = string.Empty;

            //
            string MaQuay = string.Empty;
            string MaNV = string.Empty;

            string MaThue = string.Empty;
            string SoHopDong = string.Empty;
            string TenGianHang = string.Empty;
            string Tich_Diem = string.Empty;

            string NgayBatDau = string.Empty;
            int GioBatDau = 0;

            Int64 Money = 0;
            int Point = 0;

            string Buy_Time_Day = string.Empty;
            string Buy_Time_Month = string.Empty;
            string Buy_Time_Year = string.Empty;

            //
            Receipt = Remove_Space_String(Receipt);

            ////
            //Sql_Query =

            //    " DECLARE @Buy_Time NVARCHAR(MAX)"
            //    + " SET @Buy_Time = CONVERT(DATETIME, @Buy_Time_Temp, 103)"

            //    + " DECLARE @Buy_Date NVARCHAR(MAX)"
            //    + " SET @Buy_Date = CONVERT(DATE, @Buy_Time_Temp, 103)"
            //    + " SET @Buy_Date = REPLACE(@Buy_Date, '-', '/')"

            //    + " IF EXISTS (SELECT * FROM GARDEN_CRM.DBO.T_SALE_INFO WHERE ((RECP_NO = @Receipt) AND (BIZ_DAY = @Buy_Date) AND ((MaThue = @MaThue) OR (BRD_CD IS NOT NULL))))"
            //    + " SELECT 'true'"
            //    + " ELSE"
            //    + " SELECT 'false'"
            //    ;

            ////
            //Sql_Query = Check_Sql_Query(Sql_Query);
            //Get_Sql_Connection(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            //bool EXISTS_Receipt = Convert_String_To_Boolean(Check_Sql_Query_Result(Sql_Command.ExecuteScalar()));

            //if (EXISTS_Receipt)
            //{
            //    Result = "0";
            //}
            //else
            if (Receipt.Length == 16)
            {
                char[] Receipt_Array = Receipt.ToCharArray();

                //
                string Data_Table_Hoa_Don = "HoaDon" + Receipt_Array[8].ToString() + Receipt_Array[9].ToString() + "20" + Receipt_Array[10].ToString() + Receipt_Array[11].ToString();

                //
                if (Check_Exists_Table_POS("TOPOS_DB", Data_Table_Hoa_Don))
                {
                    Sql_Query =
                        "SELECT MaQuay, MaNV, Shop_Alias.MaThue, SoHopDong, TenGianHang, Tich_Diem, NgayBatDau, GioBatDau, SUM(ThanhTienBan) AS Money"
                        + " FROM TOPOS_DB.DBO.ThueGianHang Shop_Alias"

                        + " JOIN"
                        + " ("

                        + " SELECT MaQuay, MaNV, MaThue, NgayBatDau, GioBatDau, TTHD_Alias.ThanhTien AS ThanhTienBan"
                        + " FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don + " HD_Alias"

                        + " JOIN TOPOS_DB.DBO.ThanhToan" + Data_Table_Hoa_Don + " TTHD_Alias"
                        + " ON HD_Alias.MaHD = TTHD_Alias.MaHD"

                        + " WHERE (ID = @Receipt) AND (TTHD_Alias.ThanhTien > 0) AND ((MaHinhThuc = '0001') OR (MaHinhThuc = '0013') OR (MaHinhThuc = '0031') OR (MaNhomThanhToan = '003') OR ((MaHinhThuc IS NULL) AND (MaNhomThanhToan IS NULL)))"

                        + " ) AS Hoa_Don_Alias"
                        + " ON Hoa_Don_Alias.MaThue = Shop_Alias.MaThue"

                        + " WHERE ((Shop_Alias.Hien_Thi = '1') AND (Shop_Alias.TenGianHang NOT IN ('0')) AND (Shop_Alias.TenGianHang IS NOT NULL) AND (Shop_Alias.TenGianHang NOT LIKE ''))"

                        + " GROUP BY MaQuay, MaNV, Shop_Alias.MaThue, SoHopDong, TenGianHang, Tich_Diem, NgayBatDau, GioBatDau"
                        ;

                    //
                    Sql_Query = Check_Sql_Query(Sql_Query);
                    Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    Sql_Command.Parameters.AddWithValue("@Receipt", Receipt);

                    SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

                    try
                    {
                        if (Sql_Data_Reader.Read())
                        {
                            Have_Result = true;

                            MaQuay = Sql_Data_Reader["MaQuay"].ToString();
                            MaNV = Sql_Data_Reader["MaNV"].ToString();

                            MaThue = Sql_Data_Reader["MaThue"].ToString();
                            SoHopDong = Sql_Data_Reader["SoHopDong"].ToString();
                            TenGianHang = Sql_Data_Reader["TenGianHang"].ToString();
                            Tich_Diem = Convert_String_To_Bit_String(Sql_Data_Reader["Tich_Diem"].ToString());

                            NgayBatDau = Sql_Data_Reader["NgayBatDau"].ToString();
                            //GioBatDau = Convert_String_To_Int(Sql_Data_Reader["GioBatDau"].ToString(), 0);

                            string Money_Temp = Sql_Data_Reader["Money"].ToString();
                            Money_Temp = Remove_From_String_To_End(Money_Temp, ".");

                            Money = Convert_String_To_BigInt(Money_Temp, 0);
                        }
                    }
                    catch (SqlException Sql_Exception)
                    {
                    }

                    if (!Sql_Data_Reader.IsClosed)
                    {
                        Sql_Data_Reader.Dispose(); Sql_Command.Dispose(); Sql_Connection.Close(); Sql_Connection.Dispose();
                    }

                    //
                    NgayBatDau = Remove_From_String_To_End(NgayBatDau, " ");

                    string[] NgayBatDau_Array = NgayBatDau.Split('/');

                    if (NgayBatDau_Array.Length == 3)
                    {
                        Buy_Time_Day = NgayBatDau_Array[0];
                        Buy_Time_Month = NgayBatDau_Array[1];
                        Buy_Time_Year = NgayBatDau_Array[2];
                    }

                    //
                    if (Tich_Diem == "1")
                    {
                        Point = Convert.ToInt32(Money / 15000);
                    }
                }
            }

            //
            if (Have_Result)
            {
                string[] MaQuay_Array = new string[] { MaQuay };
                string[] MaNV_Array = new string[] { MaNV };

                string[] MaThue_Array = new string[] { MaThue };
                string[] Shop_AND_Code_Array = new string[] { TenGianHang + " (" + SoHopDong + ")" };
                string[] Tich_Diem_Array = new string[] { Tich_Diem };

                string[] Buy_Time_Day_Array = new string[] { Buy_Time_Day };
                string[] Buy_Time_Month_Array = new string[] { Buy_Time_Month };
                string[] Buy_Time_Year_Array = new string[] { Buy_Time_Year };

                string[] Money_Array = new string[] { Split_Thousand(Money.ToString()) };
                string[] Point_Array = new string[] { Point.ToString() };

                //
                Result = Creat_JSON_From_List_10_Item(MaQuay_Array, MaNV_Array,
                    MaThue_Array, Shop_AND_Code_Array, Tich_Diem_Array,
                    Buy_Time_Day_Array, Buy_Time_Month_Array, Buy_Time_Year_Array,
                    Money_Array, Point_Array
                    );
            }

            //
            return Result;
        }

        public string Read_Card_Info(string Card)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            //
            string Result = string.Empty;
            string Card_Info = string.Empty;
            string Current_Point = string.Empty;

            //
            Card = Remove_Space_String(Card);

            //
            if ((Card.Length == 11) && (Check_ID(Card)) && (!Card.StartsWith("0107")))
            {
                Sql_Query =

                    " DECLARE @MEM_SEQ NVARCHAR(MAX)"
                    + " SET @MEM_SEQ = (SELECT TOP 1 MEM_SEQ FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card))"

                    + " IF @MEM_SEQ IS NOT NULL"
                    + " BEGIN"

                    + "     DECLARE @Total_Point BIGINT"
                    + "     SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"

                    //
                    + "     IF EXISTS (SELECT * FROM GARDEN_CRM.DBO.T_MEM_NOW_PNT WHERE (MEM_SEQ = @MEM_SEQ))"
                    + "         UPDATE GARDEN_CRM.DBO.T_MEM_NOW_PNT SET NOW_TOT_PNT = @Total_Point WHERE (MEM_SEQ = @MEM_SEQ)"
                    + "     ELSE"
                    + "         INSERT INTO GARDEN_CRM.DBO.T_MEM_NOW_PNT (MEM_SEQ, NOW_TOT_PNT) VALUES (@MEM_SEQ, @Total_Point)"

                    //
                    + "     SELECT Mem_Nm AS Name, MEM_BIRTHDAY AS Birthday, YEAR(GETDATE()) - YEAR(Mem_birthday) AS Age, Sex_CD AS Sex, MOBILE_NO AS Phone, E_MAIL AS Email, CERTI_NO as ID, MEM_ADDR AS Address, NOW_TOT_PNT AS Current_Point"
                    + "     FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"

                    + "     LEFT JOIN GARDEN_CRM.DBO.T_MEM_NOW_PNT Member_Point_NOW_Alias"
                    + "     ON Member_Alias.MEM_SEQ = Member_Point_NOW_Alias.MEM_SEQ"

                    + "     WHERE (Member_Alias.MEM_SEQ = @MEM_SEQ)"
                    + " END"
                    ;

                //
                Sql_Query = Check_Sql_Query(Sql_Query);
                Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                Sql_Command.Parameters.Add("@Card", Card);

                SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

                try
                {
                    if (Sql_Data_Reader.Read())
                    {
                        string Name = Sql_Data_Reader["Name"].ToString();
                        string Birthday = Sql_Data_Reader["Birthday"].ToString();
                        string Age = Sql_Data_Reader["Age"].ToString();
                        string Sex = Sql_Data_Reader["Sex"].ToString();
                        string Phone = Sql_Data_Reader["Phone"].ToString();
                        string Email = Sql_Data_Reader["Email"].ToString();
                        string ID = Sql_Data_Reader["ID"].ToString();
                        string Address = Sql_Data_Reader["Address"].ToString();
                        Current_Point = Split_Thousand(Sql_Data_Reader["Current_Point"].ToString());

                        if (Current_Point == string.Empty)
                        {
                            Current_Point = "0";
                        }

                        string[] Birthday_Array = Birthday.Split('/');

                        if (Birthday_Array.Length == 3)
                        {
                            Birthday = Birthday_Array[2] + "-" + Birthday_Array[1] + "-" + Birthday_Array[0];
                        }

                        if (Sex.ToLower() == "f")
                        {
                            Sex = "Nữ";
                        }
                        else
                        {
                            Sex = "Nam";
                        }

                        Card_Info =
                            " - Khách hàng: <span class='Bold_Red_Text_css'>" + Name + "</span><br/>"
                            + " - Tổng điểm hiện tại: <span class='Bold_Red_Text_css'>" + Current_Point + "</span><br/>"
                            + " - Ngày sinh: <span class='Bold_Red_Text_css'>" + Birthday + "</span><br/>"
                            + " - Tuổi: <span class='Bold_Red_Text_css'>" + Age + "</span>"
                            + " - Giới tính: <span class='Bold_Red_Text_css'>" + Sex + "</span><br/>"
                            + " - Phone: <span class='Bold_Red_Text_css'>" + Phone + "</span><br/>"
                            + " - Email: <span class='Bold_Red_Text_css'>" + Email + "</span><br/>"
                            + " - CMND/Hộ chiếu: <span class='Bold_Red_Text_css'>" + ID + "</span><br/>"
                            + " - Đ/C: <span class='Bold_Red_Text_css'>" + Address + "</span>"
                            ;
                    }
                }
                catch (SqlException Sql_Exception)
                {
                }

                if (!Sql_Data_Reader.IsClosed)
                {
                    Sql_Data_Reader.Dispose(); Sql_Command.Dispose(); Sql_Connection.Close(); Sql_Connection.Dispose();
                }
            }

            //
            if (Card_Info != string.Empty)
            {
                string[] Card_Info_Array = new string[] { Card_Info };
                string[] Current_Point_Array = new string[] { Current_Point };

                //
                Result = Creat_JSON_From_List_2_Item(Card_Info_Array, Current_Point_Array);
            }

            //
            return Result;
        }

        public string Submit_Add_Point(string Submit_Add_Point_JSON)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            //
            //string UserId = Read_UserId(string.Empty);

            //
            string Sucessfull = "0";

            try
            {
                //
                //if (Check_GUID(UserId))
                {
                    bool Valid = true;

                    //
                    List_7_Item Submit_Add_Point_obj = Convert_JSON_7(Submit_Add_Point_JSON);

                    //            
                    string Card = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_1, false);
                    string Receipt = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_2, false);
                    string MaThue = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_3, false);
                    string Buy_Time = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_4, false);
                    string Money = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_5, false);
                    string Point = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_6, false);
                    string Reason = Get_Input_String_From_Encoded_HTML_To_SQL(Submit_Add_Point_obj.Item_7, false);

                    Card = Remove_Space_String(Card);
                    Receipt = Remove_Space_String(Receipt);
                    MaThue = Remove_Space_String(MaThue);
                    Buy_Time = Remove_Space_String(Buy_Time);
                    Money = Remove_Space_String(Money);
                    Point = Remove_Space_String(Point);
                    Reason = Remove_Space_String(Reason);

                    Money = Money.Replace(".", string.Empty).Replace(",", string.Empty);
                    Point = Point.Replace(".", string.Empty).Replace(",", string.Empty);

                    //
                    if (
                        (!Check_String_Length(Card, 11, 11))
                        || (!Check_ID(Card))
                        || (Card.StartsWith("0107"))

                        || (!Check_ID(Money))
                        || (!Check_ID(Point))
                        )
                    {
                        Valid = false;
                    }

                    //
                    if (Reason == "Add" || Reason == "Sale")
                    {
                        if (
                            (!Check_String_Is_Not_Null_OR_Space(Receipt))
                            || (!Check_String_Is_Not_Null_OR_Space(MaThue))
                            || (!Check_Date(Buy_Time))
                           )
                        {
                            Valid = false;
                        }
                    }

                    ////False - False - False - False - True - True - True - 
                    //Write_To_File_Temp("\\Error.txt",
                    //    (!Check_String_Length(Card, 11, 11)).ToString() + " - "
                    //    + (!Check_ID(Card)).ToString() + " - "
                    //    + (!Check_ID(Money)).ToString() + " - "
                    //    + (!Check_ID(Point)).ToString() + " - "
                    //    + (!Check_String_Is_Not_Null_OR_Space(Receipt)).ToString() + " - "
                    //    + (!Check_String_Is_Not_Null_OR_Space(MaThue)).ToString() + " - "
                    //    + (!Check_Date(Buy_Time)).ToString() + " - "
                    //    );

                    //
                    if (Valid)
                    {
                        string IP = HttpContext.Current.Request.UserHostAddress;

                        //UserName
                        string UserName = new _4e().Read_UserName();

                        string Ho_Va_Ten = string.Empty;
                        string Phong_Ban = string.Empty;

                        bool Duoc_Xem_Bao_Cao = false;
                        bool Duoc_Tich_Diem = false;
                        bool Duoc_Tru_Diem = false;
                        bool Duoc_Doi_Diem = false;

                        new _4e().Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

                        //
                        string Computer = string.Empty;
                        string Domain = string.Empty;
                        string UserName_Services = string.Empty;

                        string QuayBan = string.Empty;
                        string CaBan = string.Empty;
                        string NVBan = string.Empty;

                        new _4e().Read_Iam(31, out Computer, out Domain, out UserName_Services, out QuayBan, out CaBan, out NVBan);

                        if (QuayBan == string.Empty)
                        {
                            if (Computer != string.Empty)
                            {
                                QuayBan = Computer;
                            }
                            else
                            {
                                QuayBan = IP;
                            }
                        }

                        if ((UserName.ToLower() == "pos2") || (UserName_Services.ToLower() == "pos2"))
                        {
                            if (NVBan == string.Empty)
                            {
                                if (UserName_Services != string.Empty)
                                {
                                    NVBan = UserName_Services;
                                }
                                else
                                {
                                    NVBan = UserName;
                                }
                            }
                        }
                        else
                        {
                            NVBan = UserName;
                        }

                        //
                        string Add_At = QuayBan;
                        string Add_By = NVBan;
                        string Add_Time = Time;

                        string Name = string.Empty;
                        string Total_Point = string.Empty;

                        //
                        if ((UserName.ToLower() == "pos2") || (UserName_Services.ToLower() == "pos2"))
                        {
                            string Cashier_Code = Read_Cashier_Code(NVBan);

                            if (Cashier_Code != string.Empty)
                            {
                                Add_By = Cashier_Code;
                            }
                        }

                        //
                        Sql_Query =

                            " DECLARE @Sale_SEQ NVARCHAR(MAX)"

                            + " DECLARE @Mem_SEQ NVARCHAR(MAX)"
                            + " SET @Mem_SEQ = (SELECT TOP 1 Mem_SEQ FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card))"

                            + " DECLARE @Add_Time NVARCHAR(MAX)"
                            + " SET @Add_Time = CONVERT(DATETIME, @Add_Time_Temp, 103)"

                            + " DECLARE @Name NVARCHAR(MAX)"
                            + " DECLARE @Total_Point BIGINT"

                            //
                            //+ " DELETE FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                            //+ " WHERE SALE_SEQ IN (SELECT SALE_SEQ FROM GARDEN_CRM.DBO.T_SALE_INFO WHERE (MEM_SEQ IS NULL) OR (MEM_SEQ = '0'))"

                            //+ " DELETE FROM GARDEN_CRM.DBO.T_SALE_INFO"
                            //+ " WHERE (MEM_SEQ IS NULL) OR (MEM_SEQ = '0')"

                            + " UPDATE GARDEN_CRM.DBO.T_SALE_INFO"
                            + " SET RECP_NO_Old = RECP_NO, RECP_NO = NULL"
                            + " WHERE (MEM_SEQ IS NULL) OR (MEM_SEQ = '0')"
                            ;

                        //
                        if (Reason == "Add" || Reason == "Sale")
                        {
                            Sql_Query +=

                                " DECLARE @Buy_Time NVARCHAR(MAX)"
                                + " SET @Buy_Time = CONVERT(DATETIME, @Buy_Time_Temp, 103)"

                                + " DECLARE @Buy_Date NVARCHAR(MAX)"
                                + " SET @Buy_Date = CONVERT(DATE, @Buy_Time_Temp, 103)"
                                + " SET @Buy_Date = REPLACE(@Buy_Date, '-', '/')"

                                + " IF NOT EXISTS (SELECT * FROM GARDEN_CRM.DBO.T_SALE_INFO WHERE ((RECP_NO = @Receipt) AND (BIZ_DAY = @Buy_Date) AND ((MaThue = @MaThue) OR (BRD_CD IS NOT NULL))))"
                                + " BEGIN"

                                + "     INSERT INTO GARDEN_CRM.DBO.T_SALE_INFO"
                                + "     (MEM_SEQ, RECP_NO, MaThue, BIZ_DAY, SALE_DT, PAY_AMT, ADD_PNT, POS_NO, CASHER_NO, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, @Receipt, @MaThue, @Buy_Date, @Buy_Time, @Money, @Point, @Add_At, @Add_By, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"

                                + "     SET @Sale_SEQ = (SELECT SCOPE_IDENTITY());"

                                //
                                + "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, @Point, @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"

                                + " END"
                                ;
                        }
                        else
                            if (Reason == "Mistake")
                        {
                            Sql_Query +=

                                " SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"
                                + " IF (@Point <= @Total_Point)"
                                + " BEGIN"
                                + "     DECLARE @Buy_Time NVARCHAR(MAX)"
                                + "     SET @Buy_Time = CONVERT(DATETIME, @Buy_Time_Temp, 103)"

                                + "     DECLARE @Buy_Date NVARCHAR(MAX)"
                                + "     SET @Buy_Date = CONVERT(DATE, @Buy_Time_Temp, 103)"
                                + "     SET @Buy_Date = REPLACE(@Buy_Date, '-', '/')"

                                + "     SET @Sale_SEQ = (SELECT TOP 1 Sale_SEQ FROM GARDEN_CRM.DBO.T_SALE_INFO WHERE ((RECP_NO = @Receipt) AND (BIZ_DAY = @Buy_Date) AND ((MaThue = @MaThue) OR (BRD_CD IS NOT NULL))))"

                                + "     IF @Sale_SEQ IS NULL"
                                + "     BEGIN"
                                + "         INSERT INTO GARDEN_CRM.DBO.T_SALE_INFO"
                                + "         (MEM_SEQ, RECP_NO, MaThue, BIZ_DAY, SALE_DT, PAY_AMT, ADD_PNT, POS_NO, CASHER_NO, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "         VALUES"
                                + "         (@MEM_SEQ, @Receipt, @MaThue, @Buy_Date, @Buy_Time, @Money, @Point, @Add_At, @Add_By, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"

                                + "         SET @Sale_SEQ = (SELECT SCOPE_IDENTITY());"
                                + "     END"

                                + "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, '-' + CONVERT(NVARCHAR(MAX), @Point), @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"
                                + " END"
                                ;
                        }
                        else
                                if (Reason.StartsWith("Minus"))
                        {
                            Sql_Query +=
                                " SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"

                                + " IF (@Point <= @Total_Point)"
                                + "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, '-' + CONVERT(NVARCHAR(MAX), @Point), @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"
                                ;
                        }
                        else
                                    if (Reason.StartsWith("Reward"))
                        {
                            Sql_Query +=
                                "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, @Point, @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"
                                ;
                        }
                        else
                                        if (Reason == "Redeem")
                        {
                            Sql_Query +=
                                " SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"

                                + " IF (@Point <= @Total_Point)"
                                + "     INSERT INTO GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                                + "     (MEM_SEQ, CHG_PNT, PNT_RSN, SALE_SEQ, Add_At, Add_By_Group, REG_ID, REG_DT, MOD_ID, MOD_DT, Is_Manual_Add_Point)"
                                + "     VALUES"
                                + "     (@MEM_SEQ, '-' + CONVERT(NVARCHAR(MAX), @Point), @Reason, @Sale_SEQ, @Add_At, @Add_By_Group, @Add_By, @Add_Time, @Add_By, @Add_Time, @Is_Manual_Add_Point)"
                                ;
                        }

                        //
                        Sql_Query +=

                                " SET @Total_Point = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @MEM_SEQ))"

                                //
                                + " IF EXISTS (SELECT * FROM GARDEN_CRM.DBO.T_MEM_NOW_PNT WHERE (MEM_SEQ = @MEM_SEQ))"
                                + "     UPDATE GARDEN_CRM.DBO.T_MEM_NOW_PNT SET NOW_TOT_PNT = @Total_Point WHERE (MEM_SEQ = @MEM_SEQ)"
                                + " ELSE"
                                + "     INSERT INTO GARDEN_CRM.DBO.T_MEM_NOW_PNT (MEM_SEQ, NOW_TOT_PNT) VALUES (@MEM_SEQ, @Total_Point)"

                                //
                                + " SET @Name = (SELECT Mem_Nm AS Name FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card))"

                                + " SELECT @Name AS Name, @Total_Point AS Total_Point"
                                ;

                        //
                        Sql_Query = Check_Sql_Query(Sql_Query);
                        Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                        //Sql_Command.Parameters.Add("@UserId", UserId);

                        Sql_Command.Parameters.Add("@Card", Card);
                        Sql_Command.Parameters.Add("@Receipt", Receipt);
                        Sql_Command.Parameters.Add("@MaThue", MaThue);
                        Sql_Command.Parameters.Add("@Buy_Time_Temp", Buy_Time);
                        Sql_Command.Parameters.Add("@Money", Money);
                        Sql_Command.Parameters.Add("@Point", Point);
                        Sql_Command.Parameters.Add("@Reason", Reason);

                        Sql_Command.Parameters.Add("@Add_At", Add_At);
                        Sql_Command.Parameters.Add("@Add_By", Add_By);
                        Sql_Command.Parameters.Add("@Add_By_Group", Phong_Ban);

                        Sql_Command.Parameters.Add("@Add_Time_Temp", Add_Time);

                        Sql_Command.Parameters.Add("@Is_Manual_Add_Point", "1");

                        SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

                        try
                        {
                            if (Sql_Data_Reader.Read())
                            {
                                Name = Sql_Data_Reader["Name"].ToString();
                                Total_Point = Sql_Data_Reader["Total_Point"].ToString();
                            }
                        }
                        catch (SqlException Sql_Exception)
                        {
                            Write_To_File_Temp("\\Error-SQL.txt", Sql_Exception.Message.ToString());
                        }

                        if (!Sql_Data_Reader.IsClosed)
                        {
                            Sql_Data_Reader.Dispose(); Sql_Command.Dispose(); Sql_Connection.Close(); Sql_Connection.Dispose();
                        }

                        //
                        string Reason_Friendy_Name = string.Empty;

                        //
                        if (Reason == "Add")
                        {
                            Reason_Friendy_Name = "Tích điểm";
                        }
                        else
                            if (Reason == "Mistake")
                        {
                            Reason_Friendy_Name = "Trừ điểm tích nhầm";
                        }
                        else
                                if (Reason.StartsWith("Minus"))
                        {
                            Reason_Friendy_Name = "Trừ điểm";
                        }
                        else
                                    if (Reason.StartsWith("Reward"))
                        {
                            Reason_Friendy_Name = "Thưởng điểm";
                        }
                        else
                                        if (Reason == "Redeem")
                        {
                            Reason_Friendy_Name = "Đổi điểm lấy Voucher";
                        }
                        else if (Reason == "Sale")
                        {
                            Reason_Friendy_Name = "Tích doanh số";
                        }

                        //
                        Point = Split_Thousand(Point);
                        Total_Point = Split_Thousand(Total_Point);

                        if (Point == string.Empty)
                        {
                            Point = "0";
                        }

                        if (Total_Point == string.Empty)
                        {
                            Total_Point = "0";
                        }

                        //
                        if (Reason == "Add")
                        {
                            if (Point != "0")
                            {
                                Sucessfull =
                                    "- Khách hàng: " + Name
                                    + "\n- Số thẻ: " + Card

                                    + "\n\n- Hóa đơn: " + Receipt
                                    + "\n- Tiền: " + Split_Thousand(Money) + " VND"
                                    + "\n\n- Điểm: " + Point
                                    + "\n- Thao tác: " + Reason_Friendy_Name

                                    + "\n\n- Tổng điểm hiện tại: " + Total_Point
                                    ;
                            }
                            else
                            {
                                Sucessfull =
                                    "- Khách hàng: " + Name
                                    + "\n- Số thẻ: " + Card

                                    + "\n\n- Hóa đơn: " + Receipt
                                    + "\n- Tiền: " + Split_Thousand(Money) + " VND"

                                    + "\n\n- Tổng điểm hiện tại: " + Total_Point
                                    ;
                            }
                        }
                        else if (Reason == "Sale")
                        {
                            Sucessfull =
                                "- Khách hàng: " + Name
                                                 + "\n- Số thẻ: " + Card

                                                 + "\n\n- Hóa đơn: " + Receipt
                                                 + "\n- Tiền: " + Split_Thousand(Money) + " VND"
                                                 + "\n\n- Điểm: 0"
                                                 + "\n- Thao tác: " + Reason_Friendy_Name

                                                 + "\n\n- Tổng điểm hiện tại: " + Total_Point
                                ;
                        }
                        else
                        {
                            Sucessfull =
                                "- Khách hàng: " + Name
                                + "\n- Số thẻ: " + Card

                                + "\n\n- Điểm: " + Point
                                + "\n- Thao tác: " + Reason_Friendy_Name

                                + "\n\n- Tổng điểm hiện tại: " + Total_Point
                                ;
                        }
                    }
                }
            }
            catch (Exception EX)
            {
                Write_To_File_Temp("\\Error.txt", EX.Message.ToString());
            }

            //
            return Sucessfull;
        }

        public void Iam_ashx_ProcessRequest()
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            //
            string IP = HttpContext.Current.Request.UserHostAddress;

            string Computer = Determine_Query_String_Text(string.Empty, "Computer");
            string Domain = Determine_Query_String_Text(string.Empty, "Domain");
            string UserName = Determine_Query_String_Text(string.Empty, "UserName");

            string QuayBan = Determine_Query_String_Text(string.Empty, "QuayBan");
            string CaBan = Determine_Query_String_Text(string.Empty, "CaBan");
            string NVBan = Determine_Query_String_Text(string.Empty, "NVBan");

            string Table = Determine_Query_String_Text(string.Empty, "Table");
            string NgayThang = Determine_Query_String_Text(string.Empty, "NgayThang");

            string Check = Determine_Query_String_Text(string.Empty, "Check");

            Computer = UpperCase_First_Character(Computer);
            Domain = UpperCase_First_Character(Domain);
            UserName = UpperCase_First_Character(UserName);

            HttpContext.Current.Application["Computer_Of_" + IP] = Computer;
            HttpContext.Current.Application["Domain_Of_" + IP] = Domain;
            HttpContext.Current.Application["UserName_Of_" + IP] = UserName;

            HttpContext.Current.Application["QuayBan_Of_" + IP] = QuayBan;
            HttpContext.Current.Application["CaBan_Of_" + IP] = CaBan;
            HttpContext.Current.Application["NVBan_Of_" + IP] = NVBan;

            //
            string RowIDList = string.Empty;

            //
            if ((Check == "Failed") || (Check == "Uploaded"))
            {
                if (QuayBan.ToLower().StartsWith("aa"))
                {
                    if (Check_Exists_Table_POS("TOPOS_DB", Table))
                    {
                        if (Check == "Failed")
                        {
                            Sql_Query =
                                " USE TOPOS_DB"

                                + " INSERT INTO HoaDon0Dong"
                                + " SELECT * FROM " + Table
                                + " WHERE ((RowID IS NOT NULL) AND ((DaIn = 0) OR (LoaiHoaDon = 0)) AND (CONVERT(DATETIME, NgayBatDau, 103) = CONVERT(DATETIME, @NgayThang, 103)) AND (RowID NOT IN (SELECT RowID FROM HoaDon0Dong WHERE (RowID IS NOT NULL))))"

                                + " DELETE " + Table + " WHERE ((RowID IS NOT NULL) AND (RowID IN (SELECT RowID FROM HoaDon0Dong WHERE (RowID IS NOT NULL))))"
                                + " SELECT RowID FROM HoaDon0Dong WHERE ((RowID IS NOT NULL) AND (CONVERT(DATETIME, NgayBatDau, 103) = CONVERT(DATETIME, @NgayThang, 103)))"
                                ;
                        }
                        else
                        if (Check == "Uploaded")
                        {
                            if (Table.StartsWith("HoaDon"))
                            {
                                Sql_Query =
                                    " SELECT RowID"
                                    + " FROM TOPOS_DB.DBO." + Table
                                    + " WHERE ((RowID IS NOT NULL) AND (DaIn = 1) AND (LoaiHoaDon = 1) AND (MaQuay LIKE @MaQuay) AND (CONVERT(DATETIME, NgayBatDau, 103) = CONVERT(DATETIME, @NgayThang, 103)))"
                                    ;
                            }
                            else
                                if ((Table.StartsWith("CTHoaDon")) || (Table.StartsWith("ThanhToanHoaDon")))
                            {
                                string tableHoaDon = Remove_String_First(Table, "CT");
                                tableHoaDon = Remove_String_First(tableHoaDon, "ThanhToan");

                                Sql_Query =
                                    " SELECT RowID"
                                    + " FROM TOPOS_DB.DBO." + Table

                                    + " WHERE ((RowID IS NOT NULL) AND (MaHD IN (SELECT MaHD FROM TOPOS_DB.DBO." + tableHoaDon + " WHERE ((RowID IS NOT NULL) AND (DaIn = 1) AND (LoaiHoaDon = 1) AND (MaQuay LIKE @MaQuay) AND (CONVERT(DATETIME, NgayBatDau, 103) = CONVERT(DATETIME, @NgayThang, 103))))))"
                                    ;
                            }
                        }

                        //
                        if (Sql_Query != string.Empty)
                        {
                            Sql_Query = Check_Sql_Query(Sql_Query);
                            Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                            Sql_Command.Parameters.AddWithValue("@MaQuay", QuayBan);
                            Sql_Command.Parameters.AddWithValue("@NgayThang", NgayThang);

                            SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

                            try
                            {
                                while (Sql_Data_Reader.Read())
                                {
                                    RowIDList += Sql_Data_Reader["RowID"].ToString() + "#";
                                }
                            }
                            catch (SqlException Ex)
                            {
                                //Log_Error(Sql_Exception.ToString());                                

                                bool Log_Error = true;

                                //
                                if (
                                    (Ex.ToString().Contains("cannot be autostarted during server shutdown or startup"))
                                    || (Ex.ToString().Contains("SHUTDOWN is in progress."))

                                    || (Ex.ToString().Contains("Waiting until recovery is finished."))

                                    || (Ex.ToString().Contains("error: 40 - Could not open a connection to SQL Server"))
                                    || (Ex.ToString().Contains("error: 0 - An existing connection was forcibly closed by the remote host"))
                                    )
                                {
                                    Log_Error = false;
                                }

                                //
                                if (Log_Error)
                                {
                                    Write_To_File("\\Error\\" + Get_Time_Name_Code() + ".txt", Ex.ToString() + Environment.NewLine + Environment.NewLine);
                                }
                            }

                            if (!Sql_Data_Reader.IsClosed)
                            {
                                Sql_Data_Reader.Dispose(); Sql_Command.Dispose(); Sql_Connection.Close(); Sql_Connection.Dispose();
                            }
                        }
                    }
                }
            }

            HttpContext.Current.Response.Write(RowIDList);
        }

        public void Read_Iam(int Try_Times, out string Computer, out string Domain, out string UserName, out string QuayBan, out string CaBan, out string NVBan)
        {
            Try_Times++;

            Computer = string.Empty;
            Domain = string.Empty;
            UserName = string.Empty;

            QuayBan = string.Empty;
            CaBan = string.Empty;
            NVBan = string.Empty;

            string IP = HttpContext.Current.Request.UserHostAddress;

            if (
                (HttpContext.Current.Application["Computer_Of_" + IP] != null)
                && (HttpContext.Current.Application["Domain_Of_" + IP] != null)
                && (HttpContext.Current.Application["UserName_Of_" + IP] != null)
                && (HttpContext.Current.Application["QuayBan_Of_" + IP] != null)
                && (HttpContext.Current.Application["CaBan_Of_" + IP] != null)
                && (HttpContext.Current.Application["NVBan_Of_" + IP] != null)
                )
            {
                Computer = HttpContext.Current.Application["Computer_Of_" + IP].ToString();
                Domain = HttpContext.Current.Application["Domain_Of_" + IP].ToString();
                UserName = HttpContext.Current.Application["UserName_Of_" + IP].ToString();

                QuayBan = HttpContext.Current.Application["QuayBan_Of_" + IP].ToString();
                CaBan = HttpContext.Current.Application["CaBan_Of_" + IP].ToString();
                NVBan = HttpContext.Current.Application["NVBan_Of_" + IP].ToString();
            }
            else
            {
                if (Try_Times <= 30)
                {
                    Thread.Sleep(1000 * 1);
                    Read_Iam(Try_Times, out Computer, out Domain, out UserName, out QuayBan, out CaBan, out NVBan);
                }
            }
        }

        public string Read_Iam_Info()
        {
            string UserName = new _4e().Read_UserName();

            //
            string Computer = string.Empty;
            string Domain = string.Empty;
            string UserName_Services = string.Empty;

            string QuayBan = string.Empty;
            string CaBan = string.Empty;
            string NVBan = string.Empty;

            new _4e().Read_Iam(31, out Computer, out Domain, out UserName_Services, out QuayBan, out CaBan, out NVBan);

            if (QuayBan == string.Empty)
            {
                //QuayBan = Computer;
            }

            if ((UserName.ToLower() == "pos2") || (UserName_Services.ToLower() == "pos2"))
            {
                if (NVBan == string.Empty)
                {
                    if (UserName_Services != string.Empty)
                    {
                        NVBan = UserName_Services;
                    }
                    else
                    {
                        NVBan = UserName;
                    }
                }
            }
            else
            {
                NVBan = UserName;
            }

            string[] QuayBan_Array = new string[] { QuayBan };
            string[] NVBan_Array = new string[] { NVBan };

            //
            return Creat_JSON_From_List_2_Item(QuayBan_Array, NVBan_Array);
        }

        public void Read_User_Phan_Quyen(string UserName, out string Ho_Va_Ten, out string Phong_Ban, out bool Result_Duoc_Xem_Bao_Cao, out bool Result_Duoc_Tich_Diem, out bool Result_Duoc_Tru_Diem, out bool Result_Duoc_Doi_Diem)
        {
            UserName = UserName.ToLower();

            Ho_Va_Ten = "Garden";
            Phong_Ban = string.Empty;

            Result_Duoc_Xem_Bao_Cao = false;
            Result_Duoc_Tich_Diem = false;
            Result_Duoc_Tru_Diem = false;
            Result_Duoc_Doi_Diem = false;

            //Test
            if (HttpContext.Current.Request.IsLocal)
            {
                Ho_Va_Ten = "Ngô Vương Quyền";
                Phong_Ban = "IT";

                Result_Duoc_Xem_Bao_Cao = true;
                Result_Duoc_Tich_Diem = true;
                Result_Duoc_Tru_Diem = true;
                Result_Duoc_Doi_Diem = true;
            }
            else
            {
                bool MUST_Update = false;

                string File_Path = @"F:\Websites\Garden\File_Upload\User-Phan-Quyen.xlsx";
                DateTime LastWriteTime = File.GetLastWriteTime(File_Path);

                DateTime Last_LastWriteTime = DateTime.Now;

                if (HttpContext.Current.Application["GetLastWriteTime_Of_User-Phan-Quyen"] != null)
                {
                    Last_LastWriteTime = (DateTime)HttpContext.Current.Application["GetLastWriteTime_Of_User-Phan-Quyen"];
                }

                if (LastWriteTime != Last_LastWriteTime)
                {
                    MUST_Update = true;
                    HttpContext.Current.Application["GetLastWriteTime_Of_User-Phan-Quyen"] = LastWriteTime;
                }

                //
                if (
                    !MUST_Update
                    && (HttpContext.Current.Application["Ho_Va_Ten_Of_" + UserName] != null)
                    && (HttpContext.Current.Application["Phong_Ban_Of_" + UserName] != null)

                    && (HttpContext.Current.Application["Duoc_Xem_Bao_Cao_Of_" + UserName] != null)
                    && (HttpContext.Current.Application["Duoc_Tich_Diem_Of_" + UserName] != null)
                    && (HttpContext.Current.Application["Duoc_Tru_Diem_Of_" + UserName] != null)
                    && (HttpContext.Current.Application["Duoc_Doi_Diem_Of_" + UserName] != null)
                    )
                {
                    Ho_Va_Ten = HttpContext.Current.Application["Ho_Va_Ten_Of_" + UserName].ToString();
                    Phong_Ban = HttpContext.Current.Application["Phong_Ban_Of_" + UserName].ToString();

                    Result_Duoc_Xem_Bao_Cao = (bool)HttpContext.Current.Application["Duoc_Xem_Bao_Cao_Of_" + UserName];
                    Result_Duoc_Tich_Diem = (bool)HttpContext.Current.Application["Duoc_Tich_Diem_Of_" + UserName];
                    Result_Duoc_Tru_Diem = (bool)HttpContext.Current.Application["Duoc_Tru_Diem_Of_" + UserName];
                    Result_Duoc_Doi_Diem = (bool)HttpContext.Current.Application["Duoc_Doi_Diem_Of_" + UserName];
                }
                else
                {
                    FileInfo Input_File_Info = new FileInfo(File_Path);
                    ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

                    //
                    var Input_Work_sheets = Input_Excel_Package.Workbook.Worksheets[1];

                    //
                    for (int i1 = 8; i1 <= Input_Work_sheets.Dimension.End.Row; i1++)
                    {
                        string Ten_Dang_Nhap = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 3].Value).ToLower();

                        if (Ten_Dang_Nhap == UserName)
                        {
                            Ho_Va_Ten = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 2].Value);
                            Phong_Ban = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 1].Value);

                            string Duoc_Xem_Bao_Cao = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 4].Value).ToLower();
                            string Duoc_Tich_Diem = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 5].Value).ToLower();
                            string Duoc_Tru_Diem = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 6].Value).ToLower();
                            string Duoc_Doi_Diem = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 7].Value).ToLower();

                            //
                            Ho_Va_Ten = Remove_Space_String(Ho_Va_Ten);
                            HttpContext.Current.Application["Ho_Va_Ten_Of_" + UserName] = Ho_Va_Ten;

                            //
                            Duoc_Xem_Bao_Cao = Remove_Space_String(Duoc_Xem_Bao_Cao);

                            if (Duoc_Xem_Bao_Cao == "x")
                            {
                                Result_Duoc_Xem_Bao_Cao = true;
                                HttpContext.Current.Application["Duoc_Xem_Bao_Cao_Of_" + UserName] = true;
                            }
                            else
                            {
                                HttpContext.Current.Application["Duoc_Xem_Bao_Cao_Of_" + UserName] = false;
                            }

                            //
                            Duoc_Tich_Diem = Remove_Space_String(Duoc_Tich_Diem);

                            if (Duoc_Tich_Diem == "x")
                            {
                                Result_Duoc_Tich_Diem = true;
                                HttpContext.Current.Application["Duoc_Tich_Diem_Of_" + UserName] = true;
                            }
                            else
                            {
                                HttpContext.Current.Application["Duoc_Tich_Diem_Of_" + UserName] = false;
                            }

                            //
                            Duoc_Tru_Diem = Remove_Space_String(Duoc_Tru_Diem);

                            if (Duoc_Tru_Diem == "x")
                            {
                                Result_Duoc_Tru_Diem = true;
                                HttpContext.Current.Application["Duoc_Tru_Diem_Of_" + UserName] = true;
                            }
                            else
                            {
                                HttpContext.Current.Application["Duoc_Tru_Diem_Of_" + UserName] = false;
                            }

                            //
                            Duoc_Doi_Diem = Remove_Space_String(Duoc_Doi_Diem);

                            if (Duoc_Doi_Diem == "x")
                            {
                                Result_Duoc_Doi_Diem = true;
                                HttpContext.Current.Application["Duoc_Doi_Diem_Of_" + UserName] = true;
                            }
                            else
                            {
                                HttpContext.Current.Application["Duoc_Doi_Diem_Of_" + UserName] = false;
                            }

                            //
                            break;
                        }
                    }
                }
            }
        }

        public void UPDATE_List_Shop_Add_Point_To_DataBase()
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            bool MUST_Update = false;

            string File_Path = @"F:\Websites\Garden\File_Upload\Shop_Code_23052016.xlsx";
            DateTime LastWriteTime = File.GetLastWriteTime(File_Path);

            DateTime Last_LastWriteTime = DateTime.Now;

            if (HttpContext.Current.Application["GetLastWriteTime_Of_Shop_Code"] != null)
            {
                Last_LastWriteTime = (DateTime)HttpContext.Current.Application["GetLastWriteTime_Of_Shop_Code"];
            }

            if (LastWriteTime != Last_LastWriteTime)
            {
                MUST_Update = true;
                HttpContext.Current.Application["GetLastWriteTime_Of_Shop_Code"] = LastWriteTime;
            }

            //
            if (MUST_Update)
            {
                Delete_File("\\Sai_Shop_Code.txt");

                FileInfo Input_File_Info = new FileInfo(File_Path);
                ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

                //
                var Input_Work_sheets = Input_Excel_Package.Workbook.Worksheets[1];

                //
                string Sql_Where_Hien_Thi = string.Empty;
                string Sql_Where_Tich_Diem = string.Empty;

                //
                for (int i1 = 7; i1 <= Input_Work_sheets.Dimension.End.Row; i1++)
                {
                    string Duoc_Tich_Diem = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 10].Value).ToLower();
                    string Shop_Code = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 3].Value).ToLower();
                    string Shop_Name = new _4e().Object_ToString(Input_Work_sheets.Cells[i1, 4].Value).ToLower();

                    Shop_Code = Remove_Space_String(Shop_Code);
                    //Shop_Code = Remove_String_Last(Shop_Code, ".");
                    //Shop_Code = Remove_String_Last(Shop_Code, "-");

                    if (Shop_Code != string.Empty)
                    {
                        Sql_Where_Hien_Thi += "'" + Shop_Code + "', ";

                        if (Duoc_Tich_Diem == "ok")
                        {
                            Sql_Where_Tich_Diem += "'" + Shop_Code + "', ";
                        }

                        //string Sql_Query_Check_Exists =
                        //    "IF EXISTS (SELECT * FROM TOPOS_DB.DBO.ThueGianHang WHERE (SoHopDong LIKE '" + Shop_Code + "'))"
                        //    + " SELECT 'true'"

                        //    + " ELSE"
                        //    + " SELECT 'false'"
                        //    ;

                        //Sql_Query_Check_Exists = Check_Sql_Query(Sql_Query_Check_Exists);
                        //Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query_Check_Exists, Sql_Connection); Sql_Command.CommandTimeout = 0;

                        //if (!Convert_String_To_Boolean(Check_Sql_Query_Result(Sql_Command.ExecuteScalar())))
                        //{
                        //    Write_To_File_Temp("\\Sai_Shop_Code.txt", i1.ToString() + " = " + Shop_Code + " : " + Shop_Name);
                        //}
                    }
                }

                //
                Sql_Where_Hien_Thi = Remove_String_Last(Sql_Where_Hien_Thi, ",");

                if (Sql_Where_Hien_Thi != string.Empty)
                {
                    Sql_Query +=
                       //"UPDATE TOPOS_DB.DBO.ThueGianHang"
                       //+ " SET Hien_Thi = NULL"

                       " UPDATE TOPOS_DB.DBO.ThueGianHang"
                       + " SET Hien_Thi = '1'"
                       + " WHERE (SoHopDong IN (" + Sql_Where_Hien_Thi + "))"
                       ;
                }

                //
                Sql_Where_Tich_Diem = Remove_String_Last(Sql_Where_Tich_Diem, ",");

                if (Sql_Where_Tich_Diem != string.Empty)
                {
                    Sql_Query +=
                       "UPDATE TOPOS_DB.DBO.ThueGianHang"
                       + " SET Tich_Diem = NULL"

                       + " UPDATE TOPOS_DB.DBO.ThueGianHang"
                       + " SET Tich_Diem = '1'"
                       + " WHERE (SoHopDong IN (" + Sql_Where_Tich_Diem + "))"
                       ;
                }

                //
                Sql_Query = Check_Sql_Query(Sql_Query);

                if (Sql_Query != string.Empty)
                {
                    Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;
                    Sql_Command.ExecuteNonQuery();
                }
            }
        }

        public string Creat_Search_Name_List(string Search_Name_OR_Phone)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            Search_Name_OR_Phone = Remove_Space_String(Search_Name_OR_Phone);

            //
            string[] Name_Array = new string[0];
            string[] Phone_Array = new string[0];
            string[] Card_Array = new string[0];

            Sql_Query =
                " SELECT TOP 10 Mem_Nm AS Name, MOBILE_NO AS Phone, Mem_Card AS Card"
                + " FROM GARDEN_CRM.DBO.T_MEM_MST"

                + " WHERE ("
                + " (Mem_Card NOT LIKE '0107%') AND (Mem_Card NOT LIKE '07%') AND (Mem_Card NOT LIKE '0108%') AND (Mem_Card NOT LIKE '08%') AND (Mem_Card NOT LIKE '-%')"
                + " AND ((Mem_Nm LIKE '%' + @Search_Name_OR_Phone + '%') OR (REPLACE(MOBILE_NO, '-', '') LIKE '%' + @Search_Name_OR_Phone + '%') OR (REPLACE(CERTI_NO, '-', '') LIKE '%' + @Search_Name_OR_Phone + '%'))"
                + ")"
                ;

            //
            Sql_Query = Check_Sql_Query(Sql_Query);
            Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@Search_Name_OR_Phone", Search_Name_OR_Phone);

            SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

            try
            {
                while (Sql_Data_Reader.Read())
                {
                    Name_Array = Add_Value_To_Array_String(Name_Array, Sql_Data_Reader["Name"].ToString());
                    Phone_Array = Add_Value_To_Array_String(Phone_Array, Sql_Data_Reader["Phone"].ToString());
                    Card_Array = Add_Value_To_Array_String(Card_Array, Sql_Data_Reader["Card"].ToString());
                }
            }
            catch (SqlException Sql_Exception)
            {
            }

            if (!Sql_Data_Reader.IsClosed)
            {
                Sql_Data_Reader.Dispose(); Sql_Command.Dispose(); Sql_Connection.Close(); Sql_Connection.Dispose();
            }

            //
            return Creat_JSON_From_List_3_Item(Name_Array, Phone_Array, Card_Array);
        }

        public string Read_Cashier_Code(string Cashier_UserName)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            //
            Sql_Query =
                " SELECT TOP 1 MaNV"
                + " FROM TOPOS_DB.DBO.NhanVien"

                + " WHERE (TenDangNhap LIKE @Cashier_UserName)"
                ;

            //
            Sql_Query = Check_Sql_Query(Sql_Query);
            Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@Cashier_UserName", Cashier_UserName);

            //
            return Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
        }

        public void Update_ashx_ProcessRequest()
        {
            string Domain = Add_http_To_URL(Read_Domain(string.Empty));

            string Update_For = Determine_Query_String_Text(string.Empty, "Software");

            string Update = string.Empty;

            if (Update_For == "POS_Updater")
            {
                Update = "2017.04.04_16h30m0s" + Environment.NewLine

                    + "Run_Console=C:\\Client\\System\\Update.exe#Arguments=-uninstall" + Environment.NewLine
                    + "Download=" + Domain + "/Software/POS_Services/Update.exe#Save=C:\\Client\\System\\Update.exe" + Environment.NewLine
                    + "Run_Console=C:\\Client\\System\\Update.exe#Arguments=-install" + Environment.NewLine
                    ;
            }
            else
            if (Update_For == "POS_Services")
            {
                bool Update_Run_GUI = false;

                Update = "2018.04.03_10h00m0s" + Environment.NewLine

                    + "Run_Console=C:\\Client\\System\\svchost.exe#Arguments=-uninstall" + Environment.NewLine
                    + "Download=" + Domain + "/Software/POS_Services/svchost.exe#Save=C:\\Client\\System\\svchost.exe" + Environment.NewLine
                    + "Run_Console=C:\\Client\\System\\svchost.exe#Arguments=-install" + Environment.NewLine

                    //+ "Download=" + Domain + "/Software/POS_Services/Registry.reg#Save=C:\\Client\\System\\Registry.reg" + Environment.NewLine
                    //+ "Run_Console=C:\\Windows\\Regedit.exe#Arguments=/s C:\\Client\\System\\Registry.reg" + Environment.NewLine

                    + "Kill=POS_Start.exe" + Environment.NewLine
                    + "Download=" + Domain + "/Software/POS_Services/POS_Start.exe#Save=C:\\Client\\System\\POS_Start.exe" + Environment.NewLine

                    + "Kill=POS_Member.exe" + Environment.NewLine
                    ;

                //
                if (Update_Run_GUI)
                {
                    //Update +=
                    //    "Kill=Windows_Audio.exe" + Environment.NewLine
                    //    + "Download=" + Domain + "/Software/POS_Services/Windows_Audio.exe#Save=C:\\Client\\System\\Windows_Audio.exe" + Environment.NewLine
                    //    + "Run_GUI=C:\\Client\\System\\Windows_Audio.exe" + Environment.NewLine
                    //    ;
                }
                else
                {
                    Update +=
                        "Kill=TOPOSUpdater.exe" + Environment.NewLine
                        + "Download=" + Domain + "/Software/POS_Services/POS_Open.exe#Save=C:\\Client\\TOPOSUpdater.exe" + Environment.NewLine
                        + "Run_GUI=C:\\Client\\TOPOSUpdater.exe" + Environment.NewLine
                        ;
                }
            }

            HttpContext.Current.Response.Write(Update);
        }

        public void Upload_ashx_ProcessRequest()
        {
            string[] NoNullColumn = new string[] {
                "CaBan", "DaIn", "DGBan", "LoaiHoaDon", "MaHD", "MaHH", "MaHinhThuc", "MaNccDM", "MaNhomThanhToan",
                "MaNV", "MaQuay", "MaThe", "MaThue", "SoLuong", "STT", "TenNVBanHang", "ThanhTien", "ThanhTienBan", "ThanhTienQuiDoi",
                "TienCK", "TienCKHD", "TienGiamGia", "TLCK1", "TLCK2", "TLCKGiamGia", "TLCKHD", "TriGiaBan" };


            string Type = Determine_Query_String_Text(string.Empty, "Upload_For");

            //
            if (Type == "Hoa_Don_POS")
            {
                for (int i1 = 0; i1 < HttpContext.Current.Request.Files.Count; i1++)
                {
                    HttpPostedFile Http_Posted_File = HttpContext.Current.Request.Files.Get(i1);

                    //
                    using (TextReader reader = new StreamReader(Http_Posted_File.InputStream, Encoding.Unicode))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            string Table = string.Empty;
                            string Sql_Where = "(RowID = @RowID)";

                            string Column_List_1 = string.Empty;
                            string Column_List_2 = string.Empty;

                            //
                            string[] Row = line.Split('#');

                            for (int x1 = 0; x1 < Row.Length; x1++)
                            {
                                string[] Column = Row[x1].Split('=');

                                if (Column.Length >= 2)
                                {
                                    string name = Column[0];
                                    string value = Column[1];

                                    if (name == "Table")
                                    {
                                        Table = value;
                                    }
                                    else
                                    {
                                        Column_List_1 += name + ", ";
                                        Column_List_2 += "@" + name + ", ";
                                    }
                                }
                            }

                            Column_List_1 = Remove_String_Last(Column_List_1, ",");
                            Column_List_2 = Remove_String_Last(Column_List_2, ",");

                            if ((Table != string.Empty) && (Sql_Where != string.Empty) && (Column_List_1 != string.Empty) && (Column_List_2 != string.Empty))
                            {
                                string Sql_Query =
                                    " DELETE FROM TOPOS_DB.DBO." + Table + " WHERE ((MaHD = @MaHD) AND (RowID IS NULL))"

                                    + " IF NOT EXISTS (SELECT * FROM TOPOS_DB.DBO." + Table + " WHERE (RowID = @RowID))"
                                    + " BEGIN"
                                    + "     INSERT INTO TOPOS_DB.DBO." + Table
                                    + "     (" + Column_List_1 + ")"
                                    + "     VALUES"
                                    + "     (" + Column_List_2 + ")"
                                    + " END"
                                    ;

                                //
                                Sql_Query = Check_Sql_Query(Sql_Query);
                                Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                                for (int x1 = 0; x1 < Row.Length; x1++)
                                {
                                    string[] Column = Row[x1].Split('=');

                                    if (Column.Length >= 2)
                                    {
                                        string name = Column[0];
                                        string value = Column[1];

                                        if (name == "DaVanChuyen")
                                        {
                                            value = "1";
                                        }

                                        if (name != "Table")
                                        {
                                            if (value != "NULL")
                                            {
                                                //27-03-19 12:00:00 AM
                                                //4/1/2019 12:00:00 AM
                                                if ((name == "NgayBatDau") || (name == "NgayKetThuc") || (name == "NgayGioQuet"))
                                                {
                                                    //new _4e().Write_To_File_Temp("Date.txt", value);

                                                    //string date = value.Split(' ')[0];
                                                    //date = date.Replace("-", "/");

                                                    //value = "20" + date.Split('/')[2] + "-" + date.Split('/')[1] + "-" + date.Split('/')[0];

                                                    string date = value.Split(' ')[0];
                                                    date = date.Replace("-", "/");

                                                    value = date.Split('/')[2] + "-" + date.Split('/')[0] + "-" + date.Split('/')[1];
                                                }

                                                Sql_Command.Parameters.AddWithValue("@" + name, value);
                                            }
                                            else
                                            {
                                                if (Check_Exists_In_Array(NoNullColumn, name))
                                                {
                                                    Sql_Command.Parameters.AddWithValue("@" + name, string.Empty);
                                                }
                                                else
                                                {
                                                    Sql_Command.Parameters.AddWithValue("@" + name, DBNull.Value);
                                                }
                                            }
                                        }
                                    }
                                }

                                Sql_Command.ExecuteNonQuery(); Sql_Connection.Close(); Sql_Connection.Dispose();
                            }
                        }
                    }
                }
            }
        }

        public string Submit_Switch_Point(string Card_1, string Card_2)
        {
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            //
            string Sucessfull = "0";

            bool Valid = true;

            Card_1 = Remove_Space_String(Card_1);
            Card_2 = Remove_Space_String(Card_2);

            //
            if (
                (!Check_String_Length(Card_1, 11, 11))
                || (!Check_String_Length(Card_2, 11, 11))

                || (!Check_ID(Card_1))
                || (!Check_ID(Card_2))
                )
            {
                Valid = false;
            }

            //
            if (Valid)
            {
                string IP = HttpContext.Current.Request.UserHostAddress;

                //UserName
                string UserName = new _4e().Read_UserName();

                string Ho_Va_Ten = string.Empty;
                string Phong_Ban = string.Empty;

                bool Duoc_Xem_Bao_Cao = false;
                bool Duoc_Tich_Diem = false;
                bool Duoc_Tru_Diem = false;
                bool Duoc_Doi_Diem = false;

                new _4e().Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

                //
                string Computer = string.Empty;
                string Domain = string.Empty;
                string UserName_Services = string.Empty;

                string QuayBan = string.Empty;
                string CaBan = string.Empty;
                string NVBan = string.Empty;

                new _4e().Read_Iam(31, out Computer, out Domain, out UserName_Services, out QuayBan, out CaBan, out NVBan);

                if (QuayBan == string.Empty)
                {
                    if (Computer != string.Empty)
                    {
                        QuayBan = Computer;
                    }
                    else
                    {
                        QuayBan = IP;
                    }
                }

                NVBan = UserName;

                //
                string Add_At = QuayBan;
                string Add_By = NVBan;
                string Add_Time = Time;

                //
                string Card_1_NEW = "-" + Card_1;

                //
                Sql_Query =

                    " DECLARE @Mem_SEQ_1 NVARCHAR(MAX)"
                    + " DECLARE @Mem_SEQ_2 NVARCHAR(MAX)"

                    + " SET @Mem_SEQ_1 = (SELECT TOP 1 Mem_SEQ FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card_1))"
                    + " SET @Mem_SEQ_2 = (SELECT TOP 1 Mem_SEQ FROM GARDEN_CRM.DBO.T_MEM_MST WHERE (Mem_Card = @Card_2))"

                    //Sale
                    + " UPDATE GARDEN_CRM.DBO.T_SALE_INFO"
                    + " SET Mem_SEQ_Old = @Mem_SEQ_1, Mem_SEQ = @Mem_SEQ_2, Mem_SEQ_Edit_At = @Add_At, Mem_SEQ_Edit_By = @Add_By, Mem_SEQ_Edit_Time = @Add_Time"
                    + " WHERE (Mem_SEQ = @Mem_SEQ_1)"

                    //Point
                    + " UPDATE GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                    + " SET Mem_SEQ_Old = @Mem_SEQ_1, Mem_SEQ = @Mem_SEQ_2, Mem_SEQ_Edit_At = @Add_At, Mem_SEQ_Edit_By = @Add_By, Mem_SEQ_Edit_Time = @Add_Time"
                    + " WHERE (Mem_SEQ = @Mem_SEQ_1)"

                    //Mem-Card
                    + " UPDATE GARDEN_CRM.DBO.T_MEM_MST"
                    + " SET Mem_Card_Old = @Card_1, Mem_Card = @Card_1_NEW, Mem_Card_Edit_At = @Add_At, Mem_Card_Edit_By = @Add_By, Mem_Card_Edit_Time = @Add_Time"
                    + " WHERE (Mem_SEQ = @Mem_SEQ_1)"

                    //1
                    + " DELETE FROM GARDEN_CRM.DBO.T_MEM_NOW_PNT WHERE (MEM_SEQ = @Mem_SEQ_1)"

                    //2
                    + " DECLARE @Total_Point_2 NVARCHAR(MAX)"
                    + " SET @Total_Point_2 = (SELECT SUM(CHG_PNT) FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO WHERE (MEM_SEQ = @Mem_SEQ_2))"

                    + " IF EXISTS (SELECT * FROM GARDEN_CRM.DBO.T_MEM_NOW_PNT WHERE (MEM_SEQ = @Mem_SEQ_2))"
                    + "     UPDATE GARDEN_CRM.DBO.T_MEM_NOW_PNT SET NOW_TOT_PNT = @Total_Point_2 WHERE (MEM_SEQ = @Mem_SEQ_2)"
                    + " ELSE"
                    + "     INSERT INTO GARDEN_CRM.DBO.T_MEM_NOW_PNT (MEM_SEQ, NOW_TOT_PNT) VALUES (@Mem_SEQ_2, @Total_Point_2)"
                    ;

                //
                Sql_Query = Check_Sql_Query(Sql_Query);
                Get_Sql_Connection_DB(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                Sql_Command.Parameters.Add("@Card_1", Card_1);
                Sql_Command.Parameters.Add("@Card_2", Card_2);
                Sql_Command.Parameters.Add("@Card_1_NEW", Card_1_NEW);

                Sql_Command.Parameters.Add("@Add_At", Add_At);
                Sql_Command.Parameters.Add("@Add_By", Add_By);
                Sql_Command.Parameters.Add("@Add_Time", Add_Time);

                Sql_Command.ExecuteNonQuery();

                Sucessfull = "1";
            }

            //
            return Sucessfull;
        }

        public string Submit_Switch_Multi_Point(string Card_1_List, string Card_2)
        {
            string Sucessfull = "1";

            string[] Card_1_Array = Split_List_To_Array_ID(Card_1_List);

            for (int i1 = 0; i1 < Card_1_Array.Length; i1++)
            {
                if (Card_1_Array[i1] != Card_2)
                {
                    Submit_Switch_Point(Card_1_Array[i1], Card_2);
                }
            }

            return Sucessfull;
        }
    }
}
