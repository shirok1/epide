using System.Text;
using System.Windows.Controls;

namespace Epide.Utility
{
    public class WordStatistic
    {
        private readonly StringBuilder _statLabelBuilder = new StringBuilder();

        public void update(RichTextBox box)
        {
            // int index = box.Document.Blocks.GetFirstCharIndexOfCurrentLine();
            // int line = box.Document.Blocks.GetLineFromCharIndex(index) + 1;
            // int column = box.Document.Blocks.SelectionStart - index + 1;
            // int totalLine = box.Document.Blocks.GetLineFromCharIndex(box.Document.Blocks.TextLength) + 1;
        }
    }
}