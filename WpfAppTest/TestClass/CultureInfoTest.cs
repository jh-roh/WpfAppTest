using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.TestClass
{
    public class CultureInfoTest
    {

        public CultureInfoTest()
        {
            CultureInfo culture = new CultureInfo("ko-KR");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
