using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Data.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Timers;
using System.Collections;
using System.Windows.Forms;
using System.Net;
using System.Web.Services.Discovery;



public partial class TimeEntry : System.Web.UI.Page
{
    WorkDescDataContext workDescDB = new WorkDescDataContext();
    LocationsDataContext locDB = new LocationsDataContext();
    TNEDataContext TNEDB = new TNEDataContext();
    Semaphore _pool = new Semaphore(2, 2);
    Decimal val;
    ArrayList appList = new ArrayList();
    ArrayList arList = new ArrayList();
    TNEDataContext tne = new TNEDataContext();
    BusLogic bus = new BusLogic();
    string Loaduser = string.Empty;
    string row = string.Empty;
    string Location, WorkDescription = string.Empty;
    bool flag;
    bool updateGrid = false;
    bool emailGrid;
    string startweek;
    string endweek;
    String userId;
    int RowIndex, rowCount,rowId,editRecordId;
    BusLogic bl = new BusLogic();
    protected void Page_Init(object sender, EventArgs e)
    {
     
        //Boolean v = bl.ValidateEmailAddress("richardfordjr@gmail.com");
        if (Request.QueryString["enum"] != null)
            userId = Request.QueryString["enum"].ToString();
        else
            Response.Redirect("Login.aspx");

        ContentPlaceHolder cp = (ContentPlaceHolder)Master.FindControl("MainContent");       
        foreach (System.Web.UI.Control c in cp.Controls)
        {  
            if (c is System.Web.UI.WebControls.TextBox)
            {
                arList.Add(c);
            }  
             
        }

        for (int k = 0; k < arList.Count; k++)
        {
            string val = ((System.Web.UI.WebControls.TextBox)arList[k]).Text;
        }                     
        string xdoc = "C:\\Users\\Rich\\Documents\\PurchaseOrder.xml";
        //XDocument data = XDocument.Load(xdoc);
        //XElement elem =
        //    (XElement)data.Descendants("Address").
        //    Where(c => c.Attribute("Type").Value == "Billing").FirstOrDefault();

        //var notes = from c in data.Descendants("PurchaseOrder")
        //            Where(c => c.Attribute("Type").Value == "Billing")
        //            select new
        //            {
                    
        //                nNotes = c.Element("DeliveryNotes").Value
        //            }

        //var d = from c in data.Descendants("Address").
        //        Where(c => c.Attribute("Type").Value == "Billing")
        //        select new
        //        {
        //            Name = c.Element("Name").Value,
        //            Street = c.Element("Street").Value,
        //            City = c.Element("City").Value,
        //            State = c.Element("State").Value,
        //            Country = c.Element("Country").Value,
        //            Zip = c.Element("Zip").Value
        //        };
   
        //XmlDocument doc = new XmlDocument();
        //doc.Load(xdoc);
        //XmlNodeList nodeList = doc.DocumentElement.SelectNodes("Address");
        //int nodeCount = doc.SelectNodes("PurchaseOrder").Count;
        ////XmlNodeList nodeList = doc.SelectNodes("PurchaseOrder");
        //Customer cust = new Customer();
        //foreach (XmlNode node in nodeList)
        //{
        //    if (node.Attributes.GetNamedItem("Type").Value == "Billing")
        //    {
        //        cust.Name = node["Name"].InnerText;
        //    }
        //}

        ////Customer cust = new Customer();
        //foreach (var x in d)
        //{
        //    cust.Name = x.Name;
        //    cust.Street = x.Street;
        //    cust.City = x.City;
        //    cust.State = x.State;
        //    cust.Zip = x.Zip;
        //}
        
        //foreach (var n in notes)
        //{
        //    string ntes = n.nNotes;
        //}
     
        ////Get list of elements
        // var allElements = (from p in data.Root.DescendantNodes().OfType<XElement>()
        //                   where p.Name == p.Name 
        //                   select p) .Distinct();
        // foreach (var name in data.Root.DescendantNodes().OfType<XElement>()
        //     .Select(x => x.Name).Distinct())
        // {
        //     string element = name.LocalName;
        // }

        
         if (Session["SessionId"] == null)
         {
             Response.Redirect("Login.aspx", false);
         }       
    }

    protected override object SaveViewState()
    {
        return base.SaveViewState();
    }

    protected override void OnLoad(EventArgs e)
    {
        List<tblTimeExpensesSummary> t = GetData(2);
        for (int k = 0; k < t.Count; k++)
        {
            string x = t[k].Comments;
        }
        //ShowClientFunctionInUpdatePanel("GetUserBrowser();");
        base.OnLoad(e);
    }
   
 

