<%@ Page Title="" Language="C#" MasterPageFile="~/PageMaster.Master" AutoEventWireup="true" CodeBehind="InspecList.aspx.cs" Inherits="GJTH_ISPC.InspecList" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script type="text/javascript">

        function DeleteTrans(id, po) {

            $("#deletransferid").text(id);

            $("#<%= HdeleteId.ClientID %>").val(id);
            $("#<%= HPO.ClientID %>").val(po);

            $("#mddelete").modal("show");


        }


    </script>


    <script>

        $('#Txtfromdate').blur(function () {
            alert("aaa");
            var s = $('#Txtfromdate').val();
            var dateRegex = '^(0[1-9]|1[012])[\/\-](0[1-9]|[12][0-9]|3[01])[\/\-](19|20)\d\d$';
            if (!dateRegex.match(s)) {
                $("#msgDOB").val("date format not valid");
                $("#msgDate").css("display", "block");
            } else {
                $("#msgDate").css("display", "none");
            }
        });


        function checkdate() {
            var input = document.getElementById("Txtfromdate");
            var validformat = /^\d{2}\/\d{2}\/\d{4}$/ //Basic check for format validity
            if (!validformat.test(input.value))
                alert("Invalid Date Format. Please correct and submit again.")
            else { //Detailed check for valid date ranges
                var monthfield = input.value.split("/")[0]
                var dayfield = input.value.split("/")[1]
                var yearfield = input.value.split("/")[2]
                var dayobj = new Date(yearfield, monthfield - 1, dayfield)
                if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield))
                    alert("Invalid Day, Month, or Year range detected. Please correct and submit again.")
                else
                    alert("You have entered the valid date format!")
            }
        }

        $("#Txtfromdate").blur(function () {
            alert('working');
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
    </style>
    <style>
        .btn-upload {
            width: 220px;
            height: 50px;
            border: none;
            border-radius: 30px;
            background: linear-gradient(135deg, #28a745, #20c997);
            color: white;
            font-size: 16px;
            font-weight: 600;
            box-shadow: 0 4px 12px rgba(40,167,69,0.3);
            transition: all 0.3s ease;
        }

            .btn-upload:hover {
                background: linear-gradient(135deg, #218838, #17a2b8);
                transform: translateY(-2px);
                box-shadow: 0 6px 16px rgba(40,167,69,0.4);
            }

            .btn-upload:active {
                transform: translateY(0px);
            }
    </style>
    <div class="row" style="margin-top: 10px; margin-left: 5px;">

        <div class="col-lg-2 col-md-2  col-sm-2 text-right">
            <asp:Label ID="Label5" runat="server" Text="PO :"></asp:Label>
        </div>

        <div class="col-lg-2 text-left">
            <asp:TextBox ID="txtPO" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
        </div>
        <div class="col-lg-2 col-md-2  col-sm-2 text-right">
            <asp:Label ID="Label3" runat="server" Text="MemoID :"></asp:Label>
        </div>

        <div class="col-lg-2 text-left">
            <asp:TextBox ID="txtMemoID" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
        </div>

        <div class="col-lg-2 col-md-2  col-sm-2 text-right">
        </div>

        <div class="col-lg-2 col-md-2  col-sm-2 text-left">
            <asp:Button ID="btnSearchlist" runat="server" Text="🔍 Search" CssClass="btn btn-info" OnClick="btnSearchlist_Click" Width="100%" ValidationGroup="chkCld" />
        </div>

    </div>

    <hr style="border: 0px; border-bottom: 1px solid #000;" />
    <div style="text-align: center; margin-top: 20px;">
        <asp:Label ID="lblMessage" runat="server" ForeColor="#dc3545" Font-Bold="true" Font-Size="Large"> </asp:Label>
    </div>
    <div class="row">
        <div class="col-lg-6 col-md-6  col-sm-6 text-left">
        </div>

        <div class="col-lg-6 col-md-6  col-sm-6 text-right">
            <asp:Button ID="btnNew" runat="server" Text="➕ New Invoice/Memo" CssClass="btn btn-success" OnClick="btnNew_Click" Width="200px" />
            <%--<asp:Button ID="btnUp" runat="server" Text="UploadFile" CssClass="btn btn-success" OnClick="btnUp_Click" Width="200px" />--%>

            <asp:Button ID="btnUp" runat="server" Text="📤 Upload Excel File" CssClass="btn btn-primary" OnClick="btnUp_Click" Width="200px" Height="33px" />

        </div>


    </div>

    <div class="row" style="margin-top: 10px; margin-left: 10px;">

        <asp:GridView CssClass="table table-striped table-hover table-bordered"
            ID="GridView1"
            runat="server"
            AutoGenerateColumns="False"
            OnRowDataBound="GridView1_RowDataBound"
            OnPageIndexChanging="GridView1_PageIndexChanging"
            PageSize="20"
            AllowPaging="true">
            <HeaderStyle Font-Bold="true" />
            <PagerStyle CssClass="cssPager" ForeColor="Black" HorizontalAlign="Right" />
            <PagerSettings PageButtonCount="10" FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
            <Columns>

                <asp:TemplateField HeaderText="PO" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblPO" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Invoice" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblInvoice" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="CreateDate" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="LnkEdit" runat="server" OnClick="LnkEdit_Click" ImageUrl="~/imgs/Edit.png"></asp:ImageButton>


                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <div style="padding: 5px 0 0 0;">
                            <a class="edlink" href="javascript:void(0)" onclick="DeleteTrans('<%# Eval("MemoID") %>','<%# Eval("PO") %>')" title="Delete">
                                <asp:Image ID="imgDel" runat="server" ImageUrl="~/imgs/Cancel.png" />
                            </a>
                            <asp:Image ID="imgDelDis" runat="server" ImageUrl="~/imgs/CancelDis.png" Visible="false" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

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
                    <asp:HiddenField ID="HPO" runat="server" />
                    Do You Want To Delete Job Invoice : <span id="deletransferid" />
                </div>
                <div class="modal-footer">
                    <button id="btncanceldelete" class="btn btn-danger" data-dismiss="modal">No</button>
                    <asp:Button ID="btndelete" CssClass="btn btn-success" OnClick="btndelete_Click" runat="server" Text="Yes" />
                </div>
            </div>
        </div>
    </div>


    </span>

  
</asp:Content>


