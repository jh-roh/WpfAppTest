using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfMultipleCommandParameter
{
    [POCOViewModel]
    public class TestViewModel
    {
        public static TestViewModel Create()
        {
            return ViewModelSource.Create(() => new TestViewModel());
        }
        protected TestViewModel() { }

        public void ClickMethod(object[] parameter)
        {
            
        }

    }

    public class MultiValueConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
