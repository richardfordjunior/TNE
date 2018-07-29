<%@ Page Title="" Debug="true" Language="C#" MasterPageFile="~/Site.master" 
AutoEventWireup="true" CodeFile="TimeEntry.aspx.cs" Inherits="TimeEntry" MaintainScrollPositionOnPostback="true"  EnableViewState="true" EnableSessionState="True"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">


   <%-- <meta http-equiv="Page-Exit" content="progid:DXImageTransform.Microsoft.Fade(duration=.5)">--%>
    <style type="text/css">
        .style1
        {
            width: 734px;
        }
        .style3
        {
            height: 161px;
        }
        .style4
        {
            width: 202px;
        }
        .style5
        {
            height: 161px;
            width: 202px;
        }
        .style6
        {
            color: #FF0000;
        }
        .style7
        {
            width: 202px;
            height: 27px;
        }
        .style8
        {
            height: 27px;
        }
        </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server"   >
    </asp:ToolkitScriptManager>
 
 
 <script language="javascript" type="text/javascript">
     function Show(ctl) {
         var ddl = $get("MainContent_" + ctl);
         if (ctl == 'ddlAddLocation') {
             var ddlVal = ddl.options[ddl.selectedIndex].value;
               switch (ddlVal) {
            case 'Add Location':
                Locl.style.visibility = "visible"
                break;
            case 'Add New Client':
                Locl.style.visibility = "visible"
                break;
            default:
                     Locl.style.visibility = "hidden";
        }
               

     }
         if (ctl == 'ddlWorkDesc') {
             if (ddl.options[ddl.selectedIndex].value == 'Edit List') {
               
                 WkDesc.style.visibility = "visible";
             }
             else {
                 WkDesc.style.visibility = "hidden";
             }
         }
     }

</script>
     
 <script language="javascript" type="text/javascript" >
     function Validate() {
         var msg = "";
         var ctl = $get("MainContent_" + 'txtHours');
         var btn = $get("MainContent_" + 'btnSubmitAll');
         var send = $get("MainContent_" + 'btnSend');
         var cal = $get("MainContent_" + 'lstCalendar');
             if (!checkForDecimal()) {
                 msg = 'Enter the number of hours worked for this location.\n'; alert(msg); 
             }
             else {
             btn.disabled = false;
             send.disabled = false;

         }
     }

     function GotoHistory() {
        
         var start = $get("MainContent_" + 'txtHistStart').value;
         var end = $get("MainContent_" + 'txtHistEnd').value;
         window.open('TimeEntryHistory.aspx?S=' + start + '&E=' + end, "TimeHistory", "width=700,height=500");
     }

     function showHideDate() {
         alert();
         var ctl1 = $get("MainContent_" + 'Dates');
         if (ctl1.style.visibilty == "hidden") {
             ctl1.style.visibility = "visible";
         }
         else {
             ctl1.style.visibilty = "hidden";
         }

     }

     function ValidateDates(ctl,ctl1) {
     var msg ="";
     var ctl = $get("MainContent_" + ctl);
     var ctl1 = $get("MainContent_" + ctl1);
     if (ctl.value == '' || ctl1.value == '') {
         msg = 'Please enter a valid start and end date.';
         alert(msg);
     }
     else {
         GotoHistory();
     }
 }

 function Enable(ctl) {
     var ctl = $get("MainContent_" + ctl);
     ctl.disabled = true;
 }
 function check(){
 var ctl = $get("MainContent_" + 'btnSubmitAll');
 alert(ctl);
 }

 function ChangeText() {
 alert();
     var ctl = $get("MainContent_" + 'btnSubmitAll');
     var cal = $get("MainContent_" + 'lstCalendar');
     ctl.style.color = 'red';         
     ctl.value = 'Loading.......';
     } 
 function ChangeTextBack() {
     var ctl = $get("MainContent_" + 'btnSubmitAll');
     var ctl1 = $get("MainContent_" + 'txtHours');
     var ctl2 = $get("MainContent_" + 'txtComments');
     ctl.style.color = 'black';
     ctl.value = 'Enter Hours';
   
     //ctl1.value = ''; 
     //ctl2.value = '';

 }
 function OpenEmail() {
     var ctl = $get("MainContent_" + 'hfUserId');
     ctl = ctl.value;
     window.open('TimeEntryHistory.aspx?enum=' + ctl, '_parent');
 }
 function EmailSuccess() {
     alert('Email was sent successfully!!!');
 }

 function GetComments(param1, param2) {
     window.open("Details.aspx?enum=" + param1 + "&Row=" + param2, "_blank", "width=500,height=500;");
 }

 function GetEditRecord(param1, param2) {
     window.open("Edit.aspx?enum=" + param1 + "&Row=" + param2, "Edit Record", "width=500,height=650;");
 }

 function onOK() {
     var ctl = $get("MainContent_" + 'Panel1');
     ctl.style.visibility = "hidden";

 }
 function UpdatePanelNotify() {
     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
 }
 function endRequestHandler(sender, args) {
//     alert('Update panel updated.');
//     var ctl = $get("MainContent_" + 'btnSubmitAll');
     //     ctl.innerText = 'Test';
     EmailSuccess()
 }
 function checkForDecimal() {
     var num = $get("MainContent_" + 'txtHours');
     return (num.value/1) >= 1 ? true :false;
 }
  function check() {
     var ctrl = $get("MainContent_" + 'lblRunTime');
     alert();
 }

 function disable()
 {
    var ctrl = $get("MainContent_" + 'btnSubmitAll');
    alert();
}

 function fnClickUpdate(sender, e)
{
  __doPostBack(sender,e);
}

