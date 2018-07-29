using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Data;
using System.Xml;


/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://randtconsulting.com/tne/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
 
public class WebService : System.Web.Services.WebService
{
    TNEDataContext tne = new TNEDataContext();
    tblTimeExpensesSummary tbl = new tblTimeExpensesSummary();
    BusLogic bl = new BusLogic();
    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 

    }

    //[WebMethod]
    //public string HelloWorld()
    //{
    //    return "Hello World";
    //}
    
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Xml)]
    public List<tblTimeExpensesSummary> GetData(int user)
    {
        var p = (from x in tne.tblTimeExpensesSummaries
                 where x.UserId == user
                select x);
        return p.ToList<tblTimeExpensesSummary>();
    }
}
