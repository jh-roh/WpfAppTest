using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using Microsoft.CodeAnalysis.Emit;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;

namespace CodeDomTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
        //    string jsonString = @"
        //{
        //    ""StartTime"": ""2020-08-01T00:00:00"",
        //    ""EndTime"": ""2022-11-06T00:00:00"",
        //    ""SearchText"": """",
        //    ""Language"": 1,
        //    ""PageNumber"": 1,
        //    ""PageSize"": 10000
        //}";

        //    SearchRequest searchRequest = JsonSerializer.Deserialize<SearchRequest>(jsonString);


            #region Roslyn 사용하여 코드돔 기능 구현
            string codeToCompile = File.ReadAllText("dynamic_code.cs");
            string assemblyName = "MyAssembly";

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codeToCompile);

            var refPaths = new[] {
                    typeof(System.Object).GetTypeInfo().Assembly.Location,
                    typeof(Console).GetTypeInfo().Assembly.Location,
                    Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll")
                };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            //CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Debug);

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: options);

            using (var ms = new MemoryStream())
            {
                var emitOptions = new EmitOptions(debugInformationFormat: DebugInformationFormat.Embedded);
                EmitResult result = compilation.Emit(ms,
        options: emitOptions);
                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);

                    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                    var type = assembly.GetType("MyType");
                    var instance = assembly.CreateInstance("MyType");
                    var meth = type.GetMember("Print").First() as MethodInfo;
                    meth.Invoke(instance, new[] { "World" });
                }
            }
            #endregion

            #region Helloworld.cs 파일 생성 코드돔

            //            // 1. C# 소스 코드 생성
            //            string sourceCode = @"
            //using System;
            //using System.Diagnostics;

            //public class HelloWorld
            //{
            //    public static void Main()
            //    {
            //        Console.WriteLine(""Hello, World!"");
            //    }
            //}";

            //            // 2. C# 코드 파싱 및 구문 트리 생성
            //            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            //            // 3. 어셈블리 참조 설정
            //            string assemblyName = Path.GetRandomFileName();
            //            var coreDir = Path.GetDirectoryName(typeof(object).Assembly.Location);
            //            var references = new[]
            //            {
            //            MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Private.CoreLib.dll")),
            //            MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Runtime.dll")),
            //            MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Console.dll")),
            //            MetadataReference.CreateFromFile(Path.Combine(coreDir, "System.Diagnostics.Debug.dll"))
            //        };

            //            // 4. 컴파일러 설정 및 컴파일
            //            CSharpCompilation compilation = CSharpCompilation.Create(
            //                assemblyName,
            //                syntaxTrees: new[] { syntaxTree },
            //                references: references,
            //                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            //                    .WithOptimizationLevel(OptimizationLevel.Debug)
            //                    .WithPlatform(Platform.AnyCpu));

            //            // 5. PDB 파일 경로 설정
            //            string pdbPath = Path.Combine(Environment.CurrentDirectory, "HelloWorld.pdb");
            //            string exePath = Path.Combine(Environment.CurrentDirectory, "HelloWorld.exe");

            //            // 6. 컴파일 결과 어셈블리 생성
            //            EmitOptions emitOptions = new EmitOptions(debugInformationFormat: DebugInformationFormat.PortablePdb);
            //            EmitResult result;
            //            using (var exeStream = new FileStream(exePath, FileMode.Create))
            //            using (var pdbStream = new FileStream(pdbPath, FileMode.Create))
            //            {
            //                result = compilation.Emit(exeStream, pdbStream, options: emitOptions);
            //            }

            //            // 7. 컴파일 결과 출력
            //            if (result.Success)
            //            {
            //                Console.WriteLine("Compilation successful! Executable generated: " + exePath);

            //                // 8. 컴파일된 어셈블리 실행
            //                ProcessStartInfo processStartInfo = new ProcessStartInfo(exePath)
            //                {
            //                    UseShellExecute = false,
            //                    RedirectStandardOutput = true,
            //                    RedirectStandardError = true,
            //                    CreateNoWindow = true
            //                };

            //                using (Process process = Process.Start(processStartInfo))
            //                {
            //                    process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            //                    process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
            //                    process.BeginOutputReadLine();
            //                    process.BeginErrorReadLine();
            //                    process.WaitForExit();
            //                }
            //            }
            //            else
            //            {
            //                foreach (Diagnostic diagnostic in result.Diagnostics)
            //                {
            //                    Console.WriteLine(diagnostic.ToString());
            //                }
            //            }

            #endregion

            #region System.CodeDom 기능이 제한적이라 사용하지 않음 (System.PlatformNotSupportedException: 'Operation is not supported on this platform.')

            ////1. Create a new CodeCompileUnit to contain the program graph
            //CodeCompileUnit compileUnit = new CodeCompileUnit();

            ////2. Create a new CodeNamespace to contain the program namespace
            //CodeNamespace samples = new CodeNamespace("CodeDomTest");

            ////3. Import the System namespace
            //samples.Imports.Add(new CodeNamespaceImport("System"));
            //compileUnit.Namespaces.Add(samples);

            ////4. Create a new CodeTypeDeclaration to contain the class
            //CodeTypeDeclaration class1 = new CodeTypeDeclaration("HelloWorldClass");
            //samples.Types.Add(class1);

            ////5. Create a new CodeEntryPointMethod to contain the program method
            //CodeEntryPointMethod start = new CodeEntryPointMethod();
            //start.Statements.Add(new CodeMethodInvokeExpression(
            //    new CodeTypeReferenceExpression("Console"),
            //    "WriteLine", new CodePrimitiveExpression("Hello World!")));
            //class1.Members.Add(start);

            ////6. Generate C# code from the compile unit
            //CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            //using(StringWriter sourceWriter = new StringWriter())
            //{
            //    provider.GenerateCodeFromCompileUnit(compileUnit, sourceWriter, new CodeGeneratorOptions());
            //    string sourceCode = sourceWriter.ToString();
            //    Console.WriteLine("Generated C# Code: ");

            //    Console.WriteLine(sourceCode);

            //    File.WriteAllText("HelloWorldClass.cs", sourceCode);

            //}


            //Console.ReadKey();

            //// 8. 컴파일 설정
            //CompilerParameters parameters = new CompilerParameters
            //{
            //    IncludeDebugInformation = true,
            //    GenerateInMemory = true,
            //    TempFiles = new TempFileCollection(Environment.GetEnvironmentVariable("TEMP"), true)
            //};

            //// 9. 코드를 컴파일하여 어셈블리 생성
            //CompilerResults results = provider.CompileAssemblyFromDom(parameters, compileUnit);


            //// 10. 컴파일 결과 출력
            //if (results.Errors.Count > 0)
            //{
            //    foreach (CompilerError error in results.Errors)
            //    {
            //        Console.WriteLine($"Error ({error.ErrorNumber}): {error.ErrorText}");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Compilation successful! Executable generated: HelloWorld.exe");
            //}
            #endregion

        }
    }

    public struct SearchRequest
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string SearchText { get; set; }

        public int Language { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public SearchRequest(string startTime, string endTime, string searchText, int language, int pageNumber, int pageSize)
        {
            StartTime = startTime;
            EndTime = endTime;
            SearchText = searchText;
            Language = language;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
