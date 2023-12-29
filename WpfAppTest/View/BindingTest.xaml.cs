using System;
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
using WpfAppTest.Model;

namespace WpfAppTest.View
{
    /// <summary>
    /// BindingTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BindingTest : UserControl
    {
        Custs custList;

        public BindingTest()
        {
            InitializeComponent();

            custList = (Custs)this.FindResource("custs2");

            //custList = new Custs();

            //grdMain.DataContext = custList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            custList.Add(new Cust(txtName.Text.Trim(), txtTel.Text.Trim(),0));
        }
    }
}
