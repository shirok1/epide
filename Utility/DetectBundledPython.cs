using System;
using System.IO;

namespace Epide.Utility
{
    internal static class DetectBundledPython
    {
        public static string Detect()
        {
            var programPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            DirectoryInfo dirInfo = new DirectoryInfo(programPath);
            var dirSub = dirInfo.GetDirectories();
            foreach (var dir in dirSub)
            {
                if (File.Exists(dir.FullName + "\\python.exe"))
                {
                    return dir.FullName + "\\python.exe";
                }
            }
            return null;
        }
    }
}