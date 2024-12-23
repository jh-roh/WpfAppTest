using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SocketTester.UI.Converter
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MultiValueIntArrayConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0)
            {
                return null; // 또는 빈 배열: new int[0];
            }

            // 모든 값을 int로 변환 시도. 변환 실패 시 null 반환
            int[] intArray = values.Select(v =>
            {
                if (v == DependencyProperty.UnsetValue) return -1; // DependencyProperty.UnsetValue 처리
                if (v == null) return -1; // null 값 처리
                if (int.TryParse(v.ToString(), out int parsedValue))
                {
                    return parsedValue;
                }
                return -1; // 변환 실패 시 -1 반환 (또는 다른 적절한 값이나 예외 처리)
            }).ToArray();


            // 하나라도 변환에 실패했다면 null을 반환하는 방법 (선택적)
            //if (intArray.Any(i => i == -1)) return null;

            return intArray; // int[] 배열을 object로 반환
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // 일반적으로 ConvertBack은 필요하지 않음
        }
    }

}
