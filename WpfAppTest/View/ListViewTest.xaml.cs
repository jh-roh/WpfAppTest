using DevExpress.Xpf.Grid;
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
using WpfAppTest.Model;

namespace WpfAppTest.View
{
    /// <summary>
    /// ListViewTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ListViewTest : UserControl
    {
        public ListViewTest()
        {
            InitializeComponent();
            LoadData();
        }

        List<Cust> list;
        private void LoadData()
        {
            list = new List<Cust>();
            list.Add(new Cust { Name = "홍길동", Age = 17 });
            list.Add(new Cust { Name = "이순신", Age = 58 });
            list.Add(new Cust { Name = "김유신", Age = 23 });
    
            lvCust.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            list.Add(new Cust() { Name = "을지문덕", Age = 45 });
            lvCust.ItemsSource = null;
            lvCust.ItemsSource = list;

        }
    }

}
