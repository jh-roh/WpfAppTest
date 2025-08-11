using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfAppTest.Model;

namespace WpfAppTest.ViewModel
{
    public class ReceivingItem : PropertyChangedBase
    {
        private string _warehouse;
        public string Warehouse
        {
            get => _warehouse;
            set
            {
                _warehouse = value;
                OnPropertyChanged();
            }
        }

        private string _drugName;
        public string DrugName
        {
            get => _drugName;
            set
            {
                _drugName = value;
                OnPropertyChanged();
            }
        }

        private string _serialNumber;
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BoxInfo));
            }
        }

        private int _currentTotalQuantity;
        public int CurrentTotalQuantity
        {
            get => _currentTotalQuantity;
            set
            {
                _currentTotalQuantity = value;
                OnPropertyChanged();
            }
        }

        public string BoxInfo
        {
            get
            {
                if (Quantity >= 10)
                {
                    int boxes = Quantity / 10;
                    int loose = Quantity % 10;
                    return $"{boxes}BOX {loose}EA";
                }
                return $"0BOX {Quantity}EA";
            }
        }
    }

    public class ItemLocation : PropertyChangedBase
    {
        private string _locationCode;
        public string LocationCode
        {
            get => _locationCode;
            set
            {
                _locationCode = value;
                OnPropertyChanged();
            }
        }

        private string _serialNumber;
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        private string _drugCode;
        public string DrugCode
        {
            get => _drugCode;
            set
            {
                _drugCode = value;
                OnPropertyChanged();
            }
        }

        private string _drugName;
        public string DrugName
        {
            get => _drugName;
            set
            {
                _drugName = value;
                OnPropertyChanged();
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private bool _isNew;
        public bool IsNew
        {
            get => _isNew;
            set
            {
                _isNew = value;
                OnPropertyChanged();
            }
        }
    }

    public class DrugCategory : PropertyChangedBase
    {
        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }
    }

    public class ReceivingScreenViewModel : PropertyChangedBase
    {
        private const int ItemsPerPage = 8; // 페이지당 표시할 약품 분류 수

        public ReceivingScreenViewModel()
        {
            InitializeData();
            InitializeCommands();
        }

        private void InitializeData()
        {
            SearchText = "";
            BarcodeText = "";
            SelectedFilter = "A"; // 기본값으로 A 마약 선택
            CurrentPage = 1;
            ReceivingList = new ObservableCollection<ReceivingItem>();
            ItemLocations = new ObservableCollection<ItemLocation>();
            AllDrugCategories = new List<DrugCategory>();

            // 샘플 데이터 추가
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            // 모든 약품 분류 데이터 로드
            LoadAllDrugCategories();

            // 필터에 따른 데이터 로드
            UpdateLocationData();

            // 샘플 입고 데이터
            ReceivingList.Add(new ReceivingItem
            {
                Warehouse = "D 마약",
                DrugName = "D 마약",
                SerialNumber = "일련번호 A",
                Quantity = 10,
                CurrentTotalQuantity = 980
            });

            ReceivingList.Add(new ReceivingItem
            {
                Warehouse = "D 마약",
                DrugName = "D 마약",
                SerialNumber = "일련번호 B",
                Quantity = 10,
                CurrentTotalQuantity = 100
            });
        }

        private void LoadAllDrugCategories()
        {
            // 마약 분류들
            AllDrugCategories.Add(new DrugCategory { Code = "A", Name = "A 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "B", Name = "B 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "C", Name = "C 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "D", Name = "D 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "E", Name = "E 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "F", Name = "F 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "G", Name = "G 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "H", Name = "H 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "I", Name = "I 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "J", Name = "J 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "K", Name = "K 마약", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "L", Name = "L 마약", Type = "마약" });

            // 향정 분류들
            AllDrugCategories.Add(new DrugCategory { Code = "A_PSYCH", Name = "A 향정", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "B_PSYCH", Name = "B 향정", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "C_PSYCH", Name = "C 향정", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "D_PSYCH", Name = "D 향정", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "E_PSYCH", Name = "E 향정", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "F_PSYCH", Name = "F 향정", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "G_PSYCH", Name = "G 향정", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "H_PSYCH", Name = "H 향정", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "I_PSYCH", Name = "I 향정", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "J_PSYCH", Name = "J 향정", Type = "향정" });

            UpdatePagedDrugCategories();
        }

        private void UpdatePagedDrugCategories()
        {
            if (AllDrugCategories == null)
                return;

            var startIndex = (CurrentPage - 1) * ItemsPerPage;
            var pagedItems = AllDrugCategories.Skip(startIndex).Take(ItemsPerPage).ToList();

            if (PagedDrugCategories == null)
                PagedDrugCategories = new ObservableCollection<DrugCategory>();

            PagedDrugCategories.Clear();
            foreach (var item in pagedItems)
            {
                PagedDrugCategories.Add(item);
            }

            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(PageInfo));
        }

        private void UpdateLocationData()
        {
            if(ItemLocations == null)
                ItemLocations = new ObservableCollection<ItemLocation>();
            ItemLocations.Clear();

            switch (SelectedFilter)
            {
                case "A":
                    // A 마약 데이터
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "1-3-3-2",
                        SerialNumber = "A-001",
                        DrugCode = "DRUG-A-001",
                        DrugName = "A 마약 제품1",
                        Quantity = 5,
                        Status = "정상"
                    });
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "1-4-4",
                        SerialNumber = "A-002",
                        DrugCode = "DRUG-A-002",
                        DrugName = "A 마약 제품2",
                        Quantity = 15,
                        Status = "신규",
                        IsNew = true
                    });
                    break;

                case "B":
                    // B 마약 데이터
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "2-1-1",
                        SerialNumber = "B-001",
                        DrugCode = "DRUG-B-001",
                        DrugName = "B 마약 제품1",
                        Quantity = 8,
                        Status = "정상"
                    });
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "2-2-2",
                        SerialNumber = "B-002",
                        DrugCode = "DRUG-B-002",
                        DrugName = "B 마약 제품2",
                        Quantity = 12,
                        Status = "유효기간경과"
                    });
                    break;

                case "C":
                    // C 마약 데이터
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "3-1-1",
                        SerialNumber = "C-001",
                        DrugCode = "DRUG-C-001",
                        DrugName = "C 마약 제품1",
                        Quantity = 3,
                        Status = "정상"
                    });
                    break;

                case "D":
                    // D 마약 데이터
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "4-1-1",
                        SerialNumber = "D-001",
                        DrugCode = "DRUG-D-001",
                        DrugName = "D 마약 제품1",
                        Quantity = 20,
                        Status = "정상"
                    });
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "4-2-2",
                        SerialNumber = "D-002",
                        DrugCode = "DRUG-D-002",
                        DrugName = "D 마약 제품2",
                        Quantity = 7,
                        Status = "일련번호중복"
                    });
                    break;

                case "A_PSYCH":
                    // A 향정 데이터
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "5-1-1",
                        SerialNumber = "PSYCH-A-001",
                        DrugCode = "PSYCH-A-001",
                        DrugName = "A 향정 제품1",
                        Quantity = 10,
                        Status = "정상"
                    });
                    break;

                case "B_PSYCH":
                    // B 향정 데이터
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "6-1-1",
                        SerialNumber = "PSYCH-B-001",
                        DrugCode = "PSYCH-B-001",
                        DrugName = "B 향정 제품1",
                        Quantity = 6,
                        Status = "정상"
                    });
                    break;

                case "C_PSYCH":
                    // C 향정 데이터
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "7-1-1",
                        SerialNumber = "PSYCH-C-001",
                        DrugCode = "PSYCH-C-001",
                        DrugName = "C 향정 제품1",
                        Quantity = 9,
                        Status = "신규",
                        IsNew = true
                    });
                    break;

                case "D_PSYCH":
                    // D 향정 데이터
                    ItemLocations.Add(new ItemLocation
                    {
                        LocationCode = "8-1-1",
                        SerialNumber = "PSYCH-D-001",
                        DrugCode = "PSYCH-D-001",
                        DrugName = "D 향정 제품1",
                        Quantity = 4,
                        Status = "정상"
                    });
                    break;
            }
        }

        private void InitializeCommands()
        {
            FilterCommand = new RelayCommand(ExecuteFilter);
            AddItemCommand = new RelayCommand(ExecuteAddItem);
            RemoveReceivingItemCommand = new RelayCommand(ExecuteRemoveReceivingItem);
            AddQuantityCommand = new RelayCommand(ExecuteAddQuantity);
            SaveCommand = new RelayCommand(ExecuteSave);
            PreviousPageCommand = new RelayCommand(ExecutePreviousPage, CanExecutePreviousPage);
            NextPageCommand = new RelayCommand(ExecuteNextPage, CanExecuteNextPage);
        }

        #region Properties

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // 검색 로직 구현
            }
        }

        private string _barcodeText;
        public string BarcodeText
        {
            get => _barcodeText;
            set
            {
                _barcodeText = value;
                OnPropertyChanged();
                // 바코드 스캔 로직 구현
            }
        }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
                UpdateLocationData(); // 필터 변경 시 데이터 업데이트
            }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                UpdatePagedDrugCategories();
            }
        }

        public int TotalPages
        {
            get => (int)Math.Ceiling((double)AllDrugCategories.Count / ItemsPerPage);
        }

        public string PageInfo
        {
            get => $"{CurrentPage} / {TotalPages}";
        }

        private List<DrugCategory> _allDrugCategories;
        public List<DrugCategory> AllDrugCategories
        {
            get => _allDrugCategories;
            set
            {
                _allDrugCategories = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DrugCategory> _pagedDrugCategories;
        public ObservableCollection<DrugCategory> PagedDrugCategories
        {
            get => _pagedDrugCategories;
            set
            {
                _pagedDrugCategories = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ReceivingItem> _receivingList;
        public ObservableCollection<ReceivingItem> ReceivingList
        {
            get => _receivingList;
            set
            {
                _receivingList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ItemLocation> _itemLocations;
        public ObservableCollection<ItemLocation> ItemLocations
        {
            get => _itemLocations;
            set
            {
                _itemLocations = value;
                OnPropertyChanged();
            }
        }

        public int TotalReceivingQuantity
        {
            get => ReceivingList?.Sum(item => item.Quantity) ?? 0;
        }

        public string BoxSummary
        {
            get
            {
                int totalQuantity = TotalReceivingQuantity;
                int boxes = totalQuantity / 10;
                int loose = totalQuantity % 10;
                return $"{boxes}BOX, 낱개{loose}EA";
            }
        }

        #endregion

        #region Commands

        public ICommand FilterCommand { get; private set; }
        public ICommand AddItemCommand { get; private set; }
        public ICommand RemoveReceivingItemCommand { get; private set; }
        public ICommand AddQuantityCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }

        #endregion

        #region Command Executions

        private void ExecuteFilter(object parameter)
        {
            string filterType = parameter as string;
            if (string.IsNullOrEmpty(filterType)) return;

            SelectedFilter = filterType;
        }

        private void ExecuteAddItem(object parameter)
        {
            string locationCode = parameter as string;
            if (string.IsNullOrEmpty(locationCode)) return;

            // 항목 추가 로직 구현
            MessageBox.Show($"{locationCode} 위치에 항목이 추가되었습니다.");
        }

        private void ExecuteRemoveReceivingItem(object parameter)
        {
            if (parameter is ReceivingItem item)
            {
                ReceivingList.Remove(item);
                OnPropertyChanged(nameof(TotalReceivingQuantity));
                OnPropertyChanged(nameof(BoxSummary));
                MessageBox.Show($"{item.DrugName} - {item.SerialNumber} 항목이 삭제되었습니다.");
            }
        }

        private void ExecuteAddQuantity(object parameter)
        {
            if (parameter is ReceivingItem item)
            {
                item.Quantity += 1;
                OnPropertyChanged(nameof(TotalReceivingQuantity));
                OnPropertyChanged(nameof(BoxSummary));
            }
        }

        private void ExecuteSave(object parameter)
        {
            // 저장 로직 구현
            MessageBox.Show($"총 {TotalReceivingQuantity}EA의 입고 데이터가 저장되었습니다.", "저장 완료", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecutePreviousPage(object parameter)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        private bool CanExecutePreviousPage(object parameter)
        {
            return CurrentPage > 1;
        }

        private void ExecuteNextPage(object parameter)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
        }

        private bool CanExecuteNextPage(object parameter)
        {
            return CurrentPage < TotalPages;
        }

        #endregion
    }
}