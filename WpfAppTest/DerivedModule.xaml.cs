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

namespace WpfAppTest
{
    /// <summary>
    /// Interaction logic for DerivedModule.xaml
    /// </summary>
    public partial class DerivedModule : BaseModule
    {
        public DerivedModule()
        {
            InitializeComponent();
        }


        private void Grid_AgingControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GetModuleVisibleData(ContentControl_AgingControl);
        }

    }
}
