using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Epide.Utility;
using Microsoft.Win32;

namespace Epide
{
    /// <summary>
    ///     Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings
    {
        private readonly DataBox _mainDataBox;

        public Settings(DataBox extData)
        {
            InitializeComponent();
            _mainDataBox = extData;
        }

        private void Settings_OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var font in Fonts.SystemFontFamilies) CBoxFontFamily.Items.Add(font.Source);
            if (_mainDataBox.BundledInterpreter.Availability)
            {
                RButtonInterpreterBundled.IsEnabled = true;
                LabelPythonVersionBundled.Content = _mainDataBox.BundledInterpreter.Version;
            }

            if (_mainDataBox.SystemInterpreter.Availability)
            {
                RButtonInterpreterSystem.IsEnabled = true;
                LabelPythonVersionSystem.Content = _mainDataBox.SystemInterpreter.Version;
            }
            
            switch (_mainDataBox.Interpreter)
            {
                case InterpreterBundled _:
                    RButtonInterpreterBundled.IsChecked = true;
                    break;
                case InterpreterSystem _:
                    RButtonInterpreterSystem.IsChecked = true;
                    break;
                default:
                    RButtonInterpreterCustom.IsChecked = true;
                    LabelPythonVersionCustom.Content = _mainDataBox.CustomInterpreter.Version;
                    break;
            }
            TBoxPathOfCustomPythonExe.Text = _mainDataBox.CustomInterpreter.InterpreterPath;
        }

        private void ButtonBrowsePythonExe_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select custom python.exe",
                Filter = "Python interpreter|python.exe",
                FileName = string.Empty,
                FilterIndex = 1,
                Multiselect = false,
                RestoreDirectory = true
                // DefaultExt = "py"
            };
            if (openFileDialog.ShowDialog() == false) return;

            TBoxPathOfCustomPythonExe.Text = openFileDialog.FileName;
            _mainDataBox.CustomInterpreter.SetInterpreterPath(openFileDialog.FileName); 
            LabelPythonVersionCustom.Content = DetectPythonVersion.Detect(openFileDialog.FileName);

            _mainDataBox.Interpreter = _mainDataBox.CustomInterpreter;
            RButtonInterpreterCustom.IsChecked = true;
        }

        private void RButtonInterpreterBundled_Click(object sender, RoutedEventArgs e)
        {
            _mainDataBox.Interpreter = _mainDataBox.BundledInterpreter;
        }

        private void RButtonInterpreterSystem_Click(object sender, RoutedEventArgs e)
        {
            _mainDataBox.Interpreter = _mainDataBox.SystemInterpreter;
        }

        private void RButtonInterpreterCustom_Click(object sender, RoutedEventArgs e)
        {
            _mainDataBox.Interpreter = _mainDataBox.CustomInterpreter;
        }

        private void TBoxPathOfCustomPythonExe_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _mainDataBox?.CustomInterpreter.SetInterpreterPath(TBoxPathOfCustomPythonExe.Text);
        }
    }
}