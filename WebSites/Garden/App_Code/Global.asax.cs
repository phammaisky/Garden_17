using System;
using _4u4m;

/// <summary>
/// Summary description for Global
/// </summary> 
public class Global : System.Web.HttpApplication
{
    void Application_Error(object sender, EventArgs e)
    {
        //This method is invoked whenever an unhandled exception occurs in the application.
        // Code that runs when an unhandled error occurs        

        //Bat loi toan bo APP, Page
        new _4e().Global_asax_Application_Error(sender, e);
    }

    void Application_Start(object sender, EventArgs e)
    {
        
    }


    //Kích hoạt mỗi lần Có Request
    void Application_BeginRequest(object sender, EventArgs e)
    {
        Application["Password_SQL"] = "123@123a";

        //Initial Catalog='A4u4m4e'; 
        if (Application["Sql_Connection_String"] == null)
        {
            if (Application["Password_SQL"] != null)
            {
                Application["Sql_Connection_String"] = "Data Source='" + new _4e().Read_DataBase_Source() + "'; User ID='sa'; Password='" + Application["Password_SQL"] + "'; MultipleActiveResultSets=true; Pooling=true; Max Pool Size=32767; Min Pool Size=1;";
            }
        }

        //CRM
        if (Application["Sql_Connection_String_DB"] == null)
        {
            Application["Sql_Connection_String_DB"] = "Data Source='10.15.40.16'; User ID='sa'; Password='123@123a'; MultipleActiveResultSets=true; Pooling=true; Max Pool Size=32767; Min Pool Size=1;";
        }

        //CRM
        if (Application["Sql_Connection_String_QLTS"] == null)
        {
            Application["Sql_Connection_String_QLTS"] = "Data Source='10.15.40.181'; User ID='sa'; Password='123@123a'; MultipleActiveResultSets=true; Pooling=true; Max Pool Size=32767; Min Pool Size=1;";
        }

        //
        new _4e().Global_asax_Application_BeginRequest(sender, e);
    }
}