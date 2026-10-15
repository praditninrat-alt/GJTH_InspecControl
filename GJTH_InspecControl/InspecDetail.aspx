<%@ Page Title="" Language="C#" MasterPageFile="~/PageMaster.Master" AutoEventWireup="true" CodeBehind="InspecDetail.aspx.cs" Inherits="GJTH_ISPC.InspecDetail" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script type="text/javascript">

        ///** เพิ่ม document 02/02/2026 ***//
        $(document).ready(function () {

            $("#btnSuccess").click(function () {
                window.location.href = "InspecList.aspx";
            });

        });

        function DeleteTrans(id, itemid) {


            $("#deletransferid").text(itemid);

            $("#<%= HdeleteId.ClientID %>").val(id);

            $("#mddelete").modal("show");

        }


        function isNumber(evt) {

            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46) {
                return false;
            }
            else {
                // If the number field already has . then don't allow to enter . again.
                if (evt.target.value.search(/\./) > -1 && charCode == 46) {
                    return false;
                }
                return true;
            }

        }



    </script>




    <style type="text/css">
        TH {
            text-align: center;
        }

        .dtxtLeft {
            margin-left: -180px;
        }

        .itxtleft {
            margin-left: -20px;
        }

        #Tenav a:hover {
            color: #00FFFF
        }


        .StickyHeader th {
            position: sticky;
            top: 0;
            background-color: #d5defe;
        }
    </style>


    <div class="row" style="margin-top: 0px; margin-left: 0px;">


        <div class="col-lg-4 col-md-4  col-sm-4 text-left">
            <div class="row" style="margin-top: 15px; margin-left: 0px; text-align: left;">
                <div class="col-lg-12 col-md-12  col-sm-12 text-left">
                    <asp:RadioButton ID="rdoDiamond" runat="server" Text="Diamond" TextAlign="Right" GroupName="rdochk" Checked="true" OnCheckedChanged="rdoChange" AutoPostBack="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoStone" runat="server" Text="Stone" TextAlign="Right" GroupName="rdochk" OnCheckedChanged="rdoChange" AutoPostBack="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       <%-- <asp:RadioButton ID="rdoSheetTube" runat="server" Text="Sheet/Tube" TextAlign="Right"  GroupName="rdochk"  OnCheckedChanged="rdoChange" AutoPostBack="true"/>--%>
                </div>



            </div>
        </div>

        <div class="col-lg-8 col-md-8  col-sm-8 text-center">
            <div class="row" style="margin-top: 10px; margin-left: 5px;">


                <div class="col-lg-2 col-md-2  col-sm-2 text-right">
                    <asp:Label ID="Label5" runat="server" Text="PO :"></asp:Label>
                </div>

                <div class="col-lg-2 text-left">
                    <asp:TextBox ID="txtPO" CssClass="form-control" runat="server" autocomplete="off" required="required"></asp:TextBox>
                </div>
                <div class="col-lg-2 col-md-2  col-sm-2 text-right">
                    <asp:Label ID="lblInvSearch" runat="server" Text="Invoice/Memo :"></asp:Label>
                </div>

                <div class="col-lg-2 text-left">
                    <asp:TextBox ID="txtInvSearch" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>

                </div>
                <div class="col-lg-2 text-right">
                    <asp:Button ID="btnSearch" runat="server" Text="🔍 Search" CssClass="btn btn-info" Width="100%" OnClick="btnSearch_Click" />
                </div>
                <div class="col-lg-2 col-md-2  col-sm-2 text-left">
                    <asp:Button ID="btnClear" runat="server" Text="🧹 Clear" CssClass="btn btn-warning" Width="100%" OnClick="btnClear_Click" CausesValidation="False" UseSubmitBehavior="False" />
                </div>

            </div>
        </div>


    </div>


    <hr style="border: 0px; border-bottom: 1px solid #000;" />
    <div>
        <div style="text-align: center; margin-top: 20px;">
            <asp:Label ID="lblMessage" runat="server" ForeColor="#dc3545" Font-Bold="true" Font-Size="Large"> </asp:Label>
        </div>
    </div>

    <div class="row" style="margin-top: 10px; margin-left: 5px;">


        <div class="col-lg-2 col-md-2  col-sm-2 text-right">
            <asp:Label ID="lblInvData" runat="server" Text="Invoice/Memo :"></asp:Label>
        </div>

        <div class="col-lg-2 text-left">
            <asp:TextBox ID="txtInvData" CssClass="form-control" runat="server" autocomplete="off" required="required"></asp:TextBox>
        </div>
        <div class="col-lg-2 col-md-2  col-sm-2 text-right">
        </div>

        <div class="col-lg-2 text-left">
        </div>
        <div class="col-lg-2 text-right">
        </div>
        <div class="col-lg-2 col-md-2  col-sm-2 text-left">
        </div>

    </div>

    <div class="row" style="margin-top: 10px; margin-left: 5px; padding: 0px 0px 0px 0px;">

        <asp:GridView CssClass="table  table-hover table-bordered "
            ID="GridView1"
            runat="server"
            AutoGenerateColumns="False"
            OnRowDataBound="GridView1_RowDataBound"
            OnPageIndexChanging="GridView1_PageIndexChanging"
            PageSize="100"
            AllowPaging="true"
            ShowHeaderWhenEmpty="true" Datakeys="RowSeq">
            <HeaderStyle Font-Bold="true" CssClass="StickyHeader" />
            <PagerStyle CssClass="cssPager" ForeColor="Black" HorizontalAlign="Right" />
            <PagerSettings PageButtonCount="10" FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />

            <Columns>


                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRowID" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblRowSeq" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="PO" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblPO" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Memo/Invoice" ItemStyle-HorizontalAlign="Center" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblMemo" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="ItemID" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblItemID" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="ItemName" ItemStyle-HorizontalAlign="left">
                    <ItemTemplate>
                        <asp:Label ID="lblItemName" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="QTY Recv" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <%-- <asp:Label ID="lblRec" runat="server" ></asp:Label>--%>
                        <asp:TextBox ID="txtRec" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>

                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="QTY Good" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblGood" runat="server" ></asp:Label>--%>
                        <asp:TextBox ID="txtGood" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="QTY Scrap" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <%--  <asp:Label ID="lblScrap" runat="server" ></asp:Label>--%>
                        <asp:TextBox ID="txtScrap" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="GoodBut OverPO" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtGoodButOverPO" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="NotInspct" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtNotInspc" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Yellow" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtYellow" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Light" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtLight" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Crack" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCrack" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="BadCut" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtBadCut" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Big" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtBig" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Small" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSmall" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Zoning" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtZoning" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Clearity" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtClearity" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Other" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtOther" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Remark" ItemStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRemark" runat="server" autocomplete="off" Width="100%" Style="text-align: right" TextMode="MultiLine"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="QTY Missing" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtMissingQty" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="QTY Over" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtOverQty" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Reason" ItemStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <asp:TextBox ID="txtReason" runat="server" autocomplete="off" Style="text-align: left" Width="100%" TextMode="MultiLine"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="ShelfNo" ItemStyle-HorizontalAlign="center" Visible="false">
                    <ItemTemplate>
                        <asp:TextBox ID="txtShelfNo" runat="server" autocomplete="off" Style="text-align: left" Width="100%"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Line" ItemStyle-HorizontalAlign="center" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPurchLine" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <%--เพิ่ม Field-- Return QTY-- 01/02/2026--%>
                <asp:TemplateField HeaderText="Return QTY" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:TextBox ID="txtReturnQTY" runat="server" autocomplete="off" Width="100%" Style="text-align: right" onkeypress="return isNumber(event)"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <div style="padding: 5px 0 0 0;">
                            <a class="edlink" href="javascript:void(0)" onclick="DeleteTrans('<%# Eval("RowSeq") %>','<%# Eval("ITEMID") %>')" title="Delete">

                                <asp:Image ID="imgDel" runat="server" ImageUrl="~/imgs/Cancel.png" />
                            </a>

                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>


    <div class="row" style="margin-top: 10px; margin-left: 5px;">


        <div class="col-lg-2 col-md-2  col-sm-2 text-right">
        </div>

        <div class="col-lg-2 text-left">
        </div>
        <div class="col-lg-2 col-md-2  col-sm-2 text-right">
        </div>

        <div class="col-lg-2 text-left">
        </div>
        <div class="col-lg-2 text-right">
        </div>
        <div class="col-lg-2 col-md-2  col-sm-2 text-left">
            <asp:Button ID="btnSave" runat="server" Text="💾 Save New Memo/Invoice" CssClass="btn btn-success" Width="100%" OnClick="btnSave_Click" />
        </div>

    </div>


    <div id="mdtrndetail" class="modal fade bs-example-modal-sm" role="dialog" tabindex="-1">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" id="textHeader">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Drescription</h4>
                </div>
                <div class="text-center">

                    <span id="mdTrnid"></span>

                </div>
                <div class="modal-footer">
                    <button id="btnCancel" class="btn btn-danger" data-dismiss="modal">OK</button>
                    <button id="btnSuccess" class="btn btn-success" data-dismiss="modal">OK</button>


                </div>
            </div>
        </div>
    </div>


    <div id="mddelete" class="modal fade bs-example-modal-sm" role="dialog" tabindex="-1">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Delete </h4>
                </div>
                <div class="text-center">
                    <asp:HiddenField ID="HdeleteId" runat="server" />
                    <asp:HiddenField ID="HJobID" runat="server" />

                    Do You Want To Delete Job ItemID : <span id="deletransferid" />
                </div>
                <div class="modal-footer">
                    <button id="btncanceldelete" class="btn btn-danger" data-dismiss="modal">No</button>
                    <asp:Button ID="btndelete" CssClass="btn btn-success" OnClick="btndelete_Click" runat="server" Text="Yes" />
                </div>
            </div>
        </div>
    </div>


    <asp:HiddenField ID="hRowID" runat="server" />
    <asp:HiddenField ID="hIsUsed" runat="server" />

    <div id="mdPopResponse" class="modal fade bs-example-modal-sm" role="dialog" tabindex="-1" data-backdrop="static">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" id="mdResHead">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Drescription</h4>
                </div>
                <div class="text-center">
                    <span id="mdResDetail"></span>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>


    </span>


</asp:Content>


