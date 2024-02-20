using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using WcfClassTestLibrary;

namespace WcfAppTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(HelloWorldWCFService)
                        , new Uri("http://localhost/wcf/example/hellowworldservice")
                        , new Uri("net.tcp://localhost:809/wcf/example/hellowworldservice"));


            ContractDescription desc = ContractDescription.GetContract(typeof(IHelloWorld));
            Console.WriteLine(desc.Namespace);

            host.AddServiceEndpoint(                
                typeof(IHelloWorld),                //서비스 계약
                new BasicHttpBinding(),             //바인딩
                "");                                //상대주소

            host.AddServiceEndpoint(
                typeof(IHelloWorld),                //서비스 계약
                new NetTcpBinding(),                //바인딩
                "");                                //상대주소
                                                    //별도로 TCP 포트를 지정해주지 않으면 808포트를 사용하도록 되어 있음.

            host.Open();
            Console.WriteLine("Press Any key to stop the service");
            Console.ReadKey(true);
            host.Close();


        }
    }
}
