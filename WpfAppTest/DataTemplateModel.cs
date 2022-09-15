using System;
using System.Windows.Input;

namespace WpfAppTest
{
    public class DataTemplateModel : PropertyChangedBase
    {

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private int _Number;

        public int Number
        {
            get { return _Number; }
            set
            {
                _Number = value;
                OnPropertyChanged();
            }

        }


        private RelayCommand _changeViewCommand;
        public ICommand ChangeViewCommand
        {
            get { return _changeViewCommand ?? (_changeViewCommand = new RelayCommand(this.ChangeView)); }
        }

        private void ChangeView(object obj)
        {



        }
    }
}
