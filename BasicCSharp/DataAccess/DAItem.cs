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

        public DataTable GetAllItem()
        {
            string cmdText = "SELECT * FROM [Item],[Category] WHERE Item.CategoryId = Category.Id";
            return _exec.ExecuteQueryWithResult(cmdText, new List<Param>());
        }

    }

  
}