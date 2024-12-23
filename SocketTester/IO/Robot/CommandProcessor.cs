using SocketTester.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SocketTester.Robot
{
    public struct RobotIOSend
    {
        public byte DataLength;
        public byte Command;
        public byte[] DataArray;
    }

    public struct RobotIOResult
    {
        public SocketHandlerType HandlerType;

        public int ClientId;

        public int DataLength;

        public byte[] buffer;

        public byte Command;
    }

    public class CommandProcessor
    {

        public const byte ROBOT_IO_STX = 0xF0;
        public const byte ROBOT_IO_ETX = 0xF4;

        /*********************Middleware -> Board*************************/

        public const byte ROBOT_KEEP_ALIVE              = 0x45;

        //Robot 호출 수신 응답
        public const byte ROBOT_CALL_RECEPTION_RESPONSE = 0x61;

        //Robot 진입 가능 여부
        public const byte ROBOT_ENTRY_POSSIBLE          = 0x62;

        /*********************Board -> Middleware*************************/
        //Button 누름 Event
        public const byte ROBOT_BUTTON_PRESS_EVENT      = 0x60;

        //Robot IN 완료 (EVENT)
        public const byte ROBOT_IN_COMPLETED_EVENT      = 0x63;

        //로봇 OUT 완료 (EVENT)
        public const byte ROBOT_OUT_COMPLETED_EVENT     = 0x64;


        /**********************도킹 관련 Board 쪽으로 명령 요청*************/
        public static byte[] ConstructRobot(RobotIOSend ioSendData)
        {
            List<byte> constructData = new List<byte>();
            ushort length = (ushort)(ioSendData.DataLength + 1);
            
            byte lengthLow = Convert.ToByte(length & 0x00FF);
            byte lengthHigh = Convert.ToByte(length >> 8);

            constructData.Add(CommandProcessor.ROBOT_IO_STX);
            constructData.Add(lengthLow);
            constructData.Add(lengthHigh);
            constructData.Add(ioSendData.Command);

            byte checkSum = lengthLow;
            checkSum ^= lengthHigh;
            checkSum ^= ioSendData.Command;

            if (ioSendData.DataArray != null)
            {
                foreach (var item in ioSendData.DataArray)
                {
                    constructData.Add(item);
                    checkSum ^= item;
                }
            }

            constructData.Add(checkSum);
            constructData.Add(CommandProcessor.ROBOT_IO_ETX);

            return constructData.ToArray();
        }
    }


    public class CommadParser
    {
        public static RobotIOSend RequestRobotKeepAlive()
        {
            return new RobotIOSend
            {
                Command = CommandProcessor.ROBOT_KEEP_ALIVE,
                DataLength = 0,
                DataArray = null,
            };

        }

        public static RobotIOSend RequestRobotCallReception()
        {
            return new RobotIOSend
            {
                Command = CommandProcessor.ROBOT_CALL_RECEPTION_RESPONSE,
                DataLength = 0,
                DataArray = null
            };
        }

        public static RobotIOSend ResponseRobotEntryPossible()
        {
            return new RobotIOSend
            {
                Command = CommandProcessor.ROBOT_ENTRY_POSSIBLE,
                DataLength = 0,
                DataArray = null
            };
        }
    }

    public class CommandAnalyer
    {
        private const int _lengthLow = 1;
        private const int _lengthHigh = 2;
        private const int _commandPosition = 3;

        /**********************도킹 관련 Board 쪽에서 명령 응답*************/

        public static RobotIOResult AnalyzeRobot(byte[] receiveDatas)
        {
            RobotIOResult ioResult = new RobotIOResult();

            ioResult.Command = receiveDatas[_commandPosition];

            //0 ~ 65535
            ushort lengthLow = (ushort)(0x00FF & receiveDatas[_lengthLow]);
            ushort lengthHigh = (ushort)(receiveDatas[_lengthHigh] << 8);
            lengthHigh = (ushort)(0xFFFF & lengthHigh);
            ushort length = (ushort)(lengthLow | lengthHigh);

            ioResult.DataLength = length;

            byte checkSum = receiveDatas[_lengthLow];
            checkSum ^= receiveDatas[_lengthHigh];

            for(int i = _commandPosition; i <= (_commandPosition + length); i++)
            {
                checkSum ^= receiveDatas[i];
            }

            switch (ioResult.Command)
            {
                case CommandProcessor.ROBOT_IN_COMPLETED_EVENT:
                    break;

                case CommandProcessor.ROBOT_OUT_COMPLETED_EVENT:

                    break;


                case CommandProcessor.ROBOT_BUTTON_PRESS_EVENT:
                    break;

                case CommandProcessor.ROBOT_ENTRY_POSSIBLE:

                    break;

                case CommandProcessor.ROBOT_CALL_RECEPTION_RESPONSE:

                    break;
            }

            return ioResult;
        }

    }

}
