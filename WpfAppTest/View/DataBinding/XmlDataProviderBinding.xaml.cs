using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Xml;

namespace WpfAppTest.View.DataBinding
{
    /// <summary>
    /// XmlDataProviderBinding.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class XmlDataProviderBinding : UserControl
    {
        XmlDocument doc;
        public XmlDataProviderBinding()
        {
            InitializeComponent();

            LoadCustListXml();
        }

        private void LoadCustListXml()
        {
            doc = new XmlDocument();
            doc.Load("XmlData/CustList.xml");

            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("sb","http://test01.co.kr");
            Binding.SetXmlNamespaceManager(grdMain, ns);

            Binding b = new Binding();
            b.XPath = "sb:CustList/sb:Cust";
            b.Source = doc;
            grdMain.SetBinding(Grid.DataContextProperty, b);
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //DataSourceProvider provider = (DataSourceProvider)this.FindResource("custList");

            //ICollectionView view = CollectionViewSource.GetDefaultView(provider.Data);
            ICollectionView view = CollectionViewSource.GetDefaultView(grdMain.DataContext);

            XmlElement cust = (XmlElement)view.CurrentItem;


            //cust.SetAttribute("Age", "77");
            Debug.WriteLine(cust.Attributes["Name"].Value.ToString());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            XmlElement c = doc.CreateElement("Cust", "http://test01.co.kr");

            c.SetAttribute("Name", "을지문덕");
            c.SetAttribute("Age", "99");

            doc.DocumentElement.AppendChild(c);
        }
    }
}
