using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;

public partial class TimeEntryHistory : System.Web.UI.Page
{    
    protected static string startDate = string.Empty;
    protected static string endDate = string.Empty;
    protected static string _UserEnum = string.Empty;
    protected static string _WeekEndDate = string.Empty;
    protected static string _Task = string.Empty;
    protected static string [] val = null;
    protected static string passdate = string.Empty;
    protected static bool success = false;
    protected static string SendImageYesNo = string.Empty;
    private object thislock = new object();
    int totalrow = 0, RowPageCount, rowN = 0, totRows;
    int rowNumb;
    decimal totHrs;
    string userId;
    TNEDataContext tne = new TNEDataContext();
    LocationsDataContext locDB = new LocationsDataContext();
    BusLogic bl = new BusLogic();
    bool emailGrid;
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
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = "TimeKeeper Pro&#xA9 | Time Entry History";
        if (!IsPostBack)
        {
            //userId = Application["UserId"].ToString();
            if (userId == string.Empty)
            {
                Response.Redirect("Login.aspx", false);
            }
            LoadClientList(userId);
        }

        //Get server path
        string path = Server.MapPath("/TNE/Email.aspx");
        path = Request.Url.OriginalString;
        string v = Server.MapPath(Request.ServerVariables["PATH_INFO"]).ToString();