function GetUserBrowser() {
    //alert();
    var userAgent = navigator.userAgent;
    var hfUserAgent = $get("MainContent_" + 'hfUserAgent');
    if (userAgent.indexOf('MSIE') >= 0) {
        hfUserAgent = 0;
    }
    if (userAgent.indexOf('FireFox') >= 0) {
        hfUserAgent = 1;
    }
    if (userAgent.indexOf('Chrome') >= 0) {
        hfUserAgent = 2;
    }
    return userAgent;
}

 </script>
  
    <br />  
        
      <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
      TargetControlID="btnEmailNo"
      PopupControlID ="Panel2"
      DropShadow = "false"
      BackgroundCssClass="modalBackground"
      CancelControlID="btnEmailNo" >    
    </asp:ModalPopupExtender>            
    <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
      TargetControlID="btnEmailNo"
      PopupControlID ="Panel2"
      DropShadow = "false"
      BackgroundCssClass="modalBackground"
      CancelControlID="btnEmailNo" >    
    </asp:ModalPopupExtender>

<%--<asp:ConfirmButtonExtender ID="btnSendEmail_ConfirmButtonExtender" 
                          runat="server"  Enabled="True" TargetControlID="btnSendEmail"
                           ConfirmText = "Is the recipient's e-mail address filled in correctly?"
                        >
                        
                      </asp:ConfirmButtonExtender>--%>
                      
<asp:HiddenField id="hfUserId" runat="server"/>
<asp:HiddenField id="hfUserAgent" runat="server"/>
   <asp:Panel ID="pnlExcel" runat="server" Width="236px" Height="150px"   Visible="false"
                   BorderColor="Black" ForeColor="Black" BorderStyle="Outset" 
                   HorizontalAlign="Center" CssClass="ModalWindow"  DefaultButton="btnConfirmExport">
         Enter a date range for the records to be exported<br />
         Start Date:<asp:TextBox ID="txtStartDateExcel" runat="server"></asp:TextBox>
         <asp:CalendarExtender ID="txtStartDateExcel_CalendarExtender" runat="server" 
             Enabled="True" TargetControlID="txtStartDateExcel">
         </asp:CalendarExtender>
         <br />
         End Date:<asp:TextBox ID="txtEndDateExcel" runat="server" 
            ></asp:TextBox>
         <asp:CalendarExtender ID="txtEndDateExcel_CalendarExtender" runat="server" 
             Enabled="True" TargetControlID="txtEndDateExcel">
         </asp:CalendarExtender>
         <br />
         <br />
      <asp:Button ID="btnConfirmExport" runat="server" Text="Export to Excel"  OnClick ="btnConfirmExport_Click" />
      <asp:Button ID="btnExcelCancel" runat="server" Text="Close" 
              />
     </asp:Panel> 
    <table style="width: 100%; height: 121px;">
        <tr>
            <td class="style7">
                  Work Day<br />
