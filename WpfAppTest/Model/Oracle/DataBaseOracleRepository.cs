using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace WpfAppTest.Model.Oracle
{
    internal class DataBaseOracleRepository : IDatabaseRepository
    {
        private String ErrorCodeString1 = "id, connectionsString, pw, Query 의 Parameter의 이름 및 값을 정확하게 입력해야 합니다. (대소문자 구분 필요)";
        public DataBaseOracleRepository() { }

        public List<dynamic> CreateDynamicList(List<Dictionary<string, object>> dataDictList)
        {
            // 데이터의 첫 번째 사전에서 동적 클래스를 생성하는데 사용될 형식 추출
            Type dynamicType = CreateDynamicClass(dataDictList[0]);

            // 동적 클래스의 List<T> 생성
            List<dynamic> dynamicList = new List<dynamic>();

            // 각 사전에서 동적 클래스 인스턴스를 생성하고 List에 추가
            foreach (var dataDict in dataDictList)
            {
                dynamic dynamicInstance = Activator.CreateInstance(dynamicType);

                // 생성된 동적 클래스의 속성 값 설정
                foreach (var kvp in dataDict)
                {
                    PropertyInfo dynamicProperty = dynamicType.GetProperty(kvp.Key);
                    dynamicProperty.SetValue(dynamicInstance, Convert.ChangeType(kvp.Value, dynamicProperty.PropertyType), null);
                }

                // List에 동적 클래스 인스턴스 추가
                dynamicList.Add(dynamicInstance);
            }

            return dynamicList;
        }

        public Type CreateDynamicClass(Dictionary<string, object> dataDict)
        {

            // 어셈블리 빌더 생성
            AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
            AppDomain currentDomain = AppDomain.CurrentDomain;
            AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            // 모듈 빌더 생성
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            // 클래스 빌더 생성
            TypeBuilder typeBuilder = moduleBuilder.DefineType("DynamicClass", TypeAttributes.Public);

            // 데이터의 키를 기반으로 동적 클래스에 속성 추가
            foreach (var kvp in dataDict)
            {
                FieldBuilder fieldBuilder = typeBuilder.DefineField(String.Format($"_{0}", kvp.Key.ToLower()), kvp.Value.GetType(), FieldAttributes.Private);
                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(kvp.Key, PropertyAttributes.HasDefault, kvp.Value.GetType(), null);

                // Getter 메서드 생성
                MethodBuilder getterMethod = typeBuilder.DefineMethod(String.Format("get_{0}", kvp.Key), MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, kvp.Value.GetType(), Type.EmptyTypes);
                ILGenerator getterIL = getterMethod.GetILGenerator();
                getterIL.Emit(OpCodes.Ldarg_0);
                getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
                getterIL.Emit(OpCodes.Ret);

                // Setter 메서드 생성
                MethodBuilder setterMethod = typeBuilder.DefineMethod(String.Format("set_{0}", kvp.Key), MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { kvp.Value.GetType() });
                ILGenerator setterIL = setterMethod.GetILGenerator();
                setterIL.Emit(OpCodes.Ldarg_0);
                setterIL.Emit(OpCodes.Ldarg_1);
                setterIL.Emit(OpCodes.Stfld, fieldBuilder);
                setterIL.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getterMethod);
                propertyBuilder.SetSetMethod(setterMethod);
            }

            // 클래스 완성
            Type dynamicType = typeBuilder.CreateType();

            return dynamicType;

            //// 어셈블리 빌더 생성
            //AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
            //AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            //// 모듈 빌더 생성
            //ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            //// 클래스 빌더 생성
            //TypeBuilder typeBuilder = moduleBuilder.DefineType("DynamicClass", TypeAttributes.Public);

            //// 데이터의 키를 기반으로 동적 클래스에 속성 추가
            //foreach (var kvp in dataDict)
            //{
            //    FieldBuilder fieldBuilder = typeBuilder.DefineField(String.Format($"_{0}", kvp.Key.ToLower()), kvp.Value.GetType(), FieldAttributes.Private);
            //    PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(kvp.Key, PropertyAttributes.HasDefault, kvp.Value.GetType(), null);

            //    // Getter 메서드 생성
            //    MethodBuilder getterMethod = typeBuilder.DefineMethod(String.Format("get_{0}", kvp.Key), MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, kvp.Value.GetType(), Type.EmptyTypes);
            //    ILGenerator getterIL = getterMethod.GetILGenerator();
            //    getterIL.Emit(OpCodes.Ldarg_0);
            //    getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
            //    getterIL.Emit(OpCodes.Ret);

            //    // Setter 메서드 생성
            //    MethodBuilder setterMethod = typeBuilder.DefineMethod(String.Format("set_{0}", kvp.Key), MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new[] { kvp.Value.GetType() });
            //    ILGenerator setterIL = setterMethod.GetILGenerator();
            //    setterIL.Emit(OpCodes.Ldarg_0);
            //    setterIL.Emit(OpCodes.Ldarg_1);
            //    setterIL.Emit(OpCodes.Stfld, fieldBuilder);
            //    setterIL.Emit(OpCodes.Ret);

            //    propertyBuilder.SetGetMethod(getterMethod);
            //    propertyBuilder.SetSetMethod(setterMethod);
            //}

            //// 클래스 완성
            //Type dynamicType = typeBuilder.CreateType();

            //return dynamicType;
        }

        public string GetExecuteProcedure(string getJsonData)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(getJsonData);

            // Reflection을 사용하여 변수 이름을 추출
            var jsonObject = (JObject)dynamicObject;
            var jsonPropertyList = jsonObject.Properties();

            string queryString = dynamicObject.queryString;

            string jsonResult = "";
            string errorDescription = "";

            if (dynamicObject.tnsName == null || dynamicObject.id == null || dynamicObject.pw == null)
            {
                errorDescription = ErrorCodeString1;

                return "";
            }

            var connectionString = $"Data Source={dynamicObject.tnsName};User Id={dynamicObject.id};Password={dynamicObject.pw};";

            // Oracle 데이터베이스에 연결
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // OracleCommand 객체 생성
                    using (OracleCommand command = new OracleCommand(queryString, connection))
                    {
                        foreach(var property in jsonPropertyList)
                        {
                            if(property.Name.Contains("outParamater_RefCursor"))
                            {
                                // 출력 매개변수 설정
                                OracleParameter cursorParam = new OracleParameter(property.Value.ToString(), OracleDbType.RefCursor);
                                cursorParam.Direction = System.Data.ParameterDirection.Output;
                                command.Parameters.Add(cursorParam);
                            }
                        }
                            
                        // OracleDataReader를 사용하여 데이터 읽기
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            // 데이터를 담을 List 생성
                            var dataList = new List<object>();

                            while (reader.Read())
                            {
                                // 데이터를 읽어서 Dictionary에 추가
                                var dataDict = new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue = reader.GetValue(i);
                                    dataDict.Add(columnName, columnValue);
                                }

                                dataList.Add(dataDict);
                            }

                            // List를 JSON 문자열로 변환
                            jsonResult = JsonConvert.SerializeObject(dataList);

                            // JSON 결과 출력
                            Console.WriteLine(jsonResult);
                        }
                    }

                }
                catch (Exception ex)
                {
                    errorDescription = ex.Message;
                }
            }

            return jsonResult;

        }

        public string GetExecuteSelectQuery(string getJsonData)
        {

            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(getJsonData);

            string jsonResult = "";
            string errorDescription = "";
            string queryString = dynamicObject.queryString;
            if (dynamicObject.tnsName == null || dynamicObject.id == null || dynamicObject.pw == null)
            {
                errorDescription = ErrorCodeString1;

                return "";
            }

            var connectionString = $"Data Source={dynamicObject.tnsName};User Id={dynamicObject.id};Password={dynamicObject.pw};";

            // Oracle 데이터베이스에 연결
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {   
                    connection.Open();

                    // OracleCommand 객체 생성
                    using (OracleCommand command = new OracleCommand(queryString, connection))
                    {
                        // OracleDataReader를 사용하여 데이터 읽기
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            // 데이터를 담을 List 생성
                            var dataList = new List<Dictionary<string, object>>();

                            while (reader.Read())
                            {
                                // 데이터를 읽어서 Dictionary에 추가
                                var dataDict = new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue = reader.GetValue(i);
                                    dataDict.Add(columnName, columnValue);
                                }

                                dataList.Add(dataDict);
                            }

                            // List를 JSON 문자열로 변환
                            //jsonResult = JsonConvert.SerializeObject(dataList);

                            // JSON 결과 출력
                            Console.WriteLine(jsonResult);

                            // 동적 클래스를 기반으로 List<T> 생성
                            List<dynamic> dynamicList = CreateDynamicList(dataList);


                        }
                    }
                }
                catch (Exception ex)
                {
                    errorDescription = ex.Message;
                }
            }

            return jsonResult;
        }

        public string SetExecuteProcedure(string setJsonData)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(setJsonData);

            // Reflection을 사용하여 변수 이름을 추출
            var jsonObject = (JObject)dynamicObject;
            var jsonPropertyList = jsonObject.Properties();

            string jsonResult = "";
            string errorDescription = "";
            string queryString = dynamicObject.queryString;
            if (dynamicObject.tnsName == null || dynamicObject.id == null || dynamicObject.pw == null)
            {
                errorDescription = ErrorCodeString1;

                return "";
            }

            var connectionString = $"Data Source={dynamicObject.tnsName};User Id={dynamicObject.id};Password={dynamicObject.pw};";

            // Oracle 데이터베이스에 연결
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OracleTransaction transaction = connection.BeginTransaction())
                    {
                        // OracleCommand 객체 생성
                        using (OracleCommand command = new OracleCommand(queryString, connection))
                        {
                            foreach (var property in jsonPropertyList)
                            {
                                if (property.Name.Contains("outParamater_VARCHAR2"))
                                {
                                    // 출력 매개변수 설정
                                    OracleParameter cursorParam = new OracleParameter(property.Value.ToString(), OracleDbType.Varchar2);
                                    cursorParam.Direction = System.Data.ParameterDirection.Output;
                                    cursorParam.Size = 500;
                                    command.Parameters.Add(cursorParam);
                                }
                            }

                            try
                            {
                                // UPDATE 쿼리 실행
                                command.ExecuteNonQuery();

                                // 트랜잭션 커밋
                                transaction.Commit();

                                foreach(var para in command.Parameters)
                                {
                                    var outPara = (OracleParameter)para;

                                    if (outPara.Direction == System.Data.ParameterDirection.Output)
                                    {
                                        string outPutName = outPara.ParameterName.ToString();
                                        string outPutValue = outPara.Value.ToString();
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                errorDescription = ex.Message;
                                transaction.Rollback();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    errorDescription = ex.Message;
                }
            }

            return jsonResult;
        }

        public string SetExecuteUpdateQuery(string setJsonData)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(setJsonData);

            string jsonResult = "";
            string errorDescription = "";
            string queryString = dynamicObject.queryString;

            if (dynamicObject.tnsName == null || dynamicObject.id == null || dynamicObject.pw == null)
            {
                errorDescription = ErrorCodeString1;

                return "";
            }

            var connectionString = $"Data Source={dynamicObject.tnsName};User Id={dynamicObject.id};Password={dynamicObject.pw};";

            // Oracle 데이터베이스에 연결
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (OracleTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // OracleCommand 객체 생성
                            using (OracleCommand command = new OracleCommand(queryString, connection))
                            {
                                // UPDATE 쿼리 실행
                                int rowsAffected = command.ExecuteNonQuery();

                                // 트랜잭션 커밋
                                transaction.Commit();
                                Console.WriteLine($"총 {rowsAffected}개의 행이 업데이트되었습니다.");
                            }
                        }
                        catch (Exception ex)
                        {
                            errorDescription = ex.Message;
                            transaction.Rollback();
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorDescription = ex.Message;
                }
            }

            return jsonResult;
        }
    }
}
