using SocketTester.Helper;
using SocketTester.MVVM;
using SocketTester.Services;
using System;
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
            SocketMediator.RegisterClient(1, SocketManageHandler);
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

        private void SocketManageHandler(object sender, SocketClientEventArgs e)
        {
            lock (_lock)
            {
                try
                {
                
                    switch (e.HandlerType)
                    {
                        case SocketHandlerType.Close:
                            break;

                        case SocketHandlerType.Connect:
                            break;

                        case SocketHandlerType.Receive:
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
