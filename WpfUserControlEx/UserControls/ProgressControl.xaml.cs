using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfUserControlEx.UserControls
{
    /// <summary>
    /// ProgressControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProgressControl : UserControl
    {
        private DispatcherTimer timer;

        public static readonly DependencyProperty IsRunningProperty =
        DependencyProperty.Register(nameof(IsRunning), typeof(bool), typeof(ProgressControl),
            new PropertyMetadata(false, OnIsRunningChanged));

        public bool IsRunning
        {
            get => (bool)GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }

        private static void OnIsRunningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressControl control && e.NewValue is bool isRunning)
            {
                control.SetRun(isRunning);
            }
        }

        public ProgressControl()
        {
            InitializeComponent();
        }

        private void SetRun(bool isRun)
        {
            if (timer == null)
            {
                timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(100)
                };
                timer.Tick += timer_Tick;
            }

            if (isRun)
            {
                timer.Start();
                canvas_Processing.Visibility = Visibility.Visible;
                textBlock_LoadingMessage.Text = "Processing...";
            }
            else
            {
                timer.Stop();
                canvas_Processing.Visibility = Visibility.Collapsed;
                textBlock_LoadingMessage.Text = "Ready...";
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                SpinnerRotate.Angle += 30;
                if (SpinnerRotate.Angle == 360)
                {
                    SpinnerRotate.Angle = 0;
                }
            }
            catch
            {
                // Handle exceptions appropriately
            }
        }
    }
}
