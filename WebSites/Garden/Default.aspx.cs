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
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.Security;

public partial class Default : System.Web.UI.Page
{
    SqlConnection Sql_Connection;
    SqlCommand Sql_Command;

    //
    string Message = string.Empty;
    string Time = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss:fff");

    string On_Page_Load = "Home_Onload();";

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
        if (!new _4e().Check_PageMethods_Is_undefined())
        {
            new _4e().Delete_Time_Out_File(Server.MapPath("~/File/"), 0);
            new _4e().Delete_All_Empty_Directory(Server.MapPath("~/File/"), Server.MapPath("~/File/"));

            if (!IsPostBack)
            {
                //
                new _4e().Set_Index_Host(Index_Host_hdf, null);

                //
                Creat_Menu();
            }
        }
    }

    //OK+OK
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!new _4e().Check_PageMethods_Is_undefined())
        {
            new _4e().Add_ALL_JavaScript_AND_CSS_File_To_Header(Index_Host_hdf.Value);
            new _4e().Add_CSS_File_To_Header(Index_Host_hdf.Value + "/index/CSS/4u4m4e_Home.css?" + Guid.NewGuid().ToString());
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
    protected void Creat_Menu()
    {
        string Sql_Query = string.Empty;
        string Sql_Join = string.Empty;

        string Sql_Where = string.Empty;
        string Sql_Order = string.Empty;

        string Column_List_1 = string.Empty;
        string Column_List_2 = string.Empty;

        //UserName
        string UserName = new _4e().Read_UserName();

        string Ho_Va_Ten = string.Empty;
        string Phong_Ban = string.Empty;

        bool Duoc_Xem_Bao_Cao = false;
        bool Duoc_Tich_Diem = false;
        bool Duoc_Tru_Diem = false;
        bool Duoc_Doi_Diem = false;

        new _4e().Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

        if (!Duoc_Xem_Bao_Cao && !Duoc_Tich_Diem && !Duoc_Tru_Diem && !Duoc_Doi_Diem)
        {
            //Response.Redirect("http://10.15.40.17/Tool/Add_Point.aspx");
        }
        else
        {
            new _4e().UPDATE_List_Shop_Add_Point_To_DataBase();

            //Menu_1
            string[] Menu_1_Array = new string[0];
            string[] Menu_1_ID_Array = new string[0];
            string[] Menu_1_URL_Array = new string[0];
            string[] Menu_1_CMD_Array = new string[0];

            //FC
            Menu_1_Array = new _4e().Add_Value_To_Array_String(Menu_1_Array, "FC");
            Menu_1_URL_Array = new _4e().Add_Value_To_Array_String(Menu_1_URL_Array, string.Empty);

            //Báo cáo Tích điểm
            if (Duoc_Xem_Bao_Cao)
            {
                Menu_1_Array = new _4e().Add_Value_To_Array_String(Menu_1_Array, "Báo cáo Tích điểm !");
                Menu_1_URL_Array = new _4e().Add_Value_To_Array_String(Menu_1_URL_Array, string.Empty);
            }
            else
            {
                //Điểm đã tích
                if (Duoc_Tich_Diem)
                {
                    Menu_1_Array = new _4e().Add_Value_To_Array_String(Menu_1_Array, "Điểm đã tích");
                    Menu_1_URL_Array = new _4e().Add_Value_To_Array_String(Menu_1_URL_Array, "/Tool/Report.aspx?R=Add_point");
                }
            }

            //Tích điểm thủ công
            if (Duoc_Tich_Diem || Duoc_Tru_Diem || Duoc_Doi_Diem)
            {
                Menu_1_Array = new _4e().Add_Value_To_Array_String(Menu_1_Array, "Tích điểm thủ công");
                Menu_1_URL_Array = new _4e().Add_Value_To_Array_String(Menu_1_URL_Array, "/Tool/Add_Point.aspx");
            }

            //Chuyển điểm
            if (Duoc_Doi_Diem)
            {
                Menu_1_Array = new _4e().Add_Value_To_Array_String(Menu_1_Array, "Chuyển điểm");
                Menu_1_URL_Array = new _4e().Add_Value_To_Array_String(Menu_1_URL_Array, "/Tool/Switch_Point.aspx");
            }

            //SAP
            if ((Phong_Ban.ToLower() == "Accounting".ToLower()) || (Phong_Ban.ToLower() == "IT".ToLower()))
            {
                Menu_1_Array = new _4e().Add_Value_To_Array_String(Menu_1_Array, "SAP");
                Menu_1_URL_Array = new _4e().Add_Value_To_Array_String(Menu_1_URL_Array, "/Tool/Report.aspx?R=SAP");
            }

            //IT
            if (Phong_Ban.ToLower() == "IT".ToLower())
            {
                Menu_1_Array = new _4e().Add_Value_To_Array_String(Menu_1_Array, "IT");
                Menu_1_URL_Array = new _4e().Add_Value_To_Array_String(Menu_1_URL_Array, string.Empty);
            }

            //
            Menu_List = "<ul>";

            if (UserName.ToLower() == "Pos2".ToLower())
            {
                Menu_List += "<li class='dropdown'><a href='#' onclick=\"Menu_Home_On_Click(); return false;\"><span class='Bold_White_Text_css'>Xin chào !</span></a></li>";
            }
            else
            {
                Menu_List += "<li class='dropdown'><a href='#' onclick=\"Menu_Home_On_Click(); return false;\"><span class='Bold_White_Text_css'>Xin chào: " + UserName + " (" + Ho_Va_Ten + ") !</span></a></li>";
            }

            //
            if (Menu_1_Array.Length > 0)
            {
                for (int i1 = 0; i1 < Menu_1_Array.Length; i1++)
                {
                    //Menu_2
                    string[] Menu_2_Array = new string[0];
                    string[] Menu_2_ID_Array = new string[0];
                    string[] Menu_2_URL_Array = new string[0];
                    string[] Menu_2_CMD_Array = new string[0];

                    if (Menu_1_Array[i1] == "FC")
                    {
                        if ((Phong_Ban.ToLower() == "Accounting".ToLower()) || (Phong_Ban.ToLower() == "IT".ToLower()))
                        {
                            Menu_2_Array = new string[] {
                                "Số dư Tài khoản",
                                "Khóa thẻ",// / Mở lại",
                                "Lịch sử Giao dịch"
                                };

                            Menu_2_URL_Array = new string[] {
                                "/Tool/Report.aspx?R=FC_ACC",
                                "/Tool/Report.aspx?R=FC_Block",
                                "/Tool/Report.aspx?R=FC_History"
                                };
                        }
                        else
                        {
                            Menu_2_Array = new string[] {
                                "Số dư Tài khoản",
                                "Lịch sử Giao dịch"
                                };

                            Menu_2_URL_Array = new string[] {
                                "/Tool/Report.aspx?R=FC_ACC",
                                "/Tool/Report.aspx?R=FC_History"
                                };
                        }
                    }

                    if (Menu_1_Array[i1] == "Báo cáo Tích điểm !")
                    {
                        Menu_2_Array = new string[] {
                        "Add point",
                        "Statement",
                        "Current point",

                        "Compare",

                        "Max buying by Shop",
                        "Inquiry discount",
                        "Analys transaction",
                        "Buying list",
                        "Member list",
                        "Member buy",
                        "Sale",
                        "Card not use",
                    };

                        Menu_2_URL_Array = new string[] {
                        "/Tool/Report.aspx?R=Add_point",
                        "/Tool/Report.aspx?R=Statement",
                        "/Tool/Report.aspx?R=Current_point",

                        "/Tool/Report.aspx?R=Compare",

                        "/Tool/Report.aspx?R=Max_buying_by_Shop",
                        "/Tool/Report.aspx?R=Inquiry_discount",
                        "/Tool/Report.aspx?R=Analys_transaction",
                        "/Tool/Report.aspx?R=Buying_list",
                        "/Tool/Report.aspx?R=Member_list",
                        "/Tool/Report.aspx?R=Member_buy&Onload=1",
                        "/Tool/Report.aspx?R=Sale",
                        "/Tool/Report.aspx?R=Card_not_use",
                    };
                    }

                    if (Menu_1_Array[i1] == "IT")
                    {
                        Menu_2_Array = new string[] {
                        "Setup",
                        "Tạo thêm thẻ FC + Club"
                    };

                        Menu_2_URL_Array = new string[] {
                        "/RUN/Setup.zip",
                        "/Tool/Insert_Card.aspx"
                    };
                    }

                    //
                    string Menu_1_On_Click = string.Empty;

                    if (Menu_1_URL_Array[i1] != string.Empty)
                    {
                        Menu_1_On_Click = " onclick=\"Menu_On_Click('" + Menu_1_Array[i1] + "', '" + Menu_1_URL_Array[i1] + "'); return false;\"";
                    }

                    //
                    if (Menu_2_Array.Length > 0)
                    {
                        Menu_List += "<li class='dropdown'><a class='Menu_Arrow_Down' href='#'" + Menu_1_On_Click + "><span class='Bold_White_Text_css'>" + Menu_1_Array[i1] + "#Total_Menu_2#" + "</span></a>";

                        Menu_List += "<ul class='sub-menu'>";

                        int Total_Menu_2 = 0;

                        //
                        for (int i2 = 0; i2 < Menu_2_Array.Length; i2++)
                        {
                            if (!Menu_2_URL_Array[i2].StartsWith("X"))
                            {
                                Total_Menu_2++;

                                //Menu_3
                                string[] Menu_3_Array = new string[0];
                                string[] Menu_3_ID_Array = new string[0];
                                string[] Menu_3_URL_Array = new string[0];
                                string[] Menu_3_CMD_Array = new string[0];

                                //
                                string Menu_2_Color = "White_Text_css";

                                //
                                string Menu_2_On_Click = " onclick=\"Menu_On_Click('" + Menu_2_Array[i2] + "', '" + Menu_2_URL_Array[i2] + "'); return false;\"";

                                //
                                if (Menu_3_Array.Length > 0)
                                {
                                    Menu_List += "<li class='dropdown'><a class='Menu_Arrow_Right' href='#'" + Menu_2_On_Click + "><span class='" + Menu_2_Color + "'>" + Total_Menu_2.ToString() + ". " + Menu_2_Array[i2] + "</span></a>";

                                    Menu_List += "<ul class='sub-menu'>";

                                    //
                                    for (int i3 = 0; i3 < Menu_3_Array.Length; i3++)
                                    {
                                        //
                                        string Menu_3_On_Click = string.Empty;

                                        if (Menu_3_URL_Array[i3] != string.Empty)
                                        {
                                            Menu_3_On_Click = " onclick=\"Menu_On_Click('" + Menu_3_Array[i3] + "', '" + Menu_3_URL_Array[i3] + "'); return false;\"";
                                        }

                                        //
                                        Menu_List += "<li class='dropdown'><a href='#'" + Menu_3_On_Click + "><span class='White_Text_css'>" + Menu_3_Array[i3] + "</span></a>";
                                        Menu_List += "</li>";
                                    }

                                    //End 3
                                    Menu_List += "</ul>";
                                    Menu_List += "</li>";
                                }
                                else
                                {
                                    Menu_List += "<li class='dropdown'><a href='#'" + Menu_2_On_Click + "><span class='" + Menu_2_Color + "'>" + Total_Menu_2.ToString() + ". " + Menu_2_Array[i2] + "</span></a></li>";
                                }
                            }
                        }

                        //
                        if (Total_Menu_2 > 0)
                        {
                            //Menu_List = Menu_List.Replace("#Total_Menu_2#", " (<span class='Bold_White_Text_css'>" + Total_Menu_2.ToString() + " / " + Menu_2_Array.Length + " New Features</span>)");
                            Menu_List = Menu_List.Replace("#Total_Menu_2#", string.Empty);
                        }
                        else
                        {
                            Menu_List = Menu_List.Replace("#Total_Menu_2#", string.Empty);
                        }

                        //End 2
                        Menu_List += "</ul>";
                        Menu_List += "</li>";
                    }
                    else
                    {
                        Menu_List += "<li class='dropdown'><a href='#'" + Menu_1_On_Click + "><span class='Bold_White_Text_css'>" + Menu_1_Array[i1] + "</span></a></li>";
                    }
                }
            }

            //End 1
            Menu_List += "</ul>";

            //All
            Menu_div.InnerHtml = "<nav>" + Menu_List + "</nav>";
        }
    }
}

