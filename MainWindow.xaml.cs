using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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

        public bool Unsaved
        {
            get => _unsaved;
            set
            {
                if (value)
                {
                    Title = dataBox?.FileName + " - UNSAVED";
                }
                else
                {
                    Title = dataBox?.FileName;
                }

                _unsaved = value;
            }
        }

        private bool _unsaved;

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataBox.ProfileAvailability)
            {
                dataBox.ReadProfile();
            }

            // dataBox.ReadProfile();
            CodeBox.FontFamily = new FontFamily(dataBox.EditorFont);
            CodeBox.FontSize = dataBox.FontSize;
            Title = "Epide";
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

        public bool SaveFile()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save this Python script file to:",
                Filter = "Python script|*.py",
                FileName = dataBox.FileName,
                FilterIndex = 1,
                RestoreDirectory = true,
                DefaultExt = "py"
            };
            if (saveFileDialog.ShowDialog() == false) return false;
            var pyWriter = new StreamWriter(saveFileDialog.FileName, false);
            pyWriter.Write(RichTextBoxOperator.RTBoxGet(CodeBox));
            pyWriter.Close();
            // Title = dataBox?.FileName;
            Unsaved = false;
            dataBox.FilePath = saveFileDialog.FileName;
            return true;
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
            dataBox.FilePath = openFileDialog.FileName;
            var pyReader = new StreamReader(dataBox.FilePath, Encoding.UTF8);
            var code = pyReader.ReadToEnd();
            pyReader.Close();

            RichTextBoxOperator.RTBoxSet(CodeBox, code);
            Unsaved = false;
            // Title = dataBox.FileName;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            new Settings(dataBox).Show();
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            if (Unsaved)
            {
                switch (HandyControl.Controls.MessageBox.Show("Please save the script first.","SCRIPT UNSAVED",MessageBoxButton.OKCancel))
                {
                    case MessageBoxResult.OK:
                        if (!SaveFile()) return;
                        break;
                    /*case MessageBoxResult.None:
                        return;
                    case MessageBoxResult.Cancel:
                        return;*/
                    default:
                        return;
                }
            }

            Executer.Execute($"\"{dataBox.Interpreter.InterpreterPath}\"",
                $"\"{dataBox.FilePath}\"", "Running...");
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
                    var startPosition = CodeBox.Selection.Start;
                    var endPosition = CodeBox.Selection.End;
                    if (startPosition.GetOffsetToPosition(endPosition) >= 1)
                    {
                        startPosition = startPosition.GetPositionAtOffset(1); // Shrink 1 char to fix #2
                    }

                    if (!startPosition.IsAtLineStartPosition)
                    {
                        // MessageBox.Show("Not at start");
                        // MessageBox.Show(new TextRange(startPosition,startPosition.GetLineStartPosition(0)).Text);
                        startPosition = startPosition.GetLineStartPosition(0);
                    }

                    endPosition = endPosition.GetLineStartPosition(0);
                    if (!(key.KeyboardDevice.IsKeyDown(Key.LeftShift) && key.KeyboardDevice.IsKeyDown(Key.RightShift)))
                    {
                        // Shift is not pressed
                        while (startPosition?.GetOffsetToPosition(endPosition) >= 0)
                        {
                            // MessageBox.Show(startPosition?.GetOffsetToPosition(endPosition).ToString());
                            endPosition?.InsertTextInRun(new string(' ', dataBox.TabWidth));
                            endPosition = endPosition?.GetLineStartPosition(-1);
                        }
                    }
                    else
                    {
                        // Shift is pressed
                        while (startPosition?.GetOffsetToPosition(endPosition) != 0)
                        {
                            if (new TextRange(endPosition, endPosition.GetPositionAtOffset(dataBox.TabWidth)).Text
                                .Trim().Length == 0)
                            {
                                endPosition.DeleteTextInRun(dataBox.TabWidth);
                            }

                            endPosition = endPosition?.GetLineStartPosition(-1);
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

        private void CodeBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Unsaved = true;
        }
    }
}