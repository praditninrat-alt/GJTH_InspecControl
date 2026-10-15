using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace GJTH_ISPC
{ 
    public partial class PageMaster : System.Web.UI.MasterPage
    {
        string strUserId = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split('\\')[1];
        ConnectionDB db = new ConnectionDB();
        
   
        protected void Page_Load(object sender, EventArgs e)
        {

            lblUser.Text = strUserId;

            //imgH.Visible = strUserId == "nuntawan.k" ? true : false; 
            //imgH.Visible = strUserId == "kittisak.n" ? true : false;

            ChkeckPermission();

            //lblHType.Text = "General Bonded Warehouse";
            //lkImport.Visible = true;
            //lkImpShipment.Visible = true;
            //lkExpShipment.Visible = true;
            //lkRpt10Repair.Visible = false;
            //lkRpt5Repair.Visible = false;
            //lkRptAnnualRepair.Visible = false;
            //lkRptFcRepair.Visible = false;
            //lkRptFcReturn.Visible = false;
            //lkRptKpi.Visible = false;
            //lkMRptRepair.Visible = false;
            //lkMRptReturn.Visible = false;
            //lkFC.Visible = false;
            //divHeader.Style.Add("background-color", "#eb7a0b");
           
        }

        public void ChkeckPermission()
        {
            string strPm = "Select * from CMS_UserRole where UserId='" + strUserId + "'";
            DataSet ds = db.QueryToDataSetExt(strPm);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //if (ds.Tables[0].Rows[0]["Joblist"].ToString() != "Admin")
                //{
                //    lkAdmin.Visible = false;

                //    if (ds.Tables[0].Rows[0]["Joblist"].ToString() != "Edit" && ds.Tables[0].Rows[0]["Joblist"].ToString() != "View" && ds.Tables[0].Rows[0]["Joblist"].ToString() != "Delete")
                //    {
                //        lkJobList.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //        lkJobList.HRef = "";
                //    }

                //    if (ds.Tables[0].Rows[0]["WaxTree"].ToString() != "Edit" && ds.Tables[0].Rows[0]["WaxTree"].ToString() != "View" && ds.Tables[0].Rows[0]["WaxTree"].ToString() != "Delete")
                //    {
                //        lkWaxTree.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //        lkWaxTree.HRef = "";
                //    }

                //    if (ds.Tables[0].Rows[0]["Casting"].ToString() != "Edit" && ds.Tables[0].Rows[0]["Casting"].ToString() != "View" && ds.Tables[0].Rows[0]["Casting"].ToString() != "Delete")
                //    {
                //        lkCasting.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //        lkCasting.HRef = "";
                //    }

                //    if (ds.Tables[0].Rows[0]["Stock"].ToString() != "Edit" && ds.Tables[0].Rows[0]["Stock"].ToString() != "View" && ds.Tables[0].Rows[0]["Stock"].ToString() != "Delete")
                //    {
                //        lkStock.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //        lkStock.HRef = "";
                //    }

                //    if (ds.Tables[0].Rows[0]["MTData"].ToString() != "Edit" && ds.Tables[0].Rows[0]["MTData"].ToString() != "View" && ds.Tables[0].Rows[0]["MTData"].ToString() != "Delete")
                //    {
                //        lkMasterData.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //        lkMasterData.HRef = "";
                //    }
                //}
                //else
                //{
                //    lkAdmin.Visible = true;
                //}

            }
            else
            {

                //lkJobList.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //lkJobList.HRef = "";
                //lkWaxTree.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //lkWaxTree.HRef = "";
                //lkCasting.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //lkCasting.HRef = "";
                //lkStock.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //lkStock.HRef = "";
                //lkMasterData.Visible = false;
                //lkAdmin.Visible = false;
                //lkDoc.Visible = false;
                //lkMasterData.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //lkMasterData.HRef = "";
                //lkAdmin.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //lkAdmin.HRef = "";
                //lkDoc.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //lkDoc.HRef = "";
                //lkTOP.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Gray");
                //lkTOP.HRef = "";

            }
        }
    }
}