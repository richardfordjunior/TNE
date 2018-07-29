<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" EnableViewState="true"
    CodeFile="Admin.aspx.cs" Inherits="Admin" MaintainScrollPositionOnPostback="true" %>

<%@ PreviousPageType VirtualPath="~/LogIn.aspx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
      
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <asp:Panel ID="pnlView" runat="server">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" Height="500px"
            Width="928px" Style="margin-top: 0px">
         <!--Upload File-->
              <asp:TabPanel ID="pnlUploadfile" HeaderText="Upload File" runat="server">
                 <ContentTemplate>         
                   
                          <br />
                           Select file to upload to server....<br />
                           <asp:FileUpload ID="upLoader" runat="server" ToolTip="Upload File" Width="529px"  />
                           <asp:FileUpload ID="upLoader1" runat="server" ToolTip="Upload File" Width="529px"  />
                           <br />
                        <asp:UpdatePanel ID="updatePanUploadFile" runat="server">
                            <ContentTemplate>
                               <asp:Button ID="cmdUploadMe"  Text="Upload File(s)" runat="server" onClick="cmdUploadMe_Click"  />
                               <br />
                               <asp:Label ID ="lblSub" runat="server"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                            <asp:PostBackTrigger ControlID="cmdUploadMe" />
                            </Triggers>
                        </asp:UpdatePanel>
                
                </ContentTemplate>                          
          </asp:TabPanel>

               <!--Download File-->
              <asp:TabPanel ID="pnlDownloadFile" HeaderText="Download File" runat="server">
                 <ContentTemplate>         
                   
                          <br />
                           Select file to download....<br />
                          
                           <br />
                       <!-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>-->
                         <asp:DropDownList ID="ddlFileList" runat="server"
            OnSelectedIndexChanged="ddlFileList_SelectedIndexChanged"
            AutoPostBack="true">
        </asp:DropDownList>
                           <!-- </ContentTemplate>
                        </asp:UpdatePanel>--> 
                
                </ContentTemplate>                         
          </asp:TabPanel>

                <!--Delete File-->
                <asp:TabPanel ID="DeleteFile" HeaderText="Files on Server" runat="server">
                <ContentTemplate>
                  <asp:ModalPopupExtender ID="MdlPopDeleteFile" runat="server" 
                    TargetControlID="btnNoDeleteFile"
                    PopupControlID ="pnlDelete"
                    BackgroundCssClass="modalBackground"
                    CancelControlID="btnNoDeleteFile" DynamicServicePath="" Enabled="True"> 
                  </asp:ModalPopupExtender>
               <asp:Panel ID="pnlDelete" runat="server" Height="150px"   Visible="False"
                  BorderColor="Black" ForeColor="Black" BorderStyle="Outset" 
                        HorizontalAlign="Center" CssClass="ModalWindow" >
                 <br />
                 <br />
                 <asp:Label ID="lblDeleteFile" runat="server" Width="207px"></asp:Label>
                 <br />
                 <br />
                 <br />
                 &nbsp;<asp:Button ID="btnYesDeleteFile" runat="server" Text="Yes"  OnClick= "btnYesDeleteFile_Click" />&nbsp;
                 <asp:Button 
                     ID="btnNoDeleteFile" runat="server" Text="Don't Delete"
                     />
         </asp:Panel>
                <asp:Panel ID="pnlDeleteFile" ScrollBars="Auto" runat="server" BorderStyle="Inset" 
                        BorderColor="Blue" Height="431px">
                    <br />
                    <asp:Label ID="lblText" runat="server" /> <br /> <br />
                    <asp:CheckBoxList ID="cboFiles"  
                    runat ="server" 
                    Width="696px" 
                    onselectedindexchanged="cboFiles_SelectedIndexChanged" 
                    AutoPostBack="True"
                    ToolTip = "Files on the remote server." BorderStyle="Inset"     >
                    
                    </asp:CheckBoxList>
                 </asp:Panel>
                </ContentTemplate>  
                </asp:TabPanel>
           
                
              
            <asp:TabPanel runat="server" ToolTip="Create User" HeaderText="Create User" ID="tbCreate">
                <ContentTemplate>
                    <br />
                    User Name<br />
                    <asp:TextBox ID="txtNewUser" runat="server"></asp:TextBox><br />
                    Password<br />
                    <asp:TextBox ID="txtPword" runat="server"></asp:TextBox><br />
                    Role<br />
                    <asp:TextBox ID="txtRole" runat="server" Text="2"></asp:TextBox><br />
                    E-Mail Address<br />
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox><br />
                    <br />
                   
                    <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Text="Create User" /></ContentTemplate>
                <%--<div id="block" runat="server" visible="true">--%>
 

   <%-- </div>--%>
            </asp:TabPanel>
            <asp:TabPanel ID="tpEdit" runat="server" ToolTip="Manage Users" HeaderText="Manage Users"
                OnLoad="btnView_Click">
                <ContentTemplate>
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnNo"
                        PopupControlID="pnl1" BackgroundCssClass="modalBackground" CancelControlID="btnNo"
                        DynamicServicePath="" Enabled="True">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnl1" runat="server" Width="236px" Height="100px" Visible="False"
                        BorderColor="Red" ForeColor="Black" BorderStyle="Inset" HorizontalAlign="Center"
                        CssClass="ModalWindow">
                        <asp:Label ID="lblmesg" runat="server" Width="207px"></asp:Label><br />
                        &#160;<asp:Button ID="btnYes" runat="server" Text="Delete" OnClick="btnYes_Click" />&nbsp;
                        <asp:Button ID="btnNo" runat="server" Text="Don't Delete" /></asp:Panel>
                    <asp:GridView ID="gView" runat="server" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True"
                        DataKeyNames="Id" OnRowDataBound="gView_OnRowDataBound" OnRowDeleting="gView_RowDeleting"
                        OnRowEditing="gView_OnRowEditing" AllowPaging="True">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#FF3C3C" />
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
                    <asp:Button ID="btnView" runat="server" Text="View Users" OnClick="btnView_Click" /></td><td>
                        &nbsp;&nbsp;
                    </td>
                    </tr><tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                        <td>
                            <br />
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    </table></ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="tpExec" HeaderText="Execute DB Scripts" runat="server">
                <ContentTemplate>
                    <br />
                    <asp:DataList ID="dList" runat="server" CaptionAlign="Top" DataKeyField="name" GridLines="Both"
                        RepeatLayout="Flow" Style="color: #000000; font-style: italic">
                        <HeaderTemplate>
                            <b>Available database tables</b></HeaderTemplate>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem,"name") %>
                        </ItemTemplate>
                    </asp:DataList><br />
                    <br />
                    <strong>Enter query<asp:RadioButtonList ID="rdoSelectQuery" runat="server">
                        <asp:ListItem>DDL Script</asp:ListItem>
                        <asp:ListItem>DML Script</asp:ListItem>
                    </asp:RadioButtonList>
                    </strong>
                    <br />
                    <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" Height="58px" Width="311px"></asp:TextBox>
                    <br />
                    <asp:Button ID="btnSubmit" runat="server" Height="21px" OnClick="btnSubmit_Click"
                        Text="Submit" />
                    <strong></strong>
                    <br />
                    <asp:UpdatePanel ID="UpPanelSubmit" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="qryView" runat="server" Width="449px" Height="84px">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <EditRowStyle BackColor="#FF3C3C" />
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
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    </td>
                    </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </asp:Panel>
    <br />
    &nbsp;
    <script language="javascript" type="text/javascript">
        function Toggle() {

        }

              
    
    </script>
</asp:Content>
