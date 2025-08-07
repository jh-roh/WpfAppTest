using DevExpress.Xpf.Editors.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfAppTestDevExpress.Views
{
    public class Person
    {
        public string Name { get; set; }
        public ObservableCollection<Tag> Tags { get; set; } = new();
    }

    public class Tag
    {
        public string Name { get; set; }
    }

    public class MainViewModel
    {
        public ObservableCollection<Person> People { get; set; }
        public ObservableCollection<Tag> AllTags { get; set; }

        public MainViewModel()
        {
            AllTags = new ObservableCollection<Tag> {
            new Tag { Name = "SW" },
            new Tag { Name = "QA" },
            new Tag { Name = "운영" }
        };

            People = new ObservableCollection<Person> {
            new Person { Name = "홍길동", Tags = new ObservableCollection<Tag> { AllTags[0] } },
            new Person { Name = "김철수", Tags = new ObservableCollection<Tag> { AllTags[1], AllTags[2] } }
        };
        }
    }

    /// <summary>
    /// TokenEditControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TokenEditControl : UserControl
    {
        public TokenEditControl()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
