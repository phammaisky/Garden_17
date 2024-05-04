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

public partial class Login : System.Web.UI.Page
{
    string Time = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss:fff");

    string Message = string.Empty;
    string On_Page_Load = "Onload();";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.IsAuthenticated)
        {
            new _4e().Logout();

            Response.Redirect("/Default.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                new _4e().Set_Index_Host(Index_Host_hdf, null);
            }
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
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

    protected override void Render(HtmlTextWriter Html_Text_Writer_System)
    {
        using (HtmlTextWriter Html_Text_Writer_Temp = new HtmlTextWriter(new StringWriter()))
        {
            base.Render(Html_Text_Writer_Temp);
            string Rendered_Content = Html_Text_Writer_Temp.InnerWriter.ToString();

            //END
            Rendered_Content = Rendered_Content.Replace("http://Upload_Host.com/", Index_Host_hdf.Value + "/");
            Html_Text_Writer_System.Write(Rendered_Content);
        }
    }

    protected void Login_btn_Click(object sender, EventArgs e)
    {
        string UserName = UserName_tbx.Text;
        string Password = Password_tbx.Text;

        if (Membership.ValidateUser(UserName, Password))
        {
            FormsAuthentication.SetAuthCookie(UserName, false);

            //
            if (Request.QueryString["ReturnUrl"] != null)
            {
                string ReturnUrl = Request.QueryString["ReturnUrl"].ToString();
                On_Page_Load += " window.location.href='" + ReturnUrl + "';";
            }
            else
            {
                Response.Redirect("/Default.aspx");
            }
        }
        else
        {
            Message = "Incorrect Email or Password !";
            On_Page_Load += new _4e().Add_Alert_Message_To_On_Page_Load(Message);
        }
    }
}