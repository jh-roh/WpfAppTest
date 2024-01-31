using DevExpress.Data.TreeList;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppTest.ViewModel
{
    [POCOViewModel]
    public class ItemInfoClass
    {
        public virtual string HospitalMedicineCode { get; set; }

        public virtual string ItemName { get; set; }


        public ItemInfoClass() { }
    }


    [POCOViewModel]
    public class StockingScreen
    {
        public virtual int SeqNo { get; set; }

        public virtual string HospitalMedicineCode { get; set; }

        public virtual string ItemName { get; set; }

        public virtual DateTime ExpiredDate { get; set; }

        public virtual String LotNo { get; set; }

        public virtual String SerialNo { get; set; }

        public virtual string BoxUnit { get; set; }


        public virtual int RefillQty { get; set; }

        public virtual int CompleteQty { get; set; } 

        public StockingScreen() 
        {
        }

    }

    [POCOViewModel]
    public class StockingScreenTestModel
    {
        public static StockingScreenTestModel Create()
        {
            return ViewModelSource.Create(() => new StockingScreenTestModel());
        }
        public StockingScreenTestModel() 
        {
            this.AllStockList = new List<StockingScreen>();
            this.StockList = new ObservableCollection<StockingScreen>();
            this.ItemList = new ObservableCollection<ItemInfoClass>();
            this.IsVisibleDetailInputControl = false;
        }

        public virtual List<StockingScreen> AllStockList { get; set; }

        public virtual ObservableCollection<StockingScreen> StockList { get; set; }
        public virtual ObservableCollection<ItemInfoClass> ItemList { get; set; }

        public virtual ItemInfoClass SelectedItemInfo { get; set; }

        public virtual bool IsVisibleDetailInputControl { get; set; }

        public void RemoveStockData(StockingScreen removeData)
        {
            MessageBox.Show($"{removeData.SeqNo} 데이터 삭제");
        }

        public void PreviousMoveData()
        {
            MessageBox.Show("이전 항목");


        }

        public void NextMoveData()
        {
            MessageBox.Show("다음 항목");
        }

        public void DetailInput()
        {
            IsVisibleDetailInputControl = true;
        }

        //public void Login()
        //{
        //    this.GetService<IMessageBoxService>().Show("Login succeeded", "Login", MessageButton.OK, MessageIcon.Information, MessageResult.OK);
        //}
        //public bool CanLogin()
        //{
        //    return !string.IsNullOrEmpty(UserName);
        //}
    }
}
