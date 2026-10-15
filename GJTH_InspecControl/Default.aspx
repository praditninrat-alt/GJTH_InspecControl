<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/PageMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GJTH_ISPC._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <script type="text/javascript">
         
            function test()
            {
             
                $('#mdtrndetail').modal('show');
               
            }
         

            </script>
     <asp:Button ID="Button2" runat="server" Text="aaa"  CssClass="btn btn-info"   OnClientClick="test"  Width="100%"  Visible="false"  />  
    <asp:Button ID="Button1" runat="server" Text="Search"  CssClass="btn btn-info"  onclick="btnSearchlist_Click"  Width="100%"  Visible="false"  />
   
   <%-- <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>--%>

  <%--  <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Get more libraries</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>--%>


       <div id="mdtrndetail" class="modal fade bs-example-modal-sm" role="dialog" tabindex="-1" data-backdrop="static"> 
          <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" id="textHeader">
                  <button type="button" class="close" data-dismiss="modal">&times;</button>
                  <h4 class="modal-title">Drescription</h4>
                 
                </div>
                <div class="text-center" > 
         
                 <span id="mdTrnid"> </span>

                </div>
                <div class="modal-footer">      
                    <button ID="btnCancel" class="btn btn-danger" data-dismiss="modal">No</button>              
                    <asp:Button ID="btnConfirm" CssClass="btn btn-success"  runat="server" Text="Yes"   />
                </div>
            </div>
          </div>
        </div>

</asp:Content>
