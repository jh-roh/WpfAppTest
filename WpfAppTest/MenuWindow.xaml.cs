using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppTest.View.DrugManagement;

namespace WpfAppTest
{
    /// <summary>
    /// MenuWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MenuWindow : Window
    {

        int cnt = 0;
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            txtStatus.Text = Application.Current.ShutdownMode.ToString();
        }

        private void btnNewWindow_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();

            cnt++;

            win.Title = "새창" + cnt.ToString();
            win.Width = 200;
            win.Height = 100;
            win.Show();

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("정말로 종료 하시겠습니까", "종료확인", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }


        private void mnuWInd_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            mnuWInd.Items.Clear();

            foreach (Window win in Application.Current.Windows)
            {
                if (win == this) continue;
                if (win.Title == "") continue;

                MenuItem item = new MenuItem();



                item.Header = win.Title;
                item.Click += Item_Click;
                item.Tag = win;


                mnuWInd.Items.Add(item);

                if (activeWindow != null && activeWindow == win)
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        Window activeWindow = null;
        private void Item_Click(object sender, RoutedEventArgs e)
        {
            Window w = (Window)((MenuItem)sender).Tag;
            ((MenuItem)sender).IsChecked = true;

            w.Activate();
            activeWindow = w;
        }

        private void btnReceivingItems_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            win.Title = "입고 항목 (상세)";
            win.Width = 1200;
            win.Height = 900;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var receivingItemsControl = new ReceivingItemsControl();
            win.Content = receivingItemsControl;

            win.Show();
        }

        private void btnReceivingItemsGrouped_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            win.Title = "입고 항목 (약품코드별 그룹화)";
            win.Width = 1200;
            win.Height = 900;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var receivingItemsGroupedControl = new ReceivingItemsGroupedControl();
            win.Content = receivingItemsGroupedControl;

            win.Show();
        }

        private void btnReceivingDetail_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            win.Title = "입고 상세 (위치별)";
            win.Width = 1200;
            win.Height = 900;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var receivingDetailControl = new DrugReceivingDetailControl();
            win.Content = receivingDetailControl;

            win.Show();
        }
    }
}