        if (!string.IsNullOrEmpty(hfUserId.Value))
            hfUserId.Value = userId.ToString();
        string test = "Test1,Test2";
        string[] param = test.Split(new char[] { ',' });
        string Reqval = string.Empty;
        for (int i = 0; i < Request.QueryString.Count; i++)
        {
            Reqval = Request.QueryString[i].ToString();
        }
        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.ToString().Contains("Email"))
                {
                    if (Request.QueryString["task"] != null)
                    {
                        if (Request.QueryString["task"].ToString() == "goEmail")
                        {
                            if (Request.QueryString["eid"] != null)
                            {
                                if (Request.QueryString["date"] != null)
                                {
                                    val = Request.QueryString["eid"].Split(new char[] {';'});
                                    passdate = Request.QueryString["date"].ToString();
                                    if (Application["HistGrid"] != null)
                                    {
                                        lock (thislock)
                                        {
                                            if (((GridView)Application["HistGrid"]).Rows.Count > 0)
                                            {
                                                success = PrepareSendEmail(val, passdate);
                                            }
                                        }                                    
                                    }
                                    else //Regenerate dataset and try again
                                    {
                                        if (!success)
                                            RegenerateHistGView();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    startDate = txtHistStart.Text;
    endDate = txtHistEnd.Text;
    imgExcel.Visible = false;
    lnkExportResults.Visible = false;
    lnkMailResults.Visible = false;
    }
  
    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("TimeEntry.aspx?enum=" + userId,true);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SendImageYesNo = "Send";
        try
        {
            if (startDate != string.Empty && endDate != string.Empty && ddlClientList.SelectedIndex >-1)
            {
                ShowData(startDate, endDate, histGView);
                int rowNum = (int)tne.tblTimeExpensesSummaries.Where(p => p.Date
                    >= DateTime.Parse(startDate) && p.Date <= DateTime.Parse(endDate)
                    && p.NewId.ToString() == userId && p.Location == ddlClientList.SelectedValue).Count();
              
                if (rowNum != 0)
                {
                    lblRowCount.Visible = true;
                    lblRowCount.Text = String.Format("{0} total record(s) returned.", rowNum.ToString());
                    lblText.Visible = false;                  
                }
                else
                {
                    lblRowCount.Visible = false;
                    lblText.Visible = true;
                    lblText.ForeColor = System.Drawing.Color.Red;
                    lblText.Text = "No records exists for the dates provided.\n Modify your dates if necessary.";
                    btnEmail.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            string source = ex.Source;
            string stack = ex.StackTrace;
           bl.GetError(message,source,stack);
           
        //    string msg = ex.Message;
        //    ArrayList ar = new ArrayList(4);
        //    ar.Add(DateTime.Now);
        //    ar.Add(userId);
        //    ar.Add(Request.UserHostAddress);
        //    ar.Add(msg);
        //    bl.WriteToErrorLog(ar);
        //    Response.Redirect("Error.aspx",false);
        }
 
    }


    protected void histGView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ShowData(startDate, endDate, histGView);
        histGView.PageIndex = e.NewPageIndex;
        int pagerRow = histGView.Rows.Count - totalrow;
        int totRows = histGView.Rows.Count;
        lblNumStart.Text = histGView.Rows.Count.ToString();
        lblNumTotal.Text = rowNumb.ToString();       
    }

    public static IEnumerable<tblTimeExpensesSummary> GetUserById(int in_id)
    {
        TNEDataContext tb = new TNEDataContext();
        tb.tblTimeExpensesSummaries.Single(p => p.Id == in_id);
        return tb.tblTimeExpensesSummaries;        
    }
    public void ShowData(string d1,string d2,GridView g)
    {
        if (d1 != string.Empty && d2 != string.Empty && ddlClientList.SelectedIndex > -1)
        {
            var d = from j in tne.tblTimeExpensesSummaries 
                    where j.Date >= DateTime.Parse(d1) && j.Date <= DateTime.Parse(d2) && j.NewId.ToString() == userId
                    && j.Location == ddlClientList.SelectedValue                  
                    orderby j.Date ascending
                    select new
                    {
                        Date = string.Format("{0:dddd, MMMM d, yyyy}", j.Date),
                        Client = j.Location,
                        Description = j.WorkDescription,
                        Hours = j.WorkHrs,
                        Comments = j.Comments,
                        PaidYesNo = j.PaymentReceived.ToString()                                                 
                    };
          
        
            rowNumb = d.Count();
            
            g.DataSource = d.ToList();
            g.DataBind();
            lblNumStart.Text = g.Rows.Count.ToString();
            lblNumTotal.Text = rowNumb.ToString();
            if (rowNumb != 0)
            {
                btnEmail.Visible = true;
                imgExcel.Visible = true;
                lnkMailResults.Visible = true;
                lnkExportResults.Visible = true;
            }
            Application.Lock();
            Application["HistGrid"] = g;
            Session["HistGrid"] = g;
            Application.UnLock();
            bl.GetDataForXMLOut(g, @"C:\Users\Rich\ExportedData.xml");
        }       
    }

    

    protected override void OnError(EventArgs e)
    {
        Exception ex = new Exception();
        bl.GetError(ex.Message, ex.Source, ex.StackTrace);
        //string message = ex.Message;
        //string source = ex.Source;
        //string stack = ex.StackTrace;
        //bl.GetError(message, source, stack);
         base.OnError(e);
    }

    protected void RegenerateHistGView()
    { 
    if (txtHistStart.Text != string.Empty && txtHistEnd.Text != string.Empty && ddlClientList.SelectedIndex > -1)
    {
        var d = from j in tne.tblTimeExpensesSummaries
                where j.Date >= DateTime.Parse(txtHistStart.Text) && j.Date <= DateTime.Parse(txtHistEnd.Text) && j.NewId.ToString() == userId
                && j.Location == ddlClientList.SelectedValue
                orderby j.Date ascending
                select new
                {
                    Date = string.Format("{0:dddd, MMMM d, yyyy}", j.Date),
                    Client = j.Location,
                    Description = j.WorkDescription,
                    Hours = j.WorkHrs,
                    Comments = j.Comments,
                    PaidYesNo = j.PaymentReceived.ToString() 
                 
                    //(j.PaymentReceived == null || j.PaymentReceived == Convert.ToBoolean(0)
                    //|| j.PaymentReceived.ToString() == "False") ? "No" : "Yes"
                 
                };

    
        GridView HistGView = new GridView();
        HistGView.DataSource = d.ToList();
        HistGView.DataBind();
        Application["HistGrid"] = HistGView;
        if (((GridView)Application["HistGrid"]).Rows.Count > 0)
        {
            PrepareSendEmail(val, passdate);
        }
    }
    }
    protected void histGView_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
     
        String getImage = string.Empty;
        ImageButton img = new ImageButton();           
        rowN = e.Row.RowIndex + 1;
        totalrow += rowN-1;
        int chkCell = e.Row.Cells.Count;
        if (e.Row.Cells.Count.Equals(chkCell))
        {
            //e.Row.Cells[chkCell-1].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            totHrs += decimal.Parse(e.Row.Cells[3].Text);
            if (e.Row.Cells[chkCell-1].Text == "True")
            {              
                    img.ImageUrl = @"~/images/OK.png";             
            }
            else
            {               
                    img.ImageUrl = @"~/images/Annuler.png";              
            }
            if (SendImageYesNo != "Send")
            {
               
            }
            else
            {
              e.Row.Cells[chkCell - 1].Controls.Add(img);
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Total";
            e.Row.Cells[1].Font.Size = 14;
            e.Row.Cells[1].Text = totHrs.ToString();
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[5].Text = "Payment Received";
       
        }
    }

    protected void LoadClientList(String userId)
    {
        if (userId != null)
        {
            var clients = (from j in locDB.tblLocations
                           where j.newId.ToString() == userId && !j.LocationName.Contains("Testing")
                           select j);
            foreach (var p in clients)
            {
                ddlClientList.Items.Add(new ListItem(p.LocationName));
            }
        }
        else
            return;
    }

    public bool PrepareSendEmail(string[] EmailId,string WeekEndDate)
    {
        WeekEndDate = WeekEndDate.Substring(0, 9) + " " + DateTime.Now.ToShortTimeString();
        string email1 = string.Empty;
        string email2 = string.Empty;
        string emailIdFromRequestor = Request.QueryString["eid"];
        char[] delimiter = new char[] { ';' };
        string [] emailArray = emailIdFromRequestor.Split(delimiter);
        email1 = EmailId[0].ToString();
        email2 = EmailId[1].ToString();
        if (email2!= "NoCC")
        {
            System.Configuration.ConfigurationManager.AppSettings["ToAddress"] = GetEmailId(userId, email1);
            System.Configuration.ConfigurationManager.AppSettings["CCAddress"] = GetEmailId(userId, email2);
        }
        else
        {
            System.Configuration.ConfigurationManager.AppSettings["ToAddress"] = GetEmailId(userId, email1);
            System.Configuration.ConfigurationManager.AppSettings["CCAddress"] = System.Configuration.ConfigurationManager.AppSettings["NetUser"];
        }       
        emailGrid = true;
        string from = System.Configuration.ConfigurationManager.AppSettings["FromAddress"].ToString();
        string to = System.Configuration.ConfigurationManager.AppSettings["ToAddress"].ToString();
        string cc = System.Configuration.ConfigurationManager.AppSettings["CCAddress"].ToString();
        string subject = string.Format("Time Sheet for Week Ending {0}",WeekEndDate);
        string Body = GetBody(Application["HistGrid"] as GridView);
        string Attachment = "";
        string msg = string.Empty;
        BusLogic chk = new BusLogic();
        if (chk.ValidateEmail(from) == true && chk.ValidateEmail(to) == true)
        {
            try
            {                         
                SendEmail(from, to, cc, subject, Body);
                success = true;  
                ShowClientFunction("EmailSuccess();");                  
             }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
               
            }
        }
        return success;
    }
    public void ShowClientFunctionInUpdatePanel(string fX)
    {
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                 Guid.NewGuid().ToString(), fX, true);
    }
    public void ShowClientFunction(string function)
    {
        string str = "<script language='javascript' type='text/javascript'>" + function + "</script>";
        Page.ClientScript.RegisterStartupScript(GetType(), "key", str);
    }

    protected void btnEmail_OnClick(object sender, ImageClickEventArgs e)
    {
        string nurl = string.Format("Email.aspx?enum={0}" , userId);
        Response.Redirect(nurl,true);    
    }
    public string GetBody(GridView grid)
    {
        Application["HistGrid"] = grid;
        StringWriter msw = new StringWriter();
        HtmlTextWriter mHtw = new HtmlTextWriter(msw);
        try
        {
            if (grid != null)
                grid.RenderControl(mHtw);
        }
        catch (Exception)
        {
        }
     
       return msw.GetStringBuilder().ToString();
        
    }
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        // Confirms that an HtmlForm control is rendered for the+
        // specified ASP.NET server control at run time.
        // No code required here.
    }
    public string GetEmailId(String user,string emailId)
    {   
        BusLogic.GetApplicationValues b = new BusLogic.GetApplicationValues();
        string retVal = string.Empty;
        EmailDataContext emailDB  = new EmailDataContext();
        if (emailId == "NoCC")
        {
            return " ";
        }
        else
        {
            var data = (from d in emailDB.tblEmails
                        where d.newId.ToString() == user && d.EmailId == int.Parse(emailId)
                        select d);
            foreach (var c in data)
            {
                retVal = c.EmailAddress;
                b.EmailId = retVal;
                Application.Add("EmailIdentification", b.EmailId);
            }
            return retVal;
        }
    }

    public void SendEmail(string from, string to,string cc, string subj, string body)
    {
        //create the mail message 
        MailMessage mail = new MailMessage();
        //set the addresses 
        mail.From = new MailAddress(from);
        mail.To.Add(to);
        if (cc != "")
        {
            mail.CC.Add(cc);
        }
        //set the content 
        mail.Subject = subj;
        mail.Body = body;
        mail.IsBodyHtml = true;
        //send the message 
        SmtpClient smtp = new SmtpClient("mail.randtconsulting.com");
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        string PostalId = System.Configuration.ConfigurationManager.AppSettings["postmasterId"].ToString();
        NetworkCredential Credentials = new NetworkCredential("postmaster@randtconsulting.com", PostalId);
        smtp.Credentials = Credentials;
        try
        {
            smtp.Send(mail);
        }
        catch (Exception)
        {

        }
        finally
        {

        }
    }

    
    protected void SendExcelInfo(string d1, string d2)
    {
        if (!string.IsNullOrEmpty(d1) || !string.IsNullOrEmpty(d2))
        {
            try
            {
                var excelData = from j in tne.tblTimeExpensesSummaries
                        where j.Date >= DateTime.Parse(d1) && j.Date <= DateTime.Parse(d2) && j.NewId.ToString() == userId
                        && j.Location == ddlClientList.SelectedValue
                        orderby j.Date ascending
                        select new
                        {
                            Date = string.Format("{0:dddd, MMMM d, yyyy}", j.Date),
                            Client = j.Location,
                            Description = j.WorkDescription,
                            Hours = j.WorkHrs,
                            Comments = j.Comments,
                            PaidYesNo = j.PaymentReceived.ToString()== "1" ? "Yes" : "No"
                        };
                histGView.DataSource = excelData.ToList();
                histGView.DataBind();
                Application.Lock();
                Application["ExcelGrid"] = histGView;
                Application.UnLock();
                string datetime = DateTime.Now.ToShortDateString();
                try
                {
                    bl.Export(String.Format("TNE_{0}.xls", datetime), Application["ExcelGrid"] as GridView);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
            catch (Exception p)
            {
                string s = p.Message;
            }
        }
    }


    protected void imgExcel_Click(object sender, ImageClickEventArgs e)
    {
        SendExcelInfo(txtHistStart.Text,txtHistEnd.Text);
    }

    protected void lnkMailResults_Click(object sender, EventArgs e)
    {
        if (userId != null)
        {
            string nurl = string.Format("Email.aspx?enum={0}", userId);
            Response.Redirect(nurl, true);
        }
        else
        {
            Response.Redirect("Login.aspx", false);

        }
    }

    protected void lnkExportResults_Click(object sender, EventArgs e)
    {
        SendExcelInfo(txtHistStart.Text, txtHistEnd.Text);
    }


    public void GetControl(String controlName)
    {
        ContentPlaceHolder cph = (ContentPlaceHolder)Master.FindControl("MainContent");
        if (cph.HasControls())
        {
            foreach (Control ctrl in cph.Controls)
            {
                if (ctrl.ID == "UpdatePanel1")
                {
                    if (ctrl.HasControls())
                    {
                        foreach (Control ctp in UpdatePanel1.ContentTemplateContainer.Controls)
                        {
                            if (ctp.ID == "txtHistStart")
                            {
                            }
                        }

                    }
                }
            }

        }
       
    }
    
    public static byte[] ConvertImageToBinary(System.Drawing.Image img)
    {
        MemoryStream ms = new MemoryStream();
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        return ms.ToArray();

    }
    public static System.Drawing.Image ConvertBinaryToImage(Byte[] byt)
    {
        MemoryStream ms = new MemoryStream(byt);
        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
        return returnImage;
    }

    //public  ImageTable getImage()
    //{  
    //    string fileToUpload = string.Empty;
    //    fileToUpload = @"C:\Users\Rich\Pictures\Photo_00001.jpg";
    //    Byte [] ImageData = System.IO.File.ReadAllBytes(fileToUpload);    
    //    ImageTable t = new ImageTable();
    //    t.NewId = Guid.Parse(userId);
    //    t.id = 1;
    //    t.CreatedDate = DateTime.Now;
    //    t.UpdatedDate = null;
    //    t.Image = ImageData;
    //    return t;
    //}

}
    
  
