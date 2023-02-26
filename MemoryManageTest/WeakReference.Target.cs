using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemoryManageTest
{

    /// <summary>
    /// WeakReference로 의도하지 않은 참조 삭제
    /// </summary>
    public class Sample
    {
        public class Fruit
        {

            public Fruit(string name) { this.Name = name; }
            public string Name { get; private set; }
        }

        public static void TestWeakRef()
        {
            Fruit apple = new Fruit("Apple");
            Fruit orange = new Fruit("Orange");

            Fruit fruit1 = apple; //강한 참조
            WeakReference fruit2 = new WeakReference(orange);

            //이렇게 해놓으면 WeakReference가 해제되지 않음. 참조가 걸려 있어서 그런듯
            //Fruit target = fruit2.Target as Fruit;

            string log1 = $"Fruit1 = {fruit1.Name}, Fruit2 = {(fruit2.Target == null ? "" : fruit2.Target)}";

            apple = null;

            orange = null;
            fruit2.Target = null;

            System.GC.Collect(0, GCCollectionMode.Forced);
            System.GC.WaitForFullGCComplete();
            //System.GC.Collect();
            //fruite1과 fruit2의 값을 바꾼적은 없지만 fruit2의 결과가 달라진다.

            if (fruit2.IsAlive)
            {
                string log2 = $"Fruit1 = {(fruit1 == null ? "" : fruit1.Name)}, Fruit2 = {(fruit2.Target == null ? "" : fruit2.Target)}";
            }
            else
            {
                string log3 = $"Fruit1 = {(fruit1 == null ? "" : fruit1.Name)}, Fruit2 = {(fruit2.Target == null ? "" : fruit2.Target)} The object has been garbage collected.";

            }





        }
    }
}
