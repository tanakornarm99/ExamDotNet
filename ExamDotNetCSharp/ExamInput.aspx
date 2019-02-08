<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamInput.aspx.cs" Inherits="ExamDotNetCSharp.ExamInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label Text="Prime factors :" runat="server" />
            <asp:TextBox ID="txtInputNumber" runat="server" />
            <asp:Button ID="btnSummit" Text="Submit" runat="server" Onclick="BtnSummit_Click"/>
        </div>
        <div>
            <asp:Label ID="lblResult" Text="" runat="server" />
        </div>
    </form>
</body>
</html>
