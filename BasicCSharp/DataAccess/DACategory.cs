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

        public DataTable GetAllCategory()
        {
            string cmdText = "SELECT * FROM [Category]";
            return _exec.ExecuteQueryWithResult(cmdText, new List<Param>());
        }
    }
}