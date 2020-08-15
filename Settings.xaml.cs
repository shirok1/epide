using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Epide.Utility;
using HandyControl.Data;
using Microsoft.Win32;

namespace Epide
{
    /// <summary>
    ///     Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings
    {
        public readonly DataBox MainDataBox;

        public Settings(DataBox extData)
        {
            InitializeComponent();
            MainDataBox = extData;
        }
        
        public void ReloadValue()
        {
            // CBoxFontFamily.Items.
            if (MainDataBox.BundledInterpreter.Availability)
            {
                RButtonInterpreterBundled.IsEnabled = true;
                LabelPythonVersionBundled.Content = MainDataBox.BundledInterpreter.Version;
            }

            if (MainDataBox.SystemInterpreter.Availability)
            {
                RButtonInterpreterSystem.IsEnabled = true;
                LabelPythonVersionSystem.Content = MainDataBox.SystemInterpreter.Version;
            }

            switch (MainDataBox.Interpreter)
            {
                case InterpreterBundled _:
                    RButtonInterpreterBundled.IsChecked = true;
                    break;
                case InterpreterSystem _:
                    RButtonInterpreterSystem.IsChecked = true;
                    break;
                default:
                    RButtonInterpreterCustom.IsChecked = true;
                    LabelPythonVersionCustom.Content = MainDataBox.CustomInterpreter.Version;
                    break;
            }

            TBoxPathOfCustomPythonExe.Text = MainDataBox.CustomInterpreter.InterpreterPath;
            NudTabWidth.Value = MainDataBox.TabWidth;
            NudFontSize.Value = MainDataBox.FontSize;
        }
        
        private void LoadCBoxFontFamily(string fontName = "")
        {
            if (string.IsNullOrEmpty(fontName))
            {
                fontName = MainDataBox.EditorFont;
            }
            CBoxFontFamily.Items.Clear();
            CBoxFontFamily.Items.Add(fontName);
            CBoxFontFamily.SelectedIndex = 0;
            foreach (var font in Fonts.SystemFontFamilies) CBoxFontFamily.Items.Add(font.Source);
        }

        private void Settings_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadCBoxFontFamily();
            ReloadValue();
            // CBoxFontFamily.SelectionBoxItem
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
                RestoreDirectory = true,
                DefaultExt = "py"
            };
            if (openFileDialog.ShowDialog() == false) return;
            TBoxPathOfCustomPythonExe.Text = openFileDialog.FileName;
            MainDataBox.CustomInterpreter.SetInterpreterPath(openFileDialog.FileName);
            LabelPythonVersionCustom.Content = DetectPythonVersion.Detect(openFileDialog.FileName);

            MainDataBox.Interpreter = MainDataBox.CustomInterpreter;
            // MainDataBox.CustomInterpreter;
            RButtonInterpreterCustom.IsChecked = true;
        }

        private void RButtonInterpreterBundled_Click(object sender, RoutedEventArgs e)
        {
            MainDataBox.Interpreter = MainDataBox.BundledInterpreter;
        }

        private void RButtonInterpreterSystem_Click(object sender, RoutedEventArgs e)
        {
            MainDataBox.Interpreter = MainDataBox.SystemInterpreter;
        }

        private void RButtonInterpreterCustom_Click(object sender, RoutedEventArgs e)
        {
            MainDataBox.Interpreter = MainDataBox.CustomInterpreter;
        }

        private void TBoxPathOfCustomPythonExe_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            MainDataBox?.CustomInterpreter.SetInterpreterPath(TBoxPathOfCustomPythonExe.Text);
        }

        private void CBoxFontFamily_OnDropDownClosed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CBoxFontFamily.Text))
            {
                return;
            }
            MainDataBox.EditorFont = CBoxFontFamily.Text;
            // MainDataBox.CodeBox.FontFamily = new FontFamily(CBoxFontFamily.Text);
        }

        private void NudTabWidth_OnValueChanged(object sender, FunctionEventArgs<double> e)
        {
            MainDataBox.TabWidth = (short) NudTabWidth.Value;
        }

        private void NudFontSize_OnValueChanged(object sender, FunctionEventArgs<double> e)
        {
            MainDataBox.FontSize = (short) NudFontSize.Value;
            // MainDataBox.CodeBox.FontSize = MainDataBox.FontSize;
        }

        private void ButtonSaveProfile(object sender, RoutedEventArgs e)
        {
            MainDataBox.WriteProfile();
        }
        private void ButtonLoadProfile(object sender, RoutedEventArgs e)
        {
            /*DataBox.ReadProfile(out string editorFont,out short fontSize,out short tabWidth);
            MainDataBox.EditorFont = editorFont;
            MainDataBox.FontSize = fontSize;
            MainDataBox.TabWidth = tabWidth;*/
            MainDataBox.ReadProfile();
            ReloadValue();
            LoadCBoxFontFamily();
        }
    }
}