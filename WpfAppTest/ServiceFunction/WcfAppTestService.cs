using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using WcfClassTestLibrary;

namespace WpfAppTest.ServiceFunction
{
    public class WcfAppTestService
    {
        public WcfAppTestService() { }


        public void InvokeUsingHttp()
        {
            Uri uri = new Uri("http://localhost/wcf/example/hellowworldservice");

            ServiceEndpoint ep = new ServiceEndpoint(
                ContractDescription.GetContract(typeof(IHelloWorld))
              , new BasicHttpBinding()
              , new EndpointAddress(uri));

            ChannelFactory<IHelloWorld> factory = new ChannelFactory<IHelloWorld>(ep);
            IHelloWorld proxy = factory.CreateChannel();
            string result = proxy.sayHello();
            (proxy as IDisposable).Dispose();
            Console.WriteLine(result);

        }

        private void InvokeUsingTCP()
        {
            Uri uri = new Uri("net.tcp://localhost:809/wcf/example/hellowworldservice");

            ServiceEndpoint ep = new ServiceEndpoint(
                ContractDescription.GetContract(typeof(IHelloWorld))
              , new NetTcpBinding()
              , new EndpointAddress(uri));

            ChannelFactory<IHelloWorld> factory = new ChannelFactory<IHelloWorld>(ep);
            IHelloWorld proxy = factory.CreateChannel();
            string result = proxy.sayHello();
            (proxy as IDisposable).Dispose();
            Console.WriteLine(result);

            //트랜스포트 xml soap 메시지를 네트워크 상에서 전송해주는 매개체 프로토콜(Http, tcp, 명명된 파이프, MSMQ, P2P)
            //http, net.tcp, net.pipe, net.msmq

            /*
             트랜스포트               바인딩                                                                        스킴

            HTTP(s)                   BasicHttpBinding, WSHttpBinding,WSDualHttpBinding,WSFederationBinding          http 혹은 https
            TCP                       NetTcpBinding                                                                 net.tcp
            명명된 파이프             NetNamedPipeBinding                                                           net.pipe
            MSMQ                      NetMsmqBinding, MsmqIntegrationBinding                                        net.msmq
            P2P                       NetPeerTcpBinding                                                             net.p2p
             
             */

        }


        public void TestServiceFunction()
        {
            InvokeUsingHttp();

            InvokeUsingTCP();







        }

    }
}
