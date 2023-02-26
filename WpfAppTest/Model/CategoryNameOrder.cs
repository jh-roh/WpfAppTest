using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfAppTest.Model
{


    [Obsolete("SortOrder 열거형을 사용하세요.")]
    public enum CategoryNameOrder
    {
        Asc,
        Desc,
    }

    /// <summary>
    /// SortOrder 열거형 : 행의 데이터 정렬방법
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// 오름차순
        /// </summary>
        Ascending,
        /// <summary>
        /// 내림차순
        /// </summary>
        Descending,
        /// <summary>
        /// 정렬 순서 없음
        /// </summary>
        None
    }
}
