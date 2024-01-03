using DevExpress.Mvvm.UI.ModuleInjection;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfAppTest.Model;

namespace WpfAppTest.View.DataBinding
{
    /// <summary>
    /// PreviousBinding.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PreviousBinding : UserControl
    {
        private Cust c1;
        public PreviousBinding()
        {
            InitializeComponent();

            c1 = getData();
            ViewData();

            c1.PropertyChanged += C1_PropertyChanged;
        }

        private void C1_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "Name":
                    txtName.Text = (sender as Cust).Name;
                    break;

                case "Age":
                    txtAge.Text = (sender as Cust).Age.ToString();

                    break;

                case "Tel":
                    break;

                case "intPoint":
                    break;
            }


        }

        private Cust getData()
        {
            Cust c = new Cust() { Age = 17, Name = "홍길동", Tel = "010-1111-1111", IntPoint = 10 };

            return c;
        }

        private void ViewData()
        {
            txtName.Text = c1.Name;
            txtAge.Text = c1.Age.ToString();
        }

        private void btnYear_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            int year = dt.Year - Convert.ToInt32(txtAge.Text) + 1;

            MessageBox.Show("당신의 출생년도는 " + year + "입니다");


        }

        private void btnAddAge_Click(object sender, RoutedEventArgs e)
        {
            c1.Age++;

            //ViewData();
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine("82-Name Changed:" + txtName.Text);
            c1.Name = txtName.Text;
        }

        private void txtAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine("87-Age Changed:" + txtAge.Text);
            
            try
            {
                c1.Age = Convert.ToInt32(txtAge.Text); 
            }
            catch (Exception ex)
            {

            }
        }
    }
}
