using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTestDevExpress.TestSource
{
    public class TestDataSet
    {

        public void CreateDataSet()
        {
            DataSet ds = new DataSet("School");

            ds.Tables.Add("TEAHER");
            ds.Tables.Add("STUDENT");

        }
    }
}
