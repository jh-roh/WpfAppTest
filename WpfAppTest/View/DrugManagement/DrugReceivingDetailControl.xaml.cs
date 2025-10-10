using System.Windows.Controls;

namespace WpfAppTest.View.DrugManagement
{
    /// <summary>
    /// DrugReceivingDetailControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DrugReceivingDetailControl : UserControl
    {
        public DrugReceivingDetailControl()
        {
            InitializeComponent();
        }

        public DrugReceivingDetailControl(string drugCode, string drugName) : this()
        {
            // ViewModel에 선택된 약품 정보 전달
            if (DataContext is ViewModel.DrugReceivingDetailViewModel viewModel)
            {
                viewModel.SelectedDrugCode = drugCode;
                viewModel.SelectedDrugName = drugName;
                viewModel.InitializeDataForDrug(drugCode);
            }
        }
    }
}
