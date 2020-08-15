namespace Epide.Utility
{
    public class InterpreterBundled : InterpreterBase
    {
        private string _interpreterPath;

        public override string InterpreterPath
        {
            get
            {
                if (_interpreterPath is null) _interpreterPath = DetectBundledPython.Detect();

                return _interpreterPath;
            }
        }

        public override bool Availability => DetectBundledPython.Detect() != null && base.Availability;
    }
}