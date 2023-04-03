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

            ProcessStartInfoTest test = new ProcessStartInfoTest();

            test.ProcessTest();
        }

        private void button_DoorAging_Click(object sender, RoutedEventArgs e)
        {
            //popup_DoorAging.IsOpen = !popup_DoorAging.IsOpen;  
        }

    }
}
