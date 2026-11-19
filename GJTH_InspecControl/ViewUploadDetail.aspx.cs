using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Net.Mail;

namespace GJTH_ISPC
{
    public partial class ViewUploadDetail : System.Web.UI.Page
    {
        string fileUpload;
        string templateType;
        ConnectionDB db = new ConnectionDB();
        string strUserId = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split('\\')[1];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Label lblHd = (Label)Page.Master.FindControl("lblModule");
                lblHd.Text = "(View Upload File)";
            }

            fileUpload = System.Web.HttpUtility.UrlDecode(Request.QueryString["fileUpload"]);
            templateType = Request.QueryString["templateType"];

            btnConfirm.Click += new EventHandler(btnConfirm_Click);

            this.readExcel();

            btnUpload.Enabled = grvData.Rows.Count > 0 ? true : false;

            lblError.Visible = grvData.Rows.Count > 0 ? false : true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (templateType == "STONE")
            {
                SaveStoneData();
                return;
            }

            SaveDiamondData();


        }
        private void SaveDiamondData()
        {
            try
            {
                string strsql = "";
                string PO = "";
                string ItemID = "";
                string Supplier = "";
                string MemoID = "";
                if (grvData.Rows.Count > 0)
                {
                     MemoID = Server.HtmlDecode(grvData.Rows[0].Cells[4].Text)
                   .Replace("\u00A0", "")
                   .Trim();

                    Supplier = Server.HtmlDecode(grvData.Rows[0].Cells[5].Text)
                                       .Replace("\u00A0", "")
                                       .Trim();
                    PO = Server.HtmlDecode(grvData.Rows[0].Cells[1].Text)
                                       .Replace("\u00A0", "")
                                       .Trim();
                }
                //** เช็คเงื่อนไข ต่างๆ ก่อนทำการ Insert into Data**//
                if (string.IsNullOrWhiteSpace(MemoID))
                {
                    ShowModalDialog(
                        "Warning",
                        "MemoID cannot be blank. Please check the uploaded file.",
                        "OK",
                        false
                    );

                    return;
                }
                if (string.IsNullOrWhiteSpace(PO) && string.IsNullOrWhiteSpace(Supplier) )
                {
                    ShowModalDialog(
                        "Warning",
                        "Supplier Code cannot be blank. Please check the uploaded file.",
                        "OK",
                        false
                    );

                    return;
                }

                string sql = @"
                        SELECT TOP 1 MemoID
                        FROM [AX-EXT].dbo. ISPC_ItemInspect
                        WHERE MemoID = '" + MemoID.Replace("'", "''") + "'";

                DataTable dtChk = db.QueryToDataTable(sql);

                if (dtChk.Rows.Count > 0)
                {

                    ShowModalDialog(
                         "Warning",
                         "❌ MemoID : " + MemoID + " already exists in the system. File upload is not allowed.",
                         "OK",
                         false,
                         "UploadFile.aspx"
                     );

                    return;
                }

                foreach (GridViewRow gr in grvData.Rows)
                {
                    string itemId = gr.Cells[2].Text.Trim();

                    if (itemId == "Qty.To Receive")
                        continue;


                    PO = Server.HtmlDecode(gr.Cells[1].Text).Replace("\u00A0", "").Trim();

                    ItemID = Server.HtmlDecode(gr.Cells[2].Text).Replace("\u00A0", "").Trim();
                    
                    string PurchLineNo = "";
                    string ItemName = "";
                    

                    PO = "";
                    ItemID = "";
                    MemoID = "";
                    var QTYRecv = "";
                    var QTYGood = "";
                    var QTYScrap = "";
                    var GoodButOverPOQty = "";
                    var NotInspecQty = "";
                    var YellowQty = "";
                    var TooLightQty = "";
                    var CrackQty = "";
                    var BadCutQty = "";
                    var TooBigQty = "";
                    var TooSmallQty = "";
                    var Zoning = "";
                    var Clearity = "";
                    var Other = "";
                    var MissingQty = "";
                    var OverQty = "";
                    var ReturnQty = "";
                    var Remark = "";
                    var Reason = "";
                    var ShelfNo = "";
                    var InspecType = "0";
                    var CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    for (int j = 0; j < gr.Cells.Count; j++)
                    {
                        if (gr.Cells[j].Text != "&nbsp;")
                        {


                            switch (j)
                            {


                                case 1:
                                    PO = gr.Cells[j].Text;
                                    break;
                                case 2:
                                    ItemID = gr.Cells[j].Text;
                                    break;
                                case 4:
                                    MemoID = gr.Cells[j].Text;
                                    break;
                                case 5:
                                    Supplier = gr.Cells[j].Text;
                                    break;
                                case 6:
                                    QTYRecv = gr.Cells[j].Text;
                                    break;
                                case 7:
                                    QTYGood = gr.Cells[j].Text;
                                    break;
                                case 8:
                                    QTYScrap = gr.Cells[j].Text;
                                    break;
                                case 9:
                                    GoodButOverPOQty = gr.Cells[j].Text;
                                    break;
                                case 10:
                                    NotInspecQty = gr.Cells[j].Text;
                                    break;
                                case 11:
                                    YellowQty = gr.Cells[j].Text;
                                    break;
                                case 12:
                                    TooLightQty = gr.Cells[j].Text;
                                    break;
                                case 13:
                                    CrackQty = gr.Cells[j].Text;
                                    break;
                                case 14:
                                    BadCutQty = gr.Cells[j].Text;
                                    break;
                                case 15:
                                    TooBigQty = gr.Cells[j].Text;
                                    break;
                                case 16:
                                    TooSmallQty = gr.Cells[j].Text;
                                    break;
                                case 17:
                                    Zoning = gr.Cells[j].Text;
                                    break;
                                case 18:
                                    Clearity = gr.Cells[j].Text;
                                    break;
                                case 19:
                                    Other = gr.Cells[j].Text;
                                    break;
                                case 20:
                                    MissingQty = gr.Cells[j].Text;
                                    break;
                                case 21:
                                    OverQty = gr.Cells[j].Text;
                                    break;
                                case 22:
                                    ReturnQty = gr.Cells[j].Text;
                                    break;
                                case 23:
                                    Remark = gr.Cells[j].Text;
                                    break;
                                case 24:
                                    Reason = gr.Cells[j].Text;
                                    break;
                                case 25:
                                    ShelfNo = gr.Cells[j].Text;
                                    break;


                            }
                        }
                    }

                    if (PO != "")
                    {
                        string sqlP = @"
                        SELECT TOP 1 LINENUMBER,NAME,VENDACCOUNT
                        FROM PURCHLINE
                        WHERE PURCHID='" + PO.Replace("'", "''") + @"'
                        AND ITEMID='" + ItemID.Replace("'", "''") + "'";

                        DataTable dtP = db.QueryToDataTable(sqlP);

                        if (dtP.Rows.Count > 0)
                        {
                            PurchLineNo = dtP.Rows[0]["LINENUMBER"].ToString();
                            ItemName = dtP.Rows[0]["NAME"].ToString();
                            Supplier = dtP.Rows[0]["VENDACCOUNT"].ToString();
                        }


                    }
                    else
                    {
                        string sqlP = @"SELECT top 1 ITEMID, NAME,0 AS PurchLineNo FROM _V_ItemDesc
                        WHERE ITEMID='" + ItemID.Replace("'", "''") + "'";

                        DataTable dtP = db.QueryToDataTable(sqlP);

                        if (dtP.Rows.Count > 0)
                        {
                            PurchLineNo = dtP.Rows[0]["PurchLineNo"].ToString();
                            ItemName = dtP.Rows[0]["NAME"].ToString();
                        }


                    }

                    //---Chkeck Data Decimal

                    decimal dQTYRecv = QTYRecv == "" ? 0 : Convert.ToDecimal(QTYRecv);
                    decimal dQTYGood = QTYGood == "" ? 0 : Convert.ToDecimal(QTYGood);
                    decimal dQTYScrap = QTYScrap == "" ? 0 : Convert.ToDecimal(QTYScrap);
                    decimal dGoodButOverPOQty = GoodButOverPOQty == "" ? 0 : Convert.ToDecimal(GoodButOverPOQty);
                    decimal dNotInspecQty = NotInspecQty == "" ? 0 : Convert.ToDecimal(NotInspecQty);
                    decimal dYellowQty = YellowQty == "" ? 0 : Convert.ToDecimal(YellowQty);
                    decimal dTooLightQty = TooLightQty == "" ? 0 : Convert.ToDecimal(TooLightQty);
                    decimal dCrackQty = CrackQty == "" ? 0 : Convert.ToDecimal(CrackQty);
                    decimal dBadCutQty = BadCutQty == "" ? 0 : Convert.ToDecimal(BadCutQty);
                    decimal dTooBigQty = TooBigQty == "" ? 0 : Convert.ToDecimal(TooBigQty);
                    decimal dTooSmallQty = TooSmallQty == "" ? 0 : Convert.ToDecimal(TooSmallQty);
                    decimal dZoning = Zoning == "" ? 0 : Convert.ToDecimal(Zoning);
                    decimal dClearity = Clearity == "" ? 0 : Convert.ToDecimal(Clearity);
                    decimal dOther = Other == "" ? 0 : Convert.ToDecimal(Other);
                    decimal dMissingQty = MissingQty == "" ? 0 : Convert.ToDecimal(MissingQty);
                    decimal dOverQty = OverQty == "" ? 0 : Convert.ToDecimal(OverQty);
                    decimal dReturnQty = ReturnQty == "" ? 0 : Convert.ToDecimal(ReturnQty);
                    decimal dShelfNo = ShelfNo == "" ? 0 : Convert.ToDecimal(ShelfNo);
                    int sInspecType = InspecType == "" ? 0 : Convert.ToInt32(InspecType);

                    //---Keep Data from AXDB

                    strsql = "insert into [AX-EXT].dbo.ISPC_ItemInspect (PO,ItemID,ItemName,MemoID,QTYRecv,QTYGood,QTYScrap,GoodButOverPOQty,NotInspecQty,YellowQty,TooLightQty,CrackQty,BadCutQty,TooBigQty,TooSmallQty,Zoning,Clearity,Other,MissingQty,OverQty,ReturnQty,Remark,Reason,PurchLineNo,ShelfNo,CreateDate,InspecType,Createby,SupplierCode,IsActive )values(";
                    strsql += "'" + PO + "',";
                    strsql += "'" + ItemID + "',";
                    strsql += "'" + ItemName + "',";
                    strsql += "'" + MemoID + "',";
                    strsql += "'" + dQTYRecv + "',";
                    strsql += "'" + dQTYGood + "',";
                    strsql += "'" + dQTYScrap + "',";
                    strsql += "'" + dGoodButOverPOQty + "',";
                    strsql += "'" + dNotInspecQty + "',";
                    strsql += "'" + dYellowQty + "',";
                    strsql += "'" + dTooLightQty + "',";
                    strsql += "'" + dCrackQty + "',";
                    strsql += "'" + dBadCutQty + "',";
                    strsql += "'" + dTooBigQty + "',";
                    strsql += "'" + dTooSmallQty + "',";
                    strsql += "'" + dZoning + "',";
                    strsql += "'" + dClearity + "',";
                    strsql += "'" + dOther + "',";
                    strsql += "'" + dMissingQty + "',";
                    strsql += "'" + dOverQty + "',";
                    strsql += "'" + dReturnQty + "',";
                    strsql += "'" + Remark + "',";
                    strsql += "'" + Reason + "',";
                    strsql += "'" + PurchLineNo + "',";
                    strsql += "'" + dShelfNo + "',";
                    strsql += "'" + CreateDate + "',";
                    strsql += "'" + sInspecType + "',";
                    strsql += "'" + strUserId + "',";
                    strsql += "'" + Supplier + "',";
                    strsql += "'1'";
                    strsql += ")";

                    db.ExecuteDBExt(strsql);

                }

                ShowModalDialog("Success", "Data has been imported successfully.", "OK", false, "InspecList.aspx");


            }
            catch
            {
                ShowModalDialog(
                        "Error",
                        "An error occurred while importing the data. Please contact the system administrator.",
                        "OK",
                        false
                    );
            }
        }

        private void SaveStoneData()
        {
            try
            {
                string PO = "";
                string MemoID = "";
                string Supplier = "";
                if (grvData.Rows.Count > 0)
                {
                    PO = Server.HtmlDecode(grvData.Rows[0].Cells[1].Text)
                                       .Replace("\u00A0", "")
                                       .Trim();
                    MemoID = Server.HtmlDecode(grvData.Rows[0].Cells[4].Text)
                                    .Replace("\u00A0", "")
                                    .Trim();

                    Supplier = Server.HtmlDecode(grvData.Rows[0].Cells[5].Text)
                                    .Replace("\u00A0", "")
                                    .Trim();
                    
                }
                if (string.IsNullOrWhiteSpace(MemoID))
                {
                    ShowModalDialog(
                        "Warning",
                        "MemoID cannot be blank. Please check the uploaded file.",
                        "OK",
                        false
                    );

                    return;
                }
                if (string.IsNullOrWhiteSpace(PO) && string.IsNullOrWhiteSpace(Supplier))
                {
                    ShowModalDialog(
                        "Warning",
                        "Supplier Code cannot be blank. Please check the uploaded file.",
                        "OK",
                        false
                    );

                    return;
                }
                string sqlChk =
                    @"SELECT TOP 1 MemoID
                      FROM [AX-EXT].dbo.ISPC_ItemInspect
                      WHERE MemoID='" + MemoID.Replace("'", "''") + "'";

                DataTable dtChk = db.QueryToDataTable(sqlChk);

                if (dtChk.Rows.Count > 0)
                {
                    ShowModalDialog(
                        "Warning",
                        "❌ MemoID : " + MemoID + " already exists in the system.",
                        "OK",
                        false,
                        "UploadFile.aspx"
                    );
                    return;
                }

                foreach (GridViewRow gr in grvData.Rows)
                {
                    string itemId = gr.Cells[2].Text.Trim();

                    if (itemId == "Qty.To Receive")
                        continue;

                     PO =
                        Server.HtmlDecode(gr.Cells[1].Text)
                        .Replace("\u00A0", "")
                        .Trim();


                    string ItemID =
                        Server.HtmlDecode(gr.Cells[2].Text)
                        .Replace("\u00A0", "")
                        .Trim();

                    string Memo =
                     Server.HtmlDecode(gr.Cells[4].Text)
                     .Replace("\u00A0", "")
                     .Trim();

                    string SupplierCode =
                      Server.HtmlDecode(gr.Cells[5].Text)
                     .Replace("\u00A0", "")
                     .Trim();
                    decimal QTYRecv = 0;
                    decimal.TryParse(gr.Cells[6].Text.Replace("&nbsp;", "").Trim(), out QTYRecv);

                    decimal QTYGood = 0;
                    decimal.TryParse(gr.Cells[7].Text.Replace("&nbsp;", "").Trim(), out QTYGood);

                    decimal QTYScrap = 0;
                    decimal.TryParse(gr.Cells[8].Text.Replace("&nbsp;", "").Trim(), out QTYScrap);

                    decimal GoodButOverPOQty = 0;
                    decimal.TryParse(gr.Cells[9].Text.Replace("&nbsp;", "").Trim(), out GoodButOverPOQty);

                    decimal NotInspecQty = 0;
                    decimal.TryParse(gr.Cells[10].Text.Replace("&nbsp;", "").Trim(), out NotInspecQty);

                    decimal MissingQty = 0;
                    decimal.TryParse(gr.Cells[11].Text.Replace("&nbsp;", "").Trim(), out MissingQty);

                    decimal OverQty = 0;
                    decimal.TryParse(gr.Cells[12].Text.Replace("&nbsp;", "").Trim(), out OverQty);



                    string Reason =
                        Server.HtmlDecode(gr.Cells[13].Text)
                        .Replace("\u00A0", "")
                        .Trim();

                    decimal ReturnQty = 0;
                    decimal.TryParse(gr.Cells[14].Text.Replace("&nbsp;", "").Trim(), out ReturnQty);
                   
                    string PurchLineNo = "";
                    string ItemName = "";
                    if (PO != "")
                    {
                        string sqlP = @"
                        SELECT TOP 1 LINENUMBER,NAME,VENDACCOUNT
                        FROM PURCHLINE
                        WHERE PURCHID='" + PO.Replace("'", "''") + @"'
                        AND ITEMID='" + ItemID.Replace("'", "''") + "'";

                        DataTable dtP = db.QueryToDataTable(sqlP);

                        if (dtP.Rows.Count > 0)
                        {
                            PurchLineNo = dtP.Rows[0]["LINENUMBER"].ToString();
                            ItemName = dtP.Rows[0]["NAME"].ToString();
                            SupplierCode = dtP.Rows[0]["VENDACCOUNT"].ToString();
                        }


                    }
                    else
                    {
                        string sqlP = @"SELECT top 1 ITEMID, NAME,0 AS PurchLineNo FROM _V_ItemDesc
                        WHERE ITEMID='" + ItemID.Replace("'", "''") + "'";

                        DataTable dtP = db.QueryToDataTable(sqlP);

                        if (dtP.Rows.Count > 0)
                        {
                            PurchLineNo = dtP.Rows[0]["PurchLineNo"].ToString();
                            ItemName = dtP.Rows[0]["NAME"].ToString();
                        }


                    }

                    string strsql = @"
                    INSERT INTO [AX-EXT].dbo.ISPC_ItemInspect
                    (
                        PO, ItemID,ItemName,MemoID,SupplierCode,QTYRecv,QTYGood,QTYScrap, GoodButOverPOQty,
                        NotInspecQty,MissingQty,OverQty,ReturnQty,Reason,PurchLineNo,
                        ShelfNo,CreateDate,InspecType,Createby,IsActive
                    )
                    VALUES
                    (
                        '" + PO + @"',
                        '" + ItemID + @"',
                        '" + ItemName + @"',
                        '" + Memo + @"',
                        '" + SupplierCode + @"',
                        '" + QTYRecv + @"',
                        '" + QTYGood + @"',
                        '" + QTYScrap + @"',
                        '" + GoodButOverPOQty + @"',
                        '" + NotInspecQty + @"',
                        '" + MissingQty + @"',
                        '" + OverQty + @"',
                        '" + ReturnQty + @"',
                        '" + Reason + @"',
                        '" + PurchLineNo + @"',
                        '0',
                        GETDATE(),
                        '1',
                        '" + strUserId + @"',
                        '1'
                    )";

                    db.ExecuteDBExt(strsql);
                }

                ShowModalDialog(
                    "Success",
                    "Stone Template Import Success",
                    "OK",
                    false,
                    "InspecList.aspx"
                );
            }
            catch (Exception ex)
            {
                ShowModalDialog(
                    "Error",
                    ex.Message,
                    "OK",
                    false
                );
            }
        }

        public void ShowModalDialog(string strHeader, string strDetail, string strButton, bool bIsWaring, string redirectUrl = "")
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<script type='text/javascript'>");

            sb.Append("$('#textHeader').text('" + strHeader + "');");
            
            if (strHeader == "Warning")
            {
                sb.Append("$('#textHeader').css({'color':'#ff8c00','font-weight':'bold'});");
            }
            else if (strHeader == "Success")
            {
                sb.Append("$('#textHeader').css({'color':'#28a745','font-weight':'bold'});");
            }
            else if (strHeader == "Error")
            {
                sb.Append("$('#textHeader').css({'color':'#dc3545','font-weight':'bold'});");
            }
            else if (strHeader == "Delete")
            {
                sb.Append("$('#textHeader').css({'color':'#b02a37','font-weight':'bold'});");
            }
            else
            {
                sb.Append("$('#textHeader').css({'color':'#0dcaf0','font-weight':'normal'});");
            }
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
                sb.Append("$('#btnCancel2').hide();");
            }

            sb.Append("$('#mdtrndetail').modal('show');");
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                sb.Append("$('#btnSuccess').attr('onclick',\"window.location.href='" + ResolveUrl(redirectUrl) + "';\");");
            }
            sb.Append("</script>");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalScript", sb.ToString(), false);

        }


        private void readExcel()
        {
            string strUserName = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.Split('\\')[1];

            string filePath = Server.MapPath("~/App_Data/tempfile-" + strUserName + ".xlsx");

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    string ext = Path.GetExtension(fileUpload);

                    if (ext == ".xlsx" || ext == ".xls")
                    {
                        ExcelPackage package = new ExcelPackage(fs);

                        // อ่าน Excel เป็น DataTable
                        DataTable dt = package.ToDataTable();

                        // เพิ่ม Column NAME
                        if (!dt.Columns.Contains("NAME"))
                        {
                            DataColumn colName = dt.Columns.Add("NAME", typeof(string));

                            // ย้าย NAME ไปอยู่หลัง ItemID
                            colName.SetOrdinal(dt.Columns["ItemID"].Ordinal + 1);
                        }

                        //------------------------------------------------
                        // ดึงข้อมูล ITEMID + NAME จาก _V_ItemDesc
                        //------------------------------------------------
                        string sqlP = @"SELECT ITEMID,MAX(NAME) AS NAME FROM _V_ItemDesc GROUP BY ITEMID";

                        DataTable dtPurch = db.QueryToDataTable(sqlP);

                        //------------------------------------------------
                        // สร้าง Dictionary
                        //------------------------------------------------
                        Dictionary<string, string> itemNames =
                            dtPurch.AsEnumerable()
                                   .GroupBy(r => r["ITEMID"].ToString().Trim())
                                   .ToDictionary(
                                        g => g.Key,
                                        g => g.First()["NAME"].ToString()
                                   );

                        //------------------------------------------------
                        // เติม NAME กลับเข้า DataTable
                        //------------------------------------------------
                        foreach (DataRow dr in dt.Rows)
                        {
                            string itemId = dr["ItemID"].ToString().Trim();

                            if (itemNames.ContainsKey(itemId))
                            {
                                dr["NAME"] = itemNames[itemId];
                            }
                            else
                            {
                                dr["NAME"] = "";
                            }
                        }

                        //------------------------------------------------
                        // SUM QTY Recv
                        //------------------------------------------------
                        decimal totalQtyRecv = 0;

                        foreach (DataRow dr in dt.Rows)
                        {
                            decimal qtyRecv;
                            decimal.TryParse(
                                dr["QTYRecv"].ToString(),
                                out qtyRecv);

                            totalQtyRecv += qtyRecv;
                        }

                        // เพิ่มแถว Total QTY Recv
                        DataRow totalRow = dt.NewRow();

                        totalRow["ItemID"] = "Qty.To Receive";
                        totalRow["QTYRecv"] = totalQtyRecv.ToString("N2");

                        dt.Rows.Add(totalRow);

                        //------------------------------------------------
                        // Bind GridView
                        //------------------------------------------------
                        grvData.DataSource = dt;
                        grvData.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        "readExcel Error : " + ex.Message);
                }
            }
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/UploadFile.aspx");
        }

        protected void grvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (drv["ItemID"].ToString() == "Qty.To Receive")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFF2CC");

                    e.Row.Font.Bold = true;
                    e.Row.Cells[0].Font.Size = FontUnit.Point(11);
                    // Merge ตั้งแต่ No ถึง MemoID
                    e.Row.Cells[0].Text = "Qty.To Receive";
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.DarkBlue;
                    e.Row.Cells[0].ColumnSpan = 6;

                    for (int i = 1; i <= 5; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                    // QTYRecv สีแดง
                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[6].Font.Bold = true;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }
    }
    public static class ExcelPackageExtensions
    {

        public static DataTable ToDataTable(this ExcelPackage package)
        {

            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

            DataTable table = new DataTable();

            table.Columns.Add("No");

            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }


            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];

                var newRow = table.NewRow();


                newRow[0] = rowNumber - 1;

                foreach (var cell in row)
                {
                    newRow[cell.Start.Column] = cell.Text;
                }

                table.Rows.Add(newRow);

            }
            return table;
        }
    }
}