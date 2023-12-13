using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfAppTest.TestClass
{
    internal class TestExcuteAtTIme
    {

        public TestExcuteAtTIme() { }   

        public void TestExcuteAtTimeMethod()
        {
            int lastRefreshDay = 0;
            bool functionCalled = false;

            while (true)
            {
                // 현재 시간을 확인
                DateTime now = DateTime.Now;

                //최초 실행
                //CallSpecificFunction();

                // 매일 오후 4시 30분인지 확인
                if (!functionCalled && now.Hour == 11 && now.Minute >= 30)
                {
                    // 여기에 매일 오후 4시 30분에 한 번 호출하고자 하는 함수를 호출하는 코드를 작성
                    //CallSpecificFunction();
                    functionCalled = true;
                }
                else if (lastRefreshDay != DateTime.Now.Day)
                {
                    // 새로운 날이 시작되면 함수 호출 상태를 리셋
                    functionCalled = false;
                    lastRefreshDay = DateTime.Now.Day;
                }
            }
        }
    }
}
