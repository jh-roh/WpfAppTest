using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfAppTest.View.DrugManagement
{
    public class StringToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2) return false;

                var selected = values[0]?.ToString() ?? string.Empty;
                var code = values[1]?.ToString() ?? string.Empty;

                return string.Equals(selected, code, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
            throw new NotImplementedException();
        }
    }


    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                return isVisible ? Visibility.Visible : Visibility.Hidden;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
    /// <summary>
    /// 재고 상태를 색상으로 변환하는 컨버터
    /// </summary>
    public class StockStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCriticalStock)
            {
                if (isCriticalStock)
                    return new SolidColorBrush(Colors.Red); // 위험 상태 - 빨간색
                else
                    return new SolidColorBrush(Colors.Orange); // 부족 상태 - 주황색
            }

            return new SolidColorBrush(Colors.Green); // 정상 상태 - 초록색
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 재고 상태를 아이콘으로 변환하는 컨버터
    /// </summary>
    public class StockStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCriticalStock)
            {
                if (isCriticalStock)
                    return "⚠"; // 위험 상태 - 경고 아이콘
                else
                    return "⚠"; // 부족 상태 - 경고 아이콘
            }

            return "✓"; // 정상 상태 - 체크 아이콘
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}