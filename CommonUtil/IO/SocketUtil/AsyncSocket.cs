using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CommonUtil.IO.SocketUtil
{
    public enum SocketErrorType
    {
        SendError,

        OnSendCallBackError,

        ConnectedError,

        OnConnectCallbackError,

        ReceivedError,

        OnReceiveCallBackError,

        CloseError,

        OnCloseCallBackError,
    }


    #region 기존 Socket 소스 수정
    public class StateObject
    {
        private const int BUFFER_SIZE = 32768;

        private Socket worker;
        private byte[] buffer;

        public StateObject(Socket worker)
        {
            this.worker = worker;
            this.buffer = new byte[BUFFER_SIZE];
        }

        public Socket Worker
        {
            get { return this.worker; }
            set { this.worker = value; }
        }

        public byte[] Buffer
        {
            get { return this.buffer; }
            set { this.buffer = value; }
        }

        public int BufferSize
        {
            get { return BUFFER_SIZE; }
        }
    } // end of class StateObject

    /// <summary>
    /// 비동기 소켓에서 발생한 에러 처리를 위한 이벤트 Argument Class
    /// </summary>
    public class AsyncSocketErrorEventArgs : EventArgs
    {
        private readonly Exception exception;
        private readonly int id = 0;
        private SocketErrorType? errortype;

        public AsyncSocketErrorEventArgs(int id, Exception exception, SocketErrorType? error = null)
        {
            this.id = id;
            this.exception = exception;
            this.errortype = error;
        }

        public Exception AsyncSocketException
        {
            get { return this.exception; }
        }

        public int ID
        {
            get { return this.id; }
        }

        public SocketErrorType? ErrorType
        {
            get { return this.errortype; }
        }
    }

    /// <summary>
    /// 비동기 소켓의 연결 및 연결해제 이벤트 처리를 위한 Argument Class
    /// </summary>
    public class AsyncSocketConnectionEventArgs : EventArgs
    {
        private readonly int id = 0;

        public AsyncSocketConnectionEventArgs(int id)
        {
            this.id = id;
        }

        public int ID
        {
            get { return this.id; }
        }
    }

    /// <summary>
    /// 비동기 소캣의 데이터 전송 이벤트 처리를 위한 Argument Class
    /// </summary>
    public class AsyncSocketSendEventArgs : EventArgs
    {
        private readonly int id = 0;
        private readonly int sendBytes;

        public AsyncSocketSendEventArgs(int id, int sendBytes)
        {
            this.id = id;
            this.sendBytes = sendBytes;
        }

        public int SendBytes
        {
            get { return this.sendBytes; }
        }

        public int ID
        {
            get { return this.id; }
        }
    }

    /// <summary>
    /// 비동기 소켓의 데이터 수신 이벤트 처리를 위한 Argument Class
    /// </summary>
    public class AsyncSocketReceiveEventArgs : EventArgs
    {
        private readonly int id = 0;
        private readonly int receiveBytes;
        private readonly byte[] receiveData;

        public AsyncSocketReceiveEventArgs(int id, int receiveBytes, byte[] receiveData)
        {
            this.id = id;
            this.receiveBytes = receiveBytes;
            this.receiveData = receiveData;
        }

        public int ReceiveBytes
        {
            get { return this.receiveBytes; }
        }

        public byte[] ReceiveData
        {
            get { return this.receiveData; }
        }

        public int ID
        {
            get { return this.id; }
        }
    }

    /// <summary>
    /// 비동기 서버의 Accept 이벤트를 위한 Argument Class
    /// </summary>
    public class AsyncSocketAcceptEventArgs : EventArgs
    {
        private readonly Socket conn;

        public AsyncSocketAcceptEventArgs(Socket conn)
        {
            this.conn = conn;
        }

        public Socket Worker
        {
            get { return this.conn; }
        }
    }

    ///
    /// delegate 정의
    /// 
    public delegate void AsyncSocketErrorEventHandler(object sender, AsyncSocketErrorEventArgs e);
    public delegate void AsyncSocketConnectEventHandler(object sender, AsyncSocketConnectionEventArgs e);
    public delegate void AsyncSocketCloseEventHandler(object sender, AsyncSocketConnectionEventArgs e);
    public delegate void AsyncSocketSendEventHandler(object sender, AsyncSocketSendEventArgs e);
    public delegate void AsyncSocketReceiveEventHandler(object sender, AsyncSocketReceiveEventArgs e);
    public delegate void AsyncSocketAcceptEventHandler(object sender, AsyncSocketAcceptEventArgs e);

    public class AsyncSocketClass
    {
        protected int id;

        // Event Handler
        public event AsyncSocketErrorEventHandler OnError;
        public event AsyncSocketConnectEventHandler OnConnect;
        public event AsyncSocketCloseEventHandler OnClose;
        public event AsyncSocketSendEventHandler OnSend;
        public event AsyncSocketReceiveEventHandler OnReceive;
        public event AsyncSocketAcceptEventHandler OnAccept;

        public AsyncSocketClass()
        {
            this.id = -1;
        }

        public AsyncSocketClass(int id)
        {
            this.id = id;
        }

        public int ID
        {
            get { return this.id; }
        }

        protected virtual void ErrorOccured(AsyncSocketErrorEventArgs e)
        {
            AsyncSocketErrorEventHandler handler = OnError;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void Connected(AsyncSocketConnectionEventArgs e)
        {
            AsyncSocketConnectEventHandler handler = OnConnect;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void Closed(AsyncSocketConnectionEventArgs e)
        {
            AsyncSocketCloseEventHandler handler = OnClose;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void Sent(AsyncSocketSendEventArgs e)
        {
            AsyncSocketSendEventHandler handler = OnSend;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void Received(AsyncSocketReceiveEventArgs e)
        {
            AsyncSocketReceiveEventHandler handler = OnReceive;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void Accepted(AsyncSocketAcceptEventArgs e)
        {
            AsyncSocketAcceptEventHandler handler = OnAccept;

            if (handler != null)
                handler(this, e);
        }

    } // end of class AsyncSocketClass

    /// <summary>
    /// 비동기 소켓
    /// </summary>
    public class AsyncSocketClient : AsyncSocketClass
    {
        private Socket conn = null;     // connection socket
        private List<byte> TempReceiveBuffer;

        public AsyncSocketClient(int id)
        {
            this.id = id;
        }

        public AsyncSocketClient(int id, Socket conn)
        {
            this.id = id;
            this.conn = conn;
        }

        public Socket Connection
        {
            get { return this.conn; }
            set { this.conn = value; }
        }
        private void SetKeepAlive(Socket client_socket, bool enabled, int keep_alive_time, int keep_alive_interval)
        {
            MemoryStream in_vals_stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(in_vals_stream);

            writer.Write((int)(enabled == true ? 1 : 0));
            writer.Write(keep_alive_time);
            writer.Write(keep_alive_interval);

            try
            {
                byte[] keep_alive_values = in_vals_stream.ToArray();
                client_socket.IOControl(IOControlCode.KeepAliveValues, keep_alive_values, null);
            }
            catch
            {
                throw;
            }
            finally
            {
                in_vals_stream.Dispose();
            }
        }

        /// <summary>
        /// 연결을 시도한다.
        /// </summary>
        /// <param name="hostAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        /// 
        public bool Connect(string hostAddress, int port)
        {
            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(hostAddress);
                IPEndPoint remoteEP = new IPEndPoint(ips[0], port);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(remoteEP, new AsyncCallback(OnConnectCallback), client);
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e, SocketErrorType.ConnectedError);
                ErrorOccured(eev);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 연결 요청 처리 콜백 함수
        /// </summary>
        /// <param name="ar"></param>
        private void OnConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                // 보류 중인 연결을 완성한다.
                client.EndConnect(ar);

                conn = client;

                // 연결에 성공하였다면, 데이터 수신을 대기한다.
                Receive();

                // 연결 성공 이벤트를 날린다.
                AsyncSocketConnectionEventArgs cev = new AsyncSocketConnectionEventArgs(this.id);

                Connected(cev);
            }
            catch (System.Exception e)
            {

                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e, SocketErrorType.OnConnectCallbackError);

                ErrorOccured(eev);
            }
        }

        /// <summary>
        /// 데이터 수신을 비동기적으로 처리
        /// </summary>
        public void Receive()
        {
            try
            {
                StateObject so = new StateObject(conn);

                so.Worker.BeginReceive(so.Buffer, 0, so.BufferSize, 0, new AsyncCallback(OnReceiveCallBack), so);
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e, SocketErrorType.ReceivedError);

                ErrorOccured(eev);
            }
        }

        /// <summary>
        /// 데이터 수신 처리 콜백 함수
        /// </summary>
        /// <param name="ar"></param>
        private void OnReceiveCallBack(IAsyncResult ar)
        {
            try
            {

                StateObject so = (StateObject)ar.AsyncState;

                int bytesRead = so.Worker.EndReceive(ar);
                TempReceiveBuffer = new List<byte>();
                if (bytesRead > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        TempReceiveBuffer.Add(so.Buffer[i]);
                    }
                    // 데이터 수신 이벤트를 처리한다
                    AsyncSocketReceiveEventArgs rev = new AsyncSocketReceiveEventArgs(this.id, TempReceiveBuffer.Count, TempReceiveBuffer.ToArray());
                    Received(rev);
                }
                // 다음 읽을 데이터를 처리한다.
                Receive();
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e, SocketErrorType.OnReceiveCallBackError);

                ErrorOccured(eev);
            }
        }

        /// <summary>
        /// 데이터 송신을 비동기적으로 처리
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool Send(byte[] buffer)
        {
            try
            {
                Socket client = conn;

                client.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(OnSendCallBack), client);
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e, SocketErrorType.SendError);

                ErrorOccured(eev);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 데이터 송신 처리 콜백 함수
        /// </summary>
        /// <param name="ar"></param>
        private void OnSendCallBack(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesWritten = client.EndSend(ar);

                AsyncSocketSendEventArgs sev = new AsyncSocketSendEventArgs(this.id, bytesWritten);

                Sent(sev);
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e, SocketErrorType.OnSendCallBackError);

                ErrorOccured(eev);
            }
        }

        /// <summary>
        /// 소켓 연결을 비동기적으로 종료
        /// </summary>
        public void Close()
        {
            try
            {
                Socket client = conn;

                client.Shutdown(SocketShutdown.Both);
                client.BeginDisconnect(false, new AsyncCallback(OnCloseCallBack), client);
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e, SocketErrorType.CloseError);

                ErrorOccured(eev);
            }
        }

        /// <summary>
        /// 소켓 연결 종료를 처리하는 콜백 함수
        /// </summary>
        /// <param name="ar"></param>
        private void OnCloseCallBack(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndDisconnect(ar);
                client.Close();

                AsyncSocketConnectionEventArgs cev = new AsyncSocketConnectionEventArgs(this.id);

                Closed(cev);

                client.Dispose();
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e, SocketErrorType.OnCloseCallBackError);

                ErrorOccured(eev);
            }
        }

    } // end of class AsyncSocketClient

    /// <summary>
    /// 비동기 방식의 서버 
    /// </summary>
    public class AsyncSocketServer : AsyncSocketClass
    {
        private const int backLog = 100;

        private int port;
        private Socket listener;

        public AsyncSocketServer(int port)
        {
            this.port = port;
        }

        public int Port
        {
            get { return this.port; }
        }

        public void Listen()
        {
            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(new IPEndPoint(IPAddress.Any, this.port));
                listener.Listen(backLog);

                StartAccept();
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e);

                ErrorOccured(eev);
            }
        }

        /// <summary>
        /// Client의 접속을 비동기적으로 대기한다.
        /// </summary>
        /// <returns></returns>
        private void StartAccept()
        {
            try
            {
                listener.BeginAccept(new AsyncCallback(OnListenCallBack), listener);
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e);

                ErrorOccured(eev);
            }
        }

        /// <summary>
        /// Client의 비동기 접속을 처리한다.
        /// </summary>
        /// <param name="ar"></param>
        private void OnListenCallBack(IAsyncResult ar)
        {
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                Socket worker = listener.EndAccept(ar);

                // Client를 Accept 했다고 Event를 발생시킨다.
                AsyncSocketAcceptEventArgs aev = new AsyncSocketAcceptEventArgs(worker);

                Accepted(aev);

                // 다시 새로운 클라이언트의 접속을 기다린다.
                StartAccept();
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e);

                ErrorOccured(eev);
            }
        }

        public void Stop()
        {
            try
            {
                if (listener != null)
                {
                    if (listener.IsBound)
                        listener.Close(100);
                }
            }
            catch (System.Exception e)
            {
                AsyncSocketErrorEventArgs eev = new AsyncSocketErrorEventArgs(this.id, e);

                ErrorOccured(eev);
            }
        }

    } // end of class AsyncSocketServer
    #endregion
}
