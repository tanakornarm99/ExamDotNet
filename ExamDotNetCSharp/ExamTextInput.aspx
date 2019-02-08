<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamTextInput.aspx.cs" Inherits="ExamDotNetCSharp.ExamTextInput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnSbmt" Text="Submit" runat="server" OnClick="btnSbmt_Click"/>
        </div>
         <div>
            <asp:Label ID="lblShowtext" Text="" runat="server" />
        </div>
        <div>
            <asp:Label ID="lblShowMessagetxt" Text="" runat="server" />
        </div>
    </form>
</body>
</html>
