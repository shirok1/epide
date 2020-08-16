using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Epide.Utility;
using Microsoft.Win32;

namespace Epide
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly DataBox dataBox;

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataBox.ProfileAvailability)
            {
                dataBox.ReadProfile();
            }
            // dataBox.ReadProfile();
            CodeBox.FontFamily = new FontFamily(dataBox.EditorFont);
            CodeBox.FontSize = dataBox.FontSize;
            // TextRange textRange = new TextRange(CodeBox.Document.ContentStart, CodeBox.Document.ContentEnd);
            // using (MemoryStream ms = new MemoryStream())
            // {
            //     using (StreamWriter sw = new StreamWriter(ms))
            //     {
            //         sw.Write("Welcome.rtf");
            //         sw.Flush();
            //         ms.Seek(0, SeekOrigin.Begin);
            //         textRange.Load(ms, DataFormats.Rtf);
            //     }
            // }
        }

        public MainWindow()
        {
            InitializeComponent();
            dataBox = new DataBox(CodeBox);
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
            dataBox.ScriptPath = openFileDialog.FileName;
            Title = dataBox.ScriptPath.Split('\\').Last();
            var pyReader = new StreamReader(dataBox.ScriptPath, Encoding.UTF8);
            var code = pyReader.ReadToEnd();
            pyReader.Close();
            // Successfully selected
            RichTextBoxOperator.RTBoxSet(CodeBox, code);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save this Python script file to:",
                Filter = "Python script|*.py",
                FileName = dataBox.ScriptPath.Split('\\').Last(),
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = "py"
            };
            if (saveFileDialog.ShowDialog() == false) return;
            var pyWriter = new StreamWriter(saveFileDialog.FileName, false);
            pyWriter.Write(RichTextBoxOperator.RTBoxGet(CodeBox));
            pyWriter.Close();
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            new Settings(dataBox).Show();
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            Executer.Execute($"\"{dataBox.Interpreter.InterpreterPath}\"",
                $"\"{dataBox.ScriptPath}\"", "Running...");
        }

        /*private void CodeBox_OnKeyDown(object sender, KeyEventArgs key)
        {
            switch (key.Key)
            {
                case Key.Tab:
                    key.Handled = true;
                    // CodeBox.Selection.Start.
                    // CodeBox.Selection.Text = new string(' ', dataBox.TabWidth);
                    // CodeBox.CaretPosition = CodeBox.Selection.End;
                    var startPosition = CodeBox.Selection.Start.GetLineStartPosition(0);
                    var endPosition = CodeBox.Selection.End.GetLineStartPosition(0);
                    if (!key.KeyboardDevice.IsKeyDown(Key.LeftShift))
                    {
                        while (startPosition?.GetOffsetToPosition(endPosition) != 0)
                        {
                            endPosition?.InsertTextInRun(new string(' ', dataBox.TabWidth));
                            endPosition = endPosition?.GetPositionAtOffset(-(2 + dataBox.TabWidth))
                                ?.GetLineStartPosition(0);
                            // MessageBox.Show("wow");
                        }

                        endPosition?.InsertTextInRun(new string(' ', dataBox.TabWidth));
                    }
                    else
                    {
                        while (startPosition?.GetOffsetToPosition(endPosition) != 0)
                        {
                            // endPosition?.InsertTextInRun(new string(' ', dataBox.TabWidth));
                            if (new TextRange(endPosition, endPosition.GetPositionAtOffset(dataBox.TabWidth)).Text
                                .Trim().Length == 0)
                            {
                                endPosition.DeleteTextInRun(dataBox.TabWidth);
                            }

                            endPosition = endPosition?.GetPositionAtOffset(-(2 + dataBox.TabWidth))
                                ?.GetLineStartPosition(0);
                            // MessageBox.Show("wow");
                        }

                        if (new TextRange(endPosition, endPosition.GetPositionAtOffset(dataBox.TabWidth)).Text
                            .Trim().Length == 0)
                        {
                            endPosition.DeleteTextInRun(dataBox.TabWidth);
                        }
                    }

                    break;
            }
        }*/

        private void CodeBox_OnPreviewKeyDown(object sender, KeyEventArgs key)
        {
            // CodeBox_OnKeyDown(sender,key);
            switch (key.Key)
            {
                case Key.Tab:
                    key.Handled = true;
                    // CodeBox.Selection.Start.
                    // CodeBox.Selection.Text = new string(' ', dataBox.TabWidth);
                    // CodeBox.CaretPosition = CodeBox.Selection.End;
                    var startPosition = CodeBox.Selection.Start.GetLineStartPosition(0);
                    var endPosition = CodeBox.Selection.End.GetLineStartPosition(0);
                    if (!key.KeyboardDevice.IsKeyDown(Key.LeftShift))
                    {
                        while (startPosition?.GetOffsetToPosition(endPosition) != 0)
                        {
                            endPosition?.InsertTextInRun(new string(' ', dataBox.TabWidth));
                            endPosition = endPosition?.GetPositionAtOffset(-(2 + dataBox.TabWidth))
                                ?.GetLineStartPosition(0);
                            // MessageBox.Show("wow");
                        }

                        endPosition?.InsertTextInRun(new string(' ', dataBox.TabWidth));
                    }
                    else
                    {
                        while (startPosition?.GetOffsetToPosition(endPosition) != 0)
                        {
                            // endPosition?.InsertTextInRun(new string(' ', dataBox.TabWidth));
                            if (new TextRange(endPosition, endPosition.GetPositionAtOffset(dataBox.TabWidth)).Text
                                .Trim().Length == 0)
                            {
                                endPosition.DeleteTextInRun(dataBox.TabWidth);
                            }

                            endPosition = endPosition?.GetPositionAtOffset(-(2 + dataBox.TabWidth))
                                ?.GetLineStartPosition(0);
                            // MessageBox.Show("wow");
                        }

                        if (new TextRange(endPosition, endPosition.GetPositionAtOffset(dataBox.TabWidth)).Text
                            .Trim().Length == 0)
                        {
                            endPosition.DeleteTextInRun(dataBox.TabWidth);
                        }
                    }
                    break;
                case Key.Enter:
                    key.Handled = true;
                    var tmpText = new TextRange(CodeBox.Selection.Start.GetLineStartPosition(0),
                        CodeBox.Document.ContentEnd).Text;
                    if (tmpText.TrimStart().Split()[0].EndsWith(":"))
                    {
                        CodeBox.Selection.Text = "\n" + new string(' ',
                            tmpText.Length - tmpText.TrimStart(' ').Length + dataBox.TabWidth);
                    }
                    else
                    {
                        CodeBox.Selection.Text = "\n" + new string(' ', tmpText.Length - tmpText.TrimStart(' ').Length);
                    }

                    CodeBox.CaretPosition = CodeBox.Selection.End;
                    break;
            }
        }
    }
}