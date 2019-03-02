using BasicCSharp.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BasicCSharp.BusinessLogic
{
    public class OrderLogic
    {
        private readonly string _conString;

        public OrderLogic(string connectionString)
        {
            _conString = connectionString;
        }

        public DataTable GetAllOrder()
        {
            DAOrder dAOrder = new DAOrder(_conString);
            return dAOrder.GetAllOrder();

        }

        public string GetOrderNumber()
        {
            int length = 4;
            DateTime thisDay = DateTime.Today;
            string orderNo = "ORD2019000000";
            DataTable dtOrder = GetAllOrder();
            if (dtOrder.Rows.Count > 0)
            {
                DAOrder dAOrder = new DAOrder(_conString);
                orderNo = dAOrder.GetLastOrderNumber();
            }
            int subOrderNo = Convert.ToInt32(orderNo.Substring(orderNo.Length - 4)) + 1;
            var result = subOrderNo.ToString().PadLeft(length, '0');
            string resultOrderNumber = thisDay.ToString("ORD" + "yyyyMM" + result);
            return resultOrderNumber;
        }

        public void UpdateOrderSumPriceAll()
        {
            DAOrder dAOrder = new DAOrder(_conString);
            DataTable dt = dAOrder.GetAllOrder();

            string[] arryOrderId = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arryOrderId[i] = dt.Rows[i]["Id"].ToString();
                var sumPrice = GetTotalItemPrice(arryOrderId[i]);
                dAOrder.UpdateSummaryPrice(arryOrderId[i], sumPrice);
            }
        }

        private double GetTotalItemPrice(string orderId)
        {
            string totalPrice = string.Empty;
            double sumPrice = 0;
            DAOrderItem daOrderItem = new DAOrderItem(_conString);
            totalPrice = daOrderItem.GetSumPrice(orderId);
            if (!string.IsNullOrEmpty(totalPrice))
            {
                sumPrice = Convert.ToDouble(totalPrice);
            }
            return sumPrice;
        }




    }//end class
}//End