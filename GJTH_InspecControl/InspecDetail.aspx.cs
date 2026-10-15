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
    public partial class InspecDetail : System.Web.UI.Page
    {
        ConnectionDB db = new ConnectionDB();
        string strUserId = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split('\\')[1];
        DataTable dtTemp = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                Label lblHd = (Label)Page.Master.FindControl("lblModule");
                lblHd.Text = "(Inspect Detail)";
                lblInvData.Visible = false;
                txtInvData.Visible = false;
                //db.ExecuteDBExt("Delete from PLT_PlatingDescTmp where CreateBy='"+ strUserId + "'");

                if (Session["PO"].ToString() != "")
                {
                    txtInvSearch.Text = Session["Invoice"].ToString();
                    txtInvData.Text = Session["Invoice"].ToString();
                    hRowID.Value = Session["Invoice"].ToString();
                    txtPO.Text = Session["PO"].ToString();
                    btnSearch.Visible = false;
                    btnClear.Visible = false;

                    LoadData();

                    GridView1.Columns[26].Visible = false;
                }
                else
                {
                    btnSave.Enabled = false;

                }

                chkTypeChange();
                //ChkeckPermission();


            }
            dtTemp = ViewState["CurrentTable"] as DataTable;
        }

        public void ChkeckPermission()
        {
            string strPm = "Select * from CMS_UserRole where UserId='" + strUserId + "'";
            DataSet ds = db.QueryToDataSetExt(strPm);

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows[0]["WaxTree"].ToString() == "View" || ds.Tables[0].Rows[0]["WaxTree"].ToString() == "")
                {

                    GridView1.Columns[11].Visible = false;
                }
                else if (ds.Tables[0].Rows[0]["WaxTree"].ToString() == "Edit")
                {
                    GridView1.Columns[11].Visible = false;
                }

            }

        }


        public void BindData()
        {

            if (hRowID.Value == "")
            {
                string strsqlChk = @"select * from ISPC_ItemInspect Where IsActive=1 and MemoID <> '' and MemoID = '" + txtInvSearch.Text + "' ";
                DataTable dtChkUse = db.QueryToDataTableExt(strsqlChk);
                if (dtChkUse.Rows.Count > 0)
                {
                    ShowModalDialog("Warning", "Memo/Invoice already exists.", "OK", true);
                    return;
                }

            }


            //string strSql = @"select * from CMS_Casting th
            //        left outer join CMS_CastingDesc td on th.RowID = td.CastingID
            //        where th.IsActive=1 and td.WaxTreeID =  '" + hRowID.Value + "'";


            //string strsql = @"select ROW_NUMBER() OVER(ORDER BY vst.PURCHASELINELINENUMBER ASC) AS RowSeq,vj.PURCHORDERDOCNUM,pt.PURCHNAME,pl.CUSTOMERREF,vst.PURCHASELINELINENUMBER,vst.ORIGPURCHID,vst.ITEMID,vw.NAME,plv.PURCHQTY 'POQty',pl.PURCHQTY 'RecQty',vst.qty 'GoodQty',(pl.PURCHQTY-vst.qty) 'ScrapQty',pl.PURCHPRICE,vst.qty*pl.PURCHPRICE Amt
            //    ,NotInspecQty,GoodButOverPOQty,YellowQty,TooLightQty,CrackQty,BadCutQty,TooBigQty,TooSmallQty,Zoning,Clearity,Other,Remark,Reason
            //    ,ins.MissingQty,ins.OverQty,ins.RowID,ins.po,ins.MemoID,ins.ShelfNo ,ins.InspecType
            //    from VendPurchOrderJour vj left outer join
            //         (select ORIGPURCHID,Itemid,PURCHASELINELINENUMBER,sum(qty) qty from VendPackingSlipTrans 
            //        group by ORIGPURCHID,Itemid,PURCHASELINELINENUMBER
            //      ) vst on vst.ORIGPURCHID = vj.PURCHID left outer join 
            //      PurchLineAllVersions plv on vj.PURCHTABLEVERSION = plv.PURCHTABLEVERSIONRECID and vst.ITEMID = plv.ITEMID left outer join 
            //      PURCHLINE pl on pl.SOURCEDOCUMENTLINE = plv.SOURCEDOCUMENTLINE
            //      left outer join PURCHTABLE pt on pt.PURCHID = pl.PURCHID
            //      left outer join _V_ItemDesc vw on vw.ITEMID = vst.ITEMID
            //      left outer join [AX-EXT].dbo.ISPC_ItemInspect ins on ins.ItemID = vst.ITEMID and ins.po = vst.ORIGPURCHID and ins.MemoID = pl.CUSTOMERREF and ins.PurchLineNo = vst.PURCHASELINELINENUMBER
            //    where 1=1";

            //            string strsql = @"select ROW_NUMBER() OVER(ORDER BY pl.LINENUMBER ASC) AS RowSeq,vj.PURCHORDERDOCNUM,pt.PURCHNAME,pl.CUSTOMERREF,pl.LINENUMBER PURCHASELINELINENUMBER,pl.PURCHID ORIGPURCHID,plv.ITEMID,vw.NAME,plv.PURCHQTY 'POQty',pl.PURCHQTY 'RecQty',vst.qty 'GoodQty',(pl.PURCHQTY-vst.qty) 'ScrapQty',pl.PURCHPRICE,vst.qty*pl.PURCHPRICE Amt
            //                    ,NotInspecQty,GoodButOverPOQty,YellowQty,TooLightQty,CrackQty,BadCutQty,TooBigQty,TooSmallQty,Zoning,Clearity,Other,Remark,Reason
            //                    ,ins.MissingQty,ins.OverQty,ins.RowID,ins.po,ins.MemoID,ins.ShelfNo ,ins.InspecType
            //from VendPurchOrderJour vj 
            //left outer join PurchLineAllVersions plv  on vj.PURCHTABLEVERSION = plv.PURCHTABLEVERSIONRECID --and vst.ITEMID = plv.ITEMID
            // left outer join
            //     (select ORIGPURCHID,Itemid,PURCHASELINELINENUMBER,sum(qty) qty from VendPackingSlipTrans 
            //	   group by ORIGPURCHID,Itemid,PURCHASELINELINENUMBER
            //	 ) vst on vst.ORIGPURCHID = vj.PURCHID and vst.PURCHASELINELINENUMBER = plv.linenumber
            //	 left outer join PURCHLINE pl on pl.SOURCEDOCUMENTLINE = plv.SOURCEDOCUMENTLINE
            //	  left outer join PURCHTABLE pt on pt.PURCHID = pl.PURCHID
            //	                     left outer join _V_ItemDesc vw on vw.ITEMID = plv.ITEMID
            //	                     left outer join [AX-EXT].dbo.ISPC_ItemInspect ins on ins.ItemID = vst.ITEMID and ins.po = vst.ORIGPURCHID and ins.MemoID = pl.CUSTOMERREF and ins.PurchLineNo = vst.PURCHASELINELINENUMBER
            //where 1=1";

            string strsql = @"select ROW_NUMBER() OVER(ORDER BY pl.LINENUMBER ASC) AS RowSeq,pt.PURCHID,pt.PURCHNAME,pl.CUSTOMERREF,pl.LINENUMBER PURCHASELINELINENUMBER,pl.PURCHID ORIGPURCHID
                        ,pl.ITEMID,vw.NAME,pl.PURCHQTY 'POQty',pl.PURCHQTY 'RecQty'
                        ,0 'GoodQty',0 'ScrapQty'
                        ,pl.PURCHPRICE,pl.LINEAMOUNT Amt
                                            ,NotInspecQty,GoodButOverPOQty,YellowQty,TooLightQty,CrackQty,BadCutQty,TooBigQty,TooSmallQty,Zoning,Clearity,Other,Remark,Reason
                                            ,ins.MissingQty,ins.OverQty,ins.RowID,ins.po,ins.MemoID,ins.ShelfNo ,ins.InspecType,ins.ReturnQTY
                        from PURCHTABLE pt
                        left outer join Purchline pl on pt.purchID = pl.purchid
                        left outer join [AX-EXT].dbo.ISPC_ItemInspect ins on ins.ItemID = pl.ITEMID and ins.po = pt.PURCHID and ins.MemoID = pl.CUSTOMERREF and ins.PurchLineNo = pl.LINENUMBER
                        left outer join _V_ItemDesc vw on vw.ITEMID = pl.ITEMID
                        where 1=1";

            if (txtPO.Text.Trim() != "")
            {
                strsql += " and pt.purchID = '" + txtPO.Text + "'";
            }

            //if (txtInvSearch.Text.Trim() != "")
            //{
            //    //strsql += " and pl.CUSTOMERREF = '" + txtInvSearch.Text + "' and vj.PURCHORDERDOCNUM like  ('%-1')";
            //    strsql += " and pl.CUSTOMERREF = '" + txtInvSearch.Text + "'";
            //}

            strsql += " Order by pl.LINENUMBER ";

            DataTable dtData = db.QueryToDataTable(strsql);

            DataSet ds = db.QueryToDataSet(strsql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblMessage.Visible = false;
                Session["MyDataSet"] = ds;
                ViewState["CurrentTable"] = ds.Tables[0];
                ViewState["dt"] = dtData;
                GridView1.DataSource = ds;
                GridView1.DataBind();
                txtInvSearch.Enabled = false;
                txtPO.Enabled = false;
                btnSearch.Enabled = false;
                btnSave.Enabled = true;
                
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();

                lblMessage.Text = "⚠ No data found for PO : " + txtPO.Text.Trim();
            }
        }

        public void LoadData()
        {


            //    string strsql = @"select ROW_NUMBER() OVER(ORDER BY vst.PURCHASELINELINENUMBER ASC) AS RowSeq,vj.PURCHORDERDOCNUM,pt.PURCHNAME,pl.CUSTOMERREF,vst.PURCHASELINELINENUMBER,vst.ORIGPURCHID
            //    ,vst.ITEMID,vw.NAME,plv.PURCHQTY 'POQty',pl.PURCHQTY 'RecQty',vst.qty 'GoodQty',(pl.PURCHQTY-vst.qty) 'ScrapQty',pl.PURCHPRICE,vst.qty*pl.PURCHPRICE Amt
            //,NotInspecQty,GoodButOverPOQty,YellowQty,TooLightQty,CrackQty,BadCutQty,TooBigQty,TooSmallQty,Zoning,Clearity,Other,Remark,Reason
            //,ins.MissingQty,ins.OverQty,ins.RowID,ins.po,ins.MemoID,ins.ShelfNo ,ins.InspecType
            //from VendPurchOrderJour vj left outer join
            //     (select ORIGPURCHID,Itemid,PURCHASELINELINENUMBER,sum(qty) qty from VendPackingSlipTrans 
            //    group by ORIGPURCHID,Itemid,PURCHASELINELINENUMBER
            //  ) vst on vst.ORIGPURCHID = vj.PURCHID left outer join 
            //  PurchLineAllVersions plv on vj.PURCHTABLEVERSION = plv.PURCHTABLEVERSIONRECID and vst.ITEMID = plv.ITEMID left outer join 
            //  PURCHLINE pl on pl.SOURCEDOCUMENTLINE = plv.SOURCEDOCUMENTLINE
            //  left outer join PURCHTABLE pt on pt.PURCHID = pl.PURCHID
            //  left outer join _V_ItemDesc vw on vw.ITEMID = vst.ITEMID
            //  inner join [AX-EXT].dbo.ISPC_ItemInspect ins on ins.ItemID = vst.ITEMID and ins.po = vst.ORIGPURCHID  and ins.PurchLineNo = vst.PURCHASELINELINENUMBER
            //where 1=1 and vj.PURCHORDERDOCNUM = '" + txtPO.Text + "-1'";

            //string strsql = @"select ROW_NUMBER() OVER(ORDER BY pl.LINENUMBER ASC) AS RowSeq,vj.PURCHORDERDOCNUM,pt.PURCHNAME,pl.CUSTOMERREF,pl.LINENUMBER PURCHASELINELINENUMBER,pl.PURCHID ORIGPURCHID
            //    ,plv.ITEMID,vw.NAME,plv.PURCHQTY 'POQty',pl.PURCHQTY 'RecQty',vst.qty 'GoodQty',(pl.PURCHQTY-vst.qty) 'ScrapQty',pl.PURCHPRICE,vst.qty
            //    *pl.PURCHPRICE Amt
            //        ,NotInspecQty,GoodButOverPOQty,YellowQty,TooLightQty,CrackQty,BadCutQty,TooBigQty,TooSmallQty,Zoning,Clearity,Other,Remark,Reason
            //        ,ins.MissingQty,ins.OverQty,ins.RowID,ins.po,ins.MemoID,ins.ShelfNo ,ins.InspecType,ins.ReturnQTY
            //        from VendPurchOrderJour vj 
            //        left outer join PurchLineAllVersions plv  on vj.PURCHTABLEVERSION = plv.PURCHTABLEVERSIONRECID --and vst.ITEMID = plv.ITEMID
            //         left outer join
            //             (select ORIGPURCHID,Itemid,PURCHASELINELINENUMBER,sum(qty) qty from VendPackingSlipTrans 
            //            group by ORIGPURCHID,Itemid,PURCHASELINELINENUMBER
            //          ) vst on vst.ORIGPURCHID = vj.PURCHID and vst.PURCHASELINELINENUMBER = plv.linenumber
            //          left outer join PURCHLINE pl on pl.SOURCEDOCUMENTLINE = plv.SOURCEDOCUMENTLINE
            //           left outer join PURCHTABLE pt on pt.PURCHID = pl.PURCHID
            //                              left outer join _V_ItemDesc vw on vw.ITEMID = plv.ITEMID
            //                              inner join [AX-EXT].dbo.ISPC_ItemInspect ins on ins.ItemID = plv.ITEMID and ins.po =pl.PURCHID  and ins.PurchLineNo = pl.LINENUMBER 
            //         where  vj.PURCHORDERDOCNUM = '" + txtPO.Text + "-1' and ins.po = '" + txtPO.Text + "' and ins.MemoID='" + txtInvData.Text + "'";


            //strsql += " Order by pl.LINENUMBER ";

            string strsql = "";

            strsql = @" select ROW_NUMBER() OVER(ORDER BY pl.LINENUMBER ASC) AS RowSeq,pt.PURCHID,pt.PURCHNAME,pl.CUSTOMERREF,pl.LINENUMBER PURCHASELINELINENUMBER,pl.PURCHID ORIGPURCHID
                        ,pl.ITEMID,vw.NAME,pl.PURCHQTY 'POQty',ins.QTYRecv as 'RecQty',ins.QTYGood as 'GoodQty',ins.QTYScrap as 'ScrapQty'
                        ,pl.PURCHPRICE,pl.LINEAMOUNT Amt
                        ,NotInspecQty,GoodButOverPOQty,YellowQty,TooLightQty,CrackQty,BadCutQty,TooBigQty,TooSmallQty,Zoning
                        ,Clearity,Other,Remark,Reason
                        ,ins.MissingQty,ins.OverQty,ins.RowID,ins.po,ins.MemoID,ins.ShelfNo ,ins.InspecType,ins.ReturnQTY
                        from PURCHTABLE pt
                        left outer join Purchline pl on pt.purchID = pl.purchid
                        left outer join [AX-EXT].dbo.ISPC_ItemInspect ins on ins.ItemID = pl.ITEMID 
                        and ins.po = pt.PURCHID 
                        and ins.PurchLineNo = pl.LINENUMBER
                        left outer join _V_ItemDesc vw on vw.ITEMID = pl.ITEMID
                       where ins.po = '" + txtPO.Text + "' and ins.MemoID='" + txtInvData.Text + "'";
            strsql += " Order by pl.LINENUMBER ";

            DataTable dtData = db.QueryToDataTable(strsql);

            DataSet ds = db.QueryToDataSet(strsql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["MyDataSet"] = ds;
                ViewState["CurrentTable"] = ds.Tables[0];
                ViewState["dt"] = dtData;
                GridView1.DataSource = ds;
                GridView1.DataBind();
                txtInvSearch.Enabled = false;
                txtPO.Enabled = false;
                btnSearch.Enabled = false;
                btnSave.Enabled = true;

                rdoDiamond.Enabled = false;
                rdoStone.Enabled = false;
                //rdoSheetTube.Enabled = false;
                btnSave.Style["float"] = "right";
                btnSave.Text = "🚀 Update";
                btnSave.Style["width"] = "220px";
                btnSave.Style["padding"] = "8px 20px";
                if (ds.Tables[0].Rows[0]["InspecType"].ToString() != "")
                {
                    switch (Convert.ToInt32(ds.Tables[0].Rows[0]["InspecType"]))
                    {
                        case 0:
                            rdoDiamond.Checked = true;
                            rdoStone.Checked = false;
                            //rdoSheetTube.Checked = false;
                            lblInvData.Visible = false;
                            txtInvData.Visible = false;
                            lblInvSearch.Visible = true;
                            txtInvSearch.Visible = true;
                            break;
                        case 1:
                            rdoStone.Checked = true;
                            rdoDiamond.Checked = false;
                            //rdoSheetTube.Checked = false;
                            lblInvData.Visible = true;
                            txtInvData.Visible = true;
                            lblInvSearch.Visible = false;
                            txtInvSearch.Visible = false;
                            break;
                        case 2:
                            //rdoSheetTube.Checked = true;
                            rdoStone.Checked = false;
                            rdoDiamond.Checked = false;
                            lblInvData.Visible = true;
                            txtInvData.Visible = true;
                            lblInvSearch.Visible = false;
                            txtInvSearch.Visible = false;
                            break;
                    }
                }

            }
            


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                Label lblRowID = e.Row.FindControl("lblRowID") as Label;
                lblRowID.Text = rowView["RowID"].ToString();

                Label lblRowSeq = e.Row.FindControl("lblRowSeq") as Label;
                lblRowSeq.Text = rowView["RowSeq"].ToString();

                Label lblPO = e.Row.FindControl("lblPO") as Label;
                lblPO.Text = rowView["ORIGPURCHID"].ToString();

                Label lblMemo = e.Row.FindControl("lblMemo") as Label;
                lblMemo.Text = rowView["CUSTOMERREF"].ToString();

                Label lblItemID = e.Row.FindControl("lblItemID") as Label;
                lblItemID.Text = rowView["ItemID"].ToString();

                Label lblItemName = e.Row.FindControl("lblItemName") as Label;
                lblItemName.Text = rowView["Name"].ToString();

                TextBox txtRec = e.Row.FindControl("txtRec") as TextBox;
                if (rowView["RecQty"].ToString() != "")
                {
                    txtRec.Text = Convert.ToDouble(rowView["RecQty"]).ToString("#,##0.00");
                }

                TextBox txtGood = e.Row.FindControl("txtGood") as TextBox;
                if (rowView["GoodQty"].ToString() != "")
                {
                    txtGood.Text = Convert.ToDouble(rowView["GoodQty"]).ToString("#,##0.00");
                }

                TextBox txtScrap = e.Row.FindControl("txtScrap") as TextBox;
                if (rowView["ScrapQty"].ToString() != "")
                {
                    txtScrap.Text = Convert.ToDouble(rowView["ScrapQty"]).ToString("#,##0.00");
                }

                TextBox txtNotInspc = e.Row.FindControl("txtNotInspc") as TextBox;
                if (rowView["NotInspecQty"].ToString() != "")
                {
                    txtNotInspc.Text = Convert.ToDouble(rowView["NotInspecQty"]) != 0 ? Convert.ToDouble(rowView["NotInspecQty"]).ToString("#,##0.00") : "";
                }

                TextBox txtGoodButOverPO = e.Row.FindControl("txtGoodButOverPO") as TextBox;
                if (rowView["GoodButOverPOQty"].ToString() != "")
                {
                    txtGoodButOverPO.Text = Convert.ToDouble(rowView["GoodButOverPOQty"]) != 0 ? Convert.ToDouble(rowView["GoodButOverPOQty"]).ToString("#,##0.00") : "";
                }

                TextBox txtYellow = e.Row.FindControl("txtYellow") as TextBox;
                if (rowView["YellowQty"].ToString() != "")
                {
                    txtYellow.Text = Convert.ToDouble(rowView["YellowQty"]) != 0 ? Convert.ToDouble(rowView["YellowQty"]).ToString("#,##0.00") : "";
                }

                TextBox txtLight = e.Row.FindControl("txtLight") as TextBox;
                if (rowView["TooLightQty"].ToString() != "")
                {
                    txtLight.Text = Convert.ToDouble(rowView["TooLightQty"]) != 0 ? Convert.ToDouble(rowView["TooLightQty"]).ToString("#,##0.00") : "";
                }

                TextBox txtCrack = e.Row.FindControl("txtCrack") as TextBox;
                if (rowView["CrackQty"].ToString() != "")
                {
                    txtCrack.Text = Convert.ToDouble(rowView["CrackQty"]) != 0 ? Convert.ToDouble(rowView["CrackQty"]).ToString("#,##0.00") : "";
                }

                TextBox txtBadCut = e.Row.FindControl("txtBadCut") as TextBox;
                if (rowView["BadCutQty"].ToString() != "")
                {
                    txtBadCut.Text = Convert.ToDouble(rowView["BadCutQty"]) != 0 ? Convert.ToDouble(rowView["BadCutQty"]).ToString("#,##0.00") : "";
                }

                TextBox txtBig = e.Row.FindControl("txtBig") as TextBox;
                if (rowView["TooBigQty"].ToString() != "")
                {
                    txtBig.Text = Convert.ToDouble(rowView["TooBigQty"]) != 0 ? Convert.ToDouble(rowView["TooBigQty"]).ToString("#,##0.00") : "";
                }

                TextBox txtSmall = e.Row.FindControl("txtSmall") as TextBox;
                if (rowView["TooSmallQty"].ToString() != "")
                {
                    txtSmall.Text = Convert.ToDouble(rowView["TooSmallQty"]) != 0 ? Convert.ToDouble(rowView["TooSmallQty"]).ToString("#,##0.00") : "";
                }

                TextBox txtZoning = e.Row.FindControl("txtZoning") as TextBox;
                if (rowView["Zoning"].ToString() != "")
                {
                    txtZoning.Text = Convert.ToDouble(rowView["Zoning"]) != 0 ? Convert.ToDouble(rowView["Zoning"]).ToString("#,##0.00") : "";
                }

                TextBox txtClearity = e.Row.FindControl("txtClearity") as TextBox;
                if (rowView["Clearity"].ToString() != "")
                {
                    txtClearity.Text = Convert.ToDouble(rowView["Clearity"]) != 0 ? Convert.ToDouble(rowView["Clearity"]).ToString("#,##0.00") : "";
                }

                TextBox txtOther = e.Row.FindControl("txtOther") as TextBox;
                if (rowView["Other"].ToString() != "")
                {
                    txtOther.Text = Convert.ToDouble(rowView["Other"]) != 0 ? Convert.ToDouble(rowView["Other"]).ToString("#,##0.00") : "";
                }

                TextBox txtRemark = e.Row.FindControl("txtRemark") as TextBox;
                txtRemark.Text = rowView["Remark"].ToString();

                TextBox txtMissingQty = e.Row.FindControl("txtMissingQty") as TextBox;
                if (rowView["MissingQty"].ToString() != "")
                {
                    txtMissingQty.Text = Convert.ToDouble(rowView["MissingQty"]) != 0 ? Convert.ToDouble(rowView["MissingQty"]).ToString("#,##0.00") : "";
                }

                TextBox txtOverQty = e.Row.FindControl("txtOverQty") as TextBox;
                if (rowView["OverQty"].ToString() != "")
                {
                    txtOverQty.Text = Convert.ToDouble(rowView["OverQty"]) != 0 ? Convert.ToDouble(rowView["OverQty"]).ToString("#,##0.00") : "";
                }


                TextBox txtReason = e.Row.FindControl("txtReason") as TextBox;
                txtReason.Text = rowView["Reason"].ToString();

                TextBox txtShelfNo = e.Row.FindControl("txtShelfNo") as TextBox;
                txtShelfNo.Text = rowView["ShelfNo"].ToString();


                Label lblPurchLine = e.Row.FindControl("lblPurchLine") as Label;
                lblPurchLine.Text = rowView["PURCHASELINELINENUMBER"].ToString();
                //Label lblAxStartDate = e.Row.FindControl("lblAxStartDate") as Label;
                //if (rowView["SCHEDSTART"].ToString() != "")
                //{
                //    lblAxStartDate.Text = Convert.ToDateTime(rowView["SCHEDSTART"]).ToString("dd/MM/yyyy");
                //}

                /***** สร้าง Field --- ReturnQTY --- 01/02/2026 ****/
                TextBox txtReturnQTY = e.Row.FindControl("txtReturnQTY") as TextBox;
                if (rowView["ReturnQTY"].ToString() != "")
                {
                    txtReturnQTY.Text = Convert.ToDouble(rowView["ReturnQTY"]) != 0 ? Convert.ToDouble(rowView["ReturnQTY"]).ToString("#,##0.00") : "";
                }

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.DataSource = Session["MyDataSet"];
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
            DataBind();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            BindData();
            if (GridView1.Rows.Count > 0)
            {
                txtPO.Enabled = false;
                txtInvSearch.Enabled = false;
                btnSearch.Enabled = false;

                lblInvData.Visible = true;
                txtInvData.Visible = true;

                //if (rdoDiamond.Checked)
                //{
                //    lblInvData.Visible = true;
                //    txtInvData.Visible = true;
                //}
                //else
                //{
                //lblInvData.Visible = true;
                //txtInvData.Visible = true;
                //}
            }

        }

        protected void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                /***--เพิ่มโต้ดในส่วนของการลบข้อมูล ราย ItemID---01/02/2026-***/
                DataRow[] drr = dtTemp.Select("RowSeq=" + Convert.ToInt32(HdeleteId.Value));

                if (drr.Length > 0)
                {
                    string PO = drr[0]["PO"].ToString();
                    string MemoID = drr[0]["MemoID"].ToString();
                    string ItemID = drr[0]["ItemID"].ToString();

                    string strSql =
                    "DELETE FROM ISPC_ItemInspect " +
                    "WHERE PO='" + PO + "'" +
                    " AND MemoID='" + MemoID + "'" +
                    " AND ItemID='" + ItemID + "'" +
                    " AND IsActive=1";

                    db.ExecuteDBExt(strSql);

                    drr[0].Delete();

                    dtTemp.AcceptChanges();

                    GridView1.DataSource = dtTemp;
                    GridView1.DataBind();
                    ShowModalDialog("Success", "Delete Completed", "OK", true);

                }

                //DataRow[] drr = dtTemp.Select("RowSeq=" + Convert.ToInt32(HdeleteId.Value) + "");
                //for (int i = 0; i < drr.Length; i++)
                //    drr[i].Delete();
                //dtTemp.AcceptChanges();
                //GridView1.DataSource = dtTemp;
                //GridView1.DataBind();

                ////---------------------------------------------

                //string strSql = @"select tdw.JobID from CMS_Casting th
                //    left outer join CMS_CastingDesc td on th.RowID = td.CastingID
                //    left outer join CMS_WaxTree thw on thw.RowID = td.WaxTreeID
                //    left outer join CMS_WaxTreeDesc tdw on thw.RowID = tdw.WaxTreeID
                //    where th.IsActive = 1 and thw.IsActive = 1 and tdw.JobID = '"+ HJobID.Value + "'";

                //DataTable dtChkUse = db.QueryToDataTableExt(strSql);
                //if (dtChkUse.Rows.Count > 0)
                //{
                //    ShowModalDialog("Warning", "Can not delete because Job card has been used.", "OK", true);
                //    return;
                //}


                //string strSql = "Delete From  PLT_PlatingDescTmp  where RowID='" + HdeleteId.Value + "'";
                //db.ExecuteDBExt(strSql);


                //ShowModalDialog("Success!!", "Delete Completed", "OK", false);

            }
            catch
            {
                ShowModalDialog("Warning", "Can not delete,please contact administrator", "OK", false);
            }
        }

        public void ShowModalDialogPopResp(string strHeader, string strDetail)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<script type='text/javascript'>");

            sb.Append("$('#showprog').modal('hide');");

            sb.Append("$('#mdResHead').text('" + strHeader + "');");

            sb.Append("$('#mdResDetail').text('" + strDetail + "');");

            sb.Append("$('#mdPopResponse').modal('show');");


            sb.Append("</script>");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalScripts", sb.ToString(), false);

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            txtInvSearch.Text = "";
            txtPO.Text = "";
            txtPO.Enabled = true;
            txtInvSearch.Enabled = true;
            rdoDiamond.Checked = true;
            rdoDiamond.Enabled = true;
            rdoStone.Enabled = true;
            //rdoSheetTube.Enabled = true;
            hRowID.Value = "";
            btnSearch.Enabled = true;
            btnSave.Text = "Save New Memo/Invoice";
            btnSave.Enabled = false;

            txtInvData.Text = "";
            lblInvData.Visible = false;
            txtInvData.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            int iInspecTyhpe = 0;
            if (rdoDiamond.Checked) { iInspecTyhpe = 0; }
            else if (rdoStone.Checked) { iInspecTyhpe = 1; }
            //else if (rdoSheetTube.Checked) { iInspecTyhpe = 2; }
            try
            {
                if (hRowID.Value != "")
                {
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        Label lblRowID = (Label)GridView1.Rows[i].FindControl("lblRowID");
                        Label lblPO = (Label)GridView1.Rows[i].FindControl("lblPO");
                        Label lblMemo = (Label)GridView1.Rows[i].FindControl("lblMemo");
                        Label lblItemID = (Label)GridView1.Rows[i].FindControl("lblItemID");
                        Label lblItemName = (Label)GridView1.Rows[i].FindControl("lblItemName");

                        Label lblRec = (Label)GridView1.Rows[i].FindControl("lblRec");
                        TextBox txtRec = (TextBox)GridView1.Rows[i].FindControl("txtRec");
                        decimal dRec = txtRec.Text == "" ? 0 : Convert.ToDecimal(txtRec.Text);

                        Label lblGood = (Label)GridView1.Rows[i].FindControl("lblGood");
                        TextBox txtGood = (TextBox)GridView1.Rows[i].FindControl("txtGood");
                        decimal dGood = txtGood.Text == "" ? 0 : Convert.ToDecimal(txtGood.Text);

                        Label lblScrap = (Label)GridView1.Rows[i].FindControl("lblScrap");
                        TextBox txtScrap = (TextBox)GridView1.Rows[i].FindControl("txtScrap");
                        decimal dScrap = txtScrap.Text == "" ? 0 : Convert.ToDecimal(txtScrap.Text);

                        Label lblPurchLine = (Label)GridView1.Rows[i].FindControl("lblPurchLine");
                        int iPurchLine = lblPurchLine.Text == "" ? 0 : Convert.ToInt32(lblPurchLine.Text);
                        TextBox txtNotInspc = (TextBox)GridView1.Rows[i].FindControl("txtNotInspc");
                        decimal dNotInspc = txtNotInspc.Text == "" ? 0 : Convert.ToDecimal(txtNotInspc.Text);
                        TextBox txtGoodButOverPO = (TextBox)GridView1.Rows[i].FindControl("txtGoodButOverPO");
                        decimal dGoodButOverPO = txtGoodButOverPO.Text == "" ? 0 : Convert.ToDecimal(txtGoodButOverPO.Text);
                        TextBox txtYellow = (TextBox)GridView1.Rows[i].FindControl("txtYellow");
                        decimal dYellow = txtYellow.Text == "" ? 0 : Convert.ToDecimal(txtYellow.Text);
                        TextBox txtLight = (TextBox)GridView1.Rows[i].FindControl("txtLight");
                        decimal dLight = txtLight.Text == "" ? 0 : Convert.ToDecimal(txtLight.Text);
                        TextBox txtCrack = (TextBox)GridView1.Rows[i].FindControl("txtCrack");
                        decimal dCrack = txtCrack.Text == "" ? 0 : Convert.ToDecimal(txtCrack.Text);
                        TextBox txtBadCut = (TextBox)GridView1.Rows[i].FindControl("txtBadCut");
                        decimal dBadCut = txtBadCut.Text == "" ? 0 : Convert.ToDecimal(txtBadCut.Text);
                        TextBox txtBig = (TextBox)GridView1.Rows[i].FindControl("txtBig");
                        decimal dBig = txtBig.Text == "" ? 0 : Convert.ToDecimal(txtBig.Text);
                        TextBox txtSmall = (TextBox)GridView1.Rows[i].FindControl("txtSmall");
                        decimal dSmall = txtSmall.Text == "" ? 0 : Convert.ToDecimal(txtSmall.Text);
                        TextBox txtZoning = (TextBox)GridView1.Rows[i].FindControl("txtZoning");
                        decimal dZoning = txtZoning.Text == "" ? 0 : Convert.ToDecimal(txtZoning.Text);
                        TextBox txtClearity = (TextBox)GridView1.Rows[i].FindControl("txtClearity");
                        decimal dClearity = txtClearity.Text == "" ? 0 : Convert.ToDecimal(txtClearity.Text);
                        TextBox txtOther = (TextBox)GridView1.Rows[i].FindControl("txtOther");
                        decimal dOther = txtOther.Text == "" ? 0 : Convert.ToDecimal(txtOther.Text);
                        TextBox txtRemark = (TextBox)GridView1.Rows[i].FindControl("txtRemark");
                        TextBox txtMissingQty = (TextBox)GridView1.Rows[i].FindControl("txtMissingQty");
                        decimal dMissingQty = txtMissingQty.Text == "" ? 0 : Convert.ToDecimal(txtMissingQty.Text);
                        TextBox txtOverQty = (TextBox)GridView1.Rows[i].FindControl("txtOverQty");
                        decimal dOverQty = txtOverQty.Text == "" ? 0 : Convert.ToDecimal(txtOverQty.Text);
                        TextBox txtReason = (TextBox)GridView1.Rows[i].FindControl("txtReason");
                        TextBox txtShelfNo = (TextBox)GridView1.Rows[i].FindControl("txtShelfNo");

                        TextBox txtReturnQTY = (TextBox)GridView1.Rows[i].FindControl("txtReturnQTY");
                        decimal dReturnQTY = txtReturnQTY.Text == "" ? 0 : Convert.ToDecimal(txtReturnQTY.Text);

                        string strsql = "Update ISPC_ItemInspect Set ";
                        strsql += " MemoID='" + txtInvData.Text + "',";

                        strsql += " NotInspecQty=" + dNotInspc + ",";
                        strsql += " GoodButOverPOQty=" + dGoodButOverPO + ",";
                        strsql += " YellowQty=" + dYellow + ",";
                        strsql += " TooLightQty=" + dLight + ",";
                        strsql += " CrackQty=" + dCrack + ",";
                        strsql += " BadCutQty=" + dBadCut + ",";
                        strsql += " TooBigQty=" + dBig + ",";
                        strsql += " TooSmallQty=" + dSmall + ",";
                        strsql += " Zoning=" + dZoning + ",";
                        strsql += " Clearity=" + dClearity + ",";
                        strsql += " Other=" + dOther + ",";
                        strsql += " OverQty=" + dOverQty + ",";
                        strsql += " MissingQty=" + dMissingQty + ",";
                        strsql += " Remark='" + txtRemark.Text + "',";
                        strsql += " Reason='" + txtReason.Text + "',";
                        strsql += " ShelfNo='" + txtShelfNo.Text + "',";
                        strsql += " ReturnQTY=" + dReturnQTY + ",";
                        /** เพิ่ม Field ModifyDate ที่ DataBase-- 01/02/2026 --**/
                        strsql += " QTYRecv=" + dRec + ",";
                        strsql += " QTYGood=" + dGood + ",";
                        strsql += " QTYScrap=" + dScrap + ",";
                        strsql += " ModifyDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        strsql += " Where RowID=" + Convert.ToInt32(lblRowID.Text) + "";


                        db.ExecuteDBExt(strsql);


                    }
                    ShowModalDialog("Success", "Data have been updated.", "OK", false);
                }
                else
                {

                    string strsqlChk = @"select * from ISPC_ItemInspect Where IsActive=1  and MemoID = '" + txtInvData.Text + "'";
                    DataTable dtChkUse = db.QueryToDataTableExt(strsqlChk);
                    if (dtChkUse.Rows.Count > 0)
                    {
                        ShowModalDialog("Warning", "Memo/Invoice already exists.", "OK", true);
                        return;
                    }

                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        Label lblRowID = (Label)GridView1.Rows[i].FindControl("lblRowID");
                        Label lblPO = (Label)GridView1.Rows[i].FindControl("lblPO");
                        Label lblMemo = (Label)GridView1.Rows[i].FindControl("lblMemo");
                        Label lblItemID = (Label)GridView1.Rows[i].FindControl("lblItemID");
                        Label lblItemName = (Label)GridView1.Rows[i].FindControl("lblItemName");

                        Label lblRec = (Label)GridView1.Rows[i].FindControl("lblRec");
                        TextBox txtRec = (TextBox)GridView1.Rows[i].FindControl("txtRec");
                        decimal dRec = txtRec.Text == "" ? 0 : Convert.ToDecimal(txtRec.Text);

                        Label lblGood = (Label)GridView1.Rows[i].FindControl("lblGood");
                        TextBox txtGood = (TextBox)GridView1.Rows[i].FindControl("txtGood");
                        decimal dGood = txtGood.Text == "" ? 0 : Convert.ToDecimal(txtGood.Text);

                        Label lblScrap = (Label)GridView1.Rows[i].FindControl("lblScrap");
                        TextBox txtScrap = (TextBox)GridView1.Rows[i].FindControl("txtScrap");
                        decimal dScrap = txtScrap.Text == "" ? 0 : Convert.ToDecimal(txtScrap.Text);


                        Label lblPurchLine = (Label)GridView1.Rows[i].FindControl("lblPurchLine");
                        int iPurchLine = lblPurchLine.Text == "" ? 0 : Convert.ToInt32(lblPurchLine.Text);
                        TextBox txtNotInspc = (TextBox)GridView1.Rows[i].FindControl("txtNotInspc");
                        decimal dNotInspc = txtNotInspc.Text == "" ? 0 : Convert.ToDecimal(txtNotInspc.Text);
                        TextBox txtGoodButOverPO = (TextBox)GridView1.Rows[i].FindControl("txtGoodButOverPO");
                        decimal dGoodButOverPO = txtGoodButOverPO.Text == "" ? 0 : Convert.ToDecimal(txtGoodButOverPO.Text);
                        TextBox txtYellow = (TextBox)GridView1.Rows[i].FindControl("txtYellow");
                        decimal dYellow = txtYellow.Text == "" ? 0 : Convert.ToDecimal(txtYellow.Text);
                        TextBox txtLight = (TextBox)GridView1.Rows[i].FindControl("txtLight");
                        decimal dLight = txtLight.Text == "" ? 0 : Convert.ToDecimal(txtLight.Text);
                        TextBox txtCrack = (TextBox)GridView1.Rows[i].FindControl("txtCrack");
                        decimal dCrack = txtCrack.Text == "" ? 0 : Convert.ToDecimal(txtCrack.Text);
                        TextBox txtBadCut = (TextBox)GridView1.Rows[i].FindControl("txtBadCut");
                        decimal dBadCut = txtBadCut.Text == "" ? 0 : Convert.ToDecimal(txtBadCut.Text);
                        TextBox txtBig = (TextBox)GridView1.Rows[i].FindControl("txtBig");
                        decimal dBig = txtBig.Text == "" ? 0 : Convert.ToDecimal(txtBig.Text);
                        TextBox txtSmall = (TextBox)GridView1.Rows[i].FindControl("txtSmall");
                        decimal dSmall = txtSmall.Text == "" ? 0 : Convert.ToDecimal(txtSmall.Text);
                        TextBox txtZoning = (TextBox)GridView1.Rows[i].FindControl("txtZoning");
                        decimal dZoning = txtZoning.Text == "" ? 0 : Convert.ToDecimal(txtZoning.Text);
                        TextBox txtClearity = (TextBox)GridView1.Rows[i].FindControl("txtClearity");
                        decimal dClearity = txtClearity.Text == "" ? 0 : Convert.ToDecimal(txtClearity.Text);
                        TextBox txtOther = (TextBox)GridView1.Rows[i].FindControl("txtOther");
                        decimal dOther = txtOther.Text == "" ? 0 : Convert.ToDecimal(txtOther.Text);
                        TextBox txtRemark = (TextBox)GridView1.Rows[i].FindControl("txtRemark");
                        TextBox txtMissingQty = (TextBox)GridView1.Rows[i].FindControl("txtMissingQty");
                        decimal dMissingQty = txtMissingQty.Text == "" ? 0 : Convert.ToDecimal(txtMissingQty.Text);
                        TextBox txtOverQty = (TextBox)GridView1.Rows[i].FindControl("txtOverQty");
                        decimal dOverQty = txtOverQty.Text == "" ? 0 : Convert.ToDecimal(txtOverQty.Text);
                        TextBox txtReason = (TextBox)GridView1.Rows[i].FindControl("txtReason");
                        TextBox txtShelfNo = (TextBox)GridView1.Rows[i].FindControl("txtShelfNo");

                        TextBox txtReturnQTY = (TextBox)GridView1.Rows[i].FindControl("txtReturnQTY");
                        decimal dReturnQTY = txtReturnQTY.Text == "" ? 0 : Convert.ToDecimal(txtReturnQTY.Text);


                        string strsql = String.Format(@"Insert into ISPC_ItemInspect (PO,MemoID,ItemID,ItemName,NotInspecQty,GoodButOverPOQty,YellowQty,TooLightQty
                                ,CrackQty,BadCutQty,TooBigQty,TooSmallQty,Zoning,Clearity,Other,OverQty,MissingQty,Remark,Reason,ShelfNo,CreateDate,PurchLineNo,InspecType,ReturnQTY,QTYRecv,QTYGood,QTYScrap,IsActive) 
                                Values('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},'{17}','{18}','{19}','{20}',{21},{22},{23},{24},{25},{26},1)"
                                , lblPO.Text, txtInvData.Text, lblItemID.Text, lblItemName.Text.Replace("'", "''")
                                , dNotInspc, dGoodButOverPO, dYellow, dLight, dCrack, dBadCut, dBig, dSmall, dZoning, dClearity, dOther, dOverQty, dMissingQty
                               , txtRemark.Text, txtReason.Text, txtShelfNo.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), iPurchLine, iInspecTyhpe, dReturnQTY, dRec, dGood, dScrap);
                        db.ExecuteDBExt(strsql);

                    }
                    ShowModalDialog("Success", "Data have been insert", "OK", false);
                }
            }
            catch
            {
                ShowModalDialog("Warning", "Can not updated,please contact administrator", "OK", false);
            }
        }


        protected void rdoChange(object sender, EventArgs e)
        {
            chkTypeChange();
        }

        public void chkTypeChange()
        {
            if (rdoDiamond.Checked)
            {
                lblInvSearch.Visible = true;
                txtInvSearch.Visible = true;
                GridView1.Columns[9].Visible = true;
                GridView1.Columns[10].Visible = true;
                GridView1.Columns[11].Visible = true;
                GridView1.Columns[12].Visible = true;
                GridView1.Columns[13].Visible = true;
                GridView1.Columns[14].Visible = true;
                GridView1.Columns[15].Visible = true;
                GridView1.Columns[16].Visible = true;
                GridView1.Columns[17].Visible = true;
                GridView1.Columns[18].Visible = true;
                GridView1.Columns[19].Visible = true;
                GridView1.Columns[20].Visible = true;
                GridView1.Columns[21].Visible = true;
                GridView1.Columns[22].Visible = true;
                GridView1.Columns[23].Visible = false;
                GridView1.Columns[26].Visible = true;

            }
            else if (rdoStone.Checked)
            {
                lblInvSearch.Visible = false;
                txtInvSearch.Visible = false;
                GridView1.Columns[9].Visible = true;
                GridView1.Columns[10].Visible = true;
                GridView1.Columns[11].Visible = false;
                GridView1.Columns[12].Visible = false;
                GridView1.Columns[13].Visible = false;
                GridView1.Columns[14].Visible = false;
                GridView1.Columns[15].Visible = false;
                GridView1.Columns[16].Visible = false;
                GridView1.Columns[17].Visible = false;
                GridView1.Columns[18].Visible = false;
                GridView1.Columns[19].Visible = false;
                GridView1.Columns[20].Visible = false;
                GridView1.Columns[21].Visible = true;
                GridView1.Columns[22].Visible = true;
                GridView1.Columns[23].Visible = true;
                GridView1.Columns[26].Visible = true;

            }

            /** ปิด เอาไว้เพราะว่าไม่ได้ใช้งาน 01/02/2026 ***/
            //else if (rdoSheetTube.Checked)
            //{
            //    lblInvSearch.Visible = false;
            //    txtInvSearch.Visible = false;
            //    GridView1.Columns[9].Visible = false;
            //    GridView1.Columns[10].Visible = false;
            //    GridView1.Columns[11].Visible = false;
            //    GridView1.Columns[12].Visible = false;
            //    GridView1.Columns[13].Visible = false;
            //    GridView1.Columns[14].Visible = false;
            //    GridView1.Columns[15].Visible = false;
            //    GridView1.Columns[16].Visible = false;
            //    GridView1.Columns[17].Visible = false;
            //    GridView1.Columns[18].Visible = false;
            //    GridView1.Columns[19].Visible = false;
            //    GridView1.Columns[20].Visible = false;
            //    GridView1.Columns[21].Visible = true;
            //    GridView1.Columns[22].Visible = true;
            //    GridView1.Columns[23].Visible = false;
            //}
        }
    }

}