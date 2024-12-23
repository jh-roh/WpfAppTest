﻿using SocketTester.Helper;
using SocketTester.IO.Robot;
using SocketTester.Model;
using SocketTester.MVVM;
using SocketTester.Robot;
using SocketTester.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace SocketTester
{
    public class MainWindowModel : PropertyChangedBase
    {
        object _lock = new object();

        private ObservableCollection<ClientModel> _clients;
        public ObservableCollection<ClientModel> Clients
        {
            get
            {
                return _clients;
            }
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        private String _processMessage;

        public String ProcessMessage
        {
            get
            {
                return _processMessage;
            }
            set
            {
                _processMessage = value;
                OnPropertyChanged();
            }
        }

        private int _processMessageMaxLength;

        public int ProcessMessageMaxLength
        {
            get
            {
                return _processMessageMaxLength;
            }
            set
            {
                _processMessageMaxLength = value;
                OnPropertyChanged();
            }
        }
        public MainWindowModel()
        {
            ProcessMessageMaxLength = 65535;
            ProcessMessage = "";
            ProtocolMessageManager.NotifyProtocolMessage();
            ProtocolMessageManager.MessageNotify += ProtocolMessageManager_MessageNotify;


            Clients = new ObservableCollection<ClientModel>();
            Clients.Add(new ClientModel
            {
                ClientId = 1,
                IpAddress = "192.168.125.171", // 기본값
                Port = 4001,           // 기본값
                ConnectCommand = new RelayCommand(this.ClientConnectMethod),
                DisconnectCommand = new RelayCommand(this.ClientDisconnectMethod),
                IsConnected = false
            });

            Clients.Add(new ClientModel
            {
                ClientId = 2,
                IpAddress = "192.168.125.172", // 기본값
                Port = 4001,           // 기본값
                ConnectCommand = new RelayCommand(this.ClientConnectMethod),
                DisconnectCommand = new RelayCommand(this.ClientDisconnectMethod),
                IsConnected =false
            });

            foreach (var item in Clients)
            {
                SocketMediator.RegisterClient(item.ClientId);

            }

            var receiveProcessThread = new Thread(() =>
            {
                while (true)
                {
                    var iOResult = SocketMediator.RobotIoResult.Take();

                    switch(iOResult.HandlerType)
                    {
                        case SocketHandlerType.Close:
                        case SocketHandlerType.Connect:
                            var client = Clients.FirstOrDefault(p => p.ClientId == iOResult.ClientId);

                            if (client != null)
                            {
                                client.IsConnected = iOResult.HandlerType  == SocketHandlerType.Connect ? true : false;
                            }

                            break;


                        case SocketHandlerType.Receive:

                            break;
                    }

                }

            });

            receiveProcessThread.IsBackground = true;
            receiveProcessThread.SetApartmentState(ApartmentState.STA);
            receiveProcessThread.Start();
        }

        private void ProtocolMessageManager_MessageNotify(string e)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(e);
            if(ProcessMessage.Length > ProcessMessageMaxLength) { ProcessMessage = ""; }
            ProcessMessage += message.ToString();
        }

        private RelayCommand _addNewClientCommand;

        public ICommand AddNewClientCommand
        {
            get { return _addNewClientCommand ?? (_addNewClientCommand = new RelayCommand(this.AddNewClientMethod)); }
        }

        private RelayCommand _sendClientCommand;

        public ICommand SendClientCommand
        {
            get { return _sendClientCommand ?? (_sendClientCommand = new RelayCommand(this.SendClientCommandMethod)); }
        }



        private void SendClientCommandMethod(object obj)
        {
            int[] parameter = (int[])obj;

            int clientId = parameter[0];
            byte command = (byte)parameter[1];

            SocketMediator.SendMessageToServer(clientId, command);
            
        }

        private void ClientDisconnectMethod(object obj)
        {
            int clientId = Convert.ToInt32(obj);

            var client = Clients.FirstOrDefault(p => p.ClientId == clientId);

            if (client != null)
            {
                SocketMediator.DisconnectClient(clientId);
            }
            
        }

        private void ClientConnectMethod(object obj)
        {
            int clientId = Convert.ToInt32(obj);

            var client = Clients.FirstOrDefault(p => p.ClientId == clientId);

            if(client != null)
            {
                SocketMediator.ConnectClient(client.ClientId, client.IpAddress, client.Port);
            }
        }



        private void AddNewClientMethod(object obj)
        {
            int maxId = Clients.Max(p => p.ClientId);

            maxId += 1;

            Clients.Add(new ClientModel
            {
                ClientId = maxId,
                IpAddress = "",      // 기본값
                Port = 0,           // 기본값
                ConnectCommand = new RelayCommand(this.ClientConnectMethod),
                DisconnectCommand = new RelayCommand(this.ClientDisconnectMethod),
            });

        }

    }
}
