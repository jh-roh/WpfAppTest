using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// TreeViewWithDriveInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TreeViewWithDriveInfo : UserControl
    {
        public TreeViewWithDriveInfo()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            DriveInfo[] drvies = DriveInfo.GetDrives();

            foreach (DriveInfo drv in drvies)
            {
                Debug.WriteLine(drv.Name);
                trvDrive.Items.Add(CreateTreeViewItem(drv));
            }
        }

        private TreeViewItem CreateTreeViewItem(object o)
        {
            TreeViewItem item = new TreeViewItem();

            item.Header = o.ToString();
            item.Tag = o;
            item.Items.Add("Loading...");

            return item;

        }

        private void trvDrive_Expanded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("trvDrive_Expanded");
            Debug.WriteLine(sender);

            TreeViewItem item = e.Source as TreeViewItem;

            if(item.Items.Count == 1 && item.Items[0] is string)
            {
                item.Items.Clear();

                DirectoryInfo expandedDir = null;

                if(item.Tag is DriveInfo)
                {
                    expandedDir = (item.Tag as DriveInfo).RootDirectory;
                }
                else if (item.Tag is DirectoryInfo)
                {
                    expandedDir = item.Tag as DirectoryInfo;
                }

                try
                {
                    foreach (DirectoryInfo subDir in expandedDir.GetDirectories())
                    {
                        item.Items.Add(CreateTreeViewItem(subDir));
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
