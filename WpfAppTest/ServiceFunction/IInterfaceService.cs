using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.ServiceFunction
{
    [ServiceContract]
    public interface IInterfaceService
    {
        [OperationContract]
        string GetDataFromWebServiceJSON(string userToken, string xmlName, string parametersJson);

    }
}
