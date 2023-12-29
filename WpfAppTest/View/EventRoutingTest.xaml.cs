using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppTest.View
{
    /// <summary>
    /// EventRoutingTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EventRoutingTest : UserControl
    {
        public EventRoutingTest()
        {
            InitializeComponent();
        }

        private void sp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("스택 패널");
        }

        private void btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("버튼 마우스 다운");

        }

        private void grd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("그리드");

        }

        private void cvs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("캔버스");
            e.Handled = true;

        }

        private void elp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("타원");

        }

        private void txt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("텍스트박스");

        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("버튼 클릭");

        }

        private void grd_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Preview 그리드");

        }

        private void cvs_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Preview 캔버스");
            //e.Handled = true;
        }

        private void txt_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Preview 텍스트박스");

        }

        private void elp_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Preview 타원");

        }
    }
}
