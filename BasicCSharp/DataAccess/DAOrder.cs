using BasicCSharp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BasicCSharp.DataAccess
{
    public class DAOrder
    {
        private ExecuteQuery _exec;

        public DAOrder(string connectionString)
        {
            _exec = new ExecuteQuery(connectionString);
        }

        public DataTable GetAllOrder()
        {
            string cmdText = "SELECT * FROM [Order]";
            return _exec.ExecuteQueryWithResult(cmdText, new List<Param>());
        }

        public string GetLastOrderNumber()
        {
            string cmdText = "SELECT TOP 1 OrderNumber FROM [Order] ORDER BY ID DESC";
            return _exec.ExecuteQueryScalar(cmdText, new List<Param>()).ToString();
        }

        public void UpdateSummaryPrice(string orderId, double sumPrice)
        {
            string cmdText = "UPDATE [Order] SET [SummaryPrice] = @sumPrice WHERE [Id] = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderId", orderId));
            parameters.Add(_exec.SetParam("sumPrice", sumPrice));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }





    }
}