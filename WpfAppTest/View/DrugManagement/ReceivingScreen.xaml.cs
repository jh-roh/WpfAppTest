using System.Windows.Controls;
using WpfAppTest.Helper;

namespace WpfAppTest.View.DrugManagement
{
    /// <summary>
    /// ReceivingScreen.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReceivingScreen : UserControl
    {
        public ReceivingScreen()
        {
            InitializeComponent();
        }

        private void SearchTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            VirtualKeyboardHelper.ShowKeyboard();
        }

        private void SearchTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            VirtualKeyboardHelper.HideKeyboard();
        }
    }
}