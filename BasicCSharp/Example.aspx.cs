using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BasicCSharp
{
    public partial class Example : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnClick_Click(object sender, EventArgs e)
        {
            string textFile = File.ReadAllText(@"C:\GitHub\ExamDotNet\text.txt", Encoding.UTF8);
            string[] stringArry = textFile.Split(' ');
            string typeStringText = string.Concat(stringArry);

            //string[] aryText = new[] { textFile }; // string->string[]
            //int count = stringTextFile.Length;
            //string[] stringReverseText = new string[count];
            //for (int i = 0; i < stringTextFile.Length-1; i++)
            //{
            //    stringReverseText[i] = stringTextFile[count-1];
            //    count--;
            //} //ArrayIndex End->Start

            char[] charArry = typeStringText.ToCharArray();
            string reverseText = string.Empty;
            for (int i = charArry.Length-1 ; i > -1 ; i--)
            {
                reverseText += charArry[i];
            }
            
            string[] TextArry = textFile.Split();
            string[] reverseArry = reverseText.Split();
            int k = TextArry.Length;
            int count = 0;

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (!string.IsNullOrEmpty(TextArry[i]))
                    {
                        if (TextArry[i] == reverseArry[j])
                        {
                            count++;
                        }
                    }
                }
            }
            lblTextFile.Text = typeStringText;
            lblTextReverse.Text = reverseText;
            lblResult.Text = count.ToString();


            //---------------------------//
            string typeString1 = "My name is Armm";
            string[] typeStringSpilt = typeString1.Split(' '); //เก็บในรูปแบบ Array

            typeString1 = typeString1.Replace("Armm", "Tanakorn");
            typeString1 = typeString1.Replace(" ", string.Empty);
            typeString1 = typeString1.Replace(" ", "");
            typeString1 = typeString1.Replace("Tanakorn", "Armm");

            typeString1 = " My name is Armm ";
            typeString1 = typeString1.Trim(); //ตัดค่าช่องว่างหน้า-หลัง

            typeString1 = " My name is Armm ";
            typeString1 = typeString1.TrimStart();

            typeString1 = " My name is Armm ";
            typeString1 = typeString1.TrimEnd();

            typeString1 = "My name is Armm";
            typeString1 = typeString1.Substring(0,10); //ตัดตั้งแต่ Index ที่ 0-10 จะได้ค่า Arm

            string typeString2 = ""; 
            string typeString3 = string.Empty;
            string[] typeString4 = new string[400];


            if (string.IsNullOrEmpty(typeString3))
            {
                // other logic
            }

            int typeInt1;
            int typeInt2 = 0;
            int typeInt3 = -1;

            long typeLong1;
            long typeLong2 = 0;
            long typeLong3 = -1;

            double typeDouble1;
            double typeDouble2 = 0.00;
            double typeDouble3 = -10.00;

            string[] arryString1 = new string[400];
            string[] arryString2 = { "1", "Hello" };
            string[] arryString3 = { "1", "2" };

            arryString1[0] = arryString2[0];
            arryString1[1] = arryString2[0];
            arryString1[2] = arryString2[0];
                                    
            if (arryString2.Length > 0)
            {
                typeString3 = arryString2[1];
            }

            foreach (var item in arryString2)
            {
                if (item == "Hello")
                {
                    typeString3 = item;
                }
            }

            for (int i = 0; i < arryString2.Length; i++)
            {
                typeString3 = arryString2[0];
            }
        }
    }
}