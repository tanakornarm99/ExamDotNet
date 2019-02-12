<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Example.aspx.cs" Inherits="BasicCSharp.Example" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="BtnClick" Text="Button" runat="server" Onclick="BtnClick_Click"/>
        </div>
        <div>
            <asp:Label Text="Text File : " runat="server" />
            <asp:Label ID="lblTextFile" runat="server" />
        </div>
        <div>
            <asp:Label Text="Text Reverse : " runat="server" />
            <asp:Label ID="lblTextReverse" runat="server" />
        </div>
        <div>
            <asp:Label Text="Text Palindrome : " runat="server" />
            <asp:Label ID="lblResult" runat="server" />
        </div>
    </form>
</body>
</html>
