<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
  
      
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        Server.ClearError();
        Application.Clear();
    
    }

        
    void Application_Error(object sender, EventArgs e) 
    {
        ErrorLoggingDataContext error = new ErrorLoggingDataContext();
        ErrorStack t = new ErrorStack();
        BusLogic bus = new BusLogic();
        string user = string.Empty;
        string errString = string.Empty;
        string requestorIP = Request.ServerVariables["REMOTE_ADDR"].ToString();
        string browser = Request.Browser.Browser;
        string requestType = Request.RequestType;
        errString += Server.GetLastError().InnerException; //Stacktrace
        DateTime logdate = DateTime.Now;//CreatedDate       
        errString += "Browser: "+ browser + ';' +"\nForm Method: "+ requestType + " from IP Address: " + requestorIP;
        //Insert Errors       
        t.CreatedDate = logdate;
        t.StackTrace = errString;
        t.RequestorIp = requestorIP;
        try
        {
            error.ErrorStacks.InsertOnSubmit(t);
            error.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
            //Send email to administrator
            string emailFrom = System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString();
            string emailTo = System.Configuration.ConfigurationManager.AppSettings["Support"].ToString();
            string subject = "Application Error";
            //bus.SendEmail(emailFrom, emailTo, subject, errString);
           // Response.Redirect("Error.aspx");
        }
        catch(Exception){
            }
        
       
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started  
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        Session.Clear();

    }
       
</script>
