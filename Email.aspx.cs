using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Email : System.Web.UI.Page
{
    BusLogic bl = new BusLogic();
    bool emailGrid;
    int emailId;
    string userId;
    string PassedDate;
    EmailDataContext emailDB = new EmailDataContext();
    BusLogic.GetApplicationValues bus = new BusLogic.GetApplicationValues();
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["SessionId"] == null)
        {
            Response.Redirect("Login.aspx", false);
        }
        else
        {
            if (Request.QueryString["enum"] != null)
            {
                userId = (Request.QueryString["enum"].ToString());
                userId = Application["UserId"].ToString();
                //if (!bl.IsValidUser(userId))
                //{
                //    Response.Redirect("Login.aspx", false);
                //}
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = "TimeKeeper Pro&#xA9 | Email";
        hfUserId.Value = userId.ToString();
        if (!IsPostBack)
        {
            LoadEmailList(Application["UserId"].ToString());
            LoadWeekEndDate();
        }
    }
    protected void LoadWeekEndDate()
    {
        double numDay = 7.0;
        double numDay1 = -7.0;
        string day = DateTime.Today.AddDays(numDay1).ToShortDateString();
        string day1 = DateTime.Today.AddDays(numDay).ToShortDateString();
        ddlWeekEndDate.DataTextField = "Day";
        ddlWeekEndDate.DataValueField = "Dates";
        ddlWeekEndDate.DataSource = bl.PopulateDates(day, day1);
        ddlWeekEndDate.DataBind();

    }
    protected void LoadEmailList(string userId)
    { 
        ddlEmalList0.Items.Add(new ListItem(string.Empty,"NoCC"));
        if (userId != null)
        {
            var emails = (from j in emailDB.tblEmails
                          where j.newId.ToString() == userId
                          orderby j.EmailId
                          select j);
            foreach (var p in emails)
            {
                ddlEmalList.Items.Add(new ListItem(p.EmailAddress, p.EmailId.ToString()));              
                ddlEmalList0.Items.Add(new ListItem(p.EmailAddress, p.EmailId.ToString()));
            }
        }
        else
            return;
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        if (ddlEmalList.SelectedIndex > -1 && ddlWeekEndDate.SelectedIndex > -1)
        {
            PassedDate = ddlWeekEndDate.SelectedValue;
            string emailId = ddlEmalList.SelectedValue.ToString();
            string emailId0 = ddlEmalList0.SelectedValue.ToString();
            string concatEmailIds = emailId +";"+ emailId0;
            string[] emailIds = concatEmailIds.Split(new char[] { ';' });
            string url = string.Format("TimeEntryHistory.aspx?enum={0}&task=goEmail&eid={1}&date={2}", Application["UserId"].ToString(), concatEmailIds, PassedDate);
            Response.Redirect(url);
        }
    }
    public void ShowClientFunction(string function)
    {
        string str = "<script language='javascript' type='text/javascript'>" + function + "</script>";
        Page.ClientScript.RegisterStartupScript(GetType(), "key", str);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(HashData().EmailAddress))
            {
                if (!CheckIfExists(HashData().EmailAddress))
                {
                    if (bl.ValidateEmail(HashData().EmailAddress))
                    {
                        emailDB.tblEmails.InsertOnSubmit(HashData());
                        emailDB.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                }
            }

            else
            {
                ShowClientFunction("alert('Please enter a valid email address.')");
            }
        }
        catch (Exception)
        {
            throw new Exception("Insert of email address failed!");
        }
        finally
        {
            ddlEmalList.Items.Clear();
            ddlEmalList0.Items.Clear();
            LoadEmailList(userId);
        }
    }

    public virtual tblEmail HashData()
    {   
        tblEmail email = new tblEmail();
        email.newId = Guid.Parse(userId);
        if (!String.IsNullOrEmpty(txtFname.Text))
            email.FirstName = txtFname.Text;
        if (!String.IsNullOrEmpty(txtLname.Text))
            email.LastName = txtLname.Text;
        if (!String.IsNullOrEmpty(txtEmail.Text))
            email.EmailAddress = txtEmail.Text;
        return email;
    }

    public bool CheckIfExists(string val)
    {
        bool w = false;
        bool retV = false;
        var v = (from s in emailDB.tblEmails
                 where s.newId.ToString() == userId
                 select s.EmailAddress);
        foreach (var n in v)
        {
            if (!string.IsNullOrEmpty(n))
            {
                if (n.Trim().Equals(val.Trim()))
                    retV = w = true;
            }
        }
        return w;
    }

}