using SocketTester.Helper;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SocketTester
{
    public class MainWindowModel : PropertyChangedBase
    {
        object _lock = new object();

        private ClientRepository<ClientModel> _clientRepository;

        public ClientRepository<ClientModel> ClientRepository
        {
            get
            {
                return _clientRepository;
            }
            set
            {
                _clientRepository = value;
                OnPropertyChanged();
            }
        }

        private ClientRepository<RobotResultModel> _robotRepository;

        public ClientRepository<RobotResultModel> RobotRepository
        {
            get
            {
                return _robotRepository;
            }
            set
            {
                _robotRepository = value;
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
            ClientRepository = new ClientRepository<ClientModel>();
            RobotRepository = new ClientRepository<RobotResultModel>();

            ClientRepository.Add(new ClientModel
            {
                ClientId = 1,
                IpAddress = "192.168.125.171", // 기본값
                Port = 4001,           // 기본값
                ConnectCommand = new RelayCommand(this.ClientConnectMethod),
                DisconnectCommand = new RelayCommand(this.ClientDisconnectMethod),
                IsConnected = false
            });

            ClientRepository.Add(new ClientModel
            {
                ClientId = 2,
                IpAddress = "192.168.125.172", // 기본값
                Port = 4001,           // 기본값
                ConnectCommand = new RelayCommand(this.ClientConnectMethod),
                DisconnectCommand = new RelayCommand(this.ClientDisconnectMethod),
                IsConnected =false
            });

            foreach (var item in ClientRepository.GetAllItems())
            {
                RobotRepository.Add(new RobotResultModel()
                {
                    ClientId = item.ClientId,
                });
                SocketMediator.RegisterClient(item.ClientId);
            }

            var receiveProcessThread = new Thread(() =>
            {
                while (true)
                {
                    var iOResult = SocketMediator.RobotIoResult.Take();
                    var client = ClientRepository.GetAllItems().FirstOrDefault(p => p.ClientId == iOResult.ClientId);

                    switch (iOResult.HandlerType)
                    {
                        case SocketHandlerType.Close:
                        case SocketHandlerType.Connect:
                            
                            if (client != null)
                            {
                                ClientRepository.Update(client, (item) => 
                                {
                                    item.IsConnected = iOResult.HandlerType == SocketHandlerType.Connect ? true : false; ;
                                });
                            }

                            break;


                        case SocketHandlerType.Receive:
                            {

                                client?.Enqueue(() =>
                                {
                                    var robotResult = RobotRepository.GetAllItems().FirstOrDefault(p => p.ClientId == iOResult.ClientId);

                                    switch (iOResult.Command)
                                    {
                                        case RobotIOConstant.IO_CMD_ROBOT_APPROACH_REQUEST:
                                            if (robotResult != null)
                                            {
                                                RobotRepository.Update(robotResult, (item) =>
                                                {
                                                    item.RobotApproachRequestResult = iOResult.ResponseActionResult?.ToTrimmedString();
                                                });
                                            }
                                            break;
                                        case RobotIOConstant.IO_CMD_ROBOT_IN_COMPLETED_EVENT:
                                            if (robotResult != null)
                                            {
                                                RobotRepository.Update(robotResult, (item) =>
                                                {
                                                    item.RobotInCompletedResult = iOResult.ResponseActionResult?.ToTrimmedString();
                                                });
                                            }
                                            break;
                                        case RobotIOConstant.IO_CMD_ROBOT_OUT_COMPLETED_EVENT:
                                            if (robotResult != null)
                                            {
                                                RobotRepository.Update(robotResult, (item) =>
                                                {
                                                    item.RobotOutCompletedResult = iOResult.ResponseActionResult?.ToTrimmedString();
                                                });
                                            }
                                            break;

                                        case RobotIOConstant.IO_CMD_ROBOT_BUTTON_PRESS_EVENT:
                                            if (robotResult != null)
                                            {
                                                RobotRepository.Update(robotResult, (item) =>
                                                {
                                                    item.ButtonPressResult = iOResult.ButtonPressResult?.ToTrimmedString();
                                                });
                                            }
                                            break;

                                        case RobotIOConstant.IO_CMD_ROBOT_CALL_REQUEST:
                                            break;

                                       
                                    }

                                    return Task.CompletedTask;
                                });
                            }
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

            var client = ClientRepository.GetAllItems().FirstOrDefault(p => p.ClientId == clientId);

            if (client != null)
            {
                SocketMediator.DisconnectClient(clientId);
            }
            
        }

        private void ClientConnectMethod(object obj)
        {
            int clientId = Convert.ToInt32(obj);

            var client = ClientRepository.GetAllItems().FirstOrDefault(p => p.ClientId == clientId);

            if(client != null)
            {
                if(client.ValidationError != null 
                && client.ValidationError.Length > 0)
                {
                    MessageBox.Show(client.ValidationError);
                    return;
                }
                    
                SocketMediator.ConnectClient(client.ClientId, client.IpAddress, client.Port);
            }
        }



        private void AddNewClientMethod(object obj)
        {
            int maxId = ClientRepository.GetAllItems().Max(p => p.ClientId);

            maxId += 1;

            ClientRepository.Add(new ClientModel
            {
                ClientId = maxId,
                IpAddress = "",      // 기본값
                Port = 0,           // 기본값
                ConnectCommand = new RelayCommand(this.ClientConnectMethod),
                DisconnectCommand = new RelayCommand(this.ClientDisconnectMethod),
            });

            RobotRepository.Add(new RobotResultModel()
            {
                ClientId = maxId,
            });
        }

    }
}
