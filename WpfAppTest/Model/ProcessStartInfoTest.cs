using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace WpfAppTest.Model
{
    public class ProcessStartInfoTest
    {
        public ProcessStartInfoTest()
        {

        }

        public void ProcessTest()
        {
            try
            {
                

                // Launch Notepad++ if the executable file is found
                
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    //startInfo.FileName = @"C:\Program Files\Notepad++\notepad.exe";
                    startInfo.FileName = @"C:\Program Files\Notepad++\notepad.exe";
                    startInfo.Arguments = @"D:\TFS\JSW\INTIPharm\Branch\R3\JVMService\INTIPharmDBManager\PollingWebServiceForDeploy\Sample\SampleSource\ParentPathSample\ParentPathByWebService_Sample.xml";
                    var returnValue = Process.Start(startInfo);
                



            }
            catch (Exception ex)
            {
                MessageBox.Show("notepad++를 설치해주세요");


                Console.WriteLine("Error launching Notepad++: " + ex.Message);
            }




        }




    }
}
