using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WpfAppTest.TestClass
{
    public class TestHttpRequestMethod
    {

        public class HttpResponseResult
        {
            public HttpStatusCode StatusCode { get; set; }
            public string ResponseResult { get; set; }
        }

        public TestHttpRequestMethod() { }

        private static String HOST = "127.0.0.1";     //host 설정
        private static int PORT = 9196;               //port 설정
        private static int SSL_PORT = 8243;           //ssl-port 설정
        private static String CREDENTIALS = "";       //개발버전 인증키 설정
        private static String TOKEN = "";             //InMemory Access Token
        private static bool isSSL = false;            //ssl 통신 여부
        private static int RETRY_CNT = 2;             //인증 실패시 재시도 횟수

        public enum InterfaceID
        {
            getTestInfoList,
            setTestInfoList,
            token,
        }

        public static String GetBasePath(InterfaceID id)
        {
            string basePath = "";

            switch(id)
            {
                case InterfaceID.getTestInfoList:
                    basePath = "/api/v1/test/medicine/getTestInfoList";
                    break;

                case InterfaceID.setTestInfoList:
                    basePath = "/api/v1/test/medicine/setTestInfoList";
                    break;

                case InterfaceID.token:
                    basePath = "/token";
                    break;
            }


            return basePath;

        }

        //public static void GenerateToken()
        //{
        //    string basePath = GetBasePath(InterfaceID.token);
        //    var apiUrl = CreateURIBuilder(basePath, null);

        //    StringBuilder authorization = new StringBuilder();
        //    authorization.Append("Basic ");
        //    authorization.Append(CREDENTIALS);
        //    // Create a WebRequest
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);

        //    // Set the method to GET
        //    request.Method = "POST";
        //    // Set the Accept header to indicate JSON response
        //    request.Accept = "application/json";
        //    request.ContentType = "application/json;charset=UTF-8";

        //    // Set the Authorization header if needed (replace "your-token" with your actual token)
        //    request.Headers["Authorization"] = authorization.ToString();

        //    try
        //    {
        //        // Get the response
        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        using (Stream responseStream = response.GetResponseStream())
        //        using (StreamReader reader = new StreamReader(responseStream))
        //        {
        //            if (response.StatusCode == HttpStatusCode.OK)
        //            {
        //                string responseBody = reader.ReadToEnd();

        //                var dynamicObject = JsonConvert.DeserializeObject<dynamic>(responseBody);

        //                TOKEN = dynamicObject.access_token;

        //                Console.WriteLine("Response: " + responseBody);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Error: " + response.StatusCode);
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //    }
        //}

        public static Uri CreateURIBuilder(string basePath, NameValueCollection nvc)
        {
            string scheme = isSSL ? "https" : "http";
            int port = isSSL ? SSL_PORT : PORT;


            UriBuilder builder = new UriBuilder();
            builder.Scheme = scheme;
            builder.Host = HOST;
            builder.Port = port;
            builder.Path = basePath;

            if(nvc != null && nvc.Count > 0)
                builder.Query = ToQueryString(nvc);

            return builder.Uri;
        }

        public static string ToQueryString(NameValueCollection nvc)
        {
            var array = (
                from key in nvc.AllKeys
                from value in nvc.GetValues(key)
                select $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}"
            ).ToArray();
            return "?" + string.Join("&", array);
        }

        public static void GenerateToken()
        {
            var tokenResult = CallEHRApi(false, InterfaceID.token, null);

            if (tokenResult != null
            && tokenResult.StatusCode == HttpStatusCode.OK)
            {
                var dynamicObject = JsonConvert.DeserializeObject<dynamic>(tokenResult.ResponseResult);

                TOKEN = dynamicObject.access_token;
            }
        }

        public static HttpResponseResult CallEHRApi(bool isGetMode, InterfaceID id, NameValueCollection nvc)
        {
            HttpResponseResult returnResult = new HttpResponseResult();

            HttpWebResponse response = null;


            string basePath = GetBasePath(id);
            var apiUrl = CreateURIBuilder(basePath, nvc);

            StringBuilder authorization = new StringBuilder();
            if(id == InterfaceID.token)
            {
                authorization.Append("Basic ");
                authorization.Append(CREDENTIALS);
            }
            else
            {
                authorization.Append("Bearer ");
                authorization.Append(TOKEN);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);

            if(isGetMode)
            {
                request.Method = "GET";
            }
            else
            {
                request.Method = "POST";
            }

            // Set the Accept header to indicate JSON response
            request.Accept = "application/json";
            request.ContentType = "application/json;charset=UTF-8";
            // Set the Authorization header if needed (replace "your-token" with your actual token)
            request.Headers["Authorization"] = authorization.ToString();

            try
            {
                // Get the response
                using (response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string responseBody = reader.ReadToEnd();

                        returnResult.ResponseResult = responseBody; ;

                        Console.WriteLine("Response: " + responseBody);
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }

                    returnResult.StatusCode = response.StatusCode;
                }
            }
            catch (WebException ex)
            {
                if(ex.Response != null)
                {
                    returnResult.StatusCode = ((HttpWebResponse)ex.Response).StatusCode;
                }

                Console.WriteLine("Error: " + ex.Message);
            }

            return returnResult;

        }

        public static void SendHttpRequestMethod()
        {
            int tryCnt = RETRY_CNT;
            // Construct URL parameters
            var queryUpdateParams = new NameValueCollection
            {
                { "test1", "127.0.0.1" },
                { "test2", "TestSerial_1" },
                { "test3", "AA" },
                { "test4", "ATIVA4" },
                { "test5", "0159132920230914E00020000002595" }
            };


            var responseResult = CallEHRApi(true, InterfaceID.setTestInfoList, queryUpdateParams);
            
            if (responseResult.StatusCode == HttpStatusCode.Unauthorized)
            {
                GenerateToken();
                responseResult = CallEHRApi(true, InterfaceID.setTestInfoList, queryUpdateParams);
            }
        }


        //public void SendHttpRequestMethod()
        //{
        //    
        //    // Create a WebRequest
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl );


        //    // Set the method to GET
        //    request.Method = "GET";

        //    // Set the Accept header to indicate JSON response
        //    request.Accept = "application/json";

        //    // Set the Authorization header if needed (replace "your-token" with your actual token)
        //    request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJlaHJ1c2VyIiwiaW";


        //    // Construct URL parameters
        //    var queryParams = new NameValueCollection
        //    {
        //        { "atcEqpmCd", "01" },
        //        { "cnctIp", "127.0.0.1" },
        //        { "injcPlacCd", "ER" }

        //    };

        //    //string fullUrl = baseUrl + endpoint + ToQueryString(queryParams);

        //    try
        //    {
        //        // Get the response
        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        using (Stream responseStream = response.GetResponseStream())
        //        using (StreamReader reader = new StreamReader(responseStream))
        //        {
        //            if (response.StatusCode == HttpStatusCode.OK)
        //            {
        //                string responseBody = reader.ReadToEnd();
        //                Console.WriteLine("Response: " + responseBody);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Error: " + response.StatusCode);
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //    }
        //}



    }
}
