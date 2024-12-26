using SocketTester.IO.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTester.Helper
{
    public static class EnumExtensions
    {
        public static string ToTrimmedString<T>(this T enumValue) where T : Enum
        {
            // Enum 타입별 접두사 정의
            string prefix;

            // switch 문을 사용하여 Enum 타입에 맞는 접두사를 지정
            switch (enumValue)
            {
                case IO_ROBOT_RESPONSE_ACTION_RESULT _:
                    prefix = "IO_ROBOT_RESPONSE_ACTION_";
                    break;
                case IO_ROBOT_BUTTON_PRESS_RESULT _:
                    prefix = "IO_ROBOT_BUTTON_PRESS_";
                    break;
                default:
                    prefix = string.Empty; // 기본값: 접두사가 없으면 그대로 반환
                    break;
            }

            string enumString = enumValue.ToString();

            // 접두사 제거
            if (!string.IsNullOrEmpty(prefix) && enumString.StartsWith(prefix))
            {
                return enumString.Substring(prefix.Length);
            }

            return enumString;
        }
    }
}
