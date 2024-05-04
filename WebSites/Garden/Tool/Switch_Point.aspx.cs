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

public partial class Switch_Point : System.Web.UI.Page
{
    SqlConnection Sql_Connection;
    SqlCommand Sql_Command;

    //
    string Message = string.Empty;
    string Time = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss:fff");

    string On_Page_Load = " Switch_Point_Onload();";
    string Control_Call_Postback = string.Empty;

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
        //UserName
        string UserName = new _4e().Read_UserName();

        string Ho_Va_Ten = string.Empty;
        string Phong_Ban = string.Empty;

        bool Duoc_Xem_Bao_Cao = false;
        bool Duoc_Tich_Diem = false;
        bool Duoc_Tru_Diem = false;
        bool Duoc_Doi_Diem = false;

        new _4e().Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

        if (!Duoc_Doi_Diem)
        {
            Response.Redirect(new _4e().Add_http_To_URL(new _4e().Read_Domain(string.Empty)));
        }
        else
        {
            //
            if (!IsPostBack)
            {
                new _4e().Set_Index_Host(Index_Host_hdf, null);
                PageMethods_Path_hdf.Value = "/Tool/Switch_Point.aspx";
            }
        }
    }

    //OK+OK
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!new _4e().Check_PageMethods_Is_undefined())
        {
            new _4e().Add_ALL_JavaScript_AND_CSS_File_To_Header(Index_Host_hdf.Value);
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

    [WebMethod(enableSession: true)]
    public static string Read_Card_Info(string Card)
    {
        return new _4e().Read_Card_Info(Card);
    }

    //
    [WebMethod(enableSession: true)]
    public static string Submit_Switch_Point(string Card_1, string Card_2)
    {
        return new _4e().Submit_Switch_Point(Card_1, Card_2);
    }
}