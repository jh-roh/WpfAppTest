﻿using DevExpress.XtraPrinting.Native.WebClientUIControl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfAppTest.TestJSon
{
    internal class TestJSonClass
    {
        public string SquidGame => @"
        {
            ""Name"": ""Squid Game"",
            ""Genre"": ""Thriller"",
            ""Rating"": {
                ""Imdb"": 8.1,
                ""Rotten Tomatoes"": 0.94
            },
            ""Year"": 2021,
            ""Stars"": [""Lee Jung-jae"", ""Park Hae-soo""],
            ""Language"": ""Korean"",
            ""Budget"": ""$21.4 million""
        }";
        public string SquidGame1 => @"
        [
                {
                ""Name"": ""Squid Game"",
                ""Genre"": ""Thriller"",
                ""Year"": 2021,
                ""Stars"": [""Lee Jung-jae"", ""Park Hae-soo""],
                ""Language"": ""Korean"",
                ""Budget"": ""$21.4 million""
                },
                {
                ""Name"": ""Squid Game1"",
                ""Genre"": ""Thriller"",
                ""Year"": 2021,
                ""Stars"": [""Lee Jung-jae"", ""Park Hae-soo""],
                ""Language"": ""Korean"",
                ""Budget"": ""$21.4 million""
                },
        ]";


        public TestJSonClass()
        {
            JSonParameterSample();
            JSonDynamicSample();
            JSonAnonymousTypeSample();



            //WebServiceXML webServiceXML = new WebServiceXML();
            //JObject paraInfo = new JObject(
            //	new JProperty("OCSPath",@"D:\JVM\OCSReading\Standard"),
            //	new JProperty("OCSFileName", String.Format("{0}_{1}_{2}.ocs",DateTime.Now.ToString("yyyyMMddHHmmss"),firstInfo.CHART_NO, firstInfo.GROUP_NO)),
            //	new JProperty("OCSFIleContent",ocsData.ToString())
            //);
            //String paraInfoString = paraInfo.ToString();
        }

        private void JSonParameterSample()
        {
            JObject paraInfo = new JObject(
                new JProperty("HospitalMedicineCode", ""),
                new JProperty("IsAll", true));

            Console.WriteLine(paraInfo.ToString());

        }

        private void JSonDynamicSample()
        {
            DateTime nowDate = DateTime.Now;
            string myDate = nowDate.ToString("yyyyMMdd");

            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(SquidGame1);

            foreach (var item in dynamicObject)
            {
                var testData = item["Name"];
            }
        }
        private void JSonAnonymousTypeSample()
        {
          //  var medicineInfoList = new[]
          //{
          //      new
          //      {
          //          ObjectType = 1,
          //          ItemName = "(향)5mg/5ml부광미다졸람주사",
          //          HospitalMedicineCode = "IMIDA5",
          //          HospitalName = "(향)(검정색)5mg/5mL Midazolam(낙)",
          //          CommercialName = "부광약품(주)",
          //          GenericName = "",
          //          mnemonic= "",
          //          KDCode = "",
          //          SpellCode = "",
          //          HospitalEngName = "(향)5mg/5mL Midazolam(Midazolam)",
          //          Rank =  3,
          //          MedicationType =  3,
          //          Measure = "ampoule",
          //      },
          //      new
          //      {
          //          ObjectType = 1,
          //          ItemName = "(마) 1mg 레미바 주 (하나제약)",
          //          HospitalMedicineCode = "IREMIVA1",
          //          HospitalName = "(마)1mg Remiva= Ultiva",
          //          CommercialName = "하나제약(주)",
          //          GenericName = "",
          //          mnemonic= "",
          //          KDCode = "",
          //          SpellCode = "",
          //          HospitalEngName = "(마)Remiva Inj 1mg(Remifentanil HCl)",
          //          Rank =  6,
          //          MedicationType =  3,
          //          Measure = "vial",
          //      },
          //}.ToList();

            var medicineInfoList = new[]
            {
                new
                {
                    ObjectType = 1,
                    ItemName = "(향)5mg/5ml부광미다졸람주사",
                    HospitalMedicineCode = "IMIDA5",
                    HospitalName = "(향)(검정색)5mg/5mL Midazolam(낙)",
                    CommercialName = "부광약품(주)",
                    GenericName = "",
                    mnemonic= "",
                    KDCode = "",
                    SpellCode = "",
                    HospitalEngName = "(향)5mg/5mL Midazolam(Midazolam)",
                    Rank =  3,
                    MedicationType =  3,
                    Measure = "ampoule",
                },
            }.ToList();

            var anonType = new { Name = "", Genre = "" };
            var anonTypeList = new[] { anonType }.ToList();

            var jsonResponse = JsonConvert.DeserializeAnonymousType(SquidGame1, anonTypeList);


            JObject o = JObject.FromObject(new
            {
                InsertItemList = medicineInfoList.ToList()
            });


            var testJson = o.ToString();

        }
    }
}