﻿using System;
using System.IO;
using System.Linq;
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

        private DataBox()
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

        [Newtonsoft.Json.JsonIgnore] public string FilePath { get; set; } = "untitled.py";
        [Newtonsoft.Json.JsonIgnore] public string FileName => FilePath.Split('\\').Last();

        [Newtonsoft.Json.JsonIgnore] public RichTextBox CodeBox { get; set; }

        public string EditorFont
        {
            get => _editorFont;
            set
            {
                _editorFont = value;
                CodeBox.FontFamily = new System.Windows.Media.FontFamily(_editorFont);
                // MessageBox.Show(value);
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

        private const string ProfilePath = "Settings.json";

        public static bool ProfileAvailability
        {
            get
            {
                if (!File.Exists(ProfilePath))
                {
                    // MessageBox.Show("Profile NOT exist!");
                    return false;
                }

                try
                {
                    var profileReader = new StreamReader(ProfilePath, Encoding.UTF8);
                    string json = profileReader.ReadToEnd();
                    profileReader.Close();
                    // return Newtonsoft.Json.JsonConvert.DeserializeObject<DataBox>(json);
                    var format = new { EditorFont = "", FontSize = (short)0, TabWidth = (short)0 };
                    var toReturn = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, format);
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    MessageBox.Show("Profile failed. Using default settings instead.","Profile Error");
                    return false;
                }
                return true;
            }
        }

        public void ReadProfile(string path = ProfilePath)
        {
            var profileReader = new StreamReader(path, Encoding.UTF8);
            string json = profileReader.ReadToEnd();
            profileReader.Close();
            // return Newtonsoft.Json.JsonConvert.DeserializeObject<DataBox>(json);
            var format = new { EditorFont = "", FontSize = (short)0, TabWidth = (short)0 };
            var toReturn = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, format);
            EditorFont = toReturn.EditorFont;
            FontSize = toReturn.FontSize;
            TabWidth = toReturn.TabWidth;
        }

        public void WriteProfile(string path = ProfilePath)
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