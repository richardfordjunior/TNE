<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UpdateRecord.aspx.cs" Inherits="UpdateRecord" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
    <h2>Update Record</h2>
    <table style="width: 100%;">
        <tr>
            <td class="style2">
                User</td>
            
            <td>
                &nbsp;
                <asp:TextBox ID="txtUser" runat="server" Height="22px" Width="195px"></asp:TextBox>
            </td>
            <td>
            
               
            </td>
        </tr>
        <tr>
            <td class="style2">
               Role
            </td>
            <td>
                &nbsp;
                <asp:TextBox ID="txtRole" runat="server" Height="22px" Width="195px"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                Email Address
            </td>
            <td>
                &nbsp;
                <asp:TextBox ID="txtEmail" runat="server" Height="22px" Width="195px"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
          <tr>
            <td class="style2">
               
            </td>
            <td>
                <asp:Button ID="btnSubmitChanges" runat="server" Text="Update" 
                    onclick="btnSubmitChanges_Click" />
                &nbsp;</td>
             
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>

