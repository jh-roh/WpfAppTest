using CommonUtil;
using CommonUtil.IO.SocketUtil;
using SocketTester.Globals;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SocketTester.Services
{
    public enum SocketHandlerType
    {
        Connect,
        Close,
        Receive
    }

    public struct SocketClientEventArgs 
    {
        public int ClientId { get; set; }
        public SocketHandlerType HandlerType { get; set; }

        public byte[] ReceiveDatas { get; set; }

        public SocketClientEventArgs(int clientId, SocketHandlerType handlerType, byte[] receiveDatas)
        {
            ClientId = clientId;
            HandlerType = handlerType;
            ReceiveDatas = receiveDatas != null ? receiveDatas.ToArray() : null;
        }
    }

    public delegate void SocketClientManagerEventHandler(object sender, SocketClientEventArgs e);


    public class SocketClientManager : IDisposable
    {
        private int _clientId;

        public int ClientId
        {
            get { return _clientId; }
        }

        private AsyncSocketClient Socket;

        // 연결 완료 이벤트
        public ManualResetEvent ConnectionCompleted = new ManualResetEvent(false);

        // 연결 닫기 완료 이벤트
        public ManualResetEvent CloseCompleted = new ManualResetEvent(false);

        // 연결 타임아웃 (밀리초)
        public int ConnectionTimeout = 3000;

        // 닫기 타임아웃 (밀리초)
        public int CloseTimeout = 2000;

        private BlockingCollection<byte[]> SendQueue;
        private BlockingCollection<byte[]> ReceiveQueue;

        private Thread SendLoopThread;
        private Thread ReceiveLoopThread;

        private CancellationTokenSource SendCts = new CancellationTokenSource();
        private CancellationTokenSource ReceiveCts = new CancellationTokenSource();

        public event SocketClientManagerEventHandler ManageHandler;

        private bool IsSendWorkThread = false;
        private bool IsReceiveWorkThread = false;

        private object ReceiveLock = new object();

        private bool disposedValue;

        private bool _isConnected;

        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; }
        }

        public String IpAddress
        {
            get
            {
                try
                {
                    if (Socket.Connection != null)
                    {
                        IPEndPoint ipEndPoint = Socket.Connection.RemoteEndPoint as IPEndPoint;

                        return ipEndPoint.Address.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public int? Port
        {
            get
            {
                try
                {
                    if (Socket.Connection != null)
                    {
                        IPEndPoint ipEndPoint = Socket.Connection.RemoteEndPoint as IPEndPoint;

                        return ipEndPoint.Port;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        public SocketClientManager()
        {
            InitProcessThread();
        }

        public void InitProcessThread()
        {
            SendQueue = new BlockingCollection<byte[]>();
            ReceiveQueue = new BlockingCollection<byte[]>();
            IsSendWorkThread = true;
            IsReceiveWorkThread = true;


            //SendQueue 처리 쓰레드
            SendLoopThread = new Thread(() =>
            {
                StringBuilder sb = new StringBuilder();

                while (IsSendWorkThread)
                {
                    try
                    {
                        var sendData = SendQueue.Take(SendCts.Token);

                        sb.Clear();

                        foreach (var item in sendData)
                        {
                            sb.Append($"{item:X2} ");
                        }

                        if (Socket.Connection != null)
                        {
                            Socket.Send(sendData);
                        }

                        Log.Info2("SEND", $"Client{_clientId},{IpAddress},{Port}", sb.ToString(), ApplicationConfig.SocketTesterLogFileName);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });

            SendLoopThread.IsBackground = true;
            SendLoopThread.SetApartmentState(ApartmentState.STA);
            SendLoopThread.Start();

            //ReceiveQueue 처리 쓰레드
            ReceiveLoopThread = new Thread(() =>
            {
                while (IsReceiveWorkThread)
                {
                    var receiveData = ReceiveQueue.Take(ReceiveCts.Token);

                    ManageHandler(this, new SocketClientEventArgs(_clientId ,SocketHandlerType.Receive, receiveData));
                }

            });

            ReceiveLoopThread.IsBackground = true;
            ReceiveLoopThread.SetApartmentState(ApartmentState.STA);
            ReceiveLoopThread.Start();
        }

        public void SetSocketClient(int? clientId)
        {
            _clientId = (int)clientId;
            Socket = new AsyncSocketClient((int)clientId);
            Socket.OnClose += Socket_OnClose;
            Socket.OnConnect += Socket_OnConnect;
            Socket.OnError += Socket_OnError;
            Socket.OnReceive += Socket_OnReceive;
        }

        public void Connect(string host, int port)
        {
            ConnectionCompleted.Reset();

            Log.Info2("CONNECT", $"Client{_clientId},{host},{port}", "Trying to connect", ApplicationConfig.SocketTesterLogFileName);

            Socket.Connect(host, port);

            ConnectionCompleted.WaitOne(ConnectionTimeout);
            
        }

        public void SendDataToQueue(byte[] data)
        {
            SendQueue.Add(data);
        }

        public void Close()
        {
            if (Socket.Connection != null)
            {
                CloseCompleted.Reset();

                Log.Info2("CLOSE", $"Client{_clientId},{IpAddress},{Port}", "Trying to close", ApplicationConfig.SocketTesterLogFileName);

                Socket.Close();

                CloseCompleted.WaitOne(CloseTimeout);
            }
        }

        private void Socket_OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            lock(ReceiveLock)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < e.ReceiveData.Length; i++)
                {
                    sb.Append($"{e.ReceiveData[i]:X2} ");
                }

                Log.Info2("RECV", $"Client{_clientId},{IpAddress},{Port}", sb.ToString(), ApplicationConfig.SocketTesterLogFileName);

                ReceiveQueue.Add(e.ReceiveData);

            }
        }

        private void Socket_OnError(object sender, AsyncSocketErrorEventArgs e)
        {
            Log.Error(e.AsyncSocketException);
        }

        private void Socket_OnConnect(object sender, AsyncSocketConnectionEventArgs e)
        { 
            Log.Info2("CLOSE", $"Client{_clientId},{IpAddress},{Port}", "Connection Complete", ApplicationConfig.SocketTesterLogFileName);

            IsConnected = true;

            ConnectionCompleted.Set();

            ManageHandler(this, new SocketClientEventArgs(_clientId,SocketHandlerType.Connect, null));

        }

        private void Socket_OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {
            Log.Info2("CLOSE", $"Client{_clientId},{IpAddress},{Port}", "Close Complete", ApplicationConfig.SocketTesterLogFileName);

            IsConnected = false;

            CloseCompleted.Set();

            ManageHandler(this, new SocketClientEventArgs(_clientId, SocketHandlerType.Close, null));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.

                    //자원 해제
                    SendCts.Cancel();
                    SendCts.Dispose();

                    SendQueue.CompleteAdding();
                    SendQueue.Dispose();
                    SendQueue = null;

                    //자원 해제
                    ReceiveCts.Cancel();
                    ReceiveCts.Dispose();

                    ReceiveQueue.CompleteAdding();
                    ReceiveQueue.Dispose();
                    ReceiveQueue = null;

                    IsSendWorkThread = false;
                    IsReceiveWorkThread = false;
                    

                    Socket.OnClose -= Socket_OnClose;
                    Socket.OnConnect -= Socket_OnConnect;
                    Socket.OnError -= Socket_OnError;
                    Socket.OnReceive -= Socket_OnReceive;
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~SocketClientManager()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
