using CommonUtil;
using Microsoft.Win32;
using SocketTester.Globals;
using SocketTester.Helper;
using SocketTester.IO.Robot;
using SocketTester.Model;
using SocketTester.MVVM;
using SocketTester.Robot;
using SocketTester.Services;
using SocketTester.UI.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private bool _isAllSelected;
        public bool IsAllSelected
        {
            get => _isAllSelected;
            set
            {
                if (_isAllSelected != value)
                {
                    _isAllSelected = value;
                    OnPropertyChanged(nameof(IsAllSelected));
                    UpdateSelection(value);
                }
            }
        }

        private void UpdateSelection(bool isSelected)
        {
            foreach(var item in ClientRepository.GetAllItems())
            {
                ClientRepository.Update(item, (p) =>
                {
                    p.IsSelected = isSelected;
                });
                
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

            //IAPFunctionCheckEvnet.Add((1,RobotIOConstant.IO_SUB_CMD_IAP_MODE_SETTING), new ManualResetEvent(false));
            //IAPFunctionCheckEvnet.Add((1,RobotIOConstant.IO_SUB_CMD_IAP_ENTRANCE), new ManualResetEvent(false));
            //IAPFunctionCheckEvnet.Add((1,RobotIOConstant.IO_SUB_CMD_IAP_DATA_WRITE_RESULT), new ManualResetEvent(false));
            //IAPFunctionCheckEvnet.Add((1,RobotIOConstant.IO_SUB_CMD_IAP_WRITE_COMPLETE_RESULT), new ManualResetEvent(false));


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

                                        case RobotIOConstant.IO_CMD_COMMON_IAP:
                                            switch (iOResult.SubCommand)
                                            {
                                                case RobotIOConstant.IO_SUB_CMD_IAP_MODE_SETTING:
                                                    ClientRepository.Update(client, p =>
                                                    {
                                                        p.iAPUIResult = new ClientModel.IAPUIResult()
                                                        {
                                                             IAPMode = iOResult.IAPModeResult,
                                                        };
                                                    });

                                                    client.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_MODE_SETTING].Set();
                                                    break;

                                                case RobotIOConstant.IO_SUB_CMD_IAP_ENTRANCE:
                                                    ClientRepository.Update(client, p =>
                                                    {
                                                        p.iAPUIResult = new ClientModel.IAPUIResult()
                                                        {
                                                            AciotnResult = iOResult.IAPActionResult,
                                                        };
                                                    });

                                                    client.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_ENTRANCE].Set();
                                                    break;

                                                case RobotIOConstant.IO_SUB_CMD_IAP_DATA_WRITE_RESULT:
                                                    ClientRepository.Update(client, p =>
                                                    {
                                                        p.iAPUIResult = new ClientModel.IAPUIResult()
                                                        {
                                                            AciotnResult = iOResult.IAPActionResult,
                                                        };
                                                    });

                                                    client.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_DATA_WRITE_RESULT].Set();

                                                    break;

                                                case RobotIOConstant.IO_SUB_CMD_IAP_WRITE_COMPLETE_RESULT:
                                                    ClientRepository.Update(client, p =>
                                                    {
                                                        p.iAPUIResult = new ClientModel.IAPUIResult()
                                                        {
                                                            AciotnResult = iOResult.IAPActionResult,
                                                        };
                                                    });

                                                    client.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_WRITE_COMPLETE_RESULT].Set();

                                                    break;
                                            }
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

        private RelayCommand _executeIAPFunctionCommand;

        public ICommand ExecuteIAPFunctionCommand
        {
            get { return _executeIAPFunctionCommand ?? (_executeIAPFunctionCommand = new RelayCommand(this.ExecuteIAPFunctionMethod)); }
        }

        private void SendClientCommandMethod(object obj)
        {
            int[] parameter = (int[])obj;

            int clientId = parameter[0];
            byte command = (byte)parameter[1];

            SocketMediator.SendMessageToServer(clientId, command);
            
        }

        private void ExecuteIAPFunctionMethod(object obj)
        {
            try
            {
                var isAnySelected = ClientRepository.GetAllItems().Any(p => p.IsSelected);

                var items = ClientRepository.GetAllItems();

                if (!isAnySelected)
                {
                    MessageBox.Show("There are no items selected.");
                    return;
                }

                var functionType = (IAPFunctionType)obj;

                switch (functionType)
                {
                    case IAPFunctionType.NormalModeSetting:
                        foreach (var item in ClientRepository.GetAllItems().Where(p => p.IsSelected == true))
                        {
                            SocketMediator.SendMessageToServer(item.ClientId,
                                                                    RobotIOConstant.IO_CMD_COMMON_IAP,
                                                                    RobotProtocolProcessor.ConvertIAPModeDatasToByte(IAP_MODE.IAP_MODE_NORMAL));
                        }
                        break;
                    case IAPFunctionType.IAPModeSetting:
                        foreach (var item in ClientRepository.GetAllItems().Where(p => p.IsSelected == true))
                        {
                            if (item.IsSelected) SocketMediator.SendMessageToServer(item.ClientId,
                                                                RobotIOConstant.IO_CMD_COMMON_IAP,
                                                                RobotProtocolProcessor.ConvertIAPModeDatasToByte(IAP_MODE.IAP_MODE_ENTRANCE));
                        }  

                        break;
                    case IAPFunctionType.FileLoad:
                        {
                            LoadDockingLiftBinaryFile();
                        }
                        break;

                    case IAPFunctionType.IAPStop:

                    case IAPFunctionType.IAPStart:
                        {
                            foreach (var item in ClientRepository.GetAllItems().Where(p => p.IsSelected == true))
                            {
                                if(item.FilePath == String.Empty)
                                {
                                    ClientRepository.Update(item, (p) =>
                                    {
                                        p.IAPStatus = "IAP Start Fail";
                                        p.IAPDescription = $"Please load the binary file.";

                                        Log.Info($"[IAP Function][Client {p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}");


                                    });
                                }
                                else
                                {
                                    RunIAPStartMethod(item);
                                }
                            }
                        }
                        break;
                }
                
            }
            catch (Exception ex)
            {

            }
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

        private void LoadDockingLiftBinaryFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // 기본 확장자를 Bin으로, Filter로 binary 파일만 보이게한다.
            dlg.DefaultExt = ".bin";
            dlg.Filter = "Bin files (*.bin)|*.bin";
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //|All files (*.*)|*.*


            //Open File Dialog 창을 띄움.
            Nullable<bool> result = dlg.ShowDialog();

            // Text 박스에 선택된 파일 이름을 띄운다.
            if (result == true)
            {
                string fileConent = String.Empty;
                string drawerNumber = "31CG10126"; //도킹 리프트 도번
                                                   // Open document 
                using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] binaryData = new byte[br.BaseStream.Length];

                        binaryData = br.ReadBytes((int)br.BaseStream.Length).ToArray();

                        fileConent = Encoding.UTF8.GetString(binaryData);

                        br.Close();
                        fs.Close();
                    }
                }

                if (fileConent.Contains(drawerNumber))
                {
                    //MessageBox.Show("도킹 리프트 도번 존재");
                    foreach (var item in ClientRepository.GetAllItems().Where(p => p.IsSelected == true))
                    {
                        FileInfo fileInfo = new FileInfo(dlg.FileName);

                        ClientRepository.Update(item, (p) =>
                        {
                            p.FilePath = dlg.FileName;
                            p.FileSize = fileInfo.Length.ToString();

                            p.IAPStatus = "File Load Success";
                            p.IAPDescription = $"FilePath : {p.FilePath}, FileSize : {p.FileSize} bytes";

                            Log.Info($"[IAP Function][Client {p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}");


                        });
                    }
                }
                else
                {
                    foreach (var item in ClientRepository.GetAllItems().Where(p => p.IsSelected == true))
                    {
                        ClientRepository.Update(item, (p) =>
                        {
                            p.FilePath = String.Empty;
                            p.FileSize = String.Empty;

                            p.IAPStatus = "File Load Fail";
                            p.IAPDescription = $"The drawing number in the file does not match the docking lift board drawing number({drawerNumber}).";
                            Log.Info($"[IAP Function][Client {p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}");

                        });
                    }
                }
            }


        }

        private void RunIAPStartMethod(ClientModel item)
        {
            bool isIapProgress = true;

            int bufferSize = 1024;

            byte[] pageDatas;

            StringBuilder requestDatasString = new StringBuilder();


            using (FileStream fileStream = new FileStream(item.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int fileStreamLength = (int)fileStream.Length;

                item.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_MODE_SETTING].Reset();

                //IAP 모드 설정 명령 전송
                SocketMediator.SendMessageToServer(item.ClientId,
                    RobotIOConstant.IO_CMD_COMMON_IAP
                    , RobotProtocolProcessor.ConvertIAPModeDatasToByte(IAP_MODE.IAP_MODE_ENTRANCE));

                isIapProgress = item.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_MODE_SETTING].WaitOne(2000);

                if (isIapProgress)
                {
                    if (item.iAPUIResult.IAPMode == IAP_MODE.IAP_MODE_ENTRANCE)
                    {
                        ClientRepository.Update(item, (p) =>
                        {
                            p.IAPStatus = "IAP Mode setting successful(0x20, 0x84)";
                            p.IAPDescription = $"The change to IAP mode has been completed. Resopnse Datas: [0x{item.iAPUIResult.IAPMode:X2}]";
                            
                            Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                        });
                    }
                    else
                    {
                        ClientRepository.Update(item, (p) =>
                        {
                            p.IAPStatus = "Normal mode setting successful(0x20, 0x84)";
                            p.IAPDescription = $"\r\nChange to Normal mode was successful. Resopnse Datas: [0x{item.iAPUIResult.IAPMode:X2}]";

                            Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                        });
                    }
                }
                else
                {
                    ClientRepository.Update(item, (p) =>
                    {
                        p.IAPStatus = "IAP Mode Setting Timeout(0x20, 0x84)";
                        p.IAPDescription = $"A timeout occurred in the IAP mode change command.";

                        Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                    });
                }

                if (isIapProgress == false) return;

                item.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_ENTRANCE].Reset();

                var iapEntranceDatas = RobotProtocolProcessor.ConvertIAPEntranceDatasToByte(2041, fileStreamLength);

                //IAP 진입 명령 전송
                SocketMediator.SendMessageToServer(item.ClientId,
                    RobotIOConstant.IO_CMD_COMMON_IAP
                    , iapEntranceDatas);

                requestDatasString.Clear();
                foreach (var data in iapEntranceDatas)
                {
                    requestDatasString.Append($"0x{data:X2} ");
                }

                isIapProgress = item.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_ENTRANCE].WaitOne(2000);

                if (isIapProgress == true)
                {
                    if (item.iAPUIResult.AciotnResult == IAP_ACTION_RESULT.IAP_ACTION_SUCCESS)
                    {
                        ClientRepository.Update(item, (p) =>
                        {
                            p.IAPStatus = "Successfully entered IAP(0x20, 0x85)";
                            p.IAPDescription = $"Board ID : 2041, FileSize: {fileStreamLength}, Request Datas: [{requestDatasString.ToString()}], Resopnse Datas: [0x{item.iAPUIResult.AciotnResult:X2}]";

                            Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                        });
                    }
                    else
                    {
                        ClientRepository.Update(item, (p) =>
                        {
                            p.IAPStatus = "IAP entry failure(0x20, 0x85)";
                            p.IAPDescription = $"Board ID : 2041, FileSize: {fileStreamLength}, Request Datas: [{requestDatasString.ToString()}], Resopnse Datas: [0x{item.iAPUIResult.AciotnResult:X2}]";

                            Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                        });
                    }
                }
                else
                {
                    ClientRepository.Update(item, (p) =>
                    {
                        p.IAPStatus = "IAP Entrance Timeout(0x20, 0x85)";
                        p.IAPDescription = $"A timeout occurred in the IAP Entrance command.";

                        Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                    });
                }

                if (isIapProgress == false) return;

                int numBytesReadPosition = 0;

                pageDatas = new byte[bufferSize];

                int fileReadLength = fileStream.Read(pageDatas, numBytesReadPosition, pageDatas.Length);

                int pageCount = 0;

                while (fileReadLength > 0
                   && isIapProgress == true)
                {
                    pageCount++;

                    long remainingBytes = fileStream.Length - fileStream.Position;
                    if (remainingBytes < 1024)
                    {
                        pageDatas = new byte[remainingBytes];
                    }
                    item.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_DATA_WRITE_RESULT].Reset();

                    //IAP PAGE WRITE 명령 전송 
                    SocketMediator.SendMessageToServer(item.ClientId,
                    RobotIOConstant.IO_CMD_COMMON_IAP
                    , RobotProtocolProcessor.ConvertIAPPageDatasToReverseByte(pageDatas));

                    isIapProgress = item.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_DATA_WRITE_RESULT].WaitOne(2000);

                    if (isIapProgress == true)
                    {
                        if (item.iAPUIResult.AciotnResult == IAP_ACTION_RESULT.IAP_ACTION_SUCCESS)
                        {
                            ClientRepository.Update(item, (p) =>
                            {
                                p.IAPStatus = "IAP Page Write Successful(0x20, 0x87)";
                                p.IAPDescription = $"IAP {pageCount} Page Write was successful({fileReadLength}bytes). Remaining bytes : {remainingBytes}. Resopnse Datas: [0x{item.iAPUIResult.AciotnResult:X2}]. ";

                                Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                            });
                        }
                        else
                        {
                            ClientRepository.Update(item, (p) =>
                            {
                                p.IAPStatus = "IAP Page Write Failure(0x20, 0x87)";
                                p.IAPDescription = $"IAP {pageCount} Page Write failed({fileReadLength}bytes). Remaining bytes : {remainingBytes}. Resopnse Datas: [0x{item.iAPUIResult.AciotnResult:X2}].";

                                Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                            });
                        }
                    }
                    else
                    {
                        ClientRepository.Update(item, (p) =>
                        {
                            p.IAPStatus = "IAP Page Write Timeout(0x20, 0x87)";
                            p.IAPDescription = $"A timeout occurred in the IAP Page Write command.";

                            Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                        });

                    }

                    fileReadLength = fileStream.Read(pageDatas, numBytesReadPosition, pageDatas.Length);
                }

                if (isIapProgress == false) return;

                item.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_WRITE_COMPLETE_RESULT].Reset();

                //IAP FIRMWARE WRITE 완료 명령 전송
                SocketMediator.SendMessageToServer(item.ClientId,
                    RobotIOConstant.IO_CMD_COMMON_IAP
                    , RobotProtocolProcessor.ConvertIAPWriteCompleteDatasToByte());


                isIapProgress = item.CommandCheckEvnet[RobotIOConstant.IO_SUB_CMD_IAP_WRITE_COMPLETE_RESULT].WaitOne(2000);

                if (isIapProgress)
                {
                    if (item.iAPUIResult.AciotnResult == IAP_ACTION_RESULT.IAP_ACTION_SUCCESS)
                    {
                        ClientRepository.Update(item, (p) =>
                        {
                            p.IAPStatus = "IAP Page Write Complete Successful(0x20, 0x89)";
                            p.IAPDescription = $"IAP Page Write Complete was successful. Resopnse Datas: [0x{item.iAPUIResult.AciotnResult:X2}].";

                            Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                        });
                    }
                    else
                    {
                        ClientRepository.Update(item, (p) =>
                        {
                            p.IAPStatus = "IAP Page Write Complete Successful(0x20, 0x89)";
                            p.IAPDescription = $"IAP Page Write Complete failed. Resopnse Datas: [0x{item.iAPUIResult.AciotnResult:X2}].";

                            Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                        });
                    }
                }
                else
                {
                    ClientRepository.Update(item, (p) =>
                    {
                        p.IAPStatus = "IAP Page Write Complete Timeout(0x20, 0x89)";
                        p.IAPDescription = $"A timeout occurred in the IAP Page Write Complete command.";

                        Log.Info2(null, null, $"[IAP Function][Client{p.ClientId}][{p.IpAddress}:{p.Port}]{p.IAPStatus}. {p.IAPDescription}", ApplicationConfig.RobotIOParserFileName);
                    });

                }
            }

        }

    }
}
