using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.TestClass
{
    public class SomeType
    {
        private int _someFiled;
        private string someProperty { get; set; }

        public event EventHandler SomeEvent;

        public SomeType(ref String str ) 
        {
            str = "Enter Constructor of SomeType"; 
        }

        public string SomePublicMethod() { return "SomeType's Public Method"; }
        private string SomePrivateMethod() { return "SomeType's Private Method"; }
    }


    public static class TestReflection
    {
        public static bool isSameType(this object o1, object o2)
        {
            return o1.GetType() == o2.GetType();
        }

        public static void TestMethod1()
        {
            Type openType = typeof(Dictionary<,>);
            Type closedType = openType.MakeGenericType(typeof(int), typeof(string));

            object obj = Activator.CreateInstance(closedType);

            var castedObj = obj as Dictionary<int, string>;
            if(castedObj != null)
            {
                castedObj.Add(1, "HELLO WORLD!");
                Console.WriteLine(castedObj[1]);
            }
            else
            {
                Console.WriteLine("Casting Fails");
            }

            foreach(MemberInfo mi in typeof(SomeType).GetTypeInfo().DeclaredMembers)
            {
                Console.WriteLine($"{mi.MemberType} : {mi.Name}");
            }
        }

        public static void TestMethod2()
        {
            /*------------------------ 인스턴스 생성 ------------------------------*/
            Type ctorArg = Type.GetType("System.String&");
            //typeof(string).MakeByRefType(); 와 같다.

            //ConstructorInfo ctor = typeof(SomeType).GetConstructor(new Type[] { ctorArg });

            ConstructorInfo ctor = typeof(SomeType).GetTypeInfo().DeclaredConstructors.First(p => p.GetParameters()[0].ParameterType == ctorArg);

            object[] args = new object[] { "Befor Enter Constructor of SomeType" };

            Console.WriteLine(args[0]);
        
            object obj = ctor.Invoke(args);

            Console.WriteLine(args[0]);

            /*------------------------ 필드 읽고 쓰기 ------------------------------*/
            //FieldInfo fi = obj.GetType().GetField("_someFiled", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo fi = obj.GetType().GetTypeInfo().GetDeclaredField("_someFiled");
            fi.SetValue(obj, 33);
            
            /*------------------------ 메서드 호출 ------------------------------*/
            MethodInfo mi = obj.GetType().GetTypeInfo().GetDeclaredMethod("SomePrivateMethod");

            //private 메서드 임에도 호출 가능
            string s = (String)mi.Invoke(obj, null); // 두번째는 파라미터

            Console.WriteLine(s);

            //fi.SetValue(obj, 44);

            PropertyInfo pi = obj.GetType().GetTypeInfo().GetDeclaredProperty("someProperty");
            pi.SetValue(obj, "PropertyInfo.SetValue");

            Console.WriteLine(pi.GetValue(obj));

            /*------------------------ 이벤트 ---------------------------*/

            EventInfo ei = obj.GetType().GetTypeInfo().GetDeclaredEvent("SomeEvent");
            EventHandler eh = new EventHandler((object sender, EventArgs e) => { });

            ei.AddEventHandler(obj, eh);
            ei.RemoveEventHandler(obj, eh);
        }

    }
}
