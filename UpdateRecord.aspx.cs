using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UpdateRecord : System.Web.UI.Page
{
    BusLogic bl = new BusLogic();
    AdminDataContext ad = new AdminDataContext();
    string userId ;
    int recId = 0;
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["SessionId"] == null)
        {
            Response.Redirect("Login.aspx", false);
        }
 
        if (Request.QueryString["enum"] != null)
        {
            userId = Request.QueryString["enum"].ToString();            
        }

        if (Request.QueryString["recNum"] == null || Request.QueryString["recNum"] == "")
        {
            Response.Redirect("Admin.aspx?enum=" + Application["UserId"], false);
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {      
      recId = int.Parse(Request.QueryString["recNum"].ToString());
      if (!Page.IsPostBack)
      {
          var t = (from j in ad.tblLogonIds
                   where j.Id == recId
                   select j).Single();
          txtUser.Text = t.UserName;
          txtEmail.Text = t.emailAddress;
          txtRole.Text = t.Role.ToString();
      }


      if (Session["Role"].ToString() == "1")
      {

              SiteMaster master = (SiteMaster)this.Master;
              Menu mnu = ((Menu)master.FindControl("NavigationMenu"));
              MenuItem item = new MenuItem("Admin Console");
              for (int k = 0; k < mnu.Items.Count; k++)
              {
                  
                  if (mnu.Items[k].Value == item.Text)
                  {//Don't add
                     continue;
                  }
                  else
                  {
                      //mnu.Items.Add(item);
                  }
                      
              }         
      }
    }

 
    public virtual void SaveData()
    {
        var newData = (from p in ad.tblLogonIds
                       where p.Id == recId
                       select p).Single();
        if(!string.IsNullOrEmpty(txtUser.Text))
           newData.UserName = txtUser.Text;
        if (!string.IsNullOrEmpty(txtEmail.Text))
        {
            if(bl.ValidateEmail(txtEmail.Text))
            {
                newData.emailAddress = txtEmail.Text;
            }
        }           
        if (!string.IsNullOrEmpty(txtRole.Text))
           newData.Role = int.Parse(txtRole.Text);
          try
          {
              ad.SubmitChanges();
          }
          catch (Exception)
          {
          }
          finally
          {
              string url = string.Format("Admin.aspx?enum={0}", userId);
              Response.Redirect(url);
          }
    }

    protected void btnSubmitChanges_Click(object sender, EventArgs e)
    {           
        SaveData();
    }
} 