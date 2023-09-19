using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WpfAppTest.TestClass
{
    public class TestTrim
    {

        public TestTrim() { }


        public void TestMethod()
        {
            string tempString = "ABCD 1234       \t      \r\n     5678        9999   ";


            //var resultString = tempString.Trim();
            var resultString1 = tempString.Replace(" ","")
                                         .Replace("\t", "")
                                         .Replace("\n","")
                                         .Replace("\r","");


            var resultString2 = Regex.Replace(tempString, @"\s+", "");


        }

        public void ToCharMethod()
        {
            byte a1 = (byte)0x33;
            byte a2 = (byte)0x32;
            byte a3 = (byte)0x41;
            byte a4 = (byte)0x43;
            byte a5 = (byte)0x33;
            byte a6 = (byte)0x03;
            byte a7 = (byte)0xA7;


            Char convertChar1 = Convert.ToChar(a1);
            Char convertChar2 = Convert.ToChar(a2);
            Char convertChar3 = Convert.ToChar(a3);
            Char convertChar4 = Convert.ToChar(a4);
            Char convertChar5 = Convert.ToChar(a5);
            decimal convertChar6 = Convert.ToDecimal((ushort)0x03A7);

            ushort combinedData = (ushort)((a6 << 8) | a7);
            string strCombinedData = combinedData.ToString("D4");
        }


    }
}
