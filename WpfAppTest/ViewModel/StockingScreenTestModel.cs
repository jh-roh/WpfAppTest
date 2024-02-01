using DevExpress.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfAppTest.Model;

namespace WpfAppTest.ViewModel
{
    public class ItemInfoClass : PropertyChangedBase
    {
        private string _HospitalMedicineCode;

        public string HospitalMedicineCode
        {
            get
            {
                return _HospitalMedicineCode;
            }
            set
            {
                _HospitalMedicineCode = value;
                OnPropertyChanged();
            }
        }


        private string _ItemName;

        public string ItemName
        {
            get
            {
                return _ItemName;
            }
            set
            {
                _ItemName = value;
                OnPropertyChanged();
            }
        }

       
    }

    public class StockingScreen : PropertyChangedBase
    {
        private int _SeqNo;

        public int SeqNo
        {
            get
            {
                return _SeqNo;
            }
            set
            {
                _SeqNo = value;
                OnPropertyChanged();
            }
        }


        private string _HospitalMedicineCode;

        public string HospitalMedicineCode
        {
            get
            {
                return _HospitalMedicineCode;
            }
            set
            {
                _HospitalMedicineCode = value;
                OnPropertyChanged();
            }
        }


        private string _ItemName;

        public string ItemName
        {
            get
            {
                return _ItemName;
            }
            set
            {
                _ItemName = value;
                OnPropertyChanged();
            }
        }


        private DateTime _ExpiredDate;

        public DateTime ExpiredDate
        {
            get
            {
                return _ExpiredDate;
            }
            set
            {
                _ExpiredDate = value;
                OnPropertyChanged();
            }
        }

        private string _LotNo;

        public string LotNo
        {
            get
            {
                return _LotNo;
            }
            set
            {
                _LotNo = value;
                OnPropertyChanged();
            }
        }


        private string _SerialNo;

        public string SerialNo
        {
            get
            {
                return _SerialNo;
            }
            set
            {
                _SerialNo = value;
                OnPropertyChanged();
            }
        }



        private int _BaseUnit;

        public int BaseUnit
        {
            get
            {
                return _BaseUnit;
            }
            set
            {
                _BaseUnit = value;
                OnPropertyChanged();
            }
        }

        private string _BoxQtyString;

        public string BoxQtyString
        {
            get
            {
                return _BoxQtyString;
            }
            set
            {
                _BoxQtyString = value;
                OnPropertyChanged();
            }
        }
        private int _RefillQty;

        public int RefillQty
        {
            get
            {
                return _RefillQty;
            }
            set
            {
                _RefillQty = value;


                if (_RefillQty == BaseUnit)
                {
                    BoxQtyString = $"1Box, 0EA";
                }
                else
                {
                    BoxQtyString = $"0Box, {_RefillQty}EA";

                }



                OnPropertyChanged();
            }
        }

    }

    public class StockingScreenTestModel : PropertyChangedBase
    {
        public StockingScreenTestModel() 
        {
            this.AllStockList = new List<StockingScreen>();
            this.StockList = new ObservableCollection<StockingScreen>();
            this.ItemList = new ObservableCollection<ItemInfoClass>();
            this.IsVisibleDetailInputControl = false;
        }

        public List<StockingScreen> AllStockList { get; set; }

        private ObservableCollection<StockingScreen> _StockList;

        public ObservableCollection<StockingScreen> StockList
        {
            get
            {
                return _StockList;
            }
            set
            {
                _StockList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ItemInfoClass> _ItemList;

        public ObservableCollection<ItemInfoClass> ItemList
        {
            get
            {
                return _ItemList;
            }
            set
            {
                _ItemList = value;
                OnPropertyChanged();
            }
        }

        private ItemInfoClass _SelectedItemInfo;

        public ItemInfoClass SelectedItemInfo 
        {
            get => _SelectedItemInfo;
            set
            {
                _SelectedItemInfo = value;
                OnPropertyChanged();
            }
        }

        private StockingScreen _FocusedItem;

        public StockingScreen FocusedItem
        {
            get => _FocusedItem;
            set
            {
                _FocusedItem = value;
                OnPropertyChanged();
            }
        }

        private bool _IsVisibleDetailInputControl;

        public bool IsVisibleDetailInputControl
        {
            get => _IsVisibleDetailInputControl;
            set
            {
                _IsVisibleDetailInputControl = value;
                OnPropertyChanged();
            }
        }


        private RelayCommand _RemoveStockDataCommand;

        public ICommand RemoveStockDataCommand
        {
            get
            {
                return _RemoveStockDataCommand ?? (_RemoveStockDataCommand = new RelayCommand(RemoveStockDataExcute));
            }
        }


        private RelayCommand _PreviousMoveDataCommand;

        public ICommand PreviousMoveDataCommand
        {
            get
            {
                return _PreviousMoveDataCommand ?? (_PreviousMoveDataCommand = new RelayCommand(PreviousMoveDataExcute));
            }
        }


        private RelayCommand _NextMoveDataCommand;

        public ICommand NextMoveDataCommand
        {
            get
            {
                return _NextMoveDataCommand ?? (_NextMoveDataCommand = new RelayCommand(NextMoveDataExcute));
            }
        }


        private RelayCommand _DetailInputCommand;

        public ICommand DetailInputCommand
        {
            get
            {
                return _DetailInputCommand ?? (_DetailInputCommand = new RelayCommand(DetailInputExcute));
            }
        }

        private void RemoveStockDataExcute(object obj)
        {

            StockingScreen removeData = obj as StockingScreen;

            MessageBox.Show($"{removeData.SeqNo} 데이터 삭제");
        }



        private void PreviousMoveDataExcute(object obj)
        {
            MessageBox.Show("이전 항목");
        }


        private void NextMoveDataExcute(object obj)
        {
            MessageBox.Show("다음 항목");
        }

        private void DetailInputExcute(object obj)
        {
            IsVisibleDetailInputControl = true;
        }

    }
}
