using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTaskTest
{
    public class TaskQueue : IDisposable
    {
        BlockingCollection<Func<Task>> _taskQueue = new BlockingCollection<Func<Task>>(); // 작업 큐
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _workerTask;

        private bool disposedValue;

        public TaskQueue()
        {
            _workerTask = Task.Run(() => ProcessTasks(_cancellationTokenSource.Token));
        }
        // 작업 추가
        public void Enqueue(Func<Task> taskGenerator)
        {
            if (!_taskQueue.IsAddingCompleted)
            {
                _taskQueue.Add(taskGenerator); // 큐에 작업 추가
            }
        }
        private async Task ProcessTasks(CancellationToken cancellationToken)
        {
            foreach (var taskGenerator in _taskQueue.GetConsumingEnumerable(cancellationToken))
            {
                try
                {
                    await taskGenerator(); // 작업 실행
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing task: {ex.Message}");
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _taskQueue.CompleteAdding(); // 더 이상 작업을 추가하지 않음
                    _cancellationTokenSource.Cancel(); // 작업 취소
                    _workerTask.Wait(); // 워커 종료 대기
                    _taskQueue.Dispose();
                    _cancellationTokenSource.Dispose();
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~TaskQueue()
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
