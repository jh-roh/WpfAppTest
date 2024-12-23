using SocketTester.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocketTester.Model
{
    public class ClientModel : PropertyChangedBase
    {
        private int _clientId;
        public int ClientId 
        {
            get { return _clientId; } 
            set 
            {
                _clientId = value;
                OnPropertyChanged();
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }
    }

}
