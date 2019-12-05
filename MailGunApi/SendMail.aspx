<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMail.aspx.cs" Inherits="MailGunApi.SendMail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-group">
            <label>Subject</label>
            <asp:TextBox runat="server" ID="txtSub" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <label>Message</label>
            <asp:TextBox runat="server" ID="txtMessage" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
        </div>
        <asp:GridView ID="grdview" runat="server" AutoGenerateColumns="false">
             <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="30" />
        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="150" />
                 <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="150" />
       <%-- <asp:TemplateField HeaderText="Email">
            <ItemTemplate>
                <asp:HyperLink ID="lnkEmail" runat="server" Text='<%# Eval("Email") %>' NavigateUrl='<%# Eval("Email", "mailto:{0}") %>' />
            </ItemTemplate>
        </asp:TemplateField>--%>
    </Columns>
        </asp:GridView>
         <asp:FileUpload ID="FileUpload1"  CssClass="button"  runat="server" />  
    <asp:Button ID="btnImport" CssClass="button" runat="server" Text="Import" OnClick="ImportCSV" />  
        <div>
            <asp:Button runat="server" ID="btnSend" OnClick="btnSend_Click" Text="Send"/>
        </div>
    </form>
</body>
</html>
