using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfAppTest.View.MouseHandling
{
    /// <summary>
    /// MouseLocation.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MouseLocation : UserControl
    {
        public MouseLocation()
        {
            InitializeComponent();
        }

        private void cvsMain_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(cvsMain);
            Debug.WriteLine($"cvsMain x: {p.X}, y : {p.Y}");

            Point p2 = Mouse.GetPosition(this);
            Debug.WriteLine($"grdMain x: {p2.X}, y : {p2.Y}");

        }

    }
}
