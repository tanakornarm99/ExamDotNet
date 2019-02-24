<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormExample.aspx.cs" Inherits="BasicCSharp.WebFormExample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.min.js"></script>
</head>
<body>
    <form runat="server">
        <div class="container">
            <asp:Label Text="Category Name :" runat="server" />
            <asp:Label ID="lblCategoryId" runat="server" Visible="false" />
            <asp:TextBox ID="txtCategory" runat="server" CssClass="col-sm-2" />
            <asp:Button ID="btnAddCategory" Text="Add Category" runat="server" CssClass="btn btn-primary" OnClick="AddCategory" />
            <asp:Button ID="btnUpdateCategory" Text="Update Category" runat="server" CssClass="btn btn-primary" OnClick="UpdateCategory" Visible="false" />
            <asp:Button ID="btnClearCategory" Text="Cancel" runat="server" CssClass="btn btn-danger" OnClick="ClearCategory" />
        </div>
        <div class="container">
            <asp:GridView ID="gvCategory" runat="server" DataKeyNames="Id" OnSelectedIndexChanged="GvCategory_Selected" OnRowDeleting="GvCategory_Delete">
                <Columns>
                    <asp:CommandField HeaderText="Update" ShowSelectButton="True" />
                    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
            <br />
        </div>

        <div class="container">
            <asp:Label Text="Item Name :" runat="server" />
            <asp:Label ID="lblItemId" runat="server" Visible="false" />
            <asp:TextBox ID="txtItemName" runat="server" CssClass="col-sm-2" />
            <asp:Label Text="Item Price :" runat="server" />
            <asp:TextBox ID="txtItemPrice" runat="server" CssClass="col-sm-2" />
            <asp:Label ID="lblDropList" Text="Item Category : " runat="server" />
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="col-sm-2" AppendDataBoundItems="true">
                <asp:ListItem Value="0" Text="Select Category" />
            </asp:DropDownList>
            <asp:Button ID="btnAddItem" Text="Add Item" runat="server" CssClass="btn btn-primary" OnClick="AddItem" />
            <asp:Button ID="btnUpdateItem" Text="Update Item" runat="server" CssClass="btn btn-primary" OnClick="UpdateItem" Visible="false" />
            <asp:Button ID="btnClearItem" Text="Cancel" runat="server" CssClass="btn btn-danger" OnClick="ClearItem" />
        </div>
        <div class="container">
            <asp:GridView ID="gvItem" runat="server" DataKeyNames="Id" OnSelectedIndexChanged="GvItem_Selected" OnRowDeleting="GvItem_Delete">
                <Columns>
                    <asp:CommandField HeaderText="Update" ShowSelectButton="True" />
                    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
            <br />
        </div>

        <div class="container">
            <asp:Label Text="GridView : Order List" runat="server" />
            <asp:GridView ID="gvOrder" runat="server" DataKeyNames="Id" OnSelectedIndexChanged="GvOrder_Selected" OnRowDeleting="GvOrder_Delete">
                <Columns>
                    <asp:CommandField HeaderText="Update" ShowSelectButton="true" />
                    <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
            <br />
        </div>
        <div class="container">
            <asp:Label ID="lblGvAddItem" Text="GridView : Add Item" runat="server" Visible="false" />
            <asp:GridView ID="gvAddOrderItem" runat="server" DataKeyNames="Id" OnSelectedIndexChanged="GvAddOrderItem_Selected">
                <Columns>
                    <asp:CommandField HeaderText="Select" ShowSelectButton="true" />
                </Columns>
            </asp:GridView>
            <br />
        </div>
        <div class="container">
            <asp:Label Text="Order No :" runat="server" />
            <asp:Label ID="lblOrderNo" runat="server" CssClass="col-sm-3" />
            <%--<asp:Label Text="Name :" runat="server" />
            <asp:TextBox ID="txtFirstname" runat="server" CssClass="col-sm-3" />--%>
            <asp:Button ID="btnAddOrder" Text="Add Order" runat="server" CssClass="btn btn-primary" OnClick="AddOrder" />
            <asp:Button ID="btnUpdateOrder" Text="Update Order" runat="server" CssClass="btn btn-primary" OnClick="UpdateOrder" Visible="false" />
            <asp:Button ID="btnClearOrder" Text="Cancel" runat="server" CssClass="btn btn-danger" OnClick="ClearOrder" /><br />
        </div>
        <div class="container">
            <asp:Label Text="Firstname :" runat="server" />
            <asp:TextBox ID="txtFirstname" runat="server" CssClass="col-sm-3" />
            <asp:Label Text="Surename :" runat="server" CssClass="col-sm-2" />
            <asp:TextBox ID="txtSurename" runat="server" CssClass="col-sm-3" /><br />
            <br />
        </div>
        <div class="container">
            <asp:Label Text="Contact No :" runat="server" />
            <asp:TextBox ID="txtContact" runat="server" CssClass="col-sm-3" />
            <asp:Label Text="E-mail :" runat="server" CssClass="col-sm-2" />
            <asp:TextBox ID="txtEmail" runat="server" CssClass="col-sm-3" /><br/><br/>
        </div>
        <div class="container">
            <asp:Button ID="btnAddOrderItem" Text="Add Item" runat="server" CssClass="btn btn-primary" OnClick="AddOrderItem_Click" Visible="false" />
            <asp:Label ID="lblTotaltxt" Text="Total :" runat="server" CssClass="col-sm-3" Visible="false" />
            <asp:Label ID="lblTotalPrice" runat="server" />
        </div>
        <div class="container">
            <asp:GridView ID="gvOrderItem" runat="server" DataKeyNames="Id,OrderId,ItemName,Qty" OnRowDeleting="GvOrderItem_Delete">
                <Columns>
                    <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
            <br />
        </div>

        <br />
    </form>
</body>
</html>
