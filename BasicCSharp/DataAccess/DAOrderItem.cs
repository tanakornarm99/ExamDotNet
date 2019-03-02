using BasicCSharp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BasicCSharp.DataAccess
{
    public class DAOrderItem
    {
        private ExecuteQuery _exec;

        public DAOrderItem(string connectionString)
        {
            _exec = new ExecuteQuery(connectionString);
        }

        public DataTable GetOrderItemByIdAndName(string orderId, string itemName)
        {
            string cmdText = "SELECT * FROM [OrderItem] WHERE OrderId = @orderId AND ItemName = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderId", orderId));
            parameters.Add(_exec.SetParam("itemName", itemName));
            return _exec.ExecuteQueryWithResult(cmdText, parameters);
        }


        public DataTable GetOrderItemByName(string itemName)
        {
            string paraPrice = string.Empty;
            string cmdText = "SELECT * FROM [OrderItem] WHERE ItemName = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("itemName", itemName));
            return _exec.ExecuteQueryWithResult(cmdText, parameters);
        }

        public string GetSumPrice(string orderId)
        {
            string cmdText = "SELECT sum(Price) FROM [OrderItem] WHERE OrderId = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderId", orderId));
            return _exec.ExecuteQueryScalar(cmdText, parameters).ToString();
        }

        public void UpdateCategoryOrderItem(string cateOldName, string cateNewName)
        {
            string cmdText = "UPDATE [OrderItem] SET Category = @cateNewName WHERE Category = @cateOldName";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("cateNewName", cateNewName));
            parameters.Add(_exec.SetParam("cateOldName", cateOldName));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void UpdatePriceOrderItem(double newPrice, string itemName, string orderItemId)
        {
            string cmd = "UPDATE [OrderItem] SET Price = @newPrice WHERE ItemName = @itemName AND Id = @orderItemId";
            List<Param> paramety = new List<Param>();
            paramety.Add(_exec.SetParam("newPrice", newPrice));
            paramety.Add(_exec.SetParam("itemName", itemName));
            paramety.Add(_exec.SetParam("orderItemId", orderItemId));
            _exec.ExecuteNonQuery(cmd, paramety);
        }

        public void DeleteCategoryOrderItem(string categoryName)
        {
            string cmdText = "DELETE FROM [OrderItem] WHERE Category = @categoryName";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("categoryName", categoryName));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void UpdateItemNameOrderItem(string itemOldName, string itemNewName)
        {
            string cmdText = "UPDATE [OrderItem] SET ItemName = @itemNewName WHERE ItemName = @itemOldName";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("itemNewName", itemNewName));
            parameters.Add(_exec.SetParam("itemOldName", itemOldName));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void DeleteItemOrderItem(string itemName)
        {
            string cmdText = "DELETE FROM [OrderItem] WHERE ItemName = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("itemName", itemName));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void AddNewOrderItem(string orderId, string itemName, string itemPrice, string category)
        {
            int qty = 1;
            string cmdText = "INSERT INTO [OrderItem] (OrderId,ItemName,Category,Qty,Price) VALUES (@orderId,@itemName,@category,@qty,@price)";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderId", orderId));
            parameters.Add(_exec.SetParam("itemName", itemName));
            parameters.Add(_exec.SetParam("category", category));
            parameters.Add(_exec.SetParam("qty", qty));
            parameters.Add(_exec.SetParam("price", itemPrice));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void UpdateOrderItem(string orderId, double sumQty, double sumPrice, string itemName)
        {
            string cmdText = "UPDATE [OrderItem] SET Qty = @sumQty, Price = @sumPrice WHERE OrderId = @orderId AND ItemName = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("sumQty", sumQty));
            parameters.Add(_exec.SetParam("sumPrice", sumPrice));
            parameters.Add(_exec.SetParam("orderId", orderId));
            parameters.Add(_exec.SetParam("itemName", itemName));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void DeleteOrderItemID(string orderId)
        {
            string cmdText = "DELETE FROM [OrderItem] WHERE [OrderId] = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderId", orderId));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void DeleteOrderItemName(string orderItemId)
        {
            string cmdText = "DELETE FROM [OrderItem] WHERE [Id] = @orderItemId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderItemId", orderItemId));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public DataTable GetAllOrderItem(string orderId)
        {
            string cmdText = "SELECT * FROM [OrderItem] WHERE OrderId = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderId", orderId));
            return _exec.ExecuteQueryWithResult(cmdText, parameters);
        }





    }
}