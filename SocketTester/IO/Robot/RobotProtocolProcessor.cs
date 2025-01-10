using SocketTester.IO.Robot;
using SocketTester.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SocketTester.Robot
{
    /// <summary>
    /// STX      BYTE 1      0xF0 고정        Header
    /// LENGTH   BYTE 2      0 ~ 65535        Command + Data Length
    /// COMMAND  BYTE 1      0x20 ~ 0x97      Command
    /// DATA     0 ~ 254     0x00 ~ 0xFF      Data String
    /// CHECKSUM BYTE 1      0x00 ~ 0xFF      Length ^ Command ^ Data
    /// ETX      BYTE 1      0xF4 고정        Foot
    /// </summary>
    public class RobotProtocolProcessor
    {
        private const int _lengthLow = 1;
        private const int _lengthHigh = 2;
        private const int _commandPosition = 3;

        /**********************도킹 관련 Board 쪽으로 명령 요청*************/

        public static byte[] ConstructRobot(RobotIOSend ioSendData)
        {
            List<byte> constructData = new List<byte>();
            ushort length = (ushort)(ioSendData.DataLength + 1);
            
            byte lengthLow = Convert.ToByte(length & 0x00FF);
            byte lengthHigh = Convert.ToByte(length >> 8);

            constructData.Add(RobotIOConstant.ROBOT_IO_STX);
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
            constructData.Add(RobotIOConstant.ROBOT_IO_ETX);

            return constructData.ToArray();
        }


        /**********************도킹 관련 데이터 byte[]로 컨버터*************/

        public static byte[] ConvertIAPModeDatasToByte(IAP_MODE mode)
        {
            return new byte[] { (byte)mode };
        }


        public static byte[] ConvertIAPEntranceDatasToByte(ushort boardId, int fileSize)
        {
            byte boardLow = Convert.ToByte(boardId & 0x00FF);
            byte boardHigh = Convert.ToByte(boardId >> 8);


            byte fileSize1 = Convert.ToByte(fileSize & 0x000000FF);
            byte fileSize2 = Convert.ToByte((fileSize >> 8) & 0x000000FF);
            byte fileSize3 = Convert.ToByte((fileSize >> 16) & 0x000000FF);
            byte fileSize4 = Convert.ToByte(fileSize >> 24);


            return new byte[] { RobotIOConstant.IO_SUB_CMD_IAP_ENTRANCE , boardLow, boardHigh, fileSize1 , fileSize2, fileSize3, fileSize4 };
        }

        public static byte[] ConvertIAPPageDatasToReverseByte(byte[] pageDatas)
        {
            byte[] reversePageDatas = new byte[pageDatas.Length];

            // 원본 배열 복사
            Array.Copy(pageDatas, reversePageDatas, pageDatas.Length);

            // 역순 처리
            Array.Reverse(reversePageDatas);
            
            // 출력
            Console.WriteLine($"Original: {string.Join(", ", pageDatas)}");
            Console.WriteLine($"Reversed: {string.Join(", ", reversePageDatas)}");

            return reversePageDatas;
        }


        /**********************도킹 관련 Board 쪽에서 명령 응답*************/

        public static List<byte[]> ParseProtocol(byte[] data)
        {

            var result = new List<byte[]>();
            int startIndex = -1;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == RobotIOConstant.ROBOT_IO_STX)
                {
                    startIndex = i; // STX 시작 위치 설정
                }
                else if (data[i] == RobotIOConstant.ROBOT_IO_ETX && startIndex != -1)
                {
                    int length = i - startIndex + 1; // STX에서 ETX까지 길이 계산
                    var segment = new byte[length];
                    Array.Copy(data, startIndex, segment, 0, length); // 데이터 분리
                    result.Add(segment);
                    startIndex = -1; // 초기화
                }
            }

            return result;
        }


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

            for (int i = _commandPosition; i <= (_commandPosition + length); i++)
            {
                checkSum ^= receiveDatas[i];
            }

            if (length > 1)
            {
                ioResult.Datas = new byte[length - 1];
                Array.Copy(receiveDatas, (_commandPosition + 1), ioResult.Datas, 0, length - 1);
            }


            switch (ioResult.Command)
            {
                case RobotIOConstant.IO_CMD_ROBOT_BUTTON_PRESS_EVENT:
                    ioResult.ButtonPressResult = (IO_ROBOT_BUTTON_PRESS_RESULT)receiveDatas[_commandPosition + 1];
                    break;


                case RobotIOConstant.IO_CMD_ROBOT_IN_COMPLETED_EVENT:
                    ioResult.ResponseActionResult = (IO_ROBOT_RESPONSE_ACTION_RESULT)receiveDatas[_commandPosition + 1];
                    break;

                case RobotIOConstant.IO_CMD_ROBOT_OUT_COMPLETED_EVENT:
                    ioResult.ResponseActionResult = (IO_ROBOT_RESPONSE_ACTION_RESULT)receiveDatas[_commandPosition + 1];
                    break;

                case RobotIOConstant.IO_CMD_ROBOT_APPROACH_REQUEST:
                    ioResult.ResponseActionResult = (IO_ROBOT_RESPONSE_ACTION_RESULT)receiveDatas[_commandPosition + 1];

                    break;

                case RobotIOConstant.IO_CMD_ROBOT_CALL_REQUEST:
                    break;

                case RobotIOConstant.IO_CMD_COMMON_IAP:
                    
                    byte subCommand = receiveDatas[_commandPosition + 1];
                    ioResult.SubCommand = subCommand;
                    switch (subCommand)
                    {
                        case RobotIOConstant.IO_SUB_CMD_IAP_MODE_SETTING:
                            ioResult.IAPModeResult = (IAP_MODE)receiveDatas[_commandPosition + 2];
                            break;

                        case RobotIOConstant.IO_SUB_CMD_IAP_ENTRANCE:
                            ioResult.IAPActionResult = (IAP_ACTION_RESULT)receiveDatas[_commandPosition + 2];

                            break;

                        case RobotIOConstant.IO_SUB_CMD_IAP_DATA_WRITE_RESULT:
                            ioResult.IAPActionResult = (IAP_ACTION_RESULT)receiveDatas[_commandPosition + 2];

                            break;

                        case RobotIOConstant.IO_SUB_CMD_IAP_WRITE_COMPLETE_RESULT:
                            ioResult.IAPActionResult = (IAP_ACTION_RESULT)receiveDatas[_commandPosition + 2];

                            break;
                    }
                    break;
            }

            return ioResult;
        }
    }

}
