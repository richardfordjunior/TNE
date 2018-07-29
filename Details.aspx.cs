using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Details : System.Web.UI.Page
{
    TNEDataContext t = new TNEDataContext();
    BusLogic bl = new BusLogic();
    
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["SessionId"] == null)
        {
            Response.Redirect("Login.aspx", false);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

       this.Page.Title = "TimeKeeper Pro&#xA9 | Details";
       SiteMaster mast = (SiteMaster)this.Master;
       Menu mnu = ((Menu)mast.FindControl("NavigationMenu"));
       mnu.Visible = false;
       LoadData();
    }
                                                                                                      
    protected void LoadData()
    {
        String userid ="";
        try

        {
          
              userid = Request.QueryString["enum"].ToString();
                    int rowNum = int.Parse(Request.QueryString[1].ToString());

            var data = (from x in t.tblTimeExpensesSummaries
                        where x.NewId.ToString() == userid && x.Id == rowNum
                        select x.Comments);
            dView.DataSource = data.ToList();
            dView.DataBind();
        }
        catch (Exception)
        {
           
        }
    }

 
}