<%@ Page Title="Edit" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 314px;
        }
        .style3
        {
            width: 314px;
            font-weight: bold;
            color: #000000;
        }
        .style4
        {
            font-weight: bold;
            color: #000000;
        }
        .style5
        {
            width: 314px;
            font-weight: bold;
            color: #000000;
            background-color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
<ProfileService  Path="~/Edit.aspx"/>
</asp:ScriptManagerProxy>
<asp:ToolkitScriptManager ID="ScriptManager1" runat="server"   >
    </asp:ToolkitScriptManager>
    <script>
        function CloseMe() {
            window.close();
        }
    </script>
    <table style="width: 100%;">
        <tr>
            <td class="style3">
                <b>Date:&nbsp;&nbsp; </b> <asp:TextBox ID="txtDate" runat="server" CssClass="bold" 
                    Height="27px" Width="235px"></asp:TextBox>
            </td>
            <td>
               
               
            </td>
            <td>
                
            </td>
        </tr>
        <tr>
            <td class="style3">
                <b>Client: </b> 
                <asp:TextBox ID="txtClient" runat="server" CssClass="bold" 
                    Height="27px" Width="235px" ></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
           <tr>
            <td class="style3">
                <b>Work Description: </b> <asp:TextBox ID="txtWorkDesc" runat="server" CssClass="bold" 
                    Height="27px" Width="235px"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style3">
                <b>Work Hours: </b> 
                <asp:TextBox ID="txtHours" runat="server" CssClass="bold"
                Height="27px" Width="78px"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
             <tr>
            <td class="style3">
                <b>Comments: 
                <br />
                </b> <asp:TextBox ID="txtComments" runat="server" TextMode=MultiLine 
                    CssClass="style4" Width="301px"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
                <tr>
            <td class="style3">
                Payment Received: <asp:CheckBox ID="cbPayment" runat="server" />
               
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
                   <tr>
            <td class="style2">
                
                <asp:UpdatePanel ID="upPanelEdit" runat="server" ChildrenAsTriggers ="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnUpdate" runat="server" onclick="btnUpdate_Click" 
                            Text="Update Record" Width="117px" />
                            <asp:UpdateProgress AssociatedUpdatePanelID ="upPanelEdit" ID="upProgEmail" runat="server">
                             <ProgressTemplate>
                             <span class="style5">Updating.....</span>
                             </ProgressTemplate>
                            </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
                      
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>

