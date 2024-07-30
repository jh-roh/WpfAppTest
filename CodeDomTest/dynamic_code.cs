
#line 3 "dynamic_code.txt"

using System;
using System.Text;

public class MyType
{
    public void Print(object obj)
    {
        if (System.Diagnostics.Debugger.IsAttached)
            System.Diagnostics.Debugger.Break();

        StringBuilder sb = new StringBuilder();
        sb.Append(
            DateTime.Now
        );

        Console.WriteLine($"{obj.ToString()} {sb.ToString()}");
    }
}