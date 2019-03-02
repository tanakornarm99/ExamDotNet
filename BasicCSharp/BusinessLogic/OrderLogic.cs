using BasicCSharp.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
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
            DAOrderItem daOrderItem = new DAOrderItem(_conString);
            string totalPrice = string.Empty;
            double sumPrice = 0;
            totalPrice = daOrderItem.GetSumPrice(orderId);
            if (!string.IsNullOrEmpty(totalPrice))
            {
                sumPrice = Convert.ToDouble(totalPrice);
            }
            return sumPrice;
        }

        public string ShowTotalPrice(string orderId)
        {
            DAOrderItem daOrderItem = new DAOrderItem(_conString);
            var sumPrice = Convert.ToDouble(daOrderItem.GetSumPrice(orderId));
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            numberFormatInfo.CurrencySymbol = "฿"; // Replace with "$" or "£" or whatever you need
            var formattedPrice = sumPrice.ToString("C", numberFormatInfo);
            //lblTotalPrice.Text = formattedPrice; //show ฿xxx.xx
            return formattedPrice;
        }

        public void UpdatePriceOrderItem(string itemName, string itemPrice)
        {
            //GetAllQtyOrderItem
            DAOrderItem dAOrderItem = new DAOrderItem(_conString);
            DataTable dt = dAOrderItem.GetItemNameOrderItem(itemName);
            double newPrice = Convert.ToDouble(itemPrice);
            string[] arryId = new string[dt.Rows.Count];
            double[] arryQty = new double[dt.Rows.Count];
            double[] arrySumPrice = new double[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arryId[i] = dt.Rows[i]["Id"].ToString();
                arryQty[i] = Convert.ToInt32(dt.Rows[i]["Qty"]);
                arrySumPrice[i] = arryQty[i] * newPrice;
                dAOrderItem.UpdatePriceOrderItem(arrySumPrice[i], itemName, arryId[i]);
            }
        }

        public void AddOrder(string orderNumber, string firstName, string sureName, string contact, string email)
        {
            DAOrder dAOrder = new DAOrder(_conString);
            DACustomer dACustomer = new DACustomer(_conString);
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(sureName))
            {
                dAOrder.AddOrder(orderNumber, firstName, sureName);
                //string oderId = GetOrderId(orderNumber);
                //paramety.Add(SetParam("orderId", orderId));
                dACustomer.AddCustomer(firstName, sureName, contact, email);
            }
        }

        public void UpdateOrder(string name, string orderId)
        {
            DAOrder dAOrder = new DAOrder(_conString);
            if (!string.IsNullOrEmpty(name))
            {
                dAOrder.UpdateOrder(name, orderId);
            }
        }


    }//end class
}//End