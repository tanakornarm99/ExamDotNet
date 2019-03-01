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
        public static string CONTSTANT_OrderID = string.Empty;
        public static string CONTSTANT_CatID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtCategory = GetAllCategory();
                ddlCategory.DataSource = dtCategory;
                ddlCategory.DataValueField = "Id";
                ddlCategory.DataTextField = "Name";
                ddlCategory.DataBind();
                gvCategory.DataSource = dtCategory;
                gvCategory.DataBind();

                DataTable dtItem = GetAllItem();
                gvItem.DataSource = dtItem;
                gvItem.DataBind();

                UpdateOrderSumPriceAll();
                DataTable dtOrder = GetAllOrder();
                gvOrder.DataSource = dtOrder;
                gvOrder.DataBind();

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
                    string cmdText = "INSERT INTO [Category] (Name) VALUES (@name)";
                    List<Param> parameters = new List<Param>();
                    parameters.Add(SetParam("name", categoryName));
                    ExecuteQuery(cmdText, parameters);
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
                    string cmdText = "UPDATE [Category] SET Name = @name WHERE Id = @catId";
                    List<Param> parameters = new List<Param>();
                    parameters.Add(SetParam("name", categoryName));
                    parameters.Add(SetParam("catId", categoryId));
                    ExecuteQuery(cmdText, parameters);
                }
            }
            ClearCategory(sender, e);
            btnAddCategory.Visible = true;
            btnUpdateCategory.Visible = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        private void UpdateCategoryOrderItem(string cateOldName, string cateNewName)
        {
            string cmdText = "UPDATE [OrderItem] SET Category = @cateNewName WHERE Category = @cateOldName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("cateNewName", cateNewName));
            parameters.Add(SetParam("cateOldName", cateOldName));
            ExecuteQuery(cmdText, parameters);
        }

        private void DeleteCategory(string catId)
        {
            string cmdText = "DELETE FROM [Category] WHERE Id = @categoryId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("categoryId", catId));
            ExecuteQuery(cmdText, parameters);
        }

        private void DeleteCategoryItem(string catId)
        {
            string cmdText = "DELETE FROM [Item] WHERE CategoryId = @catId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("catId", catId));
            ExecuteQuery(cmdText, parameters);
        }

        private void DeleteCategoryOrderItem(string categoryName)
        {
            string cmdText = "DELETE FROM [OrderItem] WHERE Category = @categoryName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("categoryName", categoryName));
            ExecuteQuery(cmdText, parameters);
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
                    string cmdText = "INSERT INTO [Item] (Name,Price,CategoryId) VALUES (@name,@price,@categoryId)";
                    List<Param> parameters = new List<Param>();
                    parameters.Add(SetParam("name", itemName));
                    parameters.Add(SetParam("price", itemPrice));
                    parameters.Add(SetParam("categoryId", categoryId));
                    ExecuteQuery(cmdText, parameters);
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
            string categoryId = ddlCategory.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(itemName) && categoryId != null)
            {
                string itemOldName = GetItemName(itemId);
                string categoryNewName = GetCategoryName(categoryId);
                string categoryOldName = GetCategoryName(CONTSTANT_CatID); //Set CONTSTANT_CatID from Method GvItem_Selected 
                UpdatePriceOrderItem(itemName, itemPrice);
                UpdateItemNameOrderItem(itemOldName, itemName);
                UpdateCategoryOrderItem(categoryOldName, categoryNewName);
                string cmdText = "UPDATE [Item] SET Name = @name, Price = @price, CategoryId = @catId WHERE Id = @itemId";
                List<Param> parameters = new List<Param>();
                parameters.Add(SetParam("itemId", itemId));
                parameters.Add(SetParam("name", itemName));
                parameters.Add(SetParam("price", itemPrice));
                parameters.Add(SetParam("catId", categoryId));
                ExecuteQuery(cmdText, parameters);
            }
            ClearItem(sender, e);
            btnAddItem.Visible = true;
            btnUpdateItem.Visible = false;
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        private void UpdatePriceOrderItem(string itemName, string itemPrice)
        {
            //GetAllQtyOrderItem
            string paraPrice = string.Empty;
            string cmdText = "SELECT * FROM [OrderItem] WHERE ItemName = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("itemName", itemName));
            DataTable dt = ExecuteQueryWithResult(cmdText, parameters);
            double newPrice = Convert.ToDouble(itemPrice);
            string[] arryId = new string[dt.Rows.Count];
            double[] arryQty = new double[dt.Rows.Count];
            double[] arrySumPrice = new double[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arryId[i] = dt.Rows[i]["Id"].ToString();
                arryQty[i] = Convert.ToInt32(dt.Rows[i]["Qty"]);
                arrySumPrice[i] = arryQty[i] * newPrice;
                string cmd = "UPDATE [OrderItem] SET Price = @newPrice WHERE ItemName = @itemName AND Id = @arryId";
                List<Param> paramety = new List<Param>();
                paramety.Add(SetParam("newPrice", arrySumPrice[i]));
                paramety.Add(SetParam("itemName", itemName));
                paramety.Add(SetParam("arryId", arryId[i]));
                ExecuteQuery(cmd, paramety);
            }
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

        private void UpdateOrderSumPriceAll()
        {
            DataTable dt = GetAllOrder();
            string[] arryOrderId = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arryOrderId[i] = dt.Rows[i]["Id"].ToString();
                var sumPrice = GetTotalItemPrice(arryOrderId[i]);
                string cmdText = "UPDATE [Order] SET [SummaryPrice] = @sumPrice WHERE [Id] = @orderId";
                List<Param> parameters = new List<Param>();
                parameters.Add(SetParam("orderId", arryOrderId[i]));
                parameters.Add(SetParam("sumPrice", sumPrice));
                ExecuteQuery(cmdText, parameters);
            }
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

        private DataTable GetAllItem()
        {
            string cmdText = "SELECT * FROM [Item],[Category] WHERE Item.CategoryId = Category.Id";
            return ExecuteQueryWithResult(cmdText, new List<Param>());
        }

        private DataTable GetAllCategory()
        {
            string cmdText = "SELECT * FROM [Category]";
            return ExecuteQueryWithResult(cmdText, new List<Param>());
        }

        private DataTable GetAllOrder()
        {
            string cmdText = "SELECT * FROM [Order]";
            return ExecuteQueryWithResult(cmdText, new List<Param>());
        }

        private DataTable GetAllOrderItem(string orderId)
        {
            string cmdText = "SELECT * FROM [OrderItem] WHERE OrderId = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("orderId", orderId));
            return ExecuteQueryWithResult(cmdText, parameters);
        }

        private string GetOrderNumber()
        {
            int length = 4;
            DateTime thisDay = DateTime.Today;
            string orderNo = "ORD2019000000";
            DataTable dtOrder = GetAllOrder();
            if (dtOrder.Rows.Count > 0)
            {
                string cmdText = "SELECT TOP 1 OrderNumber FROM [Order] ORDER BY ID DESC";
                orderNo = ExecuteQueryScalar(cmdText, new List<Param>()).ToString();
            }
            int subOrderNo = Convert.ToInt32(orderNo.Substring(orderNo.Length - 4)) + 1;
            var result = subOrderNo.ToString().PadLeft(length, '0');
            string resultOrderNumber = thisDay.ToString("ORD" + "yyyyMM" + result);
            return resultOrderNumber;
        }

        private string GetCategoryName(string catId)
        {
            string cmdText = "SELECT Name FROM [Category] WHERE Id = @catId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("catId", catId));
            return ExecuteQueryScalar(cmdText, parameters).ToString();
        }

        private string GetItemName(string itemId)
        {
            string cmdText = "SELECT Name FROM [Item] WHERE Id = @itemId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("itemId", itemId));
            return ExecuteQueryScalar(cmdText, parameters).ToString();
        }

        private string GetItemPrice(string itemName)
        {
            string cmdText = "SELECT [Price] FROM [Item] WHERE Name = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("itemName", itemName));
            return ExecuteQueryScalar(cmdText, parameters).ToString();
        }

        private double GetTotalItemPrice(string orderId)
        {
            string totalPrice = string.Empty;
            double sumPrice = 0;
            string cmdText = "SELECT sum(Price) FROM [OrderItem] WHERE OrderId = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("orderId", orderId));
            totalPrice = ExecuteQueryScalar(cmdText, parameters).ToString();
            if (!string.IsNullOrEmpty(totalPrice))
            {
                sumPrice = Convert.ToDouble(totalPrice);
            }
            return sumPrice;
        }

        private bool CategoryIsNotExist(string categoryName)
        {
            string cmdText = "SELECT Name FROM [Category] WHERE Name = @categoryName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("categoryName", categoryName));
            DataTable dt = ExecuteQueryWithResult(cmdText, parameters);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        private bool ItemIsNotExist(string itemName)
        {
            string cmdText = "SELECT Name FROM [Item] WHERE Name = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(SetParam("itemName", itemName));
            DataTable dt = ExecuteQueryWithResult(cmdText, parameters);
            if (dt.Rows.Count > 0)
            {
                return false;
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
            DataTable dtItem = GetAllItem();
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

        private void ExecuteQuery(string commandText, List<Param> parameters)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection,
                    CommandText = commandText,
                    CommandType = CommandType.Text
                };
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.value);
                }
                cmd.ExecuteNonQuery();
            }
        }   //Insert Update Delete 

        private object ExecuteQueryScalar(string commandText, List<Param> parameters)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection,
                    CommandText = commandText,
                    CommandType = CommandType.Text
                };
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.value);
                }
                return cmd.ExecuteScalar();
            }
        }   //get ExecuteScalar

        private DataTable ExecuteQueryWithResult(string commandText, List<Param> parameters)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection,
                    CommandText = commandText,
                    CommandType = CommandType.Text
                };
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.value);
                }
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
        }   //get datatable

        private Param SetParam(string key, Object value)
        {
            return new Param
            {
                Key = key,
                value = value
            };
        } //Set Parameter

    } //End class WebFormExample

    public class Param
    {
        public string Key { get; set; }
        public object value { get; set; }
    }  //Object Param SET KEY,VALUE
}//End