    public List<tblTimeExpensesSummary> GetData(int user)
    {
        var p = (from x in tne.tblTimeExpensesSummaries
                 where x.UserId == user
                 select x);
        return p.ToList<tblTimeExpensesSummary>();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool d = checkforcurl("hello I have a & in my text.");
       // btnSubmitAll.Click += new System.EventHandler(btnUpdate_Click);
        //String userId = Application["NewId"].ToString();
        string userId = Request.QueryString["enum"].ToString();
        BusLogic.BaseDt b = new BusLogic.BaseDt();
        b.name  = "Rich";
        string x= b.name;

        BusLogic bus = new BusLogic();

        {

            if (!IsPostBack)
            {
                Bind();
                BindSummary();
            }
            if (IsPostBack)
            {
                Bind();
            }
            if (!IsPostBack)
            {
                this.lblPageView.Text = string.Format("Viewing page {0} of {1}", "1", gView.PageCount.ToString());
                this.lblPageView.Text = string.Format("Viewing page {0} of {1}. Displaying records {2} thru {3}.", 1, gView.PageCount, 1, 10);
            }
            this.Page.Title = "TimeKeeper Pro&#xA9 | Time Entry";
            HttpSessionStateWrapper Wrapper = new HttpSessionStateWrapper(this.Session);
            int cnt = Wrapper.Count;
            bool s = Wrapper.IsNewSession;
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(userId.ToString()))
                    Response.Redirect("Login.aspx");
                hfUserId.Value = userId.ToString();
            }
           // if (userId == 2 || userId == 1)
            //{
                //lblLocation.Text = "Client";
                //lblLoc.Text = "Select Client";
            //}

