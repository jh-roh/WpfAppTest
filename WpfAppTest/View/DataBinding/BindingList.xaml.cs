using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
            CustList list = (CustList)this.FindResource("custList");

            ICollectionView view = CollectionViewSource.GetDefaultView(list);

            Cust c = (Cust)view.CurrentItem;

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

            if (view.IsCurrentAfterLast)
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
            Debug.WriteLine(String.Format("선택 고객 코드 : {0}", lstCust.SelectedValue.ToString()));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //틀린예...
            //MessageBox.Show(((Cust)lstCust.SelectedItem).Name + "을 삭제합니다.", "알림");

            Cust c = ((Button)sender).DataContext as Cust;

            MessageBox.Show(c.Name + "을 삭제합니다.", "알림");

        }

        private void btnAddCust_Click(object sender, RoutedEventArgs e)
        {
            DataSourceProvider provider = (DataSourceProvider)this.FindResource("custList)");
            CustList list = provider.Data as CustList;
            list.Add(new Cust { Name="세종대왕", Age=40 });
        }


        private void btnAddCust_Click_old(object sender, RoutedEventArgs e)
        {
            CustList list = (CustList)this.FindResource("custList");
            list.Add(new Cust("세종대왕", "30", 15));

        }

        private void btnSortName_Click(object sender, RoutedEventArgs e)
        {
            ListCollectionView list = (ListCollectionView)GetList();

            if (list.CustomSort == null)
            {

                list.CustomSort = new CustSorter();
            }
            else
            {
                list.CustomSort = null;
            }

            //if (list.SortDescriptions.Count == 0)
            //{
            //    list.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            //    list.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Descending));
            //}
            //else
            //{
            //    list.SortDescriptions.Clear();
            //}
        }

        private void btnSortAge_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView list = GetList();

            if (list.SortDescriptions.Count == 0)
            {
                list.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Ascending));
                list.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
            }
            else
            {
                list.SortDescriptions.Clear();
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView list = GetList();

            if(list.Filter == null)
            {
                list.Filter = delegate (object item)
                {
                    return ((Cust)item).Age >= 20;
                };
            }
            else
            {
                list.Filter = null;
            }
        }

        private void btnGroupName_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView list = GetList();

            if(list.GroupDescriptions.Count == 0)
            {
                list.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
            }
            else
            {
                list.GroupDescriptions.Clear();
            }
        }

        private void btnAgeRangeGroup_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView list = GetList();
            if (list.GroupDescriptions.Count == 0)
            {
                list.GroupDescriptions.Add(
                    new PropertyGroupDescription("Age",new AgeToRangeConverter())
                );

                //다층 그룹용
                list.GroupDescriptions.Add(
                   new PropertyGroupDescription("Name")
               );
            }
            else
            {
                list.GroupDescriptions.Clear();
            }

        }

    }


    class CustSorter : IComparer
    {
        public int Compare(object obj1, object obj2)
        {
            //- : X
            //0 : 같다
            //+ : 바꾼다

            Cust c1 = ((Cust)obj1);
            Cust c2 = ((Cust)obj2);

            int r = c1.Name.CompareTo(c2.Name);

            Debug.WriteLine("c1: " + c1.Name);
            Debug.WriteLine("c2: " + c2.Name);
            Debug.WriteLine("r: " + r);

            return r;
        }
    }

    class AgeToRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) >= 20 ? "성년" : "미성년";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    class RemortCustListLoader
    {
        public CustList loadCustList()
        {
            CustList list = new CustList();


            list.Add(new Cust { Name = "홍길동1", Age = 17 });
            list.Add(new Cust { Name = "홍길동1", Age = 18 });
            list.Add(new Cust { Name = "홍길동1", Age = 37 });
            list.Add(new Cust { Name = "홍길동2", Age = 47 });
            list.Add(new Cust { Name = "홍길동3", Age = 57 });
            list.Add(new Cust { Name = "홍길동3", Age = 67 });
            list.Add(new Cust { Name = "홍길동4", Age = 77 });

            return list;
        }

        public CustList loadCustList(string min, string max)
        {
            CustList list = new CustList();

            Debug.WriteLine("min:" + min);
            Debug.WriteLine("max:" + max);

            list.Add(new Cust { Name = "홍길동1", Age = 37 });
            list.Add(new Cust { Name = "홍길동2", Age = 47 });

            return list;
        }

    }




}
