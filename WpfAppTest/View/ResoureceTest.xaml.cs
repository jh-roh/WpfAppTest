﻿using System;
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
    /// ResoureceTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResoureceTest : UserControl
    {
        public ResoureceTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(this.FindResource("strClass").ToString());
            //MessageBox.Show(this.FindResource("strClass").ToString());
            //MessageBox.Show(Application.Current.FindResource("strClass").ToString());
            MessageBox.Show(stackPanel2.FindResource("strClass").ToString());
        }
    }
}
