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
            ServiceHost host = new ServiceHost(typeof(HelloWorldWCFService), new Uri("http://localhost/wcf/example/hellowworldservice"));


            ContractDescription desc = ContractDescription.GetContract(typeof(IHelloWorld));
            Console.WriteLine(desc.Namespace);

            host.AddServiceEndpoint(typeof(IHelloWorld), new BasicHttpBinding(),"");

            host.Open();
            Console.WriteLine("Press Any key to stop the service");
            Console.ReadKey(true);
            host.Close();


        }
    }
}
