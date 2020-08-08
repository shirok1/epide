namespace Epide.Utility
{
    public abstract class InterpreterBase : IPythonInterpreter
    {
        public abstract string InterpreterPath { get; }
        public virtual bool Availability => Version != "N/A";
        public string Version => DetectPythonVersion.Detect(InterpreterPath,false);
    }
}