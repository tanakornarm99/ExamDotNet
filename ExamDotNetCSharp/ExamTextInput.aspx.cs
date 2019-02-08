using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamDotNetCSharp
{
    public partial class ExamTextInput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSbmt_Click(object sender, EventArgs e)
        {
            //string[] lines = File.ReadAllLines(@"C:\GitHub\ExamDotNet\text.txt");
            string text = File.ReadAllText(@"C:\GitHub\ExamDotNet\text.txt");
            lblShowtext.Text = text;

            //// Location of the letter c.
            //int i = text.IndexOf("palindrome");
            //// Remainder of string starting at c.
            //string d = text.Substring(i);
            //lblShowMessagetxt.Text = d;

            int count = 0;
            string stringToCheck = "palindrome";
            string[] stringArray = { "palindrome", "text1", "texttest", "palindrome" };
            foreach (string x in stringArray)
            {
                if (stringToCheck.Contains(x))
                {
                    count++;
                }
            }
            lblShowMessagetxt.Text = count.ToString();
        }
    }
}