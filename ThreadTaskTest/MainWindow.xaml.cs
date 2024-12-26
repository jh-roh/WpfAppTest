using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThreadTaskTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            var taskQueue = new TaskQueue();

            // 작업 추가
            taskQueue.Enqueue(async () =>
            {
                Console.WriteLine("Task 1 started");
                await Task.Delay(1000); // 작업 시뮬레이션
                Console.WriteLine("Task 1 completed");
            });

            taskQueue.Enqueue(async () =>
            {
                Console.WriteLine("Task 2 started");
                await Task.Delay(500); // 작업 시뮬레이션
                Console.WriteLine("Task 2 completed");
            });

            taskQueue.Enqueue(async () =>
            {
                Console.WriteLine("Task 3 started");
                await Task.Delay(700); // 작업 시뮬레이션
                Console.WriteLine("Task 3 completed");
            });

        }

        //public async Task ResponseProcessTask()
        //{
        //    await Task.Run(() =>
        //    {
        //        // 실제 작업 내용
        //        Console.WriteLine("ResponseProcessTask Test Start");

        //        Thread.Sleep(5000);
        //        Console.WriteLine("ResponseProcessTask Test End");


        //    });
        //}

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    await ResponseProcessTask();
        //}
    }
}
