using System;
using System.IO;
using HandyControl.Controls;

namespace Epide.Utility
{
    internal static class DetectBundledPython
    {
        public static string Detect()
        {
            var programPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dirInfo = new DirectoryInfo(programPath);
            var dirSub = dirInfo.GetDirectories();
            
            foreach (var dir in dirSub){
                // Console.WriteLine(dir.FullName);
                // MessageBox.Show(dir.FullName);
                if (File.Exists(dir.FullName + "\\python.exe"))
                    return dir.FullName + "\\python.exe";}
            return null;
        }
    }
}