using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class LogOut : System.Web.UI.Page
{

    protected override void  OnPreInit(EventArgs e)
    {        
        AdminDataContext ad = new AdminDataContext();
        var users = from g in ad.tblLogonIds
                    select g;
        foreach (var f in users)
        {
            if ((bool)f.IsAuthenticated)
            {
                f.IsAuthenticated = false;
                ad.SubmitChanges();
            }        
        }
        SiteMaster MyMasterObj = (SiteMaster)this.Master;
        Menu mnu = ((Menu)MyMasterObj.FindControl("NavigationMenu"));
        LinkButton lnk = ((LinkButton)MyMasterObj.FindControl("LinkButton1"));
        Label lbl = ((Label)MyMasterObj.FindControl("lblVersion"));
        mnu.Visible = false;
        lnk.Visible = false;
        lbl.Visible = false;
 	 base.OnPreInit(e);
    }
    

    protected void Page_Load(object sender, EventArgs e)
    {
             
    }
    protected override void OnUnload(EventArgs e)
    {
        Session["SessionId"] = null;
        Session.Abandon();
        Session.Clear();       
        Application.RemoveAll();
        base.OnUnload(e);
    }

}