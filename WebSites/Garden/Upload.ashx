<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using _4u4m;

public class Upload : IHttpHandler
{

    public void ProcessRequest(HttpContext Http_Context)
    {
        new _4e().Upload_ashx_ProcessRequest();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}