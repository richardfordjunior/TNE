    <%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TimeEntryHistory.aspx.cs" Inherits="TimeEntryHistory" MaintainScrollPositionOnPostback="true" EnableSessionState="True" EnableViewState="true" %>
<%@ PreviousPageType VirtualPath="~/TimeEntry.aspx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        #btnEmail
        {
            width: 73px;
            height: 26px;
            margin-top: 0px;
        }
        </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <h2>Time & Expenses Details</h2>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <asp:HiddenField ID="hfUserId" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
   <Triggers>
    <asp:AsyncPostBackTrigger ControlID="txtHistStart" />
    <asp:AsyncPostBackTrigger ControlID="txtHistEnd" />
    <asp:PostBackTrigger ControlID ="imgExcel" />
    <asp:PostBackTrigger ControlID ="btnEmail" />
    <asp:AsyncPostBackTrigger ControlID ="btnSubmit" />
    <asp:PostBackTrigger ControlID="lnkExportResults"/>
   </Triggers>
    <ContentTemplate>
    
    Start Date <asp:TextBox ID="txtHistStart" runat="server" AutoPostBack="false" ></asp:TextBox>
                          &nbsp;End Date&nbsp; <asp:TextBox ID="txtHistEnd" runat="server"  AutoPostBack="false" ></asp:TextBox>

    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
     TargetControlID="txtHistStart" 
     PopupPosition="Right"
     Format= "M/d/yyyy" CssClass="MyCalendar">
    </asp:CalendarExtender>  
        <asp:CalendarExtender ID="CalendarExtender2" runat="server"
     TargetControlID="txtHistEnd" 
     PopupPosition="Right"
     Format= "M/d/yyyy" CssClass="MyCalendar">
    </asp:CalendarExtender>
     
        &nbsp;
        <br />
        <asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>
&nbsp;<asp:DropDownList ID="ddlClientList" runat="server" Height="30px" Width="263px">
        </asp:DropDownList>
        &nbsp;&nbsp;<asp:Button ID="btnSubmit" runat="server" onclick="btnSubmit_Click" 
            Text="View Results"  OnClientClick="HideProcess();"/>
        <%--<img alt="Email Results" id = "btnEmail" src="Images/emailIco.jpg"   runat="server" onclick="OpenEmail();" height="29" visible="false" width="65"/>--%>
        <asp:Label ID="lblProcess" runat="server" ForeColor="Red" Visible="false" Text="Loading....."></asp:Label>
      <asp:ImageButton  AlternateText="Email Results" 
      ID="btnEmail" ImageUrl="Images/emailIco.jpg"  OnClick="btnEmail_OnClick"
      runat="server" height="29" visible="false" width="65" ToolTip="Email Results" />
        
        <asp:LinkButton ID="lnkMailResults" runat="server" 
            onclick="lnkMailResults_Click" ToolTip="Email Results">Email Results</asp:LinkButton>
        &nbsp;&nbsp;<asp:ImageButton ID="imgExcel" runat="server" 
            AlternateText="Export to Excel" Height="29px" ImageUrl="~/Images/excel1.jpg" 
            OnClick="imgExcel_Click" ToolTip="Export to Excel" Width="65px" />
        <asp:LinkButton ID="lnkExportResults" runat="server" 
            Onclick="lnkExportResults_Click" ToolTip="Export to Excel">Export Results</asp:LinkButton>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
            AssociatedUpdatePanelID="UpdatePanel1" >
            <ProgressTemplate> 
            <img  alt="Loading......." src="Images/rotating_arrow.gif" style="width: 42px" />
            <asp:Label  ID="lblProgress" runat="server" Text="Loading......" ForeColor="Blue" 
                    style="color: #000000"></asp:Label>
             </ProgressTemplate>
        </asp:UpdateProgress>
        <br />
        <br />
            <asp:Label ID="lblRowCount" runat="server" >
            </asp:Label>
        <asp:Label ID="lblText" runat="server" 
            Visible="False"></asp:Label> 
        <br />
        &nbsp;<asp:Label ID="lblNumStart" runat="server" Visible="False"></asp:Label>
        &nbsp;&nbsp;
        <asp:Label ID="lblNumTotal" runat="server" Visible="False"></asp:Label>
        <br /> <hr /> 
             <asp:GridView ID="histGView" runat="server"  AutoGenerateColumns="true" 
             OnPageIndexChanging="histGView_PageIndexChanging"
              OnRowDataBound="histGView_OnRowDataBound" GridLines="None"
                ShowFooter="True" Caption="Time Details" CaptionAlign="Top" 
            Width="359px" >
       <%--   <Columns>

            
 
    
           <asp:ImageField DataImageUrlField="Profile_Picture"  
             HeaderText="Payment Received" />
        
           
                   
          </Columns>--%>
           <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />

                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" 
                     HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                 <PagerSettings Position="TopAndBottom" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
    </ContentTemplate> 

    </asp:UpdatePanel>  
           
  
<%--   <asp:UpdatePanelAnimationExtender 
    ID="UpdatePanelAnimationExtender1" 
    runat="server" 
    BehaviorID="animation"
    TargetControlID ="UpdatePanel1" >
    <Animations>
        <OnUpdating>
            <Parallel duration ="0">
                <FadeOut minimumOpacity=".5" />
                <EnableAction AnimationTarget="btnSubmit" Enabled="false" />
            </Parallel>
        </OnUpdating>
        <OnUpdated>
            <Parallel duration ="0">
                <FadeIn minimumOpacity=".5" />
                <EnableAction AnimationTarget="btnSubmit" Enabled="true" />
            </Parallel>
        </OnUpdated>
    </Animations>
    </asp:UpdatePanelAnimationExtender>--%>
   
   
   
    <asp:LinkButton ID="lnkBack" runat="server"  OnClientClick="Pageback();" 
        onclick="lnkBack_Click">Back</asp:LinkButton>
    <script type="text/javascript" language="javascript">
        function Pageback() {
            
            //window.open("TimeEntry.aspx");
           
        }
        function EmailSuccess() {
        alert('Email was sent successfully!!!');
            window.close()
        }

        function Disable() {
            alert();
            var ctl = $get("MainContent_" + 'btnSend');
            ctl.disabled = true;
        }

        function OpenEmail(val_In) {      
//            var ctl = $get("MainContent_" + 'hfUserId');
            //            ctl = ctl.value;
            var ctl = val_In;
            window.open('Email.aspx?enum=' + ctl, 'Email Time Sheet', "target=_blank,toolbar=no,menubar=no,width=400,height=400,resizable=no,location=no,directories=no")
        }
        function HideProcess() {
            var divstyle = new String();

//            if (document.getElementById("lblProcess").style != null || document.getElementById("lblProcess").style != 'undefined') 
//            {
//                divstyle = document.getElementById("lblProcess").style.display;
//                if (divstyle.toLowerCase() == "block" || divstyle == "") {
//                    document.getElementById("lblProcess").style.display = "none";
//                }
//                else {
//                    document.getElementById("lblProcess").style.display = "block";
//                }
//            }
        }
    </script>

    <script type="text/javascript" src="Scripts/jquery-1.7.1.js">
    
    </script>
            </asp:Content>

