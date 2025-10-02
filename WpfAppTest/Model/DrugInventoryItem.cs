using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAppTest.Model
{
    /// <summary>
    /// 약품 재고 항목 데이터 모델
    /// </summary>
    public class DrugInventoryItem : INotifyPropertyChanged
    {
        private string _location;
        private string _drugCode;
        private string _drugName;
        private int _maxQuantity;
        private int _maxQuantityBox;
        private int _alarmQuantity;
        private int _alarmQuantityBox;
        private int _currentQuantity;
        private string _currentQuantityDetail;
        private bool _isSelected;
        private bool _isLowStock;
        private bool _isCriticalStock;

        /// <summary>
        /// 위치 (예: 장비1-3-5-2)
        /// </summary>
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        /// <summary>
        /// 약품코드 (예: DM-PERLI)
        /// </summary>
        public string DrugCode
        {
            get => _drugCode;
            set => SetProperty(ref _drugCode, value);
        }

        /// <summary>
        /// 약품이름 (예: 페링가니트 주사)
        /// </summary>
        public string DrugName
        {
            get => _drugName;
            set => SetProperty(ref _drugName, value);
        }

        /// <summary>
        /// 최대수량 (EA 단위)
        /// </summary>
        public int MaxQuantity
        {
            get => _maxQuantity;
            set => SetProperty(ref _maxQuantity, value);
        }

        /// <summary>
        /// 최대수량 (Box 단위)
        /// </summary>
        public int MaxQuantityBox
        {
            get => _maxQuantityBox;
            set => SetProperty(ref _maxQuantityBox, value);
        }

        /// <summary>
        /// 알람수량 (EA 단위)
        /// </summary>
        public int AlarmQuantity
        {
            get => _alarmQuantity;
            set => SetProperty(ref _alarmQuantity, value);
        }

        /// <summary>
        /// 알람수량 (Box 단위)
        /// </summary>
        public int AlarmQuantityBox
        {
            get => _alarmQuantityBox;
            set => SetProperty(ref _alarmQuantityBox, value);
        }

        /// <summary>
        /// 현재 재고수량 (EA 단위)
        /// </summary>
        public int CurrentQuantity
        {
            get => _currentQuantity;
            set
            {
                if (SetProperty(ref _currentQuantity, value))
                {
                    UpdateStockStatus();
                }
            }
        }

        /// <summary>
        /// 현재 재고수량 상세 (예: (2Box, 5EA))
        /// </summary>
        public string CurrentQuantityDetail
        {
            get => _currentQuantityDetail;
            set => SetProperty(ref _currentQuantityDetail, value);
        }

        /// <summary>
        /// 선택 여부
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        /// <summary>
        /// 재고 부족 상태 (현재수량 < 최대수량)
        /// </summary>
        public bool IsLowStock
        {
            get => _isLowStock;
            set => SetProperty(ref _isLowStock, value);
        }

        /// <summary>
        /// 재고 위험 상태 (현재수량 < 알람수량)
        /// </summary>
        public bool IsCriticalStock
        {
            get => _isCriticalStock;
            set => SetProperty(ref _isCriticalStock, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 속성 변경 알림을 위한 SetProperty 메서드
        /// </summary>
        /// <typeparam name="T">속성 타입</typeparam>
        /// <param name="backingStore">백킹 필드</param>
        /// <param name="value">새로운 값</param>
        /// <param name="propertyName">속성 이름 (자동으로 설정됨)</param>
        /// <returns>값이 변경되었는지 여부</returns>
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 재고 상태 업데이트
        /// </summary>
        private void UpdateStockStatus()
        {
            IsLowStock = CurrentQuantity < MaxQuantity;
            IsCriticalStock = CurrentQuantity < AlarmQuantity;
        }

        /// <summary>
        /// 속성 변경 이벤트 발생
        /// </summary>
        /// <param name="propertyName">속성 이름 (자동으로 설정됨)</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
