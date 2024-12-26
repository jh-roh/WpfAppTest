using SocketTester.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SocketTester.Model
{
    public class ClientRepository<T> : PropertyChangedBase, IDisposable
    {
        private bool disposedValue;
        private readonly object _lockObject = new object(); 

        public ClientRepository() 
        {
            Items = new ObservableCollection<T>(); 
        }
        
        private ObservableCollection<T> _items;

        public ObservableCollection<T> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        //private ObservableCollection<RobotResultModel> _robotResults;
        //public ObservableCollection<RobotResultModel> RobotResults
        //{
        //    get
        //    {
        //        return _robotResults;
        //    }
        //    set
        //    {
        //        _robotResults = value;
        //        OnPropertyChanged();
        //    }
        //}


        public IEnumerable<T> GetAllItems()
        {
            lock (_lockObject)
            {
                return Items.ToList(); // ToList()로 복사본을 반환하여 스레드 안전성 보장
            }
        }


        public void Add(T item)
        {
            lock (_lockObject)
            {
                if (!Items.Contains(item))
                {
                    Items.Add(item);
                }
            }
        }

        public void Update(T item, Action<T> updateAction)
        {
            lock (_lockObject)
            {
                lock (_lockObject)
                {
                    var existingItem = Items.FirstOrDefault(i => i.Equals(item));
                    if (existingItem != null)
                    {
                        updateAction(existingItem); // 항목 업데이트 작업 수행
                    }
                }
            }
        }

        public void Remove(T item)
        {
            lock (_lockObject)
            {
                Items.Remove(item);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Items.Clear();
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~ClientModel()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
