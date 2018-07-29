using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Drawing;

public partial class Admin : System.Web.UI.Page
{
    int rowIdx;
    string userId;
    string deletedFileName;
    public static string selVal = string.Empty;
    public static string returnContentype = "";
    public static string downLoadedFile = "";
    BusLogic bl = new BusLogic();
    AdminDataContext ad = new AdminDataContext();
    LocationsDataContext loc = new LocationsDataContext();
    WorkDescDataContext workDes = new WorkDescDataContext();
    TNEDataContext tne = new TNEDataContext();
    ErrorLoggingDataContext err = new ErrorLoggingDataContext();
    s03_DB_13029_randtDataContext db = new s03_DB_13029_randtDataContext();
    Guid userIdent;
    protected void Page_PreInit(object sender, EventArgs e)
    {       
        if (Session["SessionId"] == null)
        {
            Response.Redirect("Login.aspx", false);
        }
    }

  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Role"].ToString() == "1")
        {
           
            RemoveMenuItemFromMasterMenu("Admin Console");
            
        }

        if (Session["UserId"] != null)
        {
            userId = Session["UserId"].ToString();
        }
        else
        {
            Response.Redirect("Error.aspx");

        }

        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        lblText.Text = "Select file(s) to delete from the remote server.";
        if (!IsPostBack)
        {           
            ListFilesInDirectory();
            if (!IsPostBack)
                ListServerFilesInDirectory();
            LoadTableList();
            ArrayList al = new ArrayList();
            foreach (var n in GetDBTables())
            {
                al.Add(n);
            }
            dList.DataSource = al;
            dList.DataBind();
        }
        if (!CheckIfItsMe())
        {
            upLoader.Visible = false;
            cmdUploadMe.Visible = false;
        }
        else
        {
            upLoader.Visible = true;
            cmdUploadMe.Visible = true;
        }
    }
    void RemoveMenuItemFromMasterMenu(String newMenuItem)
    {
        SiteMaster master = (SiteMaster)this.Master;
        Menu mnu = ((Menu)master.FindControl("NavigationMenu"));
        //MenuItem item = new MenuItem("Admin Console");
        MenuItem item = new MenuItem(newMenuItem[0].ToString());
        int k = 0;
        foreach (MenuItem i in mnu.Items)
        {
            if(i.Text == newMenuItem.ToString())
            {   //Remove it
               // mnu.Items.RemoveAt(k);
            }
        }



    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        AdminDataContext ad = new AdminDataContext();
        tblLogonId logIn = new tblLogonId();
        BusLogic bl = new BusLogic();
        if (txtNewUser.Text != string.Empty)
            logIn.UserName = txtNewUser.Text;
        if (txtPword.Text != string.Empty)
            logIn.Password = bl.getMd5Hash(txtPword.Text);
        if (txtRole.Text != string.Empty)
            logIn.Role = int.Parse(txtRole.Text);
        if (txtEmail.Text != string.Empty)
        {
            if (bl.ValidateEmail(txtEmail.Text))
                logIn.emailAddress = txtEmail.Text;
        }

        try
        {
            ad.tblLogonIds.InsertOnSubmit(logIn);
            ad.SubmitChanges();
            string fn = "alert('" + txtNewUser.Text + " was successfully added!');";
            ShowClientFunction(fn);
            btnView_Click(sender, e);
        }
        catch (Exception)
        {

        }

        finally
        {
            txtNewUser.Text = string.Empty;
            txtPword.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }
    }

    public void ShowClientFunction(string function)
    {
        string str = "<script language='javascript' type='text/javascript'>" + function + "</script>";
        Page.ClientScript.RegisterStartupScript(GetType(), "key", str);
    }
    protected void gView_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["OrigData"] = e.Row.Cells[2].Text;
            if (e.Row.Cells[2].Text.Length >= 20)
            {
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Substring(0, 20) + "See more...";
                e.Row.Cells[2].ToolTip = ViewState["OrigData"].ToString();
            }
        }
    }
    protected void gView_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gView.EditIndex = e.NewEditIndex;
        string url = String.Empty;
        int pk = int.Parse(gView.DataKeys[e.NewEditIndex].Value.ToString());
        if (pk != null && userId != null)
        {
            url = string.Format("UpdateRecord.aspx?recNum={0}&enum={1}", pk, userId);
            Response.Redirect(url);
        }
        else
        {
            Response.Redirect("Error.axpx");
        }
        
    }

    protected void gView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ViewState["index"] = e.RowIndex;
        int deleteId = 0;
        if (gView.DataKeys[e.RowIndex].Value.ToString() != null)
            deleteId = int.Parse(gView.DataKeys[e.RowIndex].Value.ToString());
        var users = (from j in ad.tblLogonIds
                     where j.Id == deleteId
                     select new
                     {
                         j.UserName
                     });
        foreach (var d in users)
        {

            lblmesg.Text = d.UserName;
        }

        lblmesg.Text = string.Format("{0} will be permanently deleted!!!", lblmesg.Text);
        ModalPopupExtender1.Show();
        pnl1.Visible = true;
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        gView.AutoGenerateDeleteButton = true;
        gView.DataSource = BusLogic.SelectUsers();
        gView.DataBind();
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        int deleteId = 0;
        if (ViewState["index"] != null)
        {
            rowIdx = int.Parse(ViewState["index"].ToString());
        }

        if (gView.DataKeys[rowIdx].Value.ToString() != null)
            deleteId = int.Parse(gView.DataKeys[rowIdx].Value.ToString());
        AdminDataContext admin = new AdminDataContext();
        var rec = (from d in admin.tblLogonIds
                   select d
                   ).Where(p => p.Id == deleteId);
        foreach (var deletedRecord in rec)
        {
            admin.tblLogonIds.DeleteOnSubmit(deletedRecord);
            admin.SubmitChanges();
        }
        gView.EditIndex = -1;
        var dat = BusLogic.SelectUsers();
        gView.DataSource = dat;
        gView.DataBind();
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {

    }

    protected void LoadTableList()
    {
        var list = GetDBTables();

    }

    protected IEnumerable GetDBTables()
    {
        s03_DB_13029_randtDataContext d = new s03_DB_13029_randtDataContext();
        return d.GetAllDBTables();
    }

    //protected void LoadData()
    //{
    //   string query = @"Select * from "+ cbListTables.SelectedValue;
    //   string res = string.Empty;
    //   bool flag = false;
    //   int selected = 0;
    //   IQueryable vals = null;
    //   IEnumerable results = null;
    //   switch(cbListTables.SelectedValue)
    //   {
    //       case "ErrorStack":
    //           results = err.ExecuteQuery<ErrorStack>(query);
    //           res = results.ToString();
    //           flag = res.Contains(cbListTables.SelectedValue);
    //           vals = from f in err.ErrorStacks
    //                   select f;
    //           break;
    //       case "tblLogonId":
    //           results = ad.ExecuteQuery<tblLogonId>(query);
    //           res = results.ToString();
    //           flag = res.Contains(cbListTables.SelectedValue);         
    //           vals = from f in ad.tblLogonIds
    //                   select f;
    //           break;
    //       case "tblLocations":
    //           results = loc.ExecuteQuery<tblLocation>(query);
    //           res = results.ToString();
    //           flag = res.Contains(cbListTables.SelectedValue.Substring(0,cbListTables.SelectedValue.Length -1));
    //           vals = from f in loc.tblLocations
    //                  select f;
    //           break;
    //       case "tblWorkDescriptions":
    //           results = workDes.ExecuteQuery<tblWorkDescription>(query);
    //           res = results.ToString();
    //           flag = res.Contains(cbListTables.SelectedValue.Substring(0, cbListTables.SelectedValue.Length - 1));
    //           vals = from f in workDes.tblWorkDescriptions
    //                  select f;
    //           break;
    //       case "tblTimeExpensesSummary":
    //           results = tne.ExecuteQuery<tblTimeExpensesSummary>(query);
    //           res = results.ToString();
    //           flag = res.Contains(cbListTables.SelectedValue);
    //           vals = from f in tne.tblTimeExpensesSummaries
    //                  select f;
    //           break;         
    //   }

    //   if (flag)
    //   {
    //       int cnt = cbListTables.Items.Count;

    //       try
    //       {
    //           for (int y = 0; y < cnt; y++)
    //           {
    //               if (cbListTables.Items[y].Selected)
    //               {
    //                   selected++;                      
    //               }                               
    //           } 

    //           if(selected== 1)
    //           {
    //               //ArrayList al = new ArrayList();
    //               //foreach (var n in results)
    //               //{
    //               //    al.Add(n);
    //               //}

    //               RecView.DataSource = vals;
    //               RecView.DataBind(); 
    //           }

    //       }
    //       catch (Exception)
    //       {
    //       }
    //   }
    //}


    protected void cbListTables_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadData();
    }

    protected void RecView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //RecView.PageIndex = e.NewPageIndex;
        //LoadData();
    }
    protected void RecView_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["OrigData"] = e.Row.Cells[2].Text;
            if (e.Row.Cells[2].Text.Length >= 20)
            {
                e.Row.Cells[2].Text = "See more...";
                e.Row.Cells[2].ToolTip = ViewState["OrigData"].ToString();
            }
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int succeed = 0;
        if (rdoSelectQuery.SelectedItem.Value != null)
        {
            if (rdoSelectQuery.SelectedItem.Value == "DML Script")
            {
                //RunQuery(); 
            }
            else
            {

                succeed = ExecuteDDL();
            }
        }
        if (succeed < 0)
        {
            string str = "alert('Script executed successfully!');";
            ShowClientFunctionInUpdatePanel(str);
        }
    }

    protected void qryView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        qryView.PageIndex = e.NewPageIndex;
        qryView.DataBind();

    }

    public void RunQuery()
    {
        string qry = "";
        qry = txtQuery.Text.Trim().ToLower();
        IEnumerable outPut = null;
        //Get database tables
        ArrayList al = new ArrayList();
        foreach (var n in GetDBTables())
        {
            al.Add(n);
        }

        s03_DB_13029_randtDataContext sb = new s03_DB_13029_randtDataContext();
        Array a = sb.GetAllDBTables().ToArray();
        for (int j = 0; j < a.Length; j++)
        {


            switch (((GetAllDBTablesResult)(a.GetValue(j))).name)
            {
                case "tblEmail":
                    IEnumerable<tblEmail> results = sb.ExecuteQuery<tblEmail>(qry);
                    qryView.DataSource = results;
                    break;
                case "tblLocation":
                    IEnumerable<tblLocation> results1 = sb.ExecuteQuery<tblLocation>(qry);
                    qryView.DataSource = results1;
                    break;
                case "tblLogonId":
                    IEnumerable<tblLogonId> results2 = sb.ExecuteQuery<tblLogonId>(qry);
                    qryView.DataSource = results2;
                    break;
                case "tblTimeExpensesSummary>":
                    IEnumerable<tblTimeExpensesSummary> results3 = sb.ExecuteQuery<tblTimeExpensesSummary>(qry);
                    qryView.DataSource = results3;
                    break;
                case "tblWorkDescription":
                    IEnumerable<tblWorkDescription> results4 = sb.ExecuteQuery<tblWorkDescription>(qry);
                    qryView.DataSource = results4;
                    break;

            }
        }

        qryView.DataBind();
    }

    public int ExecuteDDL()
    {
        int outPut = 0;
        string qry = "";
        qry = txtQuery.Text.Trim().ToLower();
        try
        {
            outPut = db.ExecuteCommand(qry);
        }
        catch (Exception)
        {
        }
        return outPut;
    }
    public void ShowClientFunctionInUpdatePanel(string fX)
    {
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                 Guid.NewGuid().ToString(), fX, true);
    }
    protected void cmdUploadMe_Click(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            SaveFile();          
        }
        ListFilesInDirectory(); 
    }

    public void SaveFile()
    {       
        string filename = ""; lblSub.Text = "";       
        HttpPostedFile postedFile = null;
        HttpFileCollection _files = HttpContext.Current.Request.Files;
        try
        {
            if (upLoader.HasFile)
            {
                for (int ifile = 0; ifile < _files.Count; ifile++)
                {
                    postedFile = _files[ifile];
                    if (postedFile.ContentLength != 0)
                    {
                        postedFile.SaveAs(HttpContext.Current.Request.MapPath("~/Upload/" + postedFile.FileName));
                        filename += postedFile.FileName + ";";
                    }
                }
                string[] arFileName = filename.Split(new char[] { ';' });
                lblSub.ForeColor = System.Drawing.Color.Green;
                if (string.IsNullOrEmpty(arFileName[1].ToString()))
                    lblSub.Text = string.Format("{0} uploaded successfully!!", arFileName[0].ToString());
                else
                    lblSub.Text = string.Format("{0} and {1} uploaded successfully!!", arFileName[0].ToString(), arFileName[1].ToString());
            }

            else
            {
                lblSub.ForeColor = System.Drawing.Color.Red;
                lblSub.Text = "Please select file to upload!";
            }
        }
        catch(Exception e)
        {
            Response.Redirect("Error.aspx");
        }
    }
        
    public Boolean CheckIfItsMe()
    {
        bool val = false;
        if (BusLogic.SelectUserId(Request.QueryString["enum"]) == 1)
        {
            val = true;
        }
        return val;
    }

    void DeleteFtpFile(string xFile)
    {
     
        if (!string.IsNullOrEmpty(xFile))
        {           
                FileInfo x = new FileInfo(xFile);
                xFile = x.Name;
                string ftpUrl = System.Configuration.ConfigurationManager.AppSettings["ftpUrl"].ToString();
                FtpWebRequest deleteRequest = (FtpWebRequest)WebRequest.Create(ftpUrl + xFile);
                deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                string userId = System.Configuration.ConfigurationManager.AppSettings["ftpUser"].ToString();
                string passwd = System.Configuration.ConfigurationManager.AppSettings["ftpAccess"].ToString();
                deleteRequest.Credentials = new NetworkCredential(userId, passwd);
                deleteRequest.KeepAlive = true;
                if (FilesInDirectory(xFile))
                {
                    try
                    {
                        FtpWebResponse response = (FtpWebResponse)deleteRequest.GetResponse();
                        string msg = response.ExitMessage + "\n";
                        msg += response.BannerMessage + "\n";
                        msg += response.StatusCode + "\n";
                        msg += response.StatusDescription;
                        if (response.StatusDescription.Contains("DELE"))
                        {
                            ListFilesInDirectory();
                        }
                        response.Close();
                        deleteRequest.Abort();
                    }
                    catch (WebException e)
                    {

                    }
                }               
        }
    }

    public int CountFilesInDirectory()
    {
        int count =0;
        string path = Server.MapPath("~/Upload/");
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] fi = di.GetFiles();
        count = cboFiles.Items.Count;
        return count;

    }

   public int parseFileSize(long number)
    {
        if(number !=0)
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

    public void ListFilesInDirectory()
    {
        cboFiles.Items.Clear();
        string path = Server.MapPath("~/Upload/");
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] fi = di.GetFiles();
        string fileText = "";
        string fName = "";
        foreach (var f in fi)
        {  
            long len = f.Length;
            if (len > 999)
                fName = string.Format("{0} Kb", parseFileSize(len));
            else if(len > 9999)
                fName = string.Format("{0} Mb", parseFileSize(len));
            else
                fName = string.Format("{0} bytes",len);
            fileText = string.Format("File: {0}    Created On: {1}    Size: {2}", f.Name, f.CreationTime, fName);
            cboFiles.Items.Add(new ListItem(fileText));
        }
    }


    public Boolean FilesInDirectory(string checkFile)
    {
        bool val = false;
        if (checkFile != null)
        {
            string path = Server.MapPath("~/Upload/");
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fi = di.GetFiles();
            foreach (var file in fi)
            {
                if (file.Name.Equals(checkFile))
                    val = true;
            }
        }
        return val;
    }
    protected void cboFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int idx = cboFiles.SelectedIndex;
            string selName = string.Empty;
            int FileCount = cboFiles.Items.Count;
            string path = Server.MapPath("~/Upload/");
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fileInfo = dir.GetFiles();

            for (int j = 0; j < FileCount; j++)
            {
                if (cboFiles.Items[j].Selected)
                {
                    selVal = fileInfo.ElementAt<FileInfo>(j).Name;
                }
            }

            if (cboFiles.SelectedIndex >= 0)
            {
                lblDeleteFile.Text = String.Format("{0} will be permanently removed. Are you certain?", selVal);
                MdlPopDeleteFile.Show();
                pnlDelete.Visible = true;
                Application.Remove("DeletedFileName");
                Application.Add("DeletedFileName", selVal);

            }
            cboFiles.ClearSelection();
        }
        catch (IndexOutOfRangeException idx)
        {
            Response.Redirect("Error.aspx", true);
        }
    }
    protected void btnYesDeleteFile_Click(object sender, EventArgs e)
    {
        if (selVal != null)
        {
            if (FilesInDirectory(selVal))
            {
                try
                {
                    DeleteFtpFile(selVal);
                }
                catch (Exception r)
                {
                    Response.Redirect("Error.aspx");
                }
            }
        }
        else
            lblText.Text = "The file selected did not exist on the remote server.";
    }
  
    public void ListServerFilesInDirectory()
    {
        ddlFileList.Items.Clear();
        string path = Server.MapPath("~/upload/");
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] fi = di.GetFiles();
        string fileText = "";
        string fName = "";
        foreach (var f in fi)
        {
            long len = f.Length;
            if (len > 999)
                fName = string.Format("{0} Kb", BusLogic.parseFileSize(len));
            else if (len > 9999)
                fName = string.Format("{0} Mb", BusLogic.parseFileSize(len));
            else
                fName = string.Format("{0} bytes", len);
            fileText = string.Format("File: {0}    Created On: {1}    Size: {2}", f.Name, f.CreationTime, fName);
            // ddlFileList.Items.Add(new ListItem(fileText));
            ddlFileList.Items.Add(new ListItem(f.Name));

        }
    }
    protected void ddlFileList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFileList.SelectedValue.Contains(".txt"))
            returnContentype = Constants.ContentTypePlain;
        else if (ddlFileList.SelectedValue.Contains(".htm") || ddlFileList.SelectedValue.Contains(".html"))
            returnContentype = Constants.ContentTypeHtml;
        else if (ddlFileList.SelectedValue.Contains(".doc") || ddlFileList.SelectedValue.Contains(".rtf") || ddlFileList.SelectedValue.Contains(".docx"))
            returnContentype = Constants.ContentTypeWORD;
        else if (ddlFileList.SelectedValue.Contains(".xls") || ddlFileList.SelectedValue.Contains(".xlsx"))
            returnContentype = Constants.ContentTypeEXCEL;
        else if (ddlFileList.SelectedValue.Contains(".jpg") || ddlFileList.SelectedValue.Contains(".jpeg"))
            returnContentype = Constants.ContentTypeJPEG;
        else if (ddlFileList.SelectedValue.Contains(".gif"))
            returnContentype = Constants.ContentTypePDF;
        else if (ddlFileList.SelectedValue.Contains(".pdf"))
            returnContentype = Constants.ContentTypePdf;
        else if (ddlFileList.SelectedValue.Contains(".cab") || ddlFileList.SelectedValue.Contains(".cab".ToUpper()))
            returnContentype = Constants.ContentTypeCAB;
        else if (ddlFileList.SelectedValue.Contains(".zip"))
            returnContentype = Constants.ContentTypeZIP;
        else returnContentype = Constants.ContentTypePlain;

        //ShowClientFunction(string.Format("alert('{0}');",ddlFileList.SelectedItem.Text + ddlFileList.SelectedIndex));
        downLoadedFile = ddlFileList.SelectedItem.Text;
        DownloadFile(returnContentype, downLoadedFile);
    }

    public void DownloadFile(string contentType, string LoadedFile)
    {
        try
        {
            string appendHeader = "";
            Response.ContentType = contentType;
            appendHeader = string.Format("attachment; filename={0}", LoadedFile);
            Response.AppendHeader("Content-Disposition", appendHeader);
            //Response.AppendHeader("Content-Disposition", "attachment; filename=TestFile.txt");
            Response.TransmitFile(Server.MapPath("~/Upload/" + LoadedFile));
            //Response.Clear();
            this.ShowClientFunction("alert();");
        }

        catch (Exception d)
        {
            Response.Redirect("Error.aspx");
        }



    }
}

  
