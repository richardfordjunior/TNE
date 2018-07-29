using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.Data.OleDb;


/// <summary>
/// 
/// Summary description for BusLogic
/// </summary>
public class BusLogic
{

    public bool ValidateEmail(string ValidateThis)
    {
        string pattern = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        Regex CheckExp = new Regex(pattern);
        bool check = CheckExp.IsMatch(ValidateThis, 0);
        return check;
    }


    public void SendEmail(string from, string to, string subj, string body)
    {
        string Netuser = System.Configuration.ConfigurationManager.AppSettings["NetUser"].ToString();
        string Netpword = System.Configuration.ConfigurationManager.AppSettings["postmasterId"].ToString();
        //create the mail message 
        MailMessage mail = new MailMessage();
        //set the addresses 
        mail.From = new MailAddress(from);
        mail.To.Add(to);
        mail.CC.Add(from);
        //set the content 
        mail.Subject = subj;
        mail.Body = body;
        mail.IsBodyHtml = true;
        //send the message 
        SmtpClient smtp = new SmtpClient("mail.randtconsulting.com");
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        NetworkCredential Credentials = new NetworkCredential(Netuser, Netpword);
        smtp.Credentials = Credentials;
        try
        {
            smtp.Send(mail);
        }
        catch (Exception)
        {

        }
    }
    public string GetBody(GridView grid)
    {
        StringWriter msw = new StringWriter();
        HtmlTextWriter mHtw = new HtmlTextWriter(msw);
        grid.RenderControl(mHtw);
        return msw.GetStringBuilder().ToString();
    }
    public static int parseFileSize(long number)
    {
        if (number != 0)
        {
            if (number > 999)
            {
                number = number / 1000;
            }
            else if (number > 9999)
            {
                number = number / 10000;
            }
            else
                number = number / 1;
        }
        return Convert.ToInt32(number);
    }
    public string GetCityFromIP(string IP)
    {/// <summary>
        /// This class returns the city,state,region,lat,long for a http request from a API call.
        /// </summary>
        String values = string.Empty;
        try
        {
            string city = string.Empty;
            string country = string.Empty;
            string region = string.Empty;
            string latit = string.Empty;
            string longit = string.Empty;
            string valIp = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load("http://www.ipgp.net/api/xml/" + IP + "/Eo66mITdn1");
            XmlNodeList nodes = doc.SelectNodes("IpLookup");
            foreach (XmlNode node in nodes)
            {
                valIp = node["Ip"].InnerText;
                city = node["City"].InnerText;
                country = node["Country"].InnerText;
                region = node["Region"].InnerText;
                latit = node["Lat"].InnerText;
                longit = node["Lng"].InnerText;
                values = valIp + ';' + city + ';' + country + ';' + region + ';' + latit + ';' + longit;
            }
        }
        catch (Exception)
        {

        }
        return values;
    }

    public string getMd5Hash(string input)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5 md5Hasher = MD5.Create();

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    public static IEnumerable<tblLogonId> SelectUsers()
    {
        AdminDataContext t = new AdminDataContext();
        return t.tblLogonIds;
    }

