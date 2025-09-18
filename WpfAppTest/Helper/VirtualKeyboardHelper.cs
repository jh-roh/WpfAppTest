using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTest.Helper
{
    using System.Diagnostics;

    public class VirtualKeyboardHelper
    {
        private static Process _keyboardProcess;

        // 띄우기 (Windows 10 이상이면 TabTip, 아니면 osk)
        public static  void ShowKeyboard()
        {
            if (_keyboardProcess == null || _keyboardProcess.HasExited)
            {
                string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                string tabTipPath = @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
                //string tabTipPath = @"C:\windows\system32\osk.exe";
            
                if (System.IO.File.Exists(tabTipPath))
                {
                    _keyboardProcess = Process.Start(tabTipPath);
                }
                else
                {
                    _keyboardProcess = Process.Start("osk.exe");
                }
            }
        }

        // 닫기
        public static void HideKeyboard()
        {
            foreach (var process in Process.GetProcessesByName("TabTip"))
                process.Kill();

            foreach (var process in Process.GetProcessesByName("osk"))
                process.Kill();
        }
    }

}
