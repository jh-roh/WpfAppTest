using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAppTest.Model
{
    /// <summary>
    /// 약품코드별 그룹화된 재고 항목 데이터 모델
    /// </summary>
    public class DrugInventoryGroupedItem : INotifyPropertyChanged
    {
        private string _drugCode;
        private string _drugName;
        private int _totalMaxQuantity;
        private int _totalMaxQuantityBox;
        private int _totalAlarmQuantity;
        private int _totalAlarmQuantityBox;
        private int _totalCurrentQuantity;
        private string _totalCurrentQuantityDetail;
        private int _locationCount;
        private bool _hasLowStock;
        private bool _hasCriticalStock;
        private string _stockStatus;

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
        /// 총 최대수량 (EA 단위)
        /// </summary>
        public int TotalMaxQuantity
        {
            get => _totalMaxQuantity;
            set => SetProperty(ref _totalMaxQuantity, value);
        }

        /// <summary>
        /// 총 최대수량 (Box 단위)
        /// </summary>
        public int TotalMaxQuantityBox
        {
            get => _totalMaxQuantityBox;
            set => SetProperty(ref _totalMaxQuantityBox, value);
        }

        /// <summary>
        /// 총 알람수량 (EA 단위)
        /// </summary>
        public int TotalAlarmQuantity
        {
            get => _totalAlarmQuantity;
            set => SetProperty(ref _totalAlarmQuantity, value);
        }

        /// <summary>
        /// 총 알람수량 (Box 단위)
        /// </summary>
        public int TotalAlarmQuantityBox
        {
            get => _totalAlarmQuantityBox;
            set => SetProperty(ref _totalAlarmQuantityBox, value);
        }

        /// <summary>
        /// 총 현재 재고수량 (EA 단위)
        /// </summary>
        public int TotalCurrentQuantity
        {
            get => _totalCurrentQuantity;
            set
            {
                if (SetProperty(ref _totalCurrentQuantity, value))
                {
                    UpdateStockStatus();
                }
            }
        }

        /// <summary>
        /// 총 현재 재고수량 상세 (예: (2Box, 5EA))
        /// </summary>
        public string TotalCurrentQuantityDetail
        {
            get => _totalCurrentQuantityDetail;
            set => SetProperty(ref _totalCurrentQuantityDetail, value);
        }

        /// <summary>
        /// 위치 개수
        /// </summary>
        public int LocationCount
        {
            get => _locationCount;
            set => SetProperty(ref _locationCount, value);
        }

        /// <summary>
        /// 재고 부족 상태 (현재수량 < 최대수량)
        /// </summary>
        public bool HasLowStock
        {
            get => _hasLowStock;
            set => SetProperty(ref _hasLowStock, value);
        }

        /// <summary>
        /// 재고 위험 상태 (현재수량 < 알람수량)
        /// </summary>
        public bool HasCriticalStock
        {
            get => _hasCriticalStock;
            set => SetProperty(ref _hasCriticalStock, value);
        }

        /// <summary>
        /// 재고 상태 텍스트
        /// </summary>
        public string StockStatus
        {
            get => _stockStatus;
            set => SetProperty(ref _stockStatus, value);
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
            HasLowStock = TotalCurrentQuantity < TotalMaxQuantity;
            HasCriticalStock = TotalCurrentQuantity < TotalAlarmQuantity;

            if (HasCriticalStock)
            {
                StockStatus = "위험";
            }
            else if (HasLowStock)
            {
                StockStatus = "부족";
            }
            else
            {
                StockStatus = "정상";
            }
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
