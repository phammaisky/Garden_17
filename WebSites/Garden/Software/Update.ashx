<%@ WebHandler Language="C#" Class="Update" %>

using System;
using System.Web;
using _4u4m;

public class Update : IHttpHandler
{

    public void ProcessRequest(HttpContext Http_Context)
    {
        new _4e().Update_ashx_ProcessRequest();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}