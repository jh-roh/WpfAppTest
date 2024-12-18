using SocketTester.Services;
using System;
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

        public static void RegisterClient(int? clientId, SocketClientManagerEventHandler handler)
        {
            lock (_lock)
            {
                if (clientId == null)
                    throw new ArgumentException("Client ID cannot be null or empty.", nameof(clientId));

                _clients[clientId] = new SocketClientManager();
                _clients[clientId].SetSocketClient(clientId);
                _clients[clientId].ManageHandler += handler;
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
        public static void UnregisterClient(int? clientId, SocketClientManagerEventHandler handler)
        {
            lock (_lock)
            {
                if (clientId == null)
                    throw new ArgumentNullException(nameof(clientId));


                if (_clients.ContainsKey(clientId))
                {
                    _clients[clientId].ManageHandler -= handler;
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
    }
}
