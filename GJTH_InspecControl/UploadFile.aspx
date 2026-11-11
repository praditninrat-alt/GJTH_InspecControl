<%@ Page Title="" Language="C#" MasterPageFile="~/PageMaster.Master" AutoEventWireup="true" CodeBehind="UploadFile.aspx.cs" Inherits="GJTH_ISPC.UploadFile" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script type="text/javascript">


        $(document).ready(function () {

            $("#<%= FileUpload1.ClientID  %>").change(function () {

                if ($("#<%= FileUpload1.ClientID  %>").val() != "") {

                    // -------------------------------------------------                    
                    $("#<%= btnNewwindow.ClientID %>").trigger("click");
                    // -------------------------------------------------                 
                }
            });



        });

        function PostBackParentWindow() {

            __doPostBack(null, null);
        }

    </script>
    <style>
        .upload-box {
            width: 500px;
            margin: 0 auto;
            padding: 30px;
            border: 2px dashed #0d6efd;
            border-radius: 12px;
            background-color: #f8f9fa;
            text-align: center;
        }

        .custom-file-upload {
            width: 100%;
            padding: 10px;
            border: 1px solid #ced4da;
            border-radius: 8px;
            background: #fff;
            cursor: pointer;
        }

            .custom-file-upload:hover {
                border-color: #0d6efd;
            }

        .text-danger {
            font-size: 14px;
        }
    </style>
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
    </style>

    <div class="row" style="margin-top: 20px;">
        <div class="col-lg-12 text-center">

            <div class="upload-box">

                <asp:FileUpload ID="FileUpload1"
                    runat="server"
                    CssClass="custom-file-upload"
                    accept=".xls,.xlsx" />

                <div style="margin-top: 10px;">
                    <asp:RegularExpressionValidator
                        ID="RegularExpressionValidator1"
                        runat="server"
                        ControlToValidate="FileUpload1"
                        ErrorMessage="Only .xls, .xlsx files are allowed"
                        ValidationExpression="([a-zA-Z0-9()'\s_\\.\-:])+(.xls|.xlsx)$"
                        CssClass="text-danger">
                    </asp:RegularExpressionValidator>
                    <div>
                    </div>
                    <asp:RadioButton ID="rdoDiamond" runat="server" Text="Diamond" TextAlign="Right" GroupName="rdochk" Checked="true" OnCheckedChanged="rdoChange" AutoPostBack="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoStone" runat="server" Text="Stone" TextAlign="Right" GroupName="rdochk" OnCheckedChanged="rdoChange" AutoPostBack="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
            </div>

            <div style="display: none;">
                <asp:Button ID="btnNewwindow" runat="server" Text="Button" />
            </div>
        </div>
    </div>

    <!-- Modal Dialog -->
    <div id="mdtrndetail" class="modal fade bs-example-modal-sm" role="dialog" tabindex="-1">
        <div class="modal-dialog" style="max-width:600px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="textHeader">Description</h4>
                </div>
                <div class="text-center" style="padding: 15px;">
                    <span id="mdTrnid" style="font-size:18px;"></span>
                </div>
                <div class="modal-footer">
                    <button id="btnCancel" type="button" class="btn btn-danger" data-dismiss="modal">OK</button>
                    <button id="btnSuccess" type="button" class="btn btn-success" data-dismiss="modal">OK</button>
                </div>
            </div>
        </div>
    </div>

 <script src="Scripts/bootstrap.min.js"></script>

</asp:Content>
