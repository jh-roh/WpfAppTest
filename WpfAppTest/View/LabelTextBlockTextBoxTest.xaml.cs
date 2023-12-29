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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppTest.View
{
    /// <summary>
    /// LabelTextBlockTextBoxTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LabelTextBlockTextBoxTest : UserControl
    {
        public LabelTextBlockTextBoxTest()
        {
            InitializeComponent();

           // AddButton();
        }

        //private void AddButton()
        //{
        //    Button btn = new Button();
        //    btn.FontWeight = FontWeights.Bold;

        //    WrapPanel p = new WrapPanel();
            
        //    TextBlock txt = new TextBlock();
        //    txt.Text = "멀티";
        //    txt.Foreground = Brushes.Blue;
        //    p.Children.Add(txt);

        //    TextBlock txt2 = new TextBlock();
        //    txt2.Text = "색상";
        //    txt2.Foreground = Brushes.Red;
        //    p.Children.Add(txt2);

        //    btn.Content = p;

        //    spMain.Children.Add(btn);
        //}

        private void hlYH_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //txtDispInfo.Text = "선택 시작 문자 인덱스 :" + txtInput.SelectionStart + Environment.NewLine;
            //txtDispInfo.Text += "선택문자열 수 :" + txtInput.SelectionLength + Environment.NewLine;
            //txtDispInfo.Text += "선택문자열 :" + txtInput.SelectedText + Environment.NewLine;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("저장되었습니다.");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
