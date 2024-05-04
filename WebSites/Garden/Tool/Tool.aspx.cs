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

using System.Runtime.InteropServices;
using OfficeOpenXml;

public partial class Tool : System.Web.UI.Page
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
            Sql_Connection = new SqlConnection(Application["Sql_Connection_String_181"].ToString());

            if (Sql_Connection.State != ConnectionState.Open)
            {
                Sql_Connection.Open();
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //TimeCount
        string Start_Time = DateTime.Now.ToString();

        //FC
        //Khoa_The_FC();

        //QLTS : Gen
        //Chuyen_Doi_Excel_QLTS_Van_Phong();

        //Chuyen_Doi_Excel_QLTS_Vi_Tri(2, 4, 110, "Thietbile");
        //Chuyen_Doi_Excel_QLTS_Vi_Tri(3, 4, 58, "TSCC");
        //Chuyen_Doi_Excel_QLTS_Vi_Tri(4, 4, 16, "GoldproHC");
        //Chuyen_Doi_Excel_QLTS_Vi_Tri(5, 4, 29, "GoldproKP");
        //Chuyen_Doi_Excel_QLTS_Vi_Tri(6, 4, 13, "Dongphuc");

        //QLTS : Insert
        //Auto_Insert_QLTS_Van_Phong(767);

        //Auto_Insert_QLTS_Vi_Tri("Thietbile", 467, 23);
        //Auto_Insert_QLTS_Vi_Tri("TSCC", 1857, 24);
        //Auto_Insert_QLTS_Vi_Tri("Dongphuc", 1668, 28);

        //Auto_Insert_QLTS_Vi_Tri("GoldproKP", 441, 25);
        //Auto_Insert_QLTS_Vi_Tri("GoldproHC", 38, 26);

        //TimeShow
        string End_Time = DateTime.Now.ToString();
        Response.Write("OK !<br/>" + Start_Time + "<br/>" + End_Time);
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
    protected void Chuyen_Doi_Excel_QLTS_Van_Phong()
    {
        //
        FileInfo Input_File_Info = new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\QLTS\TS_2019_04_17.xlsx");
        ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

        FileInfo Export_File_Info = new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\QLTS\Mau-ke-khai-Tai-san.xlsx");
        ExcelPackage Export_Excel_Package = new ExcelPackage(Export_File_Info);

        //
        var Input_Work_sheets_TS = Input_Excel_Package.Workbook.Worksheets[1];
        var Export_Work_sheets = Export_Excel_Package.Workbook.Worksheets[1];

        //output
        int Row = 5;

        //input
        int from = 5;
        int to = 442;

        //Read Data
        for (int i1 = from; i1 <= to; i1++)
        {
            string Ten_Tai_San = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 1].Value);
            string Loai_Tai_San = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 1].Value);

            string Ma_Tai_San_ALL = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 2].Value);
            int So_Luong = new _4e().Convert_String_To_Int(new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 3].Value), 0);

            string Nguoi_Dung = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 4].Value);
            string Phong_Ban = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 5].Value);

            if (Phong_Ban == string.Empty)
            {
                Phong_Ban = Nguoi_Dung;
            }

            // --- Ma_Tai_San

            // --- Loai_Tai_San
            // --- Ten_Tai_San

            // --- Nguoi_Dung
            // --- Phong_Ban

            bool Only_One = true;

            Ma_Tai_San_ALL = Ma_Tai_San_ALL.Replace(Environment.NewLine, " ");
            Ma_Tai_San_ALL = Ma_Tai_San_ALL.Replace("\r\n", " ");
            Ma_Tai_San_ALL = new _4e().Remove_Space_String(Ma_Tai_San_ALL);

            Ma_Tai_San_ALL = Ma_Tai_San_ALL.Replace(",", "-").Replace("&", "-");
            Ma_Tai_San_ALL = Ma_Tai_San_ALL.Replace(" -", "-").Replace("- ", "-");

            //
            if (So_Luong > 1)
            {
                string[] Ma_Tai_San_Array = Ma_Tai_San_ALL.Split('-');

                if (Ma_Tai_San_Array.Length == 2)
                {
                    string Ma_Tai_San_Prefix_AND_Number = Ma_Tai_San_Array[0];

                    string Ma_Tai_San_Prefix = new _4e().Remove_From_String_To_End(Ma_Tai_San_Prefix_AND_Number, " ");

                    string[] Ma_Tai_San_Prefix_AND_Number_Array = Ma_Tai_San_Prefix_AND_Number.Split(' ');

                    string Number_Start = Ma_Tai_San_Prefix_AND_Number_Array[Ma_Tai_San_Prefix_AND_Number_Array.Length - 1];
                    string Number_End = Ma_Tai_San_Array[1];

                    //
                    Ma_Tai_San_Prefix = new _4e().Remove_Space_String(Ma_Tai_San_Prefix);
                    Number_Start = new _4e().Remove_Space_String(Number_Start);
                    Number_End = new _4e().Remove_Space_String(Number_End);

                    //
                    bool Number_Start_With_DOT = Number_Start.Contains(".");
                    bool Number_End_With_DOT = Number_End.Contains(".");

                    //
                    if (Number_Start_With_DOT && Number_End_With_DOT)
                    {
                        Number_Start = Number_Start.Replace(" .", ".").Replace(". ", ".");
                        Number_End = Number_End.Replace(" .", ".").Replace(". ", ".");

                        new _4e().Write_To_File_Temp("\\Space.txt", "0: " + Ma_Tai_San_ALL + " >" + Number_Start + "<");

                        string[] Number_Start_Array = Number_Start.Split('.');

                        Number_Start = Number_Start_Array[0];
                        string Number_Start_After_DOT = Number_Start_Array[1];

                        string[] Number_End_Array = Number_End.Split('.');

                        Number_End = Number_End_Array[0];
                        string Number_End_After_DOT = Number_End_Array[1];

                        //
                        Number_Start_After_DOT = new _4e().Remove_Space_String(Number_Start_After_DOT);
                        Number_End_After_DOT = new _4e().Remove_Space_String(Number_End_After_DOT);

                        Number_Start_After_DOT = new _4e().Remove_0_Before(Number_Start_After_DOT);
                        Number_End_After_DOT = new _4e().Remove_0_Before(Number_End_After_DOT);

                        int Number_Start_int = new _4e().Convert_String_To_Int(Number_Start_After_DOT, 0);
                        int Number_End_int = new _4e().Convert_String_To_Int(Number_End_After_DOT, 0);

                        //
                        for (int i4 = Number_Start_int; i4 <= Number_End_int; i4++)
                        {
                            Only_One = false;

                            Row++;

                            string Ma_Tai_San = Ma_Tai_San_Prefix + " " + Number_Start + "." + i4;

                            //
                            Export_Work_sheets.Cells[Row, 1].Value = Ma_Tai_San;
                            //Export_Work_sheets.Cells[Row, 2].Value = Ma_Tai_San_ALL;

                            Export_Work_sheets.Cells[Row, 3].Value = Loai_Tai_San;
                            Export_Work_sheets.Cells[Row, 4].Value = Ten_Tai_San;

                            Export_Work_sheets.Cells[Row, 6].Value = Nguoi_Dung;
                            Export_Work_sheets.Cells[Row, 7].Value = Phong_Ban;
                        }

                    }
                    else
                        if (!Number_Start_With_DOT && !Number_End_With_DOT)
                    {
                        Number_Start = new _4e().Remove_0_Before(Number_Start);
                        Number_End = new _4e().Remove_0_Before(Number_End);

                        int Number_Start_int = new _4e().Convert_String_To_Int(Number_Start, 0);
                        int Number_End_int = new _4e().Convert_String_To_Int(Number_End, 0);

                        //
                        for (int i4 = Number_Start_int; i4 <= Number_End_int; i4++)
                        {
                            Only_One = false;

                            Row++;

                            string Ma_Tai_San = Ma_Tai_San_Prefix + " " + i4;

                            if (i4 < 10)
                            {
                                Ma_Tai_San = Ma_Tai_San_Prefix + " 00" + i4;
                            }
                            else
                                if (i4 < 100)
                            {
                                Ma_Tai_San = Ma_Tai_San_Prefix + " 0" + i4;
                            }

                            //
                            Export_Work_sheets.Cells[Row, 1].Value = Ma_Tai_San;
                            //Export_Work_sheets.Cells[Row, 2].Value = Ma_Tai_San_ALL;

                            Export_Work_sheets.Cells[Row, 3].Value = Loai_Tai_San;
                            Export_Work_sheets.Cells[Row, 4].Value = Ten_Tai_San;

                            Export_Work_sheets.Cells[Row, 6].Value = Nguoi_Dung;
                            Export_Work_sheets.Cells[Row, 7].Value = Phong_Ban;
                        }
                    }
                }
            }

            //
            if (Only_One)
            {
                Row++;

                //
                string Ma_Tai_San = Ma_Tai_San_ALL;

                Export_Work_sheets.Cells[Row, 1].Value = Ma_Tai_San;
                //Export_Work_sheets.Cells[Row, 2].Value = Ma_Tai_San_ALL;

                Export_Work_sheets.Cells[Row, 3].Value = Loai_Tai_San;
                Export_Work_sheets.Cells[Row, 4].Value = Ten_Tai_San;

                Export_Work_sheets.Cells[Row, 6].Value = Nguoi_Dung;
                Export_Work_sheets.Cells[Row, 7].Value = Phong_Ban;
            }
        }

        //Save
        Export_Excel_Package.SaveAs(new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\QLTS\Mau-ke-khai-Tai-san_OK.xlsx"));
    }
    protected void Chuyen_Doi_Excel_QLTS_Vi_Tri(int sheet, int from, int to, string File_Name)
    {
        //
        FileInfo Input_File_Info = new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\QLTS\TS_2019_04_17.xlsx");
        ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

        FileInfo Export_File_Info = new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\QLTS\Mau-ke-khai-Tai-san.xlsx");
        ExcelPackage Export_Excel_Package = new ExcelPackage(Export_File_Info);

        //Worksheets
        var Input_Work_sheets_TS = Input_Excel_Package.Workbook.Worksheets[sheet];
        var Export_Work_sheets = Export_Excel_Package.Workbook.Worksheets[1];

        //output
        int Row = 5;

        //Read Data
        for (int i1 = from; i1 <= to; i1++)
        {
            string Ten_Tai_San = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 1].Value);
            string Loai_Tai_San = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 1].Value);

            string Ma_Tai_San_ALL = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 2].Value);
            int So_Luong = new _4e().Convert_String_To_Int(new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 3].Value), 0);

            string Vi_Tri_ALL = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 4].Value);

            //
            bool Only_One = true;

            Ma_Tai_San_ALL = Ma_Tai_San_ALL.Replace(Environment.NewLine, " ");
            Ma_Tai_San_ALL = Ma_Tai_San_ALL.Replace("\r\n", " ");
            Ma_Tai_San_ALL = new _4e().Remove_Space_String(Ma_Tai_San_ALL);

            Ma_Tai_San_ALL = Ma_Tai_San_ALL.Replace(",", "-").Replace("&", "-");
            Ma_Tai_San_ALL = Ma_Tai_San_ALL.Replace(" -", "-").Replace("- ", "-");

            //
            if (So_Luong > 1)
            {
                string[] Ma_Tai_San_Array = Ma_Tai_San_ALL.Split('-');

                if (Ma_Tai_San_Array.Length == 2)
                {
                    string Ma_Tai_San_Prefix_AND_Number = Ma_Tai_San_Array[0];

                    string Ma_Tai_San_Prefix = new _4e().Remove_From_String_To_End_Last(Ma_Tai_San_Prefix_AND_Number, " ");

                    string[] Ma_Tai_San_Prefix_AND_Number_Array = Ma_Tai_San_Prefix_AND_Number.Split(' ');

                    string Number_Start = Ma_Tai_San_Prefix_AND_Number_Array[Ma_Tai_San_Prefix_AND_Number_Array.Length - 1];
                    string Number_End = Ma_Tai_San_Array[1];

                    //
                    Ma_Tai_San_Prefix = new _4e().Remove_Space_String(Ma_Tai_San_Prefix);
                    Number_Start = new _4e().Remove_Space_String(Number_Start);
                    Number_End = new _4e().Remove_Space_String(Number_End);

                    //
                    Number_Start = new _4e().Remove_0_Before(Number_Start);
                    Number_End = new _4e().Remove_0_Before(Number_End);

                    int Number_Start_int = new _4e().Convert_String_To_Int(Number_Start, 0);
                    int Number_End_int = new _4e().Convert_String_To_Int(Number_End, 0);

                    //
                    string[] Ten_Tai_San_Array = new string[0];

                    for (int i4 = Number_Start_int; i4 <= Number_End_int; i4++)
                    {
                        Ten_Tai_San_Array = new _4e().Add_Value_To_Array_String(Ten_Tai_San_Array, Ten_Tai_San);
                    }

                    //
                    string[] Vi_Tri_OK_Array = new string[0];

                    for (int i4 = Number_Start_int; i4 <= Number_End_int; i4++)
                    {
                        Vi_Tri_OK_Array = new _4e().Add_Value_To_Array_String(Vi_Tri_OK_Array, Vi_Tri_ALL);
                    }

                    if (!Vi_Tri_ALL.Contains(";"))
                        Vi_Tri_ALL += ";";

                    //
                    string[] Vi_Tri_Array = Vi_Tri_ALL.Split(';');

                    if (Vi_Tri_Array.Length >= 2)
                    {
                        Ten_Tai_San_Array = new string[0];
                        Vi_Tri_OK_Array = new string[0];

                        //Quầy POS:7;B2 bảo vệ thu vé:2;Mr Tuấn
                        //GC; Kỹ thuật
                        //HCNS: Sony Corp 8323147; MAR: Canon Power G12; Kho ĐN:Sony 2386959

                        for (int j2 = 0; j2 < Vi_Tri_Array.Length; j2++)
                        {
                            string Vi_Tri_AND_Number = new _4e().Remove_Space_String(Vi_Tri_Array[j2]);
                            string[] Vi_Tri_AND_Number_Array = Vi_Tri_AND_Number.Split(':');

                            if (Vi_Tri_AND_Number_Array.Length == 2)
                            {
                                string Vi_Tri = new _4e().Remove_Space_String(Vi_Tri_AND_Number_Array[0]);
                                string Number = new _4e().Remove_Space_String(Vi_Tri_AND_Number_Array[1]);

                                if (new _4e().Check_ID(Number))
                                {
                                    int Number_int = new _4e().Convert_String_To_Int(Number, 0);

                                    for (int j3 = 0; j3 < Number_int; j3++)
                                    {
                                        Vi_Tri_OK_Array = new _4e().Add_Value_To_Array_String(Vi_Tri_OK_Array, Vi_Tri);
                                        Ten_Tai_San_Array = new _4e().Add_Value_To_Array_String(Ten_Tai_San_Array, Ten_Tai_San);
                                    }
                                }
                                else
                                {
                                    Vi_Tri_OK_Array = new _4e().Add_Value_To_Array_String(Vi_Tri_OK_Array, Vi_Tri);
                                    Ten_Tai_San_Array = new _4e().Add_Value_To_Array_String(Ten_Tai_San_Array, Ten_Tai_San + " " + Number);
                                }
                            }
                            else
                            {
                                string Vi_Tri = Vi_Tri_AND_Number;

                                Vi_Tri_OK_Array = new _4e().Add_Value_To_Array_String(Vi_Tri_OK_Array, Vi_Tri);
                                Ten_Tai_San_Array = new _4e().Add_Value_To_Array_String(Ten_Tai_San_Array, Ten_Tai_San);
                            }
                        }
                    }

                    //
                    int j1 = 0;

                    //
                    for (int i4 = Number_Start_int; i4 <= Number_End_int; i4++)
                    {
                        Only_One = false;

                        Row++;

                        string Ma_Tai_San = Ma_Tai_San_Prefix + " " + i4;

                        if (i4 < 10)
                        {
                            Ma_Tai_San = Ma_Tai_San_Prefix + " 00" + i4;
                        }
                        else
                            if (i4 < 100)
                        {
                            Ma_Tai_San = Ma_Tai_San_Prefix + " 0" + i4;
                        }

                        //
                        Export_Work_sheets.Cells[Row, 1].Value = Ma_Tai_San;

                        Export_Work_sheets.Cells[Row, 3].Value = Loai_Tai_San;
                        Export_Work_sheets.Cells[Row, 4].Value = Ten_Tai_San_Array[j1];

                        Export_Work_sheets.Cells[Row, 8].Value = Vi_Tri_OK_Array[j1];

                        j1++;
                    }
                }
            }

            //
            if (Only_One)
            {
                Row++;

                //
                Export_Work_sheets.Cells[Row, 1].Value = Ma_Tai_San_ALL;

                Export_Work_sheets.Cells[Row, 3].Value = Loai_Tai_San;
                Export_Work_sheets.Cells[Row, 4].Value = Ten_Tai_San;

                Export_Work_sheets.Cells[Row, 8].Value = Vi_Tri_ALL;
            }
        }

        //Save
        Export_Excel_Package.SaveAs(new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\QLTS\Mau-ke-khai-Tai-san_OK_" + File_Name + ".xlsx"));
    }

    protected void Auto_Insert_QLTS_Van_Phong(int Row_End)
    {
        string Sql_Query = string.Empty;
        string Sql_Join = string.Empty;

        string Sql_Where = string.Empty;
        string Sql_Order = string.Empty;

        string Column_List_1 = string.Empty;
        string Column_List_2 = string.Empty;

        //
        FileInfo Input_File_Info = new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\QLTS\Mau-ke-khai-Tai-san_OK_VP.xlsx");
        ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

        //
        var Input_Work_sheets_TS = Input_Excel_Package.Workbook.Worksheets[1];

        //Read Data
        for (int i1 = 6; i1 <= Row_End; i1++)
        {
            string Ma_TS = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 1].Value);
            string Loai_TS = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 3].Value);
            string Ten_TS = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 4].Value);

            string Nguoi_Dung = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 6].Value);
            string Phong_Ban = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 7].Value);

            //Sua lai sau
            //Nguoi_Dung = "garden\\" + Nguoi_Dung;

            //
            Sql_Query =
                    " USE PDSBitexco"

                    + " DECLARE @Ma_Phong_Ban NVARCHAR(MAX)"
                    + " DECLARE @Ma_Nguoi_Dung NVARCHAR(MAX)"

                    + " DECLARE @Ma_Loai_Tai_San NVARCHAR(MAX)"
                    + " DECLARE @Tai_San_ID NVARCHAR(MAX)"

                    //Ma_Phong_Ban
                    + " SET @Ma_Phong_Ban = (SELECT TOP 1 ID FROM Departments WHERE (DeptName LIKE @Phong_Ban))"

                    + " IF @Ma_Phong_Ban IS NULL"
                    + " BEGIN"
                    + "     INSERT INTO Departments"
                    + "     (CompanyId, DeptCode, DeptName, DeptDesc, Active) VALUES (1, @Phong_Ban, @Phong_Ban, @Phong_Ban, 1)"
                    + "     SET @Ma_Phong_Ban = (SELECT SCOPE_IDENTITY());"
                    + " END"

                    //Ma_Nguoi_Dung
                    + " SET @Ma_Nguoi_Dung = (SELECT TOP 1 ID FROM UserInfo WHERE (DeptId = @Ma_Phong_Ban) AND (UserName LIKE @Nguoi_Dung))"

                    + " IF @Ma_Nguoi_Dung IS NULL"
                    + " BEGIN"
                    + "     INSERT INTO UserInfo"
                    + "     (DeptId, UserName, FullName, Active) VALUES (@Ma_Phong_Ban, @Nguoi_Dung, @Nguoi_Dung, 1)"
                    + "     SET @Ma_Nguoi_Dung = (SELECT SCOPE_IDENTITY());"
                    + " END"

                    //GroupUser_User
                    + " IF NOT EXISTS (SELECT * FROM GroupUser_User WHERE (UserId = @Ma_Nguoi_Dung))"
                    + " BEGIN"
                    + "     INSERT INTO GroupUser_User (UserId, GroupUserId) VALUES (@Ma_Nguoi_Dung, 2)"
                    + "     INSERT INTO GroupUser_User (UserId, GroupUserId) VALUES (@Ma_Nguoi_Dung, 12)"
                    + "     INSERT INTO GroupUser_User (UserId, GroupUserId) VALUES (@Ma_Nguoi_Dung, 1018)"
                    + " END"

                    //Ma_Loai_Tai_San
                    + " SET @Ma_Loai_Tai_San = (SELECT TOP 1 ID FROM DeviceCategory WHERE (DeviceCatName LIKE @Loai_Tai_San))"

                    + " IF @Ma_Loai_Tai_San IS NULL"
                    + " BEGIN"
                    + "     INSERT INTO DeviceCategory"
                    + "     (DeviceCatName, Type, Active) VALUES (@Loai_Tai_San, 0, 1)"
                    + "     SET @Ma_Loai_Tai_San = (SELECT SCOPE_IDENTITY());"
                    + " END"

                    //Tai_San_ID - CreateById, CreateDate
                    + " SET @Tai_San_ID = (SELECT TOP 1 ID FROM DeviceAndTool WHERE (CompanyId = 1) AND (DeviceCatId = @Ma_Loai_Tai_San) AND (AssetsCode = @Ma_Tai_San))"

                    + " IF @Tai_San_ID IS NULL"
                    + " BEGIN"
                    + "     INSERT INTO DeviceAndTool"
                    + "     (CompanyId, DeviceCatId, AssetsCode, DeviceName, CreateById, CreateDate) VALUES (1, @Ma_Loai_Tai_San, @Ma_Tai_San, @Ten_Tai_San, @Ma_Nguoi_Dung, GETDATE())"
                    + "     SET @Tai_San_ID = (SELECT SCOPE_IDENTITY());"
                    + " END"

                    //HistoryUse - DeviceToolId, HandedToStaffId, DeptId, HandedDate, StatusId
                    + " IF NOT EXISTS (SELECT * FROM HistoryUse WHERE (DeviceToolId = @Tai_San_ID))"
                    + " INSERT INTO HistoryUse (DeviceToolId, HandedToStaffId, DeptId, HandedDate, StatusId) VALUES (@Tai_San_ID, @Ma_Nguoi_Dung, @Ma_Phong_Ban, GETDATE(), 2)"

                    ;

            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@Nguoi_Dung", Nguoi_Dung);
            Sql_Command.Parameters.Add("@Phong_Ban", Phong_Ban);

            Sql_Command.Parameters.Add("@Ten_Tai_San", Ten_TS);
            Sql_Command.Parameters.Add("@Ma_Tai_San", Ma_TS);
            Sql_Command.Parameters.Add("@Loai_Tai_San", Loai_TS);

            Sql_Command.ExecuteNonQuery();
        }
    }
    protected void Auto_Insert_QLTS_Vi_Tri(string File_Name, int Row_End, int Company_ID)
    {
        string Sql_Query = string.Empty;
        string Sql_Join = string.Empty;

        string Sql_Where = string.Empty;
        string Sql_Order = string.Empty;

        string Column_List_1 = string.Empty;
        string Column_List_2 = string.Empty;

        //
        FileInfo Input_File_Info = new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\QLTS\Mau-ke-khai-Tai-san_OK_" + File_Name + ".xlsx");
        ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

        //
        var Input_Work_sheets_TS = Input_Excel_Package.Workbook.Worksheets[1];

        //Read Data
        for (int i1 = 6; i1 <= Row_End; i1++)
        {
            string Ma_TS = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 1].Value);
            string Loai_TS = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 3].Value);
            string Ten_TS = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 4].Value);

            string Vi_Tri = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 8].Value);

            //
            Sql_Query =
                    " USE PDSBitexco"

                    + " DECLARE @CompanyId NVARCHAR(MAX)"
                    + " SET @CompanyId = " + Company_ID

                    + " DECLARE @LocationId NVARCHAR(MAX)"

                    + " DECLARE @Ma_Loai_Tai_San NVARCHAR(MAX)"
                    + " DECLARE @Tai_San_ID NVARCHAR(MAX)"

                    //LocationId, ShortName, LocationName  --- Location
                    + " SET @LocationId = (SELECT TOP 1 ID FROM Location WHERE (LocationName LIKE @Vi_Tri))"

                    + " IF @LocationId IS NULL"
                    + " BEGIN"
                    + "     INSERT INTO Location"
                    + "     (ShortName, LocationName) VALUES (@Vi_Tri, @Vi_Tri)"
                    + "     SET @LocationId = (SELECT SCOPE_IDENTITY());"
                    + " END"

                    //Ma_Loai_Tai_San
                    + " SET @Ma_Loai_Tai_San = (SELECT TOP 1 ID FROM DeviceCategory WHERE (DeviceCatName LIKE @Loai_Tai_San))"

                    + " IF @Ma_Loai_Tai_San IS NULL"
                    + " BEGIN"
                    + "     INSERT INTO DeviceCategory"
                    + "     (DeviceCatName, Type, Active) VALUES (@Loai_Tai_San, 0, 1)"
                    + "     SET @Ma_Loai_Tai_San = (SELECT SCOPE_IDENTITY());"
                    + " END"

                    //Tai_San_ID - CreateById, CreateDate
                    + " SET @Tai_San_ID = (SELECT TOP 1 ID FROM DeviceAndTool WHERE (CompanyId = @CompanyId) AND (DeviceCatId = @Ma_Loai_Tai_San) AND (AssetsCode = @Ma_Tai_San))"

                    + " IF @Tai_San_ID IS NULL"
                    + " BEGIN"
                    + "     INSERT INTO DeviceAndTool"
                    + "     (CompanyId, DeviceCatId, AssetsCode, DeviceName, CreateById, CreateDate) VALUES (@CompanyId, @Ma_Loai_Tai_San, @Ma_Tai_San, @Ten_Tai_San, 8, GETDATE())"
                    + "     SET @Tai_San_ID = (SELECT SCOPE_IDENTITY());"
                    + " END"

                    //HistoryUse - DeviceToolId, LocationId, HandedDate, StatusId
                    + " IF NOT EXISTS (SELECT * FROM HistoryUse WHERE (DeviceToolId = @Tai_San_ID))"
                    + " INSERT INTO HistoryUse (DeviceToolId, LocationId, HandedDate, StatusId) VALUES (@Tai_San_ID, @LocationId, GETDATE(), 2)"

                    ;

            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@Vi_Tri", Vi_Tri);

            Sql_Command.Parameters.Add("@Ten_Tai_San", Ten_TS);
            Sql_Command.Parameters.Add("@Ma_Tai_San", Ma_TS);
            Sql_Command.Parameters.Add("@Loai_Tai_San", Loai_TS);

            Sql_Command.ExecuteNonQuery();
        }
    }

    protected void Khoa_The_FC()
    {
        string Sql_Query = string.Empty;
        string Sql_Join = string.Empty;

        string Sql_Where = string.Empty;
        string Sql_Order = string.Empty;

        string Column_List_1 = string.Empty;
        string Column_List_2 = string.Empty;

        //
        FileInfo Input_File_Info = new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\File_Upload\Khoa the FC 07.03.19.xlsx");
        ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

        //
        var Input_Work_sheets_TS = Input_Excel_Package.Workbook.Worksheets[1];

        //Read Data
        for (int i1 = 3; i1 <= 136; i1++)
        {
            string MaFoodCourt = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 3].Value);

            MaFoodCourt = new _4e().Remove_Space_String(MaFoodCourt);

            Sql_Query =
                    " USE TOPOS_DB"

                    + " SELECT SoTien FROM TTT_DanhMuc"
                    + " WHERE (MaFoodCourt = @MaFoodCourt)"

                    + " UPDATE TTT_DanhMuc"
                    + " SET SoTien_Lock = SoTien, SoTien = 0, TrangThai = 0"
                    + " WHERE (MaFoodCourt = @MaFoodCourt)"
                    ;

            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@MaFoodCourt", MaFoodCourt);

            string SoTien = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());

            SoTien = new _4e().Remove_String_Last(SoTien, ".00");
            //SoTien = new _4e().Split_Thousand(SoTien);

            Input_Work_sheets_TS.Cells[i1, 4].Value = SoTien;
        }

        Input_Excel_Package.SaveAs(new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\File_Upload\Khoa the FC 07.03.19 - (IT da khoa).xlsx"));
    }
    protected void FC17()
    {
        FileInfo Input_File_Info = new FileInfo(HttpContext.Current.Server.MapPath("~") + @"\File_Upload\FC122017.xlsx");
        ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

        //
        var Input_Work_sheets_TS = Input_Excel_Package.Workbook.Worksheets[1];

        //Read Data
        //for (int i1 = 9; i1 <= 93407; i1++)
        //for (int i1 = 9; i1 <= 1000; i1++)
        for (int i1 = 93407; i1 >= 9; i1--)
        {
            string TongTien_Checked = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 17].Value);
            TongTien_Checked = new _4e().Remove_Space_String(TongTien_Checked);

            if (TongTien_Checked == "x")
            {
                //
                string MaFoodCourt = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 2].Value);
                MaFoodCourt = new _4e().Remove_Space_String(MaFoodCourt);

                string SoTien_2017 = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 9].Value);
                SoTien_2017 = new _4e().Remove_Space_String(SoTien_2017);

                //
                if (!MaFoodCourt.StartsWith("0104"))
                {
                    string MaFoodCourt_Start = "0104";

                    int Need_Add = 11 - 4 - MaFoodCourt.Length;

                    for (int x = 1; x <= Need_Add; x++)
                    {
                        MaFoodCourt_Start += "0";
                    }

                    MaFoodCourt = MaFoodCourt_Start + MaFoodCourt;
                }

                //
                string Sql_Query =
                        " USE TOPOS_DB"

                        + " UPDATE TTT_DanhMuc"
                        + " SET SoTien_2017 = @SoTien_2017"
                        + " WHERE (MaFoodCourt = @MaFoodCourt)"

                        //SD hien tai = TongTien From Checked
                        + " SELECT TongTien FROM [TOPOS_DB_Backup_12].[dbo].[FC_Checked]"
                        + " WHERE (MaFoodCourt = @MaFoodCourt)"

                        ;

                Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                Sql_Command.Parameters.Add("@MaFoodCourt", MaFoodCourt);
                Sql_Command.Parameters.Add("@SoTien_2017", SoTien_2017);

                string TongTien = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());

                TongTien = new _4e().Remove_String_Last(TongTien, ".00");
                //TongTien = new _4e().Split_Thousand(TongTien);

                Input_Work_sheets_TS.Cells[i1, 17].Value = TongTien;
            }
        }

        string File_Path_New = HttpContext.Current.Server.MapPath("~") + @"\File_Upload\FC122017 - Checked " + DateTime.Now.Ticks + Guid.NewGuid() + ".xlsx";

        new _4e().Delete_File(File_Path_New);

        Input_Excel_Package.SaveAs(new FileInfo(File_Path_New));
    }
    protected void Convert_Phone_11()
    {
        //[GARDEN_CRM].[dbo].[T_MEM_MST_Backup]
        //MEM_SEQ, MEM_CARD, MEM_NM, MOBILE_NO_Backup, MOBILE_NO_New, MOBILE_NO_Edited

        string Sql_Query_Update = " USE GARDEN_CRM";

        new _4e().Write_To_File_Temp("Sql_Query_Update.sql", Sql_Query_Update);

        string Sql_Query =

            " USE GARDEN_CRM"

            + " SELECT MEM_SEQ, MOBILE_NO_Backup"
            + " FROM T_MEM_MST_Backup"
            + " WHERE (MOBILE_NO_Edited = 0)"
            ;


        Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
        Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

        SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

        try
        {
            while (Sql_Data_Reader.Read())
            {
                string MEM_SEQ = Sql_Data_Reader["MEM_SEQ"].ToString();
                string MOBILE_NO_Backup = Sql_Data_Reader["MOBILE_NO_Backup"].ToString();

                string MOBILE_NO_New = "";// new _4e().Convert_Phone_Number_11(MOBILE_NO_Backup);

                Sql_Query_Update =

                    " UPDATE T_MEM_MST"
                    + " SET MOBILE_NO = '" + MOBILE_NO_New + "'"
                    + " WHERE (MEM_SEQ = '" + MEM_SEQ + "')"

                    + Environment.NewLine

                    + " UPDATE T_MEM_MST_Backup"
                    + " SET MOBILE_NO_New = '" + MOBILE_NO_New + "', MOBILE_NO_Edited = 1"
                    + " WHERE (MEM_SEQ = '" + MEM_SEQ + "')"

                    ;

                new _4e().Write_To_File_Temp("Sql_Query_Update.sql", Sql_Query_Update);
            }
        }
        catch (SqlException Sql_Exception)
        {
        }

        if (!Sql_Data_Reader.IsClosed)
        {
            Sql_Data_Reader.Dispose(); Sql_Command.Dispose();
        }
    }
}