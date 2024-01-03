using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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

namespace WpfAppTest.View.DataBinding
{
    /// <summary>
    /// DefaultBinding.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BindingList : UserControl
    {
        public BindingList()
        {
            InitializeComponent();
        }

        private void btnYear_Click(object sender, RoutedEventArgs e)
        {
            CustList list =  (CustList)this.FindResource("custList");

            ICollectionView view = CollectionViewSource.GetDefaultView(list);

            Cust c =  (Cust)view.CurrentItem;

            Debug.WriteLine("성명: " + c.Name);
            Debug.WriteLine("나이: " + c.Age);
        }

        private void btnPre_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = GetList();

            view.MoveCurrentToPrevious();

            if (view.IsCurrentBeforeFirst)
            {
                view.MoveCurrentToFirst();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = GetList();

            view.MoveCurrentToNext();

            if(view.IsCurrentAfterLast)
            {
                view.MoveCurrentToLast();
            }
        }

        private ICollectionView GetList()
        {
            CustList list = (CustList)this.FindResource("custList");

            return CollectionViewSource.GetDefaultView(list);
        }

        private void lstCust_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
             Debug.WriteLine(String.Format("선택 고객 코드 : {0}", lstCust.SelectedValue.ToString())) ;
        }
    }


}
