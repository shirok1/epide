using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Epide.Utility;
using Microsoft.Win32;

namespace Epide
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly DataBox _data;

        public MainWindow()
        {
            InitializeComponent();
            _data = new DataBox();
            CodeBox.SetBinding(TextBox.TextProperty, new Binding("Code") {Source = _data});
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select a Python script file:",
                Filter = "Python script|*.py",
                FileName = string.Empty,
                FilterIndex = 1,
                Multiselect = false,
                RestoreDirectory = true,
                DefaultExt = "py"
            };
            if (openFileDialog.ShowDialog() == false) return;
            _data.ScriptPath = openFileDialog.FileName;
            Title = _data.ScriptPath.Split('\\').Last();
            var pyReader = new StreamReader(_data.ScriptPath, Encoding.UTF8);
            _data.Code = pyReader.ReadToEnd();
            pyReader.Close();
            // Successfully selected
            RichTextBoxOperator.RTBoxSet(CodeBox, _data.Code);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save this Python script file to:",
                Filter = "Python script|*.py",
                FileName = _data.ScriptPath.Split('\\').Last(),
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = "py"
            };
            if (saveFileDialog.ShowDialog() == false) return;
            var pyWriter = new StreamWriter(saveFileDialog.FileName, false);
            pyWriter.Write(Utility.RichTextBoxOperator.RTBoxGet(CodeBox));
            pyWriter.Close();

        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            new Settings(_data).Show();
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            Executer.Execute($"\"{_data.Interpreter.InterpreterPath}\"",
                $"\"{_data.ScriptPath}\"", "Running...");
        }
    }
}