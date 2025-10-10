using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfAppTest.Model;

namespace WpfAppTest.ViewModel
{
    /// <summary>
    /// 약품 위치별 입고 상세 화면 ViewModel
    /// </summary>
    public class DrugReceivingDetailViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DrugReceivingDetailItem> _receivingDetailItems;
        private ObservableCollection<DrugReceivingDetailItem> _pagedReceivingDetailItems;
        private ObservableCollection<DrugInventoryItem> _availableLocations;
        private ObservableCollection<DrugInventoryItem> _pagedAvailableLocations;
        private int _currentPage = 1;
        private int _itemsPerPage = 10;
        private int _totalItems;
        private string _pageInfo;
        private string _totalReceivingQuantity;
        private string _selectedDrugCode;
        private string _selectedDrugName;
        private string _searchText;
        private int _locationCurrentPage = 1;
        private int _locationItemsPerPage = 10;
        private string _locationPageInfo;

        public DrugReceivingDetailViewModel()
        {
            InitializeCommands();
            InitializeData();
        }

        public DrugReceivingDetailViewModel(string drugCode, string drugName) : this()
        {
            SelectedDrugCode = drugCode;
            SelectedDrugName = drugName;
            InitializeDataForDrug(drugCode);
        }

        #region Properties

        /// <summary>
        /// 전체 입고 상세 항목 목록
        /// </summary>
        public ObservableCollection<DrugReceivingDetailItem> ReceivingDetailItems
        {
            get => _receivingDetailItems;
            set
            {
                _receivingDetailItems = value;
                OnPropertyChanged();
                TotalItems = _receivingDetailItems?.Count ?? 0;
                UpdatePagedItems();
                UpdateTotalReceivingQuantity();
            }
        }

