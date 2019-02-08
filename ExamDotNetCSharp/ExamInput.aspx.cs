using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamDotNetCSharp
{
    public partial class ExamInput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void BtnSummit_Click(object sender, EventArgs e)
        {
            string strNumInput = txtInputNumber.Text;
            int num = Convert.ToInt32(strNumInput);
            int mod = 2, n = 0;
            int[] array = new int[10];
            string result = "";

            if (num == 1)
            {
                result += num.ToString() + " ";
            }
            while (num != 1)
            {
                while (num % mod == 0)
                {
                    //result += mod.ToString() + " ";
                    array[n] = mod;
                    result += array[n].ToString() + " ";
                    num = num / mod;
                    n++;
                }
                mod++;
            }
            lblResult.Text = result;          
        }
    }
}