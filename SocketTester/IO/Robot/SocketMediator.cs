﻿using SocketTester.IO.Robot;
using SocketTester.Robot;
using SocketTester.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

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

        public static async Task SendMessageToServer(int? clientId, byte command, byte[] datas = null)
        {
            if (clientId == null)
                throw new ArgumentException("Client ID cannot be null or empty.", nameof(clientId));

            if (_clients.TryGetValue(clientId, out var manager))
            {
                RobotIOSend? robotIOSend = null;

                switch (command)
                {
                    case RobotIOConstant.IO_CMD_ROBOT_CALL_REQUEST:
                        datas = new byte[] { 0x00 };
                        robotIOSend = new RobotIOSend
                        {
                            Command = RobotIOConstant.IO_CMD_ROBOT_CALL_REQUEST,
                            DataLength = (byte)datas.Length,
                            DataArray = datas.ToArray()
                        };
                        break;

                    case RobotIOConstant.IO_CMD_ROBOT_APPROACH_REQUEST:
                        robotIOSend = new RobotIOSend
                        {
                            Command = RobotIOConstant.IO_CMD_ROBOT_APPROACH_REQUEST,
                            DataLength = 0,
                            DataArray = null
                        };
                        break;

                    case RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_SW:
                        robotIOSend = new RobotIOSend
                        {
                            Command = RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_SW,
                            DataLength = 0,
                            DataArray = null,
                        };
                        break;

                    case RobotIOConstant.IO_CMD_ROBOT_IN_COMPLETED_EVENT:
                        robotIOSend = new RobotIOSend
                        {
                            Command = RobotIOConstant.IO_CMD_ROBOT_IN_COMPLETED_EVENT,
                            DataLength = 0,
                            DataArray = null,
                        };
                        break;

                    case RobotIOConstant.IO_CMD_ROBOT_OUT_COMPLETED_EVENT:
                        robotIOSend = new RobotIOSend
                        {
                            Command = RobotIOConstant.IO_CMD_ROBOT_OUT_COMPLETED_EVENT,
                            DataLength = 0,
                            DataArray = null,
                        };

                        break;

                    case RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_HW:
                        robotIOSend = new RobotIOSend
                        {
                            Command = RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_HW,
                            DataLength = 0,
                            DataArray = null,
                        };
                        break;

                    case RobotIOConstant.IO_CMD_COMMON_IAP:
                        robotIOSend = new RobotIOSend
                        {
                            Command = RobotIOConstant.IO_CMD_COMMON_IAP,
                            DataLength = (byte)datas.Length,
                            DataArray = datas.ToArray(),
                        };

                        break;


                }

                var sendData = RobotProtocolProcessor.ConstructRobot((RobotIOSend)robotIOSend);
                manager.SendDataToQueue(sendData);

                if (command != RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_HW
                 && command != RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_SW)
                {
                    StringBuilder sendLog = new StringBuilder();

                    string commandName = UtilHelper.GetConstantName(typeof(RobotIOConstant), command);
                   
                    StringBuilder dataString = new StringBuilder();
                    dataString.Append("NONE");
                    if (robotIOSend.Value.DataArray != null)
                    {
                        dataString.Clear();

                        foreach (var item in robotIOSend.Value.DataArray)
                        {
                            dataString.Append($"0x{item:X2} ");
                        }
                    }


                    sendLog.Append($"[SEND][Client{clientId}][{commandName}(0x{command:X2})][DATA : {dataString}](RawData)");

                    foreach (var item in sendData)
                    {
                        sendLog.Append($"{item:X2} ");
                    }

                    ProtocolMessageManager.AddProcessMessage(sendLog.ToString());
                }
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
                StringBuilder responseLog = new StringBuilder();


                try
                {
                   
                    switch (e.HandlerType)
                    {
                        case SocketHandlerType.Close:
                        case SocketHandlerType.Connect:
                            {
                                RobotIOResult iOResult = new RobotIOResult();
                                iOResult.HandlerType = e.HandlerType;
                                iOResult.ClientId = e.ClientId;
                                RobotIoResult.Add(iOResult);

                                if(e.HandlerType == SocketHandlerType.Close)
                                {
                                    responseLog.Append($"[CLOSE][Client{e.ClientId}] Close Complete");
                                }
                                else
                                {
                                    responseLog.Append($"[CONNECT][Client{e.ClientId}] Connection Complete");

                                    ProtocolMessageManager.AddProcessMessage(responseLog.ToString());   
                                }
                            }
                            break;

                        case SocketHandlerType.Receive:
                            var parseDatas = RobotProtocolProcessor.ParseProtocol(e.ReceiveDatas);
                            foreach (var data in parseDatas)
                            {
                                RobotIOResult iOResult = RobotProtocolProcessor.AnalyzeRobot(data);
                                iOResult.HandlerType = e.HandlerType;
                                iOResult.ClientId = e.ClientId;
                                iOResult.buffer = data.ToArray();

                                if (iOResult.Command != RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_HW
                                && iOResult.Command != RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_SW)
                                {
                                    RobotIoResult.Add(iOResult);

                                    StringBuilder receveLog = new StringBuilder();
                                    foreach (var item in iOResult.buffer)
                                    {
                                        receveLog.Append($"{item:X2} ");
                                    }

                                    StringBuilder dataString = new StringBuilder();
                                    dataString.Append("NONE");
                                    if (iOResult.Datas != null)
                                    {
                                        dataString.Clear();

                                        foreach (var item in iOResult.Datas)
                                        {
                                            dataString.Append($"0x{item:X2} ");
                                        }
                                    }
                                    string commandName = UtilHelper.GetConstantName(typeof(RobotIOConstant), iOResult.Command);


                                    ProtocolMessageManager.AddProcessMessage($"[RECV][Client{e.ClientId}][{commandName}(0x{iOResult.Command:X2})][DATA : {dataString}](RawData){receveLog.ToString()}");
                                }

                                if(iOResult.Command == RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_HW)
                                {
                                    SocketMediator.SendMessageToServer(e.ClientId, RobotIOConstant.IO_CMD_ROBOT_KEEP_ALIVE_HW);
                                }
                            }

                            break;
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
