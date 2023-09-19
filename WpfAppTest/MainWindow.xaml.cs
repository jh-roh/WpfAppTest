using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfAppTest.Model;
using WpfAppTest.TestClass;
using WpfAppTest.TestJSon;
using WpfAppTest.ViewModel;

namespace WpfAppTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            //var dataTemplateVM = new DataTemplateModel();

            //ContentControl_Test.Content = dataTemplateVM;

            //MainClass.MainTest();
            //TestJSonClass testJson = new TestJSonClass();

            //ProcessStartInfoTest test = new ProcessStartInfoTest();

            //test.ProcessTest();

            //linqAnyMethodTest();

            //TestTrim test = new TestTrim();

            //test.TestMethod();
            //test.ToCharMethod();

        }

        private void button_DoorAging_Click(object sender, RoutedEventArgs e)
        {
            //popup_DoorAging.IsOpen = !popup_DoorAging.IsOpen;  
        }


        private void linqAnyMethodTest()
        {
            // 비교할 두 개의 리스트 생성
            List<int> list1 = new List<int> { 1, 2, 3, 4, 5 };
            List<int> list2 = new List<int> { 1, 2, 3, 9, 10 };

            // Any 메서드를 사용하여 두 리스트에서 공통된 요소가 있는지 확인
            bool hasCommonElements = list1.Any(element => list2.Contains(element));

            if (hasCommonElements)
            {
                Console.WriteLine("두 리스트에는 공통된 요소가 있습니다.");
            }
            else
            {
                Console.WriteLine("두 리스트에는 공통된 요소가 없습니다.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TestHttpRequestMethod.SendHttpRequestMethod();

        }
    }
}
