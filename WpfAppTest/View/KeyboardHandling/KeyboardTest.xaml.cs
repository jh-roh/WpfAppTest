using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfAppTest.View.KeyboardHandling
{
    /// <summary>
    /// KeyboardTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KeyboardTest : UserControl
    {
        public KeyboardTest()
        {
            InitializeComponent();
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            //if(e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            //{
            //   MessageBox.Show("Ctrl 키가 눌러짐 입력");

            //}

            //Debug.WriteLine((int)Keyboard.Modifiers);
            //    W  S  C  A
            //    0  0  0  1
            // &  0  0  1  0
            //    0  0  0  0

            //if((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            //{
            //    MessageBox.Show("Ctrl 키가 눌러짐 입력");
            //}

            if(Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.S))
            {
                MessageBox.Show("저장완료");
            }
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            find();
        }

        private void find()
        {
            MessageBox.Show(txtFind.Text + "문자(열)로 검색을 완료했습니다.");
        }

        private void btnFind_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.F))
            {
                e.Handled = true;
                find();
            }
        }
    }
}
