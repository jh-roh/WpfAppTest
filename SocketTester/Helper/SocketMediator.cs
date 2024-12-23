using SocketTester.Robot;
using SocketTester.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SocketTester.Helper
{
    public static class SocketMediator 
    {
        private static readonly Dictionary<int?, SocketClientManager> _clients = new Dictionary<int?, SocketClientManager>();
        
        private static object _lock = new object();
        
        public static readonly BlockingCollection<RobotIOResult> RobotIoResult = new BlockingCollection<RobotIOResult>();

        public static void RegisterClient(int? clientId)
        {
            lock (_lock)
            {
                if (clientId == null)
                    throw new ArgumentException("Client ID cannot be null or empty.", nameof(clientId));

                if (_clients.ContainsKey(clientId) == false)
                {
                    _clients[clientId] = new SocketClientManager();
                    _clients[clientId].SetSocketClient(clientId);
                    _clients[clientId].ManageHandler += ManageClientHandler;
                }
            }
        }

        public static void ConnectClient(int? clientId, string host, int port)
        {
            if (_clients.TryGetValue(clientId, out var manager))
            {
                manager.Close();
                manager.Connect(host, port);
            }
        }

        public static void DisconnectClient(int? clientId)
        {
            if (_clients.TryGetValue(clientId, out var manager))
            {
                manager.Close();
            }
        }

        public static async Task SendMessageToServer(int? clientId, byte[] data)
        {
            if (clientId == null)
                throw new ArgumentException("Client ID cannot be null or empty.", nameof(clientId));

            if (data == null || data.Length == 0)
                throw new ArgumentException("Data cannot be null or empty.", nameof(data));

            if (_clients.TryGetValue(clientId, out var manager))
            {
                manager.SendDataToQueue(data);
            }
        }

        // 클라이언트 등록 해제 메서드 (옵션)
        public static void UnregisterClient(int? clientId)
        {
            lock (_lock)
            {
                if (clientId == null)
                    throw new ArgumentNullException(nameof(clientId));


                if (_clients.ContainsKey(clientId))
                {
                    _clients[clientId].ManageHandler -= ManageClientHandler;
                    _clients[clientId].Close();
                    _clients[clientId].Dispose();
                    _clients.Remove(clientId);
                }
            }
        }

        // 등록된 모든 클라이언트 확인용 메서드 (옵션)
        public static IReadOnlyDictionary<int?, SocketClientManager> GetAllClients()
        {
            return _clients;
        }

        public static void ManageClientHandler(object sender, SocketClientEventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    RobotIOResult iOResult = new RobotIOResult();

                    switch (e.HandlerType)
                    {
                        case SocketHandlerType.Receive:
                            iOResult = CommandAnalyer.AnalyzeRobot(e.ReceiveDatas);
                            iOResult.buffer = e.ReceiveDatas;
                            break;
                    }

                    iOResult.HandlerType = e.HandlerType;
                    iOResult.ClientId = e.ClientId;
                    RobotIoResult.Add(iOResult);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
