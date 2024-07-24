
#line 3 "dynamic_code.txt"

using System;
using System.Text;

public class MyType
{
    public void Print(object obj)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(
            DateTime.Now
        );

        Console.WriteLine(sb.ToString());
    }
}