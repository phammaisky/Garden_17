<%@ WebHandler Language="C#" Class="Upload" %>

using _IQwinwin;
using System.Web;
    
public class Upload : IHttpHandler
{
    public void ProcessRequest(HttpContext Http_Context)
    {
        ams.AmsUpload();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}