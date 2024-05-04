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

public partial class Add_Point : System.Web.UI.Page
{
    SqlConnection Sql_Connection;
    SqlCommand Sql_Command;

    //
    string Message = string.Empty;
    string Time = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss:fff");

    string On_Page_Load = " Add_Point_Onload();";
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

        if (UserName.ToLower() == "Pos2".ToLower())
        {
            Is_POS_hdf.Value = "1";
        }

        string Ho_Va_Ten = string.Empty;
        string Phong_Ban = string.Empty;

        bool Duoc_Xem_Bao_Cao = false;
        bool Duoc_Tich_Diem = false;
        bool Duoc_Tru_Diem = false;
        bool Duoc_Doi_Diem = false;

        new _4e().Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

        if (!Duoc_Tich_Diem && !Duoc_Tru_Diem && !Duoc_Doi_Diem)
        {
            Duoc_Tich_Diem = true;
        }

        if (!Duoc_Tich_Diem && !Duoc_Tru_Diem && !Duoc_Doi_Diem)
        {
            Response.Redirect(new _4e().Add_http_To_URL(new _4e().Read_Domain(string.Empty)));
        }
        else
        {
            //
            if (!IsPostBack)
            {
                new _4e().Set_Index_Host(Index_Host_hdf, null);
                PageMethods_Path_hdf.Value = "/Tool/Add_Point.aspx";

                Loggedin_UserId_hdf.Value = new _4e().Object_ToString(ViewState["UserName"]);

                Today_Year_hdf.Value = DateTime.Now.Year.ToString();
                Today_Month_hdf.Value = DateTime.Now.Month.ToString();
                Today_Day_hdf.Value = DateTime.Now.Day.ToString();

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

                //
                POS_lbl.Text = QuayBan;
                Cashier_lbl.Text = NVBan;

                //
                if (Duoc_Tich_Diem)
                {
                    Reason_rdol.Items.Add(new ListItem("<span style='font-size: 12pt; color: red;'>Tích điểm</span>", "Add"));
                }

                if (Duoc_Tru_Diem)
                {
                    Reason_rdol.Items.Add(new ListItem("<span style='font-size: 12pt; color: red;'>Trừ điểm tích nhầm</span>", "Mistake"));
                    Reason_rdol.Items.Add(new ListItem("<span style='font-size: 12pt; color: red;'>Trừ điểm</span>", "Minus"));
                }

                if (Duoc_Doi_Diem)
                {
                    Reason_rdol.Items.Add(new ListItem("<span style='font-size: 12pt; color: red;'>Thưởng điểm</span>", "Reward"));
                    Reason_rdol.Items.Add(new ListItem("<span style='font-size: 12pt; color: red;'>Đổi điểm lấy Voucher</span>", "Redeem"));                 
                }
                if (Duoc_Tich_Diem)
                {
                    Reason_rdol.Items.Add(new ListItem("<span style='font-size: 12pt; color: red;'>Tích doanh số</span>", "Sale"));
                }

                Reason_rdol.SelectedIndex = 0;

                //
                if (Phong_Ban.ToLower() == "Star Fitness".ToLower())
                {
                    Shop_tbx.Text = "Star Fitness";
                    Shop_tbx.Enabled = false;

                    Shop_MaThue_lbl.Text = "AA-StarFitness";
                    Enable_Add_Point_lbl.Text = "(Được tích điểm)";

                    Enable_Creat_Shop_List_hdf.Value = "0";
                }
            }
            else
            {
                Control_Call_Postback = new _4e().Determine_Control_Call_Postback();
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

    //
    [WebMethod(enableSession: true)]
    public static string Creat_Shop_List(string Shop_Name_OR_Code)
    {
        return new _4e().Creat_Shop_List(Shop_Name_OR_Code);
    }

    //
    [WebMethod(enableSession: true)]
    public static string Creat_Search_Name_List(string Search_Name_OR_Phone)
    {
        return new _4e().Creat_Search_Name_List(Search_Name_OR_Phone);
    }

    //
    [WebMethod(enableSession: true)]
    public static string Read_Receipt_Info(string Receipt)
    {
        return new _4e().Read_Receipt_Info(Receipt);
    }

    //
    [WebMethod(enableSession: true)]
    public static string Read_Card_Info(string Card)
    {
        return new _4e().Read_Card_Info(Card);
    }

    //
    [WebMethod(enableSession: true)]
    public static string Submit_Add_Point(string Submit_Add_Point_JSON)
    {
        return new _4e().Submit_Add_Point(Submit_Add_Point_JSON);
    }

    //
    [WebMethod(enableSession: true)]
    public static string Read_Iam_Info()
    {
        return new _4e().Read_Iam_Info();
    }
}