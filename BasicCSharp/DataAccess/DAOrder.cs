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

        public void AddOrder(string orderNumber, string firstName, string sureName)
        {
            double sumPrice = 0.00;
            string date = DateTime.Now.ToString();
            string nameFrom = firstName + " " + sureName;
            string cmdText = "INSERT INTO [Order] (OrderNumber,[From],SummaryPrice,Date) VALUES (@orderNumber,@from,@sumPrice,@date)";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderNumber", orderNumber));
            parameters.Add(_exec.SetParam("from", nameFrom));
            parameters.Add(_exec.SetParam("sumPrice", sumPrice));
            parameters.Add(_exec.SetParam("date", date));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }

        public void UpdateOrder(string name,string orderId)
        {
            string date = DateTime.Now.ToString();
            string cmdText = "UPDATE [Order] SET [From] = @name, Date = @date WHERE Id = @orderId";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("name", name));
            parameters.Add(_exec.SetParam("date", date));
            parameters.Add(_exec.SetParam("orderId", orderId));
            _exec.ExecuteNonQuery(cmdText, parameters);

        }

        public void DeleteOrder(string orderId)
        {
            string cmdText = "DELETE FROM [Order] WHERE [Id] = @orderID";
            List<Param> parameters = new List<Param>();
            parameters.Add(_exec.SetParam("orderId", orderId));
            _exec.ExecuteNonQuery(cmdText, parameters);
        }



    }
}