using DevExpress.Xpf.Grid;
using System;
using System.Collections;
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
using WpfAppTest.Model;

namespace WpfAppTest.View
{
    /// <summary>
    /// ListBoxTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ListBoxTest : UserControl
    {
        public ListBoxTest()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            lstCust.ItemsSource = GetCustData();
        }

        private List<Cust> GetCustData()
        {
            List<Cust> list = new List<Cust>();
            list.Add(new Cust( "홍길동", "010-1111-1111", 50));
            list.Add(new Cust("김유신", "010-2222-2222", 20));
            list.Add(new Cust("강감찬", "010-3333-3333", 30));
            list.Add(new Cust("이순신", "010-4444-4444", 40));

            return list;
        }

        private void btnSelectCut_Click(object sender, RoutedEventArgs e)
        {
            if(lstCust.SelectedItem != null)
            {
                Cust cust = ((Cust)lstCust.SelectedItem);
                MessageBox.Show(cust.Name + "님의 전화번호는 " + cust.Tel + "입니다.");
            }
            else
            {
                MessageBox.Show("고객을 먼저 선택해 주세요.", "알림");
            }
        }

        private void lstCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstCust.SelectedItem != null )
            {
                Cust cust = ((Cust)lstCust.SelectedItem);

                lbCustomer.Content = cust.Name;
            }

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (lstCust.SelectedItem != null)
            {
                Cust cust = ((Cust)lstCust.SelectedItem);

                int index = lstCust.SelectedIndex + 1;

                if(index >= lstCust.Items.Count)
                {
                    index = 0;
                }

                lstCust.SelectedIndex = index;
            }
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            lstCust.SelectAll();
        }

        private void btnViewAll_Click(object sender, RoutedEventArgs e)
        {
            foreach(Cust item in lstCust.SelectedItems)
            {
                Debug.WriteLine(item.Name);
            }
        }
    }


}
