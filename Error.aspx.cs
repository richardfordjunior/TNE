using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        SiteMaster myMaster = (SiteMaster)this.Master;
        SiteMaster MyMasterObj = (SiteMaster)this.Master;
        Menu mnu = ((Menu)MyMasterObj.FindControl("NavigationMenu"));
        LinkButton lnk = ((LinkButton)MyMasterObj.FindControl("LinkButton1"));
        Label lbl = ((Label)MyMasterObj.FindControl("lblVersion"));
        mnu.Visible = false;
        lnk.Visible = false;
        lbl.Visible = false;
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}