&nbsp;<asp:ListBox ID="lstCalendar" runat="server" Height="82px" Rows="10" Width="268px" ></asp:ListBox>
                 
                 </td>
            <td class="style8">
       
                
            </td>
            <td class="style8">
                </td>
        </tr>
        <tr>
      
  
    <td class="style4">
            
    <asp:Label ID="lblLoc" runat="server" Text="Add location"></asp:Label>
                        <asp:UpdateProgress ID="upProgAddLoc" runat="server" 
                            AssociatedUpdatePanelID="UpPnlAddLoc">
                            <ProgressTemplate>
                                <span class="style6">Adding.....</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                <br />
                <asp:UpdatePanel ID="upPanelAddLoc" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAddLocation" runat="server" 
                            onchange="Show('ddlAddLocation');" Width="201px">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="imgAddLoc" EventName="Click" />
                    </Triggers>
        </asp:UpdatePanel>
       
          </td>
            <td> 
                <asp:UpdatePanel ID="UpPnlAddLoc" runat="server">
                <ContentTemplate>
                           
    <div id="Locl" style="visibility:hidden">
        <asp:Label ID="lblLocation" runat="server" Text="Add new location"></asp:Label>
    <asp:TextBox ID="txtAddLocations" runat="server" 
         Width="250px" Wrap ="true"></asp:TextBox>
         <asp:ImageButton ID="imgAddLoc" runat="server" ToolTip="Click to add new location." 
         onclick="imgAddLoc_Click" ImageUrl="~/Images/editIcon.jpg"/>
    </div>
    </ContentTemplate>
    <Triggers> 
    <asp:AsyncPostBackTrigger ControlID="imgAddLoc" EventName="Click" /> 
    </Triggers>
    </asp:UpdatePanel>
     </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="style4">
        
           
                Work Description 
                 <asp:UpdateProgress ID="upProgAddWk" runat="server" 
                            AssociatedUpdatePanelID="UpPnlAddWk">
                            <ProgressTemplate>
                                <span class="style6">Adding.....</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                              
               
                        <br />
               
                
                        <asp:UpdatePanel ID="upPanelAddWk" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlWorkDesc" runat="server" 
                                    onchange="Show('ddlWorkDesc');" 
                                   >
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgAddWkdesc" EventName="Click" />
                            </Triggers>
                </asp:UpdatePanel>
                    
                   
                  
            </td>
            <td>
               <asp:UpdatePanel runat="server" ID="UpPnlAddWk">
               <ContentTemplate>
                   <div id="WkDesc" style="visibility:hidden">
                       <asp:Label ID="lblWkDesc" runat="server" Text="Add new"></asp:Label>
                         
    <asp:TextBox ID="txtWorkDesc" runat="server" 
         Width="250px" Wrap ="true" ></asp:TextBox><asp:ImageButton ID="imgAddWkdesc" runat="server" ToolTip="Click to add new description." onclick="imgAddWkdesc_Click" ImageUrl="~/Images/editIcon.jpg"/>  
    </div>
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="imgAddWkdesc" EventName="Click" />
    </Triggers>
   </asp:UpdatePanel>
  
    </td>
   
            <td>
                &nbsp;</td>
        </tr>
        <tr>
          <td class="style4">
          
    Hours 
              <br />
              <asp:DropDownList ID="ddlHoursSelect" runat="server">
              </asp:DropDownList>
          
          </td>
        </tr>
         <tr>
          <td class="style5">
          
     Comments<br />
    <asp:TextBox ID="txtComments" runat="server"  Width="250" Height="100" TextMode="MultiLine" 
        ></asp:TextBox>             

