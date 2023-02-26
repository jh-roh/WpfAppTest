using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;

namespace MemoryManageTest
{

    public interface IRenderObject
    {
        void Render();
    }

    public class RenderObject :IRenderObject
    {
        public RenderObject() { 
        
        }

        public void Render()
        {
            
        }
    }


    public class SampleChar : IDisposable
    {
        public bool isRemoved = false;
        private IRenderObject m_Render = Renderer.CreateRenderObject();

        public void Dispose()
        {
            SmpleCharManager.Remove(this);
            m_Render = null;
        }

        public void Render()
        {
            if (m_Render == null) return;
            //렌더링
        }
        
        public void Update()
        {

        }
    }


    /// <summary>
    /// Dispose pateern
    /// 관리되지 않는 메모리(리소스)를 해제하는 용도
    /// IDisposable 인터페이스 제공
    /// </summary>

    public static class Renderer
    {

        public static IRenderObject CreateRenderObject()
        {
            return new RenderObject();
        }
    }

    static class SmpleCharManager
    {
        private static List<SampleChar> m_list = new List<SampleChar>();

        public static void Update()
        {
            foreach(SampleChar obj in m_list)
            {
               
            }
        }
        public static void Render()
        {
            foreach(SampleChar obj in m_list)
            {
                obj.Update();
            }
        }

        public static void Add(SampleChar obj)
        {
            m_list.Add(obj);
        }
        public static void Remove(SampleChar obj)
        {
            m_list.Remove(obj);
        }
    }

    static class DisplayCharInfo
    {

        private static List<WeakReference> m_List = new List<WeakReference>();

        private static Queue<WeakReference> m_removeQueue= new Queue<WeakReference>();

        public static void Update()
        {
            foreach(WeakReference item in m_List)
            {
                SampleChar obj = item.Target != null ? item.Target as SampleChar : null;

                if(obj == null || obj.isRemoved)
                {
                    m_removeQueue.Enqueue(item);
                }
                else
                {
                    /*캐릭터 정보 표시*/
                }
            }

            while(m_removeQueue.Count > 0)
            {
                WeakReference item = m_removeQueue.Dequeue();
                m_List.Remove(item);
            }
        }

        public static void Add(SampleChar obj)
        {
            m_List.Add(new WeakReference(obj));
        }
    }
}
