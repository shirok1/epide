using System.ComponentModel;

namespace Epide.Utility
{
    public class DataBox : INotifyPropertyChanged
    {
        public DataBox()
        {
            BundledInterpreter = new InterpreterBundled();
            SystemInterpreter = new InterpreterSystem();
            CustomInterpreter = new InterpreterCustom();
            if (BundledInterpreter.Availability)
            {
                Interpreter = BundledInterpreter;
            }
            else if (SystemInterpreter.Availability)
            {
                Interpreter = SystemInterpreter;
            }
            else
            {
                Interpreter = CustomInterpreter;
            }
        }

        private string _code;

        public string Code
        {
            get => _code;
            set
            {
                _code = value; //触发事件
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Code"));
            }
        }

        public string ScriptPath { get; set; } = "untitled.py";

        // public string InterpreterPath { get; set; } = "python.exe";
        public IPythonInterpreter Interpreter { get; set; }
        public InterpreterBundled BundledInterpreter { get; }
        public InterpreterSystem SystemInterpreter { get; }
        public InterpreterCustom CustomInterpreter { get; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}