<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Example.aspx.cs" Inherits="ExamDotNetCSharp.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnGetOrder" Text="Button" runat="server" OnClick="BtnGetOrder_Click" />
            <asp:Label  ID="lblShowMessage" Text="" runat="server" />
        </div>
        <div>
            <asp:GridView ID="tblOrder" runat="server"></asp:GridView>
        </div>
    </form>
</body>
</html>
