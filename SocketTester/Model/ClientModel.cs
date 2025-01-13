using SocketTester.IO.Robot;
using SocketTester.MVVM;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocketTester.Model
{
    public class ClientModel : PropertyChangedBase, IDisposable, IDataErrorInfo
    {
        public struct IAPUIResult
        {
            public IAP_ACTION_RESULT? AciotnResult { get; set; }

            public IAP_MODE? IAPMode { get; set; }
        }


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
        private bool disposedValue;

        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        private int _progress;

        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan _timeElapsed;

        public TimeSpan TimeElapsed
        {
            get => _timeElapsed;
            set
            {
                _timeElapsed = value;
                OnPropertyChanged();
            }
        }
        
        private String _filePath;
        public String FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        private String _fileSize;
        public String FileSize
        {
            get
            {
                return _fileSize;
            }
            set
            {
                _fileSize = value;
                OnPropertyChanged();
            }
        }

        private bool _isIapProgress;

        public bool IsIapProgress
        {
            get
            {
                return _isIapProgress;
            }
            set
            {
                _isIapProgress = value;
            }
        }

        private String _iapStatus;

        public String IAPStatus
        {
            get
            {
                return _iapStatus;
            }
            set
            {
                _iapStatus = value;
                OnPropertyChanged();
            }
        }

        private String _iapDescription;

        public String IAPDescription
        {
            get
            {
                return _iapDescription;
            }
            set
            {
                _iapDescription = value;
                OnPropertyChanged();
            }
        }
        private IAPUIResult _iapUIResult;
        public IAPUIResult iAPUIResult
        {
            get
            {
                return _iapUIResult;
            }
            set
            {
                _iapUIResult = value;
                OnPropertyChanged();
            }
        }


        public readonly Dictionary<byte, ManualResetEvent> CommandCheckEvnet = new Dictionary<byte, ManualResetEvent>();


        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }

        public string Error => null;

        private string _validationError;
        public string ValidationError
        {
            get { return _validationError; }
            set
            {
                _validationError = value;
                OnPropertyChanged();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (columnName == nameof(IpAddress))
                {
                    if (string.IsNullOrWhiteSpace(IpAddress) || !IsValidIpAddress(IpAddress))
                        error = "Invalid IP Address. Example: 192.168.0.1";
                }
                else if (columnName == nameof(Port))
                {
                    if (!IsValidPort(Port))
                        error = "Invalid Port Number. Must be between 0 and 65535.";
                }

                ValidationError = error;
                return error;
            }
        }

        private readonly BlockingCollection<Func<Task>> _taskQueue = new BlockingCollection<Func<Task>>(); // 작업 큐
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _workerTask;

        public ClientModel()
        {
            _workerTask = Task.Run(() => ProcessTasks(_cancellationTokenSource.Token));

            // RobotIOConstant의 모든 상수 값을 Reflection을 통해 가져와서 Dictionary에 추가
            FieldInfo[] fields = typeof(RobotIOConstant).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly)  // const 필드만 선택
                {
                    byte key = Convert.ToByte(field.GetValue(null));  // 필드 값을 키로 변환
                    CommandCheckEvnet.Add(key, new ManualResetEvent(false));  // ManualResetEvent 객체 추가
                }
            }
        }

        private bool IsValidIpAddress(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) return false;
            return System.Net.IPAddress.TryParse(ip, out _);
        }

        private bool IsValidPort(int port)
        {
            return port > 0 && port <= 65535;
        }

        public void Enqueue(Func<Task> taskGenerator)
        {
            if (!_taskQueue.IsAddingCompleted)
            {
                _taskQueue.Add(taskGenerator); // 큐에 작업 추가
            }
        }
        private async Task ProcessTasks(CancellationToken cancellationToken)
        {
            foreach (var taskGenerator in _taskQueue.GetConsumingEnumerable(cancellationToken))
            {
                try
                {
                    await taskGenerator(); // 작업 실행
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing task: {ex.Message}");
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                    _taskQueue.CompleteAdding(); // 더 이상 작업을 추가하지 않음
                    _cancellationTokenSource.Cancel(); // 작업 취소
                    _workerTask.Wait(); // 워커 종료 대기
                    _taskQueue.Dispose();
                    _cancellationTokenSource.Dispose();

                    CommandCheckEvnet.Clear();
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~ClientModel()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
