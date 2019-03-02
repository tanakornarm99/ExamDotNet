using BasicCSharp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BasicCSharp.DataAccess
{
    public class DAItem
    {
        private ExecuteQuery _exec;

        public DAItem(string connectionString)
        {
            _exec = new ExecuteQuery(connectionString);
        }

        public DataTable ItemIsNotExist(string itemName)
        {
            string cmdText = "SELECT Name FROM [Item] WHERE Name = @itemName";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("itemName", itemName));
            return _exec.ExecuteQueryWithResult(cmdText, parameters);
        }

        public DataTable GetAllItem()
        {
            string cmdText = "SELECT * FROM [Item]";
            return _exec.ExecuteQueryWithResult(cmdText, new List<Param>());
        }

        public string GetItemName(string itemId)
        {
            string cmdText = "SELECT Name FROM [Item] WHERE Id = @itemId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("itemId", itemId));
            return _exec.ExecuteQueryScalar(cmdText, parameters).ToString();
        }

        public void AddItem(string itemName, string itemPrice, int categoryId)
        {
            string cmdText = "INSERT INTO [Item] (Name,Price,CategoryId) VALUES (@name,@price,@categoryId)";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("name", itemName));
            parameters.Add(_exec.SetParam("price", itemPrice));
            parameters.Add(_exec.SetParam("categoryId", categoryId));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void UpdateItem(string itemId, string itemName, string itemPrice, string categoryId)
        {
            string cmdText = "UPDATE [Item] SET Name = @name, Price = @price, CategoryId = @catId WHERE Id = @itemId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("itemId", itemId));
            parameters.Add(_exec.SetParam("name", itemName));
            parameters.Add(_exec.SetParam("price", itemPrice));
            parameters.Add(_exec.SetParam("catId", categoryId));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void DeleteCategoryItem(string catId)
        {
            string cmdText = "DELETE FROM [Item] WHERE CategoryId = @catId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("catId", catId));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void DeleteItem(string itemId)
        {
            string cmdText = "DELETE FROM [Item] WHERE Id = @itemId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("itemId", itemId));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }










    }
}