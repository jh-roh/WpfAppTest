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
    /// RelayCommand 구현
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this._execute(parameter);
        }
    }

    /// <summary>
    /// 제네릭 RelayCommand 구현
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            this._execute((T)parameter);
        }
    }

    /// <summary>
    /// 입고 항목 화면 ViewModel
    /// </summary>
    public class ReceivingItemsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DrugInventoryItem> _receivingItems;
        private ObservableCollection<DrugInventoryItem> _pagedReceivingItems;
        private int _currentPage = 1;
        private int _itemsPerPage = 10;
        private int _totalItems;
        private string _pageInfo;
        private string _summaryInfo;

        public ReceivingItemsViewModel()
        {
            InitializeCommands();
            InitializeData();
        }

        #region Properties

        /// <summary>
        /// 전체 입고 항목 목록
        /// </summary>
        public ObservableCollection<DrugInventoryItem> ReceivingItems
        {
            get => _receivingItems;
            set
            {
                _receivingItems = value;
                OnPropertyChanged();
                TotalItems = _receivingItems?.Count ?? 0;
                UpdatePagedItems();
            }
        }

        /// <summary>
        /// 페이지별 입고 항목 목록
        /// </summary>
        public ObservableCollection<DrugInventoryItem> PagedReceivingItems
        {
            get => _pagedReceivingItems;
            set
            {
                _pagedReceivingItems = value;
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
        /// 총 항목 수
        /// </summary>
        public int TotalItemsCount
        {
            get => ReceivingItems?.Count ?? 0;
        }

        /// <summary>
        /// 요약 정보
        /// </summary>
        public string SummaryInfo
        {
            get => _summaryInfo;
            set
            {
                _summaryInfo = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand PreviousPageCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand ReceiveItemCommand { get; private set; }
        public ICommand ReceiveAllCommand { get; private set; }

        #endregion

        #region Methods

        private void InitializeCommands()
        {
            PreviousPageCommand = new RelayCommand(PreviousPage, CanPreviousPage);
            NextPageCommand = new RelayCommand(NextPage, CanNextPage);
            ReceiveItemCommand = new RelayCommand<DrugInventoryItem>(ReceiveItem);
            ReceiveAllCommand = new RelayCommand(ReceiveAll);
        }

        private void InitializeData()
        {
            // 샘플 데이터 초기화
            ReceivingItems = new ObservableCollection<DrugInventoryItem>
            {
                new DrugInventoryItem
                {
                    Location = "장비1-3-5-2",
                    DrugCode = "DM-PERLI",
                    DrugName = "페링가니트 주사",
                    MaxQuantity = 100,
                    MaxQuantityBox = 10,
                    AlarmQuantity = 20,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 25,
                    CurrentQuantityDetail = "(2Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-3-5-3",
                    DrugCode = "DM-MORPH",
                    DrugName = "모르핀 주사",
                    MaxQuantity = 50,
                    MaxQuantityBox = 5,
                    AlarmQuantity = 10,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 15,
                    CurrentQuantityDetail = "(1Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-3-5-4",
                    DrugCode = "DM-FENT",
                    DrugName = "펜타닐 주사",
                    MaxQuantity = 80,
                    MaxQuantityBox = 8,
                    AlarmQuantity = 16,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 30,
                    CurrentQuantityDetail = "(3Box, 0EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-3-6-1",
                    DrugCode = "DM-MIDAZ",
                    DrugName = "미다졸람 주사",
                    MaxQuantity = 60,
                    MaxQuantityBox = 6,
                    AlarmQuantity = 12,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 5, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-3-6-2",
                    DrugCode = "DM-PROPO",
                    DrugName = "프로포폴 주사",
                    MaxQuantity = 120,
                    MaxQuantityBox = 12,
                    AlarmQuantity = 24,
                    AlarmQuantityBox = 3,
                    CurrentQuantity = 8, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 8EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-3-6-3",
                    DrugCode = "DM-SUCC",
                    DrugName = "석시닐콜린 주사",
                    MaxQuantity = 40,
                    MaxQuantityBox = 4,
                    AlarmQuantity = 8,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 15, // 부족 상태
                    CurrentQuantityDetail = "(1Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-4-1-1",
                    DrugCode = "DM-ATRO",
                    DrugName = "아트로핀 주사",
                    MaxQuantity = 30,
                    MaxQuantityBox = 3,
                    AlarmQuantity = 6,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 2, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 2EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-4-1-2",
                    DrugCode = "DM-EPIN",
                    DrugName = "에피네프린 주사",
                    MaxQuantity = 50,
                    MaxQuantityBox = 5,
                    AlarmQuantity = 10,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 45, // 정상 상태
                    CurrentQuantityDetail = "(4Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-4-1-3",
                    DrugCode = "DM-NOREP",
                    DrugName = "노르에피네프린 주사",
                    MaxQuantity = 25,
                    MaxQuantityBox = 3,
                    AlarmQuantity = 5,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 3, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 3EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-4-2-1",
                    DrugCode = "DM-DOPA",
                    DrugName = "도파민 주사",
                    MaxQuantity = 80,
                    MaxQuantityBox = 8,
                    AlarmQuantity = 16,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 12, // 위험 상태
                    CurrentQuantityDetail = "(1Box, 2EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-4-2-2",
                    DrugCode = "DM-DOBU",
                    DrugName = "도부타민 주사",
                    MaxQuantity = 60,
                    MaxQuantityBox = 6,
                    AlarmQuantity = 12,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 35, // 부족 상태
                    CurrentQuantityDetail = "(3Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-4-2-3",
                    DrugCode = "DM-ISOP",
                    DrugName = "이소프레날린 주사",
                    MaxQuantity = 40,
                    MaxQuantityBox = 4,
                    AlarmQuantity = 8,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 38, // 정상 상태
                    CurrentQuantityDetail = "(3Box, 8EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-5-1-1",
                    DrugCode = "DM-LIDO",
                    DrugName = "리도카인 주사",
                    MaxQuantity = 100,
                    MaxQuantityBox = 10,
                    AlarmQuantity = 20,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 85, // 정상 상태
                    CurrentQuantityDetail = "(8Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-5-1-2",
                    DrugCode = "DM-PROCA",
                    DrugName = "프로카인 주사",
                    MaxQuantity = 50,
                    MaxQuantityBox = 5,
                    AlarmQuantity = 10,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 7, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 7EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-5-1-3",
                    DrugCode = "DM-BUPI",
                    DrugName = "부피바카인 주사",
                    MaxQuantity = 30,
                    MaxQuantityBox = 3,
                    AlarmQuantity = 6,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 4, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 4EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-5-2-1",
                    DrugCode = "DM-KETO",
                    DrugName = "케타민 주사",
                    MaxQuantity = 40,
                    MaxQuantityBox = 4,
                    AlarmQuantity = 8,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 25, // 부족 상태
                    CurrentQuantityDetail = "(2Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-5-2-2",
                    DrugCode = "DM-THIO",
                    DrugName = "티오펜탈 주사",
                    MaxQuantity = 60,
                    MaxQuantityBox = 6,
                    AlarmQuantity = 12,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 9, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 9EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-5-2-3",
                    DrugCode = "DM-ETOM",
                    DrugName = "에토미데이트 주사",
                    MaxQuantity = 25,
                    MaxQuantityBox = 3,
                    AlarmQuantity = 5,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 22, // 정상 상태
                    CurrentQuantityDetail = "(2Box, 2EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-6-1-1",
                    DrugCode = "DM-VECU",
                    DrugName = "베쿠로늄 주사",
                    MaxQuantity = 20,
                    MaxQuantityBox = 2,
                    AlarmQuantity = 4,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 1, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 1EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-6-1-2",
                    DrugCode = "DM-ROCU",
                    DrugName = "로쿠로늄 주사",
                    MaxQuantity = 30,
                    MaxQuantityBox = 3,
                    AlarmQuantity = 6,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 18, // 부족 상태
                    CurrentQuantityDetail = "(1Box, 8EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-6-1-3",
                    DrugCode = "DM-CISAT",
                    DrugName = "시사트라큐륨 주사",
                    MaxQuantity = 15,
                    MaxQuantityBox = 2,
                    AlarmQuantity = 3,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 2, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 2EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-6-2-1",
                    DrugCode = "DM-NEO",
                    DrugName = "네오스티그민 주사",
                    MaxQuantity = 25,
                    MaxQuantityBox = 3,
                    AlarmQuantity = 5,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 20, // 정상 상태
                    CurrentQuantityDetail = "(2Box, 0EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-6-2-2",
                    DrugCode = "DM-PYRI",
                    DrugName = "피리독신 주사",
                    MaxQuantity = 50,
                    MaxQuantityBox = 5,
                    AlarmQuantity = 10,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 6, // 위험 상태
                    CurrentQuantityDetail = "(0Box, 6EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-6-2-3",
                    DrugCode = "DM-FOLIC",
                    DrugName = "엽산 주사",
                    MaxQuantity = 40,
                    MaxQuantityBox = 4,
                    AlarmQuantity = 8,
                    AlarmQuantityBox = 1,
                    CurrentQuantity = 28, // 부족 상태
                    CurrentQuantityDetail = "(2Box, 8EA)"
                }
            };

            UpdatePageInfo();
            UpdatePagedItems();
            UpdateSummaryInfo();
        }

        private void UpdatePagedItems()
        {
            if (ReceivingItems == null) return;

            var startIndex = (CurrentPage - 1) * ItemsPerPage;
            var endIndex = Math.Min(startIndex + ItemsPerPage, ReceivingItems.Count);

            var pagedItems = ReceivingItems.Skip(startIndex).Take(ItemsPerPage).ToList();
            PagedReceivingItems = new ObservableCollection<DrugInventoryItem>(pagedItems);
        }

        private void UpdatePageInfo()
        {
            var totalPages = (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
            PageInfo = $"{CurrentPage} / {totalPages} 페이지";
        }

        private void UpdateSummaryInfo()
        {
            if (ReceivingItems == null || ReceivingItems.Count == 0)
            {
                SummaryInfo = "입고할 항목이 없습니다.";
                return;
            }

            var totalMaxQuantity = ReceivingItems.Sum(x => x.MaxQuantity);
            var totalCurrentQuantity = ReceivingItems.Sum(x => x.CurrentQuantity);
            var totalAlarmQuantity = ReceivingItems.Sum(x => x.AlarmQuantity);

            SummaryInfo = $"최대수량: {totalMaxQuantity}EA, 현재수량: {totalCurrentQuantity}EA, 알람수량: {totalAlarmQuantity}EA";
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

        private void ReceiveItem(DrugInventoryItem item)
        {
            if (item == null) return;

            // 입고 처리 로직
            System.Windows.MessageBox.Show($"'{item.DrugName}' 항목이 입고되었습니다.", "입고 완료",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            // 실제 구현에서는 데이터베이스 업데이트 등의 로직이 들어갑니다.
        }

        private void ReceiveAll(object parameter)
        {
            if (ReceivingItems == null || ReceivingItems.Count == 0)
            {
                System.Windows.MessageBox.Show("입고할 항목이 없습니다.", "알림",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var result = System.Windows.MessageBox.Show($"총 {ReceivingItems.Count}개 항목을 모두 입고하시겠습니까?", "전체 입고 확인",
                System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                System.Windows.MessageBox.Show("모든 항목이 입고되었습니다.", "입고 완료",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                // 실제 구현에서는 데이터베이스 업데이트 등의 로직이 들어갑니다.
            }
        }

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
