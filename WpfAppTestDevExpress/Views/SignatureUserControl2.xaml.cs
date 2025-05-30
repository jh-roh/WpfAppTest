using Microsoft.Win32;
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
    /// SignatureUserControl2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SignatureUserControl2 : UserControl
    {
        public SignatureUserControl2()
        {
            InitializeComponent();
        }

        // 서명 지우기
        private void ClearSignature_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }

        // 서명 저장
        private void SaveSignature_Click(object sender, RoutedEventArgs e)
        {
            if (inkCanvas.Strokes.Count == 0)
            {
                MessageBox.Show("서명란이 비어 있습니다.");
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "PNG 파일|*.png",
                FileName = "signature.png"
            };
            if (dlg.ShowDialog() == true)
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap(
                    (int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                rtb.Render(inkCanvas);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));
                using (FileStream fs = new FileStream(dlg.FileName, FileMode.Create))
                {
                    encoder.Save(fs);
                }
                MessageBox.Show("서명이 저장되었습니다!");
            }
        }

        // 서명 미리보기
        private void PreviewSignature_Click(object sender, RoutedEventArgs e)
        {
            if (inkCanvas.Strokes.Count == 0)
            {
                MessageBox.Show("서명란이 비어 있습니다.");
                return;
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(
                (int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            rtb.Render(inkCanvas);

            PreviewImage.Source = rtb;
            PreviewPopup.Visibility = Visibility.Visible;
        }

        // 미리보기 닫기
        private void ClosePreview_Click(object sender, RoutedEventArgs e)
        {
            PreviewPopup.Visibility = Visibility.Collapsed;
        }
    }
}
