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
    /// DataGridControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataGridControl : UserControl
    {
        public DataGridControl()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            List<Cust> list = new List<Cust>();

            list.Add(new Cust() { Id = 1, Name = "홍길동", BirthDay = new DateTime(1845, 3, 20), ImageUrl = "https://cdn3.iconfinder.com/data/icons/pixel-perfect-at-16px-volume-3-1/16/2049-512.png" });
            list.Add(new Cust() { Id = 1, Name = "이순신", BirthDay = new DateTime(1751, 5, 5), ImageUrl = "https://cdn3.iconfinder.com/data/icons/pixel-perfect-at-16px-volume-3-1/16/2049-512.png" });
            list.Add(new Cust() { Id = 1, Name = "김유신", BirthDay = new DateTime(801, 8, 24), ImageUrl = "https://cdn3.iconfinder.com/data/icons/pixel-perfect-at-16px-volume-3-1/16/2049-512.png" });
            list.Add(new Cust() { Id = 1, Name = "류관순", BirthDay = new DateTime(1902, 9, 19), ImageUrl = "https://cdn3.iconfinder.com/data/icons/pixel-perfect-at-16px-volume-3-1/16/2049-512.png" });

            dgMain.ItemsSource = list;  
        }
    }
}