<asp:UpdatePanelAnimationExtender 
    ID="UpdatePanelAnimationExtender1" 
    runat="server" 
    BehaviorID="animation"
    TargetControlID ="upGrid"   >
    <Animations>
        <OnUpdating>
            <Parallel duration ="0">
               <FadeOut minimumOpacity="1.5"/>
                <EnableAction AnimationTarget="btnSubmitAll" Enabled="false" />   
                        
            </Parallel>
        </OnUpdating>
        <OnUpdated>
            <Parallel duration ="0">          
                <FadeIn minimumOpacity="1.5" />
                <EnableAction AnimationTarget="btnSubmitAll" Enabled="true" />
            </Parallel>
        </OnUpdated>
    </Animations>
    </asp:UpdatePanelAnimationExtender>

           

              <br />
                
      <asp:Button ID="btnSubmitAll" runat="server" Text="Enter Hours" 
            OnClick="btnSubmitAll_Click"  Width="167px"  Visible= "true" />   
           
              <br />
              <asp:LinkButton ID="lnkSendToHistory" OnClientClick="OpenEmail();" 
                  Text="Click to filter/email results"  OnClick="lnkSendToHistory_OnClick" 
                  ToolTip="View filtered results" runat="server"></asp:LinkButton>
              <%--<a href="TimeEntryHistory.aspx" onclick="OpenEmail();" runat="server" ID="lnkSendToHistory" >Click here to filter results </a>--%>
       
              <br />
              
       
              <br />

           

     <asp:Button ID="btnSend1" runat="server" Text="Submit Time Sheet"  Visible="false"
                              OnClick="btnSend1_Click"  OnClientClick ="" Height="16px" 
                  Width="124px" />
                
            
       
          </td>
               
  <%--<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />--%>
             
          <td class="style3">

                  <asp:Panel ID="PnlEmail" runat="server" Width="236px" Height="180px"   Visible="false"
                  BorderColor="Black" ForeColor="Black" BorderStyle="Outset" HorizontalAlign="Center" CssClass="ModalWindow" >
            Send From:<asp:TextBox ID="txtEmailfrom" runat="server" Width="207px" 
                          ></asp:TextBox>
                 <br />
            Enter the email recipient: 
                 <asp:TextBox ID="txtEmailRec" runat="server" Width="207px"></asp:TextBox>
                      <br />
                 <br />
                 &nbsp;
                
                 <asp:Button ID="btnSendEmail" runat="server" Text="Submit Timesheet" OnClick="btnSendEmail_Click" />
                      <%--<asp:ConfirmButtonExtender ID="btnSendEmail_ConfirmButtonExtender" 
                          runat="server"  Enabled="True" TargetControlID="btnSendEmail"
                           ConfirmText = "Is the recipient's e-mail address filled in correctly?"
                        >
                        
                      </asp:ConfirmButtonExtender>--%>
                  
                     
                      &nbsp;
                 <asp:Button 
                     ID="btnEmailNo" runat="server" Text="No"  UseSubmitBehavior="true"
                     />
        
             
             </asp:Panel>
            

              <%--<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />--%>
            <asp:UpdatePanel ID="upsum"  runat="server" UpdateMode="Always" ChildrenAsTriggers="true" >
          <ContentTemplate>
           <asp:GridView ID="sumView" runat="server" Caption="Work Hours Weekly Summary" 
                    CellPadding="4" ForeColor="#333333" GridLines="None" HorizontalAlign="Left" 
                    Width="448px" 
                  ShowFooter="True"  OnRowDataBound="sumView_OnRowDataBound" Height="16px" 
                  onselectedindexchanged="sumView_SelectedIndexChanged">
                   <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                   <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>

                
    
                </ContentTemplate>
                <Triggers> <asp:AsyncPostBackTrigger ControlID="btnSubmitAll" /></Triggers>
              </asp:UpdatePanel>
              <table style="width: 25%; height: 129px;">
                  <tr>
                      <td>
                          &nbsp;
                         
                          </td>
                      <td>
                          &nbsp;
                      </td>
                      <td>
                          &nbsp;
                      </td>
                  </tr>
                  <tr>
                      <td>
                          &nbsp;
                      </td>
                      <td>
                          &nbsp;
                      </td>
                      <td>
                          &nbsp;
                      </td>
                  </tr>
                  <tr>
                      <td>
                          &nbsp;</td>
                      <td>
                          &nbsp;
                      </td>
                      <td>
                          &nbsp;
                      </td>
                  </tr>
                  <tr>
                     <td id ="History" style="visibility:hidden">
                        
                       <asp:LinkButton ID="lnkHist" runat="server" Visible="false" 
                               OnClientClick="return ValidateDates('txtHistStart','txtHistEnd')" 
                             >View History</asp:LinkButton>
                          <br />
                         <asp:Label ID="lblStart" runat="server" Text="Start Date" Visible="false"></asp:Label> <asp:TextBox ID="txtHistStart" runat="server" Visible="false" ></asp:TextBox>
                          <br />
                         <asp:Label ID="lblEnd" runat="server" Text="End Date" Visible="false"></asp:Label> <asp:TextBox ID="txtHistEnd" runat="server" Visible="false" ></asp:TextBox>
                         <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
     TargetControlID="txtHistStart" 
     PopupPosition="Right"
     Format= "M/d/yyyy" CssClass="MyCalendar">
    </asp:CalendarExtender>
    <asp:CalendarExtender ID="CalendarExtender3" runat="server" 
     TargetControlID="txtHistEnd" 
     PopupPosition="Right"
     Format= "M/d/yyyy" CssClass="MyCalendar">
    </asp:CalendarExtender>
                     </td>
                  </tr>
                
                      <tr>
                     
                     <td > 
                   
                       
                   
                         &nbsp;</td>
                  </tr>
                   <tr>
                     
                     <td > 
                        
                    </td>
                  </tr>
                
              </table>
          </td>      
        </tr>
    </table>
     
    
  
    
     
    <br /> <hr /> 
  
    <table style="width:100%;">
        <tr>
            <td >     
              
                <asp:UpdatePanel ID="upGrid" 
                runat="server" 
                UpdateMode="Always" 
                ChildrenAsTriggers="true"  
                RenderMode="Block"  
                ViewStateMode="Inherit">
                <ContentTemplate>
                <asp:UpdateProgress AssociatedUpdatePanelID ="upGrid" ID="upProg1"  runat="server" DisplayAfter = "1" >
                <ProgressTemplate>
                    <img  alt="Loading......." src="Images/rotating_arrow.gif" style="width: 42px" />Loading......
                </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:Label ID="lblRowText" runat="server" Visible="False" 
                    Text ="Total record(s) returned: " ForeColor="Black"></asp:Label>
                <asp:Label ID="lblRowCount" runat="server" Visible="False" ForeColor="Black" ></asp:Label>
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:Label ID="lblPageView" runat="server" ForeColor="Black" Height="16px" 
                        Width="376px" ></asp:Label>
                    <asp:GridView ID="gView" runat="server" AllowPaging="True" 
                        AutoGenerateEditButton="false" DataKeyNames="Id" GridLines="None" 
                        HorizontalAlign="Center" OnPageIndexChanging="gView_PageIndexChanging" 
                        OnRowCreated="gView_OnRowCreated" OnRowDataBound="gView_OnRowDataBound" 
                        OnRowDeleting="gView_RowDeleting" Width="918px">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRowEdit" runat="server" OnClick="lnkRowEdit_OnClick" 
                                        Text="Edit">
                      
                                </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#FF3C3C" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
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
                <a href="#" onclick="window.scrollTo(0,0); return false">Back to Top</a>            
                <asp:HiddenField ID="hfDeleted" runat="server" />
               

                    <asp:TextBox ID="txtEnterQuote" runat="server" Height="25px" Width="484px" Visible="false"></asp:TextBox>
               

                 <asp:Panel ID="pnlDeleteRow" runat="server" Width="236px" Height="75px"   Visible="false"
                  BorderColor="Black" ForeColor="Black" BorderStyle="Outset" HorizontalAlign="Center" CssClass="ModalWindow" >
             
                 <asp:Label ID="lblmesg" runat="server" Width="207px" Text="Are you sure you want to delete this record?"></asp:Label>
                 <br />
                 &nbsp;<asp:Button ID="btnYes" runat="server" Text="Delete"  OnClick= "btnYes_Click" />&nbsp;
                 <asp:Button 
                     ID="btnNo" runat="server" Text="Don't Delete"  UseSubmitBehavior="true"
                     />
         </asp:Panel>

        <asp:Panel ID="pnlEditRecord" runat="server" 
                  Wrap="true" 
                  Width="500px" 
                  Height="350px"   
                  Visible="false"
                  BorderColor="Black" 
                  ForeColor="Black" 
                  BorderStyle="Outset" 
                  HorizontalAlign="Center" 
                  CssClass="ModalWindow" >
                  Edit Record <br />
                  <br />
                 <asp:Label ID="lblDate" runat="server" Width="207px" Text="Date"></asp:Label>
                
                 <asp:TextBox ID="txtDate" runat="server" CssClass="bold" 
                    Height="27px" Width="235px"></asp:TextBox>
                 <asp:Label ID="lblClient" runat="server" Width="207px" Text="Client"></asp:Label>
                
                 <asp:TextBox ID="txtClient" runat="server" CssClass="bold" 
                    Height="27px" Width="235px" ></asp:TextBox>
                 <asp:Label ID="lblWorkDescr" runat="server" Width="207px" Text="Work Description"></asp:Label>
               
                 <asp:TextBox ID="txtEditWorkDesc" runat="server" CssClass="bold" 
                    Height="27px" Width="235px"></asp:TextBox>
                 <asp:Label ID="lblWorkHrs" runat="server" Width="207px" Text="Work Hours"></asp:Label>
               
                 <asp:TextBox ID="txtEditHours" runat="server" CssClass="bold"
                Height="27px" Width="78px"></asp:TextBox>
                 <asp:Label ID="lblEditComments" runat="server" Width="207px" Text="Comments"></asp:Label>
               
                <asp:TextBox ID="txtEditComments" runat="server" TextMode=MultiLine 
                    CssClass="style4" Width="301px"></asp:TextBox>
                 <asp:Label ID="lblPaid" runat="server" Width="207px" Text="Payment Received"></asp:Label>
                 <asp:CheckBox ID="cbPayment" runat="server" /><br/><br/>
                 <asp:UpdatePanel ChildrenAsTriggers ="true" ID="upPanelEditRecord" runat="server" UpdateMode="Always">
                     <ContentTemplate>
                       <asp:Button ID="btnUpdate" runat="server" onclick="btnUpdate_Click" 
                            Text="Edit Record" Width="117px" />
                         <asp:UpdateProgress ID="upProgEditRecord" runat="server" 
                             AssociatedUpdatePanelID="upPanelEditRecord">
                             <ProgressTemplate>
                                 <font color="red" style="display:inline">Saving....</font>
                             </ProgressTemplate>
                         </asp:UpdateProgress>
                     </ContentTemplate>
                     <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="btnUpdate" /> 
                     </Triggers>
                 </asp:UpdatePanel>

                            <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                            Text="Cancel Edit" Width="117px" />
         </asp:Panel>
        
          <asp:ModalPopupExtender ID="MdlPopDeleteRow" runat="server" 
                 TargetControlID="btnNo"
      PopupControlID ="pnlDeleteRow"
      DropShadow = "false"
      BackgroundCssClass="modalBackground"
      CancelControlID="btnNo"> 
                </asp:ModalPopupExtender>

                    <asp:ModalPopupExtender ID="MdlPopEditRow" runat="server" 
                 TargetControlID="btnCancel"
      PopupControlID ="pnlEditRecord"
      DropShadow = "false"
      BackgroundCssClass="modalBackground"
      CancelControlID="btnCancel"> 
                </asp:ModalPopupExtender>
                    &nbsp;<asp:Button ID="btnSubmitStock" runat="server" Text="Get Quote" Visible= "false" 
                        onclick="btnSubmitStock_Click" />
                 </ContentTemplate>
            
            <Triggers>
         <asp:AsyncPostBackTrigger ControlID = "btnSubmitAll" />
            </Triggers>
        </asp:UpdatePanel> 
              
     
    </table>



    
</asp:Content>
   



















































