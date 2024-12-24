using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocketTester.Helper
{
    public class UtilHelper
    {
        /// <summary>
        /// 클래스에서 특정 값의 상수 이름을 반환합니다.
        /// </summary>
        /// <param name="type">상수를 포함한 클래스의 Type</param>
        /// <param name="value">찾고자 하는 상수 값</param>
        /// <returns>상수 이름 또는 null</returns>
        public static string GetConstantName(Type type, object value)
        {
            // 클래스의 모든 public static 필드 조회
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly) // 상수인지 확인
                {
                    var fieldValue = field.GetValue(null); // 상수 값 가져오기
                    if (fieldValue.Equals(value))
                    {
                        return field.Name; // 상수 이름 반환
                    }
                }
            }

            return null; // 해당 값이 없으면 null 반환
        }
    }
}
