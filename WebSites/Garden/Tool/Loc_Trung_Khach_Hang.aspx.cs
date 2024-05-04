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

public partial class Loc_Trung_Khach_Hang : System.Web.UI.Page
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
        Chuyen_Doi_Excel_Loc_Trung_Khach_Hang();
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
    protected void Chuyen_Doi_Excel_Loc_Trung_Khach_Hang()
    {
        string Sql_Query = string.Empty;
        string Sql_Join = string.Empty;

        string Sql_Where = string.Empty;
        string Sql_Order = string.Empty;

        string Column_List_1 = string.Empty;
        string Column_List_2 = string.Empty;

        //
        string[] Name_Array = new string[0];
        string[] Phone_Array = new string[0];

        //
        Sql_Query =
            " SELECT Mem_Nm AS Name, MOBILE_NO AS Phone"
            + " FROM GARDEN_CRM.DBO.T_MEM_MST"

            + " WHERE (Mem_Card NOT LIKE '0107%')"
            + " AND (MOBILE_NO IS NOT NULL) AND (MOBILE_NO NOT LIKE '') AND (LEN(MOBILE_NO) = DATALENGTH(MOBILE_NO)) AND (LEN(MOBILE_NO) >= 10)"
            + " AND (Mem_Nm IS NOT NULL) AND (Mem_Nm NOT LIKE '') AND (LEN(Mem_Nm) = DATALENGTH(Mem_Nm)) AND (Mem_Nm NOT LIKE 'X') AND (Mem_Nm NOT LIKE 'A')"

            + " GROUP BY Mem_Nm, MOBILE_NO"
            + " HAVING (COUNT(MOBILE_NO) > 1)"
            ;

        Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
        Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

        SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

        try
        {
            while (Sql_Data_Reader.Read())
            {
                Name_Array = new _4e().Add_Value_To_Array_String(Name_Array, Sql_Data_Reader["Name"].ToString());
                Phone_Array = new _4e().Add_Value_To_Array_String(Phone_Array, Sql_Data_Reader["Phone"].ToString());
            }
        }
        catch (SqlException Sql_Exception)
        {
        }

        if (!Sql_Data_Reader.IsClosed)
        {
            Sql_Data_Reader.Dispose(); Sql_Command.Dispose();
        }

        //
        Sql_Query = string.Empty;

        //
        for (int i1 = 0; i1 < Name_Array.Length; i1++)
        {
        }

        ////
        //FileInfo File_Info = new FileInfo(@"D:\Websites\Garden\File_Upload\User-Trinh-ky-Duyet.xlsx");
        //ExcelPackage Excel_Package = new ExcelPackage(File_Info);

        ////
        //var Work_sheets = Excel_Package.Workbook.Worksheets[1];

        ////Read Data
        //for (int Row = 6; Row <= Work_sheets.Dimension.End.Row; Row++)
        //{
        //    string Ma_Phong_Ban = new _4e().Object_ToString(Work_sheets.Cells[Row, 1].Value);
        //}

        ////Save
        //Excel_Package.Save();
    }
}