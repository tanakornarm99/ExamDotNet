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
            int mod = 2; 
            string result = string.Empty;

            if (num == 1)
            {
                result = num.ToString();
            }
            while (num != 1)
            {
                while (num % mod == 0)
                {
                    result += mod.ToString() + " ";
                    num = num / mod;
                }
                mod++;
            }
            string[] resultArray = result.Trim().Split();
            string resultString = string.Empty;
            var myList = new List<string>();
            foreach (var s in resultArray)
            {
                if (!myList.Contains(s))
                    myList.Add(s);
            }
            foreach (var s in myList)
            {
                resultString += s.ToString() + " ";
            }
            lblResult.Text = resultString;

            //int check = 0;
            //string txtStr = "2 2 2 2 5 5 7";
            //string[] resultArray = txtStr.Trim().Split();
            //string resultValue = string.Empty;
            //string resultValue2 = string.Empty;
            //for (int i = 0; i < resultArray.Length; i++)
            //{
            //    for (int j = i+1; j < resultArray.Length; j++)
            //    {
            //        if (resultArray[i] == resultArray[j])
            //        {
            //            if (check == 0)
            //            {
            //                resultValue += resultArray[j].ToString() + " ";
            //                check = 1;
            //                break;
            //            }
            //        }
            //        else {
            //            //resultValue2 += resultArray[j].ToString() + " ";
            //            check = 0;
            //            break;
            //        }

            //    }
            //}

        }
    }
}