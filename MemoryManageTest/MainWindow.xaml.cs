using System;
using System.Collections.Generic;
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
   
}
