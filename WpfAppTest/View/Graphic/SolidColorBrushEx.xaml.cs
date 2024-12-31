using System;
using System.Collections.Generic;
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

namespace WpfAppTest.View.Graphic
{
    /// <summary>
    /// SolidColorBrushEx.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SolidColorBrushEx : UserControl
    {
        public SolidColorBrushEx()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //grd.Background = new SolidColorBrush(Color.FromRgb(0, 255, 255));
            //grd.Background = SystemColors.ControlBrush;
            
            //투명을 지원하는 rgb
            //grd.Background = new SolidColorBrush(Color.FromArgb(128, 0, 255, 255));

        }
    }
}
