using System;
using System.Diagnostics;
using Epide.Properties;
using HandyControl.Controls;

namespace Epide.Utility
{
    internal class Executer
    {
        internal static void Execute(string command, string arg = "", string title = null, bool pause = true)
        {
            if (command is null)
            {
                /*MessageBox.Show(Resources.CannotExecuteNull,
                    Resources.CannotExecuteNull_Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                MessageBox.Show(Resources.CannotExecuteNull, Resources.CannotExecuteNull_Caption);
                return;
            }

            if (title is null) title = Resources.RunCmd_Title;

            //以下代码来自互联网
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C (title " + title + ")&(" + command + " " + arg + (pause ? ")&pause" : ")")
            };
            process.StartInfo = startInfo;
            try
            {
                if (process.Start()) //开始进程 
                    process.WaitForExit(); //这里无限等待进程结束 
            }
            catch
            {
                throw new ExecuteExcption();
            }
            finally
            {
                process.Close();
            }
        }

        internal static string ExecuteWithOutput(string command, string arg = "", string title = null,
            bool pause = true)
        {
            if (command is null)
            {
                /*MessageBox.Show(Resources.CannotExecuteNull,
                    Resources.CannotExecuteNull_Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                MessageBox.Show(Resources.CannotExecuteNull, Resources.CannotExecuteNull_Caption);
                return null;
            }

            if (title is null) title = Resources.RunCmd_Title;

            string output = null;
            //以下代码来自互联网
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                // FileName = "cmd.exe",
                FileName = command,
                // Arguments = "/C (title " + title + ")&(" + command + " " + arg + (pause ? ")&pause" : ")"),
                Arguments = arg,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            process.StartInfo = startInfo;
            try
            {
                if (process.Start()) //开始进程 
                {
                    process.WaitForExit(); //这里无限等待进程结束 
                    output = process.StandardOutput.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(output)) output = process.StandardError.ReadToEnd();
                }
            }
            // catch
            // {
            // }
            finally
            {
                process.Close();
            }

            return output;
        }

        public class ExecuteExcption : Exception
        {
            public ExecuteExcption() : base("Exception in executing external program.")
            {
            }
        }
    }
}