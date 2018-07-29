<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;

public class Handler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello To The World");
        //context.Response.TransmitFile(@"C:\Users\Rich\Documents\TestFile.txt");
        context.Response.Write(context.Response.StatusDescription);
        context.Response.Write(context.Response.IsClientConnected.ToString());
       context.Response.AddFileDependency(@"C:\Users\Rich\Documents\TestFile.txt");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}