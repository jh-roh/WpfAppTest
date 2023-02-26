using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinformAppTest.Controller;
using WinformAppTest.Model;

namespace WinformAppTest
{
    public partial class MainForm : Form
    {
        private PersonController _personController;

        #region Field

        /// <summary>
        /// 배경 색상
        /// </summary>
        private const int BACKGROUND_COLOR = 0x2A211C;

        /// <summary>
        /// 전경 색상
        /// </summary>
        private const int FOREGROUND_COLOR = 0xB7B7B7;

        /// <summary>
        /// 코드 폴딩 원 여부
        /// </summary>
        private const bool CODEFOLDING_CIRCULAR = true;

        /// <summary>
        /// 키워드 배열 1
        /// </summary>
        private string[] keywordArray1 = new string[]
        {
            "abstract",
            "arguments",
            "as",
            "AS3",
            "author",
            "base",
            "bool",
            "break",
            "by",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "copy",
            "decimal",
            "default",
            "delegate",
            "delete",
            "deprecated",
            "descending",
            "do",
            "double",
            "dynamic",
            "each",
            "else",
            "enum",
            "event",
            "eventType",
            "example",
            "exampleText",
            "exception",
            "explicit",
            "extends",
            "extern",
            "false",
            "final",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "from",
            "function",
            "get",
            "goto",
            "group",
            "haxe",
            "if",
            "implements",
            "implicit",
            "import",
            "in",
            "include",
            "Infinity",
            "inheritDoc",
            "instanceof",
            "int",
            "interface",
            "internal",
            "into",
            "intrinsic",
            "is",
            "langversion",
            "link",
            "lock",
            "long",
            "mtasc",
            "mxmlc",
            "namespace",
            "NaN",
            "native",
            "new",
            "null",
            "object",
            "operator",
            "orderby",
            "out",
            "override",
            "package",
            "param",
            "params",
            "partial",
            "playerversion",
            "private",
            "productversion",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "see",
            "select",
            "serial",
            "serialData",
            "serialField",
            "set",
            "short",
            "since",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "super",
            "switch",
            "this",
            "throw",
            "throws",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "undefined",
            "unsafe",
            "usage",
            "use",
            "ushort",
            "using",
            "var",
            "version",
            "virtual",
            "void",
            "volatile",
            "where",
            "while",
            "with",
            "yield"
        };

        /// <summary>
        /// 키워드 배열 2
        /// </summary>
        private string[] keywordArray2 = new string[]
        {
            "ArgumentError",
            "Array",
            "Boolean",
            "Byte",
            "Char",
            "Class",
            "Date",
            "DateTime",
            "Decimal",
            "DefinitionError",
            "Double",
            "Error",
            "EvalError",
            "File",
            "Forms",
            "Function",
            "Int16",
            "Int32",
            "Int64",
            "IntPtr",
            "Math",
            "Namespace",
            "Null",
            "Number",
            "Object",
            "Path",
            "RangeError",
            "ReferenceError",
            "RegExp",
            "SByte",
            "ScintillaNET",
            "SecurityError",
            "Single",
            "String",
            "SyntaxError",
            "System",
            "TypeError",
            "UInt16",
            "UInt32",
            "UInt64",
            "UIntPtr",
            "Void",
            "Windows",
            "XML",
            "XMLList",
            "arguments",
            "int",
            "uint",
            "void"
        };

        #endregion


