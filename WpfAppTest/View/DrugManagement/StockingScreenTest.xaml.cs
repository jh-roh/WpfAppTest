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
        public StockingScreenTest()
        {
            InitializeComponent();

            StockingScreenTestModel model = (StockingScreenTestModel)this.DataContext;

            model.AllStockList.Add(new StockingScreen() { SeqNo = 1, HospitalMedicineCode="ADrug", ItemName = "A마약", 
                                                       ExpiredDate = DateTime.MaxValue, LotNo = "LotNo1_ADrug", SerialNo = "SerialNo1_ADrug"
                                                       , BoxUnit = "박스 단위", RefillQty = 1, CompleteQty = 10});
            model.AllStockList.Add(new StockingScreen() { SeqNo = 2, HospitalMedicineCode="ADrug", ItemName = "A마약", 
                                                       ExpiredDate = DateTime.MaxValue, LotNo = "LotNo2_ADrug", SerialNo = "SerialNo2_ADrug"
                                                       , BoxUnit = "낱개 단위", RefillQty = 3, CompleteQty = 3});

            model.AllStockList.Add(new StockingScreen() { SeqNo =3, HospitalMedicineCode="BDrug", ItemName = "B마약", 
                                                       ExpiredDate = DateTime.MaxValue, LotNo = "LotNo1_BDrug", SerialNo = "SerialNo1_BDrug"
                                                       , BoxUnit = "낱개 단위", RefillQty = 3, CompleteQty = 3});



            model.ItemList.Add(new ItemInfoClass() { HospitalMedicineCode = "ADrug", ItemName = "A마약" });
            model.ItemList.Add(new ItemInfoClass() { HospitalMedicineCode = "BDrug", ItemName = "B마약" });

            model.SelectedItemInfo = model.ItemList.FirstOrDefault();


            foreach (var stock in model.AllStockList.Where(p => p.HospitalMedicineCode == model.SelectedItemInfo.HospitalMedicineCode))
            {
                model.StockList.Add(stock);
            }
        }


    }
}
