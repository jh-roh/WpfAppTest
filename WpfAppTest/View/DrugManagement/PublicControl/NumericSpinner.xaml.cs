using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfAppTest.View.DrugManagement.PublicControl
{
    /// <summary>
    /// NumericSpinner.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumericSpinner : UserControl, INotifyPropertyChanged
    {

        public NumericSpinner()
        {
            InitializeComponent();
            DataContext = this;
        }


        #region Fields
        private static DependencyProperty CurrentValueProperty = DependencyProperty.Register("CurrentValue", typeof(int), typeof(NumericSpinner), new PropertyMetadata(0));

        private static DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(int), typeof(NumericSpinner), new PropertyMetadata(0));

        private static DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericSpinner), new PropertyMetadata(999));

        private static DependencyProperty EnabledProperty = DependencyProperty.Register("Enabled", typeof(bool), typeof(NumericSpinner), new PropertyMetadata(true));



        private const int CurQtyLimit = 999;


        public int CurrentValue
        {
            set
            {
                SetValue(CurrentValueProperty, value);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentValue"));

            }
            get { return (int)GetValue(CurrentValueProperty); }
        }


        public int MinValue
        {
            set { SetValue(MinValueProperty, value); }
            get { return (int)GetValue(MinValueProperty); }
        }


        public int MaxValue
        {
            set { SetValue(MaxValueProperty, value); }
            get { return (int)GetValue(MaxValueProperty); }
        }


        public bool Enabled
        {
            set { SetValue(EnabledProperty, value); }
            get { return (bool)GetValue(EnabledProperty); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        #region Event Handler







        /// <summary>
        /// 수량 감소 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repeatBtn_Down_Click(object sender, RoutedEventArgs e)
        {
            ValidationDownValue(CurrentValue);
        }



        /// <summary>
        /// 수량 증가 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repeatBtn_Up_Click(object sender, RoutedEventArgs e)
        {
            ValidationUpValue(CurrentValue);
        }



        #endregion




        #region Private



        /// <summary>
        /// 수량 증가 검증
        /// </summary>
        /// <returns></returns>
        private bool ValidationUpValue(int _inputValue)
        {
            bool isValid = false;

            if (_inputValue < MaxValue)
            {
                isValid = true;
                CurrentValue++;
            }
            else
            {
                MessageBox.Show("제한수량을 초과할 수 없습니다.");
            }

            return isValid;
        }


        /// <summary>
        /// 수량 감소 검증
        /// </summary>
        /// <returns></returns>
        private bool ValidationDownValue(int _inputValue)
        {
            bool isValid = false;

            if (_inputValue > MinValue)
            {
                isValid = true;
                CurrentValue--;
            }

            return isValid;
        }


        /// <summary>
        /// 키보드 입력 값 검증
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        private bool ValidateKeyValue(Key _key)
        {
            return (_key >= Key.D0 && _key <= Key.D9)
                || (_key >= Key.NumPad0 && _key <= Key.NumPad9)
                || _key == Key.Up
                || _key == Key.Down
                || _key == Key.Right
                || _key == Key.Left
                || _key == Key.Back;
        }



        #endregion
    }
}