    public IEnumerable<string> GetStockQuote(string Symbol)
    {
        List<string> arlist = new List<string>();
        string strData = String.Empty;
        try
        {

            if (Symbol != string.Empty)
            {
                string url = "http://www.sunnyspeed.com/stock/api/?s=" + Symbol;
                // Create a web request object to access the web service
                // and add parameters to the web service in the query string
                HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);

                // Retrieve the response of the web service copy it in a string
                // with the help of stream reader
                HttpWebResponse webServiceResponse = (HttpWebResponse)myWebRequest.GetResponse();
                Stream streamResponse = webServiceResponse.GetResponseStream();
                StreamReader reader = new StreamReader(streamResponse, Encoding.UTF8);
                strData = reader.ReadToEnd();
                // Clean up
                reader.Close();
                webServiceResponse.Close();
                streamResponse.Close();
                string[] list = strData.Split(new char[] { '|' });
                foreach (string li in list)
                {
                    arlist.Add(li);
                }
            }
        }
        catch (Exception)
        {
        }
        return arlist;
    }


    //public Boolean ValidateEmailAddress(string email)
    //{
    //    Boolean IsValid = false;
    //    if (email != null)
    //    {   
    //        string url = string.Format("http://ws.cdyne.com/emailverify/Emailvernotestemail.asmx/VerifyEmail?email={0}" + "&LicenseKey=0", email);
    //        XDocument xmlDoc = XDocument.Load(url);
    //        foreach (var results in from p in xmlDoc.Root.Elements()
    //                                select p)
    //        {
    //            if (results.Name.LocalName.Equals("GoodEmail"))
    //                IsValid = Convert.ToBoolean(results.Value);
    //        }
                 
    //    }
    //    return IsValid;
    //}


    public IEnumerable<tblLogonId> Select(string user)
    {
        AdminDataContext ad = new AdminDataContext();
        return ad.tblLogonIds.Where(p => p.newId == Guid.Parse(user));
    }

    public static int SelectUserId(string user)
    {
        var rich = 0;
        try
        {     
        
            if (!string.IsNullOrEmpty(user))
            {
                AdminDataContext ad = new AdminDataContext();
                rich = (from j in ad.tblLogonIds
                        where j.newId.ToString() == user
                        select j.Id).SingleOrDefault();
            }           
        }
        catch 
        {
            
            throw new Exception("Unable to get user Id.");
        }
        
        return rich;
    }

    public bool IsValidUser(String userId)
    {
        bool retVal = false;
        AdminDataContext ad = new AdminDataContext();
        var validate = from j in ad.tblLogonIds
                       where j.newId.ToString() == userId
                       select j.IsAuthenticated;
        foreach (var g in validate)
        {
            retVal = g.Value;
        }
        return retVal;
    }

    public void Export(string fileName, GridView gv)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a table to contain the grid
                Table table = new Table();

                //  include the gridline settings
                table.GridLines = gv.GridLines;

                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    //GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                    for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
                    {
                        if (gv.HeaderRow.Cells[i].Text == "Id")
                        {
                        }
                    }

                }


                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    //GridViewExportUtil.PrepareControlForExport(row);

                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    //GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }


                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }

    public void ExportToCSV(GridView g, String outputFile)
    {
        int numCols = g.Columns.Count;
        int numRows = g.Rows.Count;
        StreamWriter sw = new StreamWriter(outputFile);
        if (g.Rows.Count != 0)
        {
            //Get Columns
            for (int k = 0; k < numCols; k++)
            {
                sw.Write(g.Columns[k].HeaderText);
                if (k < numCols - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            //Get Rows
            foreach (GridViewRow row in g.Rows)
            {
                for (int k = 0; k < g.Rows.Count; k++)
                {
                    for (int j = 0; j < g.Rows[k].Cells.Count; j++)
                    {
                        sw.Write(g.Rows[k].Cells[j].Text);
                    }
                }
            }
            sw.Write(sw.NewLine);
        }
        sw.Close();
    }
    public DateTime GetWeekStartDate()
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

    public DateTime GetWeekEndDate()
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

    public IEnumerable PopulateDates(string d1, string d2)
    {
        DateTime val1 = DateTime.Parse(d1);
        DateTime val2 = DateTime.Parse(d2);
        s03_DB_13029_randtDataContext db = new s03_DB_13029_randtDataContext();
        return db.getCurrentDates(val1, val2).ToList();
    }
    public Boolean ValidateUser(Guid user)
    {
        bool retVal = false;
        if (HttpContext.Current.Request.QueryString["enum"] != null)
        {
            string xVal = HttpContext.Current.Request.QueryString["enum"];
            xVal = System.Guid.NewGuid().ToString();
            //user = HttpContext.Current.Request.QueryString["enum"];

            if (!IsValidUser(xVal))
            {
                retVal = false;
                HttpContext.Current.Response.Redirect("Login.aspx", false);
            }
            else
            {
                retVal = true;
            }
        }
        return retVal;
    }

    public class GetApplicationValues
    {
        private string _UserIdValue;
        private string _EmailId;

        public string UId
        {
            get { return _UserIdValue; }
            set { _UserIdValue = value; }
        }
        public string EmailId
        {
            get { return _EmailId; }
            set { _EmailId = value; }
        }
    }

    public void GetError(string msg, string src, string stk)
    {
        HttpContext context = HttpContext.Current;
        Exception exception = context.Server.GetLastError();
        if (exception != null)
        {
            string message = exception.Message; string source = exception.Source; string stack = exception.StackTrace;
        }


        string dtTime = context.Timestamp.ToLongDateString() + " " + context.Timestamp.ToLongTimeString();
        StringBuilder errorInfo = new StringBuilder();
        errorInfo.Append(string.Format("***OFFENDING URL:*** {0}", context.Request.Url.ToString()));
        errorInfo.Append(string.Format("***SOURCE:***  {0}", src));
        errorInfo.Append(string.Format("***MESSAGE:***  {0}", msg));
        errorInfo.Append(string.Format("***STACK TRACE:***  {0}", stk));
        errorInfo.Append(string.Format("***IP ADDRESS:***  {0}:{1}", context.Request.UserHostAddress.ToString(), context.Request.CurrentExecutionFilePath.ToString()));
        SendEmail("postmaster@randtconsulting.com", "richard.ford@randtconsulting.com", "Error: " + dtTime, errorInfo.ToString());
        // To let the page finish running we clear the error
        HttpContext.Current.Response.Redirect("Error.aspx", false); context.Server.ClearError();

    }
    public virtual void WriteToErrorLog(ArrayList list)
    {
        ErrorLoggingDataContext logger = new ErrorLoggingDataContext();
        ErrorStack tblError = new ErrorStack();
        tblError.CreatedDate = DateTime.Now;
        if (!string.IsNullOrEmpty(list[2].ToString()))
            tblError.RequestorIp = list[2].ToString();
        if (!string.IsNullOrEmpty(list[3].ToString()))
            tblError.StackTrace = list[3].ToString();
        if (!string.IsNullOrEmpty(list[1].ToString()))
            tblError.newId = Guid.Parse(list[1].ToString());
        logger.ErrorStacks.InsertOnSubmit(tblError);
        logger.SubmitChanges();
    }
    public int NumRecords(int num)
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

    public void GetDataForXMLOut(GridView g, string fileName)
    {
        try
        {
            DataTable dt = new DataTable("tblExport");
            //Add Columns  
            if (g.HeaderRow != null)
            {
                for (int i = 0; i < g.HeaderRow.Cells.Count; i++)
                {
                    dt.Columns.Add(g.HeaderRow.Cells[i].Text);
                }
            }
            //Add Rows
            foreach (GridViewRow row in g.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int k = 0; k < row.Cells.Count; k++)
                {
                    dr[k] = row.Cells[k].Text;
                }
                dt.Rows.Add(dr);
            }

            dt.WriteXml(fileName, XmlWriteMode.IgnoreSchema);
        }
        catch (Exception)
        {
        }
    }


    public class BaseDt
    {
        private string getName;
        public string name
        {
            get { return getName; }
            set { getName = value; }
        }
    }
    public bool CheckUID(string expression)
    {
        if (expression != null)
        {
            Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

            return guidRegEx.IsMatch(expression.ToString());
        }
        return false;
    }

    public static void GetWebResponse()
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
   
    public class Session
    {
         private string CurSession = HttpContext.Current.Session.SessionID;
         public string ThisSession
         {
             get { return CurSession; }
             set { CurSession = value; }
         }
    }

    // url = "C:\\Users\\Rich\\Documents\\Desktop\\TestEmail.xml";
    //HttpWebRequest req =  (HttpWebRequest)WebRequest.Create(url);
    //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
    //Stream streamResponse = resp.GetResponseStream();
    //StreamReader streamReader = new StreamReader(streamResponse, Encoding.UTF8);
    //streamReader.ReadToEnd();
    //string xdoc = url;


    
}

 

    



    

   
  

   
  
