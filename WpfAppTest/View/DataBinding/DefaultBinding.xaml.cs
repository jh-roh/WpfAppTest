using System;
using System.Collections.Generic;
using System.Globalization;
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
using WpfAppTest.Model;

namespace WpfAppTest.View.DataBinding
{
    /// <summary>
    /// DefaultBinding.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DefaultBinding : UserControl
    {
        private Cust c1;

        public DefaultBinding()
        {
            InitializeComponent();

            //c1 = getData();
            //this.DataContext = c1;

            c1 = getData();

            //Validation.AddErrorHandler(this.txtAge, txtAge_ValidationErrorHandler);
        }

        //private void txtAge_ValidationErrorHandler(object sender, ValidationErrorEventArgs e)
        //{
        //    //MessageBox.Show(e.Error.ErrorContent.ToString(), "유효성 검사 에러");
        //    txtAge.ToolTip = e.Error.ErrorContent.ToString();
        //}

        private Cust getData()
        {
            //Cust c = new Cust() { Age = 17, Name = "홍길동", Tel = "010-1111-1111", IntPoint = 10 };

            //return c;

            var c = (Cust)this.FindResource("c2");
            return c; 
        }

        private void btnAddAge_Click(object sender, RoutedEventArgs e)
        {
            c1.Age++;

        }
    }

    // ValidationRule
    // 나이 : 0 ~ 125

    class AgeRangeRule : ValidationRule
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int n;
            if(!int.TryParse((string)value, out n))
            {
                return new ValidationResult(false, "잘못된 숫자 입니다.");
            }

            if(n >= Min && n <= Max)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, $"나이의 범위는 {Min} ~ {Max} 사이의 숫자 이어야 합니다.");
            }
        }
    }


    // 16 <-> 10 진수 변환
    [ValueConversion(typeof(int),typeof(string))]
    class Int32ToHex : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value).ToString("X"); // 10 -> A
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return int.Parse((string)value, System.Globalization.NumberStyles.HexNumber);

            }
            catch
            {
                return null;
            }


        }
    }


    // ~ 19 : Green
    // 20 ~ 40 : Blue
    // 41~ : Red

    [ValueConversion(typeof(int), typeof(Brush))]
    class Int32ToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType !=  typeof(Brush))
            {
                return null;
            }

            int age = Int32.Parse(value.ToString());
            if(age < 20)
            {
                return Brushes.Green;
            }
            else if (age <= 40)
            {
                return Brushes.Blue;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
