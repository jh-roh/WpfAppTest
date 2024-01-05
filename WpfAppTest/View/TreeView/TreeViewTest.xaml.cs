using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WpfAppTest.View.TreeView
{
    /// <summary>
    /// TreeViewTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TreeViewTest : UserControl
    {
        public TreeViewTest()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {

            #region Treeview 사용 예제1
            //MyMenuITem root1 = new MyMenuITem() { Title = "총무부" };

            //root1.Items.Add(new MyMenuITem() { Title = "홍길동" });
            //root1.Items.Add(new MyMenuITem() { Title = "이순신" });

            //MyMenuITem p = new MyMenuITem() { Title = "김유신" };

            //p.Items.Add(new MyMenuITem { Title = "학력" });
            //p.Items.Add(new MyMenuITem { Title = "경력" });
            //p.Items.Add(new MyMenuITem { Title = "자격증" });


            //root1.Items.Add(p);

            //MyMenuITem root2 = new MyMenuITem() { Title = "영업부" };
            //root2.Items.Add(new MyMenuITem() { Title = "홍길동2" });
            //root2.Items.Add(new MyMenuITem() { Title = "이순신2" });
            //root2.Items.Add(new MyMenuITem() { Title = "김유신2" });




            //MyMenuITem root3 = new MyMenuITem() { Title = "개발부" };
            //root3.Items.Add(new MyMenuITem() { Title = "홍길동3" });
            //root3.Items.Add(new MyMenuITem() { Title = "이순신3" });
            //root3.Items.Add(new MyMenuITem() { Title = "김유신3" });


            //trvMenu.Items.Add(root1);
            //trvMenu.Items.Add(root2);
            //trvMenu.Items.Add(root3);
            #endregion

            #region Treeview 사용예제 2
            //List<Corp> corps = new List<Corp>();
            //Corp c1 = new Corp() { Name = "삼성" };
            //c1.Members.Add(new Emp() { Name = "홍길동", Age = 17 });
            //c1.Members.Add(new Emp() { Name = "김유신", Age = 32 });
            //c1.Members.Add(new Emp() { Name = "이순신", Age = 22 });
            //Corp c2 = new Corp() { Name = "LG" };
            //c2.Members.Add(new Emp() { Name = "홍길동2", Age = 172 });
            //c2.Members.Add(new Emp() { Name = "김유신2", Age = 322 });
            //c2.Members.Add(new Emp() { Name = "이순신2", Age = 222 });
            //Corp c3 = new Corp() { Name = "현대" };
            //c3.Members.Add(new Emp() { Name = "홍길동3", Age = 173 });
            //c3.Members.Add(new Emp() { Name = "김유신3", Age = 323 });
            //c3.Members.Add(new Emp() { Name = "이순신3", Age = 223 });
            //corps.Add(c1);
            //corps.Add(c2);
            //corps.Add(c3);

            //trvMain1.ItemsSource = corps;

            #endregion

            List<MyMenu> list = new List<MyMenu>();

            MyMenu mnu01 = new MyMenu() { Title = "파일", Code = 100 };
            MyMenu mnu02 = new MyMenu() { Title = "편집", Code = 200 };
            MyMenu mnu03 = new MyMenu() { Title = "보기", Code = 300 };

            MyMenu sub0101 = new MyMenu() { Title = "새로만들기", Code = 101 };
            MyMenu sub0102 = new MyMenu() { Title = "저장", Code = 102 };
            MyMenu sub0103 = new MyMenu() { Title = "닫기", Code = 103 };

            MyMenu sub0201 = new MyMenu() { Title = "복사", Code = 201 };
            MyMenu sub0202 = new MyMenu() { Title = "붙여넣기", Code = 202 };

            MyMenu sub0301 = new MyMenu() { Title = "코드", Code = 201 };
            MyMenu sub0302 = new MyMenu() { Title = "디자이너", Code = 202 };


            mnu01.Children.Add(sub0101);
            mnu01.Children.Add(sub0102);
            mnu01.Children.Add(sub0103);

            mnu02.Children.Add(sub0201);
            mnu02.Children.Add(sub0202);


            mnu03.Children.Add(sub0301);
            mnu03.Children.Add(sub0302);

            list.Add(mnu01);
            list.Add(mnu02);
            list.Add(mnu03);

            mnu01.IsExpanded = true;
            mnu02.IsExpanded = true;
            mnu03.IsExpanded = true;

            mnu02.IsSelected = true;
            trvMain2.ItemsSource = list;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (trvMain2.SelectedItem != null)
            {
                MyMenu m = trvMain2.SelectedItem as MyMenu;

                List<MyMenu> list = trvMain2.ItemsSource as List<MyMenu>;

                int index = list.IndexOf(m);

                if (index >= 0) index++;

                if (index >= list.Count)
                    index = 0;
                list[index].IsSelected = true;

                trvMain2.Focus();
            }
        }

        private void btnExpansion_Click(object sender, RoutedEventArgs e)
        {
            if (trvMain2.SelectedItem != null)
            {
                ((MyMenu)trvMain2.SelectedItem).IsExpanded = !((MyMenu)trvMain2.SelectedItem).IsExpanded;
            }

        }
    }

    class MyMenu : TreeViewItemBase
    {
        public string Title { get; set; }
        public int Code { get; set; }

        public ObservableCollection<MyMenu> Children { get; set; }

        public MyMenu()
        {
            this.Children = new ObservableCollection<MyMenu>();
        }
    }

    class TreeViewItemBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void Notify(string propName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        private bool isSelected;

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected == value) return;

            
                isSelected = value;
                Notify("IsSelected");
            }
        
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }
            set
            {
                if(this.isExpanded == value) return;
                this.isExpanded = value;
                Notify("IsExpanded");
            }
        }
    }

    class Emp
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }


    class Corp
    {
        public string Name { get; set; }
        public ObservableCollection<Emp> Members { get; set; }

        public Corp()
        {
            this.Members = new ObservableCollection<Emp>();
        }
    }

    class MyMenuITem 
    {
        public string Title { get; set; }
        public ObservableCollection<MyMenuITem> Items { get; set; }

        public MyMenuITem()
        {
            this.Items = new ObservableCollection<MyMenuITem>();
        }
    }
}
