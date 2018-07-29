using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Permissions;
using System.Text;
using System.Security.Cryptography;
using System.Security;

public partial class LogIn : System.Web.UI.Page
{
    AdminDataContext admin = new AdminDataContext();
    BusLogic b = new BusLogic();
    string u, p;
    string validUserid;
    protected void Page_Init(object sender, EventArgs e)
    {
        //reset all users 
        var reset = from a in admin.tblLogonIds
                    select a;
        foreach (var user in reset)
        {
            user.IsAuthenticated = false;
        }
        admin.SubmitChanges();
                    
        SiteMaster MyMasterObj = (SiteMaster)this.Master;
        Menu mnu = ((Menu)MyMasterObj.FindControl("NavigationMenu"));
        LinkButton lnk = ((LinkButton)MyMasterObj.FindControl("LinkButton1"));
        Label lbl = ((Label)MyMasterObj.FindControl("lblVersion"));
        mnu.Visible = false;
        lnk.Visible = false;
        lbl.Visible = false;
        if (!IsPostBack)
        {   
            TextBox txtUser = (TextBox)Login1.FindControl("UserName");
            if (Request.Cookies["RememberMe"] != null)
            {
                HttpCookie cookie = Request.Cookies.Get("RememberMe");
                Login1.UserName = cookie.Values["username"];
                Login1.RememberMeSet = (!String.IsNullOrEmpty(Login1.UserName));
            }
          
            if (txtUser != null)
                this.SetFocus(txtUser);
        }
        Response.Cache.SetNoStore();
    }
    protected void Page_Load(object sender, EventArgs e)
    {        
          if(!IsPostBack)
           ViewState["LogAttempts"] = 0;        
    }
    public void ShowClientFunctionInUpdatePanel(string fX)
    {
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                 Guid.NewGuid().ToString(), fX, true);
    }

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {

         p = Login1.Password;
         u = Login1.UserName; 
        
         string f = b.getMd5Hash(Login1.Password);
         string url = string.Empty;
        if (Authenticate(u, p))
        {
            Session["SessionId"] = HttpContext.Current.Session.SessionID;
            this.Session["SessionStartDateTime"] = DateTime.Now;
           
            SiteMaster MyMasterObj = (SiteMaster)this.Master;
            Menu mnu = ((Menu)MyMasterObj.FindControl("NavigationMenu"));
            LinkButton lnk = ((LinkButton)MyMasterObj.FindControl("LinkButton1"));
            Label lbl = ((Label)MyMasterObj.FindControl("lblVersion"));
            lbl.Visible = true;
            lnk.Visible = true;
            mnu.Visible = true;
            var t = from x in Select(u)
                    select new
                    {
                        x.Role,
                        x.Id,
                        x.newId
                    };

            foreach (var r in t)
            {
                Application.Add("NewId", r.newId);
                Application.Add("UserId", r.newId);
                Session.Add("UserId", r.newId);
                Session.Add("Role", r.Role);
                Application.Add("Id", r.Id);
                url = string.Format("TimeEntry.aspx?enum={0}", Session["UserId"]);
                validUserid = r.newId.ToString();
            }

   

            //Update valid user
            AdminDataContext a = new AdminDataContext();
            var val = a.tblLogonIds.Single(d => d.newId.ToString() == validUserid);
            val.IsAuthenticated = true;
            a.SubmitChanges();
        
            //Set User Cookie
            HttpCookie myCookie = new HttpCookie("RememberMe");
            Boolean remember = Login1.RememberMeSet;
            if (remember)
            {
                Int32 persistDays = 15;
                myCookie.Values.Add("username", Login1.UserName);
                myCookie.Expires = DateTime.Now.AddDays(persistDays);
            }
            else
            {
                myCookie.Values.Add("username", string.Empty);
                myCookie.Expires = DateTime.Now.AddMinutes(5);
            }

            Response.Cookies.Add(myCookie);
            BusLogic.GetApplicationValues ba = new BusLogic.GetApplicationValues();
            ba.UId = validUserid.ToString();
            Application.Add("UserId",ba.UId);
            Response.Redirect(url,false);

        }
    }


    protected bool Authenticate(string tryUser, string tryPassword)
    { //Uncomment before publish
        //BusLogic b = new BusLogic();      
        bool val = false;
        var q = (from u in admin.tblLogonIds
                 select new
                     {
                         u.Id,
                         u.UserName,
                         u.Password,
                         u.IsAuthenticated,
                         u.newId
                     }
                 );

        foreach (var test in q)
        {
            if (test.UserName == tryUser && test.Password == b.getMd5Hash(tryPassword))
            {
                val = true;
                Update(test.newId.ToString());
            }
        }
        return val;
        val= true;
        return val;
    }


    public void Update(string id)
    {
        try
        {
            AdminDataContext t = new AdminDataContext();
            var val = t.tblLogonIds.Single(p => p.newId.ToString() == id);
            val.LoginDate = DateTime.Now;
            t.SubmitChanges();
            ViewState["logged"] = val.LoginDate.ToString();            
        }   
        catch 
        {
            Exception ex = new Exception();
            string err = ex.InnerException.ToString();
        }
    }
   
    public IEnumerable<tblLogonId> Select(string user)
    {
        AdminDataContext ad = new AdminDataContext(); 
        return ad.tblLogonIds.Where(p => p.UserName == user);      
    }


  




 
}