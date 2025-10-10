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
    /// 약품코드별 그룹화된 입고 항목 화면 ViewModel
    /// </summary>
    public class ReceivingItemsGroupedViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DrugInventoryGroupedItem> _groupedItems;
        private ObservableCollection<DrugInventoryGroupedItem> _pagedGroupedItems;
        private int _currentPage = 1;
        private int _itemsPerPage = 10;
        private int _totalItems;
        private string _pageInfo;
        private string _summaryInfo;

        public ReceivingItemsGroupedViewModel()
        {
            InitializeCommands();
            InitializeData();
        }

        #region Properties

        /// <summary>
        /// 전체 그룹화된 입고 항목 목록
        /// </summary>
        public ObservableCollection<DrugInventoryGroupedItem> GroupedItems
        {
            get => _groupedItems;
            set
            {
                _groupedItems = value;
                OnPropertyChanged();
                TotalItems = _groupedItems?.Count ?? 0;
                UpdatePagedItems();
            }
        }

        /// <summary>
        /// 페이지별 그룹화된 입고 항목 목록
        /// </summary>
        public ObservableCollection<DrugInventoryGroupedItem> PagedGroupedItems
        {
            get => _pagedGroupedItems;
            set
            {
                _pagedGroupedItems = value;
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
            get => GroupedItems?.Count ?? 0;
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
            ReceiveItemCommand = new RelayCommand<DrugInventoryGroupedItem>(ReceiveItem);
            ReceiveAllCommand = new RelayCommand(ReceiveAll);
        }

        private void InitializeData()
        {
            // 기존 ReceivingItemsViewModel의 샘플 데이터를 그룹화
            var sampleItems = new ObservableCollection<DrugInventoryItem>
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
                    CurrentQuantity = 5,
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
                    CurrentQuantity = 8,
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
                    CurrentQuantity = 15,
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
                    CurrentQuantity = 2,
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
                    CurrentQuantity = 45,
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
                    CurrentQuantity = 3,
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
                    CurrentQuantity = 12,
                    CurrentQuantityDetail = "(1Box, 2EA)"
                },
                // 같은 약품코드의 다른 위치 추가 (그룹화 테스트용)
                new DrugInventoryItem
                {
                    Location = "장비1-4-2-4",
                    DrugCode = "DM-PERLI",
                    DrugName = "페링가니트 주사",
                    MaxQuantity = 80,
                    MaxQuantityBox = 8,
                    AlarmQuantity = 16,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 35,
                    CurrentQuantityDetail = "(3Box, 5EA)"
                },
                new DrugInventoryItem
                {
                    Location = "장비1-4-2-5",
                    DrugCode = "DM-MORPH",
                    DrugName = "모르핀 주사",
                    MaxQuantity = 60,
                    MaxQuantityBox = 6,
                    AlarmQuantity = 12,
                    AlarmQuantityBox = 2,
                    CurrentQuantity = 25,
                    CurrentQuantityDetail = "(2Box, 5EA)"
                }
            };

            // 약품코드별로 그룹화
            GroupedItems = CreateGroupedItems(sampleItems);

            UpdatePageInfo();
            UpdatePagedItems();
            UpdateSummaryInfo();
        }

        /// <summary>
        /// 약품코드별로 그룹화된 데이터 생성
        /// </summary>
        /// <param name="items">원본 데이터</param>
        /// <returns>그룹화된 데이터</returns>
        private ObservableCollection<DrugInventoryGroupedItem> CreateGroupedItems(ObservableCollection<DrugInventoryItem> items)
        {
            var groupedData = items
                .GroupBy(x => x.DrugCode)
                .Select(g => new DrugInventoryGroupedItem
                {
                    DrugCode = g.Key,
                    DrugName = g.First().DrugName,
                    TotalMaxQuantity = g.Sum(x => x.MaxQuantity),
                    TotalMaxQuantityBox = g.Sum(x => x.MaxQuantityBox),
                    TotalAlarmQuantity = g.Sum(x => x.AlarmQuantity),
                    TotalAlarmQuantityBox = g.Sum(x => x.AlarmQuantityBox),
                    TotalCurrentQuantity = g.Sum(x => x.CurrentQuantity),
                    LocationCount = g.Count(),
                    TotalCurrentQuantityDetail = CalculateTotalQuantityDetail(g.Sum(x => x.CurrentQuantity))
                })
                .OrderBy(x => x.DrugCode)
                .ToList();

            return new ObservableCollection<DrugInventoryGroupedItem>(groupedData);
        }

        /// <summary>
        /// 총 수량 상세 정보 계산
        /// </summary>
        /// <param name="totalQuantity">총 수량</param>
        /// <returns>상세 정보 문자열</returns>
        private string CalculateTotalQuantityDetail(int totalQuantity)
        {
            int boxes = totalQuantity / 10; // 1Box = 10EA 가정
            int remaining = totalQuantity % 10;
            return $"({boxes}Box, {remaining}EA)";
        }

        private void UpdatePagedItems()
        {
            if (GroupedItems == null) return;

            var startIndex = (CurrentPage - 1) * ItemsPerPage;
            var endIndex = Math.Min(startIndex + ItemsPerPage, GroupedItems.Count);

            var pagedItems = GroupedItems.Skip(startIndex).Take(ItemsPerPage).ToList();
            PagedGroupedItems = new ObservableCollection<DrugInventoryGroupedItem>(pagedItems);
        }

        private void UpdatePageInfo()
        {
            var totalPages = (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
            PageInfo = $"{CurrentPage} / {totalPages} 페이지";
        }

        private void UpdateSummaryInfo()
        {
            if (GroupedItems == null || GroupedItems.Count == 0)
            {
                SummaryInfo = "입고할 항목이 없습니다.";
                return;
            }

            var totalMaxQuantity = GroupedItems.Sum(x => x.TotalMaxQuantity);
            var totalCurrentQuantity = GroupedItems.Sum(x => x.TotalCurrentQuantity);
            var totalAlarmQuantity = GroupedItems.Sum(x => x.TotalAlarmQuantity);
            var totalLocations = GroupedItems.Sum(x => x.LocationCount);

            SummaryInfo = $"총 약품: {GroupedItems.Count}종, 위치: {totalLocations}개 | 최대수량: {totalMaxQuantity}EA, 현재수량: {totalCurrentQuantity}EA, 알람수량: {totalAlarmQuantity}EA";
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

        private void ReceiveItem(DrugInventoryGroupedItem item)
        {
            if (item == null) return;

            // 상세 입고 화면으로 이동하는 이벤트 발생
            ReceiveItemRequested?.Invoke(item.DrugCode, item.DrugName);
        }

        public event Action<string, string> ReceiveItemRequested;

        private void ReceiveAll(object parameter)
        {
            if (GroupedItems == null || GroupedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("입고할 항목이 없습니다.", "알림",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var totalLocations = GroupedItems.Sum(x => x.LocationCount);
            var result = System.Windows.MessageBox.Show($"총 {GroupedItems.Count}종 약품, {totalLocations}개 위치의 모든 항목을 입고하시겠습니까?", "전체 입고 확인",
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
