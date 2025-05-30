using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.ServiceFunction
{
    public class HttpTestFunction
    {

        public void GetDataFromWebServiceJSON(string userToken, string xmlName, string parametersJson)
        {
            //var url = "http://localhost/JVMService_R3/WebService/WebServiceXML.asmx/GetDataFromWebServiceXML" +
            //     "?userToken=SAEBC13c7281LCcJ03SemgZEjxouVSh5XSl6it/ZsMSgxTc=" +
            //     "&xmlName=GetAllMachineItem" +
            //     "&parametersJson=";

            var url = "http://localhost/JVMService_R3Develop/WebService/WebServiceXML.asmx/GetDataFromWebServiceXML" +
               "?userToken=SAEBC13c7281LCcJ03SemgZEjxouVSh5XSl6it/ZsMSgxTc=" +
               "&xmlName=GetAllMachineItem" +
               "&parametersJson=";


            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                string responseText = reader.ReadToEnd();
                                Console.WriteLine("서버 응답:");
                                Console.WriteLine(responseText);
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("요청 실패: " + ex.Message);

                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string errorText = reader.ReadToEnd();
                        Console.WriteLine("서버 에러 내용:");
                        Console.WriteLine(errorText);
                    }
                }
            }
        }


    }
}