            //To update this value use the Web.config file
            int startWeekVal = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["startWeek"]);
            int endWeekVal = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["endWeek"]);
            startweek = DateTime.Now.AddDays(startWeekVal).ToShortDateString();
            endweek = DateTime.Now.AddDays(endWeekVal).ToShortDateString();
            Thread T1 = new Thread(new ThreadStart(Thread1));
            Thread T2 = new Thread(new ThreadStart(Thread2));
            //btnSend.Visible = true;
            flag = false;

            if (IsPostBack)
            {
                updateGrid = true;
                flag = true;
            }

            if (!IsPostBack)
            {
                IEnumerable list = PopulateDates(startweek, endweek);
                lstCalendar.DataSource = list;
                lstCalendar.DataTextField = "Day";
                lstCalendar.DataValueField = "dates";
                DateTime curDate = DateTime.Today;
                //Get Server Time
                DateTime UTCTime = DateTime.UtcNow;
                lstCalendar.DataBind();
                for (int k = 0; k < lstCalendar.Items.Count; k++)
                {
                    if (lstCalendar.Items[k].Value == curDate.ToString())
                    {
                        lstCalendar.SelectedIndex = k;
                    }
                }
                LoadWorkDescriptions();
                LoadLocations();
                LoadHours();

            }



            //Get requestor info
            if (Page.Request.Browser.Browser.ToString() != null)
            {
                string browser = Page.Request.Browser.Browser.ToString();
            }
            if (Page.Request.UrlReferrer != null)
                if (Page.Request.UrlReferrer.AbsolutePath != null)
                {
                    string urlRef = Request.UrlReferrer.AbsolutePath.ToString();
                    //Application.Add("CurrentPage", urlRef);
                }
            if (Request.Url.Host != null)
            {
                string host = Request.Url.Host;
            }
            if (Request.UserHostName != null)
            {
                string dns = Request.UserHostName;
            }
            if (Request.UserAgent != null)
            {
                string useragent = Request.UserAgent;
            }
            List<string> requestVars = new List<string>();
            for (int i = 0; i < Request.ServerVariables.Count; i++)
            {
                requestVars.Add(Request.ServerVariables[i].ToString());
            }
            Application.Add("IPAddress", requestVars[28].ToString());
            Application.Add("CurrentPage", requestVars[29].ToString());
            // Application.Add("Browser", requestVars[52].ToString());

            if (!IsPostBack)
            {
                appList.Add(Application["IPAddress"]);
                appList.Add(Application["CurrentPage"]);
                //appList.Add(Application["Browser"]);
            }
        }


    }
    public ArrayList TimeList ()
    {
     ArrayList a = new ArrayList();
     ArrayList b = new ArrayList{".0", ".25", ".50", ".75"};
        for (int i = 0; i <= 20; i++)
        {
            for (int f = 0; f <= 3; f++)
            {
                a.Add(i+b[f].ToString());
            }
        }    
        return a;
    }
    protected void LoadHours()
    {
        ddlHoursSelect.DataSource = TimeList();
        ddlHoursSelect.DataBind();
    }
    protected override void OnError(EventArgs e)
    {
        base.OnError(e);
    }

    protected override object SaveControlState()
    {
        return base.SaveControlState();
    }

    public void CheckMe(object sender, EventArgs e)
    {
       //Do something
    }

    public void LoadLocations()
    {
        ddlAddLocation.Items.Clear();
        var p = (from i in locDB.tblLocations
                 where i.newId.ToString() == userId
                 orderby i.LocationName ascending
                 select i
                 ).Distinct();
        foreach (var x in p)
        {
            ddlAddLocation.Items.Add(new ListItem(x.LocationName));
        }
        ddlAddLocation.Items.Add(new ListItem("--------------"));
        //if (userId == 2)
        //{
            //ddlAddLocation.Items.Add(new ListItem("Add New Client"));
        //}
        //else
        //{
            ddlAddLocation.Items.Add(new ListItem("Add Location"));
        //}
    }

    public void LoadWorkDescriptions()
    {
        ddlWorkDesc.Items.Clear();
        var q = (from i in workDescDB.tblWorkDescriptions
                 where i.newId == Guid.Parse(userId)
                 orderby i.WorkDescription ascending
                 select i.WorkDescription
                 );
        foreach (var x in q)
        {
            ddlWorkDesc.Items.Add(new ListItem(x));
        }
        ddlWorkDesc.Items.Add(new ListItem("--------------"));
        ddlWorkDesc.Items.Add(new ListItem("Edit List"));
    }

   



    protected void btnSubmitAll_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlHoursSelect.SelectedIndex != 0)
            {
                flag = true;
                if (lstCalendar.SelectedIndex == -1)
                {
                    ShowClientFunctionInUpdatePanel("alert('Please select a date!');");
                    ShowClientFunctionInUpdatePanel("ChangeTextBack();");
                }
                if (ddlHoursSelect.SelectedIndex > -1)
                {
                    if (ddlAddLocation.Items.Count != 2 && ddlWorkDesc.Items.Count != 2 && lstCalendar.SelectedIndex >= 0)
                    {                       
                        InsertTime();
                        InsertTimeHist();
                        ddlHoursSelect.SelectedIndex = -1;
                        txtComments.Text = "";
                        ddlAddLocation.SelectedIndex = -1;
                        ddlWorkDesc.SelectedIndex = -1;
                        Bind();
                        BindSummary();
                        ShowClientFunctionInUpdatePanel("ChangeTextBack();");                        
                    }
                }
                else
                {
                    ShowClientFunctionInUpdatePanel("Validate();");
                    ShowClientFunctionInUpdatePanel("ChangeTextBack();");
                    ddlHoursSelect.Focus();
                }
            }
        }

        catch (Exception t)
        {
            string err = t.StackTrace;
            appList.Add(err);
            bl.WriteToErrorLog(appList);
        }
    }

    void InsertTimeHist()
    {
        var t = new tblTimeExpensesSummary_history();
        t.Comments = txtComments.Text == string.Empty ? "No Comments" : txtComments.Text;
        if (!String.IsNullOrEmpty(lstCalendar.SelectedValue))
            t.Date = DateTime.Parse(lstCalendar.SelectedValue);
        else
            t.Date = DateTime.Parse(string.Format("{0:dddd, MMMM d, yyyy}", DateTime.Today.ToString()));
        t.Expenses = null;
        t.LastUpdated = null;
        t.PaymentReceived = false;
        t.pictureUrl = null;
        t.CreatedDate = DateTime.Now;
        t.Location = ddlAddLocation.SelectedValue.ToString();
        t.WorkDescription = ddlWorkDesc.SelectedValue.ToString();
        if (ddlHoursSelect.SelectedIndex > -1)
            t.WorkHrs = Decimal.Parse(ddlHoursSelect.SelectedValue);
        int nUserId = BusLogic.SelectUserId(Application["UserId"].ToString());
        if (nUserId != 0)
            t.Id = nUserId;
        if (!String.IsNullOrEmpty(Request.QueryString["enum"].ToString()))
            t.NewId = Guid.Parse(Request.QueryString["enum"].ToString());
        TNEDB.tblTimeExpensesSummary_histories.InsertOnSubmit(t);
        TNEDB.SubmitChanges();
    }
    void InsertTime()
    {
        var t = new tblTimeExpensesSummary();
        t.Comments = txtComments.Text == string.Empty ? "No Comments" : txtComments.Text;
        if (!String.IsNullOrEmpty(lstCalendar.SelectedValue))
            t.Date = DateTime.Parse(lstCalendar.SelectedValue);       
        else
            t.Date = DateTime.Parse(string.Format("{0:dddd, MMMM d, yyyy}", DateTime.Today.ToString()));
        t.Expenses = null;
        t.LastUpdated = null;
        t.PaymentReceived = false;
        t.pictureUrl = null;
        t.CreatedDate = DateTime.Now;
        t.Location = ddlAddLocation.SelectedValue.ToString();
        t.WorkDescription = ddlWorkDesc.SelectedValue.ToString();
        if (ddlHoursSelect.SelectedIndex > -1)
            t.WorkHrs = Decimal.Parse(ddlHoursSelect.SelectedValue);      
        int nUserId = BusLogic.SelectUserId(Application["UserId"].ToString());
        if (nUserId != 0)
            t.Id = nUserId;
        if (!String.IsNullOrEmpty(Request.QueryString["enum"].ToString()))
            t.NewId = Guid.Parse(Request.QueryString["enum"].ToString());
        TNEDB.tblTimeExpensesSummaries.InsertOnSubmit(t);
        TNEDB.SubmitChanges();
    }




   
   
    public void Bind()
    {
        //if (Application["UserId"] != null)
        //{
        //    Guid userId = Guid.Parse(Application["UserId"].ToString());
        //}

            var data = from p in TNEDB.tblTimeExpensesSummaries
                       where p.NewId.ToString() == userId.ToString()
                       orderby p.Date descending
                       select new
                       {
                           Id = p.Id,//1
                           JobDate = string.Format("{0:dddd, MMMM d, yyyy}", p.Date),//2
                           Client = p.Location,//3
                           WorkDescription = p.WorkDescription,//4
                           WorkHours = p.WorkHrs,//5
                           PaymentReceived = p.PaymentReceived,   
                           Comments = p.Comments//7
                       };
              
        var d = from p in TNEDB.tblTimeExpensesSummaries
                where p.NewId.ToString() == userId.ToString()
                orderby p.Date descending
                select new
                {
                    JobDate = p.Date,
                    WorkDescription = p.WorkDescription,
                    WorkHours = p.WorkHrs,
                    Comments = p.Comments,
                    PaymentReceived = p.PaymentReceived
                };

        //if (userId == 1 || userId == 2)
            gView.AutoGenerateDeleteButton = true;
           
        gView.DataSource = data;        
        gView.DataBind();
        rowCount = Select().Where(p => p.NewId.ToString() == userId.ToString()).Count();
        lblRowText.Visible = true;
        lblRowCount.Visible = true;
        int pageIdx = gView.PageIndex + 1;
        lblRowCount.Text = rowCount.ToString(); 
        Session["VirtualTable"] = d;    
    }

    protected void BindSummary()
    {
        var hours = (from j in TNEDB.tblTimeExpensesSummaries
                     where (j.Date >= GetWeekStartDate() && j.Date <= GetWeekEndDate() && j.NewId.ToString() == userId.ToString())
                     group j by new { j.Location,j.Date}
                         into grp
                         select new
                             {
                                 WorkDay = string.Format("{0:dddd, MMMM d, yyyy}", grp.Key.Date),
                                 Client = grp.Key.Location,
                                 Total = grp.Sum(p => p.WorkHrs)
                             }).ToList();
        sumView.DataSource = hours;
        sumView.DataBind();
        if (sumView.Rows.Count > 0)
            sumView.FooterRow.BackColor = System.Drawing.Color.SlateGray;
        lnkHist.Visible = true;
        txtHistStart.Visible = true;
        txtHistEnd.Visible = true;
        lblStart.Visible = true;
        lblEnd.Visible = true;
    }

    protected void gView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        hfDeleted.Value = e.RowIndex.ToString();
        MdlPopDeleteRow.Show();
        pnlDeleteRow.Visible = true;
    }



    protected void btnYes_Click(object sender, EventArgs e)
    {
        int pk = 0;
        if (hfDeleted.Value != "")
        {
            RowIndex = int.Parse(hfDeleted.Value);
        }
        //Get Primary Key
        if (gView.DataKeys[RowIndex].Value.ToString() != null)
        {
            pk = int.Parse(gView.DataKeys[RowIndex].Value.ToString());
        }
        var deletedRecord = TNEDB.tblTimeExpensesSummaries.Single(c => c.Id == pk);
        var deletedHistoryRecord = TNEDB.tblTimeExpensesSummary_histories.Single(c => c.Id == pk);
        TNEDB.tblTimeExpensesSummaries.DeleteOnSubmit(deletedRecord);
        TNEDB.tblTimeExpensesSummary_histories.DeleteOnSubmit(deletedHistoryRecord);
        TNEDB.SubmitChanges();
        Bind();
        BindSummary();               
    }

    protected void gView_OnRowCreated(object sender,GridViewRowEventArgs e)
    {
        
    }
    protected void lnkRowEdit_OnClick(object sender, EventArgs e)
    {      
        editRecordId = 0;    
        LinkButton lnk = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnk.NamingContainer;
        editRecordId = row.RowIndex;
        if (gView.DataKeys[row.RowIndex].Value.ToString() != null)
        {
            editRecordId = int.Parse(gView.DataKeys[row.RowIndex].Value.ToString());
            Application.Add("EditRowNum", editRecordId);
            Session.Add("EditRowNum", editRecordId);
        }
        MdlPopEditRow.Show();
        pnlEditRecord.Visible = true;
        LoadData(userId.ToString(), editRecordId.ToString());      
    }
 
    protected void btnCancel_Click(object sender, EventArgs e)
    {
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsDirty())
            {
                HashData();
                if (!CheckIfExists(0, txtClient.Text))
                {
                    tblLocation loc = new tblLocation();
                    loc.LocationName = txtClient.Text;
                    loc.newId = Guid.Parse(userId);
                    locDB.tblLocations.InsertOnSubmit(loc);
                    locDB.SubmitChanges();
                }
              
                if (!CheckIfExists(1, txtEditWorkDesc.Text))
                {
                    tblWorkDescription wk = new tblWorkDescription();
                    wk.WorkDescription = txtEditWorkDesc.Text;
                    wk.newId = Guid.Parse(userId);
                    workDescDB.tblWorkDescriptions.InsertOnSubmit(wk);
                    workDescDB.SubmitChanges();
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
            Bind();
            BindSummary();
            pnlEditRecord.Visible = false;
        }


    }
    protected void LoadData(string u, string r)
    {
        var loadData = (from t in tne.tblTimeExpensesSummaries
                        where t.NewId == Guid.Parse(u) && t.Id == int.Parse(r)
                        select t).Single();
        txtDate.Text = ((DateTime)loadData.Date).ToShortDateString();
        txtClient.Text = loadData.Location;
        txtEditHours.Text = loadData.WorkHrs.ToString();
        txtEditWorkDesc.Text = loadData.WorkDescription.ToString();
        txtEditComments.Text = loadData.Comments;
        if (loadData.PaymentReceived == null || loadData.PaymentReceived == false)
            cbPayment.Checked = false;
        else
            cbPayment.Checked = true;
    }

    protected void HashData()
    {

        var loadData = (from t in tne.tblTimeExpensesSummaries
                        where t.NewId.ToString() == userId.ToString() && t.Id == int.Parse(Session["EditRowNum"].ToString())
                        select t).Single();
        loadData.Date = DateTime.Parse(txtDate.Text);
        loadData.WorkHrs = Decimal.Parse(txtEditHours.Text);
        loadData.Location = txtClient.Text;
        loadData.WorkDescription = txtEditWorkDesc.Text;
        loadData.Comments = txtEditComments.Text;
        loadData.PaymentReceived = cbPayment.Checked;
        loadData.LastUpdated = DateTime.Now;
        tne.SubmitChanges();
        //Update History table
        var loadHistData = (from t in tne.tblTimeExpensesSummary_histories
                            where t.NewId.ToString() == userId.ToString() && t.Id == int.Parse(Session["EditRowNum"].ToString())
                            select t).Single();

        loadHistData.Date = DateTime.Parse(txtDate.Text);
        loadHistData.WorkHrs = Decimal.Parse(txtEditHours.Text);
        loadHistData.Location = txtClient.Text;
        loadHistData.WorkDescription = txtEditWorkDesc.Text;
        loadHistData.Comments = txtEditComments.Text;
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
                         where s.newId == Guid.Parse(userId)
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
                         where s.newId == Guid.Parse(userId)
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
                        where t.NewId.ToString() == userId.ToString() && t.Id == int.Parse(Session["EditRowNum"].ToString())
                        select t).Single();
    
        if (loadData.Date != DateTime.Parse(txtDate.Text))
            dirty = true;
        if (loadData.WorkHrs != Decimal.Parse(txtEditHours.Text))
            dirty = true;
        if (loadData.Location != txtClient.Text)
            dirty = true;
        if (loadData.WorkDescription != txtEditWorkDesc.Text)
            dirty = true;
        if (loadData.Comments != txtEditComments.Text)
            dirty = true;
        if (loadData.PaymentReceived != cbPayment.Checked)
            dirty = true;
        return dirty;
    }


    protected void sumView_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           // val += Decimal.Parse(e.Row.Cells[1].Text);
            val += Decimal.Parse(e.Row.Cells[2].Text);
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.ForeColor = System.Drawing.Color.Black;
            e.Row.Cells[0].Text = "Weekly Total";
            e.Row.Cells[2].Text = val.ToString();
        }
    }

    protected void btnSubmitTime_Click(object sender, EventArgs e)
    {   //Clear out
        btnSubmitAll.Enabled = true;      
    }
  
    protected void gView_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = e.Row;
        GridViewUpdateEventArgs ev = new GridViewUpdateEventArgs(row.RowIndex);
        LinkButton lnk = new LinkButton();
        String param1, param2;
        if (e.Row.RowIndex >= 0)
        {
            if (gView.DataKeys[e.Row.RowIndex][0].ToString() != null)
            {
                rowId = int.Parse(gView.DataKeys[e.Row.RowIndex][0].ToString());
            }
        }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {               
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = false;
                lnk.Text = "Comments";
                lnk.ID = "lnkComments";
                param1 = userId.ToString();
                param2 = rowId.ToString();
                lnk.OnClientClick = string.Format("GetComments({0},{1});", "'"+ param1+"'", param2);
                e.Row.Cells[8].Controls.Add(lnk);                    
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {  
                if (e.Row.Cells[2].Text == "Id")
                {
                    e.Row.Cells[2].Visible = false;
                }
                e.Row.Cells[8].Text = "Comments";              
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
            }
    }

    protected void gView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        
        gView.PageIndex = e.NewPageIndex;
        Bind();
        int pageIdx = e.NewPageIndex + 1;
        int numb = ((pageIdx * 10) + 10);
        if (gView.PageCount != pageIdx)
        {
            this.lblPageView.Text = string.Format("Viewing page {0} of {1}. Displaying records {2} thru {3}.", pageIdx, gView.PageCount, (numb -19), numb - 10);
        }
        else
        {
            this.lblPageView.Text = string.Format("Viewing page {0} of {1}. Displaying records {2} thru {3}.", pageIdx, gView.PageCount, (numb - 19), rowCount);
        }

    }

    public static IEnumerable<tblTimeExpensesSummary> Select()
    {
        TNEDataContext t = new TNEDataContext();
        return t.tblTimeExpensesSummaries;
    }

    public static void Update(int id)
    {
        TNEDataContext t = new TNEDataContext();
        var item = t.tblTimeExpensesSummaries.Single(p => p.Id == id);
        item.LastUpdated = DateTime.Now;
        t.SubmitChanges();
    }

    private DateTime CastDate(string d)
    {
        DateTime ti = new DateTime();
        if (d != string.Empty)
        {
            ti = Convert.ToDateTime(d);
        }
        return ti;
    }
    private DateTime GetWeekStartDate()
    {
        DateTime sDate;
        double offset = 0;
        switch (DateTime.Today.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                offset = 0;
                break;
            case DayOfWeek.Monday:
                offset = -1;
                break;
            case DayOfWeek.Tuesday:
                offset = -2;
                break;
            case DayOfWeek.Wednesday:
                offset = -3;
                break;
            case DayOfWeek.Thursday:
                offset = -4;
                break;
            case DayOfWeek.Friday:
                offset = -5;
                break;
            case DayOfWeek.Saturday:
                offset = -6;
                break;
        }
        return sDate = DateTime.Today.AddDays(offset);

    }

    private DateTime GetWeekEndDate()
    {
        DateTime eDate;
        double offset = 0;
        switch (DateTime.Today.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                offset = 0;
                break;
            case DayOfWeek.Monday:
                offset = 1;
                break;
            case DayOfWeek.Tuesday:
                offset = 2;
                break;
            case DayOfWeek.Wednesday:
                offset = 3;
                break;
            case DayOfWeek.Thursday:
                offset = 4;
                break;
            case DayOfWeek.Friday:
                offset = 5;
                break;
            case DayOfWeek.Saturday:
                offset = 6;
                break;
        }
        return eDate = DateTime.Today.AddDays(7 - offset);
    }

    public void Thread1()
    {
        try
        {
            LoadLocations();
            _pool.WaitOne();
            _pool.Release();
            Console.WriteLine("Thread A released the semaphore.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Thread 1: {0}", ex.Message);
        }

    }

    public void Thread2()
    {
        try
        {
            LoadWorkDescriptions();
            _pool.WaitOne();
            _pool.Release();
            Console.WriteLine("Thread 2 exits successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Thread 2: {0}", ex.Message);
        }
    }

    public bool IsDecimal(string num)
    {
        bool yes = false;
        decimal retVal;
        if (Decimal.TryParse(num, out retVal))
        {
            yes = true;
        }
        return yes;
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (flag)
        {
            if (sumView.Rows.Count != 0)
            {
                ModalPopupExtender2.Show();
                PnlEmail.Visible = true;
            }
        }
    }
    protected void btnSend1_Click(object sender, EventArgs e)
    {
        if (flag)
        {
            if (sumView.Rows.Count != 0)
            {
                ModalPopupExtender2.Show();
                PnlEmail.Visible = true;
            }
        }
    }


    public string GetBody(GridView grid)
    {
        StringWriter msw = new StringWriter();
        HtmlTextWriter mHtw = new HtmlTextWriter(msw);
        grid.RenderControl(mHtw);
        return msw.GetStringBuilder().ToString();
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        // Confirms that an HtmlForm control is rendered for the
        // specified ASP.NET server control at run time.
        // No code required here.
    }

  
   

    public static int NumRecords(int num)
    {
        int retVal = 0;
        WorkDescDataContext t = new WorkDescDataContext();
        int wk = t.tblWorkDescriptions.Count();
        LocationsDataContext locd = new LocationsDataContext();
        int l = locd.tblLocations.Count();
        if (num == 1)
        {
            if (wk >= 0)
                retVal = wk;
        }

        if (num == 2)
        {
            if (l >= 0)
                retVal = l;
        }

        return retVal;
    }

    public void SendEmail()
    {
        emailGrid = true;
        string from = System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString();
        string to = System.Configuration.ConfigurationManager.AppSettings["ToAddress"].ToString();
        string subject = string.Format("Time Sheet for Week Ending {0}", GetWeekEndDate().ToString());
        string Body = GetBody(sumView);
        string Attachment = "";
        string msg = string.Empty;
        BusLogic chk = new BusLogic();

        if (chk.ValidateEmail(from) == true && chk.ValidateEmail(to) == true)
        {
            var hours = from j in TNEDB.tblTimeExpensesSummaries
                        where (j.Date >= GetWeekStartDate() && j.Date <= GetWeekEndDate()) && j.NewId.ToString() == userId.ToString()
                        orderby j.Date
                        group j by j.Date into grouping
                        select new
                        {
                            WorkDay = string.Format("{0:dddd, MMMM d, yyyy}", grouping.Key),
                            Total = grouping.Sum(p => p.WorkHrs)
                        };
        }

        if (sumView.Rows.Count != 0)
        {
            chk.SendEmail(from, to, subject, Body);
            ShowClientFunction("EmailSuccess();");
        }
    }

    protected void btnSendImage_Click(object sender, ImageClickEventArgs e)
    {


    }
    protected void btnEmailNo_Click(object sender, EventArgs e)
    {

    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        BusLogic bus = new BusLogic();
        if (flag)
        {
            txtEmailfrom.Text = System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString();
            if (sumView.Rows.Count != 0)
            {
                if (bus.ValidateEmail(txtEmailRec.Text))
                {
                    //Update AppSettings
                    if (!string.IsNullOrEmpty(txtEmailRec.Text))
                    {
                        System.Configuration.ConfigurationManager.AppSettings["ToAddress"] = txtEmailRec.Text;
                        SendEmail();
                    }
                }
            }
        }
    }
    public void CheckElaspedSessionTime(DateTime CurrentTime)
    {
        Boolean IsExpired = false;
        DateTime dt = (DateTime)(Session.Contents["SessionStartDateTime"]);
        CurrentTime = DateTime.Now;
        TimeSpan ts = new TimeSpan();
        if (CurrentTime >= dt)
        {
            ts = ts.Subtract(CurrentTime - dt);
            IsExpired = ts.TotalMinutes > 5.0;
        }
        
        if (IsExpired)
            Response.Redirect("Login.aspx", true);
    }

    public void ShowClientFunction(string function)
    {
        string str = "<script language='javascript' type='text/javascript'>" + function + "</script>";
        Page.ClientScript.RegisterStartupScript(GetType(), "key", str);
    }

    public static IEnumerable PopulateDates(string d1, string d2)
    {
        DateTime val1 = DateTime.Parse(d1);
        DateTime val2 = DateTime.Parse(d2);
        s03_DB_13029_randtDataContext db = new s03_DB_13029_randtDataContext();
        return db.getCurrentDates(val1, val2).ToList();
    }

    protected void sumView_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void lnkSendToHistory_OnClick(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["enum"]))
        {
            string c = Request.QueryString["enum"].ToString();

            string newUrl = string.Format("TimeEntryHistory.aspx?enum={0}", userId);
            Response.Redirect(newUrl);
        }
        else
        {
            Response.Redirect("Login.aspx", true);
        }
    }
    protected void imgAddLoc_Click(object sender, ImageClickEventArgs e)
    {
        if (string.IsNullOrEmpty(txtAddLocations.Text))
        {
            ShowClientFunction("alert('Please enter a location before submitting!');");
            return;
        }

        try
        {
            if (NumRecords(1) >= 0)
            {
                if (txtAddLocations.Text != string.Empty)
                {
                    if (!CheckIfExists(0, txtAddLocations.Text))
                    {
                        tblLocation tloc = new tblLocation();
                        tloc.LocationName = txtAddLocations.Text;
                        tloc.newId = Guid.Parse(userId);
                        locDB.tblLocations.InsertOnSubmit(tloc);
                        locDB.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                        txtAddLocations.Text = string.Empty;
                        ddlAddLocation.Items.Clear();
                        LoadLocations();

                    }
                }
            }
        }
        catch (Exception)
        {

            Response.Redirect("Error.aspx",false);
        }
    }

    protected void imgAddWkdesc_Click(object sender, ImageClickEventArgs e)
    {
        if (string.IsNullOrEmpty(txtWorkDesc.Text))
        {
            ShowClientFunction("alert('Please enter a description before submitting!');"); return;
        }
        try
        {
            if (NumRecords(2) >= 0)
            {
                if (txtWorkDesc.Text != string.Empty)
                {
                    if (!CheckIfExists(1, txtWorkDesc.Text))
                    {
                        tblWorkDescription wk = new tblWorkDescription();
                        wk.WorkDescription = txtWorkDesc.Text;
                        wk.newId = Guid.Parse(userId);
                        workDescDB.tblWorkDescriptions.InsertOnSubmit(wk);
                        workDescDB.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                        txtWorkDesc.Text = string.Empty;
                        ddlWorkDesc.Items.Clear();
                        LoadWorkDescriptions();
                    }
                }
            }
        }
        catch (Exception)
        {
            Response.Redirect("Error.aspx");
        }
    }

    protected void imgExcel_Click(object sender, ImageClickEventArgs e)
    {
        pnlExcel.Visible = true;
    }

    protected void btnSend_Click(object sender, ImageClickEventArgs e)
    {
        if (flag)
        {
            if (sumView.Rows.Count != 0)
            {
                ModalPopupExtender2.Show();
                PnlEmail.Visible = true;
                txtEmailfrom.Text = System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString();
            }
        }
    }


    protected void btnConfirmExport_Click(object sender, EventArgs e)
    {       
        SendExcelInfo();
    }
    protected void SendExcelInfo()
    {
        if (!string.IsNullOrEmpty(txtEndDateExcel.Text) && !string.IsNullOrEmpty(txtStartDateExcel.Text))
        {
            if (Convert.ToDateTime(txtStartDateExcel.Text) <= Convert.ToDateTime(txtEndDateExcel.Text))
            {
                string datetime = DateTime.Now.ToShortDateString();
                TNEDataContext t = new TNEDataContext();

                var xl = from g in t.tblTimeExpensesSummaries
                         where g.Date >= Convert.ToDateTime(txtStartDateExcel.Text)
                         && g.Date <= Convert.ToDateTime(txtEndDateExcel.Text)
                         && g.NewId.ToString() == userId.ToString()
                         orderby g.Date descending
                         select new
                         {
                             Day = g.Date,
                             Description = g.WorkDescription,
                             Hours = g.WorkHrs,
                             Comments = g.Comments,
                             PaymentReceived = g.PaymentReceived
                         };

                GridView newGrid = new GridView();
                newGrid.DataSource = xl;
                newGrid.DataBind();
                try
                {
                    bl.Export(String.Format("TNE_{0}.xls", datetime), newGrid);
                }
                catch (Exception)
                {
                }
            }
        }
    }

    protected void btnSubmitStock_Click(object sender, EventArgs e)
    {
        IEnumerable<string> myList = null;
        if (txtEnterQuote.Text != string.Empty)
        {
            myList = bus.GetStockQuote(txtEnterQuote.Text);
            txtEnterQuote.Text =  string.Format("Current quote for {0} as of {1} is {2}."
            , myList.ElementAt<string>(0), myList.ElementAt<string>(2), myList.ElementAt<string>(1));
        }
    }

    bool checkforcurl(string textTocheck)
    {
        bool chk = false;
        char[] myChar = new char [] { '&' };

        if (textTocheck.Contains(myChar[0].ToString()))
        {
            chk = true;
        }
        return chk;

    }



    private void GetWebResponse()
    {
        string wsUrl = "http://randtconsulting.com/tne/webservice.asmx?op=GetData";
         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(wsUrl);
         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
         Stream stream = response.GetResponseStream();
         StreamReader rdr = new StreamReader(stream, Encoding.UTF8);
         String ldr = rdr.ReadToEnd();
         rdr.Close();
         stream.Close();
         response.Close();

    }
        public class Customer
        {
            public string _name;
            public string Name
            {
                get
                {
                    return _name;
                }

                set
                {
                    _name = value;
                }
            }



            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }


        }




}

