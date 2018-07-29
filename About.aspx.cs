using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


public partial class About : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["SessionId"] == null)
        {
            Response.Redirect("Login.aspx", false);
        }
     
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DataBindHelper [] help = new DataBindHelper[3];
        help[0] = new DataBindHelper("Test");

    }

  
}