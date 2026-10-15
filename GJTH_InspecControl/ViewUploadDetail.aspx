<%@ Page Title="" Language="C#" MasterPageFile="~/PageMaster.Master" AutoEventWireup="true" CodeBehind="ViewUploadDetail.aspx.cs" Inherits="GJTH_ISPC.ViewUploadDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        function dispcomplete() {
            $("#showcomplete").modal('show');
            window.opener.PostBackParentWindow();
            //window.close();
            return false;
        }

        function disperror() {
            $("#showerror").modal('show');
            return false;
        }

        $(document).ready(function () {

            $("#mdconfirm").on("shown.bs.modal", function () {
                $("#btnCancel").focus();
            });

            $("#myModal").on("shown.bs.modal", function () {
                $("#<%= btnConfirm.ClientID %>").prop("disabled", "disabled");
            });

            $("#myModal").on("hide.bs.modal", function () {
                $("#<%= btnConfirm.ClientID %>").removeAttr("disabled");
            });

            $("#<%= btnUpload.ClientID  %>").click(function () {
                $("#mdconfirm").modal("show");
                return false;
            });

            $("#<%= btnConfirm.ClientID  %>").click(function () {

                $("#mdconfirm").modal("hide");

                $("#showprog").modal("show");

                return true;
            });

            $("#btnok").click(function () {
                window.close();
            });


            $("#btnerror").click(function () {
                window.close();
            });

        });






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

        .cssPager td {
            padding-left: 4px;
            padding-right: 4px;
        }

        .cssPager a {
            color: #00FFFF;
            text-decoration: none;
        }

        .cssPager a, .cssPager span {
            color: Red;
            text-decoration: none;
        }

        .cssPager hover {
            color: #AAFF00;
            text-decoration: none;
        }
        /*.cssPager .current { color:Red; text-decoration: none;}*/
        /*.cssPager { color: #885454; text-decoration: none; background-color:#F7F7DE;}*/

        .modal-message-error {
            color: red;
            font-weight: bold;
        }
    </style>


    <div id="table-fixed" class="container-fluid" style="height: 70px;">
        <div class="row" style="margin-top: 12px; margin-left: 5px;">

            <div class="col-lg-2 text-right">
                <asp:Button ID="btnUpload" Text="📊 Import Excel" CssClass="btn btn-danger" runat="server" Width="80%" />
            </div>
            <div class="col-lg-2 text-left">
                <asp:Button ID="btnUp" runat="server" Text="📤 Upload Excel File" CssClass="btn btn-primary" OnClick="btnUp_Click" Width="200px" Height="33px" />
            </div>
            <div class="col-lg-2 text-right">
            </div>
            <div class="col-lg-2 text-left">
            </div>
            <div class="col-lg-2 text-right">
            </div>
            <div class="col-lg-2 text-right">
            </div>

        </div>
    </div>



    <%-- style="margin-top:10px;    CssClass="table table-bordered table-striped" --%>
    <div class="text-center text-info">
        <asp:Label ID="lblError" runat="server" Text="File is error,please recheck excel file again." CssClass="text-left text-danger" Visible="false"></asp:Label>
    </div>

    <%--<div id="GHead" class="table table-striped" >  </div>--%>

    <%--  <div id="Gdetail" style="height:100px; overflow:auto;" >--%>
    <div class="row" style="margin-top: 2px; margin-left: 5px;">

        <%--   <asp:GridView ID="grvData" runat="server" AutoGenerateColumns ="False" 
                HeaderStyle-BackColor="#99CCFF" 
                CssClass="table table-hover table-striped" 
                GridLines="None" CellPadding="4" ForeColor="#333333" > --%>
        <asp:GridView CssClass="table table-striped table-hover table-bordered"
            ID="grvData"
            runat="server"
            AutoGenerateColumns="False"
            PageSize="20">

            <AlternatingRowStyle BackColor="White" />

            <Columns>
                <asp:BoundField HeaderText="PO" DataField="PO" />
                <asp:BoundField HeaderText="ItemID" DataField="ItemID" />
                <asp:BoundField HeaderText="MemoID" DataField="MemoID" />
                <asp:BoundField HeaderText="QTYRecv" DataField="QTYRecv" />
                <asp:BoundField HeaderText="QTYGood" DataField="QTYGood" />
                <asp:BoundField HeaderText="QTYScrap" DataField="QTYScrap" />
                <asp:BoundField HeaderText="GoodButOverPOQty" DataField="GoodButOverPOQty" />
                <asp:BoundField HeaderText="NotInspecQty" DataField="NotInspecQty" />
                <asp:BoundField HeaderText="YellowQty" DataField="YellowQty" />
                <asp:BoundField HeaderText="TooLightQty" DataField="TooLightQty" />
                <asp:BoundField HeaderText="CrackQty" DataField="CrackQty" />
                <asp:BoundField HeaderText="BadCutQty" DataField="BadCutQty" />
                <asp:BoundField HeaderText="TooBigQty" DataField="TooBigQty" />
                <asp:BoundField HeaderText="TooSmallQty" DataField="TooSmallQty" />
                <asp:BoundField HeaderText="Zoning" DataField="Zoning" />
                <asp:BoundField HeaderText="Clearity" DataField="Clearity" />
                <asp:BoundField HeaderText="Other" DataField="Other" />
                <asp:BoundField HeaderText="MissingQty" DataField="MissingQty" />
                <asp:BoundField HeaderText="OverQty" DataField="OverQty" />
                <asp:BoundField HeaderText="ReturnQty" DataField="ReturnQty" />
                <asp:BoundField HeaderText="Remark" DataField="Remark" />
                <asp:BoundField HeaderText="Reason" DataField="Reason" />
                <asp:BoundField HeaderText="ShelfNo" DataField="ShelfNo" />
                <asp:BoundField HeaderText="InspecType" DataField="InspecType" />

            </Columns>
        </asp:GridView>

    </div>


    <%-- Show Progress --%>
    <div id="showprog" class="modal fade bs-example-modal-sm" role="dialog" tabindex="-1">

        <div class="modal-dialog modal-sm">

            <div class="modal-content">

                <div class="modal-header">

                    <%--  <button type="button" class="close" data-dismiss="modal">&times;</button> --%>

                    <h4 class="modal-title">System is loading.</h4>

                </div>

                <div class="text-center text-info">

                    <div class="progress">

                        <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">
                            Please Wait...
                           <%-- 10%  <%= this.widthvalue %>  --%>
                        </div>

                    </div>

                </div>

                <div class="modal-footer">
                </div>

            </div>

        </div>

    </div>


    <%-- Show Complete --%>
    <div id="showcomplete" class="modal fade bs-example-modal-sm" role="dialog" tabindex="-1">

        <div class="modal-dialog modal-sm">

            <div class="modal-content">

                <div class="modal-header">

                    <%--  <button type="button" class="close" data-dismiss="modal">&times;</button> --%>

                    <h4 class="modal-title">Import Excel </h4>

                </div>

                <div class="text-center text-success">
                    Complete...
                </div>

                <div class="modal-footer">
                    <button id="btnok" class="btn btn-success" data-dismiss="modal">OK</button>
                </div>

            </div>

        </div>

    </div>


    <%-- Show Error --%>
    <div id="showerror" class="modal fade bs-example-modal-sm" role="dialog" tabindex="-1">

        <div class="modal-dialog modal-sm">

            <div class="modal-content">

                <div class="modal-header">

                    <%--  <button type="button" class="close" data-dismiss="modal">&times;</button> --%>

                    <h4 class="modal-title">Import Excel </h4>

                </div>

                <div class="text-center text-danger">
                    Error Duplicate Record...!
                </div>

                <div class="modal-footer">
                    <button id="btnerror" class="btn btn-danger" data-dismiss="modal">OK</button>
                </div>

            </div>

        </div>

    </div>


    <%-- Show Save Excel --%>
    <div id="mdconfirm" class="modal fade bs-example-modal-sm" role="dialog" tabindex="-1">

        <div class="modal-dialog modal-sm">

            <div class="modal-content">

                <div class="modal-header">

                    <button type="button" class="close" data-dismiss="modal">&times;</button>

                    <h4 class="modal-title">Import Excel</h4>

                </div>

                <div class="text-center">
                    Do You Want To Import Excel ?

                </div>

                <div class="modal-footer">

                    <button id="btnCancel" class="btn btn-danger" data-dismiss="modal">No</button>

                    <asp:Button ID="btnConfirm" CssClass="btn btn-success" runat="server" Text="Yes" />

                </div>

            </div>

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
                    <button id="btnCancel2" class="btn btn-danger" data-dismiss="modal">OK</button>
                    <button id="btnSuccess" class="btn btn-success" data-dismiss="modal">OK</button>


                </div>
            </div>
        </div>
    </div>

</asp:Content>