        public MainForm()
        {
            InitializeComponent();

            


            _personController = new PersonController(this);



            this.scintilla.Dock = DockStyle.Fill;
            this.scintilla.WrapMode = WrapMode.None;
            this.scintilla.IndentationGuides = IndentView.LookBoth;

            this.scintilla.SetSelectionBackColor(true, GetColor(0x114D9C));

            this.scintilla.StyleResetDefault();

            this.scintilla.Styles[Style.Default].BackColor = GetColor(0x212121);
            this.scintilla.Styles[Style.Default].ForeColor = GetColor(0xFFFFFF);
            this.scintilla.Styles[Style.Default].Font = "나눔고딕코딩";
            this.scintilla.Styles[Style.Default].Size = 12;

            this.scintilla.StyleClearAll();

            this.scintilla.Styles[Style.Cpp.Identifier].ForeColor = GetColor(0xD0DAE2);
            this.scintilla.Styles[Style.Cpp.Comment].ForeColor = GetColor(0xBD758B);
            this.scintilla.Styles[Style.Cpp.CommentLine].ForeColor = GetColor(0x40BF57);
            this.scintilla.Styles[Style.Cpp.CommentDoc].ForeColor = GetColor(0x2FAE35);
            this.scintilla.Styles[Style.Cpp.Number].ForeColor = GetColor(0xFFFF00);
            this.scintilla.Styles[Style.Cpp.String].ForeColor = GetColor(0xFFFF00);
            this.scintilla.Styles[Style.Cpp.Character].ForeColor = GetColor(0xE95454);
            this.scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = GetColor(0x8AAFEE);
            this.scintilla.Styles[Style.Cpp.Operator].ForeColor = GetColor(0xE0E0E0);
            this.scintilla.Styles[Style.Cpp.Regex].ForeColor = GetColor(0xff00ff);
            this.scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = GetColor(0x77A7DB);
            this.scintilla.Styles[Style.Cpp.Word].ForeColor = GetColor(0x48A8EE);
            this.scintilla.Styles[Style.Cpp.Word2].ForeColor = GetColor(0xF98906);
            this.scintilla.Styles[Style.Cpp.CommentDocKeyword].ForeColor = GetColor(0xB3D991);
            this.scintilla.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = GetColor(0xFF0000);
            this.scintilla.Styles[Style.Cpp.GlobalClass].ForeColor = GetColor(0x48A8EE);

            this.scintilla.Lexer = Lexer.Cpp;

            this.scintilla.SetKeywords(0, ConnectString(this.keywordArray1, " "));
            this.scintilla.SetKeywords(1, ConnectString(this.keywordArray2, " "));

            this.scintilla.Styles[Style.LineNumber].BackColor = GetColor(BACKGROUND_COLOR);
            this.scintilla.Styles[Style.LineNumber].ForeColor = GetColor(FOREGROUND_COLOR);
            this.scintilla.Styles[Style.IndentGuide].ForeColor = GetColor(FOREGROUND_COLOR);
            this.scintilla.Styles[Style.IndentGuide].BackColor = GetColor(BACKGROUND_COLOR);

            this.scintilla.Margins[1].Width = 30;
            this.scintilla.Margins[1].Type = MarginType.Number;
            this.scintilla.Margins[1].Mask = 0;
            this.scintilla.Margins[1].Sensitive = true;

            this.scintilla.Margins[2].Width = 20;
            this.scintilla.Margins[2].Type = MarginType.Symbol;
            this.scintilla.Markers[2].Symbol = MarkerSymbol.Circle;
            this.scintilla.Margins[2].Mask = (1 << 2);
            this.scintilla.Margins[2].Sensitive = true;

            this.scintilla.Markers[2].SetBackColor(GetColor(0xFF003B));
            this.scintilla.Markers[2].SetForeColor(GetColor(0x000000));
            this.scintilla.Markers[2].SetAlpha(100);

            this.scintilla.SetFoldMarginColor(true, GetColor(BACKGROUND_COLOR));
            this.scintilla.SetFoldMarginHighlightColor(true, GetColor(BACKGROUND_COLOR));

            this.scintilla.SetProperty("fold", "1");
            this.scintilla.SetProperty("fold.compact", "1");

            this.scintilla.Margins[3].Width = 20;
            this.scintilla.Margins[3].Type = MarginType.Symbol;
            this.scintilla.Margins[3].Mask = Marker.MaskFolders;
            this.scintilla.Margins[3].Sensitive = true;

            for (int i = 25; i <= 31; i++)
            {
                this.scintilla.Markers[i].SetForeColor(GetColor(BACKGROUND_COLOR));
                this.scintilla.Markers[i].SetBackColor(GetColor(FOREGROUND_COLOR));
            }

            this.scintilla.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            this.scintilla.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            this.scintilla.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            this.scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            this.scintilla.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            this.scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            this.scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            this.scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
            this.scintilla.ViewWhitespace = WhitespaceMode.VisibleAlways;
            LoadFile("../../Form1.cs");

            scintilla.ReadOnly = true;
        }
        /// <summary>
        /// 파일 로드하기
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        private void LoadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                this.scintilla.Text = File.ReadAllText(filePath);
            }
        }
        /// <summary>
        /// 색상 구하기
        /// </summary>
        /// <param name="rgb">RGB 값</param>
        /// <returns>색상</returns>
        private Color GetColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }
        /// <summary>
        /// 문자열 연결하기
        /// </summary>
        /// <param name="sourceArray">소스 문자열 배열</param>
        /// <param name="link">연결 문자열</param>
        /// <returns>연결 문자열</returns>
        private string ConnectString(string[] sourceArray, string link)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < sourceArray.Length; i++)
            {
                if (sourceArray[i].Length > 0)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(link);
                    }

                    stringBuilder.Append(sourceArray[i]);
                }
            }

            return stringBuilder.ToString();
        }

        public void SetGridDataSource(List<Person> persons)
        {
            gridControl1.DataSource = persons;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _personController.LoadPersons();
        }
    }
}
