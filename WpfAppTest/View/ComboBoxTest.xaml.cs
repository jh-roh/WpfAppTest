using System;
using System.Collections;
using System.Collections.Generic;
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

namespace WpfAppTest.View
{
    /// <summary>
    /// ComboBoxTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ComboBoxTest : UserControl
    {
        public ComboBoxTest()
        {
            InitializeComponent();

            SetData();
        }

        private void SetData()
        {
            //cboCustGbn.ItemsSource = GetData();


            //foreach(var item in GetData())
            //{
            //    ComboBoxItem ci = new ComboBoxItem();

            //    ci.Content = item.strName;
            //    cboCustGbn.Items.Add(ci);

            //}
            //cboCustGbn.SelectedIndex = 0;


            cboSido.ItemsSource = GetSiDoData();

        }

        private IEnumerable GetSiDoData()
        {
            List<Sido> list = new List<Sido>();

            list.Add(new Sido() { strCode = "02", strName = "서울" });
            list.Add(new Sido() { strCode = "031", strName = "경기" });
            list.Add(new Sido() { strCode = "032", strName = "인천" });

            return list;
        }

        private List<CommonCode> GetData()
        {
            List<CommonCode> list = new List<CommonCode>();

            list.Add(new CommonCode() { strCode = "101", strName= "일반고객"});
            list.Add(new CommonCode() { strCode = "201", strName= "중요고객"});
            list.Add(new CommonCode() { strCode = "301", strName= "VIP" });
            list.Add(new CommonCode() { strCode = "901", strName = "휴면고객" });


            return list;
        }

        private void cboSido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboSido.SelectedItem != null)
            {
                Sido sido = ((Sido)cboSido.SelectedItem);
                cboGuGun.ItemsSource = GetGuGunData(sido.strCode);
                cboDong.ItemsSource = null;
            }
        }

        private IEnumerable GetGuGunData(string strCode)
        {
            List<GuGun> list = new List<GuGun>();

            if (strCode == "02")
            {
                list.Add(new GuGun() { strCode = "101", strName = "강남구" });
                list.Add(new GuGun() { strCode = "102", strName = "강북구" });
                list.Add(new GuGun() { strCode = "103", strName = "강서구" });
                list.Add(new GuGun() { strCode = "104", strName = "강동구" });
                list.Add(new GuGun() { strCode = "105", strName = "중구" });
            }
            else if(strCode == "031")
            {
                list.Add(new GuGun() { strCode = "201", strName = "안양시" });
                list.Add(new GuGun() { strCode = "202", strName = "부천시" });
                list.Add(new GuGun() { strCode = "203", strName = "구리시" });
                list.Add(new GuGun() { strCode = "204", strName = "성남시" });

            }

            return list;

        }
    }

    class CommonCode
    {
        public string strCode { get; set; }
        public string strName { get; set; }
    }

    class Sido
    {
        public string strCode { get; set; }
        public string strName { get; set; }
    }

    class GuGun
    {
        public string strCode { get; set; }
        public string strName { get; set; }
        public string strSiDoCode { get; set; }
    }

    class Dong
    {
        public string strSoDoCode { get; set; }
        public string strGuGunCode { get; set; }
        public string strCode { get; set; }
        public string strName { get; set; }
    }

}
