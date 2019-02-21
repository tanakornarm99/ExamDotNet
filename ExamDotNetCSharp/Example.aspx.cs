using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamDotNetCSharp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnGetOrder_Click(object sender, EventArgs e)
        {
            lblShowMessage.Text = "Success !!!";
            string messageStr = lblShowMessage.Text;

            DataTable dtResultOrder = GetDataOrder();

            tblOrder.DataSource = dtResultOrder;
            tblOrder.DataBind(); //show data
        }

        private DataTable GetDataOrder()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                using (SqlConnection sqlConnection = new SqlConnection(constr))
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection;
                    cmd.CommandText = "Select * from [Order]";
                    cmd.CommandType = CommandType.Text;

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt.TableName = "Order";

                    return dt;
                }
                //return new DataTable();
            }
            catch (Exception ex)
            {
                throw ex;
                //return new DataTable();
            }
        }
    }
}