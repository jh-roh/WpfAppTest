using CommonUtil;
using SocketTester.Globals;
using SocketTester.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketTester.IO.Robot
{

    public delegate void ProtocolMessageNotifyEventHandler(string e);

    public class ProtocolMessageManager
    {
        public static BlockingCollection<string> ProcessMessage = new BlockingCollection<string>();

        public static event ProtocolMessageNotifyEventHandler MessageNotify;

        static object _lockObject = new object();


        public static void AddProcessMessage(string message)
        {
            lock(_lockObject)
            {
                ProcessMessage.Add(message);
            }
        }

        public static void NotifyProtocolMessage()
        {
            var processThread = new Thread(() =>
            {
                while (true)
                {
                    var processMessage = ProcessMessage.Take();

                    Log.Info2(null, null, processMessage, ApplicationConfig.RobotIOParserFileName);

                    if (MessageNotify != null)
                    {
                        MessageNotify(processMessage);
                    }
                }

            });

            processThread.IsBackground = true;
            processThread.SetApartmentState(ApartmentState.STA);
            processThread.Start();

            
        }
    }
}
