using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Net.Mail;

namespace GJTH_ISPC
{
    public partial class InspecList : System.Web.UI.Page
    {
        string strUserId = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split('\\')[1];
        ConnectionDB db = new ConnectionDB();
    
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                Label lblHd = (Label)Page.Master.FindControl("lblModule");
                lblHd.Text = "(Inspect List)";

                BindData();

                //ChkeckPermission();

            }
        }

        public void ChkeckPermission()
        {
            //string strPm = "Select * from ISPC_ItemInspect where MemoID='" + strUserId + "'";
            //DataSet ds = db.QueryToDataSetExt(strPm);

            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    if (ds.Tables[0].Rows[0]["WaxTree"].ToString() == "View" || ds.Tables[0].Rows[0]["WaxTree"].ToString() == "")
            //    {
            //        GridView1.Columns[7].Visible = false;
            //        btnNew.Enabled = false;
            //    }
            //    else if (ds.Tables[0].Rows[0]["WaxTree"].ToString() == "Edit")
            //    {
            //        GridView1.Columns[7].Visible = false;
            //    }
            //}

        }
        public void BindData()
        {

            string strsql = @"select distinct PO,MemoID,FORMAT(CreateDate, 'yyyy/MM/dd') CreateDate from ISPC_ItemInspect Where IsActive=1 ";

            if (txtMemoID.Text.Trim() != "")
            {
                strsql += " and MemoID like '%" + txtMemoID.Text + "%'";
            }

            if (txtPO.Text.Trim() != "")
            {
                strsql += " and PO like  '%" + txtPO.Text + "%'";
            }

            strsql += " order by CreateDate DESC ";

            DataSet ds = db.QueryToDataSetExt(strsql);
            string po = txtPO.Text.Trim();
            string memo = txtMemoID.Text.Trim();

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblMessage.Visible = false;
                Session["MyDataSet"] = ds;
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();

                lblMessage.Visible = true;

                if (!string.IsNullOrEmpty(po) && !string.IsNullOrEmpty(memo))
                {
                    lblMessage.Text = "⚠ No data found for PO : " + po +" | MemoID : " + memo;
                }
                else if (!string.IsNullOrEmpty(po))
                {
                    lblMessage.Text = "⚠ No data found for PO : " + po;
                }
                else if (!string.IsNullOrEmpty(memo))
                {
                    lblMessage.Text = "⚠ No data found for MemoID : " + memo;
                }
            }


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                Label lblInvoice = e.Row.FindControl("lblInvoice") as Label;
                lblInvoice.Text = rowView["MemoID"].ToString();


                Label lblPO = e.Row.FindControl("lblPO") as Label;
                lblPO.Text = rowView["PO"].ToString();


                Label lblCreateDate = e.Row.FindControl("lblCreateDate") as Label;
                if (rowView["CreateDate"].ToString() != "")
                {
                    lblCreateDate.Text = Convert.ToDateTime(rowView["CreateDate"]).ToString("dd/MM/yyyy");
                }

                //Label lblMinAxStartDate = e.Row.FindControl("lblMinAxStartDate") as Label;
                //if (rowView["SCHEDSTART"].ToString() != "")
                //{
                //    lblMinAxStartDate.Text = Convert.ToDateTime(rowView["SCHEDSTART"]).ToString("dd/MM/yyyy");
                //}


                //if (rowView["WaxTreeID"].ToString() != "" && Convert.ToBoolean(rowView["WaxTreeID"]))
                //{

                //    Image imgDel = e.Row.FindControl("imgDel") as Image;
                //    Image imgDelDis = e.Row.FindControl("imgDelDis") as Image;
                //    imgDel.Visible = false;
                //    imgDelDis.Visible = true;
                //}

            }

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.DataSource = Session["MyDataSet"];
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
            DataBind();
        }

        protected void btnSearchlist_Click(object sender, EventArgs e)
        {

            BindData();

        }
        protected void btnUp_Click(object sender, EventArgs e)
        {
          
            Response.Redirect("~/UploadFile.aspx");
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Session["Invoice"] = "";
            Session["PO"] = "";
            Response.Redirect("~/InspecDetail.aspx");
        }
        
        protected void LnkEdit_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(sender as ImageButton).Parent.Parent;
            Label lblInvoice = (Label)GridView1.Rows[row.RowIndex].FindControl("lblInvoice");
            Label lblPO = (Label)GridView1.Rows[row.RowIndex].FindControl("lblPO");
            Session["Invoice"] = lblInvoice.Text;
            Session["PO"] = lblPO.Text;
            Response.Redirect("~/InspecDetail.aspx");
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

        protected void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                //string strSql = @"select * from CMS_Casting th
                //    left outer join CMS_CastingDesc td on th.RowID = td.CastingID
                //    where th.IsActive=1 and td.WaxTreeID =  '" + HdeleteId.Value + "'";

                //DataTable dtChkUse = db.QueryToDataTableExt(strSql);
                //if (dtChkUse.Rows.Count > 0)
                //{
                //    ShowModalDialog("Warning", "Can not delete because Wax Tree has been used.", "OK", true);
                //    return;
                //}


                //strSql = "Select * from CMS_WaxTreeDesc where WaxTreeID='" + HdeleteId.Value + "'";
                //DataTable dtChkDesc = db.QueryToDataTableExt(strSql);
                //if (dtChkDesc.Rows.Count > 0)
                //{
                //    ShowModalDialog("Warning", "Can not delete because there are the production card in Waxtree.", "OK", true);
                //    return;
                //}

                /***----ปรับแก้ไข โค้ดเพิ่มเติ่ม ในการลบ ข้อมูลหน้า InspecList--02/02/2026---**/
                string strSql = "delete from ISPC_ItemInspect where IsActive=1  and  PO='" + HPO.Value+ "' and  MemoID='" + HdeleteId.Value+ "'";
                db.ExecuteDBExt(strSql);

                ShowModalDialog("Success", "Delete Completed", "OK", false);
                BindData();

            }
            catch
            {
                ShowModalDialog("Warning", "Can not delete,please contact administrator", "OK", true);
            }
        }


    }

}