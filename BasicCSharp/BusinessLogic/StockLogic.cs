using BasicCSharp.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BasicCSharp.BusinessLogic
{
    public class StockLogic
    {
        private readonly string _conString;

        public StockLogic(string connectionString)
        {
            _conString = connectionString;
        }

        public DataTable GetAllCategory()
        {
            DACategory dACategory = new DACategory(_conString);
            return dACategory.GetAllCategory();
        }

        public DataTable GetAllItem()
        {
            DAItem dAItem = new DAItem(_conString);
            return dAItem.GetAllItem();

        }
    }
}