using SocketTester.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTester.Model
{
    public class RobotResultModel : PropertyChangedBase
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

        private String _buttonPressResult;

        public String ButtonPressResult
        {
            get { return _buttonPressResult; }
            set
            {
                _buttonPressResult = value; 
                OnPropertyChanged();
            }

        }
        private String _robotCallRequestResult;
        public String RobotCallRequestResult
        {
            get { return _robotCallRequestResult; }
            set
            {
                _robotCallRequestResult = value;
                OnPropertyChanged();
            }
        }

        private String _robotApproachRequestResult;
        public string RobotApproachRequestResult
        {
            get { return _robotApproachRequestResult; }
            set
            {
                _robotApproachRequestResult = value;
                OnPropertyChanged();
            }
        }


        private String _robotInCompletedResult;
        public String RobotInCompletedResult
        {
            get { return _robotInCompletedResult; }
            set
            {
                _robotInCompletedResult = value;
                OnPropertyChanged();
            }
        }

        private String _robotOutCompletedResult;
        public String RobotOutCompletedResult
        {
            get { return _robotOutCompletedResult; }
            set
            {
                _robotOutCompletedResult = value;
                OnPropertyChanged();
            }
        }
    }
}
