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

            //Label1.Text = "File : " + fileUpload;

            btnConfirm.Click += new EventHandler(btnConfirm_Click);

            this.readExcel();

            btnUpload.Enabled = grvData.Rows.Count > 0 ? true : false;

            lblError.Visible = grvData.Rows.Count > 0 ? false : true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string strsql = "";
                string PO = "";
                string ItemID = "";

                PO =  Server.HtmlDecode(grvData.Rows[0].Cells[0].Text).Replace("\u00A0", "").Trim();
                string MemoID = Server.HtmlDecode(grvData.Rows[0].Cells[2].Text)
                   .Replace("\u00A0", "")
                   .Trim();

                if (string.IsNullOrWhiteSpace(PO) || string.IsNullOrWhiteSpace(MemoID))
                {
                    ShowModalDialog(
                        "Warning",
                        "PO And MemoID cannot be blank. Please check the uploaded file.",
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
                     PO = Server.HtmlDecode(gr.Cells[0].Text).Replace("\u00A0", "").Trim();

                     ItemID = Server.HtmlDecode(gr.Cells[1].Text) .Replace("\u00A0", "").Trim();

                    string sqlP = @"
                        SELECT TOP 1 PURCHID, ITEMID, LINENUMBER,NAME
                        FROM PURCHLINE
                        WHERE PURCHID = '" + PO.Replace("'", "''") + @"'
                        AND ITEMID = '" + ItemID.Replace("'", "''") + "'";

                    DataTable dtChkP = db.QueryToDataTable(sqlP);

                    string PurchLineNo = "";
                    string ItemName = "";

                    if (dtChkP.Rows.Count > 0)
                    {
                        PurchLineNo = dtChkP.Rows[0]["LINENUMBER"].ToString();
                        ItemName = dtChkP.Rows[0]["NAME"].ToString();
                    }

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
                    var InspecType = "";
                    var CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    for (int j = 0; j < grvData.Columns.Count; j++)
                    {
                        if (gr.Cells[j].Text != "&nbsp;")
                        {


                            switch (j)
                            {


                                case 0:
                                    PO = gr.Cells[j].Text;
                                    break;
                                case 1:
                                    ItemID = gr.Cells[j].Text;
                                    break;
                                //case 2:
                                //    ItemName = gr.Cells[j].Text;
                                //    break;
                                case 2:
                                    MemoID = gr.Cells[j].Text;
                                    break;
                                case 3:
                                    QTYRecv = gr.Cells[j].Text;
                                    break;
                                case 4:
                                    QTYGood = gr.Cells[j].Text;
                                    break;
                                case 5:
                                    QTYScrap = gr.Cells[j].Text;
                                    break;
                                case 6:
                                    GoodButOverPOQty = gr.Cells[j].Text;
                                    break;
                                case 7:
                                    NotInspecQty = gr.Cells[j].Text;
                                    break;
                                case 8:
                                    YellowQty = gr.Cells[j].Text;
                                    break;
                                case 9:
                                    TooLightQty = gr.Cells[j].Text;
                                    break;
                                case 10:
                                    CrackQty = gr.Cells[j].Text;
                                    break;
                                case 11:
                                    BadCutQty = gr.Cells[j].Text;
                                    break;
                                case 12:
                                    TooBigQty = gr.Cells[j].Text;
                                    break;
                                case 13:
                                    TooSmallQty = gr.Cells[j].Text;
                                    break;
                                case 14:
                                    Zoning = gr.Cells[j].Text;
                                    break;
                                case 15:
                                    Clearity = gr.Cells[j].Text;
                                    break;
                                case 16:
                                    Other = gr.Cells[j].Text;
                                    break;
                                case 17:
                                    MissingQty = gr.Cells[j].Text;
                                    break;
                                case 18:
                                    OverQty = gr.Cells[j].Text;
                                    break;
                                case 19:
                                    ReturnQty = gr.Cells[j].Text;
                                    break;
                                case 20:
                                    Remark = gr.Cells[j].Text;
                                    break;
                                case 21:
                                    Reason = gr.Cells[j].Text;
                                    break;
                                case 22:
                                    ShelfNo = gr.Cells[j].Text;
                                    break;
                                case 23:
                                    InspecType = gr.Cells[j].Text;
                                    break;

                            }
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

                    strsql = "insert into [AX-EXT].dbo.ISPC_ItemInspect (PO,ItemID,ItemName,MemoID,QTYRecv,QTYGood,QTYScrap,GoodButOverPOQty,NotInspecQty,YellowQty,TooLightQty,CrackQty,BadCutQty,TooBigQty,TooSmallQty,Zoning,Clearity,Other,MissingQty,OverQty,ReturnQty,Remark,Reason,PurchLineNo,ShelfNo,CreateDate,InspecType,IsActive )values(";
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

        public void ShowModalDialog(string strHeader, string strDetail, string strButton, bool bIsWaring, string redirectUrl = "")
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
            FileStream fs = new FileStream(Server.MapPath("~/App_Data/tempfile-" + strUserName + ".xlsx"), FileMode.Open, FileAccess.Read);
            try
            {


                if (Path.GetExtension(fileUpload) == ".xlsx" || Path.GetExtension(fileUpload) == ".xls")
                {

                    ExcelPackage package = new ExcelPackage(fs);

                    grvData.DataSource = package.ToDataTable();

                    grvData.DataBind();
                }


            }
            catch (Exception readExp)
            {

                //Label1.Text = readExp.Message + "\n" + "fileUpload " + fileUpload;

                System.Diagnostics.Debug.WriteLine("Tag UnRead Excel : " + readExp.Message);

            }
            fs.Close();
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/UploadFile.aspx");
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

                //var curLoad = System.DateTime.Now;

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