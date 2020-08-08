namespace Epide.Utility
{
    public class InterpreterCustom : InterpreterBase
    {
        public override string InterpreterPath => _interpreterPath;
        private string _interpreterPath = "python.exe";
        public void SetInterpreterPath(string path){_interpreterPath = path;}
    }
}