using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAppTest.Model
{
    /// <summary>
    /// 약품 위치별 입고 상세 항목 데이터 모델
    /// </summary>
    public class DrugReceivingDetailItem : INotifyPropertyChanged
    {
        private string _location;
        private string _drugCode;
        private string _drugName;
        private string _serialNumber;
        private int _receivingQuantity;
        private int _currentQuantity;
        private int _maxQuantity;
        private int _alarmQuantity;
        private bool _isSelected;
        private string _barcode;
        private string _gtin;
        private string _expirationDate;
        private string _manufacturingNumber;

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
        /// 일련번호 (예: PERL-001)
        /// </summary>
        public string SerialNumber
        {
            get => _serialNumber;
            set => SetProperty(ref _serialNumber, value);
        }

        /// <summary>
        /// 입고 수량
        /// </summary>
        public int ReceivingQuantity
        {
            get => _receivingQuantity;
            set => SetProperty(ref _receivingQuantity, value);
        }

        /// <summary>
        /// 현재 재고수량
        /// </summary>
        public int CurrentQuantity
        {
            get => _currentQuantity;
            set => SetProperty(ref _currentQuantity, value);
        }

        /// <summary>
        /// 최대수량
        /// </summary>
        public int MaxQuantity
        {
            get => _maxQuantity;
            set => SetProperty(ref _maxQuantity, value);
        }

        /// <summary>
        /// 알람수량
        /// </summary>
        public int AlarmQuantity
        {
            get => _alarmQuantity;
            set => SetProperty(ref _alarmQuantity, value);
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
        /// 바코드
        /// </summary>
        public string Barcode
        {
            get => _barcode;
            set => SetProperty(ref _barcode, value);
        }

        /// <summary>
        /// 품목번호(GTIN)
        /// </summary>
        public string GTIN
        {
            get => _gtin;
            set => SetProperty(ref _gtin, value);
        }

        /// <summary>
        /// 유효기간
        /// </summary>
        public string ExpirationDate
        {
            get => _expirationDate;
            set => SetProperty(ref _expirationDate, value);
        }

        /// <summary>
        /// 제조번호
        /// </summary>
        public string ManufacturingNumber
        {
            get => _manufacturingNumber;
            set => SetProperty(ref _manufacturingNumber, value);
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
        /// 속성 변경 이벤트 발생
        /// </summary>
        /// <param name="propertyName">속성 이름 (자동으로 설정됨)</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
