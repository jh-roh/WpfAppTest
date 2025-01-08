using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppTestDevExpress.Views.UserControls
{
    /// <summary>
    /// IPPortInputControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IPPortInputControl : UserControl, INotifyPropertyChanged
    {
        private string _ipAddressError;
        private string _portError;
        private bool _ipAddressIsValid = true;
        private bool _portIsValid = true;


        // DependencyProperty for IpAddress
        public static readonly DependencyProperty IpAddressProperty =
            DependencyProperty.Register("IpAddress", typeof(string), typeof(IPPortInputControl), new PropertyMetadata(string.Empty));

        // DependencyProperty for Port
        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register("Port", typeof(string), typeof(IPPortInputControl), new PropertyMetadata(string.Empty));

        public string IpAddress
        {
            get => (string)GetValue(IpAddressProperty);
            set
            {
                SetValue(IpAddressProperty, value);
                ValidateIpAddress(value);
                OnPropertyChanged(nameof(IpAddress));
            }
        }

        public String Port
        {
            get => (string)GetValue(PortProperty);
            set
            {
                SetValue(PortProperty, value);
                //ValidatePort(value);
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
        public string PortError
        {
            get => _portError;
            set
            {
                _portError = value;
                OnPropertyChanged(nameof(PortError));
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

        public bool PortIsValid
        {
            get => _portIsValid;
            set
            {
                _portIsValid = value;
                OnPropertyChanged(nameof(PortIsValid));
            }
        }




        private void ValidateIpAddress(string ip)
        {
            if (String.IsNullOrWhiteSpace(ip) || !System.Net.IPAddress.TryParse(ip, out _))
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



        public IPPortInputControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_ipAddressIsValid == false)
            {
                MessageBox.Show(_ipAddressError);
            }
        }
    }
}
