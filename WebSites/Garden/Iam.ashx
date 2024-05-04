<%@ WebHandler Language="C#" Class="Iam" %>

using System;
using System.Web;
using _4u4m;

public class Iam : IHttpHandler
{

    public void ProcessRequest(HttpContext Http_Context)
    {
        new _4e().Iam_ashx_ProcessRequest();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}