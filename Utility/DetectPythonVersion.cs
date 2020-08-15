using System.Text.RegularExpressions;
using HandyControl.Controls;

namespace Epide.Utility
{
    internal static class DetectPythonVersion
    {
        private const string VersionInfoPattern = @"Python (\d\.(\d)+\.(\d)+)";

        internal static string Detect(string path, bool messageBox = true)
        {
            if (path is null) return "N/A";

            var rawOutput = "";
            try
            {
                rawOutput = Executer.ExecuteWithOutput(path, "-V");
            }
            catch (Executer.ExecuteExcption)
            {
                if (messageBox)
                    MessageBox.Show($"Error occur when trying executing {path}.", "Version detection error");

                return "N/A";
            }

            if (Regex.IsMatch(rawOutput, VersionInfoPattern))
                return Regex.Match(rawOutput, VersionInfoPattern).Groups[1].Value;
            if (messageBox)
                MessageBox.Show($"{path} might not be a proper Python interpreter.", "Version detection error");

            return "N/A";
        }
    }
}