using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MemoryManageTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            






            //래퍼 형식 : int, string과 같은 기본 형식을 클래스 또는 구조체로 감싼 .NET 형식
            int number1 = 1234; //int 기본형식
            Int32 number2 = 1234; //System.Int32 구조체 : .NET 형식

            string str1 = "안녕"; //string 기본형식
            String str2 = "안녕"; //System.String 클래스 : .NET 형식

            TestMyVector.PrintVectorLength(100, 200);

            //가비지 컬렉터 : 힙 영역에서 사용하지 않는 메모리를 정리
            //가비지 : 더 이상 참조되지 않는 메모리

            //가비지가 발생하는 예시
            //리스트 1) +operator로 문자열을 조합할 때
            int index = 1;
            string name = "text";
            string output = "[" + index.ToString() + "]" + name;
            //각각의 string 인스턴스가 생성되기 때문에 가비지가 많이 생성.


            //C# mutable과 immutable 둘을 나누는 기준은 '할당한 데이터를 변경 가능하냐?'
            //int(값 형식) mutable한 타입, string(레퍼런스 타입) immutable 타입

            int a = 12; //스택 메모리 영역에 저장
            string b = "hello"; //Heap 메모리 영역에 데이터 저장
            b = "Pap";

            //Pap으로 값을 바꾸면 내부적으로는 "Pap"이라는 값은 힙 메모리 어딘가 새로운 곳에 할당
            //그리고 b라는 변수의 레퍼런스는 Pap 할당된 곳을 가르킴
            //StringBuilder를 사용하자.

            //StringBuilder는 문자열을 조합할 때 새로운 객체를 생성하지 않는다.
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(index);
            sb.Append("]");
            sb.Append(name);



            //리스트 2) 메서드안에서 생성한 객체, 가비지가 생성된다.
            SaveData1 sd1 = new SaveData1("홍길동");
            SaveData2 sd2 = new SaveData2("임꺽정");
            Console.WriteLine(sd1.Name);
            Console.WriteLine(sd2.Name);




            //속도 저하가 큰 Boxing
            //Boxing 이란 Value Type 객체 => Reference Type 객체로 포장하는 과정
            //C#의 모든 객체는 Object로 부터 상속
            object testBoxsing = (object)8;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //약한 참조 테스트
            Sample.TestWeakRef();
        }
    }

    /// <summary>
    /// 클래스인 경우
    /// </summary>
    public class SaveData1
    {
        public string Name { get; set; }

        public SaveData1(string _Name)
        {
            Name = _Name;   
        }
    }

    public struct SaveData2
    {
        public string Name { get; set; }

        public SaveData2(string _Name)
        {
            Name = _Name;
        }
    }

    public class NewNames
    {
        public string[] name = new string[100];
        private StringBuilder sb = new StringBuilder();
        //string 객체를 만들어내는게 아니라 이미 잡아놓은 메모리 공간에
        //문자열만 복사해 뒀다가 한번에 ToString()으로 string 객체를 생성
        public void Print()
        {

            sb.Clear();

            //for(int index = 0; index < name.Length; index++)
            //{
            //    sb.Append("[");
            //    sb.Append(index);
            //    sb.Append("]");
            //    sb.Append(name[index]);
            //    sb.AppendLine();
            //}

            for (int index = 0; index < name.Length; index++)
            {
                sb.AppendFormat("[{0}] {1}", index, name.ToString());
                sb.Append($"[{index} {name.ToString()}]");
            }


            Console.WriteLine(sb.ToString());
        }
    }

    /// <summary>
    /// <리스트 4> new로 생성된 인스턴스
    /// </summary>
    public class MyVector
    {
        public float x, y;

        public MyVector()
        {
            this.x = .0f;
            this.y = .0f;
        }

        public MyVector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public double Length()
        {
            return Math.Sqrt(x * x + y * y);
        }
    }

    ////<리스트 5> Vector 클래스를 구조체로 변환
    //public struct MyVector
    //{
    //    public float x, y;
    //    public MyVector(float x, float y)
    //    {
    //        this.x = x;
    //        this.y = y;
    //    }
    //    public double Length()
    //    {
    //        return Math.Sqrt(x * x + y * y);
    //    }
    //}

    static class TestMyVector
    {
        //구조체로 바꿀 수 없다면, <리스트 6> 처럼 멤버 사용을 권장
        //리스트 6 멤버변수 사용
        private static MyVector m_cachedVector = new MyVector();


        public static void PrintVectorLength(float x, float y)
        {
            //new로 생성된 인스턴스는 메소드를 빠져 나오면 더 이상 사용하지 않게 돼 가비지 로 처리된다.
            //이런 패턴의 메소드가 자주 호출될 수록 가비지가 많이 발생
            MyVector v = new MyVector(x, y);

            Console.WriteLine($"Vector({v.x},{v.y}), length={v.Length()}");
        }

        public static void PrintVectorLength_MemberVariable(float x, float y) 
        {
            m_cachedVector.x = x;
            m_cachedVector.y = y;

            Console.WriteLine($"Vector({m_cachedVector.x},{m_cachedVector.y}), length={m_cachedVector.Length()}");

        }
    }

    //Boxing과 UnBoxing 사용 대신, Generic Collection 사용을 권장
    class example
    {
        static public void BadCase()
        {
            ArrayList list = new ArrayList();
            int evenSum = 0;
            int oddSum = 0;

            for(int i =0; i < 1000000; i++) 
            {
                list.Add(i);
            }

            foreach(object item in list)
            {
                if(item is int)
                {
                    int num = (int)item;
                    if (num % 2 == 0) evenSum += num;
                    else oddSum += num;
                }
            }

            string log = $"EventSum={evenSum}, OddSum={oddSum}";
        }

        static public void GoodCase()
        {
            List<int> list = new List<int>();
            int evenSum = 0;
            int oddSum = 0;

            for (int i = 0; i < 1000000; i++)
                list.Add(i);

            foreach (int num in list)
            {
                if (num % 2 == 0) evenSum += num;
                else oddSum += num;
            }

            string log = $"EventSum={evenSum}, OddSum={oddSum}";

        }
    }
   
}
