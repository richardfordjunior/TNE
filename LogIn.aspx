<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LogIn.aspx.cs" Inherits="LogIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

<asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" >

</asp:Login>
<br />
<asp:HiddenField ID ="hfUser" runat = "server" />
 
    
  
 
<br />
 
    
  
 
</asp:Content>
