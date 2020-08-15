using System.IO;
using System.Text;
using System.Windows.Controls;
using HandyControl.Controls;

// using System.Xml.Serialization;

namespace Epide.Utility
{
    // [XmlType("EpideSettings")]
    public class DataBox
    {
        public DataBox(RichTextBox codeBox) : this()
        {
            CodeBox = codeBox;
        }
        
        public DataBox()
        {
            BundledInterpreter = new InterpreterBundled();
            SystemInterpreter = new InterpreterSystem();
            CustomInterpreter = new InterpreterCustom();
            if (BundledInterpreter.Availability)
                Interpreter = BundledInterpreter;
            else if (SystemInterpreter.Availability)
                Interpreter = SystemInterpreter;
            else
                Interpreter = CustomInterpreter;
        }
        
        [Newtonsoft.Json.JsonIgnore] public string ScriptPath { get; set; } = "untitled.py";
        
        [Newtonsoft.Json.JsonIgnore] public RichTextBox CodeBox { get; set; }

        public string EditorFont
        {
            get => _editorFont;
            set
            {
                _editorFont = value;
                CodeBox.FontFamily = new System.Windows.Media.FontFamily(_editorFont);
            }
        }
        private string _editorFont = "Consolas";
        public short TabWidth { get; set; } = 2;

        public short FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                CodeBox.FontSize = _fontSize;
            }
        }

        private short _fontSize = 16;

        [Newtonsoft.Json.JsonIgnore] public IPythonInterpreter Interpreter { get; set; }
        [Newtonsoft.Json.JsonIgnore] public InterpreterBundled BundledInterpreter { get; }
        [Newtonsoft.Json.JsonIgnore] public InterpreterSystem SystemInterpreter { get; }
        [Newtonsoft.Json.JsonIgnore] public InterpreterCustom CustomInterpreter { get; }
        public static void ReadProfile(DataBox targetDataBox, string path = @"settings.json")
        {
            var profileReader = new StreamReader(path, Encoding.UTF8);
            string json = profileReader.ReadToEnd();
            profileReader.Close();
            // return Newtonsoft.Json.JsonConvert.DeserializeObject<DataBox>(json);
            var format = new { EditorFont = "", FontSize = (short)0,TabWidth = (short)0 };
            var toReturn = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, format);
            targetDataBox.EditorFont = toReturn.EditorFont;
            targetDataBox.FontSize = toReturn.FontSize;
            targetDataBox.TabWidth = toReturn.TabWidth;
        }
        public void WriteProfile(string path = @"settings.json")
        {
            using (System.IO.StreamWriter file = System.IO.File.CreateText(path))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, this);
            }
            // MessageBox.Show(Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented));
        }
    }
}