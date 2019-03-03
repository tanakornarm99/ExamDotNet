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
        private static string CONTSTANT_OrderID;
        private static string CONTSTANT_CatID; 

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
            _stockLogic = new StockLogic(conString);
            string categoryName = txtCategory.Text;
            _stockLogic.AddCategory(categoryName);
            ClearCategory(sender, e);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void UpdateCategory(object sender, EventArgs e)
        {
            _stockLogic = new StockLogic(conString);
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
            _stockLogic = new StockLogic(conString);
            string itemName = txtItemName.Text;
            string itemPrice = txtItemPrice.Text;
            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            _stockLogic.AddItem(itemName, itemPrice, categoryId);
            ClearItem(sender, e);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void UpdateItem(object sender, EventArgs e)
        {
            _stockLogic = new StockLogic(conString);
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

        protected void AddOrder(object sender, EventArgs e)
        {
            _orderLogic = new OrderLogic(conString);
            string orderNumber = lblOrderNo.Text;
            string firstName = txtFirstName.Text.Trim();
            string sureName = txtSureName.Text.Trim();
            string contact = txtContact.Text.Trim();
            string email = txtEmail.Text.Trim();
            _orderLogic.AddOrder(orderNumber, firstName, sureName, contact, email);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void UpdateOrder(object sender, EventArgs e)
        {
            _orderLogic = new OrderLogic(conString);
            string name = txtFirstName.Text;
            string orderId = CONTSTANT_OrderID;
            _orderLogic.UpdateOrder(name, orderId);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

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
            _stockLogic = new StockLogic(conString);
            string catId = gvCategory.DataKeys[e.RowIndex].Value.ToString();
            _stockLogic.DeleteCategory(catId);
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
            _stockLogic = new StockLogic(conString);
            string itemId = gvItem.DataKeys[e.RowIndex].Value.ToString();
            _stockLogic.DeleteItem(itemId);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void GvOrder_Selected(object sender, EventArgs e)
        {
            _orderLogic = new OrderLogic(conString);
            GridViewRow row = gvOrder.SelectedRow;
            string orderId = row.Cells[2].Text;
            CONTSTANT_OrderID = orderId;
            lblOrderNo.Text = row.Cells[3].Text;
            string strName = row.Cells[4].Text;
            string[] strSpName = strName.Split(' ');
            txtFirstName.Text = strSpName[0];
            //txtSureName.Text = strSpName[1];

            lblTotalPrice.Text = _orderLogic.ShowTotalPrice(orderId);

            DataTable dtOrderItem = _orderLogic.GetAllOrderItem(orderId);
            gvOrderItem.DataSource = dtOrderItem;
            gvOrderItem.DataBind();

            btnAddOrder.Visible = false;
            btnUpdateOrder.Visible = true;
            btnAddOrderItem.Visible = true;
            lblTotaltxt.Visible = true;
        }

        protected void GvOrder_Delete(object sender, GridViewDeleteEventArgs e)
        {
            _orderLogic = new OrderLogic(conString);
            string orderId = gvOrder.DataKeys[e.RowIndex].Value.ToString();
            _orderLogic.DeleteOrder(orderId);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void GvAddOrderItem_Selected(object sender, EventArgs e)
        {
            _orderLogic = new OrderLogic(conString);
            GridViewRow row = gvAddOrderItem.SelectedRow;
            string orderId = CONTSTANT_OrderID;    //unknow value CONTSTANT_OrderID;
            string itemName = row.Cells[2].Text;
            string itemPrice = row.Cells[3].Text;
            string categoryId = row.Cells[4].Text;
            _orderLogic.AddOrderItemExist(orderId, itemName, itemPrice, categoryId);
            lblTotalPrice.Text = _orderLogic.ShowTotalPrice(orderId);

            DataTable dtOrderItem = _orderLogic.GetAllOrderItem(orderId);
            gvOrderItem.DataSource = dtOrderItem;
            gvOrderItem.DataBind();
        }

        protected void GvOrderItem_Delete(object sender, GridViewDeleteEventArgs e)
        {
            _orderLogic = new OrderLogic(conString);
            string orderItemId = gvOrderItem.DataKeys[e.RowIndex].Values[0].ToString();
            string orderId = gvOrderItem.DataKeys[e.RowIndex].Values[1].ToString();
            string itemName = gvOrderItem.DataKeys[e.RowIndex].Values[2].ToString();
            string qty = gvOrderItem.DataKeys[e.RowIndex].Values[3].ToString();
            _orderLogic.DeleteOrderItem(orderItemId, orderId, itemName, qty);
            lblTotalPrice.Text = _orderLogic.ShowTotalPrice(orderId);
            DataTable dtOrderItem = _orderLogic.GetAllOrderItem(orderId);
            gvOrderItem.DataSource = dtOrderItem;
            gvOrderItem.DataBind();
        }

        protected void AddOrderItem_Click(object sender, EventArgs e)
        {
            _stockLogic = new StockLogic(conString);
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