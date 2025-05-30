using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace WpfAppTestDevExpress.Views
{
    /// <summary>
    /// SignatureUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SignatureUserControl : UserControl
    {

        private bool isDrawing = false;
        private Polyline currentLine;

        public SignatureUserControl()
        {
            InitializeComponent();
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;
            Point position = e.GetPosition(SignatureCanvas);

            currentLine = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            currentLine.Points.Add(position);

            SignatureCanvas.Children.Add(currentLine);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && currentLine != null)
            {
                Point position = e.GetPosition(SignatureCanvas);
                currentLine.Points.Add(position);
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SignatureCanvas.Children.Clear();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCanvasAsImage(SignatureCanvas, "signature.png");
            MessageBox.Show("Signature saved!");
        }

        private void SaveCanvasAsImage(Canvas canvas, string filename)
        {
            // 크기 측정 및 렌더링
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                (int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            // PNG로 저장
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                encoder.Save(fs);
            }
        }
    }
}
