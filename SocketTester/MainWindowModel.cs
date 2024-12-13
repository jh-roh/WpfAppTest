using SocketTester.MVVM;
using System.Windows;
using System.Windows.Input;

namespace SocketTester
{
    public class MainWindowModel : PropertyChangedBase
    {
        private string _ipAddress;

        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
                OnPropertyChanged();
            }
        }

        private int _port;
        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        
        private RelayCommand _serverStartCommand;
        public ICommand ServerStartCommand
        {
            get { return _serverStartCommand ?? (_serverStartCommand = new RelayCommand(this.ServerStartMethod)); }
        }

        private RelayCommand _serverStopCommand;

        public ICommand ServerStopCommand
        {
            get { return _serverStopCommand ?? (_serverStopCommand = new RelayCommand(this.ServerStopMethod)); }
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

        }

        private void ClientConnectMethod(object obj)
        {
            MessageBox.Show("Client Connect Click");

        }

        private void ServerStopMethod(object obj)
        {
            MessageBox.Show("Stop Server Click");

        }

        private void ServerStartMethod(object obj)
        {
            MessageBox.Show("Start Server Click");


        }

        public MainWindowModel()
        {
        }


    }
}
