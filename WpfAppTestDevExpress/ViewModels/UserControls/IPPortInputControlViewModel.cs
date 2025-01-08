using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTestDevExpress.ViewModels.UserControls
{
    public class IPPortInputControlViewModel
    {
        private string _ipAddress;
        private string _port;

        private string _ipAddressError;
        private bool _ipAddressIsValid = true;

        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                ValidateIpAddress(value);
                OnPropertyChanged(nameof(IpAddress));
            }
        }

        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        public string IpAddressError
        {
            get => _ipAddressError;
            set
            {
                _ipAddressError = value;
                OnPropertyChanged(nameof(IpAddressError));
            }
        }

        public bool IpAddressIsValid
        {
            get => _ipAddressIsValid;
            set
            {
                _ipAddressIsValid = value;
                OnPropertyChanged(nameof(IpAddressIsValid));
            }
        }



        private void ValidateIpAddress(string ip)
        {
            if(String.IsNullOrWhiteSpace(ip) || !System.Net.IPAddress.TryParse(ip, out _))
            {
                IpAddressError = "Invalid IP Address. Example: 192.168.0.1";
                IpAddressIsValid = false;
            }
            else
            {
                IpAddressError = String.Empty;
                IpAddressIsValid = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
