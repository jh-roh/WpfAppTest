using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.ViewModel
{
    [POCOViewModel]
    public class StockingScreen
    {
        public virtual int SeqNo { get; set; }

        public virtual string HospitalMedicineCode { get; set; }

        public virtual string ItemName { get; set; }

        public virtual DateTime ExpiredDate { get; set; }

        public virtual String LotNo { get; set; }

        public virtual String SerialNo { get; set; }

        public virtual string BoxUnit { get; set; }


        public virtual int RefillQty { get; set; }

        public virtual int CompleteQty { get; set; } 

        protected StockingScreen() 
        {
        }

    }

    [POCOViewModel]
    public class StockingScreenTestModel
    {
        public static StockingScreenTestModel Create()
        {
            return ViewModelSource.Create(() => new StockingScreenTestModel());
        }
        protected StockingScreenTestModel() { }

        public virtual string UserName { get; set; }

        //public void Login()
        //{
        //    this.GetService<IMessageBoxService>().Show("Login succeeded", "Login", MessageButton.OK, MessageIcon.Information, MessageResult.OK);
        //}
        //public bool CanLogin()
        //{
        //    return !string.IsNullOrEmpty(UserName);
        //}
    }
}
