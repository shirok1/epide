namespace Epide.Utility
{
    public class InterpreterBundled : InterpreterBase
    {
        public override string InterpreterPath
        {
            get
            {
                if (_interpreterPath is null)
                {
                    _interpreterPath = DetectBundledPython.Detect();
                }

                return _interpreterPath;
            }
        }

        private string _interpreterPath = null;
        public override bool Availability => DetectBundledPython.Detect() != null && base.Availability;
    }
}