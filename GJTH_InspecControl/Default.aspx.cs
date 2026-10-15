using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GJTH_ISPC
{
    public partial class _Default : Page
    {
        string strUserId = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split('\\')[1];
        ConnectionDB db = new ConnectionDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/InspecList.aspx");
            //ChkeckPermission();
        }

        public void ChkeckPermission()
        {
            string strPm = "Select * from CMS_UserRole where UserId='" + strUserId + "'";
            DataSet ds = db.QueryToDataSetExt(strPm);

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["Joblist"].ToString() == "Admin" )
                {
                    Response.Redirect("~/AxJobList.aspx");
                }
                else if(ds.Tables[0].Rows[0]["Joblist"].ToString() == "Edit" || ds.Tables[0].Rows[0]["Joblist"].ToString() == "View")
                {
                    Response.Redirect("~/AxJobList.aspx");
                }else  if (ds.Tables[0].Rows[0]["WaxTree"].ToString() == "Edit" || ds.Tables[0].Rows[0]["WaxTree"].ToString() == "View")
                {
                    Response.Redirect("~/WaxTree.aspx");
                }else if  (ds.Tables[0].Rows[0]["Casting"].ToString() == "Edit" || ds.Tables[0].Rows[0]["Casting"].ToString() == "View")
                {
                    Response.Redirect("~/Casting.aspx");
                }else if  (ds.Tables[0].Rows[0]["Stock"].ToString() == "Edit" || ds.Tables[0].Rows[0]["Stock"].ToString() == "View")
                {
                    Response.Redirect("~/StockManage.aspx");
                }else if  (ds.Tables[0].Rows[0]["MTData"].ToString() == "Edit" || ds.Tables[0].Rows[0]["MTData"].ToString() == "View")
                {
                    Response.Redirect("~/ManageItemDesc2.aspx");

                }
                



            }
            
        }

        public void ShowModalDialog(string strHeader, string strDetail, string strButton, bool bIsWaring)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<script type='text/javascript'>");

            sb.Append("$('#textHeader').text('" + strHeader + "');");

            sb.Append("$('#mdTrnid').text('" + strDetail + "');");

            if (!string.IsNullOrEmpty(strButton))
            {
                sb.Append("$('#btnCancel').text('" + strButton + "');");
            }

            if (bIsWaring)
            {
                sb.Append("$('#btnSuccess').hide();");
            }
            else
            {
                sb.Append("$('#btnCancel').hide();");
            }

            sb.Append("$('#mdtrndetail').modal('show');");

            sb.Append("</script>");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalScript", sb.ToString(), false);

        }

        protected void btnSearchlist_Click(object sender, EventArgs e)
        {

            ShowModalDialog("Success!!", "Invoice has been updated", "OK", false);

        }
    }
}