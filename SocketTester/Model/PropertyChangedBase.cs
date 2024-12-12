using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SocketTester.Model
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        public PropertyChangedBase()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
