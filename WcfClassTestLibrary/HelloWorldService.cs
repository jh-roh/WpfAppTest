using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfClassTestLibrary
{
    //서비스 계약 선언
    [ServiceContract(Namespace="http://localhost/wcf/example/hellowworldservice")]
    public interface IHelloWorld
    {
        [OperationContract]
        string sayHello();


    }


    //서비스 타입 구현
    public class HelloWorldWCFService : IHelloWorld
    {
        public string sayHello()
        {
            return "Hello WCF World";
        }
    }
}
