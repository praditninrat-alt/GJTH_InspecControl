using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GJTH_ISPC
{
    public partial class UploadFile : System.Web.UI.Page
    {
        ConnectionDB db = new ConnectionDB();
        string strUserId = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split('\\')[1];
        public static string Fupload;
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!Page.IsPostBack)
            {
                Label lblHd = (Label)Page.Master.FindControl("lblModule");
                lblHd.Text = "(Upload File)";
            }

            if (IsPostBack && FileUpload1.PostedFile != null)
            {

                Fupload = FileUpload1.PostedFile.FileName;

                btnNewwindow.Click += new EventHandler(btnNewwindow_Click);

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
        private void btnNewwindow_Click(object sender, EventArgs e)
        {
            string strUserName = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split('\\')[1];
            FileUpload1.SaveAs(Server.MapPath("~/App_Data/tempfile-" + strUserName + ".xlsx"));
            string url = "~/ViewUploadDetail.aspx?fileUpload=" + System.Web.HttpUtility.UrlEncode(Fupload);
            Response.Redirect(url);


        }
    }
}