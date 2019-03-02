using BasicCSharp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BasicCSharp.DataAccess
{
    public class DACategory
    {
        private ExecuteQuery _exec;

        public DACategory(string connectionString)
        {
            _exec = new ExecuteQuery(connectionString);
        }

        public DataTable CategoryIsNotExist(string categoryName)
        {
            string cmdText = "SELECT Name FROM [Category] WHERE Name = @categoryName";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("categoryName", categoryName));
            return _exec.ExecuteQueryWithResult(cmdText, parameters);
        }

        public DataTable GetAllCategory()
        {
            string cmdText = "SELECT * FROM [Category]";
            return _exec.ExecuteQueryWithResult(cmdText, new List<Param>());
        }

        public string GetCategoryName(string catId)
        {
            string cmdText = "SELECT Name FROM [Category] WHERE Id = @catId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("catId", catId));
            return _exec.ExecuteQueryScalar(cmdText, parameters).ToString();
        }

        public void AddCategory(string categoryName)
        {
            string cmdText = "INSERT INTO [Category] (Name) VALUES (@name)";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("name", categoryName));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void UpdateCategory(string categoryName,string categoryId)
        {
            string cmdText = "UPDATE [Category] SET Name = @name WHERE Id = @catId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("name", categoryName));
            parameters.Add(_exec.SetParam("catId", categoryId));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void DeleteCategory(string catId)
        {
            string cmdText = "DELETE FROM [Category] WHERE Id = @categoryId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("categoryId", catId));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }




       




    }
}