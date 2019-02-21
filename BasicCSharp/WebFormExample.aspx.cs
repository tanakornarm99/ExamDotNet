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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable getCategory = GetAllCategory();
                gvCategory.DataSource = getCategory;
                gvCategory.DataBind();

                DataTable getItem = GetAllItem();
                gvItem.DataSource = getItem;
                gvItem.DataBind();

                DataTable getOrder = GetAllOrder();
                gvOrder.DataSource = getOrder;
                gvOrder.DataBind();

                DataTable getCatId = GetAllCategory();
                ddlCategory.DataSource = getCatId;
                ddlCategory.DataValueField = "Id";
                ddlCategory.DataTextField = "Name";
                ddlCategory.DataBind();

                lblOrderNo.Text = GetOrderNumber(); //make orderNo.

            }
        }

        protected void AddCategory(object sender, EventArgs e)
        {
            string categoryName = txtCategory.Text;

            if (!string.IsNullOrEmpty(categoryName))
            {
                if (CategoryIsNotExist(categoryName.Trim()))
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO [Category] (Name) VALUES (@name)";
                        cmd.Parameters.AddWithValue("@name", categoryName);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            ClearCategory(sender, e);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void UpdateCategory(object sender, EventArgs e)
        {
            string categoryName = txtCategory.Text;

            if (!string.IsNullOrEmpty(categoryName))
            {
                string categoryId = lblCategoryId.Text;
                if (CategoryIsNotExist(categoryName.Trim()))
                {
                    string categoryOldName = GetCategoryName(categoryId);
                    UpdateCategoryOrderItem(categoryOldName, categoryName);

                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE [Category] SET Name = @name WHERE Id = @catId ";
                        cmd.Parameters.AddWithValue("@name", categoryName);
                        cmd.Parameters.AddWithValue("@catId", categoryId);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            ClearCategory(sender, e);
            btnAddCategory.Visible = true;
            btnUpdateCategory.Visible = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

      

        private void UpdateCategoryOrderItem(string cateOldName, string cateNewName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE [OrderItem] SET Category = @cateNewName WHERE Category = @cateOldName ";
                cmd.Parameters.AddWithValue("@cateNewName", cateNewName);
                cmd.Parameters.AddWithValue("@cateOldName", cateOldName);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void DeleteCategory(string catId)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM [Category] WHERE Id = @categoryId";
                cmd.Parameters.AddWithValue("@categoryId", catId);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void DeleteCategoryItem(string catId)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM [Item] WHERE CategoryId = @catId";
                cmd.Parameters.AddWithValue("@catId", catId);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void DeleteCategoryOrderItem(string categoryName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM [OrderItem] WHERE Category = @categoryName";
                cmd.Parameters.AddWithValue("@categoryName", categoryName);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        protected void AddItem(object sender, EventArgs e)
        {
            string itemName = txtItemName.Text;
            string itemPrice = txtItemPrice.Text;
            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

            if (!string.IsNullOrEmpty(itemName) && categoryId != 0)
            {
                if (ItemIsNotExist(itemName.Trim()))
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO [Item] (Name,Price,CategoryId) VALUES (@name,@price,@categoryId)";
                        cmd.Parameters.AddWithValue("@name", itemName);
                        cmd.Parameters.AddWithValue("@price", itemPrice);
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            ClearItem(sender, e);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void UpdateItem(object sender, EventArgs e)
        {
            string itemId = lblItemId.Text;
            string itemName = txtItemName.Text;
            string itemPrice = txtItemPrice.Text;
            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

            if (!string.IsNullOrEmpty(itemName) && categoryId != 0)
            {
                string itemOldName = GetItemName(itemId);
                UpdateItemNameOrderItem(itemOldName, itemName);

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE [Item] SET Name = @name, Price = @price, CategoryId = @catId WHERE Id = @itemId";
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@name", itemName);
                    cmd.Parameters.AddWithValue("@price", itemPrice);
                    cmd.Parameters.AddWithValue("@catId", categoryId);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            ClearItem(sender, e);
            btnAddItem.Visible = true;
            btnUpdateItem.Visible = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        private void UpdateItemNameOrderItem(string itemOldName, string itemNewName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE [OrderItem] SET ItemName = @itemNewName WHERE ItemName = @itemOldName ";
                cmd.Parameters.AddWithValue("@itemNewName", itemNewName);
                cmd.Parameters.AddWithValue("@itemOldName", itemOldName);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void DeleteItem(string itemId)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM [Item] WHERE Id = @itemId";
                cmd.Parameters.AddWithValue("@itemId", itemId);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void DeleteItemOrderItem(string itemName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM [OrderItem] WHERE ItemName = @itemName";
                cmd.Parameters.AddWithValue("@itemName", itemName);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        protected void AddOrder(object sender, EventArgs e)
        {
            double sumPrice = 0;
            string firstName = txtFirstname.Text;
            string orderNumber = lblOrderNo.Text;
            //string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string date = DateTime.Now.ToString();
            if (!string.IsNullOrEmpty(firstName))
            {
                if (ItemIsNotExist(firstName.Trim()))
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO [Order] (OrderNumber,[From],SummaryPrice,Date) VALUES (@orderNumber,@from,@sumPrice,@date)";
                        cmd.Parameters.AddWithValue("@orderNumber", orderNumber);
                        cmd.Parameters.AddWithValue("@from", firstName);
                        cmd.Parameters.AddWithValue("@sumPrice", sumPrice);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void UpdateOrder(object sender, EventArgs e)
        {
            int orderId = Convert.ToInt32(lblOrderId.Text);
            string orderNumber = lblOrderNo.Text;
            string name = txtFirstname.Text;
            double orderPrice = Convert.ToDouble(lblOrderPrice.Text);
            string date = DateTime.Now.ToString();

            if (!string.IsNullOrEmpty(name))
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE [Order] SET [From] = @name, Date = @date WHERE Id = @orderId ";
                    //cmd.CommandText = "UPDATE [Order] SET OrderNumber =@, From =@, SummaryPrice=@ WHERE Id = @orderId ";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        private void DeleteOrder(string orderID)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM [Order] WHERE [Id] = @orderID ";
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private int AddOrderItemExist(string orderID, string itemName, string itemPrice, string category)
        {
            int itemQty = 0, sumQty = 0;
            double sumPrice, priceItem = Convert.ToDouble(itemPrice); ;
            //double Price = Convert.ToDouble(itemPrice);
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [OrderItem] WHERE OrderId = @orderId AND ItemName = @itemName";
                cmd.Parameters.AddWithValue("@orderId", orderID);
                cmd.Parameters.AddWithValue("@itemName", itemName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    itemQty = Convert.ToInt32(dt.Rows[0]["Qty"].ToString());
                    sumQty = itemQty + 1;
                    sumPrice = priceItem * sumQty;
                    UpdateOrderItem(orderID, sumQty, sumPrice, itemName);
                }
                else
                {
                    AddNewOrderItem(orderID, itemName, itemPrice, category);
                }
            }
            return 0;
        }

        private int AddNewOrderItem(string orderID, string itemName, string itemPrice, string category)
        {
            int qty = 1;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO [OrderItem] (OrderId,ItemName,Category,Qty,Price) VALUES (@orderID,@itemName,@category,@qty,@price) ";
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@itemName", itemName);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@price", itemPrice);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return 0;
        }

        private void UpdateOrderItem(string orderID, double sumQty, double sumPrice, string itemName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE [OrderItem] SET Qty = @sumQty, Price = @sumPrice WHERE OrderId = @orderID AND ItemName = @itemName";
                cmd.Parameters.AddWithValue("@sumQty", sumQty);
                cmd.Parameters.AddWithValue("@sumPrice", sumPrice);
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@itemName", itemName);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void DeleteOrderItemID(string orderID)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM [OrderItem] WHERE [OrderId] = @orderID ";
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void DeleteOrderItemName(string orderID, string itemName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM [OrderItem] WHERE [OrderId] = @orderID AND [ItemName] = @itemName";
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@itemName", itemName);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void UpdateTotalPrice(string orderID)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            numberFormatInfo.CurrencySymbol = "฿"; // Replace with "$" or "£" or whatever you need
            var sumPrice = GetTotalItemPrice(orderID);  //Method GetTotalItemPrice sumPrice
            var formattedPrice = sumPrice.ToString("C", numberFormatInfo);
            lblTotalPrice.Text = formattedPrice; //show ฿xxx.xx
            UpdateOrderSumPrice(orderID, sumPrice); //Method UpdateOrderSumPrice
        }

        private void UpdateOrderSumPrice(string orderID, double sumPrice)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE [Order] SET [SummaryPrice] = @sumPrice WHERE [Id] = @orderID";
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@sumPrice", sumPrice);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            DataTable getOrder = GetAllOrder();
            gvOrder.DataSource = getOrder;
            gvOrder.DataBind();
        }

        private DataTable GetAllItem()
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [Item],[Category] WHERE Item.CategoryId = Category.Id";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private DataTable GetAllCategory()
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [Category]";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private DataTable GetAllOrder()
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [Order]";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private DataTable GetAllOrderItem()
        {
            int orderId = Convert.ToInt32(lblOrderId.Text);

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [OrderItem] WHERE [OrderItem].orderId = @orderId";
                cmd.Parameters.AddWithValue("@orderId", orderId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private string GetOrderNumber()
        {
            int length = 4;
            string orderNo = string.Empty;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT TOP 1 OrderNumber FROM [Order] ORDER BY ID DESC";
                orderNo = cmd.ExecuteScalar().ToString();
            }
            string subOrderNo = orderNo.Substring(orderNo.Length - 4);
            int numberOrder = Convert.ToInt32(subOrderNo) + 1;
            var result = numberOrder.ToString().PadLeft(length, '0');
            DateTime thisDay = DateTime.Today;
            string resultOrderNumber = thisDay.ToString("ORD" + "yyyyMM" + result);
            return resultOrderNumber;
        }

        private string GetCategoryName(string catId)
        {
            string categoryName = string.Empty;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select Name From [Category] Where Id = @catId";
                cmd.Parameters.AddWithValue("@catId", catId);
                categoryName = cmd.ExecuteScalar().ToString();
            }
            return categoryName;
        }

        private string GetItemName(string itemId)
        {
            string itemName = string.Empty;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select Name From [Item] Where Id = @itemId";
                cmd.Parameters.AddWithValue("@itemId", itemId);
                itemName = cmd.ExecuteScalar().ToString();
            }
            return itemName;
        }

        private string GetItemPrice(string itemName)
        {
            string itemPrice = string.Empty;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT [Price] FROM [Item] WHERE Name = @itemName";
                cmd.Parameters.AddWithValue("@itemName", itemName);
                itemPrice = cmd.ExecuteScalar().ToString();
            }
            return itemPrice;
        }

        private double GetTotalItemPrice(string orderID)
        {
            string totalPrice = string.Empty;
            double sumPrice = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT sum(Price) FROM [OrderItem] WHERE OrderId = @orderID ";
                cmd.Parameters.AddWithValue("@orderID", orderID);
                totalPrice = cmd.ExecuteScalar().ToString();
                if (!string.IsNullOrEmpty(totalPrice))
                {
                    sumPrice = Convert.ToDouble(totalPrice);
                }
            }
            return sumPrice;
        }

        private bool CategoryIsNotExist(string categoryName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Name FROM [Category] WHERE Name = @CategoryName";
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        private bool ItemIsNotExist(string itemName)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Name FROM [Item] WHERE Name = @itemName";
                cmd.Parameters.AddWithValue("@itemName", itemName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
            }
            return true;
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
            //ddlCategory.SelectedValue = ddlCategory.SelectedItem.Value;
            ddlCategory.SelectedValue = row.Cells[5].Text;
            ddlCategory.SelectedItem.Value = ddlCategory.SelectedValue;
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
            lblOrderId.Text = row.Cells[2].Text;
            lblOrderNo.Text = row.Cells[3].Text;
            txtFirstname.Text = row.Cells[4].Text;
            lblOrderPrice.Text = row.Cells[5].Text;

            UpdateTotalPrice(lblOrderId.Text);

            DataTable getOrderItem = GetAllOrderItem();
            gvOrderItem.DataSource = getOrderItem;
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
            string orderID = lblOrderId.Text;
            string itemName = row.Cells[2].Text;
            string itemPrice = row.Cells[3].Text;
            string category = row.Cells[6].Text;

            AddOrderItemExist(orderID, itemName, itemPrice, category);
            UpdateTotalPrice(orderID);

            DataTable getOrderItem = GetAllOrderItem();
            gvOrderItem.DataSource = getOrderItem;
            gvOrderItem.DataBind();
        }

        protected void GvOrderItem_Delete(object sender, EventArgs e)
        {
            GridViewRow row = gvOrderItem.SelectedRow;
            string orderID = row.Cells[2].Text;
            string itemName = row.Cells[3].Text;
            string qty = row.Cells[5].Text;
            int sumQty = Convert.ToInt32(qty);
            double sumPrice, itemPrice = 0;

            if (sumQty > 1)
            {
                itemPrice = Convert.ToDouble(GetItemPrice(itemName));
                sumQty = sumQty - 1;
                sumPrice = sumQty * itemPrice;
                UpdateOrderItem(orderID, sumQty, sumPrice, itemName);
            }
            else
            {
                DeleteOrderItemName(orderID, itemName);
            }

            UpdateTotalPrice(orderID);

            DataTable getOrderItem = GetAllOrderItem();
            gvOrderItem.DataSource = getOrderItem;
            gvOrderItem.DataBind();
        }

        protected void AddOrderItem_Click(object sender, EventArgs e)
        {
            lblGvAddItem.Visible = true;
            DataTable getItem = GetAllItem();
            gvAddOrderItem.DataSource = getItem;
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




    }
}