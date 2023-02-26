using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfAppTest.Model
{
    /// <summary>
    /// 제네릭 인터페이스 : 공통 CRUD 코드
    /// </summary>
    /// <typeparam name="T">모델 클래스</typeparam>
    /// <typeparam name="V">열거형 또는 문자열 </typeparam>
    public interface IBreadShow<T> where T : class
    {
        //BREAD SHOP 패턴

        T Browse(int id);

        List<T> Read();

        T Edit(T model);

        T Add(T model);

        void Delete(int id);

        List<T> Search(string query);

        int Has();
        IEnumerable<T> Ordering(SortOrder order);

        List<T> Paging(int pageNumber, int pageSize);


    }


}
