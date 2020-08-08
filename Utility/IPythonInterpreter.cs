namespace Epide.Utility
{
    public interface IPythonInterpreter
    {
        string InterpreterPath { get; }
        bool Availability { get; }
        string Version { get; }
    }
}