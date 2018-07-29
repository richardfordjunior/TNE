using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
public partial class Edit : System.Web.UI.Page
{
    TNEDataContext tne = new TNEDataContext();
    WorkDescDataContext workDescDB = new WorkDescDataContext();
    LocationsDataContext locDB = new LocationsDataContext();
    BusLogic bus = new BusLogic();
    ArrayList appList = new ArrayList();
    string Loaduser = string.Empty;
    string row = string.Empty;
    string Location, WorkDescription = string.Empty;
    private object thislock = new object();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManagerProxy proxy = new ScriptManagerProxy();
        
        if (Request.QueryString.Count != 0 && (Request.QueryString[0]!=null && Request.QueryString[1]!=null))
        {
            Loaduser = Request.QueryString[0].ToString();
            if (Application["UserId"] != null)
                Loaduser = Application["UserId"].ToString();
        {
            //Guid userId = Guid.Parse(Application["UserId"].ToString());
        }
            row = Request.QueryString[1].ToString();
        }
        if(!IsPostBack)
        {
            LoadData(Loaduser, row);
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsDirty())
            {
                lock (thislock)
                {
                    HashData();
                    if (!CheckIfExists(0, txtClient.Text))
                    {
                        tblLocation loc = new tblLocation();
                        loc.LocationName = txtClient.Text;
                        loc.UserId = int.Parse(Loaduser);
                        locDB.tblLocations.InsertOnSubmit(loc);
                        locDB.SubmitChanges();
                    }

                    if (!CheckIfExists(1, txtWorkDesc.Text))
                    {
                        tblWorkDescription wk = new tblWorkDescription();
                        wk.WorkDescription = txtWorkDesc.Text;
                        wk.UserId = int.Parse(Loaduser);
                        workDescDB.tblWorkDescriptions.InsertOnSubmit(wk);
                        workDescDB.SubmitChanges();
                    }

                }
            }
        }
        catch (Exception t)
        {
            string err = t.StackTrace;
            appList.Add(err);
            bus.WriteToErrorLog(appList);
        }
        finally
        {
            ShowClientFunctionInUpdatePanel("alert('Record updated successfully!!');");
            ShowClientFunctionInUpdatePanel("CloseMe();");
        }
    
    
    }   

    protected void LoadData(string u,string r)
    {
        var loadData = (from t in tne.tblTimeExpensesSummaries
                        where t.UserId == int.Parse(u) && t.Id == int.Parse(r)
                        select t).Single();
        txtDate.Text = ((DateTime)loadData.Date).ToShortDateString();
        txtClient.Text = loadData.Location;
        txtHours.Text = loadData.WorkHrs.ToString();
        txtWorkDesc.Text = loadData.WorkDescription.ToString();
        txtComments.Text = loadData.Comments;
        if (loadData.PaymentReceived == null || loadData.PaymentReceived == false)
            cbPayment.Checked = false;
        else
            cbPayment.Checked = true;
    }

    protected void HashData()
    {
        var loadData = (from t in tne.tblTimeExpensesSummaries
                        where t.UserId == int.Parse(Loaduser) && t.Id == int.Parse(row)
                        select t).Single();
        loadData.Date = DateTime.Parse(txtDate.Text);
        loadData.WorkHrs = Decimal.Parse(txtHours.Text);
        loadData.Location = txtClient.Text;
        loadData.WorkDescription = txtWorkDesc.Text;
        loadData.Comments = txtComments.Text;
        loadData.PaymentReceived = cbPayment.Checked;
        loadData.LastUpdated = DateTime.Now;
        tne.SubmitChanges();
        //Update History table
        var loadHistData = (from t in tne.tblTimeExpensesSummary_histories
                        where t.UserId == int.Parse(Loaduser) && t.Id == int.Parse(row)
                        select t).Single();
        loadHistData.Date = DateTime.Parse(txtDate.Text);
        loadHistData.WorkHrs = Decimal.Parse(txtHours.Text);
        loadHistData.Location = txtClient.Text;
        loadHistData.WorkDescription = txtWorkDesc.Text;
        loadHistData.Comments = txtComments.Text;
        loadHistData.PaymentReceived = cbPayment.Checked;
        loadHistData.LastUpdated = DateTime.Now;
        tne.SubmitChanges();       
    }
    public bool CheckIfExists(int chk, string val)
    {
        bool q = false;
        bool w = false;
        bool retV = false;
        if (chk == 0)
        {
            if (bus.NumRecords(1) >= 0)
            {
                var v = (from s in locDB.tblLocations
                         where s.UserId == int.Parse(Loaduser)
                         select s.LocationName);
                foreach (var n in v)
                {
                    if (n.Trim().Equals(val.Trim()))
                        retV = w = true;
                }
            }
        }
        if (chk == 1)
        {
            if (bus.NumRecords(2) >= 0)
            {
                var v = (from s in workDescDB.tblWorkDescriptions
                         where s.UserId == int.Parse(Loaduser)
                         select s.WorkDescription);
                foreach (var n in v)
                {
                    if (n.Trim().Equals(val.Trim()))
                        retV = w = true;
                }
            }
        }
        return w;
    }

    public void ShowClientFunctionInUpdatePanel(string fX)
    {
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                 Guid.NewGuid().ToString(), fX, true);
    }

    public bool IsDirty()
    {
        bool dirty = false;
        var loadData = (from t in tne.tblTimeExpensesSummaries
                        where t.UserId == int.Parse(Loaduser) && t.Id == int.Parse(row)
                        select t).Single();
        if (loadData.Date != DateTime.Parse(txtDate.Text))
            dirty = true;
        if (loadData.WorkHrs != Decimal.Parse(txtHours.Text))
            dirty = true;
        if (loadData.Location != txtClient.Text)
            dirty = true;
        if (loadData.Comments != txtComments.Text)
            dirty = true;
        if (loadData.PaymentReceived != cbPayment.Checked)
            dirty = true;
        return dirty;
    }


 
}