using _IQwinwin;
using System;
using System.Web.Services;

public partial class Switch_Point : System.Web.UI.Page
{
    ai onPageLoad = " Switch_Point_Onload();";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!a.url.IsPageMethods_Undefined)
        {
            ai UserName = lib.Read_UserName();

            ai Ho_Va_Ten = a.e;
            ai Phong_Ban = a.e;

            bool Duoc_Xem_Bao_Cao = false;
            bool Duoc_Tich_Diem = false;
            bool Duoc_Tru_Diem = false;
            bool Duoc_Doi_Diem = false;

            crm.Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

            if (!Duoc_Doi_Diem)
            {
                Response.Redirect(a.url.DomainHttp);
            }
            else
            {
                if (!IsPostBack)
                {
                    lib.Set_Index_Host(Index_Host_hdf, null);
                    PageMethods_Path_hdf.Value = "/Tool/Switch_Point.aspx";
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
    public static string Read_Card_Info(string Card)
    {
        return crm.Read_Card_Info(Card);
    }

    [WebMethod(enableSession: true)]
    public static string Submit_Switch_Point(string Card_1, string Card_2)
    {
        return crm.Submit_Switch_Point(Card_1, Card_2);
    }
}