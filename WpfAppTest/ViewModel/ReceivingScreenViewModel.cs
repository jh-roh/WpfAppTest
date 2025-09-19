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

        private DateTime _expiryDate;
        public DateTime ExpiryDate
        {
            get => _expiryDate;
            set
            {
                _expiryDate = value;
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

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
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
        private const int ItemsPerPage = 15; // 페이지당 표시할 약품 분류 수
        private const int ItemLocationsPerPage = 6; // 페이지당 표시할 항목 위치 수 (2행 x 4열)
        private const int ReceivingGridPerPage = 10; // 페이지당 표시할 그리드 항목 수

        public ReceivingScreenViewModel()
        {
            InitializeData();
            InitializeCommands();
        }

        private void InitializeData()
        {
            SearchText = "";
            BarcodeText = "";
            SelectedFilter = "DM-PERLINGJ"; // 기본값으로 페링가니트 선택
            CurrentPage = 1;
            ItemLocationsCurrentPage = 1;
            ReceivingGridCurrentPage = 1;
            ReceivingList = new ObservableCollection<ReceivingItem>();
            PagedReceivingList = new ObservableCollection<ReceivingItem>();
            ItemLocations = new ObservableCollection<ItemLocation>();
            AllDrugCategories = new List<DrugCategory>();
            AllItemLocations = new List<ItemLocation>();
            PagedItemLocations = new ObservableCollection<ItemLocation>();

            // 샘플 데이터 추가
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            // 모든 약품 분류 데이터 로드
            LoadAllDrugCategories();

            // 필터에 따른 데이터 로드
            UpdateLocationData();

            // 많은 샘플 입고 데이터 추가
            LoadSampleReceivingData();
        }

        private void LoadSampleReceivingData()
        {
            // 페링가니트 관련 데이터
            for (int i = 1; i <= 25; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "페링가니트 창고",
                    DrugName = "페링가니트 0.1% 주사 10ml",
                    SerialNumber = $"PERL-{i:D3}",
                    Quantity = 5 + (i % 10),
                    CurrentTotalQuantity = 100 + (i * 10)
                });
            }

            // 트리람 관련 데이터
            for (int i = 1; i <= 20; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "트리람 창고",
                    DrugName = "트리람 정 0.25mg",
                    SerialNumber = $"TRI-{i:D3}",
                    Quantity = 8 + (i % 8),
                    CurrentTotalQuantity = 80 + (i * 8)
                });
            }

            // 코노펜 관련 데이터
            for (int i = 1; i <= 30; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "코노펜 창고",
                    DrugName = "코노펜캡슐",
                    SerialNumber = $"IAC-{i:D3}",
                    Quantity = 3 + (i % 7),
                    CurrentTotalQuantity = 50 + (i * 5)
                });
            }

            // 아티반 관련 데이터
            for (int i = 1; i <= 18; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "아티반 창고",
                    DrugName = "아티반주4mg",
                    SerialNumber = $"LZP-{i:D3}",
                    Quantity = 20 + (i % 10),
                    CurrentTotalQuantity = 200 + (i * 15)
                });
            }

            // 인산코데인 관련 데이터
            for (int i = 1; i <= 22; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "코데인 창고",
                    DrugName = "인산코데인 정 20mg (하나)",
                    SerialNumber = $"TCO-{i:D3}",
                    Quantity = 10 + (i % 6),
                    CurrentTotalQuantity = 120 + (i * 12)
                });
            }

            // 바스캄 관련 데이터
            for (int i = 1; i <= 15; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "바스캄 창고",
                    DrugName = "바스캄주5mg/5ml",
                    SerialNumber = $"WVSC-{i:D3}",
                    Quantity = 6 + (i % 9),
                    CurrentTotalQuantity = 60 + (i * 6)
                });
            }

            // 노스판 관련 데이터
            for (int i = 1; i <= 28; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "노스판 창고",
                    DrugName = "노스판패취5mcg/h,5mg/P",
                    SerialNumber = $"BUPN-{i:D3}",
                    Quantity = 9 + (i % 5),
                    CurrentTotalQuantity = 90 + (i * 9)
                });
            }

            // 모노틴 관련 데이터
            for (int i = 1; i <= 16; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "모노틴 창고",
                    DrugName = "모노틴 정",
                    SerialNumber = $"MNT-{i:D3}",
                    Quantity = 4 + (i % 12),
                    CurrentTotalQuantity = 40 + (i * 4)
                });
            }

            // 헥사메딘 관련 데이터
            for (int i = 1; i <= 24; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "헥사메딘 창고",
                    DrugName = "헥사메딘액0.12% 100ml",
                    SerialNumber = $"CHX-{i:D3}",
                    Quantity = 5 + (i % 15),
                    CurrentTotalQuantity = 70 + (i * 7)
                });
            }

            // 듀로제식 관련 데이터
            for (int i = 1; i <= 12; i++)
            {
                ReceivingList.Add(new ReceivingItem
                {
                    Warehouse = "듀로제식 창고",
                    DrugName = "듀로제식 디트랜스 패취 12mcg/h 5.25㎠",
                    SerialNumber = $"FENT12-{i:D3}",
                    Quantity = 7 + (i % 8),
                    CurrentTotalQuantity = 85 + (i * 8)
                });
            }

            // 페이징된 그리드 데이터 업데이트
            UpdatePagedReceivingList();
        }

        private void LoadAllDrugCategories()
        {
            //// 마약 분류들
            //AllDrugCategories.Add(new DrugCategory { Code = "DM-PERLINGJ", Name = "페링가니트 0.1% 주사 10ml", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "B", Name = "B 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "C", Name = "C 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "D", Name = "D 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "E", Name = "E 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "F", Name = "F 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "G", Name = "G 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "H", Name = "H 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "I", Name = "I 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "J", Name = "J 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "K", Name = "K 마약", Type = "마약" });
            //AllDrugCategories.Add(new DrugCategory { Code = "L", Name = "L 마약", Type = "마약" });

            //// 향정 분류들
            //AllDrugCategories.Add(new DrugCategory { Code = "A_PSYCH", Name = "A 향정", Type = "향정" });
            //AllDrugCategories.Add(new DrugCategory { Code = "B_PSYCH", Name = "B 향정", Type = "향정" });
            //AllDrugCategories.Add(new DrugCategory { Code = "C_PSYCH", Name = "C 향정", Type = "향정" });
            //AllDrugCategories.Add(new DrugCategory { Code = "D_PSYCH", Name = "D 향정", Type = "향정" });
            //AllDrugCategories.Add(new DrugCategory { Code = "E_PSYCH", Name = "E 향정", Type = "향정" });
            //AllDrugCategories.Add(new DrugCategory { Code = "F_PSYCH", Name = "F 향정", Type = "향정" });
            //AllDrugCategories.Add(new DrugCategory { Code = "G_PSYCH", Name = "G 향정", Type = "향정" });
            //AllDrugCategories.Add(new DrugCategory { Code = "H_PSYCH", Name = "H 향정", Type = "향정" });
            //AllDrugCategories.Add(new DrugCategory { Code = "I_PSYCH", Name = "I 향정", Type = "향정" });
            //AllDrugCategories.Add(new DrugCategory { Code = "J_PSYCH", Name = "J 향정", Type = "향정" });

            AllDrugCategories.Add(new DrugCategory { Code = "DM-PERLINGJ", Name = "페링가니트 0.1% 주사 10ml", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-TRI", Name = "트리람 정 0.25mg", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "DN-IAC", Name = "코노펜캡슐", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DLZP4J", Name = "아티반주4mg", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-TCO", Name = "인산코데인 정 20mg (하나)", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-WVSC", Name = "바스캄주5mg/5ml", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "DBUPN5P", Name = "노스판패취5mcg/h,5mg/P", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-MNT", Name = "모노틴 정", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DG-CHX", Name = "헥사메딘액0.12% 100ml", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-FENT12", Name = "듀로제식 디트랜스 패취 12mcg/h 5.25㎠", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DH-TERF", Name = "포스테오Coter-pen 600mcg", Type = "냉장" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-TGCR5", Name = "타진서방정 5/2.5mg", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-WVSC3", Name = "바스캄주3mg/3ml", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-7MPX", Name = "염몰핀주사 1ml", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-ABM", Name = "에스케이 알부민 주 20% 100ml", Type = "일반" });

            AllDrugCategories.Add(new DrugCategory { Code = "DM-MORP10", Name = "몰핀주사 10mg/1ml", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-DIAZ5", Name = "디아제팜주 5mg/2ml", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-PETH1", Name = "페치딘주 50mg/1ml", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-LORZ", Name = "로라제팜정 1mg", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "DN-IBU200", Name = "이부프로펜정 200mg", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DN-AMOX500", Name = "아목시실린캡슐 500mg", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DH-INSU", Name = "인슐린주 100IU/ml", Type = "냉장" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-OXY5", Name = "옥시코돈정 5mg", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-KETA", Name = "케타민주사 50mg/5ml", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-MIDA", Name = "미다졸람주사 15mg/3ml", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "DN-VITA", Name = "비타민C주 500mg/5ml", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DN-OMEP20", Name = "오메프라졸캡슐 20mg", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DN-PARA500", Name = "아세트아미노펜정 500mg", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DH-HBVV", Name = "B형간염백신 10mcg/0.5ml", Type = "냉장" });
            AllDrugCategories.Add(new DrugCategory { Code = "DH-HPVV", Name = "HPV백신 0.5ml", Type = "냉장" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-FENT50", Name = "펜타닐패취 50mcg/h", Type = "마약" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-ZOLP10", Name = "졸피뎀정 10mg", Type = "향정" });
            AllDrugCategories.Add(new DrugCategory { Code = "DN-ATOR10", Name = "아토르바스타틴정 10mg", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DN-LISIN5", Name = "리시노프릴정 5mg", Type = "일반" });
            AllDrugCategories.Add(new DrugCategory { Code = "DM-HYDRO", Name = "하이드로몰폰주사 2mg/ml", Type = "마약" });


            UpdatePagedDrugCategories();
        }

        private void UpdatePagedItemLocations()
        {
            if (AllItemLocations == null)
                return;

            var startIndex = (ItemLocationsCurrentPage - 1) * ItemLocationsPerPage;
            var pagedItems = AllItemLocations.Skip(startIndex).Take(ItemLocationsPerPage).ToList();

            if (PagedItemLocations == null)
                PagedItemLocations = new ObservableCollection<ItemLocation>();

            PagedItemLocations.Clear();
            foreach (var item in pagedItems)
            {
                PagedItemLocations.Add(item);
            }

            // 항목 선택 상태 업데이트
            UpdateItemSelectionStatus();

            OnPropertyChanged(nameof(ItemLocationsTotalPages));
            OnPropertyChanged(nameof(ItemLocationsPageInfo));
        }

        private void UpdateItemSelectionStatus()
        {
            if (AllItemLocations == null || ReceivingList == null) return;

            foreach (var item in AllItemLocations)
            {
                // 항목 그리드에 동일한 일련번호의 항목이 있는지 확인
                bool isInGrid = ReceivingList.Any(receivingItem =>
                    receivingItem.SerialNumber == item.SerialNumber);

                item.IsSelected = isInGrid;
            }
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
            if (AllItemLocations == null)
                AllItemLocations = new List<ItemLocation>();
            AllItemLocations.Clear();

            // 선택된 약품 분류에 따라 해당 약품의 항목들만 표시
            var selectedCategory = AllDrugCategories?.FirstOrDefault(c => c.Code == SelectedFilter);
            if (selectedCategory == null) return;

            // 약품 분류별로 항목 데이터 생성
            switch (SelectedFilter)
            {
                case "DM-PERLINGJ":
                    // 페링가니트 0.1% 주사 10ml 항목들
                    for (int i = 1; i <= 15; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"PERL-{i}-{i}",
                            SerialNumber = $"PERL-{i:D3}",
                            DrugCode = "DM-PERLINGJ",
                            DrugName = "페링가니트 0.1% 주사 10ml",
                            Quantity = 5 + (i % 10),
                            ExpiryDate = DateTime.Now.AddMonths(6 + (i % 12)),
                            Status = i % 5 == 0 ? "유효기간경과" : (i % 7 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 3 == 0
                        });
                    }
                    break;

                case "DM-TRI":
                    // 트리람 정 0.25mg 항목들
                    for (int i = 1; i <= 12; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"TRI-{i}-{i}",
                            SerialNumber = $"TRI-{i:D3}",
                            DrugCode = "DM-TRI",
                            DrugName = "트리람 정 0.25mg",
                            Quantity = 8 + (i % 8),
                            ExpiryDate = DateTime.Now.AddMonths(3 + (i % 9)),
                            Status = i % 4 == 0 ? "유효기간경과" : (i % 6 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 4 == 0
                        });
                    }
                    break;

                case "DN-IAC":
                    // 코노펜캡슐 항목들
                    for (int i = 1; i <= 18; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"IAC-{i}-{i}",
                            SerialNumber = $"IAC-{i:D3}",
                            DrugCode = "DN-IAC",
                            DrugName = "코노펜캡슐",
                            Quantity = 3 + (i % 7),
                            ExpiryDate = DateTime.Now.AddMonths(9 + (i % 6)),
                            Status = i % 3 == 0 ? "유효기간경과" : (i % 5 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 5 == 0
                        });
                    }
                    break;

                case "DLZP4J":
                    // 아티반주4mg 항목들
                    for (int i = 1; i <= 20; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"LZP-{i}-{i}",
                            SerialNumber = $"LZP-{i:D3}",
                            DrugCode = "DLZP4J",
                            DrugName = "아티반주4mg",
                            Quantity = 20 + (i % 10),
                            ExpiryDate = DateTime.Now.AddMonths(15 + (i % 8)),
                            Status = i % 6 == 0 ? "유효기간경과" : (i % 4 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 6 == 0
                        });
                    }
                    break;

                case "DM-TCO":
                    // 인산코데인 정 20mg (하나) 항목들
                    for (int i = 1; i <= 16; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"TCO-{i}-{i}",
                            SerialNumber = $"TCO-{i:D3}",
                            DrugCode = "DM-TCO",
                            DrugName = "인산코데인 정 20mg (하나)",
                            Quantity = 10 + (i % 6),
                            ExpiryDate = DateTime.Now.AddMonths(8 + (i % 10)),
                            Status = i % 7 == 0 ? "유효기간경과" : (i % 3 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 7 == 0
                        });
                    }
                    break;

                case "DM-WVSC":
                    // 바스캄주5mg/5ml 항목들
                    for (int i = 1; i <= 14; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"WVSC-{i}-{i}",
                            SerialNumber = $"WVSC-{i:D3}",
                            DrugCode = "DM-WVSC",
                            DrugName = "바스캄주5mg/5ml",
                            Quantity = 6 + (i % 9),
                            ExpiryDate = DateTime.Now.AddMonths(4 + (i % 11)),
                            Status = i % 8 == 0 ? "유효기간경과" : (i % 2 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 8 == 0
                        });
                    }
                    break;

                case "DBUPN5P":
                    // 노스판패취5mcg/h,5mg/P 항목들
                    for (int i = 1; i <= 22; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"BUPN-{i}-{i}",
                            SerialNumber = $"BUPN-{i:D3}",
                            DrugCode = "DBUPN5P",
                            DrugName = "노스판패취5mcg/h,5mg/P",
                            Quantity = 9 + (i % 5),
                            ExpiryDate = DateTime.Now.AddMonths(18 + (i % 7)),
                            Status = i % 9 == 0 ? "유효기간경과" : (i % 1 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 9 == 0
                        });
                    }
                    break;

                case "DM-MNT":
                    // 모노틴 정 항목들
                    for (int i = 1; i <= 17; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"MNT-{i}-{i}",
                            SerialNumber = $"MNT-{i:D3}",
                            DrugCode = "DM-MNT",
                            DrugName = "모노틴 정",
                            Quantity = 4 + (i % 12),
                            ExpiryDate = DateTime.Now.AddMonths(7 + (i % 13)),
                            Status = i % 10 == 0 ? "유효기간경과" : (i % 8 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 10 == 0
                        });
                    }
                    break;

                case "DG-CHX":
                    // 헥사메딘액0.12% 100ml 항목들
                    for (int i = 1; i <= 19; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"CHX-{i}-{i}",
                            SerialNumber = $"CHX-{i:D3}",
                            DrugCode = "DG-CHX",
                            DrugName = "헥사메딘액0.12% 100ml",
                            Quantity = 5 + (i % 15),
                            ExpiryDate = DateTime.Now.AddMonths(1 + (i % 24)),
                            Status = i % 5 == 0 ? "유효기간경과" : (i % 7 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 11 == 0
                        });
                    }
                    break;

                case "DM-FENT12":
                    // 듀로제식 디트랜스 패취 12mcg/h 5.25㎠ 항목들
                    for (int i = 1; i <= 13; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"FENT12-{i}-{i}",
                            SerialNumber = $"FENT12-{i:D3}",
                            DrugCode = "DM-FENT12",
                            DrugName = "듀로제식 디트랜스 패취 12mcg/h 5.25㎠",
                            Quantity = 7 + (i % 8),
                            ExpiryDate = DateTime.Now.AddMonths(12 + (i % 6)),
                            Status = i % 6 == 0 ? "유효기간경과" : (i % 4 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 6 == 0
                        });
                    }
                    break;

                default:
                    // 기본 데이터 - 선택된 약품 분류에 대한 샘플 데이터
                    for (int i = 1; i <= 10; i++)
                    {
                        AllItemLocations.Add(new ItemLocation
                        {
                            LocationCode = $"DEFAULT-{i}-{i}",
                            SerialNumber = $"DEFAULT-{i:D3}",
                            DrugCode = selectedCategory.Code,
                            DrugName = selectedCategory.Name,
                            Quantity = 5 + (i % 15),
                            ExpiryDate = DateTime.Now.AddMonths(1 + (i % 24)),
                            Status = i % 5 == 0 ? "유효기간경과" : (i % 7 == 0 ? "일련번호중복" : "정상"),
                            IsNew = i % 11 == 0
                        });
                    }
                    break;
            }

            // 페이징된 항목리스트 업데이트
            UpdatePagedItemLocations();
        }

        private void UpdatePagedReceivingList()
        {
            if (ReceivingList == null)
                return;

            var startIndex = (ReceivingGridCurrentPage - 1) * ReceivingGridPerPage;
            var pagedItems = ReceivingList.Skip(startIndex).Take(ReceivingGridPerPage).ToList();

            if (PagedReceivingList == null)
                PagedReceivingList = new ObservableCollection<ReceivingItem>();

            PagedReceivingList.Clear();
            foreach (var item in pagedItems)
            {
                PagedReceivingList.Add(item);
            }

            OnPropertyChanged(nameof(ReceivingGridTotalPages));
            OnPropertyChanged(nameof(ReceivingGridPageInfo));
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
            ItemLocationsPreviousPageCommand = new RelayCommand(ExecuteItemLocationsPreviousPage, CanExecuteItemLocationsPreviousPage);
            ItemLocationsNextPageCommand = new RelayCommand(ExecuteItemLocationsNextPage, CanExecuteItemLocationsNextPage);
            ReceivingGridPreviousPageCommand = new RelayCommand(ExecuteReceivingGridPreviousPage, CanExecuteReceivingGridPreviousPage);
            ReceivingGridNextPageCommand = new RelayCommand(ExecuteReceivingGridNextPage, CanExecuteReceivingGridNextPage);
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
                // GS1 바코드 파싱
                ParseGs1Barcode(_barcodeText);
                OnPropertyChanged(nameof(HasGs1Data));
            }
        }

        // GS1 파싱 결과 바인딩용 속성들
        private string _gs1ProductCode;
        public string Gs1ProductCode
        {
            get => _gs1ProductCode;
            set { _gs1ProductCode = value; OnPropertyChanged(); }
        }

        private string _gs1SerialNumber;
        public string Gs1SerialNumber
        {
            get => _gs1SerialNumber;
            set { _gs1SerialNumber = value; OnPropertyChanged(); }
        }

        private DateTime? _gs1ExpiryDate;
        public DateTime? Gs1ExpiryDate
        {
            get => _gs1ExpiryDate;
            set { _gs1ExpiryDate = value; OnPropertyChanged(); OnPropertyChanged(nameof(Gs1ExpiryDateText)); }
        }

        public string Gs1ExpiryDateText
        {
            get => Gs1ExpiryDate.HasValue ? Gs1ExpiryDate.Value.ToString("yyyy-MM-dd") : string.Empty;
        }

        private string _gs1LotNumber;
        public string Gs1LotNumber
        {
            get => _gs1LotNumber;
            set { _gs1LotNumber = value; OnPropertyChanged(); }
        }

        public bool HasGs1Data
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Gs1ProductCode)
                    || !string.IsNullOrWhiteSpace(Gs1SerialNumber)
                    || Gs1ExpiryDate.HasValue
                    || !string.IsNullOrWhiteSpace(Gs1LotNumber);
            }
        }

        private void ParseGs1Barcode(string raw)
        {
            Gs1ProductCode = string.Empty;
            Gs1SerialNumber = string.Empty;
            Gs1ExpiryDate = null;
            Gs1LotNumber = string.Empty;

            if (string.IsNullOrWhiteSpace(raw))
            {
                OnPropertyChanged(nameof(HasGs1Data));
                return;
            }

            // 그룹분리자(FNC1)
            char gs = (char)0x1D;
            string input = raw.Replace(" ", string.Empty);

            try
            {
                // 괄호 포함 형태 (01)(17)(10)(21) 지원 및 순서 무관 처리
                // 01: GTIN 14자리 숫자
                var gtinMatch = System.Text.RegularExpressions.Regex.Match(input, @"\(?01\)?(?<v>\d{14})");
                if (gtinMatch.Success)
                {
                    Gs1ProductCode = gtinMatch.Groups["v"].Value;
                }

                // 17: 유효기간 YYMMDD
                var expMatch = System.Text.RegularExpressions.Regex.Match(input, @"\(?17\)?(?<v>\d{6})");
                if (expMatch.Success)
                {
                    var v = expMatch.Groups["v"].Value;
                    // YYMMDD → 20YY 가정
                    int year = 2000 + int.Parse(v.Substring(0, 2));
                    int month = int.Parse(v.Substring(2, 2));
                    int day = int.Parse(v.Substring(4, 2));
                    if (month >= 1 && month <= 12 && day >= 1 && day <= 31)
                    {
                        Gs1ExpiryDate = new DateTime(year, month, day);
                    }
                }

                // 10: 제조번호(가변길이, FNC1/끝까지)
                // FNC1 분리자 또는 다른 AI 앞에서 종료
                string lot = TryExtractVariableField(input, "10", gs);
                if (!string.IsNullOrEmpty(lot))
                {
                    Gs1LotNumber = lot;
                }

                // 21: 일련번호(가변길이)
                string sn = TryExtractVariableField(input, "21", gs);
                if (!string.IsNullOrEmpty(sn))
                {
                    Gs1SerialNumber = sn;
                }
            }
            catch
            {
                // 파싱 중 오류는 무시하고 바인딩만 갱신
            }

            OnPropertyChanged(nameof(HasGs1Data));
        }

        private static string TryExtractVariableField(string input, string ai, char groupSep)
        {
            // 패턴: (ai)valueUntilSepOrNextAI
            // 괄호가 있든 없든 허용
            int idx = input.IndexOf("(" + ai + ")", StringComparison.Ordinal);
            int aiLen = 0;
            if (idx >= 0)
            {
                aiLen = ("(" + ai + ")").Length;
            }
            else
            {
                idx = input.IndexOf(ai, StringComparison.Ordinal);
                if (idx < 0) return null;
                aiLen = ai.Length;
            }

            int start = idx + aiLen;
            if (start >= input.Length) return string.Empty;

            // 그룹분리자까지
            int sepPos = input.IndexOf(groupSep, start);
            string slice = sepPos >= 0 ? input.Substring(start, sepPos - start) : input.Substring(start);

            // 다른 AI를 만나기 전까지 유효 (다른 AI는 2자리 또는 3자리 숫자로 시작)
            var nextAi = System.Text.RegularExpressions.Regex.Match(slice, @"\(?(?:\d{2}|\d{3})\)?");
            if (nextAi.Success)
            {
                int aiStart = nextAi.Index;
                if (aiStart > 0)
                {
                    slice = slice.Substring(0, aiStart);
                }
            }

            return slice.Trim('\u001D');
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
                // ReceivingList가 변경될 때 항목 선택 상태 업데이트
                UpdateItemSelectionStatus();
                // 페이징된 그리드 데이터 업데이트
                UpdatePagedReceivingList();
            }
        }

        // 그리드 페이징 관련 속성들
        private int _receivingGridCurrentPage = 1;
        public int ReceivingGridCurrentPage
        {
            get => _receivingGridCurrentPage;
            set
            {
                _receivingGridCurrentPage = value;
                OnPropertyChanged();
                UpdatePagedReceivingList();
            }
        }

        public int ReceivingGridTotalPages
        {
            get => (int)Math.Ceiling((double)ReceivingList.Count / ReceivingGridPerPage);
        }

        public string ReceivingGridPageInfo
        {
            get => $"{ReceivingGridCurrentPage} / {ReceivingGridTotalPages}";
        }

        private ObservableCollection<ReceivingItem> _pagedReceivingList;
        public ObservableCollection<ReceivingItem> PagedReceivingList
        {
            get => _pagedReceivingList;
            set
            {
                _pagedReceivingList = value;
                OnPropertyChanged();
            }
        }

        // 항목리스트 페이징 관련 속성들
        private int _itemLocationsCurrentPage = 1;
        public int ItemLocationsCurrentPage
        {
            get => _itemLocationsCurrentPage;
            set
            {
                _itemLocationsCurrentPage = value;
                OnPropertyChanged();
                UpdatePagedItemLocations();
            }
        }

        public int ItemLocationsTotalPages
        {
            get => (int)Math.Ceiling((double)AllItemLocations.Count / ItemLocationsPerPage);
        }

        public string ItemLocationsPageInfo
        {
            get => $"{ItemLocationsCurrentPage} / {ItemLocationsTotalPages}";
        }

        private List<ItemLocation> _allItemLocations;
        public List<ItemLocation> AllItemLocations
        {
            get => _allItemLocations;
            set
            {
                _allItemLocations = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ItemLocation> _pagedItemLocations;
        public ObservableCollection<ItemLocation> PagedItemLocations
        {
            get => _pagedItemLocations;
            set
            {
                _pagedItemLocations = value;
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
        public ICommand ItemLocationsPreviousPageCommand { get; private set; }
        public ICommand ItemLocationsNextPageCommand { get; private set; }
        public ICommand ReceivingGridPreviousPageCommand { get; private set; }
        public ICommand ReceivingGridNextPageCommand { get; private set; }

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

            // 해당 위치의 항목 찾기
            var itemToAdd = AllItemLocations?.FirstOrDefault(item => item.LocationCode == locationCode);
            if (itemToAdd == null) return;

            // 이미 그리드에 있는지 확인
            var existingItem = ReceivingList?.FirstOrDefault(item => item.SerialNumber == itemToAdd.SerialNumber);
            if (existingItem != null)
            {
                MessageBox.Show($"{itemToAdd.DrugName} - {itemToAdd.SerialNumber} 항목이 이미 그리드에 있습니다.");
                return;
            }

            // 그리드에 항목 추가
            ReceivingList.Add(new ReceivingItem
            {
                Warehouse = itemToAdd.DrugName,
                DrugName = itemToAdd.DrugName,
                SerialNumber = itemToAdd.SerialNumber,
                Quantity = 1,
                CurrentTotalQuantity = itemToAdd.Quantity
            });

            // 항목 선택 상태 업데이트
            UpdateItemSelectionStatus();

            MessageBox.Show($"{itemToAdd.DrugName} - {itemToAdd.SerialNumber} 항목이 그리드에 추가되었습니다.");
        }

        private void ExecuteRemoveReceivingItem(object parameter)
        {
            if (parameter is ReceivingItem item)
            {
                ReceivingList.Remove(item);
                OnPropertyChanged(nameof(TotalReceivingQuantity));
                OnPropertyChanged(nameof(BoxSummary));
                // 항목 선택 상태 업데이트
                UpdateItemSelectionStatus();
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
                // 항목 선택 상태 업데이트
                UpdateItemSelectionStatus();
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

        private void ExecuteItemLocationsPreviousPage(object parameter)
        {
            if (ItemLocationsCurrentPage > 1)
            {
                ItemLocationsCurrentPage--;
            }
        }

        private bool CanExecuteItemLocationsPreviousPage(object parameter)
        {
            return ItemLocationsCurrentPage > 1;
        }

        private void ExecuteItemLocationsNextPage(object parameter)
        {
            if (ItemLocationsCurrentPage < ItemLocationsTotalPages)
            {
                ItemLocationsCurrentPage++;
            }
        }

        private bool CanExecuteItemLocationsNextPage(object parameter)
        {
            return ItemLocationsCurrentPage < ItemLocationsTotalPages;
        }

        private void ExecuteReceivingGridPreviousPage(object parameter)
        {
            if (ReceivingGridCurrentPage > 1)
            {
                ReceivingGridCurrentPage--;
            }
        }

        private bool CanExecuteReceivingGridPreviousPage(object parameter)
        {
            return ReceivingGridCurrentPage > 1;
        }

        private void ExecuteReceivingGridNextPage(object parameter)
        {
            if (ReceivingGridCurrentPage < ReceivingGridTotalPages)
            {
                ReceivingGridCurrentPage++;
            }
        }

        private bool CanExecuteReceivingGridNextPage(object parameter)
        {
            return ReceivingGridCurrentPage < ReceivingGridTotalPages;
        }

        #endregion
    }
}