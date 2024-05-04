using _IQwinwin;
using System;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Add_Point : System.Web.UI.Page
{
    ai onPageLoad = " Add_Point_Onload();";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!a.url.IsPageMethods_Undefined)
        {
            ai UserName = lib.Read_UserName();

            if (UserName.ToLower() == "Pos2".ToLower())
            {
                Is_POS_hdf.Value = "1";
            }

            ai Ho_Va_Ten = a.e;
            ai Phong_Ban = a.e;

            bool Duoc_Xem_Bao_Cao = false;
            bool Duoc_Tich_Diem = false;
            bool Duoc_Tru_Diem = false;
            bool Duoc_Doi_Diem = false;

            crm.Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

            if (!Duoc_Tich_Diem && !Duoc_Tru_Diem && !Duoc_Doi_Diem)
            {
                Duoc_Tich_Diem = true;
            }

            if (!Duoc_Tich_Diem && !Duoc_Tru_Diem && !Duoc_Doi_Diem)
            {
                Response.Redirect(a.url.DomainHttp);
            }
            else
            {
                //
                if (!IsPostBack)
                {
                    lib.Set_Index_Host(Index_Host_hdf, null);
                    PageMethods_Path_hdf.Value = "/Tool/Add_Point.aspx";

                    Loggedin_UserId_hdf.Value = a.aS(ViewState["UserName"]);

                    Today_Year_hdf.Value = a.Time.Year.aS();
                    Today_Month_hdf.Value = a.Time.Month.aS();
                    Today_Day_hdf.Value = a.Time.Day.aS();

                    //
                    ai Computer = a.e;
                    ai Domain = a.e;
                    ai UserName_Services = a.e;

                    ai QuayBan = a.e;
                    ai CaBan = a.e;
                    ai NVBan = a.e;

                    crm.Read_Iam(31, out Computer, out Domain, out UserName_Services, out QuayBan, out CaBan, out NVBan);

                    if (QuayBan == a.e)
                    {
                        //QuayBan = Computer;
                    }

                    if ((UserName.ToLower() == "pos2") || (UserName_Services.ToLower() == "pos2"))
                    {
                        if (NVBan == a.e)
                        {
                            if (UserName_Services != a.e)
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
            }
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!a.url.IsPageMethods_Undefined)
        {
            lib.Add_All_JavaScript_AND_CSS_File_To_Header_Basic(true);
            lib.Add_All_JavaScript_AND_CSS_File_To_Header();

            if (!IsPostBack)
                Page_Body.Attributes.Add("onload", onPageLoad);
            else
                lib.Run_JavaScript(onPageLoad);
        }
    }

    [WebMethod(enableSession: true)]
    public static string Creat_Shop_List(string Shop_Name_OR_Code)
    {
        return crm.Creat_Shop_List(Shop_Name_OR_Code);
    }

    [WebMethod(enableSession: true)]
    public static string Creat_Search_Name_List(string Search_Name_OR_Phone)
    {
        return crm.Creat_Search_Name_List(Search_Name_OR_Phone);
    }

    [WebMethod(enableSession: true)]
    public static string Read_Receipt_Info(string Receipt)
    {
        return crm.Read_Receipt_Info(Receipt);
    }

    [WebMethod(enableSession: true)]
    public static string Read_Card_Info(string Card)
    {
        return crm.Read_Card_Info(Card);
    }

    [WebMethod(enableSession: true)]
    public static string Submit_Add_Point(string Submit_Add_Point_JSON)
    {
        return crm.Submit_Add_Point(Submit_Add_Point_JSON);
    }

    [WebMethod(enableSession: true)]
    public static string Read_Iam_Info()
    {
        return crm.Read_Iam_Info();
    }
}