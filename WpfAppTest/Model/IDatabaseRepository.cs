using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfAppTest.Model
{
    /// <summary>
    /// 각 병원 별 DB 데이터 엔진을 접속하고 사용하기 위한 인터페이스
    /// </summary>
    internal interface IDatabaseRepository
    {
        List<dynamic> CreateDynamicList(List<Dictionary<string, object>> dataDictList);

        Type CreateDynamicClass(Dictionary<string, object> dataDict);
        string GetExecuteSelectQuery(string getJsonData);

        string SetExecuteUpdateQuery(string setJsonData);

        string GetExecuteProcedure(string getJsonData);

        string SetExecuteProcedure(string setJsonData);
    }
}
