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
using System.Net;

public partial class About : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new _4e().Set_Index_Host(Index_Host_hdf, null);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        new _4e().Add_ALL_JavaScript_AND_CSS_File_To_Header_Basic(Index_Host_hdf.Value);
    }
}