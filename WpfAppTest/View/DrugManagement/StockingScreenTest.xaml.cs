using DevExpress.Xpf.Grid.LookUp;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppTest.ViewModel;

namespace WpfAppTest.View
{
    /// <summary>
    /// StockingScreenTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StockingScreenTest : UserControl
    {

        object eventLock = new object();
        StockingScreenTestModel model = null;


        public StockingScreenTest()
        {
            InitializeComponent();

            model = (StockingScreenTestModel)this.DataContext;

            model.AllStockList.Add(new StockingScreen() { SeqNo = 1, HospitalMedicineCode="ADrug", ItemName = "A마약", 
                                                       ExpiredDate = DateTime.MaxValue, LotNo = "LotNo1_ADrug", SerialNo = "SerialNo1_ADrug"
                                                       , BaseUnit = 10, RefillQty = 1 /*, BoxQtyString = "0Box, 1EA"*/});
            model.AllStockList.Add(new StockingScreen() { SeqNo = 2, HospitalMedicineCode="ADrug", ItemName = "A마약", 
                                                       ExpiredDate = DateTime.MaxValue, LotNo = "LotNo2_ADrug", SerialNo = "SerialNo2_ADrug"
                                                       , BaseUnit = 10, RefillQty = 3 /*, BoxQtyString = "0Box, 3EA"*/});

            model.AllStockList.Add(new StockingScreen() { SeqNo =3, HospitalMedicineCode="BDrug", ItemName = "B마약", 
                                                       ExpiredDate = DateTime.MaxValue, LotNo = "LotNo1_BDrug", SerialNo = "SerialNo1_BDrug"
                                                       , BaseUnit = 10, RefillQty = 3, /*BoxQtyString = "0Box, 3EA"*/});



            model.ItemList.Add(new ItemInfoClass() { HospitalMedicineCode = "ADrug", ItemName = "A마약" });
            model.ItemList.Add(new ItemInfoClass() { HospitalMedicineCode = "BDrug", ItemName = "B마약" });

            model.SelectedItemInfo = model.ItemList.FirstOrDefault();


            foreach (var stock in model.AllStockList.Where(p => p.HospitalMedicineCode == model.SelectedItemInfo.HospitalMedicineCode))
            {
                model.StockList.Add(stock);
            }
        }


        private void repeatBtn_Down_Click(object sender, RoutedEventArgs e)
        {
            lock (eventLock)
            {
                if (model.FocusedItem.RefillQty > 0)
                {
                    model.FocusedItem.RefillQty--;
                }

                gridControl_StockList.View.CommitEditing(true);
                gridControl_StockList.RefreshRow(gridControl_StockList.View.FocusedRowHandle);
            }

        }

        private void repeatBtn_Up_Click(object sender, RoutedEventArgs e)
        {
            lock (eventLock)
            {
                if (model.FocusedItem.RefillQty < model.FocusedItem.BaseUnit)
                {
                    model.FocusedItem.RefillQty++;
                }

                gridControl_StockList.View.CommitEditing(true);
                gridControl_StockList.RefreshRow(gridControl_StockList.View.FocusedRowHandle);
            }
        }
    }
}
