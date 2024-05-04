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

//using HtmlAgilityPack;
//using System.Runtime.InteropServices;
//using OfficeOpenXml;
//using System.Net;
//using System.Text;

public partial class Insert_Card : System.Web.UI.Page
{
    SqlConnection Sql_Connection;
    SqlCommand Sql_Command;

    //
    string Message = string.Empty;
    string Time = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss:fff");

    string On_Page_Load = " Insert_Card_Onload();";

    string Menu_List = string.Empty;

    //OK+OK
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!new _4e().Check_PageMethods_Is_undefined())
        {
            Sql_Connection = new SqlConnection(Application["Sql_Connection_String_DB"].ToString());

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

        if (Phong_Ban.ToLower() != "IT".ToLower())
        {
            Response.Redirect(new _4e().Add_http_To_URL(new _4e().Read_Domain(string.Empty)));
        }
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
    protected void OK_btn_Click(object sender, EventArgs e)
    {
        string Sql_Query = string.Empty;
        string Sql_Join = string.Empty;

        string Sql_Where = string.Empty;
        string Sql_Order = string.Empty;

        string Column_List_1 = string.Empty;
        string Column_List_2 = string.Empty;

        //
        string UserName = new _4e().Read_UserName();

        //
        string From_Card = From_Card_tbx.Text;
        string To_Card = To_Card_tbx.Text;

        From_Card = new _4e().Remove_Danger_String(From_Card);
        From_Card = new _4e().Remove_Space_String(From_Card);

        To_Card = new _4e().Remove_Danger_String(To_Card);
        To_Card = new _4e().Remove_Space_String(To_Card);

        From_Card_tbx.Text = From_Card;
        To_Card_tbx.Text = To_Card;

        Int64 From_Card_int = new _4e().Convert_String_To_BigInt(From_Card, 0);
        Int64 To_Card_int = new _4e().Convert_String_To_BigInt(To_Card, 0);

        bool Valid = true;

        if ((From_Card.Length != 11) || (To_Card.Length != 11) || (From_Card_int.ToString().Length != 10) || (To_Card_int.ToString().Length != 10) || (From_Card_int == 0) || (To_Card_int == 0))
        {
            Valid = false;
            Message += "<br/><br/> - Số thẻ nhập ko hợp lệ, phải nhập 11 chữ số khác 0 !";
        }

        if (From_Card_int > To_Card_int)
        {
            Valid = false;
            Message += "<br/><br/> - Phải nhập số thẻ TỪ <= số thẻ ĐẾN !";
        }

        //
        if (!Valid)
        {
            Message_lbl.Text = "LỖI: " + Message;
        }
        else
        {
            if (From_Card.StartsWith("0104"))
            {
                Sql_Query =

                    " DECLARE @New_Card NVARCHAR(MAX)"

                    + " DECLARE @Inserted_Card BIGINT"
                    + " SET @Inserted_Card = 0"

                    + " WHILE (@From_Card <= @To_Card)"

                    + " BEGIN"

                    + "     SET @New_Card = '0' + CONVERT(NVARCHAR(MAX), @From_Card)"

                    + "     IF NOT EXISTS (SELECT * FROM [Server-001-1].TOPOS_DB.DBO.TTT_DanhMuc WHERE MaFoodCourt LIKE @New_Card)"
                    + "     BEGIN"
                    + "         INSERT INTO [Server-001-1].TOPOS_DB.DBO.TTT_DanhMuc"
                    + "         (MaFoodCourt, SoTien, TrangThai, NgayTao, MaNVTao)"
                    + "         VALUES"
                    + "         (@New_Card, 0, 0, GETDATE(), @UserName)"

                    + "         SET @Inserted_Card = @Inserted_Card + 1"
                    + "     END"

                    + "     SET @From_Card = @From_Card + 1"

                    + " END"

                    + " SELECT @Inserted_Card"
                    ;
            }
            else
            {
                Sql_Query =
                    " USE GARDEN_CRM"

                    + " DECLARE @New_Card NVARCHAR(MAX)"

                    + " DECLARE @Inserted_Card BIGINT"
                    + " SET @Inserted_Card = 0"

                    + " WHILE (@From_Card <= @To_Card)"

                    + " BEGIN"

                    + "     SET @New_Card = '0' + CONVERT(NVARCHAR(MAX), @From_Card)"

                    + "     IF NOT EXISTS (SELECT * FROM T_MEM_MST WHERE Mem_Card LIKE @New_Card)"
                    + "     BEGIN"
                    + "         INSERT INTO T_MEM_MST"
                    + "         (MEM_CARD, MEM_NM, REG_ID, REG_DT, MOD_ID, MOD_DT, GRADE_CD, NAT_CD, CERTI_NO, MEM_BIRTHDAY, AGE_CD, SEX_CD, MAJOR_AREA, MINOR_AREA, MEM_ADDR, TEL_NO, MOBILE_NO, E_MAIL, COMPY_NM, COMPY_POS, COMPY_ADDR, COMPY_TEL_NO, JOB_CD, WEDDING_CD, WEDDING_DAY, CHILD_CNT, CAR_YN, AUTOBY_YN, INCOME_CD, USE_YN, LEAVE_DT)"
                    + "         VALUES"
                    + "         (@New_Card, 'X', 'IT', GETDATE(), @UserName, GETDATE(), 'GRD001', 'NAT001', '', '1990/01/01', 'AGE002', 'F', 'ARE100', 'ARE101', 'X', '0-0-0', '0-0-0', '', '', '', '', '0-0-0', '', 'N', '', 0, 'N', 'N', '', 'Y', '')"

                    + "         SET @Inserted_Card = @Inserted_Card + 1"
                    + "     END"

                    + "     SET @From_Card = @From_Card + 1"

                    + " END"

                    + " SELECT @Inserted_Card"
                    ;
            }

            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@UserName", UserName);

            Sql_Command.Parameters.Add("@From_Card", From_Card_int.ToString());
            Sql_Command.Parameters.Add("@To_Card", To_Card_int.ToString());

            int Inserted_Card = new _4e().Convert_String_To_Int(new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar()), 0);

            int Total_Card_Input = Convert.ToInt32(To_Card_int - From_Card_int) + 1;

            if (Inserted_Card == 0)
            {
                Message_lbl.Text = "CHÚ Ý ! Không có thẻ nào được kích hoạt. Vì tất cả đã có trong hệ thống.<br/><br/>"
                        + "Hãy báo cho Admin kiểm tra lại ngay !";
            }
            else
            {
                if (Inserted_Card < Total_Card_Input)
                {
                    Message_lbl.Text =
                        "CHÚ Ý ! Mới chỉ kích hoạt thành công: " + Inserted_Card + " / " + Total_Card_Input + " thẻ.<br/><br/>"
                        + "Hãy báo cho Admin kiểm tra lại ngay !";
                }
                else
                    if (Inserted_Card == Total_Card_Input)
                {
                    Message_lbl.Text =
                        "OK ! Đã kích hoạt thành công đầy đủ: " + Inserted_Card + " / " + Total_Card_Input + " thẻ !<br/><br/>"
                        + "Có thể báo Kiểm phẩm bàn giao thẻ cho Thu ngân được rồi !";
                }
            }
        }
    }
}