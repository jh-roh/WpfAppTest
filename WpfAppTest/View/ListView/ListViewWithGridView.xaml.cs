using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using WpfAppTest.Model;

namespace WpfAppTest.View.ListView
{
    /// <summary>
    /// ListViewWithGridView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ListViewWithGridView : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;


        public ListViewWithGridView()
        {
            InitializeComponent();
            LoadData();
        }

        List<Cust> list;
        private void LoadData()
        {
            list = new List<Cust>();
            list.Add(new Cust { Name = "홍길동", Age = 17, Email = "aaa@naver.com" ,Sex = "M"});
            list.Add(new Cust { Name = "홍길동", Age = 18, Email = "aaa@naver.com" ,Sex = "M"});
            list.Add(new Cust { Name = "이순신", Age = 58, Email = "bbb@naver.com" ,Sex = "M"});
            list.Add(new Cust { Name = "유관순", Age = 23, Email = "ccc@naver.com" ,Sex = "F"});
            list.Add(new Cust { Name = "이순신", Age = 31, Email = "ccc@naver.com" ,Sex = "F"});

            lvCust.ItemsSource = list;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvCust.ItemsSource);

            #region GroupDescription
            //PropertyGroupDescription gd = new PropertyGroupDescription("Name");
            //PropertyGroupDescription gd = new PropertyGroupDescription("Sex");

            //view.GroupDescriptions.Add(gd);

            //view.GroupDescriptions.Count == 0
            //view.GroupDescriptions.Clear();
            #endregion

            #region SortDescription
            //view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            //view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Age", System.ComponentModel.ListSortDirection.Descending));
            #endregion
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            list.Add(new Cust() { Name = "을지문덕", Age = 45, Email = "ddd@naver.com" , Sex = "M"});
            lvCust.ItemsSource = null;
            lvCust.ItemsSource = list;

        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader col = (GridViewColumnHeader)sender;

            string sortBy = col.Tag.ToString();

            if(listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvCust.Items.SortDescriptions.Clear();

            }

            ListSortDirection newDir = ListSortDirection.Ascending;

            if(listViewSortCol == col && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = col;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);

            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvCust.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));



            //switch(col.Tag)
            //{
            //    case "Name":
            //        lvCust.Items.SortDescriptions.Clear();
            //        lvCust.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));


            //        break;

            //    case "Age":
            //        lvCust.Items.SortDescriptions.Clear();
            //        lvCust.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Age", System.ComponentModel.ListSortDirection.Ascending));

            //        break;

            //    case "Email":
            //        lvCust.Items.SortDescriptions.Clear();
            //        lvCust.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Email", System.ComponentModel.ListSortDirection.Ascending));

            //        break;
            //}

            //MessageBox.Show(col.Tag.ToString());
            
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvCust.ItemsSource);

            if(view.Filter == null)
            {
                view.Filter = UserFilter;

            }
            else
            {
                view.Filter = null;
            }
        }

        private bool UserFilter(object item)
        {
            Cust c = (Cust)item;

            return (c.Age >= 20) ? true : false;
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvCust.ItemsSource);
            view.Filter = UserFilterByText;
        }

        private bool UserFilterByText(object item)
        {
            if(string.IsNullOrEmpty(txtFilter.Text))
            {
                return true;
            }
            else
            {
                return ((Cust)item).Name.IndexOf(txtFilter.Text) >= 0;
            }
        }
    }

    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
            Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
            Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir) : base(element)
        {
            this.Direction = dir;

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform(

                AdornedElement.RenderSize.Width - 15,
                (AdornedElement.RenderSize.Height - 5) / 2);

            drawingContext.PushTransform(transform);
            Geometry geometry = ascGeometry;

            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }

    }
}
