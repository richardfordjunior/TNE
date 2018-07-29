<%@ Page Title="Email Time Sheet" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Email.aspx.cs" Inherits="Email" %>
<%@ PreviousPageType VirtualPath="~/TimeEntryHistory.aspx"  %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            color: #000000;
        }
        .style3
        {
            color: #FF0000;
        }
    </style>
  
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="height: 257px; width: 443px;">
        <h2 class="style2">Email Time Sheet</h2>
        <br />
        <asp:ToolKitScriptManager ID="ScriptManager1" runat="server">
</asp:ToolKitScriptManager>

        <asp:Panel ID="pnlEmailAddress" runat="server" Height="211px" Width="431px" 
            HorizontalAlign="Center">
            <span class="style1"><span class="style2">Email Time Sheet for Week Ending</span> 
                <asp:DropDownList ID="ddlWeekEndDate" runat="server">
                </asp:DropDownList>
            <br />
            </span><span class="style2">Select Email Address</span>
            <asp:DropDownList ID="ddlEmalList" runat="server">
            </asp:DropDownList>
            <br />
            <span class="style2">Add CC</span><strong>:</strong>
            <asp:DropDownList ID="ddlEmalList0" runat="server">
            </asp:DropDownList>
            <br />
            <em>or</em><br /> 
             <a href='javascript:OnClientClick =Hide();' >Add New Email Address</a> 
             <div id="NewEmailAddress" style="display:none" >
                  <br />
                First Name <asp:TextBox ID="txtFname" runat="server"></asp:TextBox>
                  <br />
                Last Name <asp:TextBox ID="txtLname" runat="server"></asp:TextBox>
                  <br />
                Email Address<asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                  <br />
          <label id="lblProcess"style="display:none; color:Red">Adding.......</label>
                 <asp:Button ID="btnAdd" runat="server" Text="Add New Email Address"
                 onclick="btnAdd_Click"  OnClientClick="HideProcess();"
                 />
            </div>
            
       

            <asp:HiddenField ID="hfUserId" runat="server" />
          
          
            <br />
            <asp:UpdatePanel id= "upPnlSendEmail" runat="server" UpdateMode="Always">
           <ContentTemplate>
           <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upPnlSendEmail" ID="upPrgSendEmail">
           <ProgressTemplate><span class="style3">Sending please wait.....</span></ProgressTemplate>
           </asp:UpdateProgress>
                <asp:Button ID="btnSendEmail" runat="server" Text="Email Time Details" 
                onclick="btnSendEmail_Click"  />
           </ContentTemplate>
           <Triggers>
           <asp:AsyncPostBackTrigger ControlID="btnSendEmail" />
           </Triggers>
            </asp:UpdatePanel>
       
            <br />
        </asp:Panel>  
       
    </div>
  
    <script language="javascript" type="text/javascript">
        function Hide() {
            var divstyle = new String();
            divstyle = document.getElementById("NewEmailAddress").style.display;
            if (divstyle.toLowerCase() == "block" || divstyle == "") {
                document.getElementById("NewEmailAddress").style.display = "none";
                document.getElementById("btnSendEmail").style.display = "block";
            }
            else {
                document.getElementById("NewEmailAddress").style.display = "block";
                document.getElementById("btnSendEmail").style.display = "none";
            }
        }
        function HideProcess() {
            var val;
            var divstyle = new String();
            divstyle = document.getElementById("lblProcess").style.display;
            if (divstyle.toLowerCase() == "block" || divstyle == "") {
                document.getElementById("lblProcess").style.display = "none";
            }
            else {
                document.getElementById("lblProcess").style.display = "block";
            }
            if (val == 1) {
                document.getElementById("lblProcess").innerText = "Loading...."
            }
        }

   
    </script>

</asp:Content>

