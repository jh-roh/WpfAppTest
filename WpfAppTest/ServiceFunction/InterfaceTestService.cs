using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.ServiceFunction
{
    public class InterfaceTestService : IDisposable
    {
        private IInterfaceService channel = null;

        public IInterfaceService Channel
        {
            get
            {
                return channel;
            }
        }
        public InterfaceTestService()
        {
        }

        public IInterfaceService Open(string address)
        {
            try
            {
                ChannelFactory<IInterfaceService> factory = new ChannelFactory<IInterfaceService>();

                NetTcpBinding netTcpBinding = new NetTcpBinding(SecurityMode.None, true);
                netTcpBinding.CloseTimeout = new TimeSpan(00, 10, 00);
                netTcpBinding.OpenTimeout = new TimeSpan(00, 1, 00);
                netTcpBinding.SendTimeout = new TimeSpan(00, 1, 00);
                netTcpBinding.ReceiveTimeout = new TimeSpan(00, 1, 00);
                netTcpBinding.MaxBufferPoolSize = 2147483647;
                netTcpBinding.MaxReceivedMessageSize = 2147483647;
                factory.Endpoint.Address = new EndpointAddress(address);
                factory.Endpoint.Binding = netTcpBinding;
                foreach (OperationDescription op in factory.Endpoint.Contract.Operations)
                {
                    DataContractSerializerOperationBehavior dataContractBehavior =
                                op.Behaviors.Find<DataContractSerializerOperationBehavior>() as DataContractSerializerOperationBehavior;
                    if (dataContractBehavior != null)
                    {
                        dataContractBehavior.MaxItemsInObjectGraph = 2147483647;
                    }
                }

                channel = factory.CreateChannel();
            }
            catch (Exception ex)
            {
            }

            return channel;
        }


        public void Close()
        {
            try
            {
                if (channel != null)
                {
                    // 채널 종료
                    ((ICommunicationObject)channel).Close();

                    channel = null;
                }
            }
            catch (Exception ex)
            {
            }
        }


        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        ~InterfaceTestService()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
