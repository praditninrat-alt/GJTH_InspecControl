using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GJTH_ISPC
{
    public partial class RedirectPrint : Page
    {
        ConnectionDB db = new ConnectionDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            //string strCastNo =  db.DecryptByKey(Request.QueryString["castNo"]);
            //string waxTreeCode = db.DecryptByKey(Request.QueryString["waxTreeCode"]);

           
            string strCastNo = Session["castNo"].ToString();
            string rptPath = System.Configuration.ConfigurationManager.AppSettings["ReportServer"].ToString();
            Response.Write("<meta http-equiv='X-UA-Compatible' content='IE=edge,chrome=1'>");

            if (Session["PrintType"].ToString() == "0")
            {
               
                string waxTreeCode = Session["waxTreeCode"].ToString();
                Response.Write("<script> window.open('http://" + rptPath + "/ReportServer/Pages/ReportViewer.aspx?%2fGJCMS%2fGJTH-CMS-004&rs:Command=Render&rc:Parameters=false&castNo=" + strCastNo + "&waxTreeCode=" + waxTreeCode + "'); </script>");
            }
            else if (Session["PrintType"].ToString() == "1")
            {
                string strJobNo = Session["JobNo"].ToString();
                string strWaxTree = Session["WaxTree"].ToString();
                Response.Write("<script> window.open('http://" + rptPath + "/ReportServer/Pages/ReportViewer.aspx?%2fGJCMS%2fGJTH-CMS-005&rs:Command=Render&rc:Parameters=false&castNo=" + strCastNo + "&JobID=" + strJobNo + "&WaxTreeCode="+ strWaxTree + "'); </script>");
            }
            else if (Session["PrintType"].ToString() == "2")
            {
                string strJobNo = Session["JobNo"].ToString();
                Response.Write("<script> window.open('http://" + rptPath + "/ReportServer/Pages/ReportViewer.aspx?%2fGJCMS%2fGJTH-CMS-001&rs:Command=Render&rc:Parameters=false&prodID=" + strJobNo + "'); </script>");
            }



            string jScript = "<script>window.close();</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", jScript);
        }

      
    }
}