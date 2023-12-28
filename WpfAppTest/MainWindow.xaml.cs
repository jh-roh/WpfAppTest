using DevExpress.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfAppTest.Model;
using WpfAppTest.Model.Oracle;
using WpfAppTest.TestClass;
using WpfAppTest.TestJSon;
using WpfAppTest.ViewModel;

namespace WpfAppTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            

            #region 각종 테스트 관련 함수
            //var dataTemplateVM = new DataTemplateModel();

            //ContentControl_Test.Content = dataTemplateVM;

            //MainClass.MainTest();
            //TestJSonClass testJson = new TestJSonClass();

            //ProcessStartInfoTest test = new ProcessStartInfoTest();

            //test.ProcessTest();

            //linqAnyMethodTest();

            //TestTrim test = new TestTrim();

            //test.TestMethod();
            //test.ToCharMethod();

            //new TestExcuteAtTIme().TestExcuteAtTimeMethod();
            #endregion

            #region 오라클 관련 웹서비스 XML 테스트 함수
            var tnsName = "(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(ADDRESS = (PROTOCOL = TCP)(HOST = 10.7.5.24)(PORT = 1577)))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = ORCL)))";
            var id = "intipharm";
            var pw = "jv2511";

            JObject paraInfo = JObject.FromObject(new
            {
                tnsName = tnsName,
                id = id,
                pw = pw,
                //queryString = @"BEGIN SET_MEDICINE_DATA('IMIDA5', 9, :ErrorMsg, :ErrorYN); END;",

                //outParamater_VARCHAR2_Value1 = "ErrorMsg",
                //outParamater_VARCHAR2_Value2 = "ErrorYN",

                //queryString = @"UPDATE ADBVDRUG SET EXCHQTY = 5 WHERE DRUGCD = 'IMIDA5'",

                //queryString = "BEGIN GET_MEDICINE_DATA('1', :p_cursor); END;",
                //outParamater_RefCursor_Value1 = "p_cursor",

                //queryString = @"SELECT A.CABINETSEQ,  A.SEQNO, A.INSTCD, B.PRCPDD,B.PRNTDT, B.ORDDRID,B.RGSTRID,  to_char(A.FSTRGSTDT, 'yyyy-mm-dd hh24:mi:SSxFF') AS JVMTIME, A.PRCPFLAG, B.DRUGBARCD,
                //B.MEMO, B.PID, B.HNGNM, B.AGE, B.SEX, B.WARDNM, B.ROOMNM, B.ORDDEPTNM, B.DRUGIOFLAG, A.DRUGNO,A.DRUGCD, A.MTHDCD, A.MTHDNM,A.TOTDRUGQTY, A.PRCPVOL,A.PRCPVOLUNIT, A.DAYPRCPQTY, A.PRCPQTY, A.PRCPQTYUNIT, A.PRCPTIMS, A.PRCPDAYNO
                //FROM ADTDDRUGCABINET A INNER JOIN ADTMDRUGCABINET B ON A.INSTCD = B.INSTCD AND A.CABINETSEQ = B.CABINETSEQ
                //WHERE A.INSTCD = '126'"
                //queryString = "select * from ADTMDRUGCABINET"
                //queryString = "select * from ADBVDRUG"

            });



            //new DataBaseOracleRepository().GetExecuteSelectQuery(paraInfo.ToString());
            //new DataBaseOracleRepository().GetExecuteProcedure(paraInfo.ToString());
            //new DataBaseOracleRepository().SetExecuteUpdateQuery(paraInfo.ToString());
            //new DataBaseOracleRepository().SetExecuteProcedure(paraInfo.ToString());

            #endregion
        }

        private void button_DoorAging_Click(object sender, RoutedEventArgs e)
        {
            //popup_DoorAging.IsOpen = !popup_DoorAging.IsOpen;  
        }


        private void linqAnyMethodTest()
        {
            // 비교할 두 개의 리스트 생성
            List<int> list1 = new List<int> { 1, 2, 3, 4, 5 };
            List<int> list2 = new List<int> { 1, 2, 3, 9, 10 };

            // Any 메서드를 사용하여 두 리스트에서 공통된 요소가 있는지 확인
            bool hasCommonElements = list1.Any(element => list2.Contains(element));

            if (hasCommonElements)
            {
                Console.WriteLine("두 리스트에는 공통된 요소가 있습니다.");
            }
            else
            {
                Console.WriteLine("두 리스트에는 공통된 요소가 없습니다.");
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    TestHttpRequestMethod.SendHttpRequestMethod();

        //}
    }
}
