using SocketTester.Helper;
using SocketTester.MVVM;
using SocketTester.Robot;
using SocketTester.Services;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace SocketTester
{
    public class MainWindowModel : PropertyChangedBase
    {
        object _lock = new object();

        private string _ipAddress1;

        public string IpAddress1
        {
            get { return _ipAddress1; }
            set
            {
                _ipAddress1 = value;
                OnPropertyChanged();
            }
        }

        private int _port1;
        public int Port1
        {
            get { return _port1; }
            set
            {
                _port1 = value;
                OnPropertyChanged();
            }
        }

        private bool _connectedHost1;

        public bool ConnectedHost1
        {
            get { return _connectedHost1; }
            set
            {
                _connectedHost1 = value;
                OnPropertyChanged();
            }
        }

        public MainWindowModel()
        {
            IpAddress1 = "192.168.125.171";

            Port1 = 4001;

            SocketMediator.RegisterClient(1);

            var receiveProcessThread = new Thread(() =>
            {
                while (true)
                {
                    var receiveData = SocketMediator.ClientRobotIoResult[1].Take();

                    Console.WriteLine(receiveData);
                }

            });

            receiveProcessThread.IsBackground = true;
            receiveProcessThread.SetApartmentState(ApartmentState.STA);
            receiveProcessThread.Start();
        }

        private RelayCommand _clientConnectCommand;

        public ICommand ClientConnectCommand
        {
            get { return _clientConnectCommand ?? (_clientConnectCommand = new RelayCommand(this.ClientConnectMethod)); }
        }

        private RelayCommand _clientDisconnectCommand;

        public ICommand ClientDisconnectCommand
        {
            get { return _clientDisconnectCommand ?? (_clientDisconnectCommand = new RelayCommand(this.ClientDisconnectMethod)); }
        }

        private RelayCommand _sendClientCommand;

        public ICommand SendClientCommand
        {
            get { return _sendClientCommand ?? (_sendClientCommand = new RelayCommand(this.SendClientCommandMethod)); }
        }

        private void SendClientCommandMethod(object obj)
        {
            int command = Convert.ToInt32(obj);

            RobotIOSend? robotIOSend = null;

            switch(command)
            {
                case CommandProcessor.ROBOT_CALL_RECEPTION_RESPONSE:
                    robotIOSend = CommadParser.RequestRobotCallReception();
                    break;

                case CommandProcessor.ROBOT_ENTRY_POSSIBLE:
                    robotIOSend = CommadParser.ResponseRobotEntryPossible();
                    break;

                case CommandProcessor.ROBOT_KEEP_ALIVE:
                    robotIOSend = CommadParser.RequestRobotKeepAlive();
                    break;

            }

            if (robotIOSend != null)
            {
                SocketMediator.SendMessageToServer(1, CommandProcessor.ConstructRobot((RobotIOSend)robotIOSend));
            }
        }

        private void ClientDisconnectMethod(object obj)
        {
            MessageBox.Show("Client Disconnect Click");
            SocketMediator.DisconnectClient(1);
        }

        private void ClientConnectMethod(object obj)
        {
            
            MessageBox.Show("Client Connect Click");
            SocketMediator.ConnectClient(1, IpAddress1, Port1);
        }

        
    }
}
