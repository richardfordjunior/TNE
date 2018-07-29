using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    String user;
    BusLogic b = new BusLogic();
    string list = string.Empty;
    Double timeOut = 20 * .25;
    DateTime bTime = HttpContext.Current.Timestamp;
    DateTime ticker = HttpContext.Current.Timestamp;
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        //Response.AppendHeader("Refresh", "10000;URL=LogOut.aspx");
    }


    public void LogOut()
    {
        Session["SessionStartDateTime"] = null;
        Response.Redirect("LogOut", true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      
        CheckElaspedSessionTime(DateTime.Now);
        if (Application["UserId"] != null)
        {
            Guid userId = Guid.Parse(Application["UserId"].ToString());
        }

       
        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:noBack();", true);
        list = b.GetCityFromIP(Request.ServerVariables["REMOTE_ADDR"].ToString());
        //list = String.Empty;
        if (list != string.Empty)
        {
            //list = b.GetCityFromIP(Request.ServerVariables["REMOTE_ADDR"].ToString());
            string[] ar = list.Split(new char[] { ';' });
            lblCityState.Text = string.Format("This page is being requested from {0} located in {1},{2}."
        , ar[0].ToString(), ar[1].ToString(), ar[3].ToString());

        }
        else
        {
            lblCityState.Text = "Unable to locate IP";
        }



    }

    protected void NavigationMenu_MenuItemClick(object sender, MenuEventArgs e)
    {
         for (int k = 0; k < NavigationMenu.Items.Count; k++)
         {
             if (NavigationMenu.Items[k].Selected)
             {
                 string url = string.Empty;

                 switch (NavigationMenu.Items[k].Value)
                 {
                     case "About":
                         url = "About.aspx";                        
                         break;
                     case "Time Entry":
                         url = "TimeEntry.aspx";                     
                         break;
                     case "Time Entry History":
                         url = "TimeEntryHistory.aspx";                  
                         break;
                     case "Admin Console":
                         url = "Admin.aspx";             
                         break;
                 }
                 if (url != string.Empty)
                 {
                     if (Application["UserId"] != null)
                     {
                         url = string.Format("GetMenu('{0}?enum={1}');", url, Application["UserId"].ToString());
                         ShowClientFunctionInMaster(url);
                     }
                 }
             }
         }
    }

    protected void NavigationMenu_MenuItemDataBound(object sender, MenuEventArgs e)
    {

    }

    public static bool CheckUID(string expression)
    {
        if (expression != null)
        {
            Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
            return guidRegEx.IsMatch(expression.ToString());
        }
        return false;
    }
    protected override void OnError(EventArgs e)
    {

        HttpContext context = HttpContext.Current;
        Exception exception = context.Server.GetLastError();
        string errorInfo =
        "Offending URL: " + context.Request.Url.ToString() +
        "Source: " + exception.Source.ToString() +
        "Message: " + exception.Message.ToString() +
        "Stack trace: " + exception.StackTrace.ToString();
        context.Response.Write(errorInfo);
        b.SendEmail("postmaster@randtconsulting.com", "richard.ford@randtconsulting.com", "Error", errorInfo);
        // To let the page finish running we clear the error
        context.Server.ClearError();
        this.OnError(e);
    }

    protected override void OnInit(EventArgs e)
    {
            MenuItem Mnite = new MenuItem("Admin Console");
            Mnite.ToolTip = "Administrator's Page";

            if (Request.QueryString["enum"] != null)
            {
                if (Request.QueryString["enum"].ToString() != "")
                {
                    user = Request.QueryString["enum"].ToString();
                    Session["User"] = user;
                    Application["User"] = user;
                    if (!CheckUID(user))
                    {
                        Response.Redirect("Login.aspx", false);
                    }
                    if (Session["User"] == null || Application["User"] == null)
                    {
                        Response.Redirect("Login.aspx", false);
                    }

                    //if (!b.IsValidUser(user.ToString()))
                    //{
                    //    Response.Redirect("Login.aspx", false);
                    //}
                }
            }

            lblDateTime.Text = DateTime.Now.ToLongDateString();
            lblVersion.Text = System.Configuration.ConfigurationManager.AppSettings["Version"].ToString();

            string role = string.Empty;
            if (CheckUID(user))
            {
                if (user != null)
                {
                    try
                    {
                        var t = from x in b.Select(user.ToString())
                                select new
                                {
                                    x.Role,
                                    x.Id,
                                    x.UserName,
                                    x.emailAddress,
                                    x.newId
                                };

                        foreach (var r in t)
                        {
                            role = r.Role.ToString();
                            //Set Role
                            Session["Role"] = role;
                            lblUser.Text = String.Format("Welcome, {0}!", r.UserName);
                            hlEmailId.Text = r.emailAddress;
                            hlEmailId.NavigateUrl = string.Format("mailto:{0}", r.emailAddress);
                            if (!string.IsNullOrEmpty(r.emailAddress))
                            {
                                System.Configuration.ConfigurationManager.AppSettings["FromAddress"] = r.emailAddress;
                            }
                        }
                    }
                    catch (FormatException)
                    {
                        Response.Redirect("Login.aspx");
                    }
                    if (Session["Role"].ToString() == "1")
                    {
                        //Create new Menu item
                        if (!IsPostBack)
                            NavigationMenu.Items.Add(Mnite);
                    }

                    if (Session["Role"].ToString() == "2")
                    {

                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            base.OnInit(e);
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("LogOut.aspx", false);
    }


    public void ShowClientFunction(string function)
    {
        string str = "<script language='javascript' type='text/javascript'>" + function + "</script>";
        Page.ClientScript.RegisterStartupScript(GetType(), "key", str);
    }

    public void ShowClientFunctionInMaster(string fofX)
    {
        const string someScript = "alertMe";
        if (!this.Page.ClientScript.IsStartupScriptRegistered(this.GetType(), someScript))
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(),

                someScript, fofX, true);
        }
    }

    public void CheckElaspedSessionTime(DateTime CurrentTime)
    {
    if(Session.Contents["SessionStartDateTime"] != null)
        {
        Session["SessionStartTickerTime"] = ticker;
        Boolean IsExpired = false;
        DateTime dt = (DateTime)(Session.Contents["SessionStartDateTime"]);
        CurrentTime = DateTime.Now;
        ticker = DateTime.Now;
        TimeSpan ts = new TimeSpan();
        if (CurrentTime >= dt)
        {
            ts = CurrentTime.Subtract(dt);
            IsExpired = ts.Minutes >= 5;
            
        }

        if (IsExpired)
        {
            Response.Redirect("Logout.aspx", true);
        }
        }
    }
   



} 

