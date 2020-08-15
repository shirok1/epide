namespace Epide.Utility
{
    public class InterpreterCustom : InterpreterBase
    {
        private string _interpreterPath = "python.exe";
        public override string InterpreterPath => _interpreterPath;

        public void SetInterpreterPath(string path)
        {
            _interpreterPath = path;
        }
    }
}