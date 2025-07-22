using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.TestClass
{
    public class TryCatchInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            var hasAttr = method.GetCustomAttribute<TryCatchAttribute>() != null;

            if (hasAttr)
            {
                try
                {
                    invocation.Proceed();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[예외 발생] {method.Name}: {ex.Message}");
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
