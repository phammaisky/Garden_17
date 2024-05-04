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

public partial class Quan_Ly_Tai_San : System.Web.UI.Page
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
            Sql_Connection = new SqlConnection(Application["Sql_Connection_String_QLTS"].ToString());

            if (Sql_Connection.State != ConnectionState.Open)
            {
                Sql_Connection.Open();
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Khoa_The_FC();
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
    protected void Chuyen_Doi_Excel_QLTS()
    {
        //
        FileInfo Input_File_Info = new FileInfo(@"D:\Websites\Garden\QLTS\QLTS-1.xlsx");
        ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

        FileInfo Export_File_Info = new FileInfo(@"D:\Websites\Garden\QLTS\Mau-ke-khai-Tai-san.xlsx");
        ExcelPackage Export_Excel_Package = new ExcelPackage(Export_File_Info);

        //
        var Input_Work_sheets_TS = Input_Excel_Package.Workbook.Worksheets[1];
        var Input_Work_sheets_Ma_TS = Input_Excel_Package.Workbook.Worksheets[2];
        var Export_Work_sheets = Export_Excel_Package.Workbook.Worksheets[1];

        int Row = 5;

        //Read Data: 2 - 74
        for (int i1 = 2; i1 <= 74; i1++)
        {
            string Nguoi_Dung = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 2].Value);
            string Phong_Ban = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 3].Value);

            if (Phong_Ban == string.Empty)
            {
                Phong_Ban = Nguoi_Dung;
            }

            //4 - 19
            for (int i2 = 4; i2 <= 19; i2++)
            {
                string Loai_Tai_San = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[1, i2].Value);
                string Ten_Tai_San = Loai_Tai_San;

                //
                int So_Luong = new _4e().Convert_String_To_Int(new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, i2].Value), 0);

                //
                bool Only_One = true;

                string Ma_Tai_San_ALL = new _4e().Object_ToString(Input_Work_sheets_Ma_TS.Cells[i1, i2].Value);

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
                else
                {
                    Only_One = true;
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
        }

        //Save
        Export_Excel_Package.SaveAs(new FileInfo(@"D:\Websites\Garden\QLTS\Mau-ke-khai-Tai-san_OK.xlsx"));
    }

    protected void Auto_Insert_QLTS()
    {
        string Sql_Query = string.Empty;
        string Sql_Join = string.Empty;

        string Sql_Where = string.Empty;
        string Sql_Order = string.Empty;

        string Column_List_1 = string.Empty;
        string Column_List_2 = string.Empty;

        //
        FileInfo Input_File_Info = new FileInfo(@"E:\Websites\Garden\File_Upload\Mau-ke-khai-Tai-san_OK.xlsx");
        ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

        //
        var Input_Work_sheets_TS = Input_Excel_Package.Workbook.Worksheets[1];

        //Read Data
        for (int i1 = 6; i1 <= 680; i1++)
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

    protected void Khoa_The_FC()
    {
        string Sql_Query = string.Empty;
        string Sql_Join = string.Empty;

        string Sql_Where = string.Empty;
        string Sql_Order = string.Empty;

        string Column_List_1 = string.Empty;
        string Column_List_2 = string.Empty;

        //
        FileInfo Input_File_Info = new FileInfo(@"E:\Websites\Garden\File_Upload\IT khoa the 28.12.16.xlsx");
        ExcelPackage Input_Excel_Package = new ExcelPackage(Input_File_Info);

        //
        var Input_Work_sheets_TS = Input_Excel_Package.Workbook.Worksheets[1];

        //Read Data
        for (int i1 = 2; i1 <= 178; i1++)
        {
            string MaFoodCourt = new _4e().Object_ToString(Input_Work_sheets_TS.Cells[i1, 3].Value);

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

        Input_Excel_Package.SaveAs(new FileInfo(@"E:\Websites\Garden\File_Upload\IT khoa the 28.12.16 - OK.xlsx"));
    }
}