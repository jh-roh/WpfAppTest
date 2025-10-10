using System.Windows.Controls;
using WpfAppTest.ViewModel;

namespace WpfAppTest.View.DrugManagement
{
    /// <summary>
    /// ReceivingItemsGroupedControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReceivingItemsGroupedControl : UserControl
    {
        public ReceivingItemsGroupedControl()
        {
            InitializeComponent();
            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            // ViewModel의 입고 명령 이벤트 핸들러 등록
            if (DataContext is ReceivingItemsGroupedViewModel viewModel)
            {
                viewModel.ReceiveItemRequested += OnReceiveItemRequested;
            }

            // 상세 화면의 뒤로가기 이벤트 핸들러 등록
            DetailView.DataContextChanged += (s, e) =>
            {
                if (DetailView.DataContext is DrugReceivingDetailViewModel detailViewModel)
                {
                    detailViewModel.BackRequested += OnBackRequested;
                }
            };
        }

        private void OnReceiveItemRequested(string drugCode, string drugName)
        {
            // 상세 화면으로 전환
            GroupedView.Visibility = System.Windows.Visibility.Hidden;
            DetailView.Visibility = System.Windows.Visibility.Visible;

            // 상세 화면에 선택된 약품 정보 전달
            if (DetailView.DataContext is DrugReceivingDetailViewModel detailViewModel)
            {
                detailViewModel.SelectedDrugCode = drugCode;
                detailViewModel.SelectedDrugName = drugName;
                detailViewModel.InitializeDataForDrug(drugCode);
            }
        }

        private void OnBackRequested()
        {
            // 그룹화된 화면으로 돌아가기
            GroupedView.Visibility = System.Windows.Visibility.Visible;
            DetailView.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
