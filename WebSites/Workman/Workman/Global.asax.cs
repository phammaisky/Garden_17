using _IQwinwin;
using System;
using System.Web;

public class Global : HttpApplication
{
    void Application_Start(object sender, EventArgs e)
    {
        sys.Application_Start(sender, e);
        new _IQ().Application_Start(sender, e);
    }

    void Application_Error(object sender, EventArgs e)
    {
        sys.Application_Error(sender, e);
        new _IQ().Application_Error(sender, e);
    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        sys.Application_BeginRequest(sender, e);
        new _IQ().Application_BeginRequest(sender, e);
    }
}