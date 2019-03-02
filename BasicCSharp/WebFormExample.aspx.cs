using BasicCSharp.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BasicCSharp
{
    public partial class WebFormExample : System.Web.UI.Page
    {
        private string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        private string CONTSTANT_OrderID = string.Empty;
        private string CONTSTANT_CatID = string.Empty;

        private StockLogic _stockLogic;
        private OrderLogic _orderLogic;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _stockLogic = new StockLogic(conString);
                _orderLogic = new OrderLogic(conString);

                DataTable dtCategory = _stockLogic.GetAllCategory();
                ddlCategory.DataSource = dtCategory;
                ddlCategory.DataValueField = "Id";
                ddlCategory.DataTextField = "Name";
                ddlCategory.DataBind();
                gvCategory.DataSource = dtCategory;
                gvCategory.DataBind();

                DataTable dtItem = _stockLogic.GetAllItem();
                gvItem.DataSource = dtItem;
                gvItem.DataBind();

                _orderLogic.UpdateOrderSumPriceAll();

                DataTable dtOrder = _orderLogic.GetAllOrder();
                gvOrder.DataSource = dtOrder;
                gvOrder.DataBind();

                lblOrderNo.Text = _orderLogic.GetOrderNumber(); //make orderNo.
            }
        }

        protected void AddCategory(object sender, EventArgs e)
        {
            string categoryName = txtCategory.Text;
            _stockLogic.AddCategory(categoryName);
            ClearCategory(sender, e);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void UpdateCategory(object sender, EventArgs e)
        {
            string categoryName = txtCategory.Text;
            string categoryId = lblCategoryId.Text;
            _stockLogic.UpdateCategory(categoryName, categoryId);
            ClearCategory(sender, e);
            btnAddCategory.Visible = true;
            btnUpdateCategory.Visible = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void AddItem(object sender, EventArgs e)
        {
            string itemName = txtItemName.Text;
            string itemPrice = txtItemPrice.Text;
            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            _stockLogic.AddItem(itemName, itemPrice, categoryId);
            ClearItem(sender, e);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void UpdateItem(object sender, EventArgs e)
        {
            string itemId = lblItemId.Text;
            string itemName = txtItemName.Text;
            string itemPrice = txtItemPrice.Text;
            string categoryId = ddlCategory.SelectedValue.ToString();
            _stockLogic.UpdateItem(CONTSTANT_CatID, itemId,itemName, itemPrice, categoryId);
            ClearItem(sender, e);
            btnAddItem.Visible = true;
            btnUpdateItem.Visible = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

       

        private void UpdateItemNameOrderItem(string itemOldName, string itemNewName)
        {
            string cmdText = "UPDATE [OrderItem] SET ItemName = @itemNewName WHERE ItemName = @itemOldName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("itemNewName", itemNewName));
            parameters.Add(SetParam("itemOldName", itemOldName));
            ExecuteQuery(cmdText, parameters);
        }

        private void DeleteItem(string itemId)
        {
            string cmdText = "DELETE FROM [Item] WHERE Id = @itemId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("itemId", itemId));
            ExecuteQuery(cmdText, parameters);
        }

        private void DeleteItemOrderItem(string itemName)
        {
            string cmdText = "DELETE FROM [OrderItem] WHERE ItemName = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("itemName", itemName));
            ExecuteQuery(cmdText, parameters);
        }

        protected void AddOrder(object sender, EventArgs e)
        {
            double sumPrice = 0;
            string date = DateTime.Now.ToString();
            string orderNumber = lblOrderNo.Text;
            string firstName = txtFirstName.Text.Trim();
            string sureName = txtSureName.Text.Trim();
            string contact = txtContact.Text.Trim();
            string email = txtEmail.Text.Trim();
            string nameFrom = firstName + " " + sureName;
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(sureName))
            {
                string cmdText = "INSERT INTO [Order] (OrderNumber,[From],SummaryPrice,Date) VALUES (@orderNumber,@from,@sumPrice,@date)";
                List<Param> parameters = new List<Param>();
                parameters.Add(SetParam("orderNumber", orderNumber));
                parameters.Add(SetParam("from", nameFrom));
                parameters.Add(SetParam("sumPrice", sumPrice));
                parameters.Add(SetParam("date", date));
                ExecuteQuery(cmdText, parameters);

                //string oderId = GetOrderId(orderNumber);
                //paramety.Add(SetParam("orderId", orderId));

                string cmd = "INSERT INTO [Customer] (Firstname,Surename,Contact,Email) VALUES (@firstName,@sureName,@contact,@email)";
                List<Param> paramety = new List<Param>();
                paramety.Add(SetParam("firstName", firstName));
                paramety.Add(SetParam("sureName", sureName));
                paramety.Add(SetParam("contact", contact));
                paramety.Add(SetParam("email", email));
                ExecuteQuery(cmd, paramety);
            }
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void UpdateOrder(object sender, EventArgs e)
        {
            string orderId = CONTSTANT_OrderID;
            string date = DateTime.Now.ToString();
            string name = txtFirstName.Text;
            if (!string.IsNullOrEmpty(name))
            {
                string cmdText = "UPDATE [Order] SET [From] = @name, Date = @date WHERE Id = @orderId";
                List<Param> parameters = new List<Param>();
                parameters.Add(SetParam("name", name));
                parameters.Add(SetParam("date", date));
                parameters.Add(SetParam("orderId", orderId));
                ExecuteQuery(cmdText, parameters);
            }
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        private void ShowTotalPrice(string orderId)
        {
            var sumPrice = GetTotalItemPrice(orderId);
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            numberFormatInfo.CurrencySymbol = "฿"; // Replace with "$" or "£" or whatever you need
            var formattedPrice = sumPrice.ToString("C", numberFormatInfo);
            lblTotalPrice.Text = formattedPrice; //show ฿xxx.xx
        }


        private void DeleteOrder(string orderId)
        {
            string cmdText = "DELETE FROM [Order] WHERE [Id] = @orderID";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("orderId", orderId));
            ExecuteQuery(cmdText, parameters);
        }

        private void AddOrderItemExist(string orderId, string itemName, string itemPrice, string category)
        {
            int itemQty = 0, sumQty = 0;
            double sumPrice, priceItem = Convert.ToDouble(itemPrice); ;
            string cmdText = "SELECT * FROM [OrderItem] WHERE OrderId = @orderId AND ItemName = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("orderId", orderId));
            parameters.Add(SetParam("itemName", itemName));
            DataTable dt = ExecuteQueryWithResult(cmdText, parameters);
            if (dt.Rows.Count > 0)
            {
                itemQty = Convert.ToInt32(dt.Rows[0]["Qty"]);
                sumQty = itemQty + 1;
                sumPrice = priceItem * sumQty;
                UpdateOrderItem(orderId, sumQty, sumPrice, itemName);
            }
            else
            {
                AddNewOrderItem(orderId, itemName, itemPrice, category);
            }
        }

        private void AddNewOrderItem(string orderId, string itemName, string itemPrice, string category)
        {
            int qty = 1;
            string cmdText = "INSERT INTO [OrderItem] (OrderId,ItemName,Category,Qty,Price) VALUES (@orderId,@itemName,@category,@qty,@price)";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("orderId", orderId));
            parameters.Add(SetParam("itemName", itemName));
            parameters.Add(SetParam("category", category));
            parameters.Add(SetParam("qty", qty));
            parameters.Add(SetParam("price", itemPrice));
            ExecuteQuery(cmdText, parameters);
        }

        private void UpdateOrderItem(string orderId, double sumQty, double sumPrice, string itemName)
        {
            string cmdText = "UPDATE [OrderItem] SET Qty = @sumQty, Price = @sumPrice WHERE OrderId = @orderId AND ItemName = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("sumQty", sumQty));
            parameters.Add(SetParam("sumPrice", sumPrice));
            parameters.Add(SetParam("orderId", orderId));
            parameters.Add(SetParam("itemName", itemName));
            ExecuteQuery(cmdText, parameters);
        }

        private void DeleteOrderItemID(string orderId)
        {
            string cmdText = "DELETE FROM [OrderItem] WHERE [OrderId] = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("orderId", orderId));
            ExecuteQuery(cmdText, parameters);
        }

        private void DeleteOrderItemName(string orderItemId)
        {
            string cmdText = "DELETE FROM [OrderItem] WHERE [Id] = @orderItemId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("orderItemId", orderItemId));
            ExecuteQuery(cmdText, parameters);
        }
 
        private DataTable GetAllOrderItem(string orderId)
        {
            string cmdText = "SELECT * FROM [OrderItem] WHERE OrderId = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("orderId", orderId));
            return ExecuteQueryWithResult(cmdText, parameters);
        }

       

        private string GetItemPrice(string itemName)
        {
            string cmdText = "SELECT [Price] FROM [Item] WHERE Name = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("itemName", itemName));
            return ExecuteQueryScalar(cmdText, parameters).ToString();
        }

        //EventButton Selete Value
        protected void GvCategory_Selected(object sender, EventArgs e)
        {
            GridViewRow row = gvCategory.SelectedRow;
            lblCategoryId.Text = row.Cells[2].Text;
            txtCategory.Text = row.Cells[3].Text;
            btnAddCategory.Visible = false;
            btnUpdateCategory.Visible = true;
        }

        protected void GvCategory_Delete(object sender, GridViewDeleteEventArgs e)
        {
            string catId = gvCategory.DataKeys[e.RowIndex].Value.ToString();
            string categoryName = GetCategoryName(catId);
            DeleteCategoryOrderItem(categoryName);
            DeleteCategoryItem(catId);
            DeleteCategory(catId);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void GvItem_Selected(object sender, EventArgs e)
        {
            GridViewRow row = gvItem.SelectedRow;
            lblItemId.Text = row.Cells[2].Text;
            txtItemName.Text = row.Cells[3].Text;
            txtItemPrice.Text = row.Cells[4].Text;
            ddlCategory.SelectedValue = row.Cells[5].Text;
            ddlCategory.SelectedItem.Value = ddlCategory.SelectedValue;
            CONTSTANT_CatID = ddlCategory.SelectedValue; // SET Global variable
            btnAddItem.Visible = false;
            btnUpdateItem.Visible = true;
        }

        protected void GvItem_Delete(object sender, GridViewDeleteEventArgs e)
        {
            string itemId = gvItem.DataKeys[e.RowIndex].Value.ToString();
            string itemName = GetItemName(itemId);
            DeleteItemOrderItem(itemName);
            DeleteItem(itemId);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void GvOrder_Selected(object sender, EventArgs e)
        {
            GridViewRow row = gvOrder.SelectedRow;
            string orderId = row.Cells[2].Text;
            CONTSTANT_OrderID = orderId;
            lblOrderNo.Text = row.Cells[3].Text;
            string strName = row.Cells[4].Text;
            string[] strSpName = strName.Split(' ');
            txtFirstName.Text = strSpName[0];
            //txtSureName.Text = strSpName[1];

            ShowTotalPrice(orderId);

            DataTable dtOrderItem = GetAllOrderItem(orderId);
            gvOrderItem.DataSource = dtOrderItem;
            gvOrderItem.DataBind();

            btnAddOrder.Visible = false;
            btnUpdateOrder.Visible = true;
            btnAddOrderItem.Visible = true;
            lblTotaltxt.Visible = true;
        }

        protected void GvOrder_Delete(object sender, GridViewDeleteEventArgs e)
        {
            string orderId = gvOrder.DataKeys[e.RowIndex].Value.ToString();
            DeleteOrderItemID(orderId);
            DeleteOrder(orderId);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void GvAddOrderItem_Selected(object sender, EventArgs e)
        {
            GridViewRow row = gvAddOrderItem.SelectedRow;
            string orderId = CONTSTANT_OrderID;
            string itemName = row.Cells[2].Text;
            string itemPrice = row.Cells[3].Text;
            string category = row.Cells[6].Text;
            AddOrderItemExist(orderId, itemName, itemPrice, category);
            ShowTotalPrice(orderId);

            DataTable dtOrderItem = GetAllOrderItem(orderId);
            gvOrderItem.DataSource = dtOrderItem;
            gvOrderItem.DataBind();
        }

        protected void GvOrderItem_Delete(object sender, GridViewDeleteEventArgs e)
        {
            string orderItemId = gvOrderItem.DataKeys[e.RowIndex].Values[0].ToString();
            string orderId = gvOrderItem.DataKeys[e.RowIndex].Values[1].ToString();
            string itemName = gvOrderItem.DataKeys[e.RowIndex].Values[2].ToString();
            string qty = gvOrderItem.DataKeys[e.RowIndex].Values[3].ToString();
            int sumQty = Convert.ToInt32(qty);
            double sumPrice, itemPrice = 0;
            if (sumQty > 1)
            {
                itemPrice = Convert.ToDouble(GetItemPrice(itemName));
                sumQty = sumQty - 1;
                sumPrice = sumQty * itemPrice;
                UpdateOrderItem(orderId, sumQty, sumPrice, itemName);
            }
            else
            {
                DeleteOrderItemName(orderItemId);
            }
            ShowTotalPrice(orderId);
            DataTable dtOrderItem = GetAllOrderItem(orderId);
            gvOrderItem.DataSource = dtOrderItem;
            gvOrderItem.DataBind();
        }

        protected void AddOrderItem_Click(object sender, EventArgs e)
        {
            lblGvAddItem.Visible = true;
            DataTable dtItem = _stockLogic.GetAllItem();
            gvAddOrderItem.DataSource = dtItem;
            gvAddOrderItem.DataBind();
        }

        protected void ClearCategory(object sender, EventArgs e)
        {
            lblCategoryId.Text = string.Empty;
            txtCategory.Text = string.Empty;
            btnAddCategory.Visible = true;
            btnUpdateCategory.Visible = false;
        }

        protected void ClearItem(object sender, EventArgs e)
        {
            int temp = 0;
            lblItemId.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtItemPrice.Text = string.Empty;
            ddlCategory.SelectedValue = temp.ToString();
            btnAddItem.Visible = true;
            btnUpdateItem.Visible = false;
        }

        protected void ClearOrder(object sender, EventArgs e)
        {
            btnAddOrderItem.Visible = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

       
    } //End class WebFormExample

  
}//End