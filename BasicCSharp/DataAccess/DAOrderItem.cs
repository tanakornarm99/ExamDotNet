using BasicCSharp.Model;
using System;
using System.Collections.Generic;
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

        public string GetSumPrice(string orderId)
        {
            string cmdText = "SELECT sum(Price) FROM [OrderItem] WHERE OrderId = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderId", orderId));
            return _exec.ExecuteQueryScalar(cmdText, parameters).ToString();   
        }


    }
}