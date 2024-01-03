using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfAppTest.Model
{

    public class Custs : ObservableCollection<Cust> { }

    public class Cust :INotifyPropertyChanged
    {
        private string name;

        public String Name
        { 
            get
            {
                return this.name;   
            }
        
            set
            {
                this.name = value;
                Notify("Name");
            }
        
        }

        private string tel;


        public string Tel
        {
            get
            {
                return this.tel;
            }

            set
            {
                this.tel = value;
                Notify("Tel");

            }
        }

        private int intPoint;

        public int IntPoint
        {
            get { return intPoint; }
            set 
            { 
                intPoint = value; 
                Notify("IntPoint");

            }
        }

        private int age;
        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
                Notify("Age");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void Notify(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public Cust():this("성명", "전화번호",0)
        {

        }

        public Cust(string name, string tel, int intPoint)
        {
            this.Name = name;
            this.Tel = tel;
            this.IntPoint = intPoint;
        }

        public override string ToString()
        {
            return String.Format("성명 :{0}, 나이 :{1}",name,age);
        }
    }
}