        /// <summary>
        /// 페이지별 입고 상세 항목 목록
        /// </summary>
        public ObservableCollection<DrugReceivingDetailItem> PagedReceivingDetailItems
        {
            get => _pagedReceivingDetailItems;
            set
            {
                _pagedReceivingDetailItems = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 사용 가능한 위치 목록
        /// </summary>
        public ObservableCollection<DrugInventoryItem> AvailableLocations
        {
            get => _availableLocations;
            set
            {
                _availableLocations = value;
                OnPropertyChanged();
                UpdatePagedLocations();
            }
        }

        /// <summary>
        /// 페이지별 사용 가능한 위치 목록
        /// </summary>
        public ObservableCollection<DrugInventoryItem> PagedAvailableLocations
        {
            get => _pagedAvailableLocations;
            set
            {
                _pagedAvailableLocations = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 현재 페이지
        /// </summary>
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                UpdatePageInfo();
                UpdatePagedItems();
            }
        }

        /// <summary>
        /// 페이지당 항목 수
        /// </summary>
        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set
            {
                _itemsPerPage = value;
                OnPropertyChanged();
                UpdatePagedItems();
            }
        }

        /// <summary>
        /// 전체 항목 수
        /// </summary>
        public int TotalItems
        {
            get => _totalItems;
            set
            {
                _totalItems = value;
                OnPropertyChanged();
                UpdatePageInfo();
            }
        }

        /// <summary>
        /// 페이지 정보 (예: "1 / 3 페이지")
        /// </summary>
        public string PageInfo
        {
            get => _pageInfo;
            set
            {
                _pageInfo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 총 입고 수량 정보
        /// </summary>
        public string TotalReceivingQuantity
        {
            get => _totalReceivingQuantity;
            set
            {
                _totalReceivingQuantity = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 선택된 약품코드
        /// </summary>
        public string SelectedDrugCode
        {
            get => _selectedDrugCode;
            set
            {
                _selectedDrugCode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 선택된 약품이름
        /// </summary>
        public string SelectedDrugName
        {
            get => _selectedDrugName;
            set
            {
                _selectedDrugName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 검색 텍스트
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterLocations();
            }
        }

        /// <summary>
        /// 위치 페이지 정보
        /// </summary>
        public string LocationPageInfo
        {
            get => _locationPageInfo;
            set
            {
                _locationPageInfo = value;
                OnPropertyChanged();
            }
        }

        public int LocationItemsPerPage
        {
            get => _locationItemsPerPage;
            set
            {
                _locationItemsPerPage = value;
                OnPropertyChanged();
            }
        }

        public int LocationCurrentPage
        {
            get => _locationCurrentPage;
            set
            {
                _locationCurrentPage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand PreviousPageCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LocationPreviousPageCommand { get; private set; }
        public ICommand LocationNextPageCommand { get; private set; }
        public ICommand SelectLocationCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        #endregion

        #region Methods

        private void InitializeCommands()
        {
            PreviousPageCommand = new RelayCommand(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand(NextPage, CanNextPage);
            DeleteItemCommand = new RelayCommand<DrugReceivingDetailItem>(DeleteItem);
            SaveCommand = new RelayCommand(Save);
            LocationPreviousPageCommand = new RelayCommand(LocationPreviousPage, CanLocationPreviousPage);
            LocationNextPageCommand = new RelayCommand(LocationNextPage, CanLocationNextPage);
            SelectLocationCommand = new RelayCommand<DrugInventoryItem>(SelectLocation);
            BackCommand = new RelayCommand(Back);
        }

        private void InitializeData()
        {
            ReceivingDetailItems = new ObservableCollection<DrugReceivingDetailItem>();
            AvailableLocations = new ObservableCollection<DrugInventoryItem>();
            UpdatePageInfo();
            UpdateLocationPageInfo();
        }

        public void InitializeDataForDrug(string drugCode)
        {
            // 선택된 약품의 위치별 데이터 생성
            var sampleLocations = new ObservableCollection<DrugInventoryItem>
            {
                new DrugInventoryItem
                {
                    Location = "장비1-3-5-2",
                    DrugCode = drugCode,
                    DrugName = SelectedDrugName,
                    MaxQuantity = 100,
                    MaxQuantityBox = 10,
                    AlarmQuantity = 20,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 25,
                    CurrentQuantityDetail = "(2Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-4-2-4",
                    DrugCode = drugCode,
                    DrugName = SelectedDrugName,
                    MaxQuantity = 80,
                    MaxQuantityBox = 8,
                    AlarmQuantity = 16,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 35,
                    CurrentQuantityDetail = "(3Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-5-1-1",
                    DrugCode = drugCode,
                    DrugName = SelectedDrugName,
                    MaxQuantity = 60,
                    MaxQuantityBox = 6,
                    AlarmQuantity = 12,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 15,
                    CurrentQuantityDetail = "(1Box, 5EA)"
                }
            };

            AvailableLocations = sampleLocations;
            UpdateLocationPageInfo();
            UpdatePagedLocations();
        }

        private void UpdatePagedItems()
        {
            if (ReceivingDetailItems == null) return;

            var startIndex = (CurrentPage - 1) * ItemsPerPage;
            var endIndex = Math.Min(startIndex + ItemsPerPage, ReceivingDetailItems.Count);

            var pagedItems = ReceivingDetailItems.Skip(startIndex).Take(ItemsPerPage).ToList();
            PagedReceivingDetailItems = new ObservableCollection<DrugReceivingDetailItem>(pagedItems);
        }

        private void UpdatePagedLocations()
        {
            if (AvailableLocations == null) return;

            var startIndex = (LocationCurrentPage - 1) * LocationItemsPerPage;
            var endIndex = Math.Min(startIndex + LocationItemsPerPage, AvailableLocations.Count);

            var pagedItems = AvailableLocations.Skip(startIndex).Take(LocationItemsPerPage).ToList();
            PagedAvailableLocations = new ObservableCollection<DrugInventoryItem>(pagedItems);
        }

        private void UpdatePageInfo()
        {
            var totalPages = (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
            PageInfo = $"{CurrentPage} / {totalPages} 페이지";
        }

        private void UpdateLocationPageInfo()
        {
            if (AvailableLocations == null) return;
            var totalPages = (int)Math.Ceiling((double)AvailableLocations.Count / LocationItemsPerPage);
            LocationPageInfo = $"{LocationCurrentPage} / {totalPages} 페이지";
        }

        private void UpdateTotalReceivingQuantity()
        {
            if (ReceivingDetailItems == null || ReceivingDetailItems.Count == 0)
            {
                TotalReceivingQuantity = "총 입고 수량: 0 EA";
                return;
            }

            var totalQuantity = ReceivingDetailItems.Sum(x => x.ReceivingQuantity);
            var boxes = totalQuantity / 10; // 1Box = 10EA 가정
            var remaining = totalQuantity % 10;

            TotalReceivingQuantity = $"총 입고 수량: {totalQuantity} EA ({boxes}BOX, 낱개{remaining}EA)";
        }

        private void FilterLocations()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                UpdatePagedLocations();
                return;
            }

            var filteredLocations = AvailableLocations
                .Where(x => x.DrugName.Contains(SearchText) || x.DrugCode.Contains(SearchText))
                .ToList();

            var startIndex = (LocationCurrentPage - 1) * LocationItemsPerPage;
            var endIndex = Math.Min(startIndex + LocationItemsPerPage, filteredLocations.Count);

            var pagedItems = filteredLocations.Skip(startIndex).Take(LocationItemsPerPage).ToList();
            PagedAvailableLocations = new ObservableCollection<DrugInventoryItem>(pagedItems);
        }

        private bool CanPreviousPage(object parameter)
        {
            return CurrentPage > 1;
        }

        private void PreviousPage(object parameter)
        {
            if (CanPreviousPage(parameter))
            {
                CurrentPage--;
            }
        }

        private bool CanNextPage(object parameter)
        {
            var totalPages = (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
            return CurrentPage < totalPages;
        }

        private void NextPage(object parameter)
        {
            if (CanNextPage(parameter))
            {
                CurrentPage++;
            }
        }

        private bool CanLocationPreviousPage(object parameter)
        {
            return LocationCurrentPage > 1;
        }

        private void LocationPreviousPage(object parameter)
        {
            if (CanLocationPreviousPage(parameter))
            {
                LocationCurrentPage--;
                UpdatePagedLocations();
                UpdateLocationPageInfo();
            }
        }

        private bool CanLocationNextPage(object parameter)
        {
            if (AvailableLocations == null) return false;
            var totalPages = (int)Math.Ceiling((double)AvailableLocations.Count / LocationItemsPerPage);
            return LocationCurrentPage < totalPages;
        }

        private void LocationNextPage(object parameter)
        {
            if (CanLocationNextPage(parameter))
            {
                LocationCurrentPage++;
                UpdatePagedLocations();
                UpdateLocationPageInfo();
            }
        }

        private void DeleteItem(DrugReceivingDetailItem item)
        {
            if (item == null) return;

            ReceivingDetailItems.Remove(item);
            UpdateTotalReceivingQuantity();
        }

        private void SelectLocation(DrugInventoryItem location)
        {
            if (location == null) return;

            // 새로운 입고 항목 생성
            var newItem = new DrugReceivingDetailItem
            {
                Location = location.Location,
                DrugCode = location.DrugCode,
                DrugName = location.DrugName,
                SerialNumber = GenerateSerialNumber(),
                ReceivingQuantity = 1,
                CurrentQuantity = location.CurrentQuantity,
                MaxQuantity = location.MaxQuantity,
                AlarmQuantity = location.AlarmQuantity,
                Barcode = GenerateBarcode(),
                GTIN = GenerateGTIN(),
                ExpirationDate = DateTime.Now.AddYears(2).ToString("yyyy-MM-dd"),
                ManufacturingNumber = GenerateManufacturingNumber()
            };

            ReceivingDetailItems.Add(newItem);
            UpdateTotalReceivingQuantity();
        }

        private string GenerateSerialNumber()
        {
            var drugCode = SelectedDrugCode?.Replace("DM-", "").Replace("-", "") ?? "ITEM";
            var count = ReceivingDetailItems.Count + 1;
            return $"{drugCode}-{count:D3}";
        }

        private string GenerateBarcode()
        {
            return $"123456789{DateTime.Now:MMddHHmmss}";
        }

        private string GenerateGTIN()
        {
            return $"880123456789{DateTime.Now:MMdd}";
        }

        private string GenerateManufacturingNumber()
        {
            return $"MFG{DateTime.Now:yyyyMMddHHmm}";
        }

        private void Save(object parameter)
        {
            if (ReceivingDetailItems == null || ReceivingDetailItems.Count == 0)
            {
                System.Windows.MessageBox.Show("입고할 항목이 없습니다.", "알림",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var totalQuantity = ReceivingDetailItems.Sum(x => x.ReceivingQuantity);
            var result = System.Windows.MessageBox.Show($"총 {totalQuantity}EA의 항목을 입고하시겠습니까?", "입고 확인",
                System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                System.Windows.MessageBox.Show("입고가 완료되었습니다.", "입고 완료",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                // 실제 구현에서는 데이터베이스 업데이트 등의 로직이 들어갑니다.
                ReceivingDetailItems.Clear();
                UpdateTotalReceivingQuantity();
            }
        }

        private void Back(object parameter)
        {
            // 뒤로가기 이벤트 발생
            BackRequested?.Invoke();
        }

        public event Action BackRequested;

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
