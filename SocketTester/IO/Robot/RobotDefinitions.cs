﻿using SocketTester.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTester.IO.Robot
{
    public class RobotIOConstant
    {
        public const byte ROBOT_IO_STX = 0xF0;
        public const byte ROBOT_IO_ETX = 0xF4;

        public const byte IO_CMD_COMMON_IAP                    = 0x20;
        public const byte IO_SUB_CMD_IAP_MODE_SETTING          = 0x84;
        public const byte IO_SUB_CMD_IAP_ENTRANCE              = 0x85;
        public const byte IO_SUB_CMD_IAP_DATA_WRITE            = 0x86;
        public const byte IO_SUB_CMD_IAP_DATA_WRITE_RESULT     = 0x87;

        public const byte IO_SUB_CMD_IAP_WRITE_COMPLETE        = 0x88;
        public const byte IO_SUB_CMD_IAP_WRITE_COMPLETE_RESULT = 0x89;

        public const byte IO_CMD_ROBOT_KEEP_ALIVE_SW = 0x45;

        public const byte IO_CMD_ROBOT_KEEP_ALIVE_HW = 0x46;

        //Robot 호출 수신 응답
        public const byte IO_CMD_ROBOT_CALL_REQUEST = 0x61;

        //Robot 진입 가능 여부
        public const byte IO_CMD_ROBOT_APPROACH_REQUEST = 0x62;               //ROBOT 진입 요청 응답

        //Button 누름 Event
        public const byte IO_CMD_ROBOT_BUTTON_PRESS_EVENT = 0x60;

        //Robot IN 완료 (EVENT)
        public const byte IO_CMD_ROBOT_IN_COMPLETED_EVENT = 0x63;           //ROBOT 진입 완료 응답

        //로봇 OUT 완료 (EVENT)
        public const byte IO_CMD_ROBOT_OUT_COMPLETED_EVENT = 0x64;
    }

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

        public byte[] Datas;

        public byte Command;

        public byte SubCommand;

        public IO_ROBOT_BUTTON_PRESS_RESULT? ButtonPressResult;

        public IO_ROBOT_RESPONSE_ACTION_RESULT? ResponseActionResult;

        public IAP_MODE? IAPModeResult;

        public IAP_ACTION_RESULT IAPActionResult;

    }

    public enum IO_ROBOT_BUTTON_PRESS_RESULT
    {
        IO_ROBOT_BUTTON_PRESS_IN  = 0x01,  //IN BUTTON 눌림
        IO_ROBOT_BUTTON_PRESS_OUT = 0x02   //OUT BUTTON 눌림
    }

    public enum IO_ROBOT_RESPONSE_ACTION_RESULT
    {
        IO_ROBOT_RESPONSE_ACTION_SUCCESS = 0x00,
        IO_ROBOT_RESPONSE_ACTION_TIMEOUT = 0x01,
        IO_ROBOT_RESPONSE_ACTION_ENCODDER_ERROR = 0x02,
        IO_ROBOT_RESPONSE_ACTION_CANCLE = 0x03,
        IO_ROBOT_RESPONSE_ACTION_EMERGENCY = 0x04,
        IO_ROBOT_RESPONSE_ACTION_IN_OUT_ERROR = 0x05,
        IO_ROBOT_RESPONSE_ACTION_NO_JIG = 0x06,
    }

    public enum IAP_MODE
    {
        IAP_MODE_NORMAL  = 0x00,  //정상 Normal
        IAP_MODE_ENTRANCE = 0x01,  //IAP 모드
    }

    public enum IAP_ACTION_RESULT
    {
        IAP_ACTION_SUCCESS = 0x00,
        IAP_ACTION_ERROR   = 0xFF,
        
    }

}
